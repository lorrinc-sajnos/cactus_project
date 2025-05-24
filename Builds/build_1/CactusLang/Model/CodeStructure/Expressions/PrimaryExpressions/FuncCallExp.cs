using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class FuncCallExp : PrimaryExpression {
    private ModelFunction _function;
    public ModelFunction Function => _function;
    List<Expression> _parameters;
    public List<Expression> GetParameters() => _parameters;
    public override bool IsLValue() => false;

    public FuncCallExp(ModelFunction function, List<Expression> parameters) {
        _function = function;
        _parameters = parameters;
    }
    public override BaseType GetResultType() => _function.ReturnType;
}