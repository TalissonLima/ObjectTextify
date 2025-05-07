namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Property)]
public class FixedLengthAttribute : Attribute
{
    public int Length { get; }
    public TextAlign Alignment { get; }
    public char Format { get; }

    public FixedLengthAttribute(int length, TextAlign alignment = TextAlign.Left, char format = ' ')
    {
        if (length < 2)
            throw new ArgumentException("length must be greater than or equal to 2");

        Length = length;
        Alignment = alignment;
        Format = format;
    }
}