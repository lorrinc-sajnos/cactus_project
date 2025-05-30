using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Model.CodeStructure.Expressions;

namespace CactusLang.Model.CodeStructure.Statements;

public class ForLoop : Statement {
    public ForLoop(VarDclStatement loopDcl, Expression condition, Expression iter, CodeBlock parent) : base(parent) {
        _loopDcl = loopDcl;
        _condition = condition;
        _iter = iter;
        _loopBody = new CodeBlock(parent.Function);
    }

    private VarDclStatement _loopDcl;
    public VarDclStatement LoopDcl => _loopDcl;
    private Expression _condition;
    public Expression Condition => _condition;
    private Expression _iter;
    public Expression EndStatement => _iter;
    
    private CodeBlock _loopBody;
    public CodeBlock LoopBody => _loopBody;
}