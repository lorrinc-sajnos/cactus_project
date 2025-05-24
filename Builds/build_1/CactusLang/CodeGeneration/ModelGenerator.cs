using CactusLang.CodeGeneration.CCodeGenerator;
using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Visitors;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public class ModelGenerator : CodeModelVisitor<int> {
    public struct State { }

    private State _state;
    private CodeFile _codeFile;
    private CodeStore _codeStore;
    
    private string _directory;
    private string _fileName;
    private string _outDirectory;

    #region Source

    private CodeScope Source { get; set; }
    private void SourceStepIn() => Source = Source.StepIn();
    private void SourceStepOut() => Source = Source.StepOut();

    private void SourceStepInTop() => Source = Source.StepInTop();
    private void SourceStepOutTop() => Source = Source.StepOutTop();

    private void SourceStepInBot() => Source = Source.StepInBot();
    private void SourceStepOutBot() => Source = Source.StepOutBot();

    #endregion

    #region Header

    private CodeScope Header { get; set; }
    private void HeaderStepIn() => Header = Header.StepIn();
    private void HeaderStepOut() => Header = Header.StepOut();

    private void HeaderStepInTop() => Header = Header.StepInTop();
    private void HeaderStepOutTop() => Header = Header.StepOutTop();

    private void HeaderStepInBot() => Header = Header.StepInBot();
    private void HeaderStepOutBot() => Header = Header.StepOutBot();

    #endregion

    public ModelGenerator(CodeFile file) : base(1, 0) {
        _state = new();
        _codeFile = file;
        _codeStore = new();
        Source = _codeStore.Source;
        Header = _codeStore.Header;

        _fileName = Path.GetFileNameWithoutExtension(file.Filename);
        _directory= Path.GetDirectoryName(file.Filename) ?? "";
        _outDirectory = $"out/{_directory}";
    }

    public void GenerateCode() {
        InitHeader();
        InitSource();

        VisitCodeFile(_codeFile);
    }

    public void PrintCode() {
        Console.WriteLine($"-----------------HEADER {_fileName}.h-----------------");
        Header.Print();


        Console.WriteLine($"-----------------SOURCE {_fileName}.c-----------------");
        Source.Print();
        
    }

    private void InitSource() {
        Source.AddLineTop($"#include \"{_fileName}.h\"");
    }

    private void InitHeader() {
        string headerName = $"{_fileName.ToUpper()}_H";

        Header.AddLineTop($"#ifndef {headerName}");
        Header.AddLineTop($"#define {headerName}");

        Header.AddLineTop($"// :D");
        Header.AddLineTop($"#include \"ccts_std.h\"");

        Header.AddLineBot($"#endif");
    }

    #region FileScopeHeader

    protected override int VisitFileFunction(FileFunction fileFunction) {
        string header = CodeGenUtil.GetFileFunctionHeader(fileFunction);
        Header.AddLineTop(header + ";");


        Source.AddLine(header + " {");
        SourceStepIn();
        var res = base.VisitFileFunction(fileFunction);
        SourceStepOut();
        Source.AddLine("}");
        Source.AddLine("");
        return res;
    }

    #endregion

    #region Struct

    protected override int VisitFileStruct(FileStruct fileStruct) {
        Header.AddLineTop($"typedef struct {{");

        //Switch the order around, so we can close the struct, then declare the functions
        HeaderStepInTop();

        foreach (var field in fileStruct.Fields.GetFields())
            VisitStructField(field);

        HeaderStepOutTop();
        Header.AddLineBot();
        Header.AddLineBot();

        Header.AddLineTop($"}} {fileStruct.Name};");

        foreach (var func in fileStruct.Functions.GetFunctions())
            VisitStructFunction(func);

        return 1;
    }

    protected override int VisitStructField(StructField sF) {
        Header.AddLineTop($"{sF.Type.Name} {sF.Name};");
        return base.VisitStructField(sF);
    }

    protected override int VisitStructFunction(StructFunction function) {
        //Generate reference function
        string refHeader = CodeGenUtil.GetStructRefFuncHeader(function);
        Header.AddLineTop(refHeader + ";");

        Source.AddLine(refHeader + " {");
        SourceStepIn();
        var res = base.VisitStructFunction(function);
        SourceStepOut();
        Source.AddLine("}");
        Source.AddLine("");

        //Generate access function
        string accHeader = CodeGenUtil.GetStructAccFuncHeader(function);
        Header.AddLineTop(accHeader + ";");

        Source.AddLine(accHeader + " {");
        SourceStepIn();
        Source.AddLines(CodeGenUtil.GetStructAccFuncBody(function));
        SourceStepOut();
        Source.AddLine("}");
        Source.AddLine("");


        return res;
    }

    #endregion

    #region Statements

    protected override int VisitExpressionStatement(ExpressionStatement expressionStatement) {
        ExpressionCodeFactory factory = new(expressionStatement.Expression, _state);

        string result = factory.Start() + ";";
        Source.AddLine(result);
        return 1;
    }


    protected override int VisitReturnStatement(ReturnStatement returnStatement) {
        string retExpr = ExpressionCodeFactory.Manufacture(returnStatement.Expression, _state);
        string result = $"return  ({retExpr});";
        Source.AddLine(result);
        return 1;
    }

    protected override int VisitVarDclStatement(VarDclStatement varDclStatement) {
        string result = "";
        result += $"{varDclStatement.VarType.Name} ";
        var dclBodies = varDclStatement.GetBodies();
        UtilFunc.SeparatedForEach(dclBodies,
            (body) => {
                result += body.Variable.Name;
                if (body.HasValue) {
                    string varVal = ExpressionCodeFactory.Manufacture(body.Value!, _state);
                    result += $" = {varVal}";
                }
            },
            () => result += CodeGenUtil.PARAM_SEP
        );
        result += ";";

        Source.AddLine(result);

        return 1;
    }

    #endregion

    #region IO
    public void GenerateFiles() {
        //Creating directory if doesnt exist
        Directory.CreateDirectory(_outDirectory);
        
        //Generate Header file
        string headerPath = $"{_outDirectory}/{_fileName}.h";
        WriteCodeScopeToFile(headerPath, Header);

        //Generate Source file
        string sourcePath = $"{_outDirectory}/{_fileName}.c";
        WriteCodeScopeToFile(sourcePath, Source);
    }
    private void WriteCodeScopeToFile(string path, CodeScope codeScope) {
        
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new(fileStream)) {
            codeScope.AppendToWriter(writer);
        }

        fileStream.Close();
    }
    #endregion
}