namespace ObjectTextify.Tests;

public class PersonWithAddressTable
{
    public string? Name { get; set; }

    [Table(includeDivider: true, columnSeparator: '|')]
    public List<AddressWithColumns> Addresses { get; set; } = [];
}