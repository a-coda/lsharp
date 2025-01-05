using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp;

internal class Parser
{
    internal ListExpression? Parse(IEnumerable<Token> tokenisedInput)
    {
        Stack<ListExpression> stack = new Stack<ListExpression>();
        foreach (var token in tokenisedInput)
        {
            switch (token.Type)
            {
                case TokenType.OpenParen:
                    stack.Push(new ListExpression());
                    break;
                case TokenType.CloseParen:
                    var sublist = stack.Pop();
                    if (stack.Count > 0)
                    {
                        stack.Peek().Add(sublist);
                    }
                    else
                    {
                        return sublist;
                    }
                    break;
                default:
                    stack.Peek().Add(token);
                    break;
            }
        }
        return null;
    }
}
