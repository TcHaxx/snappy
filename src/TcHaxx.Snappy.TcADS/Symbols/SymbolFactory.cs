using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;
using TcHaxx.Snappy.Common.Verify;
using TwinCAT.Ads;
using TwinCAT.Ads.Server.TypeSystem;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols;

internal class SymbolFactory : ISymbolFactory
{
    private readonly IRpcMethodDescriptor _rpcMethodDescriptor;
    private readonly ILogger? _logger;

    private readonly Dictionary<IDataType, IVerifyMethod> _mappedStructTypeToRpcMethod = [];

    internal SymbolFactory(IRpcMethodDescriptor rpcMethodDescriptor, ILogger? logger)
    {
        _rpcMethodDescriptor = rpcMethodDescriptor;
        _logger = logger;
    }

    public void AddSymbols(ServerSymbolFactory? serverSymbolFactory)
    {
        if (serverSymbolFactory is null)
        {
            return;
        }

        uint idxGrp = 0x80000001;
        uint idxOffset = 0x10000000;
        foreach (var rpcMethodDescription in _rpcMethodDescriptor.GetRpcMethodDescription())
        {
            var paramsKvp = GetMethodParameter(rpcMethodDescription.Parameters);
            AddToServerSymbolFactory(serverSymbolFactory, paramsKvp);

            var retValKvp = GetMethodReturnValue(rpcMethodDescription.ReturnValue);
            AddToServerSymbolFactory(serverSymbolFactory, [retValKvp]);

            var fullName = rpcMethodDescription.Method.ReflectedType?.FullName ?? rpcMethodDescription.Method.Name;
            DataArea dataArea = new DataArea($"DATA::{fullName}", idxGrp, idxOffset++, 0x10000);

            serverSymbolFactory.AddDataArea(dataArea);

            var rpc = BuildRpcMethod(rpcMethodDescription.Method, paramsKvp, retValKvp);

            StructType dtStructRpc = new StructType($"STRUCT::{fullName}");
            dtStructRpc.AddMethod(rpc);
            serverSymbolFactory.AddType(dtStructRpc);
            serverSymbolFactory.AddSymbol(fullName, dtStructRpc, dataArea);

            _mappedStructTypeToRpcMethod.Add(dtStructRpc, rpcMethodDescription.RpcInvocableMethod);
        }
    }

    public AdsErrorCode InvokeRpcMethod(IDataType mappedType, object[] values, out object? returnValue)
    {
        returnValue = null;
        if (!_mappedStructTypeToRpcMethod.TryGetValue(mappedType, out var value))
        {
            return AdsErrorCode.DeviceServiceNotSupported;
        }

        var rpcMethodToInvoke = value;

        if (values.Length != rpcMethodToInvoke.GetType().GetMethod(nameof(IVerifyMethod.Verify))!.GetParameters().Length)
        {
            return AdsErrorCode.DeviceInvalidParam;
        }

        // FIXME: This is ugly...
        //        Find a better solution, e.g. reflection.
        returnValue = rpcMethodToInvoke.Verify((string)values[0], (string)values[1], (string)values[2]);

        return AdsErrorCode.NoError;
    }

    private static RpcMethod BuildRpcMethod(MethodInfo methodInfo, IEnumerable<KeyValuePair<ParameterInfo, IDataType>> paramsKvp, KeyValuePair<ParameterInfo, IDataType> retValKvp)
    {
        var nameOrAlias = methodInfo.GetCustomAttribute<AliasAttribute>()?.AliasName ?? methodInfo.Name;
        var rpc = new RpcMethod(nameOrAlias);
        foreach (var (k, v) in paramsKvp)
        {
            rpc.AddParameter(k.Name ?? "unknown", v, k.IsOut ? MethodParamFlags.Out : MethodParamFlags.In);
        }

        rpc.SetReturnType(retValKvp.Value);
        return rpc;
    }

    private static void AddToServerSymbolFactory(ServerSymbolFactory serverSymbolFactory, IEnumerable<KeyValuePair<ParameterInfo, IDataType>> paramsKvp)
    {
        _ = paramsKvp.Select(x => x.Value).Select(v => serverSymbolFactory.AddType((DataType)v)).ToArray();
    }

    private KeyValuePair<ParameterInfo, IDataType> GetMethodReturnValue(ParameterInfo returnValue)
    {
        if (!returnValue.ParameterType.IsValueType)
        {
            return ConvertToDataType(returnValue);
        }

        var structType = new StructType(returnValue.ParameterType.Name);

        foreach (var field in returnValue.ParameterType.GetFields())
        {
            var stringAttribute = field.GetCustomAttribute<MarshalAsAttribute>();
            if (stringAttribute is not null && field.FieldType == typeof(string))
            {
                var stringType = ToStringType(field);
                structType.AddAligned(new Member(field.Name ?? "unknown", stringType));
            }
            else
            {
                var dt = new PrimitiveType(field.Name ?? "unknown", field.FieldType);
                structType.AddAligned(new Member(field.Name ?? "unknown", dt));
            }
        }
        return new KeyValuePair<ParameterInfo, IDataType>(returnValue, structType);
    }

    private IEnumerable<KeyValuePair<ParameterInfo, IDataType>> GetMethodParameter(IEnumerable<ParameterInfo> parameters)
    {
        return parameters.Select(ConvertToDataType);
    }

    private KeyValuePair<ParameterInfo, IDataType> ConvertToDataType(ParameterInfo paramInfo)
    {
        if (paramInfo.ParameterType == typeof(string))
        {
            var stringType = ToStringType(paramInfo);
            return new KeyValuePair<ParameterInfo, IDataType>(paramInfo, stringType);
        }

        bool isPrimitive = IsPrimitiveType(paramInfo.ParameterType);
        if (!isPrimitive)
        {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
#pragma warning disable S3928 // Parameter names used into ArgumentException constructors should match an existing one 
            throw new ArgumentOutOfRangeException(nameof(paramInfo.ParameterType), "Expected only primitive types");
#pragma warning restore S3928 // Parameter names used into ArgumentException constructors should match an existing one 
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
        }

        var dt = new PrimitiveType(paramInfo.Name ?? "unknown", paramInfo.ParameterType);
        return new KeyValuePair<ParameterInfo, IDataType>(paramInfo, dt);
    }

    private static bool IsPrimitiveType(Type type)
    {
        return type.IsPrimitive;
    }

    private StringType ToStringType(ParameterInfo stringParameter)
    {
        var stringAttribute = stringParameter.GetCustomAttribute<StringAttribute>();
        if (stringAttribute is null)
        {
            _logger?.LogWarning("No attribute for parameter {StringParameter} specified! Using default length {DefaultStringLength} and encoding {Encoding}",
                stringParameter.Name, Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH, Common.Constants.DEFAULT_STRING_ENCODING);
            return new StringType(Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH, Common.Constants.DEFAULT_STRING_ENCODING);
        }

        var stringType = new StringType((int)stringAttribute.Length, stringAttribute.GetEncoding());
        return stringType;
    }
    private StringType ToStringType(FieldInfo stringField)
    {
        var stringAttribute = stringField.GetCustomAttribute<MarshalAsAttribute>();
        if (stringAttribute is null)
        {
            _logger?.LogWarning("No attribute for parameter {StringParameter} specified! Using default length {DefaultStringLength} and encoding {Encoding}",
                stringField.Name, Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH, Common.Constants.DEFAULT_STRING_ENCODING);
            return new StringType(Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH, Common.Constants.DEFAULT_STRING_ENCODING);
        }

        var stringType = new StringType(stringAttribute.SizeConst, Encoding.UTF8);
        return stringType;
    }
}
