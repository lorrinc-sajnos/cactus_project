using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public class AddressOperator : UnaryOp{
    internal AddressOperator() : base(Side.LEFT, "&") { }
    
    public override BaseType Evaluate(BaseType val) {
        return val.GetPointer();
        // if (val is not PointerType ptr) 
        //     return ErrorType.ERROR;
        //
        // return ptr.PointsTo;
    }
}