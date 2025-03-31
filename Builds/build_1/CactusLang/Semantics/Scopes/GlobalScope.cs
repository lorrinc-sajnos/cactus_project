using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Symbols;
using CactusLang.Util;

namespace CactusLang.Semantics.Scopes;

public class GlobalScope : Scope {
    
    private Scope _currentScope;

    private OrderedDictionary<FuncID, FunctionSymbol> _globFunctions;
    public Scope CurrentScope => _currentScope;
    
    public GlobalScope() {
        _currentScope = this;
    }

    public void StepIn() {
        Scope newScope = _currentScope.CreateChild();
        _currentScope = newScope;
    }

    public void StepOut() {
        _currentScope = _currentScope.Parent;
    }
    
    public FunctionSymbol GetFunction(FuncID id) {
        if(_globFunctions.ContainsKey(id)) return _globFunctions[id];
        return null;
    }
    public bool AddFunction(FunctionSymbol variable) {
        if(_globFunctions.ContainsKey(variable.ID)) return false;
        
        _globFunctions.Add(variable.ID, variable);
        return true;
    }
}
