using System;
using System.Reflection.Metadata;

namespace CactusLang.Semantics.Types;

public class PrimitiveType : BaseType {
    public enum Type {
        Float,
        Integer,
        Uint,
        Char,
        Bool,
        Void
    }

    public Type Kind { get; private set; }
    private readonly HashSet<PrimitiveType> _subset;
    private readonly string _id;
    public override string Name => _id;
    private int _size;
    public override int Size => _size;


    public bool IsInteger => Kind is Type.Integer or Type.Uint;
    public bool IsNumber => Kind == Type.Float || IsInteger;
    public bool IsUnsigned => Kind == Type.Uint;

    private PrimitiveType(string id, Type kind, int size) {
        _id = id;
        Kind = kind;
        _size = size;
        _subset = new HashSet<PrimitiveType>();
        primitives.Add(this);
    }

    private void Includes(PrimitiveType type) {
        _subset.Add(type);
        _subset.UnionWith(type._subset);
    }

    public bool IsSubsetOf(PrimitiveType primitive) {
        return primitive._subset.Contains(this);
    }


    #region Primitives

    private static readonly HashSet<PrimitiveType> primitives = new();

    public static HashSet<PrimitiveType> GetPrimitives() {
        return
        [ ..primitives ];
        //new HashSet<CctsPrimitive>(primitives);
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

    public static readonly PrimitiveType F32 = new("f32", Type.Float, 4);
    public static readonly PrimitiveType F64 = new("f64", Type.Float, 8);
    public static readonly PrimitiveType F128 = new("f128", Type.Float, 16);

    //Integer types
    public static readonly PrimitiveType I08 = new("i08", Type.Integer, 1);
    public static readonly PrimitiveType I16 = new("i16", Type.Integer, 2);
    public static readonly PrimitiveType I32 = new("i32", Type.Integer, 4);
    public static readonly PrimitiveType I64 = new("i64", Type.Integer, 8);

    public static readonly PrimitiveType UI08 = new("ui08", Type.Uint, 1);
    public static readonly PrimitiveType UI16 = new("ui16", Type.Uint, 2);
    public static readonly PrimitiveType UI32 = new("ui32", Type.Uint, 4);
    public static readonly PrimitiveType UI64 = new("ui64", Type.Uint, 8);

    //Charater types
    public static readonly PrimitiveType CH08 = new("ch08", Type.Char, 1);
    public static readonly PrimitiveType CH16 = new("ch16", Type.Char, 2);
    public static readonly PrimitiveType CH32 = new("ch32", Type.Char, 4);

    public static readonly PrimitiveType BOOL = new("bool", Type.Bool, 1);
    public static readonly PrimitiveType VOID = new("void", Type.Void, 0);

    #endregion
}