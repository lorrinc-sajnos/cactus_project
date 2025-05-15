using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public class IntOperator : Operator {
    public IntOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if(!lhs.IsInteger  || !rhs.IsInteger) return  ErrorType.ERROR;

        if (lhs.CanBeUsedAs(rhs)) return rhs;
        if (rhs.CanBeUsedAs(lhs)) return lhs;
        return ErrorType.ERROR;
    }
}