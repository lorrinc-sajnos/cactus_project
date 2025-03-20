using System;
using CactusLang.Semantics.Scopes;
using CactusLang.Semantics.Symbols;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class SemanticAnalyzer : GrammarBaseVisitor<StatusCode> {
    TypeSystem typeSystem;
    GlobalScope globalScope;

    public SemanticAnalyzer() {
        typeSystem = new TypeSystem();
        GlobalScope scopeManager = new GlobalScope();
    }

    public override StatusCode VisitFunc_dcl(GP.Func_dclContext context) {
        var header = context.func_dcl_header();

        FunctionSymbol func = ParseFuncDeclHeader(header);
        
        Debug.Log("ENTERED FUNCTION");
        var result =  base.VisitFunc_dcl(context);
        Debug.Log("EXITED FUNCTION");
        
        return result;
    }
    
    FunctionSymbol ParseFuncDeclHeader(GP.Func_dcl_headerContext headerContext ){
        
        string idStr = headerContext.funcName().GetText();
        string returnTypeStr = headerContext.returnType().GetText();
        
        FunctionSymbol function = new FunctionSymbol(headerContext.funcName().GetText(), typeSystem.RequestType(returnTypeStr));
        
        var parameters = headerContext.param();
        if (parameters != null) {
            foreach (var parameter in parameters) {
                function.AddParameter(ParseParamContext(parameter));
            }
        }

        return function;
    }

    VariableSymbol ParseParamContext(GP.ParamContext paramContext) {
        return new VariableSymbol(
            paramContext.ID().GetText(), 
            typeSystem.RequestType(paramContext.type().GetText()),
            0//TODO
            );
    }
    
    
}