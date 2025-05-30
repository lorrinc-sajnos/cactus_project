// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Antlr4.Runtime;
using Antlr4.Runtime.Tree;
using CactusLang.CodeGeneration;
using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Model.Types;
using CactusLang.Semantics;
using CactusLang.Semantics.Types;
using Debug = CactusLang.Util.Debug;

internal class Program {
    public static void Main(string[] args) {
        if (args.Length <= 0) {
            Console.WriteLine("Input file must be provided!");
            return;
        }
        string inputFile = args[0];
        
        string srcDir = Path.GetDirectoryName(inputFile);
        
        string outputFile = $"{srcDir}/{Path.GetFileNameWithoutExtension(inputFile)}.out";
        if(args.Length>1)
            outputFile = args[1];
        
        
        
        CactusLangModel.InitCactusLang();


        Console.WriteLine($"-\tBegin parsing file {inputFile}");

        CodeSourceFile codeFile = new CodeSourceFile(inputFile);
    
        SemanticAnalyzer analyzer = new SemanticAnalyzer(codeFile);
        analyzer.Analyze();

        analyzer.ErrorHandler.PrintErrors();

        if (analyzer.ErrorHandler.GetErrors().Count > 0) {
            Console.WriteLine("-\tParsing failed");
            Console.WriteLine("-\tC code generation terminated");
    
            return;
        }

        Console.WriteLine("-\tParsing finished");
        CodeGenerator modelGenerator = new CodeGenerator(analyzer.CodeFile, inputFile);
        modelGenerator.GenerateCode();
//analyzer.Errorhandler

        //modelGenerator.PrintCode();
        modelGenerator.GenerateFiles();

        Console.WriteLine("-\tC code generation finished");
        
        //Pipe into GCC
        string gccCmd = "gcc";
        string gccArgs = $"\"{modelGenerator.FilePath}.c\" -o \"{outputFile}\"";

        // Create a new process
        Process gccPRoc = new Process();
        gccPRoc.StartInfo.FileName = gccCmd;
        gccPRoc.StartInfo.Arguments = gccArgs;

        // Redirect output so we can read it in C#
        gccPRoc.StartInfo.RedirectStandardOutput = true;
        gccPRoc.StartInfo.RedirectStandardError = true;
        gccPRoc.StartInfo.UseShellExecute = false;
        gccPRoc.StartInfo.CreateNoWindow = true;

        // Start the process
        gccPRoc.Start();

        // Read output streams
        string gccError = gccPRoc.StandardError.ReadToEnd();
        gccPRoc.WaitForExit();

        if(!gccError.Equals("")) {
            Console.WriteLine("-\tGCC ERROR:");
            Console.WriteLine($"-\t{gccError}");
            Console.WriteLine($"-\tGCC Exit Code: {gccPRoc.ExitCode}");
            return;
        }
        Console.WriteLine("-\tGCC compilation finished with no errors");
        
    }
}
//Console.ReadKey();