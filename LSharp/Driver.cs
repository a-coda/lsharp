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
        Tokeniser tokeniser = new Tokeniser();
        Parser parser = new Parser();
        Printer writer = new Printer();
        Compiler compiler = new Compiler();

        public static void Main(string[] args)
        {
            var filename = args[0];
            new Driver().Evaluate(filename);
        }

        public void Evaluate(string filename)
        {
            using (var stream = new StreamReader(filename))
            {
                var tokens = tokeniser.Tokenise(stream);
                foreach (var sexpr in parser.Parse(tokens))
                {
                    writer.Print(sexpr);
                    var expression = compiler.Compile(sexpr);
                    Console.Write("\n=> ");
                    Console.WriteLine(((LambdaExpression)expression).Compile().DynamicInvoke());
                }
            }
        }
    } 
}

