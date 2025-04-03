using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Operators;

public class CompOperator : Operator {
    public CompOperator(string format, OperatorLvl level) : base(format, level) { }

    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if (lhs is not PrimitiveType || rhs is not PrimitiveType) {
            if (lhs.Equals(rhs))
                return PrimitiveType.BOOL;
            return ErrorType.ERROR;
        }
        PrimitiveType pLhs = lhs as PrimitiveType;
        PrimitiveType pRhs = rhs as PrimitiveType;
        
        if(!pLhs.IsNumber  || !pRhs.IsNumber){
            if (lhs.Equals(rhs))
                return PrimitiveType.BOOL;
            return ErrorType.ERROR;
        }
        
        if (pLhs.IsSubsetOf(pRhs) || pRhs.IsSubsetOf(pLhs))
            return PrimitiveType.BOOL;
        return ErrorType.ERROR;
    }
}