using System;
using System.Security.Principal;
using CactusLang.Semantics.IDs;
using CactusLang.Semantics.Types;
using CactusLang.Tags;
using CactusLang.Util;

namespace CactusLang.Semantics.Symbols;

public class FunctionSymbol  {
    public TagContainer Tags { get; private set; }
    public FuncID ID { get; private set; }
    public BaseType ReturnType { get; private set; }

    private Dictionary<string, VariableSymbol> parameters;

    public FunctionSymbol(BaseType retType, string id) {
        ID = new FuncID(id, retType);
        ReturnType = retType;
        parameters = new();
    }

    public void AddParameter(VariableSymbol parameter) => parameters.Add(parameter.Name, parameter);
}