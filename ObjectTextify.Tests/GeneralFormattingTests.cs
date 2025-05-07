using System.Globalization;

namespace ObjectTextify.Tests;

public class GeneralFormattingTests
{
    [Fact]
    public void Format_ShouldApplyCaptionAttributeCorrectly()
    {
        var person = new PersonWithAttributes
        {
            Name = "John Doe"
        };

        var formatted = TextFormatter.Format(person);

        var expected = "Nome: ---------John Doe";
        Assert.Contains(expected, formatted);
    }

    [Fact]
    public void Format_ShouldApplyFixedLengthAttributeCorrectly()
    {
        var person = new PersonWithAttributes
        {
            City = "New York"
        };

        var formatted = TextFormatter.Format(person);

        var expected = "New York**";
        Assert.Contains(expected, formatted);
    }

    [Fact]
    public void Format_ShouldApplyAffixAttributeCorrectly()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        var person = new PersonWithAttributes
        {
            Salary = 5000.75m
        };

        var formatted = TextFormatter.Format(person);

        var expected = "<<5000.75>>";
        Assert.Contains(expected, formatted);
    }

    [Fact]
    public void Format_ShouldApplyFormatAttributeCorrectly()
    {
        var person = new PersonWithAttributes
        {
            BirthDate = new DateTime(1990, 5, 10)
        };

        var formatted = TextFormatter.Format(person);

        var expected = "1990-05-10";
        Assert.Contains(expected, formatted);
    }

    [Fact]
    public void Format_ShouldApplyLineBreakAttributeCorrectly()
    {
        var person = new PersonWithAttributes
        {
            Description = "This is a description"
        };

        var formatted = TextFormatter.Format(person);

        var expected = $"{Environment.NewLine}{Environment.NewLine}This is a description";
        Assert.Contains(expected, formatted);
    }

    [Fact]
    public void Format_ShouldApplySpaceAttributeCorrectly()
    {
        var person = new PersonWithAttributes
        {
            Title = "Mr."
        };

        var formatted = TextFormatter.Format(person);

        var expected = " Mr.";
        Assert.Contains(expected, formatted);
    }

    [Fact]
    public void Format_ShouldApplyAllAttributesCorrectly()
    {
        CultureInfo.CurrentCulture = new CultureInfo("en-US");

        var person = new PersonWithAttributes
        {
            Name = "John Doe",
            City = "New York",
            Salary = 5000.75m,
            BirthDate = new DateTime(1990, 5, 10),
            Description = "This is a description",
            Title = "Mr."
        };

        var formatted = TextFormatter.Format(person);

        Assert.Contains("Nome: ---------John Doe", formatted);
        Assert.Contains("New York**", formatted);
        Assert.Contains("<<5000.75>>", formatted);
        Assert.Contains("1990-05-10", formatted);
        Assert.Contains($"{Environment.NewLine}{Environment.NewLine}This is a description", formatted);
        Assert.Contains(" Mr.", formatted);
    }
}