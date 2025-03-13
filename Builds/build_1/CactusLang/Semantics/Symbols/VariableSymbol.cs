using System;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Semantics.Symbols;

public class VariableSymbol {
    public TagContainer Tags {get; private set;}
    public string Name {get; private set;}
    public CTSType Type {get;private set;}

    public int PointerLevel {get; private set;}

    public VariableSymbol(string name, CTSType type, int ptrLvl){
        Tags = new();
        Name = name;
        Type = type;
        PointerLevel = ptrLvl;        
    }
}
