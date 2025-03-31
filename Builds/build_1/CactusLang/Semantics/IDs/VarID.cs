namespace CactusLang.Semantics.IDs;

public struct VarID {
    public string Path { get; private set; }

    public VarID(string path) {
        Path = path;
    }

    public override bool Equals(object? obj) {
        if (obj is VarID baseId) {
            return Path.Equals(baseId.Path);
        }      
        return base.Equals(obj);
    }
}