namespace CactusLang.Model.Codefiles;

public class CodeSourceFile {
    public string Path { get; private set; }

    public string Code { get; private set; }

    protected CodeSourceFile(string path, string code) {
        Path = path;
        Code = code;
    }
    public CodeSourceFile(string path) {
        Path = path;

        Code = File.ReadAllText(Path);
    }
}