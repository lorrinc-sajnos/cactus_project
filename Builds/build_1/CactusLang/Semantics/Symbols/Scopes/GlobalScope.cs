using System;

namespace CactusLang.Semantics.Symbols.Scopes;

public class GlobalScope : Scope{
    private Dictionary<string, FunctionSymbol> functions;

    public GlobalScope() : base(null) {
    }

    
    public FunctionSymbol GetFunction(string id) => functions[id];
    public bool AddVariable(string id, FunctionSymbol func) { 
        if(functions.ContainsKey(id)) 
            return false;

        functions.Add(id, func); 
        return true; 
    }
}
