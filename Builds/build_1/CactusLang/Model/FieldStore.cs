using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model;

public class FieldStore<T> where T : ModelField {
    private readonly OrderedDictionary<string, T> _fields;

    public FieldStore() {
        _fields = new OrderedDictionary<string, T>();
    }

    public T GetField(string name) => _fields[name];

    public void AddField(T field) => _fields.Add(field.Name, field);

    public List<T> GetFields() => _fields.Values.ToList();
}