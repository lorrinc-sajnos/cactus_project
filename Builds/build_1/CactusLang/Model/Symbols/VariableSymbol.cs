using CactusLang.Model.Types;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Model.Symbols;

public class VariableSymbol {
    public TagContainer Tags { get; private set; }
    public string Name { get; private set; }
    public string Id { get; private set; }
    public BaseType Type { get; private set; }

    //public VariableSymbol(BaseType type, string name) : this(type, 0, name) { }

    public VariableSymbol(VariableSymbol copy) {
        Tags = copy.Tags;
        Name = copy.Name;
        Id = copy.Id;
        Type = copy.Type;
    }
    public VariableSymbol(BaseType type, string name) {
        Tags = new TagContainer();
        Name = name;
        Type = type;
        Id = new(name);
    }
}