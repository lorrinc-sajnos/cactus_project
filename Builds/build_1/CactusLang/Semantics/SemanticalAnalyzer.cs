using System;
using CactusLang.Semantics.Scopes;
using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class SemanticAnalyzer : GrammarBaseVisitor<StatusCode> {
    private TypeSystem _typeSystem;
    private GlobalScope _globalScope;


    public SemanticAnalyzer() {
        _typeSystem = new TypeSystem();
        GlobalScope _globalScope = new GlobalScope();
    }


    public void Analyze(GP.CodefileContext ctx) {
        RegisterIncludes(ctx);
        RegisterStructs(ctx);
        //After registering structs, the Type system is completed, can begin registering functions
    }

    public override StatusCode VisitCodefile(GP.CodefileContext ctx) {
        RegisterIncludes(ctx);
        RegisterStructs(ctx);


        return base.VisitCodefile(ctx);
    }

    private void RegisterIncludes(GP.CodefileContext ctx) {
        //TODO
    }

    private void RegisterStructs(GP.CodefileContext ctx) {
        var statements = ctx.globStatement();
        //First create the structs
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                string structName = structDcl.structName().GetText();
                StructType structType = new StructType(structName);

                Debug.LogLine($"Struct {structName} declared");
                _typeSystem.AddType(structType); //TODO err Already defined?
            }
        }

        //Then go over the header in their bodies, so they can cross-reference
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                string structName = structDcl.structName().GetText();
                Debug.LogLine($"@Struct  {structName}:");
                StructType structType = (StructType)_typeSystem.Get(structName);

                var structBody = structDcl.structBody();

                //Functions
                foreach (var funcDec in structBody.funcDcl()) {
                    structType.AddFunction(FuncSymbolFromCtx(funcDec));
                    Debug.LogLine($"\tAdded function: {funcDec.funcDclHeader().GetText()}");
                }

                //Fields
                foreach (var fieldDcl in structBody.fieldDcl()) {
                    foreach (VariableSymbol symb in VariableSymbolFromCtx(fieldDcl)) {
                        structType.AddVariable(symb);
                        Debug.LogLine($"\tAdded: {symb.Type}   {symb.Name}");
                    }
                }
            }
        }
    }

    private void RegisterFunctions(GP.CodefileContext ctx) {
        var statements = ctx.globStatement();
        //First create the structs
        foreach (var statement in statements) {
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();
                FunctionSymbol symbol = FuncSymbolFromCtx(funcDcl);
                _globalScope.AddFunction(symbol);
                
            }
        }
    }


    #region UTIL

    private FunctionSymbol FuncSymbolFromCtx(GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = _typeSystem.Get(header.returnType().GetText());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            BaseType pType = _typeSystem.Get(param.type().GetText());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }

        return funcSymb;
    }

    private List<VariableSymbol> VariableSymbolFromCtx(GP.FieldDclContext ctx) => VariableSymbolFromCtx(ctx.varDcl());

    private List<VariableSymbol> VariableSymbolFromCtx(GP.VarDclContext ctx) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = _typeSystem.Get(ctx.type().GetText());
        foreach (var varBody in ctx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }

    #endregion

    public override StatusCode VisitFuncDcl(GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();


        Debug.LogLine("ENTERED FUNCTION");
        var result = base.VisitFuncDcl(ctx);
        Debug.LogLine("EXITED FUNCTION");

        return result;
    }
}