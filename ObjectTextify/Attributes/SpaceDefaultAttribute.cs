namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Class)]
public class SpaceDefaultAttribute(SpacePosition position = SpacePosition.Right) : SpaceAttribute(position) { }