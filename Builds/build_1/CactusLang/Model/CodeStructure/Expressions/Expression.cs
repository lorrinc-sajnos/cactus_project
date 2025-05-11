using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions;

public abstract class Expression {
    protected BaseType _resultType;
    public BaseType ResultType => _resultType;
}