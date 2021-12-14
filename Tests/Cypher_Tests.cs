using NUnit.Framework;
using MobApp;

namespace Tests;
public class Cypher_Tests
{

    [TestCase("�����������������������������", "�������������������", "������������������������������")]
    [TestCase("����� ������, �� �� 123  ABC!!!", "��������������", "����� �и���, �� �� 123  ABC!!!")]
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
    [TestCase("������������������������������", "�������������������", "�����������������������������")]
    [TestCase("����� �и���, �� �� 123  ABC!!!", "��������������", "����� ������, �� �� 123  ABC!!!")]
    public void Test_Decypher(string input, string key, string expected)
    {
        var cypherProvider = new CypherProvider(key);
        Assert.AreEqual(expected, cypherProvider.Decypher(input));
    }
}