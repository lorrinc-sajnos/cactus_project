using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Operators;

public class MathOperator : Operator{
    public MathOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if(lhs is not PrimitiveType || rhs is not PrimitiveType) return ErrorType.ERROR;
        PrimitiveType pLhs = lhs as PrimitiveType;
        PrimitiveType pRhs = rhs as PrimitiveType;
        
        if(!pLhs.IsNumber  || !pRhs.IsNumber) return  ErrorType.ERROR;

        if (pLhs.IsSubsetOf(pRhs)) return pRhs;
        if (pRhs.IsSubsetOf(pLhs)) return pLhs;
        return ErrorType.ERROR;
    }
}