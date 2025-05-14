using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure.File;

public class CodeFile  {
    private FileScope _scope;
    
    FieldStore<FileField> _fields;
    public FieldStore<FileField> Fields => _fields;
    FunctionStore<FileFunction> _functions;
    public FunctionStore<FileFunction> Functions => _functions;
    OrderedDictionary<string, FileStruct> _structs;

    public CodeFile(FileScope scope) {
        _scope = scope;
        _fields = new ();
        _functions = new ();
        _structs = new OrderedDictionary<string, FileStruct>();
    }

    public Scope Scope => _scope;
    
    public List<FileStruct> GetStructs() => _structs.Values.ToList();
    
}