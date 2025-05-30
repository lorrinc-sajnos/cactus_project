namespace CactusLang.Model.Visitors;

public class TagVisitor<T> : CodeModelVisitor<T> {
    private 
    protected TagVisitor(CodeModelVisitor<T> innerVisitor, T success, T failure) : base(success, failure) { }
}