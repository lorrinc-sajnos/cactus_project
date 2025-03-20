using System;
using System.Runtime.CompilerServices;
using CactusLang.Semantics.Types;
using CactusLang.Tags;
using CactusLang.Util;

namespace CactusLang.Semantics.Symbols;

public class VariableSymbol : IFinalizable{
    public TagContainer Tags { get; private set; }
    public string Name { get; private set; }
    public BaseType Type { get; private set; }

    public int PointerLevel { get; private set; }

    public VariableSymbol(BaseType type, string name, int ptrLvl) {
        Tags = new TagContainer();
        Name = name;
        Type = type;
        PointerLevel = ptrLvl;
    }
    
    public bool Finalize(TypeSystem typeSystem) {
        if (Type is not MissingType missing) return true;
        string rplcName = missing.Name;
        BaseType rplcType = typeSystem.Get(rplcName);
        
        if (rplcType == null) return false;
        
        Type = rplcType;
        return true;
    }
}