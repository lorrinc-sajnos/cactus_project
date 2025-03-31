using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class ParseUtil {

    public static FunctionSymbol FuncSymbolFromCtx(GP.FuncDclContext ctx, TypeSystem typeSystem) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = typeSystem.Get(header.returnType().GetText());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {

            string pName = param.paramName().GetText();
            BaseType pType = typeSystem.Get(param.type().GetText());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }
        return funcSymb;
    }
}
