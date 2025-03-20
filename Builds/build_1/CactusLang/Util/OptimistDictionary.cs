using System.Collections;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace CactusLang.Util;

public class __OptimistDictionary<V> where V : class {
    private Dictionary<string, V> _certainData;
    private Dictionary<string, V> _requestedData;

    public int StoredCount => CertainCount + RequestedCount;
    public int CertainCount => _certainData.Count;
    public int RequestedCount => _requestedData.Count;
    public bool HasRequests() => RequestedCount > 0;

    public __OptimistDictionary() {
        _certainData = new();
        _requestedData = new();
    }


    /// <summary>
    /// Works like Get, but if key is not present, it will add it to the requests, then return the optimist value
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value">The value to add to the requested elements.</param>
    /// <returns></returns>
    public V Request(string key, V optimistValue = null) {
        if (_certainData.ContainsKey(key)) return _certainData[key];
        _requestedData.Add(key, optimistValue);
        return optimistValue;
    }

    /// <summary>
    /// Works like Contains, but if key is not present, it will add it to the requests.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value">The value to add to the requested elements.</param>
    /// <returns></returns>
    public bool HasRequest(string key, V optimistValue = null) {
        if (_certainData.ContainsKey(key)) return true;
        _requestedData.Add(key, optimistValue);
        return false;
    }
    
    public V TryGetCertain(string key) {
        if (_certainData.ContainsKey(key)) 
            return _certainData[key];
        return null;
    }

    /// <summary>
    /// Adds an element to the Dictionary. If it was requested, it will be certified.
    /// </summary>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public bool Add(string key, V value) {
        if (_certainData.ContainsKey(key)) return false; //If already added, false

        if (!_requestedData.ContainsKey(key)) { //If not promised, then add
            _certainData.Add(key, value);
            return true;
        }

        //When promised
        //If requested type is not null, then if they are not equal, throw exception
        if (_requestedData[key] != null && !_requestedData[key].Equals(value))
            throw new Exception($"TYPE MISMATCH {key}"); //TODO err hand

        //Make requested certain
        _requestedData.Remove(key);
        _certainData.Add(key, value);

        return false; //Not sure if true or false
    }
}