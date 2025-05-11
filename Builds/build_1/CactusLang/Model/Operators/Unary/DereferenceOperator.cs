using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public class DereferenceOperator : UnaryOp {
    internal DereferenceOperator() : base(Side.LEFT, "*") { }

    public override BaseType Evaluate(BaseType val) {
        if (val is not PointerType ptr)
            return ErrorType.ERROR;

        return ptr.PointsTo;
    }
}