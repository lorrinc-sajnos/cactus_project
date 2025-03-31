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
        return _types.GetByKey(typeName);
    }

}

//TODO először a struct majd a fgv