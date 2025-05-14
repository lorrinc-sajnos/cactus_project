using Antlr4.Runtime.Tree;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Semantics;
using CactusLang.Semantics.Types;

namespace CactusLang.ModelBuilder;

public class CodeModelBuilder : GrammarBaseVisitor<StatusCode> {
    private readonly CodeFile _codeFile;
    private readonly FileScope _fileScope;
    private readonly TypeSystem _typeSystem;

    private FileStruct _currentStruct;
    private FileFunction _currentFunction; 
    
    private CodeBlock _currentBlock;
    
    
    
    public CodeModelBuilder(FileScope fileScope, TypeSystem typeSystem) {
        _typeSystem = typeSystem;
        _fileScope = fileScope;
        _codeFile = new CodeFile(_fileScope); 
    }


    public override StatusCode VisitFuncDcl(GrammarParser.FuncDclContext context) {
        FuncId id = SemanticUtil.GetIdFromDclCtx(context.funcDclHeader(),_typeSystem);
        //Due to analysis beforehand, function cannot be null
        _currentFunction = new(_fileScope.GetMatchingFunction(id)!, _codeFile);
        _currentBlock = _currentFunction.CodeBody;
        var baseResult = base.VisitFuncDcl(context);
        
        _codeFile.Functions.AddFunction(_currentFunction);
        
        return baseResult;
    }

    public override StatusCode VisitVarDcl(GrammarParser.VarDclContext context) {
        var varSymbols = SemanticUtil.GetVarSymbols(context, _typeSystem);
        
        return base.VisitVarDcl(context);
    }
}