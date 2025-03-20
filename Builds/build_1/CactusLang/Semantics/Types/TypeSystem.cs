using System;
using CactusLang.Semantics.Types;
using CactusLang.Util;

namespace CactusLang.Semantics;

//

public class TypeSystem {
    private OrderedDictionary<string,BaseType> _types;

    public TypeSystem() {
        _types = new();
        _AddPrimitives();
    }

    //Primitives
    private void _AddPrimitives() {
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
}