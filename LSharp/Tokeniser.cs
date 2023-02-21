using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LSharp
{
    enum TokenType
    {
        None, String, Number, OpenParen, CloseParen, Symbol
    }
    class Token
    { 
        internal TokenType Type;
        internal object? Value;
    }

    class Tokeniser
    {
        private StringBuilder tokenChars = new StringBuilder();

        internal IEnumerable<Token> Tokenise(string input)
        {
            foreach (char c in input)
            {
                switch (c)
                {
                    case '(':
                        if (IsStarted())
                            yield return NextToken();
                        yield return new Token { Type = TokenType.OpenParen };
                        break;
                    case ')':
                        if (IsStarted())
                            yield return NextToken();
                        yield return new Token { Type = TokenType.CloseParen };
                        break;
                    case ' ':
                        if (IsStarted())
                            yield return NextToken();
                        break;
                    default:
                        tokenChars.Append(c);
                        break;
                }
            }
            if (IsStarted())
                yield return NextToken();
        }

        internal Token NextToken()
        {
            if (tokenChars.Length > 0)
            {
                var value = tokenChars.ToString();
                Token token;
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    token = new Token { Type = TokenType.String, Value = value[1..^1] };
                }
                else if (Regex.IsMatch(value, @"^[0-9.]+$"))
                {
                    token = new Token { Type = TokenType.Number, Value = Double.Parse(value) };
                }
                else
                {
                    token = new Token { Type = TokenType.Symbol, Value = value };
                }
                tokenChars.Clear();
                return token;
            }
            throw new ArgumentException("empty token");
        }

        internal void Append(char c)
        {
            tokenChars.Append(c);
        }

        internal bool IsStarted()
        {
            return tokenChars.Length > 0;
        }
    }
}
