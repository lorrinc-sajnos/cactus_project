using CactusLang.Model.Types;
using CactusLang.Semantics.Types;

namespace CactusLang.Model.Operators;

public abstract class Operator {
    public enum OperatorLvl { Mult, Add, Bit, Comparison }
    public OperatorLvl Level { get; private set; }

    private string _id;

    public string GetId() => _id;
    
    public static Operator GetById(string id) => Operators[id];

    private static readonly Dictionary<string, Operator> Operators = new();

    protected Operator(string format, OperatorLvl level) {
        Level = level;
        _id = format;
        Operators.Add(format, this);
    }
    
    public abstract BaseType Evaluate(BaseType lhs, BaseType rhs);
    
    //Multiplication level
    public static readonly MathOperator MULT = new("*", OperatorLvl.Mult);
    public static readonly MathOperator DIV = new("/", OperatorLvl.Mult);
    public static readonly IntOperator REMAINDER = new("%", OperatorLvl.Mult);
    //Addition level
    public static readonly MathOperator ADD = new("+", OperatorLvl.Add);
    public static readonly MathOperator SUB = new("-", OperatorLvl.Add);
    //Bit level
    public static readonly IntOperator BIT_AND = new("&", OperatorLvl.Bit);
    public static readonly IntOperator BIT_OR = new("|", OperatorLvl.Bit);
    public static readonly IntOperator BIT_XOR = new("^", OperatorLvl.Bit);
    public static readonly IntOperator BIT_LEFT_SHIFT = new("<<", OperatorLvl.Bit);
    public static readonly IntOperator BIT_RIGHT_SHIFT = new(">>", OperatorLvl.Bit);
    //Comparison operator
    public static readonly CompOperator EQ = new("==",OperatorLvl.Comparison);
    public static readonly CompOperator NEQ = new("!=",OperatorLvl.Comparison);
    public static readonly CompOperator AND = new("&&",OperatorLvl.Comparison);
    public static readonly CompOperator OR = new("||",OperatorLvl.Comparison);
    public static readonly CompOperator LS_THAN = new("<",OperatorLvl.Comparison);
    public static readonly CompOperator GR_THAN = new(">",OperatorLvl.Comparison);
    public static readonly CompOperator LS_THAN_EQ = new("<=",OperatorLvl.Comparison);
    public static readonly CompOperator GR_THAN_EQ = new(">=",OperatorLvl.Comparison);
    
    
    
    
}