using System;

namespace CactusLang.Semantics.Types;

public abstract class BaseType {
    public abstract string Name { get; }
    public abstract int Size { get; }

    public  PointerType GetPointer() {
        return new PointerType(this);
    }
}