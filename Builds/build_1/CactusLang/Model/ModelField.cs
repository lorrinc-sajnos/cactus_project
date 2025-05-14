using CactusLang.Model.Symbols;

namespace CactusLang.Model;

public class ModelField {
    protected VariableSymbol _symbol;

    public ModelField(VariableSymbol symbol) {
        _symbol = symbol;
    }
    
    public string Name => _symbol.Name;
    public VariableSymbol Symbol => _symbol;
    
}