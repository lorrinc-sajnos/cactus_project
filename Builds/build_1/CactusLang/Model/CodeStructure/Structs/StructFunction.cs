using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Structs;

public class StructFunction : ModelFunction {
    private FileStruct _parent;

    public StructFunction(FunctionSymbol symbol, FileStruct parent) : base(symbol) {
        _parent = parent;
    }

    public FileStruct Parent => _parent;
}