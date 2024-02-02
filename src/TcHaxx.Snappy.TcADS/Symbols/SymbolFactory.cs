using Microsoft.Extensions.Logging;
using TwinCAT.Ads.Server.TypeSystem;
using TwinCAT.Ads.TypeSystem;

namespace TcHaxx.Snappy.TcADS.Symbols;

internal class SymbolFactory : ISymbolFactory
{
    private readonly IRpcMethodProvider _RpcMethodProvider;
    private readonly ILogger? _Logger;

    internal SymbolFactory(IRpcMethodProvider rpcMethodProvider, ILogger? logger)
    {
        _RpcMethodProvider = rpcMethodProvider;
        _Logger = logger;
    }

    public void AddSymbols(ServerSymbolFactory? serverSymbolFactory)
    {
        if (serverSymbolFactory is null)
        {
            return;
        }

        var rpc = _RpcMethodProvider.GetRpcMethodCollection();



        PrimitiveType dtBool = new PrimitiveType("BOOL", typeof(bool)); // 1-Byte size
        serverSymbolFactory.AddType(dtBool);
        // Define some ProcessImage DataAreas (virtual) used for IndexGroup/IndexOffset Alignments
        DataArea general = new DataArea("General", 0x01, 0x1000, 0x1000);

        serverSymbolFactory.AddDataArea(general);
        serverSymbolFactory.AddSymbol("Generals.bool1", dtBool, general);

        RpcMethod rpc1 = new RpcMethod("Method1");
        rpc1.AddParameter("b1", dtBool, TwinCAT.TypeSystem.MethodParamFlags.In);
        rpc1.SetReturnType(dtBool);

        StructType dtStructRpc = new StructType("MYRPCSTRUCT");
        dtStructRpc.AddMethod(rpc1);
        serverSymbolFactory.AddSymbol("Method1", dtStructRpc, general);
    }
}