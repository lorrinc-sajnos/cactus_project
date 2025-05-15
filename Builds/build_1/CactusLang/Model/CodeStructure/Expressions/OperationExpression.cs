using CactusLang.Model.Operators;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions;

public class OperationExpression : Expression {
    private BaseType _resultType;
    private Expression _lhs;
    private Expression _rhs;
    private Operator _operation;

    public Operator Operator => _operation;
    public Expression LeftExpression => _lhs;
    public Expression RightExpression => _rhs;

    public override bool IsLValue() => false;

    public OperationExpression(Expression lhs, Operator operation, Expression rhs) {
        _lhs = lhs;
        _rhs = rhs;
        _operation = operation;
        _resultType = _operation.Evaluate(_lhs.GetResultType(), _rhs.GetResultType());
    }

    public override BaseType GetResultType() => _resultType;
}