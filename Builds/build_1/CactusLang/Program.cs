// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CactusLang.CodeGeneration;
using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Types;
using CactusLang.Util;

CactusLangModel.InitCactusLang();

var filePath = "test/test3.ccts";


Debug.LogLine($"Begin parsing file {filePath}");

CodeSourceFile codeFile = new CodeSourceFile(filePath);
    
SemanticAnalyzer analyzer = new SemanticAnalyzer(codeFile);
analyzer.Analyze();

analyzer.ErrorHandler.PrintErrors();

if (analyzer.ErrorHandler.GetErrors().Count > 0) {
    Console.WriteLine("Parsing failed");
    Console.WriteLine("Gen terminated");
    
    return;
}

Console.WriteLine("Parsing finished");
ModelGenerator modelGenerator = new ModelGenerator(analyzer.CodeFile);
modelGenerator.GenerateCode();
//analyzer.Errorhandler

modelGenerator.PrintCode();
modelGenerator.GenerateFiles();

Console.WriteLine("Gen finished");
//Console.ReadKey();