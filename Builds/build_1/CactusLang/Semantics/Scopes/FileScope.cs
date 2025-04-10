using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using CactusLang.Util;

namespace CactusLang.Semantics.Scopes;

public class FileScope : Scope {
    
    private Scope _currentScope;
    private readonly ErrorHandler _errorHandler;

    private readonly OrderedDictionary<string, FunctionSymbol> _fileFunctions;
    public Scope CurrentScope => _currentScope;
    
    public FileScope(ErrorHandler errorHandler) {
        _errorHandler = errorHandler;
        _currentScope = this;
        _fileFunctions = new OrderedDictionary<string, FunctionSymbol>();
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
     public override FunctionSymbol GetFunction(string id) {
        if(!_fileFunctions.ContainsKey(id)) 
            throw new Exception($"Function {id} does not exist.");
        
        return _fileFunctions[id];
    }
    public bool AddFunction(FunctionSymbol variable) {
        if(_fileFunctions.ContainsKey(variable.ID)) 
            throw new Exception($"Function {variable.ID} already exists");
        
        _fileFunctions.Add(variable.ID, variable);
        return true;
    }
}
