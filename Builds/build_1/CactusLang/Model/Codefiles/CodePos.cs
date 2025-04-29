namespace CactusLang.Model;

public struct CodePos(string fileName, int line, int column) {
    public string FileName { get; private set; } = fileName;
    public int Line { get; private set; } = line;
    public int Column { get; private set; } = column;
    public override string ToString() => $"{FileName}|{Line}:{Column}";
}