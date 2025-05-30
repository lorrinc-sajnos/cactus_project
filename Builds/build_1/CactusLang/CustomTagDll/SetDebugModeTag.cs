using CactusLang.Model.CodeStructure.File;
using CactusLang.Tags;
using CactusLang.Tags.StatementTags;

namespace CactusLang.CustomTagDll;

public class SetDebugModeTag : Tag {
    public const string TagId = "debug_mode";
    public override string Id => TagId;

    public override void OnDeclared(CodeFile file) {
        TagContainer.Global.AddCommonVariable(new CommonVariable("DEBUG_MODE", true, this));
    }
}