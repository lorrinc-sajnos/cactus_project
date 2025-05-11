using CactusLang.Model.CodeStructure;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFieldRef : UnaryOp {
    private StructType _struct;
    //private 
    
    public ObjectFieldRef(StructType refferedType, string fieldName) : base(Side.RIGHT, fieldName) {
        _struct  = refferedType;
        
    }

    public override BaseType Evaluate(BaseType val) {
        
    }
}