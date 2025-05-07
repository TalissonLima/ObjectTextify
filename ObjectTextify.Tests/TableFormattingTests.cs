namespace ObjectTextify.Tests;

public class TableFormattingTests
{
    [Fact]
    public void Format_PersonWithAddressTable_ShouldRenderAddressListAsTable()
    {
        var person = new PersonWithAddressTable
        {
            Name = "John Doe",
            Addresses =
            [
                new() { Street = "Main St", City = "New York" },
                new() { Street = "2nd Ave", City = "Chicago" }
            ]
        };

        var result = TextFormatter.Format(person);

        Assert.Contains("Street Name", result);
        Assert.Contains("City Name", result);

        Assert.Contains("Main St", result);
        Assert.Contains("New York", result);
        Assert.Contains("2nd Ave", result);
        Assert.Contains("Chicago", result);

        Assert.Contains("-", result);
        Assert.Contains("|", result);
    }
}