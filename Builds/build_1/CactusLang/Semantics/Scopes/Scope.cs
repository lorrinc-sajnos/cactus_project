using CactusLang.Semantics.Symbols;
using CactusLang.Util;
using System.Collections.Specialized;
using CactusLang.Semantics.IDs;

namespace CactusLang.Semantics.Scopes;

public class Scope {
    private Scope _parent;
    private List<Scope> _children;

    private OrderedDictionary<VarID, VariableSymbol> _variables;

    protected Scope() : this(null) { }
    private Scope(Scope parent) {
        _parent = parent;
        _children = new();
        _variables = new();
    }

    public Scope CreateChild() => AddChild(new Scope(this));

    public Scope AddChild(Scope child) {
        _children.Add(child);
        child._parent = this;
        return child;
    }

    public Scope Parent => _parent;
    public virtual VariableSymbol GetVariable(VarID id) {
        if(_variables.ContainsKey(id)) 
            return _variables[id];
        if (_parent != null) 
            return _parent.GetVariable(id);
        throw new Exception($"Variable {id.Path} does not exist.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="variable"></param>
    /// <returns>True, if a new variable was added.</returns>
    public bool AddVariable(VariableSymbol variable) {
        if(_variables.ContainsKey(variable.ID))
            throw new Exception($"Variable {variable.ID} already exists");
        
        _variables.Add(variable.ID, variable);
        return true;
    }
    
    public virtual FunctionSymbol GetFunction(FuncID id) {
        return _parent.GetFunction(id);
    }
}