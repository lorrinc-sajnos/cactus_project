// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
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

//analyzer.Errorhandler


Console.WriteLine("Parsing finished");
//Console.ReadKey();