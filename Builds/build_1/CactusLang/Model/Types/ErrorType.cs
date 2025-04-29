namespace CactusLang.Model.Types;

public class ErrorType : BaseType {
    public override string Name => "error";
    public override int Size => 0;
    private ErrorType() { }
    public static readonly ErrorType ERROR = new();

    public override BaseType GetPointer() => ERROR;

    protected override bool CanImplicitCastInto(BaseType other) {
        return false;
    }
}