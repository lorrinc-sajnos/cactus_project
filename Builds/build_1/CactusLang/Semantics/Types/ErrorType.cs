namespace CactusLang.Semantics.Types;

public class ErrorType : BaseType {
    public override string Name => "error";
    public override int Size => 0;
    private ErrorType() { }
    public static readonly ErrorType ERROR = new();
}