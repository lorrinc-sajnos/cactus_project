using System.Xml;
using Antlr4.Runtime;

namespace CactusLang.Semantics;

public class CctsError {
    public int Code { get; private set; }
    public string Title { get; private set; }
    public string Msg { get; private set; }
    public Exception Exception { get; private set; }

    private readonly List<CctsError> errors = new();
    
    private CctsError(int code, string title, string msg) {
        Code = code;
        Title = title;
        Msg = msg;
        Exception = new Exception(msg);
        
        if (errors.Any(e => e.Code == code)) {
            var err = errors.First(e => e.Code == code);
            throw new Exception($"Two errors with the code {code}:"
                                + $"\n{Title}:\n{Msg}"
                                +  $"\n{err.Title}:\n{err.Msg}"
            );
        }
        errors.Add(this);
    }

    public CompError CompTime(ParserRuleContext ctx) => new CompError(this, ctx.Start, ctx.Stop);
    public CompError CompTime(IToken start, IToken end) => new CompError(this, start, end);
    
    public static readonly CctsError ID_NOT_FOUND = new(1, "ID Not Found", "Could not find ID");
    public static readonly CctsError ALREADY_DEFINED = new(2, "ID Already defined", "Already defined");
    public static readonly CctsError OP_MISMATCH = new(3, "Operator type mismatch", "No operator exists.");
    public static readonly CctsError LITERAL_ERROR = new(4, "Malformed literal", "The literal's value could not be parsed.");
    public static readonly CctsError TYPE_NOT_FOUND = new(5, "Type Not Found", "Could not find type.");
}