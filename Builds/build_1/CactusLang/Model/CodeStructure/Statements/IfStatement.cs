using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class IfStatement : Statement{
    private Expression _condition;
    private CodeBlock _body;
    
    public Expression Condition => _condition;
    public CodeBlock Body => _body;

    public IfStatement(CodeBlock codeBlock, Expression condition, CodeBlock body) : base(codeBlock) {
        _condition = condition;
        _body = body;
    }
        
}