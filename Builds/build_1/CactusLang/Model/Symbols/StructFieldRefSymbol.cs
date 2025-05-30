using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Types;

namespace CactusLang.Model.Symbols;

public class StructFieldRefSymbol : VariableSymbol {
    private FileStruct _parent;
    public FileStruct Parent => _parent;

    public StructFieldRefSymbol(FileStruct parent, VariableSymbol symbol) : base(symbol) {
        _parent = parent;
    }
}