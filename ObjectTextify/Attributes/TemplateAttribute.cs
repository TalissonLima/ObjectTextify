namespace ObjectTextify;

[AttributeUsage(AttributeTargets.Class)]
public class TemplateAttribute(string template) : Attribute
{
    public string Template { get; } = template;
}
