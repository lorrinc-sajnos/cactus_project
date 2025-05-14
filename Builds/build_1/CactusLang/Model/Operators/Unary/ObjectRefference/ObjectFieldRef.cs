using CactusLang.Model.CodeStructure;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFieldRef : UnaryOp {
    private FileStruct _struct;
    public FileStruct Struct => _struct;

    private StructField _field;
    public StructField Field => _field;

    public ObjectFieldRef(FileStruct referedStruct, string fieldName) : base(Side.RIGHT, fieldName) {
        _struct = referedStruct;
        _field = _struct.Fields.GetField(fieldName);
    }

    public override BaseType Evaluate(BaseType val) => _field.Symbol.Type;
}