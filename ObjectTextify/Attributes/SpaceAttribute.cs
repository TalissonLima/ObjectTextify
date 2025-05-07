namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Property)]
public class SpaceAttribute(SpacePosition position = SpacePosition.Right) : Attribute
{
    public SpacePosition Position { get; } = position;
}