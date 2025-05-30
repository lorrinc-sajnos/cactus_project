using CactusLang.Model.CodeStructure.CodeBlocks;

namespace CactusLang.Model.CodeStructure.Statements;

public class RawCCodeStatement : Statement {
    private string _rawCode;
    public string RawCode => _rawCode;

    public RawCCodeStatement(CodeBlock codeBlock, string rawCode, bool hasBrackets = true) :  base(codeBlock) {
        if (!hasBrackets) {
            _rawCode = rawCode + ";";
        }
        else {
            int length = rawCode.Length;

            _rawCode = rawCode[3..(length - 4)];
        }
    }
}