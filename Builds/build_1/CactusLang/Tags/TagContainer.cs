using CactusLang.Tags.StatementTags;

namespace CactusLang.Tags;

public class TagContainer {
    private List<Tag> _fileTags;
    private Dictionary<string, CommonVariable> _commonVariableStore;

    public void AddCommonVariable(CommonVariable variable) => _commonVariableStore.Add(variable.Name, variable);
    public bool HasCommonVariable(string name) => _commonVariableStore.ContainsKey(name);
    public CommonVariable? GetCommonVariable(string key) => _commonVariableStore[key];

    public TagContainer() {
        _fileTags = new List<Tag>();
        _commonVariableStore = new Dictionary<string, CommonVariable>();
        
    }

    public static TagContainer Global { get; } = new();

    public List<Tag> GetTags() => _fileTags;
    public void AddTag(Tag fileTag) => _fileTags.Add(fileTag);

    public void AddTags(List<Tag> tagList) {
        _fileTags.AddRange(tagList);
    }
}