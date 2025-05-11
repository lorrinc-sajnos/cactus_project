using CactusLang.Model.Scopes;
using CactusLang.Model.Types;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure;

public class FileStruct {
    private StructType _struct;
    private OrderedDictionary<string, StructField> _fields;
    private OrderedDictionary<string, StructFunction> _functions;
    
    public string Name => _struct.Name;
    public List<StructField> GetFields() => _fields.Values.ToList();
    public StructField GetField(string name) => _fields[name];
    
    public List<StructFunction> GetFunctions() => _functions.Values.ToList();
    public StructFunction GetFunction(string name) => _functions[name];
    
}