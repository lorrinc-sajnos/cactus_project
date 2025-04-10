using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Operators;

public class CheckUnaryOp: UnaryOp {
    private readonly Func<PrimitiveType,bool>  _check;
    
    public CheckUnaryOp(Side side, string format, Func<PrimitiveType,bool> check ) : base(side, format) {
        _check = check;
    }

    public override BaseType Evaluate(BaseType val) {
        if (val is not PrimitiveType) 
            return ErrorType.ERROR;
        var prim = val as PrimitiveType;
        
        if (_check(prim))
            return val;
        return ErrorType.ERROR;
    }
    
}