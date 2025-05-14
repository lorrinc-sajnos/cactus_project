using CactusLang.Model.Operators;
using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Operators;

public class BaseUnaryOp : UnaryOp {
    private readonly Func<PrimitiveType, bool> _check;

    public BaseUnaryOp(Side side, string format, Func<PrimitiveType, bool> check) : base(side, format) {
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