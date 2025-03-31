using System.Collections.Specialized;

namespace CactusLang.Util;

public class OrderedDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
    private readonly Dictionary<TKey,TValue> _orderedDictionary;
    private readonly List<TKey> _orderedKeys;
    
    public OrderedDictionary(){
        _orderedDictionary = new Dictionary<TKey,TValue>();
        _orderedKeys = new List<TKey>();
    }
    
    public  void Add(TKey key, TValue value) {
        _orderedDictionary.Add(key, value);
        _orderedKeys.Add(key);
    }

    public TValue GetByKey(TKey key) {
        return _orderedDictionary[key];
    }

    public TValue GetByIndex(int index) => GetByKey(_orderedKeys[index]);
}