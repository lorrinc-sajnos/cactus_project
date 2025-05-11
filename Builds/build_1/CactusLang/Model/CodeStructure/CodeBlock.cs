using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.Scopes;

namespace CactusLang.Model.CodeStructure;

public class CodeBlock {
    private Scope _scope;
    private List<Statement> _statements;
    
    public Scope Scope => _scope;
    public List<Statement> Statements => _statements;
}