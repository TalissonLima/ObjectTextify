namespace ObjectTextify.Tests;

[Template("Nome: {Name}, Idade: {Age}, Rua: {Address.Street}, Cidade: {Address.City}")]
public class PersonWithTemplate
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public AddressWithTemplate? Address { get; set; }
}