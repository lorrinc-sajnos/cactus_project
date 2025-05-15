using CactusLang.Model.Types;

namespace CactusLang.Model.Operators;

public class AssignmentOperator : Operator{
    public AssignmentOperator(string format) : base(format, OperatorLvl.Assignment) { }
    
    public override BaseType Evaluate(BaseType lhs, BaseType rhs) {
        if (rhs.CanBeUsedAs(lhs))
            return lhs;
        
        return ErrorType.ERROR;
    }
}