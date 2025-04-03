using System.Collections.Specialized;

namespace CactusLang.Util;

public class OrderedDictionary<TKey, TValue> : Dictionary<TKey, TValue> {
    private readonly List<TKey> _orderedKeys;
    
    public OrderedDictionary(){
        _orderedKeys = new List<TKey>();
    }
    
    public new void Add(TKey key, TValue value) {
        base.Add(key, value);
        _orderedKeys.Add(key);
    }
    public TValue GetByIndex(int index) => this[_orderedKeys[index]];
}