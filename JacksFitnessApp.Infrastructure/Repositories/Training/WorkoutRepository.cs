using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Repositories.Training;

public class WorkoutRepository : IWorkoutRepository
{
    private readonly FitnessAppDbContext _context;

    public WorkoutRepository(FitnessAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<WorkoutSession>> GetByUserIdAsync(string userId)
    {
        return await _context.WorkoutSessions
            .Include(s => s.Sets)
                .ThenInclude(ws => ws.Exercise)
            .Include(s => s.CardioSets)
                .ThenInclude(cs => cs.Exercise)
            .Include(s => s.ProgrammeDay)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.Date)
            .ToListAsync();
    }
    public async Task<WorkoutSession?> GetByIdWithSetsAsync(int id)
    {
        return await _context.WorkoutSessions
            .Include(s => s.Sets)
                .ThenInclude(ws => ws.Exercise)
            .Include(s => s.CardioSets)
                .ThenInclude(cs => cs.Exercise)
            .Include(s => s.ProgrammeDay)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<WorkoutSession> AddAsync(WorkoutSession session)
    {
        _context.WorkoutSessions.Add(session);
        await _context.SaveChangesAsync();
        return session;
    }

    public async Task UpdateAsync(WorkoutSession session)
    {
        _context.WorkoutSessions.Update(session);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var session = await _context.WorkoutSessions.FindAsync(id);
        if (session != null)
        {
            _context.WorkoutSessions.Remove(session);
            await _context.SaveChangesAsync();
        }
    }
    public async Task DeleteSetAsync(int id)
    {
        var set = await _context.WorkoutSets.FindAsync(id)
            ?? throw new KeyNotFoundException($"Workout set {id} not found");
        _context.WorkoutSets.Remove(set);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCardioSetAsync(int id)
    {
        var set = await _context.CardioSets.FindAsync(id)
            ?? throw new KeyNotFoundException($"Cardio set {id} not found");
        _context.CardioSets.Remove(set);
        await _context.SaveChangesAsync();
    }

    public async Task<WorkoutSet> AddSetAsync(WorkoutSet set)
    {
        _context.WorkoutSets.Add(set);
        await _context.SaveChangesAsync();

        // Reload with exercise navigation property
        return await _context.WorkoutSets
            .Include(ws => ws.Exercise)
            .FirstAsync(ws => ws.Id == set.Id);
    }

    public async Task<CardioSet> AddCardioSetAsync(CardioSet set)
    {
        _context.CardioSets.Add(set);
        await _context.SaveChangesAsync();

        return await _context.CardioSets
            .Include(cs => cs.Exercise)
            .FirstAsync(cs => cs.Id == set.Id);
    }

    public async Task<IEnumerable<WorkoutSet>> GetPersonalRecordsAsync(string userId)
    {
        // For each exercise get the set with the highest weight for this user
        return await _context.WorkoutSets
            .Include(ws => ws.Exercise)
            .Include(ws => ws.WorkoutSession)
            .Where(ws => ws.WorkoutSession.UserId == userId && ws.IsPersonalRecord)
            .GroupBy(ws => ws.ExerciseId)
            .Select(g => g.OrderByDescending(ws => ws.WeightKg).First())
            .ToListAsync();
    }

    public async Task<WorkoutSet?> GetPersonalRecordForExerciseAsync(string userId, int exerciseId)
    {
        return await _context.WorkoutSets
            .Include(ws => ws.WorkoutSession)
            .Where(ws => ws.WorkoutSession.UserId == userId
                      && ws.ExerciseId == exerciseId)
            .OrderByDescending(ws => ws.WeightKg)
            .FirstOrDefaultAsync();
    }

    public async Task<CardioSet?> GetBestDistanceForExerciseAsync(string userId, int exerciseId)
    {
        return await _context.CardioSets
            .Include(cs => cs.WorkoutSession)
            .Where(cs => cs.WorkoutSession.UserId == userId
                      && cs.ExerciseId == exerciseId
                      && cs.DistanceKm.HasValue)
            .OrderByDescending(cs => cs.DistanceKm)
            .FirstOrDefaultAsync();
    }

    public async Task<CardioSet?> GetBestTimeForExerciseAsync(string userId, int exerciseId)
    {
        return await _context.CardioSets
            .Include(cs => cs.WorkoutSession)
            .Where(cs => cs.WorkoutSession.UserId == userId
                      && cs.ExerciseId == exerciseId
                      && cs.DurationSeconds.HasValue)
            .OrderBy(cs => cs.DurationSeconds)
            .FirstOrDefaultAsync();
    }
}