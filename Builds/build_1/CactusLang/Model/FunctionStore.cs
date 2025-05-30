using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model;

public class FunctionStore<T> where T : ModelFunction {
    private readonly OrderedDictionary<FuncId, T> _functions;
    public FunctionStore() {
        _functions = new OrderedDictionary<FuncId, T>();
    }

    public T? GetExactFunction(FuncId id) {
        if (!_functions.ContainsKey(id)) {
            //throw new Exception($"Function {id} does not exist.");

            //:(
            return null;
        }

        return _functions[id];
    }

    public bool ContainsExactFunction(FuncId id) => _functions.ContainsKey(id);
    
    
    public T? GetMatchingFunction(FuncId id) {
        if (ContainsExactFunction(id))
            return _functions[id];

        FuncId? guessedId = GetMatchingId(id);

        if (guessedId == null)
            return null;

        return _functions[guessedId.Value];
    }
    
    public bool ContainsMatchingFunction(FuncId id) {
        if (ContainsExactFunction(id)) return true;

        FuncId? guessedId = GetMatchingId(id);
        return guessedId != null;
    }
    public FuncId? GetMatchingId(FuncId id) {
        if (_functions.ContainsKey(id))
            return id;

        //If exact match is not found, check for ones with same name and parameter count
        var matchingFunctions =
            _functions.Keys
                .Where(f => f.Name.Equals(id.Name) && f.Params.Count == id.Params.Count)
                .ToList();

        if (!matchingFunctions.Any())
            return null;

        foreach (var possibleFunc in matchingFunctions) {
            if (id.DoesMatch(possibleFunc))
                return possibleFunc;
        }


        return null;
    }

    public bool AddFunction(T variable) {
        if (_functions.ContainsKey(variable.FuncId)) {
            //throw new Exception($"Function {variable.Name} already exists");
            return false;
        }

        //If has functions, check for correct overload
        //if(_fileFunctions.Count>0) {
        var overloads = _functions.Values
            .Where(f => f.Name.Equals(variable.Name));

        if (overloads.Any() && overloads.First().ReturnType != variable.ReturnType)
            //Error!
            return false;


        _functions.Add(variable.FuncId, variable);
        return true;
    }
    
    public List<T> GetFunctions() => _functions.Values.ToList();
}