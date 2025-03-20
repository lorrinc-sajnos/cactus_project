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
    
    
}