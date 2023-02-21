using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp
{
    internal class Printer
    {
        internal void Print(List<object> list)
        {
            Console.Write("(");
            bool previousItemWasRealToken = false;
            foreach (var item in list)
            {
                if (item is Token token)
                {
                    if (previousItemWasRealToken)
                        Console.Write(" ");
                    switch (token.Type)
                    {
                        case TokenType.String:
                            Console.Write("\"");
                            Console.Write(token.Value);
                            Console.Write("\"");
                            break;
                        case TokenType.Number:
                            Console.Write(token.Value);
                            break;
                        case TokenType.Symbol:
                            Console.Write(token.Value);
                            break;
                        default:
                            throw new ArgumentException("unknown token");
                    }
                    previousItemWasRealToken = true;
                }
                else if (item is List<object> sublist)
                {
                    Print(sublist);
                    previousItemWasRealToken = false;
                }
            }
            Console.Write(')');
        }
    }
}
