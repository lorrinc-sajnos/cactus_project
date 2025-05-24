using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Scopes;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Util;

namespace CactusLang.Model.CodeStructure;

public class FileStruct {
    private readonly string _id;
    public string Name => _id;
    
    private CodeFile _codeFile;
    public CodeFile CodeFile => _codeFile;

    private FieldStore<StructField> _fields;
    public FieldStore<StructField> Fields => _fields;

    private FunctionStore<StructFunction> _functions;
    public FunctionStore<StructFunction> Functions => _functions;
    
    private FileStruct.Type _struct;
    public FileStruct.Type StructType => _struct;
    public FileStruct(string id,  CodeFile codeFile) {
        _id = id;
        _codeFile = codeFile;
        _fields = new FieldStore<StructField?>();
        _functions = new FunctionStore<StructFunction>();
        _struct = new(this);
    }


    
    
    
    public class Type : BaseType {
        private readonly FileStruct _fileStruct;
        public FileStruct FileStruct => _fileStruct;
        public override string Name => _fileStruct.Name;

        internal Type(FileStruct fileStruct) {
            _fileStruct = fileStruct;
        }

        public override int Size => -999; //TODO C generálás alapján kiszámolni...

        protected override bool CanImplicitCastInto(BaseType other) {
            throw new NotImplementedException();
        }
    }
}