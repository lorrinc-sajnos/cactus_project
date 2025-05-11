namespace CactusLang.CodeGeneration;

public class CodeGenerator {
    private string _fileName;
    private StreamWriter _streamWriter;

    public CodeGenerator(string fileName) {
        _fileName = fileName;
    }
}