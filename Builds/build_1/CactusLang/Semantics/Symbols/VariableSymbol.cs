using System;
using CactusLang.Cactus;
using CactusLang.Semantics.Types;

namespace CactusLang.Semantics.Symbols;

public class VariableSymbol {
    public  readonly List<Tag> Tags;
    public string Name {get; private set;}
    public CTSType Type {get;private set;}

    
}
