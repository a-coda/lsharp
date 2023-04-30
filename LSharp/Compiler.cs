using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Numerics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace LSharp
{
    internal class Compiler
    {
        private static readonly Dictionary<string, Func<object[], object>> definitions = new() { };

        public Compiler()
        {
        }

        internal Expression CompileTopLevel(object expr)
        {
            return Expression.Lambda(Compile(expr));
        }


        internal Expression Compile(object sexpr)
        {
            return sexpr switch
            {
                List<object> list => list[0] switch
                {
                    Token { Type: TokenType.Symbol, Value: "if" } => CompileIf(CompileForm(list)),
                    Token { Type: TokenType.Symbol, Value: "defun" } => CompileDefun(list),
                    _ => CompileFunctionCall(CompileForm(list))
                },
                Token { Type: TokenType.String } token => Expression.Constant(token.Value),
                Token { Type: TokenType.Number } token => Expression.Constant(token.Value),
                Token { Type: TokenType.Boolean } token => Expression.Constant(token.Value),
                Token { Type: TokenType.Symbol } token => Expression.Variable(typeof(object), (string)token.Value),
                _ => throw new ArgumentException($"unknown expression: {sexpr}")
            };
        }

        private Expression CompileIf(List<Expression> list)
        {
            return list.Count switch
            {
                4 => Expression.Condition(list[1], list[2], list[3]),
                _ => throw new InvalidOperationException($"if-statement must have 3 arguments, got {list.Count}")
            };
        }

        private Expression CompileDefun(List<object> list)
        {
            return CompileFunctionCall(
                new List<Expression>() {
                    Expression.Variable(typeof(object), "LSharp.Compiler.DefineFunction"),
                    Expression.Constant(((Token)list[1]).Value), // name
                    Expression.Lambda(Compile(list[3]), CompileFunctionParameters(list[2])) }); // body
        }

        private IEnumerable<ParameterExpression> CompileFunctionParameters(object sexpr)
        {
            if (sexpr is List<object> list)
            {
                var parameters = new List<ParameterExpression>();
                foreach (var x in list)
                {
                    if (x is Token { Type: TokenType.Symbol } token)
                    {
                        parameters.Add(Expression.Variable(typeof(object), (string)token.Value));
                    }
                    else
                    {
                        throw new ArgumentException($"unexpected parameter: {x}");
                    }   
                }
                return parameters;
            }
            else
            {
                throw new ArgumentException($"unexpected parameter expression: {sexpr}");
            }
        }

        // Used dynamically
        private void DefineFunction(object name, object function)
        {
            Console.WriteLine($"Defining {name} as {function}");
        }

        private List<Expression> CompileForm(List<object> list)
        {
            List<Expression> arguments = new List<Expression>();
            foreach (var item in list)
            {
                arguments.Add(Compile(item));
            }
            return arguments;
        }

        private Expression CompileFunctionCall(List<Expression> list)
        {
            var method = CompileMethod(list);
            return Expression.Call(method, list.Skip(1));
        }

        private MethodInfo CompileMethod(List<Expression> arguments)
        {
            var (typeName, functionName) = ParseFunctionName(arguments);
            var type = Type.GetType(typeName);
            if (type == null)
            {
                throw new ArgumentException($"unknown type: \"{typeName}\"");
            }
            var (method, signaturesTried) = FindMethod(type, functionName, arguments);
            if (method == null)
            {
                DumpSignaturesTried(signaturesTried);
                DumpCandidateMethods(type, functionName);
                throw new ArgumentException($"unknown method: {functionName}");
            }
            return method;
        }

        private (MethodInfo?, IEnumerable<Type[]>) FindMethod(Type type, string functionName, List<Expression> arguments)
        {
            MethodInfo? method = null;
            List<Type[]> signaturesTried = new ();
            foreach (var mapper in MapTypesOfArguments())
            {
                var typesOfArguments = arguments.Skip(1).Select(mapper).ToArray();
                signaturesTried.Add(typesOfArguments);
                method = type.GetMethod(functionName, typesOfArguments);
                if (method != null)
                    break;
            }
            return (method, signaturesTried);
        }

        private void DumpSignaturesTried(IEnumerable<Type[]> signaturesTried)
        {
            foreach (var typesOfArguments in signaturesTried)
            {
                Console.WriteLine($"\nTried: {string.Join(',', (object[])typesOfArguments)} ..");
            }
        }
        private void DumpCandidateMethods(Type type, string functionName)
        {
            foreach (var actualMethod in type.GetMethods())
            {
                if (actualMethod.Name == functionName)
                {
                    Console.WriteLine(actualMethod.Name + "(" + string.Join(',', actualMethod.GetParameters().Select(p => p.ParameterType.ToString()).ToArray()) + ")");
                }
            }
        }

        private IEnumerable<Func<Expression,Type>> MapTypesOfArguments()
        {
            yield return arg => arg.Type;
            yield return _ => typeof(object); // default kind of method
        }

        private (string, string) ParseFunctionName(List<Expression> arguments)
        {
            var fullName = ((ParameterExpression)arguments[0]).Name;
            if (fullName == null)
                throw new ArgumentNullException(fullName);
            var parts = fullName.Split(".");
            var typeName = String.Join('.', parts.SkipLast(1));
            var functionName = parts[^1];
            return (typeName, functionName);
        }
    }
}