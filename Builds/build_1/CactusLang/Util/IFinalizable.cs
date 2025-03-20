using CactusLang.Semantics;

namespace CactusLang.Util;

public interface IFinalizable {
    public bool Finalize(TypeSystem typeSystem);
}