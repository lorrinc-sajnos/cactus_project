using CactusLang.CodeGeneration.CCodeGenerator;
using CactusLang.Model;
using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.CodeStructure.Statements;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Visitors;
using CactusLang.Tags;
using CactusLang.Tags.StatementTags;
using CactusLang.Util;

namespace CactusLang.CodeGeneration;

public class CodeGenerator : CodeModelVisitor<int> {
    public struct State { }

    public const string CCTS_STD_HEADER = "ccts_std.h";

    private State _state;
    public State GenState => _state;
    private CodeFile _codeFile;
    public CodeFile CodeFile => _codeFile;
    private CodeStore _codeStore;
    public CodeStore CodeStore => _codeStore;

    private string _directory;
    private string _fileName;
    public string FileName => _fileName;
    public string FilePath => $"{_outDirectory}/{_fileName}";
    private string _outDirectory;

    #region Source

    public CodeScope Source { get; private set; }
    private void SourceStepIn() {
        Source = Source.StepIn();
    }

    private void SourceStepOut() {
        Source = Source.StepOut();
    }

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

    public CodeGenerator(CodeFile file, string outputFile) : base(1, 0) {
        _state = new();
        _codeFile = file;
        _codeStore = new();
        Source = _codeStore.Source;
        Header = _codeStore.Header;

        _fileName = Path.GetFileNameWithoutExtension(outputFile);
        _directory = Path.GetDirectoryName(outputFile) ?? "";
        _outDirectory = $"{_directory}/src";
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
        VisitStatementTags(returnStatement, t => {
            t.OnReturnStatement(returnStatement, this);
        });

        string retExpr = ExpressionCodeFactory.Manufacture(returnStatement.Expression, _state);
        string result = $"return  ({retExpr});";
        Source.AddLine(result);
        return 1;
    }


    protected override int VisitFreeStatement(FreeStatement freeStatement) {
        string freeExpr = ExpressionCodeFactory.Manufacture(freeStatement.Expression, _state);
        string result = $"free({freeExpr});";
        Source.AddLine(result);
        return 1;
    }

    protected override int VisitRawCCodeStatement(RawCCodeStatement rawCCodeStatement) {
        string[] lines = rawCCodeStatement.RawCode.Split('\n');
        foreach (string ln in lines)
            Source.AddLine(ln.TrimStart());
        return 1;
    }

    protected override int VisitVarDclStatement(VarDclStatement varDclStatement) {
        string result = CodeGenUtil.GetVarDclString(varDclStatement);
        result += ";";

        Source.AddLine(result);

        return 1;
    }

    protected override int VisitForLoop(ForLoop forLoop) {
        string loopTop = "for (";
        loopTop += CodeGenUtil.GetVarDclString(forLoop.LoopDcl);
        loopTop += $" {ExpressionCodeFactory.Manufacture(forLoop.Condition)}";
        loopTop += $"; {ExpressionCodeFactory.Manufacture(forLoop.EndStatement)}) {{";
        Source.AddLine(loopTop);
        
        SourceStepIn();
        var res = base.VisitCodeBlock(forLoop.LoopBody);
        SourceStepOut();
        
        Source.AddLine("}");
        
        return res;
    }

    #endregion

    #region Tag visits

    private void VisitFileTags(CodeFile file, Action<Tag> action) {
        foreach (Tag tag in file.TagContainer.GetTags())
            action(tag);
    }

    private void VisitFunctionTags(ModelFunction func, Action<Tag> action) {
        VisitFileTags(func.CodeFile, action);

        foreach (Tag tag in func.TagContainer.GetTags())
            action(tag);
    }

    private void VisitStatementTags(Statement statement, Action<Tag> action) {
        VisitFunctionTags(statement.CodeBlock.Function, action);

        foreach (Tag tag in statement.TagContainer.GetTags())
            action.Invoke(tag);
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

        CopyCctsStd();
    }

    private void WriteCodeScopeToFile(string path, CodeScope codeScope) {
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new(fileStream)) {
            codeScope.AppendToWriter(writer);
        }

        fileStream.Close();
    }

    #endregion

    private void CopyCctsStd() {
        File.Copy(CCTS_STD_HEADER, $"{_outDirectory}/{CCTS_STD_HEADER}", overwrite: true);
    }
}