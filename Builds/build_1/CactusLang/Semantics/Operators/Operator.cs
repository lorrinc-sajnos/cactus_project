using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Operators;

public abstract class Operator {
    public enum OperatorLvl { Mult, Add, Bit, Comparison }
    public OperatorLvl Level { get; private set; }

    private string _id;

    public string GetId() => _id;
    
    public static Operator GetById(string id) => _Operators[id];

    private static readonly Dictionary<string, Operator> _Operators = new();

    protected Operator(string format, OperatorLvl level) {
        Level = level;
        _id = format;
        _Operators.Add(format, this);
    }
    
    public abstract BaseType Evaluate(BaseType lhs, BaseType rhs);
    
    //Multiplication level
    public static readonly MathOperator Mult = new("*", OperatorLvl.Mult);
    public static readonly MathOperator Div = new("/", OperatorLvl.Mult);
    public static readonly IntOperator Remainder = new("%", OperatorLvl.Mult);
    
    //Addition level
    public static readonly MathOperator Add = new("+", OperatorLvl.Add);
    public static readonly MathOperator Sub = new("-", OperatorLvl.Add);
    //Bit level
    public static readonly IntOperator BitAnd = new("&", OperatorLvl.Bit);
    public static readonly IntOperator BitOr = new("|", OperatorLvl.Bit);
    public static readonly IntOperator BitXor = new("^", OperatorLvl.Bit);
    public static readonly IntOperator BitLeftShift = new("<<", OperatorLvl.Bit);
    public static readonly IntOperator BitRightShift = new(">>", OperatorLvl.Bit);
    //Comparison operator
    public static readonly CompOperator Eq = new("==",OperatorLvl.Comparison);
    public static readonly CompOperator Neq = new("!=",OperatorLvl.Comparison);
    public static readonly CompOperator And = new("&&",OperatorLvl.Comparison);
    public static readonly CompOperator Or = new("||",OperatorLvl.Comparison);
    public static readonly CompOperator LessThan = new("<",OperatorLvl.Comparison);
    public static readonly CompOperator GreaterThan = new(">",OperatorLvl.Comparison);
    public static readonly CompOperator LessThanEq = new("<=",OperatorLvl.Comparison);
    public static readonly CompOperator GreaterThanEq = new(">=",OperatorLvl.Comparison);
    
    
    
    
}