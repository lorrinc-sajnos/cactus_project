using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class CharLiteral :  LiteralExpression {
    PrimitiveType  _type;   //TODO float literal!
    public PrimitiveType Type => _type;

    public CharLiteral(PrimitiveType type) {
        _type = type;
    }

    public override BaseType GetResultType() => _type;
}