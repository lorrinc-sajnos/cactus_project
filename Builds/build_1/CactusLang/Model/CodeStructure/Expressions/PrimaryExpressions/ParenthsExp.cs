using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class ParenthsExp :  PrimaryExpression {
    private Expression _innerExpression;

    public ParenthsExp(Expression innerExpression) {
        _innerExpression = innerExpression;
    }
    public Expression InnerExpression => _innerExpression;
    public override BaseType GetResultType() => _innerExpression.GetResultType();
    
    public override bool IsLValue() => _innerExpression.IsLValue();
}