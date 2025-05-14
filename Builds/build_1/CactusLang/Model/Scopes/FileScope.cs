using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Errors;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Scopes;

public class FileScope : Scope {
    private Scope _currentScope;
    private readonly ErrorHandler _errorHandler;

    private readonly FunctionSymbolStore _fileFunctionSymbolStore;
    public FunctionSymbolStore FileFunctionSymbolStore => _fileFunctionSymbolStore;
    public Scope CurrentScope => _currentScope;

    public FileScope(ErrorHandler errorHandler) {
        _errorHandler = errorHandler;
        _currentScope = this;
        _fileFunctionSymbolStore = new();
    }

    public void StepIn() {
        Scope newScope = _currentScope.CreateChild();
        _currentScope = newScope;
    }

    public void StepInStructFunc(StructType structType) {
        StructFuncScope newScope = new StructFuncScope(structType);
        _currentScope.AddChild(newScope);
        _currentScope = newScope;
    }

    public void StepOut() {
        _currentScope = _currentScope.Parent;
    }

    //public override FunctionSymbol? GetExactFunction(FuncId id) => _fileFunctionStore.GetExactFunction(id);

    public override FunctionSymbol? GetMatchingFunction(FuncId id) => _fileFunctionSymbolStore.GetMatchingFunction(id);
    
    public bool AddFunction(FunctionSymbol func) => _fileFunctionSymbolStore.AddFunction(func);

    public override int GetScopeDepth() => 0;
}