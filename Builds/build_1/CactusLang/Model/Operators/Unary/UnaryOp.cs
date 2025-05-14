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

    public static readonly BaseUnaryOp PRE_INC = new BaseUnaryOp(Side.LEFT, "++", t => t.IsNumber);
    public static readonly BaseUnaryOp PRE_DCR = new BaseUnaryOp(Side.LEFT, "--", t => t.IsNumber);
    public static readonly BaseUnaryOp POST_INC = new BaseUnaryOp(Side.RIGHT, "++", t => t.IsNumber);
    public static readonly BaseUnaryOp POST_DCR = new BaseUnaryOp(Side.RIGHT, "--", t => t.IsNumber);

    public static readonly BaseUnaryOp BOOL_NOT =
        new BaseUnaryOp(Side.LEFT, "!", t => t.Kind == PrimitiveType.Type.Bool);

    public static readonly BaseUnaryOp BIT_NOT = new BaseUnaryOp(Side.LEFT, "^", t => t.IsInteger);

    public static readonly AddressOperator ADDRESS = new AddressOperator();
    public static readonly DereferenceOperator DEREFERENCE = new DereferenceOperator();
}