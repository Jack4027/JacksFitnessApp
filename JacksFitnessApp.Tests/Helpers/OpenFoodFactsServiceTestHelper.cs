namespace JacksFitnessApp.Tests.Services;

public static class OpenFoodFactsServiceTestHelper
{
    public static decimal ParseDecimal(object? value)
    {
        if (value == null) return 0;
        return decimal.TryParse(value.ToString(), out var result) ? result : 0;
    }

    public static decimal ClampMacro(decimal value, decimal min, decimal max)
    {
        return Math.Clamp(value, min, max);
    }

    public static string Sanitise(string input)
    {
        var noScript = System.Text.RegularExpressions.Regex.Replace(
            input, "<script.*?>.*?</script>", string.Empty,
            System.Text.RegularExpressions.RegexOptions.Singleline |
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        var noTags = System.Text.RegularExpressions.Regex.Replace(noScript, "<.*?>", string.Empty);

        return noTags.Trim();
    }
}