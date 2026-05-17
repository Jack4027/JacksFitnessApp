namespace JacksFitnessApp.Infrastructure.Services;

public class OpenFoodFactsProduct
{
    [System.Text.Json.Serialization.JsonPropertyName("_id")]
    public string? Id { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("code")]
    public string? Code { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("product_name")]
    public string? ProductName { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("brands")]
    public string? Brands { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("nutriments")]
    public OpenFoodFactsNutriments? Nutriments { get; set; }
}
