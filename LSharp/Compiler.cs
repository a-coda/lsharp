using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LSharp
{
    internal class Compiler
    {
        public Compiler()
        {
        }

        internal Expression CompileTopLevel(object expr)
        {
            return Expression.Lambda(Compile(expr));
        }


        internal Expression Compile(object sexpr)
        {
            if (sexpr is List<object> list)
            {
                List<Expression> arguments = new List<Expression>();
                foreach (var item in list)
                {
                    arguments.Add(Compile(item));
                }
                var method = CompileMethod(arguments);
                return Expression.Call(method, arguments.Skip(1));
            }
            else
            {
                if (sexpr is Token token)
                {
                    switch (token.Type)
                    {
                        case TokenType.String:
                            return Expression.Constant(token.Value);
                        case TokenType.Number:
                            return Expression.Constant(token.Value);
                        case TokenType.Symbol:
                            return Expression.Variable(typeof(object), (string)token.Value);
                        default:
                            throw new ArgumentException("unknown token");
                    }
                }
                else
                {
                    throw new ArgumentException($"unknown sexpr: {sexpr}");
                }
            }
        }

        private MethodInfo CompileMethod(List<Expression> arguments)
        {
            var (typeName, functionName) = ParseFunctionName(arguments);
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException($"unknown type: {typeName}");
            }
            var method = FindMethod(type, functionName, arguments);
            if (method == null)
            {
                throw new ArgumentException($"unknown method: {functionName}");
            }
            return method;
        }

        private MethodInfo? FindMethod(Type type, string functionName, List<Expression> arguments)
        {
            MethodInfo? method = null;
            foreach (var mapper in MapTypesOfArguments())
            {
                var typesOfArguments = arguments.Skip(1).Select(mapper).ToArray();
                Console.WriteLine($"\ntrying {string.Join(',', (object[])typesOfArguments)} ..");
                method = type.GetMethod(functionName, typesOfArguments);
                if (method != null)
                    break;
            }
            return method;
        }

        private IEnumerable<Func<Expression,Type>> MapTypesOfArguments()
        {
            yield return arg => arg.Type;
            yield return _ => typeof(object); // default kind of method
        }

        private (string, string) ParseFunctionName(List<Expression> arguments)
        {
            var fullName = ((ParameterExpression)arguments[0]).Name;
            var parts = fullName.Split(".");
            var typeName = String.Join('.', parts.SkipLast(1));
            var functionName = parts[^1];
            return (typeName, functionName);
        }
    }
}