using System;

namespace CactusLang.Semantics;

public class ScopeManager {
    
}

struct ScopeEntry{
    public string ID {get; private set;}
    public TypeSystem Type {get; private set;}

    public ScopeEntry(string id, TypeSystem type) {
        Type = type;
        ID= id;
    }
}