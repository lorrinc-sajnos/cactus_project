using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Operators;
using CactusLang.Model.Operators.ObjectRefference;
using ObjectFieldRef = CactusLang.Model.Operators.ObjectRefference.ObjectFieldRef;

namespace CactusLang.Model.Visitors;

public class CodeModelVisitor {
    public enum VisitStatus {
        SUCCESS,
        FAILURE,
        RERUN
    }

    private VisitStatus GetResult(params VisitStatus[] statuses) {
        foreach (var status in statuses) {
            if (status == VisitStatus.FAILURE)
                return VisitStatus.FAILURE;
        }

        return VisitStatus.SUCCESS;
    }

    private VisitStatus GetResult(List<VisitStatus> statuses) => GetResult(statuses.ToArray());

    private readonly CodeFile _file;

    public CodeModelVisitor(CodeFile file) {
        _file = file;
    }

    public VisitStatus Start() {
        return VisitCodeFile(_file);
    }

    protected virtual VisitStatus VisitCodeFile(CodeFile file) {
        var strResult = VisitFileStructs(file.GetStructs());
        var varResult = VisitFileFields(file.Fields.GetFields());
        var funcResult = VisitFileFunctions(file.Functions.GetFunctions());

        return GetResult(strResult, varResult, funcResult);
    }

    #region Structs

