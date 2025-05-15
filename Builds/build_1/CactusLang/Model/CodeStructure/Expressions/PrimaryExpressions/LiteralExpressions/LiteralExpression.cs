namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public abstract class LiteralExpression : PrimaryExpression {
    public override bool IsLValue() => false;
}