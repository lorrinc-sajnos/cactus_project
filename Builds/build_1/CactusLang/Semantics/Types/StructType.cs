using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Symbols;
using CactusLang.Util;

namespace CactusLang.Semantics.Types;

public class StructType : BaseType {
    private OrderedDictionary<VarID,VariableSymbol> _variables;
    private OrderedDictionary<FuncID,FunctionSymbol> _instanceFunctions;


    public StructType(string name) : base(name) {
        _variables = new();
        _instanceFunctions = new();
    }

    public void AddVariable(VariableSymbol variable) => _variables.Add(new(variable.Name), variable); //TODO kiegesziteni
    public void AddInstanceFunction(FunctionSymbol function) => _instanceFunctions.Add(function.ID, function);

    public List<VariableSymbol> GetVariables() {
        return _variables.Values.ToList();
    }

    public bool ContainsFunction(FuncID id) => _instanceFunctions.ContainsKey(id);
    public FunctionSymbol GetFunction(FuncID id) {
        return _instanceFunctions[id];
    }
    public override int Size {
        get => -999; //TODO C generálás alapján kiszámolni...
    }
}