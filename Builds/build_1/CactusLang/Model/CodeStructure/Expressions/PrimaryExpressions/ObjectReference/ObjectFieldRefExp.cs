using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class ObjectFieldRefExp : PrimaryExpression {
    private PrimaryExpression _object;
    public PrimaryExpression Object => _object;
    
    private ObjectFieldRef _fieldRef;
    public ObjectFieldRef FieldRef => _fieldRef;

    public override bool IsLValue() => true;
    public ObjectFieldRefExp(PrimaryExpression objExpression, ObjectFieldRef fieldRef) {
        _fieldRef = fieldRef;
        _object = objExpression;
    }

    public override BaseType GetResultType() => _fieldRef.Field!.Type;
}