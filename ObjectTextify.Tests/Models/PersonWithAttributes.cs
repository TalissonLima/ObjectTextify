namespace ObjectTextify.Tests;

public class PersonWithAttributes
{
    [Caption("Nome", ": ", 15, '-')]
    public string? Name { get; set; }

    [FixedLength(10, TextAlign.Left, '*')]
    public string? City { get; set; }

    [Affix("<<", ">>")]
    public decimal Salary { get; set; }

    [Format("yyyy-MM-dd")]
    public DateTime BirthDate { get; set; }

    [LineBreak(2)]
    public string? Description { get; set; }

    [Space(SpacePosition.Left)]
    public string? Title { get; set; }
}
