using CactusLang.Model.Codefiles;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Tags;
using CactusLang.Tags.StatementTags;
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
    public OrderedDictionary<string, FileStruct> Structs => _structs;
    
    private TagContainer _container;
    public TagContainer TagContainer => _container;

    public CodeFile(CodeSourceFile source) {
        _filename = source.Path;
        _fields = new ();
        _functions = new ();
        _structs = new OrderedDictionary<string, FileStruct>();
        _container = new ();
    }
    
    public void AddStruct(FileStruct fileStruct) => _structs.Add(fileStruct.Name, fileStruct);

    public void AddTags(List<Tag> tagList) {
        foreach (var tag in tagList) {
            tag.OnDeclared(this);
        }
        TagContainer.AddTags(tagList);
    }
    public FileStruct GetStruct(string name) => _structs[name];
    public List<FileStruct> GetStructs() => _structs.Values.ToList();
    
}