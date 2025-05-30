using CactusLang.Model.Operators;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class UnOpPrimaryExpression : PrimaryExpression {
    private PrimaryExpression _primExpr;
    public PrimaryExpression PrimaryExpression => _primExpr;
    private UnaryOp _unaryOperation = null;
    public UnaryOp UnaryOperation => _unaryOperation;

    
    
    public UnOpPrimaryExpression(PrimaryExpression exp, UnaryOp op) {
        _primExpr = exp;
        _unaryOperation = op;
    }
    public override BaseType GetResultType() => _unaryOperation.Evaluate(_primExpr.GetResultType());
    public override bool IsLValue() => _primExpr.IsLValue();
}