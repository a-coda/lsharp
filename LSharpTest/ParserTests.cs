using LSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharpTest
{
    internal class ParserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestParser() 
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
                new Token { Type = TokenType.OpenParen },
                new Token { Type = TokenType.Symbol, Value = "foo" },
                new Token { Type = TokenType.Number, Value = 10.05 },
                new Token { Type = TokenType.CloseParen },
                new Token { Type = TokenType.OpenParen },
                new Token { Type = TokenType.Symbol, Value = "concat" },
                new Token { Type = TokenType.String, Value = "a" },
                new Token { Type = TokenType.String, Value = "b" },
                new Token { Type = TokenType.CloseParen }
            };
            var actual = new Parser().Parse(input).ToList();
            var expected = new ListExpression
            {
                new ListExpression
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
                },
                new ListExpression()
                {
                    new Token { Type = TokenType.Symbol, Value = "foo" },
                    new Token { Type = TokenType.Number, Value = 10.05 },
                },
                new ListExpression()
                {
                    new Token { Type = TokenType.Symbol, Value = "concat" },
                    new Token { Type = TokenType.String, Value = "a" },
                    new Token { Type = TokenType.String, Value = "b" },
                }
            };
            Assert.That(actual, Is.EquivalentTo(expected));
        }

    }
}
