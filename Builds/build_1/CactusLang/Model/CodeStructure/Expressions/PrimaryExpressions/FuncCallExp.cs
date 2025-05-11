using CactusLang.Model.Symbols;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class FuncCallExp : PrimaryExpression {
    FuncId _funcId;
    List<Expression> _parameters;
    
    public FuncId FuncId => _funcId;
    public List<Expression> GetParameters() => _parameters;
}