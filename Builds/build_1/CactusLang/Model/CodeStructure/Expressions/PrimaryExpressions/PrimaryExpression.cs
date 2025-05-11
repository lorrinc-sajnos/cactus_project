using CactusLang.Model.Operators;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public abstract class PrimaryExpression : Expression {
    private UnaryOp? _unaryOperation;
    
    public bool HasUnaryOperation => _unaryOperation != null;
    public UnaryOp? UnaryOperation => _unaryOperation;
}