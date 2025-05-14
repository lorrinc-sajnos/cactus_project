using System.Runtime.InteropServices.JavaScript;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.File;

public class FileFunction : ModelFunction {
    protected CodeFile _codeFile;

    public FileFunction(FunctionSymbol symbol, CodeFile codeFile) : base(symbol) {
        _codeFile = codeFile;
        _symbol = symbol;
    }

    public CodeBlock CodeBody => _codeBody;
}