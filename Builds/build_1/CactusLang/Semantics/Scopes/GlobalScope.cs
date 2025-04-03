using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using CactusLang.Util;

namespace CactusLang.Semantics.Scopes;

public class GlobalScope : Scope {
    
    private Scope _currentScope;

    private readonly OrderedDictionary<FuncID, FunctionSymbol> _globFunctions;
    public Scope CurrentScope => _currentScope;
    
    public GlobalScope() {
        _currentScope = this;
        _globFunctions = new OrderedDictionary<FuncID, FunctionSymbol>();
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
     public override FunctionSymbol GetFunction(FuncID id) {
        if(!_globFunctions.ContainsKey(id)) 
            throw new Exception($"Function {id.Path} does not exist.");
        return _globFunctions[id];
    }
    public bool AddFunction(FunctionSymbol variable) {
        if(_globFunctions.ContainsKey(variable.ID)) 
            throw new Exception($"Function {variable.ID} already exists");
        
        _globFunctions.Add(variable.ID, variable);
        return true;
    }
}
