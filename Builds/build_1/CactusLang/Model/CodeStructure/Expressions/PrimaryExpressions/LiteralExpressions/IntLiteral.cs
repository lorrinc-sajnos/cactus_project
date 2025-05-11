using System.Numerics;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class IntLiteral :  LiteralExpression {
    BigInteger Value => _type.Value;
    
    LiteralIntegerType _type;
    LiteralIntegerType Type => _type;
}