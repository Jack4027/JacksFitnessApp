namespace JacksFitnessApp.Infrastructure.Services;

public class OpenFoodFactsNutriments
{
    [System.Text.Json.Serialization.JsonPropertyName("energy-kcal_100g")]
    public object? EnergyKcal100g { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("proteins_100g")]
    public object? Proteins100g { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("carbohydrates_100g")]
    public object? Carbohydrates100g { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("fat_100g")]
    public object? Fat100g { get; set; }

    [System.Text.Json.Serialization.JsonPropertyName("fiber_100g")]
    public object? Fiber100g { get; set; }
}