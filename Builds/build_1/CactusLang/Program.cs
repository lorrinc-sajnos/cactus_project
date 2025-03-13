// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

Console.WriteLine("Example parsing");

string filePath = "test.ccts";

string input = File.ReadAllText(filePath);

//Mintaprogram
AntlrInputStream inputStream = new AntlrInputStream(input);
GrammarLexer lexer = new GrammarLexer(inputStream);
CommonTokenStream tokenStream = new CommonTokenStream(lexer);
GrammarParser parser = new GrammarParser(tokenStream);


Console.WriteLine(parser.codefile().ToStringTree());

