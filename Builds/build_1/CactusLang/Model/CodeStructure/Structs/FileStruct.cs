using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure;

public class FileStruct  {
    private StructType _struct;
    
    private FieldStore<StructField> _fields;
    public FieldStore<StructField> Fields => _fields;
    
    private FunctionStore<StructFunction> _functions;
    public FunctionStore<StructFunction> Functions => _functions;
    
    public string Name => _struct.Name;
    public StructType Struct => _struct;

    
}