using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Errors;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Scopes;

public class FileScope : Scope {
    private readonly CodeFile _codeFile;
    private Scope _currentScope;
    private readonly ErrorHandler _errorHandler;
    public Scope CurrentScope => _currentScope;

    public FileScope(CodeFile codeFile,ErrorHandler errorHandler) {
        _codeFile = codeFile;
        _errorHandler = errorHandler;
        _currentScope = this;
        //_fileFunctionSymbolStore = new();
    }

    public void StepIn() {
        Scope newScope = _currentScope.CreateChild();
        _currentScope = newScope;
    }

    public void StepInStructFunc(FileStruct fileStruct) {
        StructFuncScope newScope = new StructFuncScope(fileStruct);
        _currentScope.AddChild(newScope);
        _currentScope = newScope;
    }

    public void StepOut() {
        _currentScope = _currentScope.Parent;
    }

    //public override FunctionSymbol? GetExactFunction(FuncId id) => _fileFunctionStore.GetExactFunction(id);

    public override ModelFunction? GetMatchingFunction(FuncId id) => _codeFile.Functions.GetMatchingFunction(id);
    
    public override int GetScopeDepth() => 0;
}