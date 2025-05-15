using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model.Scopes;

public class Scope {
    private Scope _parent;
    private List<Scope> _children;

    private OrderedDictionary<string, VariableSymbol> _variables;

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

    public virtual VariableSymbol? GetVariable(string id) {
        if (_variables.ContainsKey(id))
            return _variables[id];
        if (_parent != null)
            return _parent.GetVariable(id);
        return null;
        //throw new Exception($"Variable {id} does not exist.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="variable"></param>
    /// <returns>True, if a new variable was added.</returns>
    public bool AddVariable(VariableSymbol variable) {
        if (_variables.ContainsKey(variable.Id)) {
            return false;
        }

        _variables.Add(variable.Id, variable);
        return true;
    }

    public virtual ModelFunction? GetMatchingFunction(FuncId id) {
        return _parent.GetMatchingFunction(id);
    }

    public virtual int GetScopeDepth() {
        return _parent.GetScopeDepth() + 1;
    }
}