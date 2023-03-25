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
            var t = new Tokeniser();
            var p = new Parser();
            var w = new Printer();
            var c = new Compiler();
            var tokens = t.Tokenise(args[0]);
            foreach (var sexpr in p.Parse(tokens))
            {
                w.Print(sexpr);
                var expression = c.Compile(sexpr);
                Console.Write("\n=> ");
                Console.WriteLine(((LambdaExpression)expression).Compile().DynamicInvoke());
            }
        }
    } 
}

