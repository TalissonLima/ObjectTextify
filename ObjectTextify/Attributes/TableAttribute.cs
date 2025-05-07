namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
public class TableAttribute(bool includeDivider = false, char divider = '-', bool dividerSpace = true, char columnSeparator = ' ') : Attribute
{
    public bool IncludeDivider { get; } = includeDivider;
    public char Divider { get; } = divider;
    public bool DividerSpace { get; } = dividerSpace;
    public char ColumnSeparator { get; } = columnSeparator;
}