using CactusLang.Model;
using CactusLang.Model.Types;
using JetBrains.Annotations;

namespace CactusLangTests.Model;

[TestClass]
[TestSubject(typeof(CactusLangModel))]
public class CactusLangModelTest {

    [TestMethod]
    public void InitLanguageTest() {
        //TODO test more stuff
        CactusLangModel.InitCactusLang();
        
        var primitives = PrimitiveType.GetPrimitives();

        foreach (var primitive in primitives) {
            Assert.IsNotNull(primitive);
        }
    }
}