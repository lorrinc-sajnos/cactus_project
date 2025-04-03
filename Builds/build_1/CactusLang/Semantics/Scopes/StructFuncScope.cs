using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Scopes;

public class StructFuncScope : Scope {
    private StructType _structType;
    private VariableSymbol _thisPtr;

    public StructFuncScope(StructType structType) {
        _structType = structType;
        _thisPtr = new VariableSymbol(_structType, 1, "this");

        InitVariables();
    }

    private void InitVariables() {
        foreach (var varSymbol in _structType.GetVariables()) {
            this.AddVariable(varSymbol);
        }
    }

    public override FunctionSymbol GetFunction(FuncID id) {
        if(_structType.ContainsFunction(id))
            return _structType.GetFunction(id);
        
        return base.GetFunction(id);
    }
}