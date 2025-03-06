// See https://aka.ms/new-console-template for more information
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;

Console.WriteLine("Example parsing");

string filePath = "test.ccts";

string input = File.ReadAllText(filePath);

//Mintaprogram
AntlrInputStream inputStream = new AntlrInputStream(input);
CactusGrammarLexer lexer = new CactusGrammarLexer(inputStream);
CommonTokenStream tokenStream = new CommonTokenStream(lexer);
CactusGrammarParser parser = new CactusGrammarParser(tokenStream);

Console.WriteLine(parser.codefile().ToStringTree());

