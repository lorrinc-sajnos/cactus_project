using CactusLang.Model;
using CactusLang.Model.Codefiles;
using CactusLang.Semantics;
using JetBrains.Annotations;

namespace CactusLangTests.Semantics;

[TestClass]
[TestSubject(typeof(SemanticAnalyzer))]
public class GrammarTest {
    private void AssertNoErrors(SemanticAnalyzer analyzer) =>
        Assert.AreEqual(0, analyzer.ErrorHandler.GetErrors().Count);


    private void TestFileNoErrors(string path) {
        CodeFile file = new CodeFile(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticAnalyzer analyzer = new SemanticAnalyzer(file);
        analyzer.Analyze();
        analyzer.ErrorHandler.PrintErrors();
        AssertNoErrors(analyzer);
        Console.WriteLine($"Done!\n\n");
    }
    

    [TestMethod]
    public void TestObjectReference() {
        CactusLangModel.InitCactusLang();
        
        TestFileNoErrors("test_files/reference/simple_ref.ccts");
        //reference
        TestFileNoErrors("test_files/reference/nested_ref.ccts");
    }


    [TestMethod]
    public void TestOperatorsNoErrors() {
        CactusLangModel.InitCactusLang();
        
        TestFileNoErrors("test_files/type_system/operators/test_op.ccts");
        TestFileNoErrors("test_files/type_system/operators/test_int_op.ccts");
        TestFileNoErrors("test_files/type_system/operators/test_op_mixed.ccts");
    }

    [TestMethod]
    public void TestImplicitCastErrors() {
        CactusLangModel.InitCactusLang();
        
        string path = "test_files/type_system/impl_cast/test_impl_cast_fail_small.ccts";
        
        CodeFile file = new CodeFile(path);
        Console.WriteLine($"Testing file: {path}");
        SemanticAnalyzer analyzer = new SemanticAnalyzer(file);
        analyzer.Analyze();
        analyzer.ErrorHandler.PrintErrors();
        //AssertNoErrors(analyzer);
        long test = 12345678901;
        Console.WriteLine($"Done!\n\n");
    }
}