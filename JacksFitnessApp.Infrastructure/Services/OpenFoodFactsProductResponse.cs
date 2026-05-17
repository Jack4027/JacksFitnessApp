namespace JacksFitnessApp.Infrastructure.Services;

public class OpenFoodFactsProductResponse
{
    [System.Text.Json.Serialization.JsonPropertyName("product")]
    public OpenFoodFactsProduct? Product { get; set; }
}
