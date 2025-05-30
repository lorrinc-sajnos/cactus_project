using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure;

public class StructField : ModelField {
    protected FileStruct _parent;

    public StructField(VariableSymbol symbol, FileStruct parent) : base(symbol) {
        _parent = parent;
    }
    public FileStruct Parent => _parent;
}