using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class ExpressionStatement : Statement {
    private Expression _expression;
    
    public  Expression Expression => _expression;

    public ExpressionStatement(CodeBlock codeBlock, Expression expression): base(codeBlock) {
        _expression = expression;
    }
    
}