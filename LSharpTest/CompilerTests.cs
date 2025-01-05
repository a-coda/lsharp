using LSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharpTest;

internal class CompilerTests
{
    private Driver driver;

    [SetUp]
    public void Setup()
    {
        driver = new Driver();
    }

    [Test]
    public void TestConcatChars()
    {
        var result = driver.Eval("(System.String.Concat \"1\" \"1\")\")");
        Assert.That(result, Is.EqualTo("11"));
    }

    [Test]
    public void TestConcatStrings()
    {
        var result = driver.Eval("(System.String.Concat \"hello \" \"world\")");
        Assert.That(result, Is.EqualTo("hello world"));
    }

    [Test]
    public void TestNestedConcat()
    {
        var result = driver.Eval("(System.String.Concat \"abc\" (System.String.Concat \"def\" \"ghi\"))");
        Assert.That(result, Is.EqualTo("abcdefghi"));
    }

    [TestCase(true, "true path")]
    [TestCase(false, "false path")]
    public void TestIfThenElse(bool condition, string expected)
    {
        var test = condition ? "#t" : "#f";
        var result = driver.Eval($"(if {test} (System.String.Concat \"true \" \"path\") \"false path\")");
        Assert.That(result, Is.EqualTo(expected));
    }
}
