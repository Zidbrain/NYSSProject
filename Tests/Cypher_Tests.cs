using NUnit.Framework;
using MobApp;

namespace Tests;
public class Cypher_Tests
{

    [TestCase("אבגדהו¸זחטיךכלםמןנסעףפץצקרש‎‏ÿ", "באגנןאמסעאיףחגפûזגה", "בבהףףופרתטף‏ףמגיצעץףףצ¸¸קזךן‏ט")]
    [TestCase("ךנÓעמ עÅסעÈÌ, ÌÛ הא 123  ABC!!!", "אÈגנמאËפעףמסעמ", "ךשÕד‎ עÐ¸וÜÛ, ÞÍ עא 123  ABC!!!")]
    public void TestCypher(string input, string key, string expected)
    {
        var cypherProvider = new CypherProvider(key);
        Assert.AreEqual(expected, cypherProvider.Cypher(input));
    }

    [Test]
    public void Test_Cypher_Throws_Key()
    {
        Assert.Throws<System.ArgumentNullException>(() => new CypherProvider(null));

        Assert.Throws<System.ArgumentException>(() => new CypherProvider("dasda"));
    }
}

public class Decypher_Tests
{
    [TestCase("בבהףףופרתטף‏ףמגיצעץףףצ¸¸קזךן‏ט", "באגנןאמסעאיףחגפûזגה", "אבגדהו¸זחטיךכלםמןנסעףפץצקרש‎‏ÿ")]
    [TestCase("ךשÕד‎ עÐ¸וÜÛ, ÞÍ עא 123  ABC!!!", "אÈגנמאËפעףמסעמ", "ךנÓעמ עÅסעÈÌ, ÌÛ הא 123  ABC!!!")]
    public void Test_Decypher(string input, string key, string expected)
    {
        var cypherProvider = new CypherProvider(key);
        Assert.AreEqual(expected, cypherProvider.Decypher(input));
    }
}