using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace JacksFitnessApp.Infrastructure.Services;

public class AnthropicService : IAnthropicService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AnthropicService> _logger;

    public AnthropicService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<AnthropicService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<string> GetCoachResponseAsync(
        string userMessage,
        List<CoachMessageDto> history,
        UserContextDto userContext)
    {
        var apiKey = _configuration["Anthropic:ApiKey"]
            ?? throw new InvalidOperationException("Anthropic API key not configured");

        var model = _configuration["Anthropic:Model"] ?? "claude-opus-4-5";

        var systemPrompt = BuildSystemPrompt(userContext);
        var messages = BuildMessages(history, userMessage);

        var requestBody = new AnthropicRequest
        {
            Model = model,
            MaxTokens = 1024,
            System = systemPrompt,
            Messages = messages
        };

        var request = new HttpRequestMessage(HttpMethod.Post,
            "https://api.anthropic.com/v1/messages");

        request.Headers.Add("x-api-key", apiKey);
        request.Headers.Add("anthropic-version", "2023-06-01");
        request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            }),
            Encoding.UTF8,
            "application/json");

        try
        {
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseBody = await response.Content
                .ReadFromJsonAsync<AnthropicResponse>();

            return responseBody?.Content?.FirstOrDefault()?.Text
                ?? "Sorry I could not generate a response.";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calling Anthropic API");
            throw;
        }
    }

    private static string BuildSystemPrompt(UserContextDto context)
    {
        var sb = new StringBuilder();

        sb.AppendLine("You are a personal fitness coach with access to the user's training and nutrition data.");
        sb.AppendLine("Be specific, encouraging, and data-driven. Reference the user's actual numbers when relevant.");
        sb.AppendLine("Calibrate response length to the question — short questions get concise answers, analysis requests get detailed responses.");
        sb.AppendLine("Celebrate personal records and progress. Frame weaknesses as opportunities.");
        sb.AppendLine();
        sb.AppendLine("USER FITNESS DATA:");
        sb.AppendLine();

        if (context.RecentWorkouts.Any())
        {
            sb.AppendLine($"Recent workouts ({context.RecentWorkouts.Count} sessions):");
            foreach (var w in context.RecentWorkouts)
            {
                sb.Append($"  - {w.Date}: {w.TotalSets} sets, {w.TotalVolume:F0}kg volume");
                if (w.DurationMinutes.HasValue)
                    sb.Append($", {w.DurationMinutes}min");
                if (w.Exercises.Any())
                    sb.Append($" | {string.Join(", ", w.Exercises)}");
                sb.AppendLine();
            }
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine("Recent workouts: None logged yet.");
            sb.AppendLine();
        }

        if (context.PersonalRecords.Any())
        {
            sb.AppendLine("Personal records:");
            foreach (var pr in context.PersonalRecords)
            {
                sb.AppendLine($"  - {pr.ExerciseName}: {pr.WeightKg}kg × {pr.Reps} reps ({pr.AchievedOn})");
            }
            sb.AppendLine();
        }

        if (context.LatestMetrics != null)
        {
            sb.AppendLine($"Latest body metrics ({context.LatestMetrics.Date}):");
            if (context.LatestMetrics.WeightKg.HasValue)
                sb.AppendLine($"  - Weight: {context.LatestMetrics.WeightKg}kg");
            if (context.LatestMetrics.BodyFatPercentage.HasValue)
                sb.AppendLine($"  - Body fat: {context.LatestMetrics.BodyFatPercentage}%");
            if (context.LatestMetrics.MuscleMassKg.HasValue)
                sb.AppendLine($"  - Muscle mass: {context.LatestMetrics.MuscleMassKg}kg");
            sb.AppendLine();
        }
        else
        {
            sb.AppendLine("Body metrics: None logged yet.");
            sb.AppendLine();
        }

        if (context.TodayNutrition != null)
        {
            sb.AppendLine("Today's nutrition:");
            sb.AppendLine($"  - Calories: {context.TodayNutrition.TotalCalories:F0}kcal");
            sb.AppendLine($"  - Protein: {context.TodayNutrition.TotalProtein:F1}g");
            sb.AppendLine($"  - Carbs: {context.TodayNutrition.TotalCarbs:F1}g");
            sb.AppendLine($"  - Fat: {context.TodayNutrition.TotalFat:F1}g");
            sb.AppendLine($"  - Meals logged: {context.TodayNutrition.MealCount}");
        }
        else
        {
            sb.AppendLine("Today's nutrition: No log yet.");
        }

        return sb.ToString();
    }

    private static List<AnthropicMessage> BuildMessages(
        List<CoachMessageDto> history,
        string newMessage)
    {
        var messages = history
            .Select(m => new AnthropicMessage
            {
                Role = m.Role,
                Content = m.Content
            })
            .ToList();

        messages.Add(new AnthropicMessage
        {
            Role = "user",
            Content = newMessage
        });

        return messages;
    }
}

// Request/response models for Anthropic API
public class AnthropicRequest
{
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("max_tokens")]
    public int MaxTokens { get; set; }

    public string System { get; set; } = string.Empty;
    public List<AnthropicMessage> Messages { get; set; } = new();
}

public class AnthropicMessage
{
    public string Role { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
}

public class AnthropicResponse
{
    public List<AnthropicContent>? Content { get; set; }
}

public class AnthropicContent
{
    public string? Text { get; set; }
}