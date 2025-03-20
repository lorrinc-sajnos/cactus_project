using System;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Semantics.Symbols;

public class VariableSymbol {
    public TagContainer Tags { get; private set; }
    public string Name { get; private set; }
    public BaseType Type { get; private set; }

    public int PointerLevel { get; private set; }

    public VariableSymbol(string name, BaseType type, int ptrLvl) {
        Tags = new TagContainer();
        Name = name;
        Type = type;
        PointerLevel = ptrLvl;
    }
}