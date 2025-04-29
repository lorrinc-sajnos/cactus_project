using CactusLang.Model.Types;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Model.Symbols;

public class FunctionSymbol  {
    public TagContainer Tags { get; private set; }
    public string Name { get; private set; }
    public BaseType ReturnType { get; private set; }

    private Dictionary<string, VariableSymbol> _parameters;

    public FunctionSymbol(BaseType retType, string id) {
        Name = id;
        ReturnType = retType;
        _parameters = new();
    }

    public void AddParameter(VariableSymbol parameter) => _parameters.Add(parameter.Name, parameter);

    public FuncId GetId() {
        var parameterTypes = _parameters.Select(p => p.Value.Type).ToList();
        return new FuncId(Name, parameterTypes);
    }
    public List<VariableSymbol> GetParameters() => _parameters.Values.ToList();
}