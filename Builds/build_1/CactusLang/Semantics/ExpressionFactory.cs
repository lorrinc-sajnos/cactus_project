using System.Numerics;
using System.Reflection.Metadata;
using CactusLang.Model.Errors;
using CactusLang.Model.Operators;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics.Errors;
using CactusLang.Semantics.Types;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

public class ExpressionFactory {
    private readonly ErrorHandler _errorHandler;
    private readonly TypeSystem _typeSystem;
    private readonly Scope _scope;
    private readonly GrammarParser.ExpressionContext _startExpression;

    public ExpressionFactory(ErrorHandler errorHandler, TypeSystem typeSystem, Scope scope,
        GP.ExpressionContext expNode) {
        _errorHandler = errorHandler;
        _typeSystem = typeSystem;
        _scope = scope;
        _startExpression = expNode;
    }

    public BaseType EvaluateType() {
        return EvaluateExpression(_startExpression);
    }

    private BaseType EvaluateExpression(GP.ExpressionContext ctx) {
        //Primary Expression
        if (ctx.primaryExp() != null) {
            return GetPrimaryExpType(ctx.primaryExp());
        }

        //If not, then evaluate left and right expression
        if (ctx.expression() == null)
            throw new ImpossibleException();

        var operands = ctx.expression();

        var leftVar = operands[0];
        var rightVar = operands[1];

        BaseType leftType = EvaluateExpression(leftVar);
        BaseType rightType = EvaluateExpression(rightVar);

        string operatorStr = "";

        //Get operator
        operatorStr = GetOperator(ctx);

        var nodeOperator = Operator.GetById(operatorStr);

        BaseType resultType = nodeOperator.Evaluate(leftType, rightType);

        return resultType;
    }


    #region Primary Expression

    private BaseType GetPrimaryExpType(GP.PrimaryExpContext ctx) {
        if (ctx.primaryExpVal() != null) {
            var valType = GetPrimaryExpValType(ctx.primaryExpVal());
            //TODO Check unary operators...

            return valType;
        }

        if (ctx.objFuncCall() != null) {
            return GetPrimaryExpType_ObjFuncCall(ctx);
        }

        if (ctx.objFieldRef() != null) {
            return GetPrimaryExpType_ObjFieldRef(ctx);
        }
        

        if (ctx.assignment() != null)
            return GetAssignmentType(ctx.assignment());

        if (ctx.alloc() != null)
            return GetAllocType(ctx.alloc());

        return ErrorType.ERROR;
    }

    private BaseType GetPrimaryExpType_ObjFuncCall(GP.PrimaryExpContext ctx) {
        var objType = GetPrimaryExpType(ctx.primaryExp());
        var funcCallCtx =  ctx.objFuncCall();
        var refOp = GetRefOp(funcCallCtx.accOp());
        
        StructType? refType = GetRefObjType(objType, refOp);
        if (refType == null)
            return _errorHandler.PostError(CctsError.BAD_OBJ_REF.CompTime(ctx, ctx.GetText()));

        FuncId instFuncId = GetFuncId(funcCallCtx.funcCall());
        var funcSymb = refType.GetMatchingFunction(instFuncId);

        if (funcSymb == null) {
            return _errorHandler.PostError(CctsError.ID_NOT_FOUND.CompTime(funcCallCtx.funcCall(), funcCallCtx.funcCall().GetText()));
        }

        return funcSymb.ReturnType;
    }

    //TODO 
    //namármost ez Pointer legyen, vagy field?
    private BaseType GetPrimaryExpType_ObjFieldRef(GP.PrimaryExpContext ctx) {
        var objType = GetPrimaryExpType(ctx.primaryExp());
        var fieldRefCtx =  ctx.objFieldRef();
        var refOp = GetRefOp(fieldRefCtx.accOp());

        StructType? refType = GetRefObjType(objType, refOp);
        if (refType == null)
            return _errorHandler.PostError(CctsError.BAD_OBJ_REF.CompTime(ctx, ctx.GetText()));


        string fieldName = fieldRefCtx.fieldRef().GetText();
        VariableSymbol? fieldSymb = refType.GetField(fieldName);
        
        if (fieldSymb == null) {
            return _errorHandler.PostError(CctsError.ID_NOT_FOUND.CompTime(fieldRefCtx.fieldRef(), fieldName));
        }

        return fieldSymb.Type;
    }


    #region PrimaryExpVal

    private BaseType GetPrimaryExpValType(GP.PrimaryExpValContext ctx) {
        if (ctx.parenthsExp() != null)
            return GetParnenthsExpType(ctx.parenthsExp());

        if (ctx.funcCall() != null)
            return GetFuncCallType(ctx.funcCall());

        if (ctx.varRef() != null)
            return GetVarRefType(ctx.varRef());

        if (ctx.literalExp() != null)
            return GetLiteralExpType(ctx.literalExp());

        //TODO 
        //ppc funcCall

        return ErrorType.ERROR;
    }

