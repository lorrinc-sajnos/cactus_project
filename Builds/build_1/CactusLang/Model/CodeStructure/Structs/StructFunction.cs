using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure;

public class StructFunction {
    private FileStruct _parent;
    private FunctionSymbol _symbol;
    private CodeBlock _codeBody;
    
    public FileStruct Parent => _parent;
    public FunctionSymbol Symbol => _symbol;
    public CodeBlock CodeBody => _codeBody;
}