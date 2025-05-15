using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class BoolLiteral :  LiteralExpression {

    public BoolLiteral() { }
    public override BaseType GetResultType() => PrimitiveType.BOOL;
}