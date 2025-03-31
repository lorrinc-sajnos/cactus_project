using System;
using System.Reflection.Metadata;

namespace CactusLang.Semantics.Types;

public class CctsPrimitive : BaseType {
    private static readonly HashSet<CctsPrimitive> primitives = new();
    public static readonly CctsPrimitive F32 = new("f32", 4);
    public static readonly CctsPrimitive F64 = new("f64", 8);
    public static readonly CctsPrimitive F128 = new("f128", 16);

    //Integer types
    public static readonly CctsPrimitive I08 = new("i08", 1);
    public static readonly CctsPrimitive I16 = new("i16", 2);
    public static readonly CctsPrimitive I32 = new("i32", 4);
    public static readonly CctsPrimitive I64 = new("i64", 8);

    public static readonly CctsPrimitive UI08 = new("ui08", 1);
    public static readonly CctsPrimitive UI16 = new("ui16", 2);
    public static readonly CctsPrimitive UI32 = new("ui32", 4);
    public static readonly CctsPrimitive UI64 = new("ui64", 8);

    //Charater types
    public static readonly CctsPrimitive CH08 = new("ch08", 1);
    public static readonly CctsPrimitive CH16 = new("ch16", 2);
    public static readonly CctsPrimitive CH32 = new("ch32", 4);

    public static readonly CctsPrimitive BOOL = new("bool", 1);
    public static readonly CctsPrimitive VOID = new("void", 1);

    private readonly HashSet<CctsPrimitive> _subset;


    private CctsPrimitive(string id, int size) : base(id) {
        _size = size;
        _subset = new HashSet<CctsPrimitive>();
        primitives.Add(this);
    }

    private void Includes(CctsPrimitive type) {
        _subset.Add(type);
        _subset.UnionWith(type._subset);
    }

    public static void InitPrimitives() {
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

    private int _size;

    public override int Size => _size;

    public override bool CanImplicitCastTo(BaseType castTo) {
        if (castTo is CctsPrimitive primitive && primitive._subset.Contains(this)) 
            return true;
        return false;
    }

    public static HashSet<CctsPrimitive> GetPrimitives() {
        return
        [ ..primitives ];
        //new HashSet<CctsPrimitive>(primitives);
    }
}