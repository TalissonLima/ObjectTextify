namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class IndentAttribute(int indent = 4) : Attribute
{
    public int Indent { get; } = indent;
}
