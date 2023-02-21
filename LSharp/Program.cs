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

class Token
{
    StringBuilder token = new StringBuilder();
    internal string NextToken()
    {
        if (token.Length > 0)
        {
            var value = token.ToString();
            token.Clear();
            return value;
        }
        return "";
    }

    internal void Append(char c)
    { 
        token.Append(c);
    }

    internal bool IsStarted()
    {
        return token.Length > 0;
    }
}
class LSharp
{
    public static void Main(string[] args)
    {
        foreach (var list in Parse(Tokenise("(defun foo (x) (+ x 1)) (foo 10)")))
        {
            Print(list);
        }
    }
    
    static IEnumerable<string> Tokenise(string input)
    {
        Token token = new Token();
        foreach (char c in input)
        {
            switch (c)
            {
                case '(':
                    if (token.IsStarted())
                        yield return token.NextToken();
                    yield return "(";
                    break;
                case ')':
                    if (token.IsStarted())
                        yield return token.NextToken();
                    yield return ")";
                    break;
                case ' ':
                    if (token.IsStarted())
                        yield return token.NextToken();
                    break;
                default:
                    token.Append(c);
                    break;
            }
        }
        if (token.IsStarted())
            yield return token.NextToken();
    }

    internal static void Print(List<object> list)
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
                previousItemWasString= false;
            }
        }
        Console.Write(')');
    }

    internal static IEnumerable<List<object>> Parse(IEnumerable<string> tokenisedInput)
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

