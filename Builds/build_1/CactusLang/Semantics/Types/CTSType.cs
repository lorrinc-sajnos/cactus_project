using System;

namespace CactusLang.Semantics.Types;

public abstract class CTSType {
    public string Name {get; private set;}
    public int Size {get; private set;}

    public CTSType(string name, int size){
        Name = name;
        if (size <0) throw new Exception("byte size cannot be negative."); //TODO rendes err handl
        Size = size;        
    }
    abstract public bool CanImplicitCastTo(CTSType castTo);
}
