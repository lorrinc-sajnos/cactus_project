using System.Numerics;
using CactusLang.Util;

namespace CactusLang.Model.Types;

/// <summary>
/// This is the type of Integer literals. These are neccecary, because literals value matters when assigning, ie. 2 can be used as u08 and i08.
/// </summary>
public class LiteralIntegerType : BaseType {
    private List<BaseType> _canBeUsedAs;

    public BigInteger Value { get; private set; }
    public override string Name => $"Literal Integer ({Value})";
    public override int Size { get; }


    public override bool IsInteger => true;
    public override bool IsNumber => true;
    public override bool IsUnsigned => Value < 0;


    public LiteralIntegerType(BigInteger value, string? sign)  {
        Value = value;
        if (sign != null && sign.Equals("-"))
            value *= -1;
        _canBeUsedAs = IntegerParser.GetLiteralTypes(value);
    }

    //Maybe slow but it runs
    protected override bool CanImplicitCastInto(BaseType superiorType) {
        foreach (var possibleType in _canBeUsedAs) {
            if (possibleType.CanBeUsedAs(superiorType))
                return true;
        }

        return false;
    }
}