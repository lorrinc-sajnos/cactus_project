using Antlr4.Runtime;
using CactusLang.Semantics.Errors;

namespace CactusLang.Model.Errors;

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
                                + $"\n{err.Title}:\n{err.Msg}"
            );
        }

        errors.Add(this);
    }

    public CompError CompTime(ErrorHandler errorHandler, ParserRuleContext ctx, params object[] msgParams) =>
        new(this, errorHandler, ctx.Start, ctx.Stop, msgParams);

    public CompError CompTime(ErrorHandler errorHandler, IToken token, params object[] msgParams) =>
        new(this, errorHandler, token, token, msgParams);

    public CompError CompTime(ErrorHandler errorHandler, IToken start, IToken end, params object[] msgParams) =>
        new(this, errorHandler, start, end, msgParams);

    public static readonly CctsError TODO_ERROR = new(0, "Todo Error", "Todo Error ");
    public static readonly CctsError ID_NOT_FOUND = new(1, "ID \"{0}\" Not Found", "Could not find ID \"{0}\".");

    public static readonly CctsError ALREADY_DEFINED =
        new(2, "ID \"{0}\" already defined", "\"{0}\" is already defined");

    public static readonly CctsError OP_MISMATCH = new(3, "Operator type mismatch", "No operator exists.");

    public static readonly CctsError LITERAL_ERROR =
        new(4, "Malformed literal #{0}#", "The value of \"{0}\" could not be parsed.");

    public static readonly CctsError TYPE_NOT_FOUND = new(5, "Type {0} Not Found", "Could not find type {0}.");

    public static readonly CctsError OVERLOAD_DOESNT_MATCH_RET_TYPE = new(6,
        "Overload of {0} doesn't match return type \"{1}\"",
        "Overload of function \"{0}\" return type has to be \"{1}\"");

    public static readonly CctsError BAD_OBJ_REF =
        new(7, "Invalid referrence: {0}", "Object reference {0} is not valid.");

    public static readonly CctsError TYPE_MISMATCH =
        new(8, "Cannot assign type {0} =/> {1}", "Cannot assign type {0} to {1}.");

    public static readonly CctsError RVALUE_ASG = new(9, "Cannot assign to rvalue #{0}#",
        "Cannot assign value into #{0}#, as it is an rvalue, thus not acessable.");


    public static readonly CctsError SYNTAX_WRONG_TOKEN =
        new(9001, "Syntax error: Unexpected token \"{0}\"", "Token \"{0}\" was unexpected.");
}