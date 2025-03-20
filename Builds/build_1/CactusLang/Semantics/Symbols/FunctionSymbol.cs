using System;
using CactusLang.Semantics.Types;
using CactusLang.Tags;
using CactusLang.Util;

namespace CactusLang.Semantics.Symbols;

public class FunctionSymbol : IFinalizable {
    public TagContainer Tags { get; private set; }
    public string ID { get; private set; }
    public BaseType ReturnType { get; private set; }

    private Dictionary<string, VariableSymbol> parameters;

    public FunctionSymbol(BaseType retType, string id) {
        ID = id;
        ReturnType = retType;
    }

    public void AddParameter(VariableSymbol parameter) => parameters.Add(parameter.Name, parameter);

    public bool Finalize(TypeSystem typeSystem) {
        bool retFin = FinalizeReturnType(typeSystem);

        var missingParams = parameters.Where(p => p.Value.Type is MissingType);

        foreach (var missingParam in missingParams) {
            bool result = FinalizeParam(missingParam.Key, missingParam.Value.Type as MissingType, typeSystem);
            if (!result) return false;
        }

        return true;
    }

    private bool FinalizeReturnType(TypeSystem typeSystem) {
        if (ReturnType is not MissingType missing) return true;
        string rplcName = missing.Name;
        BaseType rplcType = typeSystem.Get(rplcName);

        if (rplcType == null) return false;

        ReturnType = rplcType;
        return true;
    }

    private bool FinalizeParam(string key, MissingType missing, TypeSystem typeSystem) {
        string rplcName = missing.Name;
        BaseType rplcType = typeSystem.Get(rplcName);
        if (rplcType == null) return false;

        VariableSymbol rplcVar = new VariableSymbol(rplcType, rplcName, -999);
        parameters[key] = rplcVar;
        return true;
    }
}