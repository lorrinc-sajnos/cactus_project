using CactusLang.Semantics.Symbols;
using CactusLang.Util;
using System.Collections.Specialized;

namespace CactusLang.Semantics.Scopes;

public class Scope {
    private Scope _parent;
    private List<Scope> _children;

    private OrderedDictionary<string, VariableSymbol> _variables;

    protected Scope() : this(null) { }
    private Scope(Scope parent) {
        _parent = parent;
        _children = new();
    }

    public Scope CreateChild() {
        _children.Add(_parent);
        return _children[^1];
    }

    public Scope Parent => _parent;
    public VariableSymbol GetVariable(string name) {
        if(_variables.ContainsKey(name)) return _variables[name];
        if (_parent != null) return _parent.GetVariable(name);
        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="variable"></param>
    /// <returns>True, if a new variable was added.</returns>
    public bool AddVariable(VariableSymbol variable) {
        if(_variables.ContainsKey(variable.Name)) return false;
        
        _variables.Add(variable.Name, variable);
        return true;
    }
}