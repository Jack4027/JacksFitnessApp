using JacksFitnessApp.Domain.Entities.Training;

namespace JacksFitnessApp.Domain.Interfaces.Training;

public interface IWorkoutRepository
{
    Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(string userId);
    Task<WorkoutSession?> GetByIdWithSetsAsync(int id);
    Task<WorkoutSession> AddAsync(WorkoutSession session);
    Task UpdateAsync(WorkoutSession session);
    Task DeleteAsync(int id);
    Task<WorkoutSet> AddSetAsync(WorkoutSet set);
    Task<CardioSet> AddCardioSetAsync(CardioSet set);
    Task<IEnumerable<WorkoutSet>> GetPersonalRecordsAsync(string userId);
    Task<WorkoutSet?> GetPersonalRecordForExerciseAsync(string userId, int exerciseId);
    Task<CardioSet?> GetBestDistanceForExerciseAsync(string userId, int exerciseId);
    Task<CardioSet?> GetBestTimeForExerciseAsync(string userId, int exerciseId);
}