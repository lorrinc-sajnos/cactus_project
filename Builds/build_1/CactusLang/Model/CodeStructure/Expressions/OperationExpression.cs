using CactusLang.Model.Operators;

namespace CactusLang.Model.CodeStructure.Expressions;

public class OperationExpression : Expression {
    private Expression _lhs;
    private Expression _rhs;
    private Operator _operation;
    
    public Operator Operator => _operation;
    public Expression LeftExpression => _lhs;
    public Expression RightExpression => _rhs;
}