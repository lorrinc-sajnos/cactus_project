using System.Numerics;
using CactusLang.Semantics.Scopes;
using CactusLang.Semantics.Types;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class ExpressionValidator {
    private readonly ErrorHandler _errorHandler;
    private readonly TypeSystem _typeSystem;
    private readonly Scope _scope;
    private readonly GrammarParser.ExpressionNodeContext _expNode;

    public ExpressionValidator(ErrorHandler errorHandler, TypeSystem typeSystem, Scope scope, GP.ExpressionNodeContext expNode) {
        _errorHandler = errorHandler;
        _typeSystem = typeSystem;
        _scope = scope;
        _expNode = expNode;
    }

    public BaseType Evaluate() {
        return RecEval(_expNode);
    }

    private BaseType RecEval(GP.ExpressionNodeContext ctx) {
        if (ctx.primaryExp() != null) {
            var primaryExpVal = ctx.primaryExp().primaryExpVal().literalExp();
            GetLiteralType (primaryExpVal) ;
        }
        return ErrorType.ERROR;
    }

    private BaseType GetLiteralType(GP.LiteralExpContext literalCtx) {
        //Get literals
        //TODO null
        
        if (literalCtx.numLiteral() != null) {
            //Float conversion
            var floatCtx = literalCtx.numLiteral().floatLiteral();
            if (floatCtx != null) {
                try {
                    double value = Convert.ToDouble(floatCtx.GetText());
                    return PrimitiveType.F64;
                }
                catch (FormatException) {
                    _errorHandler.AddError(new CompError(CctsError.LITERAL_ERROR, literalCtx.Start,literalCtx.Stop));
                    return ErrorType.ERROR;
                }
            }
            //Integer
            var intCtx = literalCtx.numLiteral().intLiteral();
            if (intCtx != null) {
                try {
                    BigInteger value = BigInteger.Parse(intCtx.GetText());
                    
                    //8-bit
                    if (value >= byte.MinValue && value <= byte.MaxValue)
                        return PrimitiveType.UI08;
                    if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
                        return PrimitiveType.UI08;
                    //16-bit
                    if  (value >= ushort.MinValue && value <= ushort.MaxValue)
                        return PrimitiveType.UI16;
                    if  (value >= short.MinValue && value <= short.MaxValue)
                        return PrimitiveType.I16;
                    //32-bit
                    if   (value >= uint.MinValue && value <= uint.MaxValue)
                        return PrimitiveType.UI32;
                    if   (value >= int.MinValue && value <= int.MaxValue)
                        return PrimitiveType.I32;
                    //64-bit
                    if   (value >= ulong.MinValue && value <= ulong.MaxValue)
                        return PrimitiveType.UI64;
                    if   (value >= long.MinValue && value <= long.MaxValue)
                        return PrimitiveType.I64;
                }
                catch (FormatException) {
                    _errorHandler.AddError(new CompError(CctsError.LITERAL_ERROR, literalCtx.Start,literalCtx.Stop));
                    return ErrorType.ERROR;
                }
            }
        }

        //TODO str
        
        if (literalCtx.charLiteral() != null) {
            try {
                string val = literalCtx.charLiteral().GetText();
                return PrimitiveType.CH08;
            }
            catch (FormatException) {
                _errorHandler.AddError(new CompError(CctsError.LITERAL_ERROR, literalCtx.Start,literalCtx.Stop));
                return ErrorType.ERROR;
            }
        }

        if (literalCtx.boolLiteral() != null) {
            return PrimitiveType.BOOL;
        }

        if (literalCtx.varRef() != null) {
            
        }
        
        return  ErrorType.ERROR;
    }
}