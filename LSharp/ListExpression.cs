using System.Collections;

namespace LSharp;

internal class ListExpression: ISymbolicExpression, IEnumerable<ISymbolicExpression>
{
    private List<ISymbolicExpression> elements = new List<ISymbolicExpression>();

    public ListExpression() { }

    public void Add(ISymbolicExpression expr)
    {
        elements.Add(expr);
    }

    public ISymbolicExpression this[int index]  
    {
        get { return elements[index]; }
    }

    public IEnumerator<ISymbolicExpression> GetEnumerator()
    { 
        return elements.GetEnumerator(); 
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return elements.GetEnumerator();
    }
}
