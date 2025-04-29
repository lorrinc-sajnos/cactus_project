namespace CactusLang.Model.Codefiles;

public class CodeFile {
    public string Path { get; private set; }

    public string Code { get; private set; }

    protected CodeFile(string path, string code) {
        Path = path;
        Code = code;
    }
    public CodeFile(string path) {
        Path = path;

        Code = File.ReadAllText(Path);
    }
}