using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.File;
using CactusLang.Model.Visitors;

namespace CactusLang.CodeGeneration;

public class CodeGenerator : CodeModelVisitor {
    public CodeGenerator(CodeFile  file) : base(file) {
        
    }
    
    
}