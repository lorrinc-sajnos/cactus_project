using System.Text;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Symbols;

public struct FuncId {
    public string Name { get; private set; }

    private List<BaseType> _params;
    public List<BaseType> Params => _params;

    public void AddParam(BaseType param) {
        _params.Add(param);
    }

    public FuncId(string name, params BaseType[] parameters) : this(name, new List<BaseType>(parameters)) { }

    public FuncId(string name, List<BaseType> parameters) {
        Name = name;
        _params = parameters;
    }
    
    
    public bool DoesMatch(FuncId filter) {
        if (this.Name != filter.Name) return false;
        if (this.Params.Count != filter.Params.Count) return false;

        for (int i = 0; i < this.Params.Count; i++) {
            if (!this.Params[i].CanBeUsedAs(filter.Params[i])) return false;
        }
        
        return true;
    }



    public override bool Equals(object? obj) {
        if (obj is not FuncId funcId)
            return false;

        if (!Name.Equals(funcId.Name))
            return false;

        if (_params.Count != funcId._params.Count)
            return false;

        for (int i = 0; i < _params.Count; i++) {
            if (!_params[i].Equals(funcId._params[i]))
                return false;
        }


        return true;
    }

    public override string ToString() {
        StringBuilder  sb = new();
        sb.Append($"{Name}(");
        for (int i = 0; i < _params.Count - 1; i++) {
            sb.Append($"{_params[i]}, ");
        }
        if(_params.Count!=0)
            sb.Append($"{_params[_params.Count - 1]}");
        sb.Append(')');
        return sb.ToString();
    }
}