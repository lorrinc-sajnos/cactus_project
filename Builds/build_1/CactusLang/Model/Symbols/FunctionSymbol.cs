using CactusLang.Model.Types;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Model.Symbols;

public class FunctionSymbol  {
    public TagContainer Tags { get; private set; }
    public string Name { get; private set; }
    
    public FuncId Id { get; private set; }
    public BaseType ReturnType { get; private set; }

    private Dictionary<string, VariableSymbol> _parameters;

    public FunctionSymbol(BaseType retType, string name) {
        Name = name;
        ReturnType = retType;
        _parameters = new();
        
        var parameterTypes = _parameters.Select(p => p.Value.Type).ToList();
        Id = new FuncId(Name, parameterTypes);
    }

    public void AddParameter(VariableSymbol parameter) {
        _parameters.Add(parameter.Name, parameter);
        Id.AddParam(parameter.Type);
    }
    public List<VariableSymbol> GetParameters() => _parameters.Values.ToList();
}