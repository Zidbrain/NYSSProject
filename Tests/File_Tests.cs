using NUnit.Framework;
using NYSSProject;
using System.IO;

namespace Tests;

public class File_Tests
{
    [Test]
    public void Test_Open_TXT()
    {
        Assert.AreEqual("abc \r\ndal", CypherViewModel.ReadFromFileAsync(File.OpenRead(@"Files\Example.txt"), @"Files\Example.txt").Result);
    }

    [Test]
    public void Test_Save_TXT()
    {
        CypherViewModel.SaveToFileAsync(File.Create(@"Files\SaveExample.txt"), @"Files\SaveExample.txt", "АБВ\nабв").Wait();

        FileAssert.Exists(@"Files\SaveExample.txt");
        Assert.AreEqual("АБВ\nабв", File.ReadAllText(@"Files\SaveExample.txt"));
    }

    [Test]
    public void Test_Open_DOCX()
    {
        Assert.AreEqual("abc \ndal", CypherViewModel.ReadFromFileAsync(File.OpenRead(@"Files\Example.docx"), @"Files\Example.docx").Result);
    }

    [Test]
    public void Test_Save_DOCX()
    {
        CypherViewModel.SaveToFileAsync(File.Create(@"Files\SaveExample.docx"), @"Files\SaveExample.docx", "abc \nabc").Wait();

        FileAssert.Exists(@"Files\SaveExample.docx");
        Assert.AreEqual("abc \nabc", CypherViewModel.ReadFromFileAsync(File.OpenRead(@"Files\SaveExample.docx"), @"Files\SaveExample.docx").Result);
    }
}
