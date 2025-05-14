using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.File;

public class FileField : ModelField {
    protected CodeFile _codeFile;

    public FileField(VariableSymbol symbol, CodeFile codeFile) : base(symbol) {
        _codeFile = codeFile;
    }
    
    public CodeFile CodeFile => _codeFile;
}