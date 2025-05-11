using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Semantics;
using CactusLang.Util;

namespace CactusLang.Model.Types;

//TODO REFARCTOR INTO FILESTRUCT!!!!
public class StructType : BaseType {
    private readonly FileStruct _fileStruct;


    public StructType(FileStruct fileStruct) {
        _fileStruct = fileStruct;
    }

    public override string Name => _fileStruct.Name;
    public override int Size => -999; //TODO C generálás alapján kiszámolni...

    protected override bool CanImplicitCastInto(BaseType other) {
        throw new NotImplementedException();
    }
}