using JacksFitnessApp.Application.DTOs.Coach;
using JacksFitnessApp.Application.Interfaces;
using JacksFitnessApp.Domain.Interfaces.Metrics;
using JacksFitnessApp.Domain.Interfaces.Nutrition;
using JacksFitnessApp.Domain.Interfaces.Training;

namespace JacksFitnessApp.Infrastructure.Services;

public class UserContextService : IUserContextService
{
    private readonly IWorkoutRepository _workoutRepository;
    private readonly IBodyMetricRepository _metricRepository;
    private readonly INutritionRepository _nutritionRepository;

    public UserContextService(
        IWorkoutRepository workoutRepository,
        IBodyMetricRepository metricRepository,
        INutritionRepository nutritionRepository)
    {
        _workoutRepository = workoutRepository;
        _metricRepository = metricRepository;
        _nutritionRepository = nutritionRepository;
    }

    public async Task<UserContextDto> GetUserContextAsync(string userId)
    {
        var workouts = await _workoutRepository.GetByUserIdAsync(userId);
        var recentWorkouts = workouts.Take(10).ToList();

        var personalRecords = await _workoutRepository.GetPersonalRecordsAsync(userId);
        var latestMetric = await _metricRepository.GetLatestAsync(userId);
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var todayLog = await _nutritionRepository.GetByDateAsync(userId, today);

        return new UserContextDto
        {
            RecentWorkouts = recentWorkouts.Select(w => new RecentWorkoutSummary
            {
                Date = w.Date.ToString("yyyy-MM-dd"),
                TotalSets = w.Sets.Count + w.CardioSets.Count,
                DurationMinutes = w.DurationMinutes,
                TotalVolume = w.Sets.Sum(s => s.WeightKg * s.RepsCompleted),
                Exercises = w.Sets.Select(s => s.Exercise.Name)
                    .Concat(w.CardioSets.Select(s => s.Exercise.Name))
                    .Distinct()
                    .ToList()
            }).ToList(),

            PersonalRecords = personalRecords.Select(pr => new PersonalRecordSummary
            {
                ExerciseName = pr.Exercise.Name,
                WeightKg = pr.WeightKg,
                Reps = pr.RepsCompleted,
                AchievedOn = pr.WorkoutSession.Date.ToString("yyyy-MM-dd")
            }).ToList(),

            LatestMetrics = latestMetric == null ? null : new LatestMetricsSummary
            {
                WeightKg = latestMetric.WeightKg,
                BodyFatPercentage = latestMetric.BodyFatPercentage,
                MuscleMassKg = latestMetric.MuscleMassKg,
                Date = latestMetric.Date.ToString("yyyy-MM-dd")
            },

            TodayNutrition = todayLog == null ? null : new TodayNutritionSummary
            {
                TotalCalories = todayLog.Meals.SelectMany(m => m.Items).Sum(i => i.Calories),
                TotalProtein = todayLog.Meals.SelectMany(m => m.Items).Sum(i => i.Protein),
                TotalCarbs = todayLog.Meals.SelectMany(m => m.Items).Sum(i => i.Carbs),
                TotalFat = todayLog.Meals.SelectMany(m => m.Items).Sum(i => i.Fat),
                MealCount = todayLog.Meals.Count
            }
        };
    }
}