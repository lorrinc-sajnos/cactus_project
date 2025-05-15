using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class VarRef :  PrimaryExpression {
    private VariableSymbol _variable;
    public VariableSymbol Variable => _variable;
    public string  VarName => _variable.Name;

    public VarRef(VariableSymbol variable) {
        _variable = variable;
    }
    public override bool IsLValue() => true;

    public override BaseType GetResultType() => _variable.Type;
}