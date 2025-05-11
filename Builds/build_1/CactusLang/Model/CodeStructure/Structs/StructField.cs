using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure;

public class StructField {
    private FileStruct _parent;
    private VariableSymbol _variable;
    
    public FileStruct Parent => _parent;
    public string Name => _variable.Name;
    public VariableSymbol Variable => _variable;
}