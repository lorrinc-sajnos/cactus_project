using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure;

public class StructFunction : ModelFunction {
    private FileStruct _parent;

    public StructFunction(FunctionSymbol symbol, FileStruct parent) : base(symbol) {
        _parent = parent;
    }

    public FileStruct Parent => _parent;
}