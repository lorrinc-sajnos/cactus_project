using CactusLang.Semantics;
using CactusLang.Semantics.Symbols;
using CactusLang.Util;

namespace CactusLang.Semantics.Types;

public class StructType : BaseType {
    private readonly string _id;
    public override string Name => _id;
    private OrderedDictionary<string,VariableSymbol> _variables;
    private OrderedDictionary<string,FunctionSymbol> _instanceFunctions;


    public StructType(string name) {
        _id = name;
        _variables = new();
        _instanceFunctions = new();
    }

    public void AddVariable(VariableSymbol variable) => _variables.Add(new(variable.Name), variable); //TODO kiegesziteni
    public void AddInstanceFunction(FunctionSymbol function) => _instanceFunctions.Add(function.ID, function);

    public List<VariableSymbol> GetVariables() {
        return _variables.Values.ToList();
    }

    public bool ContainsFunction(string id) => _instanceFunctions.ContainsKey(id);
    public FunctionSymbol GetFunction(string id) {
        return _instanceFunctions[id];
    }
    public override int Size {
        get => -999; //TODO C generálás alapján kiszámolni...
    }
}