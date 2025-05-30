using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Structs;

public class StructFunction : ModelFunction {
    private FileStruct _parent;

    public StructFunction(CodeFile file, FunctionSymbol symbol, FileStruct parent) : base(file, symbol) {
        _parent = parent;
    }

    public FileStruct Parent => _parent;
}