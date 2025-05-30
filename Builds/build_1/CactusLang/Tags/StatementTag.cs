using System.Diagnostics;
using CactusLang.CodeGeneration;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Statements;

namespace CactusLang.Tags.StatementTags;

public abstract class Tag {
    public static string StaticID { get; }
    public abstract string Id { get; }
    
    
    //...
    public virtual void OnDeclared(CodeFile file) { }
    //...
    public virtual void OnStatement(Statement statement, CodeGenerator codeGenerator) { }
    //...
    public virtual void OnReturnStatement(ReturnStatement statement, CodeGenerator codeGenerator) { }
    //...
}