using System;

namespace CactusLang.Semantics.Types;

public abstract class BaseType {
    public string Name { get; private set; }
    public abstract int Size { get; }

    public BaseType(string name) {
        Name = name;
    }
}