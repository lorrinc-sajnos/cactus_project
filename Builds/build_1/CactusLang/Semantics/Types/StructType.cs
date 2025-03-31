using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Symbols;
using CactusLang.Util;

namespace CactusLang.Semantics.Types;

public class StructType : BaseType , IContainElements {
    private OrderedDictionary<VarID,VariableSymbol> _variables;
    private OrderedDictionary<FuncID,FunctionSymbol> _functions;


    public StructType(string name) : base(name) {
        _variables = new();
        _functions = new();
    }

    public void AddVariable(VariableSymbol variable) => _variables.Add(new(variable.Name), variable); //TODO kiegesziteni
    public void AddFunction(FunctionSymbol function) => _functions.Add(function.ID, function);


    public override int Size {
        get => -999; //TODO C generálás alapján kiszámolni...
    }

    public override bool CanImplicitCastTo(BaseType castTo) {
        return false;
    }
}