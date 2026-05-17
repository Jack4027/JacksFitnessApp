using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Repositories.Metrics;

public class BodyMetricRepository : IBodyMetricRepository
{
    private readonly FitnessAppDbContext _context;

    public BodyMetricRepository(FitnessAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BodyMetric>> GetByUserIdAsync(string userId)
    {
        return await _context.BodyMetrics
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Date)
            .ToListAsync();
    }

    public async Task<BodyMetric?> GetLatestAsync(string userId)
    {
        return await _context.BodyMetrics
            .Where(b => b.UserId == userId)
            .OrderByDescending(b => b.Date)
            .FirstOrDefaultAsync();
    }

    public async Task<BodyMetric?> GetByIdAsync(int id)
    {
        return await _context.BodyMetrics.FindAsync(id);
    }

    public async Task<BodyMetric> AddAsync(BodyMetric metric)
    {
        _context.BodyMetrics.Add(metric);
        await _context.SaveChangesAsync();
        return metric;
    }

    public async Task UpdateAsync(BodyMetric metric)
    {
        _context.BodyMetrics.Update(metric);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var metric = await _context.BodyMetrics.FindAsync(id);
        if (metric != null)
        {
            _context.BodyMetrics.Remove(metric);
            await _context.SaveChangesAsync();
        }
    }
}