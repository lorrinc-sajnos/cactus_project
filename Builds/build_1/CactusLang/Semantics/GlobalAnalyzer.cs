using Antlr4.Runtime;
using CactusLang.Semantics.Scopes;
using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class GlobalAnalyzer : GrammarBaseVisitor<StatusCode> {
    GlobalScope _globalScope;
    TypeSystem _typeSystem;

    public GlobalAnalyzer(GlobalScope globalScope, TypeSystem typeSystem) {
        _globalScope = globalScope;
        _typeSystem = typeSystem;
    }


    public override StatusCode VisitCodefile(GrammarParser.CodefileContext context) {
        //Init

        var result = base.VisitCodefile(context);
        //Round one finalization
        _typeSystem.Finalize();
        return result;
    }


    public override StatusCode VisitStruct_dcl(GrammarParser.Struct_dclContext ctx) {
        string structName = ctx.struct_name().GetText();
        var structBody = ctx.struct_body();

        StructType newStruct = new(structName);
        var varDecls = structBody.field_dcl();
        foreach (var localVarDecl in varDecls) {
            foreach (VariableSymbol variable in ParseFieldDclContext(localVarDecl)) {
                newStruct.AddVariable(variable);
            }
        }

        var funcDecls = structBody.func_dcl();
        foreach (var localFuncDecl in funcDecls) {
            _typeSystem.MissingTypeFlag = false;
            
            var func = ParseFuncDeclHeader(localFuncDecl.func_dcl_header());
            newStruct.AddFunction(func);
        }

        var result = base.VisitStruct_dcl(ctx);

        return result;
    }

    public override StatusCode VisitFunc_dcl(GrammarParser.Func_dclContext context) {
        return base.VisitFunc_dcl(context);
    }

    List<VariableSymbol> ParseFieldDclContext(GP.Field_dclContext ctx) {
        var typeName = ctx.type().GetText();
        var varNames = ctx.varName();

        List<VariableSymbol> vars = new List<VariableSymbol>();

        foreach (var varName in varNames) {
            vars.Add(_typeSystem.CreateOptmcVarSym(typeName, varName.GetText()));
        }

        return vars;
    }

    FunctionSymbol ParseFuncDeclHeader(GP.Func_dcl_headerContext headerContext) {
        string idStr = headerContext.funcName().GetText();
        string returnTypeStr = headerContext.returnType().GetText();

        FunctionSymbol function = _typeSystem.CreateOptmcFuncSym(returnTypeStr, headerContext.funcName().GetText());

        var parameters = headerContext.param();
        if (parameters != null) {
            foreach (var parameter in parameters) {
                function.AddParameter(ParseParamContext(parameter));
            }
        }

        return function;
    }

    VariableSymbol ParseParamContext(GP.ParamContext ctx) {
        return _typeSystem.CreateOptmcVarSym(
            ctx.type().GetText(),
            ctx.ID().GetText()
        );
    }
}