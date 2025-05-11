using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class FloatLiteral :  LiteralExpression {
    double _value;
    
    public double Value => _value;
    
    PrimitiveType  _type;   //TODO float literal!
    public PrimitiveType Type => _type;
}