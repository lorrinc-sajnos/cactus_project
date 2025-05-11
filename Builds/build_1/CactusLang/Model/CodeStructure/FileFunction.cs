using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure;

public class FileFunction {
    private FunctionSymbol _symbol;
    private CodeBlock _codeBody;
    
    public CodeBlock CodeBody => _codeBody;
    public FunctionSymbol Symbol => _symbol;
}