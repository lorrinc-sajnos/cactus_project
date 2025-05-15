using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFuncCall : UnaryOp {
    private FileStruct _struct;
    public FileStruct Struct => _struct;

    private StructFunction _function;
    public StructFunction Function => _function;

    public BaseType ReturnType => _function.ReturnType;


    public ObjectFuncCall(FileStruct referedStruct, StructFunction structFunc) : base(Side.RIGHT, structFunc.FuncId.ToString()) {
        _struct = referedStruct;
        _function = structFunc;
    }

    public override BaseType Evaluate(BaseType val) => ReturnType;
}