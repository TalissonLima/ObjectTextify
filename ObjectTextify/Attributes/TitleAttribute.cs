namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class TitleAttribute : Attribute
{
    public string Text { get; }
    public int Length { get; }
    public TextAlign Alignment { get; }
    public char Format { get; }

    public TitleAttribute(string text, int length = 0, TextAlign alignment = TextAlign.Center, char format = ' ')
    {
        if (length < 0)
            throw new ArgumentException("length must be greater than or equal to 0");

        Text = text;
        Length = length;
        Alignment = alignment;
        Format = format;
    }
}
