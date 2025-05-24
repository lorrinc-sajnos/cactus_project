using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Structs;

namespace CactusLang.CodeGeneration;

public static class CodeGenUtil {
    #region Code templates

    public const string STRUCT_REF_FUNC_HEADER_FORMAT = "{0}_ref__{1}";
    public const string STRUCT_ACC_FUNC_HEADER_FORMAT = "{0}_acc__{1}";
    public const string PARAM_SEP = ", ";

    #endregion

    public static string GetStructRefFuncName(StructFunction func) => $"{func.Parent.Name.ToLower()}_ref__{func.Name}";

    public static string GetStructAccFuncName(StructFunction func) => $"{func.Parent.Name.ToLower()}_acc__{func.Name}";

    #region Util

    public static string GetStructRefFuncHeader(StructFunction func) {
        string header = "";

        header += func.ReturnType.Name + " ";
        //In C, no local functions, so file function w/ custom name:
        header += GetStructRefFuncName(func);
        //Introduce the "this" pointer as first variable
        header += $"({func.Parent.Name}* this" + GetFuncDclParams_NO_OPEN(func);

        return header;
    }

    public static string GetStructAccFuncHeader(StructFunction func) {
        string header = "";

        header += func.ReturnType.Name + " ";
        //In C, no local functions, so file function w/ custom name:
        header += GetStructAccFuncName(func);
        //Introduce the "thisVal" as first variable
        header += $"({func.Parent.Name} thisVal{GetFuncDclParams_NO_OPEN(func)}";

        return header;
    }

    public static List<string> GetStructAccFuncBody(StructFunction func) {
        List<string> body = new List<string>();

        body.Add($"{func.Parent.Name}* this = &thisVal;");
        body.Add($"return {GetStructRefFuncName(func)}(this{GetFuncCallParams_NO_OPEN(func)};");

        return body;
    }

    public static string GetFileFunctionHeader(FileFunction func) {
        string header = "";

        header += func.ReturnType.Name + " ";
        header += func.Name;
        header += "(" + GetFuncDclParams_NO_OPEN(func);
        return header;
    }

    public static string GetFuncDclParams_NO_OPEN(ModelFunction func) {
        string header = "";
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count > 0) {
            //Strunc functions first param is the struct itself
            if (func is StructFunction)
                header += PARAM_SEP;

            for (int i = 0; i < parameters.Count - 1; i++) {
                var param = parameters[i];
                header += $"{param.Type.Name} {param.Name}" + PARAM_SEP;
            }

            header += $"{parameters[^1].Type.Name} {parameters[^1].Name}";
        }

        header += ")";
        return header;
    }

    public static string GetFuncCallParams_NO_OPEN(ModelFunction func) {
        string header = "";
        var parameters = func.Symbol.GetParameters();
        if (parameters.Count > 0) {
            //Strunc functions first param is the struct itself
            if (func is StructFunction)
                header += PARAM_SEP;

            for (int i = 0; i < parameters.Count - 1; i++) {
                var param = parameters[i];
                header += $"{param.Name}" + PARAM_SEP;
            }

            header += $"{parameters[^1].Name}";
        }

        header += ")";
        return header;
    }

    #endregion
}