namespace CactusLang.Semantics.Types;

public class ErrorType : BaseType {
    public static readonly ErrorType ERROR = new("error");
    private ErrorType(string name) : base(name) { }
    public override int Size => 0;
}