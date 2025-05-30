using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFieldRef : UnaryOp {
    private FileStruct _struct;
    public FileStruct Struct => _struct;

    private StructField _field;
    public StructField Field => _field;

    public ObjectFieldRef(FileStruct referedStruct, StructField field) : base(Side.RIGHT, field.Name) {
        _struct = referedStruct;
        _field = field;
    }

    public override BaseType Evaluate(BaseType val) => _field.Symbol.Type;
}