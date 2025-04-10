using Antlr4.Runtime;

namespace CactusLang.Semantics;

public class ErrorHandler {
    private List<CompError> errors;
    
    public ErrorHandler() {
        errors = new List<CompError>();
    }

    public void AddError(CompError error) {
        errors.Add(error);
    }
    public void AddError(CctsError error, IToken start, IToken stop) => AddError(new CompError(error, start, stop));
    
    public  List<CompError> GetErrors() => errors;
}

public class CompError(CctsError error, IToken startToken, IToken endToken) {
    public CctsError Error { get; private set; } = error;
    public IToken StartToken { get; private set; } = startToken;
    public IToken EndToken { get; private set; } = endToken;
}