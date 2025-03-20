// See https://aka.ms/new-console-template for more information

using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CactusLang.Semantics;

Console.WriteLine("Example parsing");

var filePath = "test.ccts";

var input = File.ReadAllText(filePath);

//Mintaprogram
var inputStream = new AntlrInputStream(input);
var lexer = new GrammarLexer(inputStream);
var tokenStream = new CommonTokenStream(lexer);
var parser = new GrammarParser(tokenStream);


SemanticAnalyzer analyzer = new SemanticAnalyzer();

analyzer.Visit

Console.WriteLine(parser.codefile().ToStringTree());