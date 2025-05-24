namespace CactusLang.CodeGeneration.CCodeGenerator;

public class CodeStore {
    private CodeScope _source;
    public CodeScope Source => _source;
    
    private CodeScope _header;
    public CodeScope Header => _header;

    public CodeStore() {
        _source = new CodeScope();
        _header = new CodeScope();
    }
}