    private BaseType GetParnenthsExpType(GP.ParenthsExpContext ctx) {
        return EvaluateExpression(ctx.expression());
    }
    private BaseType GetFuncCallType(GP.FuncCallContext ctx) {
        FuncId funcId = GetFuncId(ctx);
        FunctionSymbol? func = _scope.GetMatchingFunction(funcId);
        if (func == null) {
            return _errorHandler.PostError(CctsError.ID_NOT_FOUND.CompTime(ctx, funcId.ToString()));
        }

        return func.ReturnType;
    }

    //TODO eldönteni: ez most pointer legyen van sima
    private BaseType GetVarRefType(GP.VarRefContext varRef) {
        string varName = varRef.GetText();
        var symbol = _scope.GetVariable(varName);

        return symbol?.Type ?? ErrorType.ERROR;
    }

    private BaseType GetLiteralExpType(GP.LiteralExpContext literalCtx) {
        string literalTxt = literalCtx.GetText();

        //Get literals
        //TODO null

        if (literalCtx.numLiteral() != null) {
            //Float conversion
            var floatCtx = literalCtx.numLiteral().floatLiteral();
            if (floatCtx != null) {
                var result = FloatParser.Parse(floatCtx.GetText());

                if (result.Type == FloatType.SINGLE)
                    return PrimitiveType.F32;

                if (result.Type == FloatType.DOUBLE)
                    return PrimitiveType.F64;

                return _errorHandler.PostError(CctsError.LITERAL_ERROR.CompTime(literalCtx, literalTxt));
            }

            //Integer
            var intCtx = literalCtx.numLiteral().intLiteral();
            if (intCtx != null) {
                try {
                    BigInteger? value = IntegerParser.Parse(intCtx.GetText());
                    if (value == null)
                        return _errorHandler.PostError(CctsError.LITERAL_ERROR.CompTime(literalCtx, literalTxt));
                    
                    return new LiteralIntegerType(value.Value);
                }
                catch (FormatException) {
                    return _errorHandler.PostError(CctsError.LITERAL_ERROR.CompTime(literalCtx, literalTxt));
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
                return _errorHandler.PostError(CctsError.LITERAL_ERROR.CompTime(literalCtx, literalTxt));
            }
        }

        if (literalCtx.boolLiteral() != null) {
            return PrimitiveType.BOOL;
        }

        //if (literalCtx.varRef() != null) { }

        return ErrorType.ERROR;
    }

    #endregion

    private BaseType GetAssignmentType(GP.AssignmentContext ctx) {
        string varName = ctx.varRef().GetText();
        var varSymb = _scope.GetVariable(varName);
        if (varSymb == null)
            return _errorHandler.PostError(CctsError.ID_NOT_FOUND.CompTime(ctx, varName));

        BaseType newValue = EvaluateExpression(ctx.expression());

        if (newValue.CanBeUsedAs(varSymb.Type)) {
            return varSymb.Type;
        }

        return _errorHandler.PostError(CctsError.TYPE_MISMATCH.CompTime(ctx, newValue, varSymb.Type));
    }

    private BaseType GetAllocType(GP.AllocContext ctx) {
        BaseType allocType;
        if (ctx.type() != null) {
            allocType = _typeSystem.Get(ctx.type());
        }
        else if (ctx.expression() != null) {
            allocType = EvaluateExpression(ctx.expression());
        }
        else {
            return _errorHandler.PostError(CctsError.TODO_ERROR.CompTime(ctx));
        }

        return allocType.GetPointer();
    }

    #endregion

    #region Util

    private static string GetOperator(GP.ExpressionContext ctx) {
        string operatorStr = "";
        if (ctx.opMultLvl() != null)
            operatorStr = ctx.opMultLvl().GetText();
        else if (ctx.opAddLvl() != null)
            operatorStr = ctx.opAddLvl().GetText();
        else if (ctx.opBitLvl() != null)
            operatorStr = ctx.opBitLvl().GetText();
        else if (ctx.opCompLvl() != null)
            operatorStr = ctx.opCompLvl().GetText();
        return operatorStr;
    }

    private FuncId GetFuncId(GP.FuncCallContext ctx) {
        string funcName = ctx.funcRef().GetText();
        List<BaseType> paramTypes = new List<BaseType>();

        if (ctx.funcParamVals() != null) {
            foreach (var exp in ctx.funcParamVals().expression()) {
                paramTypes.Add(EvaluateExpression(exp));
            }
        }

        return new FuncId(funcName, paramTypes);
    }


    private ReferenceOperatorType GetRefOp(GP.AccOpContext ctx) {
        string txt = ctx.GetText();
        if (txt == ".")
            return ReferenceOperatorType.REFERENCE;
        return ReferenceOperatorType.ADDRESS;
    }
    
    private StructType? GetRefObjType(BaseType objType, ReferenceOperatorType refOp) {
        if (refOp == ReferenceOperatorType.REFERENCE) {
            if (objType is not PointerType) {
                return null;
            }

            var pointer = (PointerType)objType;

            if (pointer.PointsTo is not StructType) {
                return null;
            }

            return (StructType)pointer.PointsTo;
        }

        if (objType is not StructType) {
            return null;
        }

        return (StructType)objType;
    }

    #endregion
}