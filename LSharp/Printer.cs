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
            Print(Console.Out, list);
        }

        internal void Print(TextWriter stream, List<object> list)
        {
            stream.Write("(");
            bool previousItemWasRealToken = false;
            foreach (var item in list)
            {
                if (item is Token token)
                {
                    if (previousItemWasRealToken)
                        stream.Write(" ");
                    switch (token.Type)
                    {
                        case TokenType.String:
                            stream.Write("\"");
                            stream.Write(token.Value);
                            stream.Write("\"");
                            break;
                        case TokenType.Number:
                            stream.Write(token.Value);
                            break;
                        case TokenType.Symbol:
                            stream.Write(token.Value);
                            break;
                        default:
                            throw new ArgumentException("unknown token");
                    }
                    previousItemWasRealToken = true;
                }
                else if (item is List<object> sublist)
                {
                    Print(stream, sublist);
                    previousItemWasRealToken = false;
                }
            }
            stream.Write(')');
        }
    }
}
