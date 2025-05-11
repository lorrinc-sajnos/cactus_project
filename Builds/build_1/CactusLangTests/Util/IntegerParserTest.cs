using CactusLang.Model.Types;
using CactusLang.Util;
using JetBrains.Annotations;

namespace CactusLangTests.Util;

[TestClass]
[TestSubject(typeof(IntegerParser))]
public class IntegerParserTest {
    [TestMethod]
    public void TestParser() {
        //Sanity test
        Assert.AreEqual(1, IntegerParser.Parse("1"));
        //Base 10 test
        Assert.AreEqual(1547618273123, IntegerParser.Parse("1547618273123"));
        //Hex test
        Assert.AreEqual(0xFFAB, IntegerParser.Parse("0xFFAB"));
        //Binary test
        Assert.AreEqual(0b010110101010, IntegerParser.Parse("0b010110101010"));
        //Octal test
        Assert.AreEqual(110921692, IntegerParser.Parse("0o647103734"));

        
        //TypeTesting
        /*
        Assert.AreEqual(PrimitiveType.I16, IntegerParser.GetLiteralTypes(Int16.MaxValue));
        Assert.AreEqual(PrimitiveType.I32, IntegerParser.GetLiteralTypes(Int32.MaxValue));
        Assert.AreEqual(PrimitiveType.I64, IntegerParser.GetLiteralTypes(Int64.MaxValue));//*/
    }
}