namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class LineBreakAttribute : Attribute
{
    public int Lines { get; }

    public LineBreakAttribute(int lines = 1)
    {
        if (lines <= 0)
            throw new ArgumentException("lines must be greater than 0");

        Lines = lines;
    }
}
