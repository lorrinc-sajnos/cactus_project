using System.Numerics;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class IntLiteral :  LiteralExpression {
    LiteralIntegerType  _type;   //TODO float literal!
    public LiteralIntegerType Type => _type;

    public IntLiteral(LiteralIntegerType type) {
        _type = type;
    }

    public override BaseType GetResultType() => _type;

}