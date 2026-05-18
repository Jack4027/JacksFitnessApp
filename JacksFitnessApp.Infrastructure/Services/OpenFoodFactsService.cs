using AutoMapper;
using JacksFitnessApp.Application.DTOs.Nutrition;
using JacksFitnessApp.Application.Interfaces;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Enums.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace JacksFitnessApp.Infrastructure.Services;

public class OpenFoodFactsService : IFoodSearchService
{
    private readonly HttpClient _httpClient;
    private readonly IFoodRepository _foodRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<OpenFoodFactsService> _logger;

    public OpenFoodFactsService(
        HttpClient httpClient,
        IFoodRepository foodRepository,
        IMapper mapper,
        ILogger<OpenFoodFactsService> logger)
    {
        _httpClient = httpClient;
        _foodRepository = foodRepository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<FoodItemDto>> SearchAsync(string query)
    {
        var localTask = _foodRepository.SearchAsync(query);
        var apiTask = SearchOpenFoodFactsAsync(query);

        await Task.WhenAll(localTask, apiTask);

        var localResults = await localTask;
        var apiResults = await apiTask;

        var localExternalIds = localResults
            .Where(r => r.ExternalId != null)
            .Select(r => r.ExternalId)
            .ToHashSet();

        var newApiResults = apiResults
            .Where(r => r.ExternalId == null || !localExternalIds.Contains(r.ExternalId));

        var combined = localResults.Concat(newApiResults).Take(20);
        return _mapper.Map<IEnumerable<FoodItemDto>>(combined);
    }

    public async Task<FoodItemDto?> GetByBarcodeAsync(string barcode)
    {
        var local = await _foodRepository.GetByBarcodeAsync(barcode);
        if (local != null) return _mapper.Map<FoodItemDto>(local);

        try
        {
            var url = $"https://world.openfoodfacts.org/api/v0/product/{barcode}.json";
            var response = await _httpClient.GetFromJsonAsync<OpenFoodFactsProductResponse>(url);

            if (response?.Product == null) return null;

            var product = response.Product;
            var foodItem = new FoodItem
            {
                Name = Sanitise(product.ProductName ?? barcode),
                Brand = product.Brands != null ? Sanitise(product.Brands) : null,
                Barcode = barcode,
                CaloriesPer100g = ClampMacro(ParseDecimal(product.Nutriments?.EnergyKcal100g), 0, 900),
                ProteinPer100g = ClampMacro(ParseDecimal(product.Nutriments?.Proteins100g), 0, 100),
                CarbsPer100g = ClampMacro(ParseDecimal(product.Nutriments?.Carbohydrates100g), 0, 100),
                FatPer100g = ClampMacro(ParseDecimal(product.Nutriments?.Fat100g), 0, 100),
                FibrePer100g = ClampMacro(ParseDecimal(product.Nutriments?.Fiber100g), 0, 100),
                Source = FoodItemSource.OpenFoodFacts,
                ExternalId = product.Id
            };

            var saved = await _foodRepository.AddAsync(foodItem);
            return _mapper.Map<FoodItemDto>(saved);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching barcode {Barcode} from Open Food Facts", barcode);
            return null;
        }
    }

    private async Task<IEnumerable<FoodItem>> SearchOpenFoodFactsAsync(string query)
    {
        try
        {
            var url = $"https://world.openfoodfacts.org/cgi/search.pl?search_terms={Uri.EscapeDataString(query)}&search_simple=1&action=process&json=1&page_size=20";
            var response = await _httpClient.GetFromJsonAsync<OpenFoodFactsSearchResponse>(url);

            if (response?.Products == null) return Enumerable.Empty<FoodItem>();

            var foodItems = new List<FoodItem>();

            foreach (var product in response.Products)
            {
                if (string.IsNullOrEmpty(product.ProductName)) continue;

                var existing = await _foodRepository.GetByExternalIdAsync(product.Id);
                if (existing != null)
                {
                    foodItems.Add(existing);
                    continue;
                }

                var foodItem = new FoodItem
                {
                    Name = Sanitise(product.ProductName),
                    Brand = product.Brands != null ? Sanitise(product.Brands) : null,
                    Barcode = product.Code,
                    CaloriesPer100g = ClampMacro(ParseDecimal(product.Nutriments?.EnergyKcal100g), 0, 900),
                    ProteinPer100g = ClampMacro(ParseDecimal(product.Nutriments?.Proteins100g), 0, 100),
                    CarbsPer100g = ClampMacro(ParseDecimal(product.Nutriments?.Carbohydrates100g), 0, 100),
                    FatPer100g = ClampMacro(ParseDecimal(product.Nutriments?.Fat100g), 0, 100),
                    FibrePer100g = ClampMacro(ParseDecimal(product.Nutriments?.Fiber100g), 0, 100),
                    Source = FoodItemSource.OpenFoodFacts,
                    ExternalId = product.Id
                };

                try
                {
                    var saved = await _foodRepository.AddAsync(foodItem);
                    foodItems.Add(saved);
                }
                catch (DbUpdateException)
                {
                    var existing2 = await _foodRepository.GetByBarcodeAsync(foodItem.Barcode!);
                    if (existing2 != null) foodItems.Add(existing2);
                }
            }

            return foodItems;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching from Open Food Facts for query: {Query}", query);
            return Enumerable.Empty<FoodItem>();
        }
    }

    public static string Sanitise(string input)
    {
        // Remove script tags and their content entirely
        var noScript = System.Text.RegularExpressions.Regex.Replace(
            input, "<script.*?>.*?</script>", string.Empty,
            System.Text.RegularExpressions.RegexOptions.Singleline |
            System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        // Strip any remaining HTML tags
        var noTags = System.Text.RegularExpressions.Regex.Replace(noScript, "<.*?>", string.Empty);

        return noTags.Trim();
    }

    private static decimal ClampMacro(decimal value, decimal min, decimal max)
    {
        return Math.Clamp(value, min, max);
    }

    private static decimal ParseDecimal(object? value)
    {
        if (value == null) return 0;
        return decimal.TryParse(value.ToString(), out var result) ? result : 0;
    }
}
