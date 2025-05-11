using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Statements;

public class VarDclStatement : Statement {
    public BaseType VarType { get; private set; }

    private List<Body> _variables;
    
    public List<Body> GetBodies() => _variables;

    public class Body {
        VariableSymbol _variable;
        public VariableSymbol Variable => _variable;
        
        Expression? _expression;
        public bool HasValue => _expression != null;
        public Expression? Value => _expression;
    }
}