using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Repositories.Nutrition;

public class NutritionRepository : INutritionRepository
{
    private readonly FitnessAppDbContext _context;

    public NutritionRepository(FitnessAppDbContext context)
    {
        _context = context;
    }

    public async Task<NutritionLog?> GetByDateAsync(string userId, DateOnly date)
    {
        return await _context.NutritionLogs
            .Include(n => n.Meals)
                .ThenInclude(m => m.Items)
                    .ThenInclude(i => i.FoodItem)
            .FirstOrDefaultAsync(n => n.UserId == userId && n.Date == date);
    }

    public async Task<NutritionLog?> GetByIdAsync(int id)
    {
        return await _context.NutritionLogs
            .Include(n => n.Meals)
                .ThenInclude(m => m.Items)
                    .ThenInclude(i => i.FoodItem)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<NutritionLog>> GetByUserIdAsync(string userId)
    {
        return await _context.NutritionLogs
            .Include(n => n.Meals)
                .ThenInclude(m => m.Items)
                    .ThenInclude(i => i.FoodItem)
            .Where(n => n.UserId == userId)
            .OrderByDescending(n => n.Date)
            .ToListAsync();
    }

    public async Task<NutritionLog> AddAsync(NutritionLog log)
    {
        _context.NutritionLogs.Add(log);
        await _context.SaveChangesAsync();
        return log;
    }

    public async Task UpdateAsync(NutritionLog log)
    {
        _context.NutritionLogs.Update(log);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var log = await _context.NutritionLogs.FindAsync(id);
        if (log != null)
        {
            _context.NutritionLogs.Remove(log);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Meal> AddMealAsync(Meal meal)
    {
        _context.Meals.Add(meal);
        await _context.SaveChangesAsync();
        return meal;
    }

    public async Task DeleteMealAsync(int id)
    {
        var meal = await _context.Meals.FindAsync(id) ?? throw new KeyNotFoundException($"Meal {id} not found");

        if (meal != null)
        {
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<MealItem> AddMealItemAsync(MealItem mealItem)
    {
        _context.MealItems.Add(mealItem);
        await _context.SaveChangesAsync();

        // Reload with FoodItem navigation property so computed properties work
        return await _context.MealItems
            .Include(mi => mi.FoodItem)
            .FirstAsync(mi => mi.Id == mealItem.Id);
    }

    public async Task DeleteMealItemAsync(int id)
    {
        var item = await _context.MealItems.FindAsync(id);
        if (item != null)
        {
            _context.MealItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }
}