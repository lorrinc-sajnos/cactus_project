namespace CactusLang.CodeGeneration.CCodeGenerator;

public abstract class CodeSegment {
    
    public int Depth { get; protected set; }

    public abstract void Print();

    public abstract void AppendToWriter(TextWriter writer);
}