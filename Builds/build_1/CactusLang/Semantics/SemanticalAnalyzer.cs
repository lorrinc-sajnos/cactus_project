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
        _globalScope = new GlobalScope();
    }

    public void Analyze(GP.CodefileContext ctx) {
        RegisterIncludes(ctx);
        RegisterStructs(ctx);
        //After registering structs, the Type system is completed, can begin registering functions
        RegisterGlobalScope(ctx);
        Debug.LogLine("Registration finished.");
        Debug.LogLine("Beginning analization.");
        AnalyzeCodefile(ctx);
    }
    
    #region Registering

    private void RegisterIncludes(GP.CodefileContext ctx) {
        //TODO
        Debug.LogLine(":O");
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

                //Instance Functions
                foreach (var funcDec in structBody.funcDcl()) {
                    RegisterStructFuncDcl(structType, funcDec);
                    Debug.LogLine($"\tAdded function: {funcDec.funcDclHeader().GetText()}");
                }

                //Fields
                foreach (var fieldDcl in structBody.fieldDcl()) {
                    RegisterStructFieldDcl(structType, fieldDcl);
                    Debug.LogLine($"\tAdded field: {fieldDcl.GetText()}");
                }
            }
        }
    }
    private void RegisterGlobalScope(GP.CodefileContext ctx) {
        var globStatement = ctx.globStatement();
        
        foreach (var statement in globStatement) {
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();
                RegisterGlobFuncDcl(funcDcl);
            } else if (statement.globVarDcl() != null) {
                var globVarCtx = statement.globVarDcl();
                RegisterGlobVarDcl(globVarCtx);
            }
        }
    }
    private void RegisterGlobFuncDcl(GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = _typeSystem.Get(header.returnType().GetText());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            BaseType pType = _typeSystem.Get(param.type().GetText());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }
        
        _globalScope.AddFunction(funcSymb);
        Debug.LogLine($"Func {funcSymb.ID.Path} declared.");
    }
    private void RegisterGlobVarDcl(GP.GlobVarDclContext ctx) {
        var varCtx = ctx.varDcl();
        
        var varSymbols = GetVarSymbols(varCtx);

        foreach (VariableSymbol symb in varSymbols) {
            _globalScope.AddVariable(symb);
            Debug.LogLine($"GlobVar Added: {symb.Type.Name}   {symb.Name}");
        }
    }



    private void RegisterStructFuncDcl(StructType structType, GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = _typeSystem.Get(header.returnType().GetText());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            BaseType pType = _typeSystem.Get(param.type().GetText());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }
        
        structType.AddInstanceFunction(funcSymb);
    }
    private void RegisterStructFieldDcl(StructType structType, GP.FieldDclContext ctx) {
        var varCtx = ctx.varDcl();
        
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = _typeSystem.Get(varCtx.type().GetText());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        foreach (VariableSymbol symb in varSymbols) {
            structType.AddVariable(symb);
            Debug.LogLine($"\tAdded: {symb.Type.Name}   {symb.Name}");
        }
    }

    
    #endregion

    private void AnalyzeCodefile(GP.CodefileContext ctx) {
        foreach (var statement in ctx.globStatement()) {
            //Global function bodies
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();
                
                ProcessFunctionBody(funcDcl);
            } else //Structs function bodies
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                string structName = structDcl.structName().GetText();
                Debug.LogLine($"\n---@Struct {structName} ENTERED");
                StructType structType = (StructType)_typeSystem.Get(structName);
                
                //Loop through the functions of the struct
                var structFuncs = structDcl.structBody().funcDcl();
                _globalScope.StepInStructFunc(structType);
                foreach (var funcDclCtx in structFuncs) {
                    ProcessFunctionBody(funcDclCtx);
                }
                _globalScope.StepOut();
                Debug.LogLine($"\n---@Struct {structName} EXITED");
                Debug.LogLine();
            }
        }
    }
    
    private void ProcessFunctionBody(GP.FuncDclContext ctx) {
        //If function has C code, trust that it will work
        if (ctx.ppc__C_Code_Body() != null || ctx.ppc__C_Func_Map() != null) {
            return;
        }
        
        if (ctx.funcLamdBody() != null) {
            //TODO...
            return;
        }
        //Handle the most common case: lines of code:

        var codeBody = ctx.codeBody();
        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} ENTERED");
        VisitCodeBody(codeBody);
        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} EXITED\n");
    }

    public override StatusCode VisitCodeBody(GrammarParser.CodeBodyContext context) {
        _globalScope.StepIn();
        Debug.LogLine("Entering code body.");
        var result =  base.VisitCodeBody(context);
        _globalScope.StepOut();
        Debug.LogLine("Exiting code body.");
        return result;
    }
    
    #region Statement Handling
    public override StatusCode VisitStatement(GP.StatementContext ctx) {
        
        Debug.LogLine("statement:"+ctx.GetText());
        return base.VisitStatement(ctx);
    }

    public override StatusCode VisitVarDcl(GrammarParser.VarDclContext context) {
        List<VariableSymbol> varSymbols = GetVarSymbols(context);
        foreach (var varSymbol in varSymbols) {
            _globalScope.CurrentScope.AddVariable(varSymbol);
        }
        return base.VisitVarDcl(context);
    }
    
    #endregion
    
    private List<VariableSymbol> GetVarSymbols(GrammarParser.VarDclContext varCtx) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = _typeSystem.Get(varCtx.type().GetText());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }
}