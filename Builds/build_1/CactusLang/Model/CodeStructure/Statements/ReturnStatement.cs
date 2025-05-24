using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Statements;

public class ReturnStatement : Statement {
    private Expression _expression;
    
    public Expression Expression => _expression;
    public BaseType ReturnType => Expression.GetResultType();

    public ReturnStatement(Expression expression) {
        _expression = expression;
    }
}