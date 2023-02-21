using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp
{
    class Tokeniser
    {
        StringBuilder token = new StringBuilder();

        internal IEnumerable<string> Tokenise(string input)
        {
            foreach (char c in input)
            {
                switch (c)
                {
                    case '(':
                        if (IsStarted())
                            yield return NextToken();
                        yield return "(";
                        break;
                    case ')':
                        if (IsStarted())
                            yield return NextToken();
                        yield return ")";
                        break;
                    case ' ':
                        if (IsStarted())
                            yield return NextToken();
                        break;
                    default:
                        token.Append(c);
                        break;
                }
            }
            if (IsStarted())
                yield return NextToken();
        }

        internal string NextToken()
        {
            if (token.Length > 0)
            {
                var value = token.ToString();
                token.Clear();
                return value;
            }
            return "";
        }

        internal void Append(char c)
        {
            token.Append(c);
        }

        internal bool IsStarted()
        {
            return token.Length > 0;
        }
    }
}
