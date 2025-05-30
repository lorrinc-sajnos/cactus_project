using CactusLang.Model.Types;
using CactusLang.Semantics.Types;
using CactusLang.Tags;
using CactusLang.Tags.StatementTags;

namespace CactusLang.Model.Symbols;

public class VariableSymbol {
    public string Name { get; private set; }
    public string Id { get; private set; }
    public BaseType Type { get; private set; }

    //public VariableSymbol(BaseType type, string name) : this(type, 0, name) { }

    public VariableSymbol(VariableSymbol copy) {
        Name = copy.Name;
        Id = copy.Id;
        Type = copy.Type;
    }
    public VariableSymbol(BaseType type, string name) {
        Name = name;
        Type = type;
        Id = new(name);
    }
}