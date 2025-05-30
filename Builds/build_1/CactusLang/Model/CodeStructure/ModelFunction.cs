using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Tags;

namespace CactusLang.Model;

public abstract class ModelFunction {
    private TagContainer _tagContainer;
    public TagContainer TagContainer => _tagContainer;
    
    protected FunctionSymbol _symbol;
    protected CodeBlock _codeBody;

    protected CodeFile _codeFile;
    public CodeFile CodeFile => _codeFile;

    public ModelFunction(CodeFile file, FunctionSymbol symbol) {
        _symbol = symbol;
        _codeBody = new CodeBlock(this);
        _codeFile = file;
        _tagContainer = new();
    }
    public CodeBlock CodeBody => _codeBody;
    public FunctionSymbol Symbol => _symbol;
    public string Name => _symbol.Name;
    public BaseType ReturnType => _symbol.ReturnType;
    public FuncId FuncId => _symbol.Id;
    
}