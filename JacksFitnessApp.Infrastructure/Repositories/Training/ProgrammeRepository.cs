using JacksFitnessApp.Domain.Entities.Training;
using JacksFitnessApp.Domain.Interfaces.Training;
using JacksFitnessApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Repositories.Training;

public class ProgrammeRepository : IProgrammeRepository
{
    private readonly FitnessAppDbContext _context;

    public ProgrammeRepository(FitnessAppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Programme>> GetByUserIdAsync(string userId)
    {
        return await _context.Programmes
            .Where(p => p.UserId == userId)
            .ToListAsync();
    }

    public async Task<Programme?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Programmes
            .Include(p => p.Weeks)
                .ThenInclude(w => w.Days)
                    .ThenInclude(d => d.PlannedExercises)
                        .ThenInclude(pe => pe.Exercise)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProgrammeWeek?> GetWeekByIdAsync(int id)
    {
        return await _context.ProgrammeWeeks
            .Include(w => w.Days)
                .ThenInclude(d => d.PlannedExercises)
                    .ThenInclude(pe => pe.Exercise)
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<ProgrammeDay?> GetDayByIdAsync(int id)
    {
        return await _context.ProgrammeDays
            .Include(d => d.PlannedExercises)
                .ThenInclude(pe => pe.Exercise)
            .FirstOrDefaultAsync(d => d.Id == id);
    }
    public async Task<Programme> AddAsync(Programme programme)
    {
        _context.Programmes.Add(programme);
        await _context.SaveChangesAsync();
        return programme;
    }

    public async Task UpdateAsync(Programme programme)
    {
        _context.Programmes.Update(programme);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var programme = await _context.Programmes.FindAsync(id);
        if (programme != null)
        {
            _context.Programmes.Remove(programme);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<ProgrammeWeek> AddWeekAsync(ProgrammeWeek week)
    {
        _context.ProgrammeWeeks.Add(week);
        await _context.SaveChangesAsync();

        return await _context.ProgrammeWeeks
            .Include(w => w.Days)
                .ThenInclude(d => d.PlannedExercises)
                    .ThenInclude(pe => pe.Exercise)
            .FirstAsync(w => w.Id == week.Id);
    }

    public async Task<ProgrammeDay> AddDayAsync(ProgrammeDay day)
    {
        _context.ProgrammeDays.Add(day);
        await _context.SaveChangesAsync();

        return await _context.ProgrammeDays
            .Include(d => d.PlannedExercises)
                .ThenInclude(pe => pe.Exercise)
            .FirstAsync(d => d.Id == day.Id);
    }

    public async Task<PlannedExercise> AddPlannedExerciseAsync(PlannedExercise plannedExercise)
    {
        _context.PlannedExercises.Add(plannedExercise);
        await _context.SaveChangesAsync();

        // Reload with Exercise navigation property
        return await _context.PlannedExercises
            .Include(pe => pe.Exercise)
            .FirstAsync(pe => pe.Id == plannedExercise.Id);
    }
}