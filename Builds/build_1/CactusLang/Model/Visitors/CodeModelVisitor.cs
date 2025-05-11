using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Operators;

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

    private CodeFile _file;

    CodeModelVisitor(CodeFile file) {
        _file = file;
    }

    public VisitStatus Start() {
        return VisitCodeFile(_file);
    }

    protected virtual VisitStatus VisitCodeFile(CodeFile file) {
        var strResult = VisitFileStructs(file.GetStructs());
        var varResult = VisitFileVariables(file.GetVariables());
        var funcResult = VisitFileFunctions(file.GetFunctions());

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

        foreach (var field in fileStruct.GetFields()) {
            var result = VisitStructField(field);
            results.Add(result);
        }

        foreach (var func in fileStruct.GetFunctions()) {
            var result = VisitStructFunction(func);
            results.Add(result);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitStructField(StructField structField) {
        //Nothing to visit!
        return VisitStatus.SUCCESS;
    }

    protected virtual VisitStatus VisitStructFunction(StructFunction function) {
        return VisitCodeBlock(function.CodeBody);
    }

    #endregion

    #region File Variables

    protected virtual VisitStatus VisitFileVariables(List<FileVariable> fileVariables) {
        var results = new List<VisitStatus>();
        foreach (var fileVariable in fileVariables) {
            var result = VisitFileVariable(fileVariable);
        }

        return GetResult(results);
    }

    protected virtual VisitStatus VisitFileVariable(FileVariable fileVariable) {
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
        //TODO
        throw new NotImplementedException();
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
    
    #endregion
}