namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Property)]
public class AffixAttribute(string left = "", string right = "") : Attribute
{
    public string Left { get; } = left;
    public string Right { get; } = right;
}