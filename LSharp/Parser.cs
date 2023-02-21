using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp
{
    internal class Parser
    {
        internal IEnumerable<List<object>> Parse(IEnumerable<Token> tokenisedInput)
        {
            Stack<List<object>> stack = new Stack<List<object>>();
            foreach (var token in tokenisedInput)
            {
                switch (token.Type)
                {
                    case TokenType.OpenParen:
                        stack.Push(new List<object>());
                        break;
                    case TokenType.CloseParen:
                        var sublist = stack.Pop();
                        if (stack.Count > 0)
                        {
                            stack.Peek().Add(sublist);
                        }
                        else
                        {
                            yield return sublist;
                        }
                        break;
                    default:
                        stack.Peek().Add(token);
                        break;
                }
            }
        }
    }
}
