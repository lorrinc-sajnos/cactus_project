using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Scopes;

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

    public override FunctionSymbol GetFunction(string id) {
        if(_structType.ContainsFunction(id))
            return _structType.GetFunction(id);
        
        return base.GetFunction(id);
    }
}