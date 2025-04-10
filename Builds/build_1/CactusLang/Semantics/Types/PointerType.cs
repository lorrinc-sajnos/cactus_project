namespace CactusLang.Semantics.Types;

public class PointerType : BaseType {
    private BaseType _pointsTo;
    
    public BaseType PointsTo => _pointsTo;
    
    
    internal PointerType(BaseType pointsTo) {
        _pointsTo = pointsTo;
    }

    public override int Size => PrimitiveType.I64.Size;
    public override string Name => $"{_pointsTo.Name}*";

    public static PointerType CreatePointer(BaseType pointsTo, int level) {
        PointerType previous = pointsTo.GetPointer();

        for (int i = 0; i < level -1 ; i++) {
            previous = previous.GetPointer();
        }
        return previous;
    }
}