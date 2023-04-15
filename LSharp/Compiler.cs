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
                return Expression.Lambda(Expression.Call(method, arguments.Skip(1)));
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
            var fullname = ((ParameterExpression)arguments[0]).Name;
            var parts = fullname.Split(".");
            var typename = String.Join('.', parts.SkipLast(1));
            var type = Type.GetType(typename);
            if (type == null)
            {
                throw new ArgumentException($"unknown type: {typename}");
            }
            var leafname = parts[^1];
            var typesOfArguments = arguments.Skip(1).Select(_ => typeof(object)).ToArray();
            var method = type.GetMethod(leafname, typesOfArguments);
            if (method == null)
            {
                throw new ArgumentException($"unknown method: {leafname} with args: {string.Join(',', (object[])typesOfArguments)}");
            }
            return method;
        }
    }
}