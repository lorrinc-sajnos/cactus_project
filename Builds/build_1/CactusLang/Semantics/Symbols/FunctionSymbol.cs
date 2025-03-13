using System;
using CactusLang.Semantics.Types;
using CactusLang.Tags;

namespace CactusLang.Semantics.Symbols;

public class FunctionSymbol {
    public TagContainer Tags {get; private set;}
    public string ID {get; private set;}
    public CTSType ReturnType {get; private set;}
    Dictionary<string,VariableSymbol> parameters;

    public FunctionSymbol(string id, CTSType retType){
        ID=id;
        ReturnType = retType;
        
    }

}
