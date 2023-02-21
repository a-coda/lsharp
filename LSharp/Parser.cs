using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp
{
    internal class Parser
    {
        internal IEnumerable<List<object>> Parse(IEnumerable<string> tokenisedInput)
        {
            Stack<List<object>> stack = new Stack<List<object>>();
            foreach (var token in tokenisedInput)
            {
                switch (token)
                {
                    case "(":
                        stack.Push(new List<object>());
                        break;
                    case ")":
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
