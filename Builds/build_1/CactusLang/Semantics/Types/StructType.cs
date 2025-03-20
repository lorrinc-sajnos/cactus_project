using CactusLang.Semantics.Symbols;
using CactusLang.Util;

namespace CactusLang.Semantics.Types;

public class StructType : BaseType , IContainElements {
    private OrderedDictionary<string,VariableSymbol> _variables;
    private OrderedDictionary<string,FunctionSymbol> _functions;


    public StructType(string name) : base(name) {
        _variables = new();
        _functions = new();
    }

    public void AddVariable(VariableSymbol variable) => _variables.Add(variable.Name, variable);
    public void AddFunction(FunctionSymbol function) => _functions.Add(function.ID, function);


    public override int Size {
        get => -999; //TODO C generálás alapján kiszámolni...
    }

    public override bool CanImplicitCastTo(BaseType castTo) {
        return false;
    }
}