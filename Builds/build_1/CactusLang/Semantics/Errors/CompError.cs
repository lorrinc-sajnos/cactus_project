using Antlr4.Runtime;
using CactusLang.Model;
using CactusLang.Model.Errors;

namespace CactusLang.Semantics.Errors;

public class CompError {
    public string Title { get; private set; }
    public string Msg { get; private set; }
    
    public int Code => Error.Code;
    public CctsError Error { get;  }

    public CodePos Start { get; private set; }
    public CodePos End { get; private set; }

    public IToken StartToken { get; private set; }
    public IToken EndToken { get; private set; }

    public CompError(CctsError error, IToken startToken, IToken endToken, params object[] errParams) {
        Error = error;
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

        Start = new CodePos("TEMP", startToken.Line, startToken.Column);
        int endTokenLength = endToken.Text.Length;

        End = new CodePos("TEMP", endToken.Line, endToken.Column + endTokenLength);
    }

    public string AsPrettyString() => $"@{Start}: [{Code}]\t{Title}\n" +
                                      $"\t{Msg}";
}