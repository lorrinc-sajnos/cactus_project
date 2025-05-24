using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model;

public abstract class ModelFunction {
    protected FunctionSymbol _symbol;
    protected CodeBlock _codeBody;

    public ModelFunction(FunctionSymbol symbol) {
        _symbol = symbol;
        _codeBody = new CodeBlock();
    }
    public CodeBlock CodeBody => _codeBody;
    public FunctionSymbol Symbol => _symbol;
    public string Name => _symbol.Name;
    public BaseType ReturnType => _symbol.ReturnType;
    public FuncId FuncId => _symbol.Id;
}