using NUnit.Framework;
using MobApp;
using System.IO;

namespace Tests;

public class File_Tests
{
    [Test]
    public void Test_Open_TXT()
    {
        Assert.AreEqual("abc \r\ndal", CypherViewModel.ReadFromFileAsync(@"Files\Example.txt").Result);
    }

    [Test]
    public void Test_Save_TXT()
    {
        CypherViewModel.SaveToFileAsync(@"Files\SaveExample.txt", "АБВ\nабв").Wait();

        FileAssert.Exists(@"Files\SaveExample.txt");
        Assert.AreEqual("АБВ\nабв", File.ReadAllText(@"Files\SaveExample.txt"));
    }

    [Test]
    public void Test_Open_DOCX()
    {
        Assert.AreEqual("abc \ndal", CypherViewModel.ReadFromFileAsync(@"Files\Example.docx").Result);
    }

    [Test]
    public void Test_Save_DOCX()
    {
        CypherViewModel.SaveToFileAsync(@"Files\SaveExample.docx", "abc \nabc").Wait();

        FileAssert.Exists(@"Files\SaveExample.docx");
        Assert.AreEqual("abc \nabc", CypherViewModel.ReadFromFileAsync(@"Files\SaveExample.docx").Result);
    }
}
