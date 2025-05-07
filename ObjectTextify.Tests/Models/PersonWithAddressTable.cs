namespace ObjectTextify.Tests;

public class AddressWithColumns
{
    [Column("Street Name")]
    public string? Street { get; set; }

    [Column("City Name")]
    public string? City { get; set; }
}