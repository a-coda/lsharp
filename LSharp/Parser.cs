using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp
{
    internal interface ISymbolicExpression
    {

    }
        
    internal class ListExpression: ISymbolicExpression, IEnumerable<ISymbolicExpression>
    {
        private List<ISymbolicExpression> elements = new List<ISymbolicExpression>();

        public ListExpression() { }

        public void Add(ISymbolicExpression expr)
        {
            elements.Add(expr);
        }

        public ISymbolicExpression this[int index]  
        {
            get { return elements[index]; }
        }

        public IEnumerator<ISymbolicExpression> GetEnumerator()
        { 
            return elements.GetEnumerator(); 
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }
    }

    internal class Parser
    {
        internal IEnumerable<ListExpression> Parse(IEnumerable<Token> tokenisedInput)
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
