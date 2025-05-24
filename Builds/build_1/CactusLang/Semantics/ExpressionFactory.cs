using System.Numerics;
using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;
using CactusLang.Model.Errors;
using CactusLang.Model.Operators;
using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Semantics.Errors;
using CactusLang.Semantics.Types;
using CactusLang.Util;
using GP = GrammarParser;

namespace CactusLang.Semantics;

//*
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

    public Expression EvaluateExpression() {
        return EvaluateExpressionCtx(_startExpression);
    }

    private Expression EvaluateExpressionCtx(GP.ExpressionContext ctx) {
        //Primary Expression
        if (ctx.primaryExp() != null) {
            return GetPrimaryExp(ctx.primaryExp());
        }

        //If not, then evaluate left and right expression
        if (ctx.expression() == null)
            throw new ImpossibleException();

        var operands = ctx.expression();

        var leftVar = operands[0];
        var rightVar = operands[1];

        Expression leftExp = EvaluateExpressionCtx(leftVar);
        Expression rightExp = EvaluateExpressionCtx(rightVar);

        string operatorStr = "";

        //Get operator
        operatorStr = GetOperator(ctx);

        var nodeOperator = Operator.GetById(operatorStr);

        //Handle assignment differently
        if (nodeOperator.Level == Operator.OperatorLvl.Assignment)
            return HandleAssignment(ctx, leftExp, nodeOperator, rightExp);

        return new OperationExpression(leftExp, nodeOperator, rightExp);
    }

    private Expression HandleAssignment(GP.ExpressionContext ctx, Expression leftExp, Operator nodeOperator,
        Expression rightExp) {
        if (rightExp.GetResultType().CanBeUsedAs(leftExp.GetResultType())) {
            if (!leftExp.IsLValue())
                _errorHandler.ErrorInExpression(CctsError.RVALUE_ASG, ctx, ctx.expression()[0].GetText());

            return new OperationExpression(leftExp, nodeOperator, rightExp);
        }

        return _errorHandler.ErrorInExpression(CctsError.TYPE_MISMATCH, ctx, rightExp.GetResultType(),
            leftExp.GetResultType());
    }

    #region Primary Expression

    private Expression GetPrimaryExp(GP.PrimaryExpContext ctx) {
        if (ctx.primaryExpVal() != null) {
            var valType = GetPrimaryExpVal(ctx.primaryExpVal());
            //TODO Check unary operators...

            return valType;
        }

        if (ctx.objFuncCall() != null) {
            return GetPrimaryExp_ObjFuncCall(ctx);
        }

        if (ctx.objFieldRef() != null) {
            return GetPrimaryExp_ObjFieldRef(ctx);
        }

        //if (ctx.alloc() != null)
        //    return GetAlloc(ctx.alloc());

        return new Expression.Error();
    }

    private Expression GetPrimaryExp_ObjFuncCall(GP.PrimaryExpContext ctx) {
        var expression = GetPrimaryExp(ctx.primaryExp());
        if (expression is not PrimaryExpression primaryExpr)
            return new Expression.Error();

        var funcCallCtx = ctx.objFuncCall();
        var refOp = GetRefOp(funcCallCtx.accOp());

        FileStruct? refObj = GetRefObj(expression.GetResultType(), refOp);
        if (refObj == null)
            return _errorHandler.ErrorInExpression(CctsError.BAD_OBJ_REF,ctx, ctx.GetText());

        var funcParams = GetFuncParams(funcCallCtx.funcCall());
        FuncId instFuncId = GetFuncId(funcCallCtx.funcCall().funcRef().GetText(), funcParams);

        var structFunc = refObj.Functions.GetMatchingFunction(instFuncId);

        if (structFunc == null) {
            return _errorHandler.ErrorInExpression(CctsError.ID_NOT_FOUND,funcCallCtx.funcCall(),
                funcCallCtx.funcCall().GetText());
        }


        return new ObjectFuncCallExp(primaryExpr, funcParams, new ObjectFuncCall(refObj, structFunc));
    }

    //TODO 
    //namármost ez Pointer legyen, vagy field?
    private Expression GetPrimaryExp_ObjFieldRef(GP.PrimaryExpContext ctx) {
        var expression = GetPrimaryExp(ctx.primaryExp());
        if (expression is not PrimaryExpression primaryExpr)
            return new Expression.Error();

        var fieldRefCtx = ctx.objFieldRef();
        var refOp = GetRefOp(fieldRefCtx.accOp());

        FileStruct? refObj = GetRefObj(expression.GetResultType(), refOp);
        if (refObj == null)
            return _errorHandler.ErrorInExpression(CctsError.BAD_OBJ_REF,ctx, ctx.GetText());


        string fieldName = fieldRefCtx.fieldRef().GetText();
        StructField? field = refObj.Fields.GetField(fieldName);

        if (field == null) {
            return _errorHandler.ErrorInExpression(CctsError.ID_NOT_FOUND,fieldRefCtx.fieldRef(), fieldName);
        }

        return new ObjectFieldRefExp(primaryExpr, new ObjectFieldRef(refObj, field));
    }


    #region PrimaryExpVal

    private Expression GetPrimaryExpVal(GP.PrimaryExpValContext ctx) {
        if (ctx.parenthsExp() != null)
            return GetParnenthsExp(ctx.parenthsExp());

        if (ctx.funcCall() != null)
            return GetFuncCall(ctx.funcCall());

        if (ctx.varRef() != null)
            return GetVarRef(ctx.varRef());

        if (ctx.literalExp() != null)
            return GetLiteralExp(ctx.literalExp());

        //TODO 
        //ppc funcCall

        return new Expression.Error();
    }

    private Expression GetParnenthsExp(GP.ParenthsExpContext ctx) {
        return new ParenthsExp(EvaluateExpressionCtx(ctx.expression()));
    }

    private Expression GetFuncCall(GP.FuncCallContext ctx) {
        var paramEpxs = GetFuncParams(ctx);
        FuncId funcId = GetFuncId(ctx.funcRef().GetText(), paramEpxs);

        ModelFunction? func = _scope.GetMatchingFunction(funcId);
        if (func == null) {
            return _errorHandler.ErrorInExpression(CctsError.ID_NOT_FOUND,ctx, funcId.ToString());
        }

        return new FuncCallExp(func, paramEpxs);
    }

    //TODO eldönteni: ez most pointer legyen van sima
    private Expression GetVarRef(GP.VarRefContext varRef) {
        string varName = varRef.GetText();
        var symbol = _scope.GetVariable(varName);

        if (symbol != null) {
            if (symbol is StructFieldRefSymbol structFieldRefSymbol)
                return new LocalVarRef(structFieldRefSymbol);

            return new VarRef(symbol);
        }

        return new Expression.Error();
    }

    private Expression GetLiteralExp(GP.LiteralExpContext literalCtx) {
        string literalTxt = literalCtx.GetText();

        //Get literals
        //TODO null

        if (literalCtx.numLiteral() != null) {
            //Float conversion
            var floatCtx = literalCtx.numLiteral().floatLiteral();
            if (floatCtx != null) {
                var result = FloatParser.Parse(floatCtx.GetText());

                if (result.Type == FloatType.SINGLE)
                    return new FloatLiteral(PrimitiveType.F32,floatCtx.GetText());

                if (result.Type == FloatType.DOUBLE)
                    return new FloatLiteral(PrimitiveType.F64,floatCtx.GetText());

                return _errorHandler.ErrorInExpression(CctsError.LITERAL_ERROR,literalCtx, literalTxt);
            }

            //Integer
            var intCtx = literalCtx.numLiteral().intLiteral();
            if (intCtx != null) {
                try {
                    BigInteger? value = IntegerParser.Parse(intCtx.GetText());
                    if (value == null)
                        return _errorHandler.ErrorInExpression(
                            CctsError.LITERAL_ERROR,literalCtx, literalTxt);

                    return new IntLiteral(new LiteralIntegerType(value.Value));
                }
                catch (FormatException) {
                    return _errorHandler.ErrorInExpression(CctsError.LITERAL_ERROR,literalCtx, literalTxt);
                }
            }
        }

        //TODO str

        if (literalCtx.charLiteral() != null) {
            try {
                string val = literalCtx.charLiteral().GetText();
                return new CharLiteral(PrimitiveType.CH08,literalCtx.charLiteral().GetText());
            }
            catch (FormatException) {
                return _errorHandler.ErrorInExpression(CctsError.LITERAL_ERROR,literalCtx, literalTxt);
            }
        }

        if (literalCtx.boolLiteral() != null) {
            return new BoolLiteral(literalCtx.boolLiteral().GetText());
        }

        if (literalCtx.strLiteral() != null) {
            return new StringLiteral(literalCtx.strLiteral().GetText());
        }
        //if (literalCtx.varRef() != null) { }

        return new Expression.Error();
    }

    #endregion

    /*private Expression GetAlloc(GP.AllocContext ctx) {
        BaseType allocType;
        if (ctx.type() != null) {
            allocType = _typeSystem.Get(ctx.type());
        }
        else if (ctx.expression() != null) {
            allocType = EvaluateExpression(ctx.expression());
        }
        else {
            return _errorHandler.ErrorInExpression(CctsError.TODO_ERROR.CompTime(ctx));
        }

        return allocType.GetPointer();
    }*/

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
        else if (ctx.opAssignmentLvl() != null)
            operatorStr = ctx.opAssignmentLvl().GetText();


        return operatorStr;
    }


    private List<Expression> GetFuncParams(GP.FuncCallContext ctx) {
        List<Expression> paramExps = new List<Expression>();

        if (ctx.funcParamVals() != null) {
            foreach (var exp in ctx.funcParamVals().expression()) {
                paramExps.Add(EvaluateExpressionCtx(exp));
            }
        }

        return paramExps;
    }

    private FuncId GetFuncId(string funcName, List<Expression> paramExps) {
        List<BaseType> paramTypes = new List<BaseType>();

        foreach (var exp in paramExps) {
            paramTypes.Add(exp.GetResultType());
        }


        return new FuncId(funcName, paramTypes);
    }


    private ReferenceOperatorType GetRefOp(GP.AccOpContext ctx) {
        string txt = ctx.GetText();
        if (txt == ".")
            return ReferenceOperatorType.REFERENCE;
        return ReferenceOperatorType.ADDRESS;
    }

    private FileStruct? GetRefObj(BaseType objType, ReferenceOperatorType refOp) {
        if (refOp == ReferenceOperatorType.REFERENCE) {
            if (objType is not PointerType) {
                return null;
            }

            var pointer = (PointerType)objType;

            if (pointer.PointsTo is not FileStruct.Type) {
                return null;
            }

            return ((FileStruct.Type)pointer.PointsTo).FileStruct;
        }

        if (objType is not FileStruct.Type) {
            return null;
        }

        return ((FileStruct.Type)objType).FileStruct;
    }

    #endregion
} //*/