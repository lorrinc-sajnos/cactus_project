namespace CactusLang.Model.Types;

public class PrimitiveType : BaseType {
    private static bool _hasBeenInitialized = false;

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


    public const char FLOAT_POSTFIX_SINGLE = 'f';
    public const char FLOAT_POSTFIX_DOUBLE = 'd';

    public const string INT_PREFIX_BIN = "0b";
    public const string INT_PREFIX_HEX = "0x";
    public const string INT_PREFIX_OCT = "0o";

    public bool IsInteger => Kind is Type.Integer or Type.Uint;
    public bool IsNumber => Kind == Type.Float || IsInteger;
    public bool IsUnsigned => Kind == Type.Uint;

    private PrimitiveType(string id, Type kind, int size) {
        _id = id;
        Kind = kind;
        _size = size;
        _subset = new HashSet<PrimitiveType>();
        Primitives.Add(this);
    }

    private void Includes(PrimitiveType type) {
        _subset.Add(type);
        _subset.UnionWith(type._subset);
    }

    public bool IsSubsetOf(PrimitiveType superiorPrimitive) {
        return IsTrueSubsetOf(superiorPrimitive) || this.Equals(superiorPrimitive);
    }


    public bool IsTrueSubsetOf(PrimitiveType superiorPrimitive) {
        return superiorPrimitive._subset.Contains(this);
    }

    protected override bool CanImplicitCastInto(BaseType superiorType) {
        if (superiorType is PrimitiveType) {
            var superiorPrim = (PrimitiveType)superiorType;
            return this.IsSubsetOf(superiorPrim);
        }

        return false;
    }

    #region Primitives

    private static readonly HashSet<PrimitiveType> Primitives = new();

    public static HashSet<PrimitiveType> GetPrimitives() {
        return
        [ ..Primitives ];
        //new HashSet<CctsPrimitive>(primitives);
    }

    public static void InitPrimitives() {
        if (_hasBeenInitialized) return;
        _hasBeenInitialized = true;

        I16.Includes(I08);
        I16.Includes(U08);
        U16.Includes(U08);

        I32.Includes(I16);
        I32.Includes(U16);
        U32.Includes(U16);

        I64.Includes(I32);
        I64.Includes(U32);
        U64.Includes(U32);

        F32.Includes(I16);
        F32.Includes(U16);

        F64.Includes(F32);
        F64.Includes(I32);
        F64.Includes(U32);

        F128.Includes(F64);
        F128.Includes(I64);
        F128.Includes(U64);

        CH16.Includes(CH08);
        CH32.Includes(CH16);

        sbyte a = 53;
        long b = 7;
        var c = a % b;
    }

    public static readonly PrimitiveType F32 = new("f32", Type.Float, 4);
    public static readonly PrimitiveType F64 = new("f64", Type.Float, 8);
    public static readonly PrimitiveType F128 = new("f128", Type.Float, 16);

    //Integer types
    public static readonly PrimitiveType I08 = new("i08", Type.Integer, 1);
    public static readonly PrimitiveType I16 = new("i16", Type.Integer, 2);
    public static readonly PrimitiveType I32 = new("i32", Type.Integer, 4);
    public static readonly PrimitiveType I64 = new("i64", Type.Integer, 8);

    public static readonly PrimitiveType U08 = new("u08", Type.Uint, 1);
    public static readonly PrimitiveType U16 = new("u16", Type.Uint, 2);
    public static readonly PrimitiveType U32 = new("u32", Type.Uint, 4);
    public static readonly PrimitiveType U64 = new("u64", Type.Uint, 8);

    //Charater types
    public static readonly PrimitiveType CH08 = new("ch08", Type.Char, 1);
    public static readonly PrimitiveType CH16 = new("ch16", Type.Char, 2);
    public static readonly PrimitiveType CH32 = new("ch32", Type.Char, 4);

    public static readonly PrimitiveType BOOL = new("bool", Type.Bool, 1);
    public static readonly PrimitiveType VOID = new("void", Type.Void, 0);

    #endregion

    public HashSet<PrimitiveType> GetSubset() => _subset;
}