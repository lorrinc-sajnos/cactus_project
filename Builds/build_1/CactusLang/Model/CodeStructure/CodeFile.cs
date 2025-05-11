using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure;

public class CodeFile {
    private FileScope _scope;
    
    OrderedDictionary<string, FileVariable> _variables;
    OrderedDictionary<FuncId, FileFunction> _functions;
    OrderedDictionary<string, FileStruct> _structs;

    public CodeFile(FileScope scope) {
        _scope = scope;
        _variables = new OrderedDictionary<string, FileVariable>();
        _functions = new OrderedDictionary<FuncId, FileFunction>();
        _structs = new OrderedDictionary<string, FileStruct>();
    }
    
    public Scope Scope => _scope;
    
    public List<FileStruct> GetStructs() => _structs.Values.ToList();
    public List<FileVariable> GetVariables() => _variables.Values.ToList();
    public List<FileFunction> GetFunctions() => _functions.Values.ToList();
    
}