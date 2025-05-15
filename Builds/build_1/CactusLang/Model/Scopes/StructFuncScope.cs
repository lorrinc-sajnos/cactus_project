using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Scopes;

public class StructFuncScope : Scope {
    private FileStruct _fileStruct;
    private VariableSymbol _thisPtr;

    public StructFuncScope(FileStruct fileStruct) {
        _fileStruct = fileStruct;
        _thisPtr = new VariableSymbol(_fileStruct.StructType.GetPointer(), "this");

        InitVariables();
    }

    private void InitVariables() {
        foreach (var varSymbol in _fileStruct.Fields.GetFields()) {
            var result = this.AddVariable(varSymbol.Symbol);
        }
    }

    public override ModelFunction? GetMatchingFunction(FuncId id) {
        if(_fileStruct.Functions.ContainsMatchingFunction(id))
            return _fileStruct.Functions.GetMatchingFunction(id);
        
        return base.GetMatchingFunction(id);
    }
}