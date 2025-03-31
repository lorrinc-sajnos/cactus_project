// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CactusLang.Semantics;
using CactusLang.Semantics.Types;
using CactusLang.Util;

CctsPrimitive.InitPrimitives();

Debug.LogLine("Begin parsing");

var filePath = "chilltest.ccts";

var input = File.ReadAllText(filePath);

//Mintaprogram
var inputStream = new AntlrInputStream(input);
var lexer = new GrammarLexer(inputStream);
var tokenStream = new CommonTokenStream(lexer);
var parser = new GrammarParser(tokenStream);


SemanticAnalyzer analyzer = new SemanticAnalyzer();
analyzer.Analyze(parser.codefile());
Console.WriteLine("Parsing finished");
Console.ReadKey();
