using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;

public class BoolLiteral :  LiteralExpression {
    public bool BoolValue {get; private set;}
    public override string Value => BoolValue ? "true" : "false";

    public BoolLiteral(string value) {
        if(value.Equals("true"))
            BoolValue = true;
        else if(value.Equals("false"))
            BoolValue = false;
        else 
            throw new Exception("Invalid boolean literal");
    }
    public override BaseType GetResultType() => PrimitiveType.BOOL;
}