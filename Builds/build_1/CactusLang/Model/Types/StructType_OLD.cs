using CactusLang.Model.Symbols;
using CactusLang.Semantics;
using CactusLang.Util;

namespace CactusLang.Model.Types;

//TODO REFARCTOR INTO FILESTRUCT!!!!
public class StructType_OLD : BaseType {
    private readonly string _id;
    public override string Name => _id;
    private OrderedDictionary<string, VariableSymbol> _variables;
    private FunctionSymbolStore _instanceFunctionsSymbol;


    public StructType_OLD(string name) {
        _id = name;
        _variables = new();
        _instanceFunctionsSymbol = new();
    }

    public void AddVariable(VariableSymbol variable) => _variables.Add(new(variable.Name), variable); //TODO kiegesziteni
    public void AddInstanceFunction(FunctionSymbol function) => _instanceFunctionsSymbol.AddFunction(function);

    public List<VariableSymbol> GetVariables() {
        return _variables.Values.ToList();
    }

    public bool ContainsMatchingFunction(FuncId id) => _instanceFunctionsSymbol.ContainsMatchingFunction(id);
    public FunctionSymbol? GetMatchingFunction(FuncId id) => _instanceFunctionsSymbol.GetMatchingFunction(id);

    public VariableSymbol? GetField(string name) {
        if  (_variables.ContainsKey(name)) { return _variables[name]; }
        return null;
    }
    public override int Size => -999; //TODO C generálás alapján kiszámolni...

    protected override bool CanImplicitCastInto(BaseType other) {
        throw new NotImplementedException();
    }
}