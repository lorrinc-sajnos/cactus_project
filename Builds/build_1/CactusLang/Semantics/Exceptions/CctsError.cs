using System.Xml;

namespace CactusLang.Semantics;

public class CctsError {
    public int Code { get; private set; }
    public string Title { get; private set; }
    public string Msg { get; private set; }
    public Exception Exception { get; private set; }

    private CctsError(int code, string title, string msg) {
        Code = code;
        Title = title;
        Msg = msg;
        Exception = new Exception(msg);
    }
    
    
    public  static readonly CctsError CCTS_ID_NOT_FOUND = new(1, "ID Not Found", "Could not find ID");
}