using System.Text;
using Antlr4.Runtime;

namespace CactusLang.Util;

public class Debug {
    
    public const bool DEBUG = true;


    public static void Log(string msg) {
        if(!DEBUG) return;
        Console.Write(msg);
    }
    public static void LogLine() => LogLine("");
    public static void LogLine(string msg) {
        if (!DEBUG) return;
        Console.WriteLine(msg);
    }
}