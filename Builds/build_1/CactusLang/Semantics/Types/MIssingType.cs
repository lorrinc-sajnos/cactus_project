namespace CactusLang.Semantics.Types;

public class MissingType : BaseType {
    
    public MissingType(string name) : base(name) { }
    
    
    public override int Size => 0;
    
    public override bool CanImplicitCastTo(BaseType castTo) {
        throw new Exception("Should not get here :O");
    }
}