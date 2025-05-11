using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Semantics;
using CactusLangTests.Util;
using JetBrains.Annotations;

namespace CactusLangTests.Semantics;

[TestClass]
[TestSubject(typeof(SemanticAnalyzer))]
public class GrammarTest {
    

    [TestMethod]
    public void TestObjectReference() {
        CactusLangModel.InitCactusLang();
        
        CctsAssert.FileNoErrors("test_files/reference/simple_ref.ccts");
        //reference
        CctsAssert.FileNoErrors("test_files/reference/nested_ref.ccts");
    }

    [TestMethod]
    public void TestImplicitCastErrors() {
        CactusLangModel.InitCactusLang();
        
        string path = "test_files/type_system/impl_cast/test_impl_cast_fail_small.ccts";
        
        CodeSourceFile file = new CodeSourceFile(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticAnalyzer analyzer = new SemanticAnalyzer(file);
        analyzer.Analyze();
        analyzer.ErrorHandler.PrintErrors();
        //AssertNoErrors(analyzer);
        long test = 12345678901;
        Console.WriteLine($"Done!\n\n");
    }
}