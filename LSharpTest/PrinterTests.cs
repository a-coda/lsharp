using LSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharpTest
{
    internal class PrinterTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestPrinter()
        {
            var input = new List<List<object>>
            {
                new List<object>
                {
                    new Token { Type = TokenType.Symbol, Value = "defun" },
                    new Token { Type = TokenType.Symbol, Value = "foo" },
                    new List<object>
                    {
                        new Token { Type = TokenType.Symbol, Value = "x" },
                    },
                    new List<object>
                    {
                        new Token { Type = TokenType.Symbol, Value = "+" },
                        new Token { Type = TokenType.Symbol, Value = "x" },
                        new Token { Type = TokenType.Number, Value = 1.0 },
                    }
                },
                new List<object>()
                {
                    new Token { Type = TokenType.Symbol, Value = "foo" },
                    new Token { Type = TokenType.Number, Value = 10.05 },
                },
                new List<object>()
                {
                    new Token { Type = TokenType.Symbol, Value = "concat" },
                    new Token { Type = TokenType.String, Value = "a" },
                    new Token { Type = TokenType.String, Value = "b" },
                }
            };
            string actual;
            using (var writer = new StringWriter())
            {
                foreach (var list in input)
                {
                    new Printer().Print(writer, list);
                }
                actual = writer.ToString();
            }
            var expected = "(defun foo (x) (+ x 1))(foo 10.05)(concat \"a\" \"b\")";
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
