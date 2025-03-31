using System;
using System.Runtime.CompilerServices;
using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Types;
using CactusLang.Tags;
using CactusLang.Util;

namespace CactusLang.Semantics.Symbols;

public class VariableSymbol {
    public TagContainer Tags { get; private set; }
    public string Name { get; private set; }
    public VarID ID { get; private set; }
    public BaseType Type { get; private set; }

    public int PointerLevel { get; private set; }

    public VariableSymbol(BaseType type, string name) : this(type, name, 0) { }

    public VariableSymbol(BaseType type, string name, int ptrLvl) {
        Tags = new TagContainer();
        Name = name;
        Type = type;
        ID = new(name);
        PointerLevel = ptrLvl;
    }
}