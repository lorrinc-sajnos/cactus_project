using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public static class SemanticUtil {
    
    public static FuncId GetIdFromDclCtx(GP.FuncDclHeaderContext ctx, TypeSystem typeSystem) {
        string funcName = ctx.funcName().GetText();
        List<BaseType> paramTypes = new();
        foreach(var param in ctx.param()) {
            
            paramTypes.Add(typeSystem.Get(param.type()) );
        }

        return new FuncId(funcName, paramTypes);
    }
    
    
    public static List<VariableSymbol> GetVarSymbols(GrammarParser.VarDclContext varCtx, TypeSystem typeSystem) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = typeSystem.Get(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }
}