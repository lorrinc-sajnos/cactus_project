using CactusLang.Model.Operators;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public abstract class PrimaryExpression : Expression {
    private UnaryOp? _unaryOperation = null;
    public bool HasUnaryOperation => _unaryOperation != null;
    public UnaryOp? UnaryOperation => _unaryOperation;

    public void SetUnaryOperation(UnaryOp op) {
        _unaryOperation = op;
    }
}