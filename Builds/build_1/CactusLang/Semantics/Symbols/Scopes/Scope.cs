using System;

namespace CactusLang.Semantics.Symbols.Scopes;

public class Scope {
    public Scope Parent {get; private set;}
    private readonly Dictionary<string, VariableSymbol> variables;

    public Scope(Scope parentScope){
        Parent = parentScope;
        variables = new();
    }

    public VariableSymbol GetVariable(string id) => variables[id];
    public bool AddVariable(string id, VariableSymbol variable) { 
        if(variables.ContainsKey(id)) 
            return false;

        variables.Add(id, variable); 
        return true; 
    }
}
