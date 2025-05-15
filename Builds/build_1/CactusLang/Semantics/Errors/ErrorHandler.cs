using Antlr4.Runtime;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Errors;
using CactusLang.Model.Types;

namespace CactusLang.Semantics.Errors;

public class ErrorHandler {
    private bool _throwOnError;
    
    public class ParserErrorListener : BaseErrorListener {
        private ErrorHandler _errorHandler;

        public ParserErrorListener(ErrorHandler errorHandler) {
            _errorHandler = errorHandler;
        }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine,
            string msg, RecognitionException e) {
            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);
            
            _errorHandler.AddError(CctsError.SYNTAX_WRONG_TOKEN.CompTime(offendingSymbol,offendingSymbol,offendingSymbol.Text));
        }
    }
    
    public ParserErrorListener ErrorListener { get; private set; }
    private readonly List<CompError> _errors;
    
    public ErrorHandler(bool throwOnError = false) {
        _errors = new List<CompError>();
        ErrorListener = new(this);
        _throwOnError =  throwOnError;
    }

    public void AddError(CompError error) {
        _errors.Add(error);
        if (_throwOnError) 
            throw new Exception($"Error added:\n{error.AsPrettyString()}");
    }

    public Expression.Error ErrorInExpression(CompError error) {
        AddError(error);
        return new Expression.Error();
    }
    
    public ErrorType ErrorInType(CompError error) {
        AddError(error);
        return ErrorType.ERROR;
    }
    
    public  List<CompError> GetErrors() => _errors;
    
    
    public void PrintErrors() {
        if (_errors.Count == 0) {
            Console.WriteLine("No errors found :)");
            return;
        }
        
        foreach (var error in _errors) {
            Console.WriteLine(error.AsPrettyString());
        }
    }
}