using CactusLang.CodeGeneration;
using CactusLang.CodeGeneration.CCodeGenerator;
using CactusLang.Model;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Tags;
using CactusLang.Tags.StatementTags;

namespace CactusLang.CustomTagDll;

public class PrintDebugTag : Tag {
    public const string TagId = "debug_print";
    public override string Id => TagId;


    private int _returnC;

    public PrintDebugTag() {
        _returnC = 0;
    }

    public override void OnReturnStatement(ReturnStatement statement, CodeGenerator generator) {
        bool isDebugMode = TagContainer.Global.HasCommonVariable("DEBUG_MODE");
        if (!isDebugMode) return;

        ModelFunction func = statement.CodeBlock.Function;
        
        string exprStr = ExpressionCodeFactory.Manufacture(statement.Expression, generator.GenState);
        generator.Source.AddLine(
            $"printf(\"[DEBUG]: function {func.Name} returned with:%i\\n\", ({exprStr}));"
            );
    }
}