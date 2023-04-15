using LSharp;

namespace LSharpTest
{
    public class TokeniserTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestTokeniser()
        {
            var tokeniser = new Tokeniser();
            var input = "(defun foo (x) (+ x 1)) (foo 10.05) (concat \"a\" \"b\")";
            var actual = tokeniser.Tokenise(new StringReader(input)).ToArray();
            var expected = new Token[] 
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
             Assert.That(actual, Is.EquivalentTo(expected));
        }
    }
}