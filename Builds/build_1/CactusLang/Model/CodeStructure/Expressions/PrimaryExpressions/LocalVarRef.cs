using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class LocalVarRef : PrimaryExpression {
    private StructFieldRefSymbol _variable;
    public StructFieldRefSymbol Variable => _variable;
    public string  VarName => _variable.Name;

    public LocalVarRef(StructFieldRefSymbol variable) {
        _variable = variable;
    }
    public override bool IsLValue() => true;

    public override BaseType GetResultType() => _variable.Type;
}