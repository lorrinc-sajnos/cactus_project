using Antlr4.Runtime;
using CactusLang.Model;
using CactusLang.Model.Errors;

namespace CactusLang.Semantics.Errors;

public class CompError {
    private ErrorHandler _errorHandler;
    public string Title { get; private set; }
    public string Msg { get; private set; }
    
    public int Code => Error.Code;
    public CctsError Error { get;  }

    public CodePos Start { get; private set; }
    public CodePos End { get; private set; }

    public IToken StartToken { get; private set; }
    public IToken EndToken { get; private set; }

    public CompError(CctsError error, ErrorHandler errorHandler, IToken startToken, IToken endToken, params object[] errParams) {
        Error = error;
        _errorHandler = errorHandler;
        try {
            Title = string.Format(error.Title, errParams);
            Msg = string.Format(error.Msg, errParams);
        }
        catch (Exception e) {
            Title = Error.Title+"|ERR";
            Msg = Error.Msg+"|ERR";
        }
        StartToken = startToken;
        EndToken = endToken;

        Start = new CodePos(_errorHandler.Filename, startToken.Line, startToken.Column);
        int endTokenLength = endToken.Text.Length;

        End = new CodePos(_errorHandler.Filename, endToken.Line, endToken.Column + endTokenLength);
    }

    public string AsPrettyString() => $"ERROR: {Start}\t\t[{Code:D4}]  {Title}";
}