using System;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Semantics.Symbols;

public class FunctionSymbol {
    public TagContainer Tags { get; private set; }
    public string ID { get; private set; }
    public BaseType ReturnType { get; private set; }

    private Dictionary<string, VariableSymbol> parameters;

    public FunctionSymbol(string id, BaseType retType) {
        ID = id;
        ReturnType = retType;
    }
    
    public void AddParameter(VariableSymbol parameter) => parameters.Add(parameter.Name, parameter);
}