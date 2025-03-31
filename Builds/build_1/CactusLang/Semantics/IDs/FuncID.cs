using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.IDs;

//TODO újragondolni


public struct FuncID  {
    public string Path { get; private set; }
    public BaseType ReturnType { get; private set; }
    
    private List<BaseType> _params;
    public void AddParam(BaseType param) {
        _params.Add(param);
    }

    public FuncID(string id, BaseType returnType, params BaseType[] parameters) : this(id, returnType, new List<BaseType>(parameters)) { }

    public FuncID(string path, BaseType returnType, List<BaseType> parameters) {
        Path = path;
        ReturnType = returnType;
        _params = parameters;
    }



    public override bool Equals(object? obj) {
        if (obj is not FuncID funcId)
            return false;

        if (!Path.Equals(funcId.Path))
            return false;

        if (!ReturnType.Equals(funcId.ReturnType))
            return false;

        if (_params.Count != funcId._params.Count)
            return false;

        for (int i = 0; i < _params.Count; i++) {
            if (!_params[i].Equals(funcId._params[i]))
                return false;
        }


        return true;
    }
}