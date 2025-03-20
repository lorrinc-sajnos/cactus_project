using Antlr4.Runtime;
using CactusLang.Semantics.Scopes;
using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class GlobalAnalyzer : GrammarBaseVisitor<StatusCode>{
    GlobalScope  _globalScope;
    TypeSystem _typeSystem;
    
    public GlobalAnalyzer(GlobalScope globalScope, TypeSystem typeSystem) {
        _globalScope = globalScope;
        _typeSystem = typeSystem;
    }

    public override StatusCode VisitStruct_dcl(GrammarParser.Struct_dclContext ctx) {
        string structName = ctx.struct_name().GetText();
        var struct_body = ctx.struct_body();
        
        StructType newStruct = new(structName);
        var vars = struct_body.field_dcl();
        var result =  base.VisitStruct_dcl(ctx);
        
        return result;
    }
    
    
    
    

    VariableSymbol ParseFieldDclContext(GP.Field_dclContext ctx) {
        var type = _typeSystem.Get(ctx.type().GetText());
        var varNames = ctx.varName();
        
        return new VariableSymbol(
            , 
            type,
            0//TODO
        );
    }
    
    FunctionSymbol ParseFuncDeclHeader(GP.Func_dcl_headerContext headerContext){
        
        string idStr = headerContext.funcName().GetText();
        string returnTypeStr = headerContext.returnType().GetText();
        
        FunctionSymbol function = new FunctionSymbol(headerContext.funcName().GetText(), _typeSystem.Get(returnTypeStr));
        
        var parameters = headerContext.param();
        if (parameters != null) {
            foreach (var parameter in parameters) {
                function.AddParameter(ParseParamContext(parameter));
            }
        }

        return function;
    }

    VariableSymbol ParseParamContext(GP.ParamContext ctx) {
        return new VariableSymbol(
            ctx.ID().GetText(), 
            _typeSystem.Get(ctx.type().GetText()),
            0//TODO
        );
    }
}