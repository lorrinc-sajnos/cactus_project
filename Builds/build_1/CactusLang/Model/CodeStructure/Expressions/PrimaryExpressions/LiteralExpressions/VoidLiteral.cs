using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class VoidLiteral :  LiteralExpression {
    public override string Value => "void";

    public VoidLiteral() { }
    public override BaseType GetResultType() => PrimitiveType.VOID;
}