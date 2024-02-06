using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Logging;
using TcHaxx.Snappy.Common;
using TcHaxx.Snappy.Common.RPC;
using TcHaxx.Snappy.Common.RPC.Attributes;
using TwinCAT.Ads.Server.TypeSystem;
using TwinCAT.Ads.TypeSystem;
using TwinCAT.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols;

internal class SymbolFactory : ISymbolFactory
{
    private readonly IRpcMethodDescriptor _RpcMethodDescriptor;
    private readonly ILogger? _Logger;

    private Dictionary<ParameterInfo, IDataType> _MappedDataTypes = new Dictionary<ParameterInfo, IDataType>();

    internal SymbolFactory(IRpcMethodDescriptor rpcMethodDescriptor, ILogger? logger)
    {
        _RpcMethodDescriptor = rpcMethodDescriptor;
        _Logger = logger;
    }

    public void AddSymbols(ServerSymbolFactory? serverSymbolFactory)
    {
        if (serverSymbolFactory is null)
        {
            return;
        }

        var rpcMethodDescription = _RpcMethodDescriptor.GetRpcMethodDescription();
        if (rpcMethodDescription is null)
        {
            throw new ArgumentNullException(nameof(rpcMethodDescription));
        }


        var paramsKvp = GetMethodParameter(rpcMethodDescription.Parameters);
        AddToServerSymbolFactory(serverSymbolFactory, paramsKvp);

        var retValKvp = GetMethodReturnValue(rpcMethodDescription.ReturnValue);
        AddToServerSymbolFactory(serverSymbolFactory, new[] { retValKvp });


        DataArea general = new DataArea("General", 0x01, 0x1000, 0x1000);

        serverSymbolFactory.AddDataArea(general);

        var rpc = BuildRpcMethod(rpcMethodDescription.Name, paramsKvp, retValKvp);

        StructType dtStructRpc = new StructType("MYRPCSTRUCT");
        dtStructRpc.AddMethod(rpc);
        serverSymbolFactory.AddType(dtStructRpc);
        serverSymbolFactory.AddSymbol("huhu.hallo", dtStructRpc, general);
    }

    private RpcMethod BuildRpcMethod(string name, IEnumerable<KeyValuePair<ParameterInfo, IDataType>> paramsKvp, KeyValuePair<ParameterInfo, IDataType> retValKvp)
    {
        var rpc = new RpcMethod(name);
        foreach (var (k, v) in paramsKvp)
        {
            rpc.AddParameter(k.Name ?? "unknown", v, k.IsOut ? MethodParamFlags.Out : MethodParamFlags.In);
        }

        rpc.SetReturnType(retValKvp.Value);
        return rpc;
    }

    private void AddToServerSymbolFactory(ServerSymbolFactory serverSymbolFactory, IEnumerable<KeyValuePair<ParameterInfo, IDataType>> paramsKvp)
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
            throw new ArgumentOutOfRangeException(nameof(paramInfo.ParameterType), "Expected only primitive types");
        }

        var dt = new PrimitiveType(paramInfo.Name ?? "unknown", paramInfo.ParameterType);
        return new KeyValuePair<ParameterInfo, IDataType>(paramInfo, dt);
    }

    private bool IsPrimitiveType(Type type)
    {
        return type.IsPrimitive;
    }

    private StringType ToStringType(ParameterInfo stringParameter)
    {
        var stringAttribute = stringParameter.GetCustomAttribute<StringAttribute>();
        if (stringAttribute is null)
        {
            _Logger?.LogWarning("No attribute for parameter {StringParameter} specified! Using default length {DefaultStringLength} and encoding {Encoding}",
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
            _Logger?.LogWarning("No attribute for parameter {StringParameter} specified! Using default length {DefaultStringLength} and encoding {Encoding}",
                stringField.Name, Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH, Common.Constants.DEFAULT_STRING_ENCODING);
            return new StringType(Common.Constants.DEFAULT_STRING_PARAMETER_LENGTH, Common.Constants.DEFAULT_STRING_ENCODING);
        }

        var stringType = new StringType(stringAttribute.SizeConst, Encoding.UTF8);
        return stringType;
    }
}
