using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Operators;

public class MathOperator : Operator{
    public MathOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if(!lhs.IsNumber  || !rhs.IsNumber) return  ErrorType.ERROR;

        if (lhs.CanBeUsedAs(rhs)) return rhs;
        if (rhs.CanBeUsedAs(lhs)) return lhs;
        return ErrorType.ERROR;
    }
}