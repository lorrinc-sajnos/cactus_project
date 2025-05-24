namespace CactusLang.CodeGeneration.CCodeGenerator;

public class CodeLine : CodeSegment {
    private List<string> _left;
    private List<string> _middle;
    private List<string> _right;

    public CodeLine(string line, int depth) : this() {
        string tab = "";
        for (int i = 0; i < depth; i++)
            tab += "\t";
        _left.Add(tab);
        _middle.Add(line);
    }

    public CodeLine() {
        _left = new List<string>();
        _middle = new List<string>();
        _right = new List<string>();
    }

    public void AddLeft(string line) => _left.Add(line);
    public void AddMiddle(string line) => _middle.Add(line);
    public void AddRight(string line) => _right.Add(line);


    public override void Print() {
        foreach (var line in _left) {
            Console.Write(line);
        }

        foreach (var line in _middle) {
            Console.Write(line);
        }

        foreach (var line in _right) {
            Console.Write(line);
        }

        Console.WriteLine();
    }

    public override void AppendToWriter(TextWriter writer) {
        foreach (var line in _left) {
            writer.Write(line);
        }

        foreach (var line in _middle) {
            writer.Write(line);
        }

        foreach (var line in _right) {
            writer.Write(line);
        }

        writer.WriteLine();
    }
}