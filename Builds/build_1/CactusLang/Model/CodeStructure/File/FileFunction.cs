using System.Runtime.InteropServices.JavaScript;
using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.File;

public class FileFunction : ModelFunction {
    public FileFunction(FunctionSymbol symbol, CodeFile codeFile) : base(codeFile, symbol) {
        _codeFile = codeFile;
        _symbol = symbol;
    }
}