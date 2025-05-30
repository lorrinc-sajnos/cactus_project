using Antlr4.Runtime;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Errors;
using CactusLang.Model.Types;

namespace CactusLang.Semantics.Errors;

public class ErrorHandler {
    private const bool THROW_ON_ERROR = false;

    public class ParserErrorListener : BaseErrorListener {
        private ErrorHandler _errorHandler;

        public ParserErrorListener(ErrorHandler errorHandler) {
            _errorHandler = errorHandler;
        }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
            int charPositionInLine,
            string msg, RecognitionException e) {
            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);

            _errorHandler.AddCompError(
                CctsError.SYNTAX_WRONG_TOKEN.CompTime(_errorHandler, offendingSymbol, offendingSymbol.Text));
        }
    }

    public ParserErrorListener ErrorListener { get; private set; }
    public string Filename { get; private set; }

    private readonly List<CompError> _errors;

    public ErrorHandler(string filename) {
        _errors = new List<CompError>();
        ErrorListener = new(this);
        //_throwOnError =  throwOnError;
        Filename = filename;
    }

    private void AddCompError(CompError error) {
        _errors.Add(error);
        if (THROW_ON_ERROR)
            throw new Exception($"Error added:\n{error.AsPrettyString()}");
    }

    
    public void Error(CctsError error, IToken token, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, token, msgParams));
    public void Error(CctsError error, IToken start, IToken end, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, start, end, msgParams));
    public void Error(CctsError error, ParserRuleContext ctx, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, ctx, msgParams));
    
    
    
    #region Expression
    public Expression.Error ErrorInExpression(CctsError error, IToken token, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, token, msgParams));
    public Expression.Error ErrorInExpression(CctsError error, IToken start, IToken end, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, start, end, msgParams));
    public Expression.Error ErrorInExpression(CctsError error, ParserRuleContext ctx, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, ctx, msgParams));
    private Expression.Error ErrorInExpressionFromCompTime(CompError compError) {
        AddCompError(compError);
        return new Expression.Error();
    }
    #endregion
    
    #region Type
    public ErrorType ErrorInType(CctsError error, IToken token, params object[] msgParams) =>
        ErrorInTypeFromCompTime(error.CompTime(this, token, msgParams));
    public ErrorType ErrorInType(CctsError error, IToken start, IToken end, params object[] msgParams) =>
        ErrorInTypeFromCompTime(error.CompTime(this, start, end, msgParams));
    public ErrorType ErrorInType(CctsError error, ParserRuleContext ctx, params object[] msgParams) =>
        ErrorInTypeFromCompTime(error.CompTime(this, ctx, msgParams));
    private ErrorType ErrorInTypeFromCompTime(CompError compError) {
        AddCompError(compError);
        return ErrorType.ERROR;
    }

    public List<CompError> GetErrors() => _errors;
    #endregion

    public void PrintErrors() {
        if (_errors.Count == 0) {
            Console.WriteLine("-\tNo errors found :)");
            return;
        }

        foreach (var error in _errors) {
            Console.WriteLine(error.AsPrettyString());
        }
    }
}