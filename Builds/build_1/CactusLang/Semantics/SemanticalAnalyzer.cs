using System;
using Antlr4.Runtime;
using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.CodeStructure.Structs;
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
    private readonly TypeSystem _typeSystem;
    private readonly FileScope _fileScope;
    private readonly ErrorHandler _errorHandler;
    private readonly CodeFile _codeFile;
    private readonly TagFactory _tagFactory;
    public CodeFile CodeFile => _codeFile;
    public ErrorHandler ErrorHandler => _errorHandler;

    private Scope CurrentScope => _fileScope.CurrentScope;

    private CodeBlock _currentBlock;


    public SemanticAnalyzer(CodeSourceFile sourceFile) {
        //Antlr
        var inputStream = new AntlrInputStream(sourceFile.Code);
        var lexer = new GrammarLexer(inputStream);
        lexer.RemoveErrorListeners();
        var tokenStream = new CommonTokenStream(lexer);
        _parser = new GrammarParser(tokenStream);
        _codeFile = new CodeFile(sourceFile);

        _errorHandler = new ErrorHandler(sourceFile.Path);
        //Remove error listeners so it doesn't print it to the log
        _parser.RemoveErrorListeners();
        _parser.AddErrorListener(_errorHandler.ErrorListener);

        _fileScope = new FileScope(_codeFile, _errorHandler);
        _typeSystem = new TypeSystem(_errorHandler);
        _tagFactory = new();
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
                string structName = structDcl.structName().GetText();
                FileStruct fileStruct = new FileStruct(structName, _codeFile);
                //Debug.LogLine($"Struct {structName} declared");
                _codeFile.AddStruct(fileStruct);
                _typeSystem.AddStruct(structDcl, fileStruct); //TODO err Already defined?
            }
        }

        //Then go over the header in their bodies, so they can cross-reference
        foreach (var statement in statements) {
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                FileStruct fileStruct = _typeSystem.GetStruct(structDcl);
                Debug.LogLine($"@Struct  {fileStruct.Name}:");

                var structBody = structDcl.structBody();

                //Instance Functions
                foreach (var funcDec in structBody.funcDcl()) {
                    RegisterStructFuncDcl(fileStruct, funcDec);
                    Debug.LogLine($"\tAdded function: {funcDec.funcDclHeader().GetText()}");
                }

                //Fields
                foreach (var fieldDcl in structBody.fieldDcl()) {
                    RegisterStructFieldDcl(fileStruct, fieldDcl);
                    Debug.LogLine($"\tAdded field: {fieldDcl.GetText()}");
                }
            }
        }
    }

    private void RegisterFileScope(GP.CodefileContext ctx) {
        var globStatement = ctx.fileStatement();

        foreach (var fileStatement in globStatement) {
            if (fileStatement.funcDcl() != null) {
                var funcDcl = fileStatement.funcDcl();
                RegisterFileFuncDcl(funcDcl);
            }
            else if (fileStatement.fileVarDcl() != null) {
                var globVarCtx = fileStatement.fileVarDcl();
                RegisterFileVarDcl(globVarCtx);
            }
            else if (fileStatement.tags() != null) {
                _codeFile.AddTags(_tagFactory.CreateTags(fileStatement.tags()));
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

        FileFunction fileFunc = new FileFunction(funcSymb, _codeFile);
        var result = _codeFile.Functions.AddFunction(fileFunc);
        if (result == false) {
            _errorHandler.ErrorInExpression(CctsError.OVERLOAD_DOESNT_MATCH_RET_TYPE, ctx, header.funcName());
        }

        if (header.tags() != null)
            fileFunc.TagContainer.AddTags(_tagFactory.CreateTags(header.tags()));

        Debug.LogLine($"Func {funcSymb.Name} declared.");
    }

    private void RegisterFileVarDcl(GP.FileVarDclContext ctx) {
        var varCtx = ctx.varDcl();

        var varSymbols = SemanticUtil.GetVarSymbols(varCtx, _typeSystem);

        foreach (VariableSymbol symb in varSymbols) {
            FileField field = new FileField(symb, _codeFile);

            var result = _fileScope.AddVariable(symb);
            _codeFile.Fields.AddField(field);

            if (!result)
                _errorHandler.Error(CctsError.ALREADY_DEFINED, ctx);

            Debug.LogLine($"GlobVar Added: {symb.Type.Name}   {symb.Name}");
        }
    }


    private void RegisterStructFuncDcl(FileStruct fileStruct, GP.FuncDclContext ctx) {
        var header = ctx.funcDclHeader();
        string funcName = header.funcName().GetText();
        BaseType returnType = _typeSystem.Get(header.returnType().type());
        FunctionSymbol funcSymb = new FunctionSymbol(returnType, funcName);

        foreach (var param in header.param()) {
            string pName = param.paramName().GetText();
            BaseType pType = _typeSystem.Get(param.type());
            funcSymb.AddParameter(new VariableSymbol(pType, pName));
        }

        StructFunction structFunc = new StructFunction(_codeFile, funcSymb, fileStruct);
        fileStruct.Functions.AddFunction(structFunc);
    }

    private void RegisterStructFieldDcl(FileStruct fileStruct, GP.FieldDclContext ctx) {
        var varCtx = ctx.varDcl();

        List<VariableSymbol> varSymbols = new List<VariableSymbol>();
        BaseType type = _typeSystem.Get(varCtx.type());
        foreach (var varBody in varCtx.varDclBody()) {
            varSymbols.Add(new VariableSymbol(type, varBody.varName().GetText()));
        }

        foreach (VariableSymbol symb in varSymbols) {
            fileStruct.Fields.AddField(new StructField(symb, fileStruct));
            Debug.LogLine($"\tAdded: {symb.Type.Name}   {symb.Name}");
        }
    }

    #endregion

    private void AnalyzeCodeFile(GP.CodefileContext ctx) {
        foreach (var statement in ctx.fileStatement()) {
            //File function bodies
            if (statement.funcDcl() != null) {
                var funcDcl = statement.funcDcl();

                ProcessFunctionBody(funcDcl);
            }
            else //Structs function bodies
            if (statement.structDcl() != null) {
                var structDcl = statement.structDcl();
                FileStruct structType = _typeSystem.GetStruct(structDcl);
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
        ModelFunction? func =
            CurrentScope.GetMatchingFunction(
                SemanticUtil.GetIdFromDclCtx(ctx.funcDclHeader(), _typeSystem)
            ); //Should be always non-null

        _currentBlock = func.CodeBody;

        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} ENTERED");
        //_fileScope.StepIn();
        //Add the parameters to the current scope
        foreach (var param in func!.Symbol.GetParameters()) {
            CurrentScope.AddVariable(param); //In theory, scope is empty at this point, thus an error cannot occur.
        }

        //If function has C code, trust that it will work
        if (ctx.ppc__C_Code_Body() != null) {
            _currentBlock.AddStatement(new RawCCodeStatement(_currentBlock, ctx.ppc__C_Code_Body().GetText(), true));
            return;
        }

        if (ctx.ppc__C_Func_Map() != null) {
            _currentBlock.AddStatement(new RawCCodeStatement(_currentBlock,
                ctx.ppc__C_Func_Map().ppc__C_Func().GetText(), false));
            return;
        }

        if (ctx.funcLamdBody() != null) {
            var factory = CreateFactory(ctx.funcLamdBody().expression());
            var lamdbaExpr = factory.EvaluateExpression();

            if (func.ReturnType.Equals(PrimitiveType.VOID)) {
                _currentBlock.AddStatement(new ExpressionStatement(_currentBlock, lamdbaExpr));
            }
            else {
                _currentBlock.AddStatement(new ReturnStatement(_currentBlock, lamdbaExpr));
            }

            return;
        }

        //Handle the most common case:
        var codeBody = ctx.codeBody();

        VisitCodeBody(codeBody);

        //_fileScope.StepOut();


        Debug.LogLine($"FUNC {ctx.funcDclHeader().GetText()} EXITED\n");
    }

    private void FillVarDclStatement(GP.VarDclContext ctx, VarDclStatement varDclStatement) {
        BaseType dclType = _typeSystem.Get(ctx.type());

        foreach (var dclBody in ctx.varDclBody()) {
            var varSymbol = new VariableSymbol(dclType, dclBody.varName().GetText());

            //Typecheck:
            if (dclBody.expression() != null) {
                var asgExprCtx = dclBody.expression();
                var factory = CreateFactory(asgExprCtx);
                var asgExpression = factory.EvaluateExpression();


                if (!asgExpression.GetResultType().CanBeUsedAs(dclType)) {
                    _errorHandler.ErrorInExpression(CctsError.TYPE_MISMATCH, ctx, asgExpression.GetResultType(),
                        dclType);
                    //continue;
                }

                varDclStatement.AddBody(new VarDclStatement.Body(varSymbol, asgExpression));
            }
            else {
                varDclStatement.AddBody(new VarDclStatement.Body(varSymbol));
            }

            _fileScope.CurrentScope.AddVariable(varSymbol);
        }
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

    private ExpressionFactory CreateFactory(GP.ExpressionContext node) =>
        new(_errorHandler, _typeSystem, _fileScope.CurrentScope, node);


    public override StatusCode VisitStatement(GP.StatementContext ctx) {
        //Debug.LogLine("statement:"+ctx.GetText());
        return base.VisitStatement(ctx);
    }

    public override StatusCode VisitVarDcl(GrammarParser.VarDclContext ctx) {
        VarDclStatement varDclStatement = new(_currentBlock);

        FillVarDclStatement(ctx, varDclStatement);

        _currentBlock.AddStatement(varDclStatement);
        return base.VisitVarDcl(ctx);
    }

    public override StatusCode VisitExpressionStatement(GrammarParser.ExpressionStatementContext ctx) {
        var factory = CreateFactory(ctx.expression());
        var expression = factory.EvaluateExpression();


        var res = base.VisitExpressionStatement(ctx);

        _currentBlock.AddStatement(new ExpressionStatement(_currentBlock, expression));
        return res;
    }

    public override StatusCode VisitFree(GrammarParser.FreeContext ctx) {
        var factory = CreateFactory(ctx.expression());
        var expression = factory.EvaluateExpression();
        if (expression.GetResultType() is not PointerType) {
            _errorHandler.ErrorInExpression(CctsError.FREE_ERROR, ctx, ctx.expression().GetText());
        }

        _currentBlock.AddStatement(new FreeStatement(_currentBlock, expression));
        return base.VisitFree(ctx);
    }


    public override StatusCode VisitReturnStatement(GrammarParser.ReturnStatementContext ctx) {
        var factory = CreateFactory(ctx.expression());
        var expression = factory.EvaluateExpression();


        _currentBlock.AddStatement(new ReturnStatement(_currentBlock, expression));

        return base.VisitReturnStatement(ctx);
    }

    public override StatusCode VisitForLoop(GrammarParser.ForLoopContext ctx) {
        VarDclStatement loopDcl = new(_currentBlock);
        _fileScope.StepIn();
        FillVarDclStatement(ctx.loopDecl().varDcl(), loopDcl);

        var loopCondfactory = CreateFactory(ctx.loopCond().expression());
        var loopCond = loopCondfactory.EvaluateExpression();
        if (loopCond.GetResultType() != PrimitiveType.BOOL) {
            _errorHandler.ErrorInExpression(CctsError.CONDITION_ERROR, ctx.loopCond(), loopCond.GetResultType());
        }

        var endExpfactory = CreateFactory(ctx.endExp().expression());
        var endExp = endExpfactory.EvaluateExpression();


        ForLoop forLoop = new(loopDcl, loopCond, endExp, _currentBlock);
        var storeCurerntBlock = _currentBlock;
        _currentBlock = forLoop.LoopBody;
        var result = base.VisitCodeBody(ctx.codeBody());

        _fileScope.StepOut();

        _currentBlock = storeCurerntBlock;
        _currentBlock.AddStatement(forLoop);
        return result;
    }

    #endregion
}