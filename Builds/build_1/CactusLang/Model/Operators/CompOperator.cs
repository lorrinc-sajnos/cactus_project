using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Operators;

public class CompOperator : Operator {
    public CompOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if(!lhs.IsNumber  || !rhs.IsNumber){
            if (lhs.Equals(rhs))
                return PrimitiveType.BOOL;
            return ErrorType.ERROR;
        }
        
        if (lhs.CanBeUsedAs(rhs) || rhs.CanBeUsedAs(lhs))
            return PrimitiveType.BOOL;
        return ErrorType.ERROR;
    }
}