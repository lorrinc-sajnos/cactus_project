namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class VarRef :  PrimaryExpression {
    string _varName;
    
    public string  VarName => _varName;
}