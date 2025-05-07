namespace ObjectTextify.Tests;

[Template("Nome: {Name}, Invalido: {NotExists}")]
public class PersonWithBrokenTemplate
{
    public string? Name { get; set; }
}