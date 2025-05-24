using CactusLang.Semantics.Types;

namespace CactusLang.Model.Types;

public class PointerType : BaseType {
    private BaseType _pointsTo;
    
    public BaseType PointsTo => _pointsTo;
    
    
    internal PointerType(BaseType pointsTo) {
        _pointsTo = pointsTo;
    }

    public override int Size => PrimitiveType.I64.Size;
    public override string Name => $"{_pointsTo.Name}*";

    public static BaseType CreatePointer(BaseType pointsTo, int level) {
        if (pointsTo is ErrorType)
            return ErrorType.ERROR;
        
        PointerType previous = (PointerType)pointsTo.GetPointer();

        for (int i = 0; i < level -1 ; i++) {
            previous = (PointerType)previous.GetPointer();
        }
        return previous;
    }

    protected override bool CanImplicitCastInto(BaseType other) {
        //TODO
        return false;
    }

    public override bool Equals(object? obj) {
        if (obj is PointerType other) {
            return this.PointsTo.Equals(other.PointsTo);
        }

        return false;
    }
}