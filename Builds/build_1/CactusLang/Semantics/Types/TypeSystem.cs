using System;
using CactusLang.Semantics.Types;

namespace CactusLang.Semantics;

public class TypeSystem {
    private Dictionary<string, CTSType> types;

    public TypeSystem(){
        types=new();
        initConst();
    }

    private void initConst(){
        //Primitives
        foreach(CTSType type in CTSPrimitive.GetPrimitives()){
            types.Add(type.Name, type);
        }
    }

}
