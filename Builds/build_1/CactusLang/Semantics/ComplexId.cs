namespace CactusLang.Semantics;

public struct ComplexId {
    private string[] idParts;
    private AccessType[] access;

    public ComplexId(GrammarParser.VarRefContext varRef) {
        List<string> idPartList = new List<string>();
        List<AccessType> accessParts = new List<AccessType>();
        
        foreach(var idPart in varRef.idPart())
            idPartList.Add(idPart.GetText());
        
        this.idParts = idPartList.ToArray();

        foreach (var accessPart in varRef.accOp()) {
            string rawOp = accessPart.GetText();
            
            if(rawOp.Equals(":"))
                accessParts.Add(AccessType.Access);
            else if(rawOp.Equals("."))
                accessParts.Add(AccessType.Reference);
        }
        if(idPartList.Count != accessParts.Count-1)
            throw new Exception("Incorrect number of id-parts");
        
        this.access = accessParts.ToArray();
    }
}


public enum AccessType { Reference, Access } 