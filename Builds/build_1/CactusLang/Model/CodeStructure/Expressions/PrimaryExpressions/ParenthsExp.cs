namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class ParenthsExp :  PrimaryExpression {
    private Expression _innerExpression;
    
    public Expression InnerExpression => _innerExpression;
}