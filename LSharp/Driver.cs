using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;

// defun, let, lambda, apply, eval, quote, car, cdr
// C# version of hylang
// Parser, tokenizer, compiler
// number, string, symbol
// Examples:
// 
// [1]
// (defun foo (x) (+ x 1))
// => dynamic foo (dynamic x) { return x + 1; }
// 
// [2]
// (Console.WriteLine "Hello, World!")
// 

namespace LSharp
{
    class Driver
    {
        public static void Main(string[] args)
        {
            foreach (var list in new Parser().Parse(new Tokeniser().Tokenise("(defun foo (x) (+ x 1)) (foo 10)")))
            {
                new Printer().Print(list);
            }
        }
    } 
}

