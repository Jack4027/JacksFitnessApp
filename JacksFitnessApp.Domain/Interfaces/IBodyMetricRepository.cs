using JacksFitnessApp.Domain.Entities.Metrics;

namespace JacksFitnessApp.Domain.Interfaces.Metrics;

public interface IBodyMetricRepository
{
    Task<IEnumerable<BodyMetric>> GetByUserIdAsync(string userId);
    Task<BodyMetric?> GetLatestAsync(string userId);
    Task<BodyMetric?> GetByIdAsync(int id);
    Task<BodyMetric> AddAsync(BodyMetric metric);
    Task UpdateAsync(BodyMetric metric);
    Task DeleteAsync(int id);
}