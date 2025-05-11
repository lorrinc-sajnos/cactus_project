using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Operators;

public class BitShiftOperator : Operator{
    public BitShiftOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType valToShift, BaseType shiftCount) {
        if(valToShift is not PrimitiveType || shiftCount is not PrimitiveType) return ErrorType.ERROR;
        PrimitiveType primValToShift = valToShift as PrimitiveType;
        PrimitiveType primShiftCount = shiftCount as PrimitiveType;
        
        if(!primValToShift.IsInteger  || !primShiftCount.IsInteger) return  ErrorType.ERROR;

        return primValToShift;
    }
}