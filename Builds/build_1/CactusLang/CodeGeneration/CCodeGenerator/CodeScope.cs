using System.Runtime.CompilerServices;

namespace CactusLang.CodeGeneration.CCodeGenerator;

public class CodeScope : CodeSegment {
    private List<CodeSegment> _top;
    private List<CodeSegment> _mid;
    private List<CodeSegment> _bot;

    private CodeScope? _parent;
    public CodeScope() : this(null) { }

    private CodeScope(CodeScope parent) {
        _top = new List<CodeSegment>();
        _mid = new List<CodeSegment>();
        _bot = new List<CodeSegment>();
        _parent = parent;
        if (parent != null) {
            Depth = parent.Depth + 1;
        }
        else {
            Depth = 0;
        }
    }

    public void AddLineTop(string line) => _top.Add(new CodeLine(line, Depth));
    public void AddLineT() => _top.Add(new CodeLine("", Depth));

    public void AddLine() => _mid.Add(new CodeLine("", Depth));
    public void AddLine(string line) => _mid.Add(new CodeLine(line, Depth));
    public void AddLines(List<string> lines) => lines.ForEach(AddLine);

    public void AddLineBot(string line) => _bot.Add(new CodeLine(line, Depth));
    public void AddLineBot() => _bot.Add(new CodeLine("", Depth));

    public CodeScope StepIn() {
        var codeScope = new CodeScope(this);
        _mid.Add(codeScope);
        return codeScope;
    }

    public CodeScope StepOut() {
        return _parent!;
    }


    public CodeScope StepInTop() {
        var codeScope = new CodeScope(this);
        _top.Add(codeScope);
        return codeScope;
    }

    public CodeScope StepOutTop() => StepOut();


    public CodeScope StepInBot() {
        var codeScope = new CodeScope(this);
        _bot.Add(codeScope);
        return codeScope;
    }

    public CodeScope StepOutBot() => StepOut();


    public override void Print() {
        foreach (var segment in _top)
            segment.Print();

        foreach (var segment in _mid)
            segment.Print();

        for (var i = 1; i <= _bot.Count; i++) {
            var segment = _bot[^i];
            segment.Print();
        }

        Console.WriteLine();
    }

    public override void AppendToWriter(TextWriter writer) {
        foreach (var segment in _top)
            segment.AppendToWriter(writer);

        foreach (var segment in _mid)
            segment.AppendToWriter(writer);

        for (var i = 1; i <= _bot.Count; i++) {
            var segment = _bot[^i];
            segment.AppendToWriter(writer);
        }

        writer.WriteLine();
    }
}