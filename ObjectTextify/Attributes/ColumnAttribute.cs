namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Property)]
public class ColumnAttribute(string text, TextAlign align = TextAlign.Left) : Attribute
{
    public string Text { get; } = text;
    public TextAlign Align { get; } = align;
}