namespace LSharp;

class Token : ISymbolicExpression
{ 
    internal TokenType Type;
    internal object Value = "";

    public override bool Equals(object? obj)
    {
        return obj is Token t && Type.Equals(t.Type) && object.Equals(Value, t.Value);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Type.GetHashCode(), Value?.GetHashCode());
    }

    public override string ToString()
    {
        var value = Value?.ToString() ?? "";
        return $"<{Type} {value}>";
    }
}