    protected virtual VisitStatus VisitFileStructs(List<FileStruct> fileStructs) {
        var results = new List<VisitStatus>();

        foreach (var fileStruct in fileStructs) {
            var strResult = VisitFileStruct(fileStruct);
            results.Add(strResult);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitFileStruct(FileStruct fileStruct) {
        var results = new List<VisitStatus>();

        foreach (var field in fileStruct.Fields.GetFields()) {
            var result = VisitStructField(field);
            results.Add(result);
        }

        foreach (var func in fileStruct.Functions.GetFunctions()) {
            var result = VisitStructFunction(func);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitStructField(StructField? structField) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitStructFunction(StructFunction function) {
        return VisitCodeBlock(function.CodeBody);
    }

    #endregion

    #region File Variables

    protected virtual VisitStatus VisitFileFields(List<FileField> fileVariables) {
        var results = new List<VisitStatus>();
        foreach (var fileVariable in fileVariables) {
            var result = VisitFileField(fileVariable);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitFileField(FileField fileField) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    #endregion

    #region Functions

    protected virtual VisitStatus VisitFileFunctions(List<FileFunction> fileFunctions) {
        var results = new List<VisitStatus>();

        foreach (var fileFunction in fileFunctions) {
            var result = VisitCodeBlock(fileFunction.CodeBody);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitCodeBlock(CodeBlock codeBlock) {
        var results = new List<VisitStatus>();
        foreach (var statement in codeBlock.Statements) {
            var result = VisitStatement(statement);
        }

        return GetResult(results);
    }

    #endregion

    #region Statements

    protected virtual VisitStatus VisitStatement(Statement statement) {
        switch (statement) {
            default:
                throw new NotImplementedException($"Statement {statement} is not implemented");
            //return VisitStatus.FAILURE;

            case ExpressionStatement expressionStatement:
                return VisitExpressionStatement(expressionStatement);

            case VarDclStatement varDclStatement:
                return VisitVarDclStatement(varDclStatement);

            case ReturnStatement returnStatement:
                return VisitReturnStatement(returnStatement);


            //TODO other statements
        }
    }

    protected virtual VisitStatus VisitReturnStatement(ReturnStatement returnStatement) {
        return VisitExpression(returnStatement.Expression);
    }

    protected virtual VisitStatus VisitVarDclStatement(VarDclStatement varDclStatement) {
        var results = new List<VisitStatus>();
        foreach (var body in varDclStatement.GetBodies()) {
            var result = VisitVarDclBody(body);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitVarDclBody(VarDclStatement.Body varBody) {
        if (varBody.HasValue)
            return VisitExpression(varBody.Value!);
        //else
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitExpressionStatement(ExpressionStatement expressionStatement) {
        return VisitExpression(expressionStatement.Expression);
    }

    #endregion

    #region Expressions

    protected virtual VisitStatus VisitExpression(Expression expression) {
        switch (expression) {
            default:
                throw new NotImplementedException($"Expression {expression} is not implemented");
            case OperationExpression operationExpression:
                return VisitOperationExpression(operationExpression);
            case PrimaryExpression primaryExpression:
                return VisitPrimaryExpression(primaryExpression);
        }
    }

    protected virtual VisitStatus VisitOperationExpression(OperationExpression operationExpression) {
        var leftResult = VisitExpression(operationExpression.LeftExpression);
        var operatorResult = VisitOperator(operationExpression.Operator);
        var rightResult = VisitExpression(operationExpression.RightExpression);
        return GetResult(leftResult, rightResult);
    }

    protected virtual VisitStatus VisitPrimaryExpression(PrimaryExpression primaryExpression) {
        VisitStatus primExpResult;
        VisitStatus opResult = VisitStatus.SUCCESS;

        if (primaryExpression.HasUnaryOperation &&
            primaryExpression.UnaryOperation!.OpSide == UnaryOp.Side.LEFT)
            opResult = VisitUnaryOperator(primaryExpression.UnaryOperation);

        switch (primaryExpression) {
            default:
                throw new NotImplementedException($"Primary expression {primaryExpression} is not implemented");
            case FuncCallExp funcCall:
                primExpResult = VisitFuncCallExpression(funcCall);
                break;
            case ParenthsExp parenthsExpression:
                primExpResult = VisitParenthsExpression(parenthsExpression);
                break;
            case VarRef varRef:
                primExpResult = VisitVarRefExpression(varRef);
                break;
            case LiteralExpression literalExpression:
                primExpResult = VisitLiteralExpression(literalExpression);
                break;
        }

        if (primaryExpression.HasUnaryOperation &&
            primaryExpression.UnaryOperation!.OpSide == UnaryOp.Side.RIGHT)
            opResult = VisitUnaryOperator(primaryExpression.UnaryOperation);

        return GetResult(primExpResult, opResult);
    }

    #region PrimaryExpression

    protected virtual VisitStatus VisitFuncCallExpression(FuncCallExp funcCall) {
        var results = new List<VisitStatus>();
        foreach (var parameter in funcCall.GetParameters()) {
            var result = VisitExpression(parameter);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitParenthsExpression(ParenthsExp parenthsExpression) {
        return VisitExpression(parenthsExpression.InnerExpression);
    }

    protected virtual VisitStatus VisitVarRefExpression(VarRef varRef) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitLiteralExpression(LiteralExpression literalExpression) {
        switch (literalExpression) {
            default:
                throw new NotImplementedException($"Literal expression {literalExpression} is not implemented");
            case FloatLiteral floatLiteral:
                return VisitFloatLiteralExpression(floatLiteral);
            case IntLiteral intLiteral:
                return VisitIntLiteralExpression(intLiteral);
        }
    }

    #region Literal Expression

    protected virtual VisitStatus VisitFloatLiteralExpression(FloatLiteral literalExpression) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitIntLiteralExpression(IntLiteral literalExpression) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    #endregion

    #endregion

    #endregion

    #region Operators

    #region Unary Operators

    protected virtual VisitStatus VisitUnaryOperator(UnaryOp op) {
        switch (op) {
            default:
                throw new NotImplementedException($"Operator {op} is not implemented");
            case AddressOperator addressOperator:
                return VisitAddressOperator(addressOperator);
            case BaseUnaryOp checkUnaryOp:
                return VisitBaseUnaryOp(checkUnaryOp);
            case DereferenceOperator dereferenceOperator:
                return VisitDereferenceOperator(dereferenceOperator);

            case ObjectFieldRef objectFieldRef:
                return VisitObjectFieldRef(objectFieldRef);
            case ObjectFuncCall objectFuncCall:
                return VisitObjectFuncCall(objectFuncCall);
        }
    }

    protected virtual VisitStatus VisitAddressOperator(AddressOperator addressOperator) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitBaseUnaryOp(BaseUnaryOp checkUnaryOp) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitDereferenceOperator(DereferenceOperator dereferenceOperator) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitObjectFieldRef(ObjectFieldRef objectFieldRef) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitObjectFuncCall(ObjectFuncCall objectFuncCall) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    #endregion

    protected VisitStatus VisitOperator(Operator op) {
        switch (op) {
            default:
                throw new NotImplementedException($"Operator {op} is not implemented");
            case BitShiftOperator bitShiftOperator:
                return VisitBitShiftOperator(bitShiftOperator);
            case CompOperator compOperator:
                return VisitCompOperator(compOperator);
            case IntOperator intOperator:
                return VisitIntOperator(intOperator);
            case MathOperator mathOperator:
                return VisitMathOperator(mathOperator);
        }
    }

    protected virtual VisitStatus VisitBitShiftOperator(BitShiftOperator op) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitCompOperator(CompOperator op) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitIntOperator(IntOperator op) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitMathOperator(MathOperator op) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    #endregion
}