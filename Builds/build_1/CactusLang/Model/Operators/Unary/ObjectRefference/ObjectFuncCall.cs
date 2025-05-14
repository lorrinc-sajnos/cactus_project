using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.Operators.ObjectRefference;

public class ObjectFuncCall : UnaryOp {
    private FileStruct _struct;
    public FileStruct Struct => _struct;

    private StructFunction? _function;
    public StructFunction? Function => _function;

    public BaseType ReturnType { get; private set; }


    public ObjectFuncCall(FileStruct referedStruct, FuncId func) : base(Side.RIGHT, func.ToString()) {
        _struct = referedStruct;
        _function = _struct.Functions.GetMatchingFunction(func);
        ReturnType = _function?.Symbol.ReturnType ?? ErrorType.ERROR;
    }

    public override BaseType Evaluate(BaseType val) => ReturnType;
}