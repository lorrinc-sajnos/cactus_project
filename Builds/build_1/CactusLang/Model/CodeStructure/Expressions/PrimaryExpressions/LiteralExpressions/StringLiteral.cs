using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class StringLiteral : LiteralExpression {
    private string _value;
    public override BaseType GetResultType() => PrimitiveType.CH08.GetPointer();
    public override string Value => $"{_value}";

    public StringLiteral(string value) {
        _value = value;
    }
}