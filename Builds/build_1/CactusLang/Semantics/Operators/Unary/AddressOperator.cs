using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Operators;

public class AddressOperator : UnaryOp{
    internal AddressOperator() : base(Side.LEFT, "&") { }
    
    public override BaseType Evaluate(BaseType val) {
        if (val is not PointerType ptr) 
            return ErrorType.ERROR;

        return ptr.PointsTo;
    }
}