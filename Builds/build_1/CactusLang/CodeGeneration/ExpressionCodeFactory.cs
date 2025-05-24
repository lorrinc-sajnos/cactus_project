using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;
using CactusLang.Model.Visitors;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public class ExpressionCodeFactory : CodeModelVisitor<string> {
    private Expression _expression;
    private ModelGenerator.State _modelState;

    public static string Manufacture(Expression expression, ModelGenerator.State state) {
        ExpressionCodeFactory factory = new(expression, state);
        return factory.Start();
    }

    public ExpressionCodeFactory(Expression expression, ModelGenerator.State state) : base("", null) {
        _modelState = state;
        _expression = expression;
    }

    public string Start() {
        return VisitExpression(_expression);
    }

    protected override string AggregateResults(params string[] statuses) {
        string result = "";
        foreach (var str in statuses) {
            result += str ?? throw new Exception("Bad implementation in Visitor!!!!");
        }

        return result;
    }

    protected override string VisitOperationExpression(OperationExpression operationExpression) {
        var lhExp = operationExpression.LeftExpression;
        var rhExp = operationExpression.RightExpression;

        var op = operationExpression.Operator;

        string lhStr = VisitExpression(lhExp);
        string rhStr = VisitExpression(rhExp);

        return $"{lhStr} {op.GetId()} {rhStr}";
    }


    #region Primaray exception

    protected override string VisitVarRefExpression(VarRef varRef) => varRef.VarName;


    protected override string VisitFuncCallExpression(FuncCallExp funcCall) {
        string call = funcCall.Function.Name + '(';

        var paramExps = funcCall.GetParameters();
        if (paramExps.Any()) {
            for (int i = 0; i < paramExps.Count() - 1; i++)
                call += VisitExpression(paramExps[i]) + ", ";

            call += VisitExpression(paramExps[^1]);
        }

        return call + ')';
    }

    protected override string VisitLocalVarRefExpression(LocalVarRef structFieldRef) =>
        $"this->{structFieldRef.VarName}";

    protected override string VisitLiteralExpression(LiteralExpression literalExpression) {
        return literalExpression.Value;
    }

    protected override string VisitObjectFieldRefExp(ObjectFieldRefExp objectFieldRefExp) {
        string structPtr = VisitExpression(objectFieldRefExp.Object);
        string fieldName = objectFieldRefExp.FieldRef.Field.Name;
        return $"{structPtr}->{fieldName}";
    }

    protected override string VisitObjectFuncCallExp(ObjectFuncCallExp objectFuncCallExp) {
        string structPtr = VisitExpression(objectFuncCallExp.Object);
        string funcName = CodeGenUtil.GetStructRefFuncName(objectFuncCallExp.FuncCall.Function);
        string otherParamStr = "";
        
        foreach (var param in objectFuncCallExp.GetParameters()) {
            var factory = new ExpressionCodeFactory(param, _modelState);
            otherParamStr += CodeGenUtil.PARAM_SEP + factory.Start();
        }

        return $"{funcName}({structPtr}{otherParamStr})";
    }

    #endregion
}