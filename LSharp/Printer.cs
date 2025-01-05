using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LSharp;

internal class Printer
{
    internal void Print(ISymbolicExpression obj)
    {
        Print(Console.Out, obj);
    }

    internal void Print(TextWriter stream, ISymbolicExpression obj)
    {
        if (obj is ListExpression list)
        {
            stream.Write("(");
            bool addSpace = false;
            foreach (var item in list)
            {
                if (addSpace)
                {
                    stream.Write(" ");
                }
                Print(stream, item);
                addSpace = true;
            }
            stream.Write(')');
        }
        else
        {
            if (obj is Token token)
            {
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
                    case TokenType.Boolean:
                        stream.Write("#");
                        stream.Write((bool)token.Value ? "t" : "f");
                        break;
                    case TokenType.Symbol:
                        stream.Write(token.Value);
                        break;
                    default:
                        throw new ArgumentException($"unknown token: {obj}");
                }                
            }
            else
            {
                throw new ArgumentException("unknown value: {obj}");
            }
        }
    }
}
