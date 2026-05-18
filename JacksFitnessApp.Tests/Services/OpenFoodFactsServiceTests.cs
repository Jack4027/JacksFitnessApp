using FluentAssertions;

namespace JacksFitnessApp.Tests.Services;

[TestFixture]
public class OpenFoodFactsServiceTests
{
    [Test]
    public void ParseDecimal_WhenValueIsNull_ReturnsZero()
    {
        var result = OpenFoodFactsServiceTestHelper.ParseDecimal(null);
        result.Should().Be(0);
    }

    [Test]
    public void ParseDecimal_WhenValueIsNumericString_ReturnsDecimal()
    {
        var result = OpenFoodFactsServiceTestHelper.ParseDecimal("165.5");
        result.Should().Be(165.5m);
    }

    [Test]
    public void ParseDecimal_WhenValueIsInteger_ReturnsDecimal()
    {
        var result = OpenFoodFactsServiceTestHelper.ParseDecimal(165);
        result.Should().Be(165m);
    }

    [Test]
    public void ParseDecimal_WhenValueIsInvalidString_ReturnsZero()
    {
        var result = OpenFoodFactsServiceTestHelper.ParseDecimal("not a number");
        result.Should().Be(0);
    }

    [Test]
    public void ClampMacro_WhenValueExceedsMax_ReturnsMax()
    {
        var result = OpenFoodFactsServiceTestHelper.ClampMacro(950, 0, 900);
        result.Should().Be(900);
    }

    [Test]
    public void ClampMacro_WhenValueBelowMin_ReturnsMin()
    {
        var result = OpenFoodFactsServiceTestHelper.ClampMacro(-10, 0, 900);
        result.Should().Be(0);
    }

    [Test]
    public void ClampMacro_WhenValueWithinRange_ReturnsValue()
    {
        var result = OpenFoodFactsServiceTestHelper.ClampMacro(165, 0, 900);
        result.Should().Be(165);
    }

    [Test]
    public void Sanitise_WhenInputContainsHtmlTags_RemovesThem()
    {
        var result = OpenFoodFactsServiceTestHelper.Sanitise("<b>Chicken</b> Breast");
        result.Should().Be("Chicken Breast");
    }

    [Test]
    public void Sanitise_WhenInputContainsScriptTag_RemovesIt()
    {
        var result = OpenFoodFactsServiceTestHelper.Sanitise("<script>alert('xss')</script>Chicken");
        result.Should().Be("Chicken");
    }

    [Test]
    public void Sanitise_WhenInputHasNoHtml_ReturnsUnchanged()
    {
        var result = OpenFoodFactsServiceTestHelper.Sanitise("Chicken Breast");
        result.Should().Be("Chicken Breast");
    }
}