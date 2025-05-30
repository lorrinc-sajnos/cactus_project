using CactusLang.Model.CodeStructure.CodeBlocks;
using CactusLang.Tags;
using CactusLang.Tags.StatementTags;

namespace CactusLang.Model.CodeStructure.Statements;

public abstract class Statement {
    private readonly CodeBlock _parent;
    public CodeBlock CodeBlock => _parent;
    
    public TagContainer TagContainer { get; private set; }

    public Statement(CodeBlock parent) {
        _parent = parent;
        TagContainer = new TagContainer();
    }
}