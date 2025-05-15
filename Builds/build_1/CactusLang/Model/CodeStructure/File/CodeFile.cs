using CactusLang.Model.Codefiles;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure.File;

public class CodeFile {
    private string _filename;
    public string Filename => _filename;
    FieldStore<FileField> _fields;
    public FieldStore<FileField> Fields => _fields;
    FunctionStore<FileFunction> _functions;
    public FunctionStore<FileFunction> Functions => _functions;
    OrderedDictionary<string, FileStruct> _structs;

    public CodeFile(CodeSourceFile source) {
        _filename = source.Path;
        _fields = new ();
        _functions = new ();
        _structs = new OrderedDictionary<string, FileStruct>();
    }
    
    public void AddStruct(FileStruct fileStruct) => _structs.Add(fileStruct.Name, fileStruct);
    public FileStruct GetStruct(string name) => _structs[name];
    public List<FileStruct> GetStructs() => _structs.Values.ToList();
    
}