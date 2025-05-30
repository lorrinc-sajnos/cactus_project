using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class Alloc : PrimaryExpression {
    private BaseType _type;
    public BaseType AllocType => _type;

    public Alloc(BaseType type) {
        _type = type;
    }
    public override BaseType GetResultType() => _type.GetPointer();

    public override bool IsLValue() => false;
}