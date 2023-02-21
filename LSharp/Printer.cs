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
            bool previousItemWasString = false;
            foreach (var item in list)
            {
                if (item is string)
                {
                    if (previousItemWasString)
                        Console.Write(" ");
                    Console.Write(item);
                    previousItemWasString = true;
                }
                else if (item is List<object> sublist)
                {
                    Print(sublist);
                    previousItemWasString = false;
                }
            }
            Console.Write(')');
        }
    }
}
