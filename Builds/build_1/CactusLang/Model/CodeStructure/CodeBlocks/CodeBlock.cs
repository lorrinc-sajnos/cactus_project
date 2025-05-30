using CactusLang.Model.CodeStructure.Statements;

namespace CactusLang.Model.CodeStructure.CodeBlocks;

public class CodeBlock {
    private List<Statement> _statements;
    public ModelFunction Function { get; private set; }
    public virtual List<Statement> Statements => _statements;

    public CodeBlock(ModelFunction parent) {
        _statements = new List<Statement>();
        Function = parent;
    }
    
    public virtual void AddStatement(Statement statement) => _statements.Add(statement);
}