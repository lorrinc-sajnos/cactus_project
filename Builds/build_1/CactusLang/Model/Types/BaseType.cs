using CactusLang.Semantics.Types;

namespace CactusLang.Model.Types;

public abstract class BaseType {
    public abstract string Name { get; }
    public abstract int Size { get; }
    
    public virtual bool IsInteger => false;
    public virtual bool IsNumber => false;
    public virtual bool IsUnsigned => false;
    
    public virtual BaseType GetPointer() {
        return new PointerType(this);
    }

    public bool CanBeUsedAs(BaseType superiorType) {
        return this.Equals(superiorType) || this.CanImplicitCastInto(superiorType);
    }
    
    protected abstract bool CanImplicitCastInto(BaseType superiorType);
    public override string ToString() => Name;
}