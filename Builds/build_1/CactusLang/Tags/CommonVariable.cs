using CactusLang.Tags.StatementTags;

namespace CactusLang.Tags;

public class CommonVariable {
    public string Name { get; private set; }
    public object Value { get; set; }
    public Tag Parent { get; private set; }

    public CommonVariable(string name, object value, Tag parent) {
        Name = name;
        Value = value;
        Parent = parent;
    }
}