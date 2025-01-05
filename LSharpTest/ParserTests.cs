using LSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharpTest;

internal class ParserTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestParseDefun()
    {
        var input = new Token[]
        {
            new Token { Type = TokenType.OpenParen },
            new Token { Type = TokenType.Symbol, Value = "defun" },
            new Token { Type = TokenType.Symbol, Value = "foo" },
            new Token { Type = TokenType.OpenParen },
            new Token { Type = TokenType.Symbol, Value = "x" },
            new Token { Type = TokenType.CloseParen },
            new Token { Type = TokenType.OpenParen },
            new Token { Type = TokenType.Symbol, Value = "+" },
            new Token { Type = TokenType.Symbol, Value = "x" },
            new Token { Type = TokenType.Number, Value = 1.0 },
            new Token { Type = TokenType.CloseParen },
            new Token { Type = TokenType.CloseParen },
        };
        var expected = new ListExpression
        {
            new Token { Type = TokenType.Symbol, Value = "defun" },
            new Token { Type = TokenType.Symbol, Value = "foo" },
            new ListExpression
            {
                new Token { Type = TokenType.Symbol, Value = "x" },
            },
            new ListExpression
            {
                new Token { Type = TokenType.Symbol, Value = "+" },
                new Token { Type = TokenType.Symbol, Value = "x" },
                new Token { Type = TokenType.Number, Value = 1.0 },
            }
        };
        var actual = new Parser().Parse(input);
        Assert.That(actual, Is.EquivalentTo(expected));
    }
}
