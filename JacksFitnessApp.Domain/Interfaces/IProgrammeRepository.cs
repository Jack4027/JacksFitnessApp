using JacksFitnessApp.Domain.Entities.Training;

namespace JacksFitnessApp.Domain.Interfaces.Training;

public interface IProgrammeRepository
{
    Task<IEnumerable<Programme>> GetByUserIdAsync(string userId);
    Task<Programme?> GetByIdWithDetailsAsync(int id);
    Task<Programme> AddAsync(Programme programme);
    Task UpdateAsync(Programme programme);
    Task DeleteAsync(int id);

    Task<ProgrammeWeek> AddWeekAsync(ProgrammeWeek week);
    Task<ProgrammeDay> AddDayAsync(ProgrammeDay day);
    Task<PlannedExercise> AddPlannedExerciseAsync(PlannedExercise plannedExercise);
}