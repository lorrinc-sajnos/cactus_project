using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public abstract class UnaryOp {
    public enum Side {LEFT, RIGHT}
    private string _id;

    public string GetId() => _id;
    
    public static UnaryOp GetLeftById(string id) => _LeftUnaryOp[id];
    public static UnaryOp GetRightById(string id) => _RightUnaryOp[id];

    private static readonly Dictionary<string, UnaryOp> _LeftUnaryOp = new();
    private static readonly Dictionary<string, UnaryOp> _RightUnaryOp = new();

    protected UnaryOp(Side side, string format) {
        _id = format;
        if (side == Side.LEFT)
            _LeftUnaryOp[format] = this;
        else if (side==Side.RIGHT)
            _RightUnaryOp[format] = this;
        
    }
    public abstract BaseType Evaluate(BaseType val);
    
    public static readonly UnaryOp PRE_INC = new CheckUnaryOp(Side.LEFT, "++", t => t.IsNumber );
    public static readonly UnaryOp PRE_DCR = new CheckUnaryOp(Side.LEFT, "--", t => t.IsNumber );
    public static readonly UnaryOp POST_INC = new CheckUnaryOp(Side.RIGHT, "++", t => t.IsNumber );
    public static readonly UnaryOp POST_DCR = new CheckUnaryOp(Side.RIGHT, "--", t => t.IsNumber );
    
    
    public static readonly UnaryOp BOOL_NOT = new CheckUnaryOp(Side.LEFT, "!", t => t.Kind ==  PrimitiveType.Type.Bool );
    public static readonly UnaryOp BIT_NOT = new CheckUnaryOp(Side.LEFT, "^", t => t.IsInteger );
    
    public static readonly UnaryOp ADDRESS = new AddressOperator();
    
    
    
}