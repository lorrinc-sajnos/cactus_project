using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Scopes;

public class StructFuncScope : Scope {
    private StructType _structType;
    private VariableSymbol _thisPtr;

    public StructFuncScope(StructType structType) {
        _structType = structType;
        _thisPtr = new VariableSymbol(_structType.GetPointer(), "this");

        InitVariables();
    }

    private void InitVariables() {
        foreach (var varSymbol in _structType.GetVariables()) {
            var result = this.AddVariable(varSymbol);
        }
    }

    public override FunctionSymbol? GetMatchingFunction(FuncId id) {
        if(_structType.ContainsMatchingFunction(id))
            return _structType.GetMatchingFunction(id);
        
        return base.GetMatchingFunction(id);
    }
}