namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Property)]
public class FormatAttribute(string format) : Attribute
{
    public string Format { get; } = format;
}
