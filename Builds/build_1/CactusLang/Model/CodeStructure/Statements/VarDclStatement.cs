using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Statements;

public class VarDclStatement : Statement {
    public VarDclStatement(CodeBlock codeBlock) : base(codeBlock) { }
    public BaseType VarType => _variables.First().Variable.Type;

    private List<Body> _variables = new();
    
    public void AddBody(Body body) => _variables.Add(body);
    
    public List<Body> GetBodies() => _variables;

    public class Body {
        VariableSymbol _variable;
        public VariableSymbol Variable => _variable;
        
        Expression? _expression;
        public bool HasValue => _expression != null;
        public Expression? Value => _expression;

        public Body(VariableSymbol variable, Expression expression) {
            _variable = variable;
            _expression = expression;
        }
        public Body(VariableSymbol variable) :this(variable, null) { }
    }
}