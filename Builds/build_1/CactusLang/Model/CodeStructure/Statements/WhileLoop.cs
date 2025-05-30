using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class WhileLoop : Statement {
    private Expression _condition;
    private CodeBlock _body;
    
    public Expression Condition => _condition;
    public CodeBlock Body => _body;

    public WhileLoop(CodeBlock func, Expression condition) :  base(func) {
        _condition = condition;
        _body = new CodeBlock(func.Function);
    }
}