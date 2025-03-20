using System.Collections.Specialized;

namespace CactusLang.Util;

public class OrderedDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
    private readonly Dictionary<TKey,TValue> _orderedDictionary;
    private readonly List<TKey> _orderedKeys;
    
    
    public  void Add(TKey key, TValue value) {
        _orderedDictionary.Add(key, value);
        _orderedKeys.Add(key);
    }
    
}