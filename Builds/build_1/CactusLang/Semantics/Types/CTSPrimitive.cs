using System;
using System.Reflection.Metadata;

namespace CactusLang.Semantics.Types;

public class CTSPrimitive : CTSType {

    private static readonly HashSet<CTSPrimitive> primitives = new();
    public static readonly CTSPrimitive F32 = new("f32",4);
    public static readonly CTSPrimitive F64= new("f64", 8);
    public static readonly CTSPrimitive F128= new("f128", 16);

    //Integer types
    public static readonly CTSPrimitive I08 = new("i08", 1);
    public static readonly CTSPrimitive I16 = new("i16", 2);
    public static readonly CTSPrimitive I32 = new("i32", 4);
    public static readonly CTSPrimitive I64 = new("i64", 8);

    public static readonly CTSPrimitive UI08 = new("ui08", 1); 
    public static readonly CTSPrimitive UI16 = new("ui16", 2);
    public static readonly CTSPrimitive UI32 = new("ui32", 4);
    public static readonly CTSPrimitive UI64 = new("ui64", 8);

    //Charater types
    public static readonly CTSPrimitive CH08 = new("ch08", 1);
    public static readonly CTSPrimitive CH16 = new("ch16", 2);
    public static readonly CTSPrimitive CH32 = new("ch32", 4);

    public static readonly CTSPrimitive BOOL = new ("bool", 1);
    public static readonly CTSPrimitive VOID = new ("void", 1);

    private readonly HashSet<CTSPrimitive> subset;

    private CTSPrimitive(string id, int size) : base(id ,size) {
        subset = new();
        primitives.Add(this);
    }

    private void Includes(CTSPrimitive type){
        subset.Add(type);
        subset.UnionWith(type.subset);
    }

    public static void InitPrimitives(){
        I16.Includes(I08);
        I16.Includes(UI08);
        UI16.Includes(UI08);

        I32.Includes(I16);
        I32.Includes(UI16);
        UI32.Includes(UI16);

        I64.Includes(I32);
        I64.Includes(UI32);
        UI64.Includes(UI32);

        F32.Includes(I16);
        F32.Includes(UI16);
        
        F64.Includes(F32);
        F64.Includes(I32);
        F64.Includes(UI32);

        F128.Includes(F64);
        F128.Includes(I64);
        F128.Includes(UI64);

        CH16.Includes(CH08);
        CH32.Includes(CH16);
    }

    public override bool CanImplicitCastTo(CTSType castTo) {
        if(castTo is not CTSPrimitive)
            return false;

        return ((CTSPrimitive)castTo).subset.Contains(this);
    }

    public static HashSet<CTSPrimitive> GetPrimitives() => new(primitives);
}