using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public abstract class UnaryOp {
    public enum Side {
        LEFT,
        RIGHT
    }

    private string _id;

    public string GetId() => _id;
    public Side _side;
    public Side OpSide => _side;

    public static UnaryOp GetLeftById(string id) => _LeftUnaryOp[id];
    public static UnaryOp GetRightById(string id) => _RightUnaryOp[id];

    private static readonly Dictionary<string, UnaryOp> _LeftUnaryOp = new();
    private static readonly Dictionary<string, UnaryOp> _RightUnaryOp = new();

    protected UnaryOp(Side side, string format) {
        _id = format;
        _side = side;
        if (side == Side.LEFT)
            _LeftUnaryOp[format] = this;
        else if (side == Side.RIGHT)
            _RightUnaryOp[format] = this;
    }

    public abstract BaseType Evaluate(BaseType val);

    public static readonly CheckUnaryOp PRE_INC = new CheckUnaryOp(Side.LEFT, "++", t => t.IsNumber);
    public static readonly CheckUnaryOp PRE_DCR = new CheckUnaryOp(Side.LEFT, "--", t => t.IsNumber);
    public static readonly CheckUnaryOp POST_INC = new CheckUnaryOp(Side.RIGHT, "++", t => t.IsNumber);
    public static readonly CheckUnaryOp POST_DCR = new CheckUnaryOp(Side.RIGHT, "--", t => t.IsNumber);

    public static readonly CheckUnaryOp BOOL_NOT =
        new CheckUnaryOp(Side.LEFT, "!", t => t.Kind == PrimitiveType.Type.Bool);

    public static readonly CheckUnaryOp BIT_NOT = new CheckUnaryOp(Side.LEFT, "^", t => t.IsInteger);

    public static readonly AddressOperator ADDRESS = new AddressOperator();
    public static readonly DereferenceOperator DEREFERENCE = new DereferenceOperator();
}