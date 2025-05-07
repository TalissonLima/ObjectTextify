namespace ObjectTextify.Tests;

public class TemplateFormattingTests
{
    [Fact]
    public void Format_WithTemplate_ShouldRenderInterpolatedValues()
    {
        var person = new PersonWithTemplate
        {
            Name = "João",
            Age = 30,
            Address = new AddressWithTemplate
            {
                Street = "Av. Paulista",
                City = "São Paulo"
            }
        };

        var result = TextFormatter.Format(person);

        Assert.Contains("Nome: João", result);
        Assert.Contains("Idade: 30", result);
        Assert.Contains("Rua: Av. Paulista", result);
        Assert.Contains("Cidade: São Paulo", result);
    }

    [Fact]
    public void Format_WithNullValues_ShouldOutputEmptyPlaceholders()
    {
        var person = new PersonWithTemplate
        {
            Name = null,
            Age = 0,
            Address = new AddressWithTemplate
            {
                Street = null,
                City = null
            }
        };

        var result = TextFormatter.Format(person);

        Assert.Contains("Nome: ", result);
        Assert.Contains("Idade: 0", result);
        Assert.Contains("Rua: ", result);
        Assert.Contains("Cidade: ", result);
    }

    [Fact]
    public void Format_WithStrictMode_ShouldThrowIfPlaceholdersRemain()
    {
        var person = new PersonWithBrokenTemplate
        {
            Name = "João"
        };

        TextFormatter.Options.StrictMode = true;
        Assert.Throws<InvalidOperationException>(() => TextFormatter.Format(person));
        TextFormatter.Options.StrictMode = false;
    }
}
