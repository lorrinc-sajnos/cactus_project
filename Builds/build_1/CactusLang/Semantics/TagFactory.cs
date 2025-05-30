using CactusLang.Tags;
using CactusLang.Tags.StatementTags;

namespace CactusLang.Semantics;

public class TagFactory {
    public TagFactory() { }

    public List<Tag> CreateTags(GrammarParser.TagsContext? tags) {
        List<Tag> tagList = new List<Tag>();
        if (tags == null) return tagList;

        foreach (var tagCtx in tags.tag()) {
            Tag? tag = TagSystem.CreateStatementFromString(tagCtx.GetText());
            if (tag == null) continue;
            
            tagList.Add(tag);
        }

        return tagList;
    }
}