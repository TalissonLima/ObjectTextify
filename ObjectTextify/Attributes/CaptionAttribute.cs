namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Property)]
public class CaptionAttribute : Attribute
{
    public string Text { get; }
    public int Length { get; }
    public char Format { get; }

    public CaptionAttribute(string text, string separator = ": ", int length = 0, char format = ' ')
    {
        if (length < 0)
            throw new ArgumentException("length must be greater than or equal to 0");

        Text = text + separator;
        Length = length;
        Format = format;
    }
}
