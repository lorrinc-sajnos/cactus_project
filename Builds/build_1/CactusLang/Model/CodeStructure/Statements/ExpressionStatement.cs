using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class ExpressionStatement : Statement {
    private Expression _expression;
    
    public  Expression Expression => _expression;
}