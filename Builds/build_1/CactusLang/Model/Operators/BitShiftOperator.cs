using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Operators;

public class BitShiftOperator : Operator{
    public BitShiftOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType valToShift, BaseType shiftCount) {
        if(!valToShift.IsInteger  || !shiftCount.IsInteger) return  ErrorType.ERROR;

        return valToShift;
    }
}