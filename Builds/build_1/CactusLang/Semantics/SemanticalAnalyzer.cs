using System;
using Antlr4.Runtime;
using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.Errors;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics.Errors;
using CactusLang.Semantics.Types;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class SemanticAnalyzer : GrammarBaseVisitor<StatusCode> {
    private readonly GrammarParser _parser;
    private readonly CodeFile _codeFile;
    private readonly TypeSystem _typeSystem;
    private readonly FileScope _fileScope;
    private readonly ErrorHandler _errorHandler;
    public ErrorHandler ErrorHandler => _errorHandler;

    private Scope CurrentScope => _fileScope.CurrentScope;
    
    public SemanticAnalyzer(CodeSourceFile codeFile) {
        //Antlr
        var inputStream = new AntlrInputStream(codeFile.Code);
        var lexer = new GrammarLexer(inputStream);
        var tokenStream = new CommonTokenStream(lexer);
        _parser = new GrammarParser(tokenStream);
        
        _codeFile = new CodeFile(); 
        
        _errorHandler = new ErrorHandler();
        
        _parser.AddErrorListener(_errorHandler.ErrorListener);
        
        _fileScope = new FileScope(_errorHandler);
        _typeSystem = new TypeSystem(_errorHandler);
    }

    public void Analyze() {
        GP.CodefileContext ctx = _parser.codefile();
        
        Debug.LogLine($"Start index:\t{ctx.Start.StartIndex}");
        Debug.LogLine($"Stop index:\t{ctx.Stop.StopIndex}");
        
        RegisterIncludes(ctx);
        RegisterStructs(ctx);
        //After registering structs, the Type system is completed, can begin registering functions
        RegisterFileScope(ctx);
        //Debug.LogLine("Registration finished.");
        //Debug.LogLine("Beginning analization.");
        AnalyzeCodeFile(ctx);
    }

    private void AddParserErrors() {
        
    }
    
    #region Registering

    private void RegisterIncludes(GP.CodefileContext ctx) {
        //TODO
        Debug.LogLine(":O");
    }

    private void RegisterStructs(GP.CodefileContext ctx) {
        var statements = ctx.fileStatement();
        //First create the structs
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();

                //Debug.LogLine($"Struct {structName} declared");
                _typeSystem.AddStruct(structDcl); //TODO err Already defined?
            }
        }

        //Then go over the header in their bodies, so they can cross-reference
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                StructType structType = _typeSystem.GetStruct(structDcl);
                Debug.LogLine($"@Struct  {structType.Name}:");

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

    private void RegisterFileScope(GP.CodefileContext ctx) {
        var globStatement = ctx.fileStatement();

        foreach (var statement in globStatement) {
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();
                RegisterFileFuncDcl(funcDcl);
            }
            else if (statement.fileVarDcl() != null) {
                var globVarCtx = statement.fileVarDcl();
                RegisterFileVarDcl(globVarCtx);
            }
        }
    }

    private void RegisterFileFuncDcl(GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = _typeSystem.Get(header.returnType().type());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            BaseType pType = _typeSystem.Get(param.type());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }

        var result = _fileScope.AddFunction(funcSymb);
        if (result == false) {
            _errorHandler.PostError(CctsError.OVERLOAD_DOESNT_MATCH_RET_TYPE.CompTime(header.funcName()));
        }
        Debug.LogLine($"Func {funcSymb.Name} declared.");
    }

    private void RegisterFileVarDcl(GP.FileVarDclContext ctx) {
        var varCtx = ctx.varDcl();

        var varSymbols = GetVarSymbols(varCtx);

        foreach (VariableSymbol symb in varSymbols) {
            var result = _fileScope.AddVariable(symb);
            if (!result)
                _errorHandler.AddError(CctsError.ALREADY_DEFINED.CompTime(ctx));
            
            Debug.LogLine($"GlobVar Added: {symb.Type.Name}   {symb.Name}");
        }
    }


    private void RegisterStructFuncDcl(StructType structType, GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = _typeSystem.Get(header.returnType().type());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            BaseType pType = _typeSystem.Get(param.type());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }

        structType.AddInstanceFunction(funcSymb);
    }

    private void RegisterStructFieldDcl(StructType structType, GP.FieldDclContext ctx) {
        var varCtx = ctx.varDcl();

        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = _typeSystem.Get(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        foreach (VariableSymbol symb in varSymbols) {
            structType.AddVariable(symb);
            Debug.LogLine($"\tAdded: {symb.Type.Name}   {symb.Name}");
        }
    }

    #endregion

    private void AnalyzeCodeFile(GP.CodefileContext ctx) {
        foreach (var statement in ctx.fileStatement()) {
            //Global function bodies
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();

                ProcessFunctionBody(funcDcl);
            }
            else //Structs function bodies
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                StructType structType = _typeSystem.GetStruct(structDcl);
                Debug.LogLine($"\n---@Struct {structType.Name} ENTERED");

                //Loop through the functions of the struct
                var structFuncs = structDcl.structBody().funcDcl();
                _fileScope.StepInStructFunc(structType);
                
                foreach (var funcDclCtx in structFuncs) {
                    ProcessFunctionBody(funcDclCtx);
                }

                _fileScope.StepOut();
                Debug.LogLine($"\n---@Struct {structType.Name} EXITED");
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
        //Handle the most common case:
        
        
        FunctionSymbol? func = CurrentScope.GetMatchingFunction(GetIdFromDclCtx(ctx.funcDclHeader()));//Should be always non-null
        
        var codeBody = ctx.codeBody();
        
        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} ENTERED");
        //_fileScope.StepIn();
        //Add the parameters to the current scope
        foreach (var param in func.GetParameters()) {
            CurrentScope.AddVariable(param);    //In theory, scope is empty at this point, thus an error cannot occur.
        }
        
        VisitCodeBody(codeBody);
        
        //_fileScope.StepOut();
        
        
        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} EXITED\n");
        
    }

    private FuncId GetIdFromDclCtx(GP.FuncDclHeaderContext ctx) {
        string funcName = ctx.funcName().GetText();
        List<BaseType> paramTypes = new();
        foreach(var param in ctx.param()) {
            
            paramTypes.Add( _typeSystem.Get(param.type()) );
        }

        return new FuncId(funcName, paramTypes);
    }

    public override StatusCode VisitCodeBody(GrammarParser.CodeBodyContext context) {
        _fileScope.StepIn();
        Debug.LogLine("Entering code body.");
        var result = base.VisitCodeBody(context);
        _fileScope.StepOut();
        Debug.LogLine("Exiting code body.");
        return result;
    }

    #region Statement Handling

    private ExpressionTypeEvaluator CreateValidator(GP.ExpressionContext node) =>
        new(_errorHandler, _typeSystem, _fileScope.CurrentScope, node);
    
    
    public override StatusCode VisitStatement(GP.StatementContext ctx) {
        //Debug.LogLine("statement:"+ctx.GetText());
        return base.VisitStatement(ctx);
    }

    public override StatusCode VisitVarDcl(GrammarParser.VarDclContext ctx) {
        BaseType dclType = _typeSystem.Get(ctx.type());
        
        foreach (var dclBody in ctx.varDclBody()) {
            var varSymbol = new VariableSymbol(dclType, dclBody.varName().GetText());
            //Typecheck:
            if (dclBody.expression()!= null) {
                var asgExpr = dclBody.expression();
                var validator =  CreateValidator(asgExpr);
                var asgType = validator.EvaluateType();

                
                if (!asgType.CanBeUsedAs(dclType)) {
                    _errorHandler.PostError(CctsError.TYPE_MISMATCH.CompTime(ctx, asgType, dclType));
                }

            }
                
            _fileScope.CurrentScope.AddVariable(varSymbol);
        }

        return base.VisitVarDcl(ctx);
    }

    public override StatusCode VisitExpression(GrammarParser.ExpressionContext ctx) {
        var validator = CreateValidator(ctx);
        validator.EvaluateType();
        
        return base.VisitExpression(ctx);
    }

    #endregion

    private List<VariableSymbol> GetVarSymbols(GrammarParser.VarDclContext varCtx) {
        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = _typeSystem.Get(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        return varSymbols;
    }
    
    
}