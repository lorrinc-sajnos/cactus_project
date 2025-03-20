using System;
using System.Diagnostics.CodeAnalysis;
using CactusLang.Semantics.Symbols;
using CactusLang.Semantics.Types;
using CactusLang.Util;

namespace CactusLang.Semantics;

//

public class TypeSystem {
    private OrderedDictionary<string, BaseType> _types;
    public bool MissingTypeFlag { get; set; }

    public TypeSystem() {
        _types = new();
        _finalizables = new();
        AddPrimitives();
    }

    //Primitives
    private void AddPrimitives() {
        foreach (var primitive in CctsPrimitive.GetPrimitives()) {
            _types.Add(primitive.Name, primitive);
        }
    }

    public void AddType(BaseType type) {
        _types.Add(type.Name, type);
    }

    public BaseType Get(string typeName) {
        return _types[typeName];
    }

    public VariableSymbol CreateOptmcVarSym(string typeName, string name) {
        VariableSymbol symbol;
        if (_types.ContainsKey(typeName)) {
            symbol = new VariableSymbol(_types[typeName], name, -999);
        }
        else {
            symbol = new VariableSymbol(new MissingType(typeName), name, -9999);
            _finalizables.Add(symbol);
        }
        return symbol; 
    }
    public FunctionSymbol CreateOptmcFuncSym(string typeName, string name) {
        FunctionSymbol symbol;
        if (_types.ContainsKey(typeName)) {
            symbol = new FunctionSymbol(name, _types[typeName]);
        }
        else {
            symbol = new FunctionSymbol(name, new MissingType(typeName));
            _finalizables.Add(symbol);
        }
        return symbol; 
    }

    List<IFinalizable> _finalizables;

    public bool Finalize() {
        foreach (var finalizable in _finalizables) {
            if(!finalizable.Finalize(this)) return false;
        }
        return true;
    }
}