using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Repositories.Nutrition;

public class FoodRepository : IFoodRepository
{
    private readonly FitnessAppDbContext _context;

    public FoodRepository(FitnessAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FoodItem>> SearchAsync(string query)
    {
        return await _context.FoodItems
            .Where(f => f.Name.Contains(query) ||
                       (f.Brand != null && f.Brand.Contains(query)))
            .Take(20)
            .ToListAsync();
    }

    public async Task<FoodItem?> GetByBarcodeAsync(string barcode)
    {
        return await _context.FoodItems
            .FirstOrDefaultAsync(f => f.Barcode == barcode);
    }

    public async Task<FoodItem?> GetByIdAsync(int id)
    {
        return await _context.FoodItems.FindAsync(id);
    }

    public async Task<FoodItem?> GetByExternalIdAsync(string? externalId)
    {
        if (externalId == null) return null;
        return await _context.FoodItems
            .FirstOrDefaultAsync(f => f.ExternalId == externalId);
    }

    public async Task<FoodItem> AddAsync(FoodItem foodItem)
    {
        _context.FoodItems.Add(foodItem);
        await _context.SaveChangesAsync();
        return foodItem;
    }
}