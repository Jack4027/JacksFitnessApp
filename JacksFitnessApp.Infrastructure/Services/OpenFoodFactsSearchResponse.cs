namespace JacksFitnessApp.Infrastructure.Services;

public class OpenFoodFactsSearchResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("products")]
    public List<OpenFoodFactsProduct>? Products { get; set; }
}
