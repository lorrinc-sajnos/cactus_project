using CactusLang.Model;
using CactusLang.Model.Codefiles;

namespace CactusLangTests;

public class TestCodeFile : CodeSourceFile {
    public TestCodeFile(string path) : base(path) { }

    public TestCodeFile(string path, string code) : base(path, code) { }
        
}