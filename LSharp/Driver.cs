using System.Linq.Expressions;

namespace LSharp;

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
                var expression = compiler.CompileTopLevel(sexpr);
                Console.Write("\n=> ");
                Console.WriteLine(((LambdaExpression)expression).Compile().DynamicInvoke());
            }
        }
    }
} 

