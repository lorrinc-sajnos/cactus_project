using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class FloatLiteral :  LiteralExpression {
    PrimitiveType  _type;   //TODO float literal!
    public PrimitiveType Type => _type;
    public string FloatValue {get; private set;}
    public override string Value => FloatValue;

    public FloatLiteral(PrimitiveType type, string value) {
        FloatValue  = value;
        _type = type;
    }

    public override BaseType GetResultType() => _type;
}