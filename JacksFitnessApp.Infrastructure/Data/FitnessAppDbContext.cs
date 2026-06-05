using JacksFitnessApp.Domain.Entities.Coach;
using JacksFitnessApp.Domain.Entities.Metrics;
using JacksFitnessApp.Domain.Entities.Nutrition;
using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JacksFitnessApp.Infrastructure.Data;

public class FitnessAppDbContext : IdentityDbContext
{
    public FitnessAppDbContext(DbContextOptions<FitnessAppDbContext> options) : base(options) { }

    // Training
    public DbSet<Exercise> Exercises { get; set; }
    public DbSet<Programme> Programmes { get; set; }
    public DbSet<ProgrammeWeek> ProgrammeWeeks { get; set; }
    public DbSet<ProgrammeDay> ProgrammeDays { get; set; }
    public DbSet<PlannedExercise> PlannedExercises { get; set; }
    public DbSet<WorkoutSession> WorkoutSessions { get; set; }
    public DbSet<WorkoutSet> WorkoutSets { get; set; }
    public DbSet<CardioSet> CardioSets { get; set; }

    // Nutrition
    public DbSet<NutritionLog> NutritionLogs { get; set; }
    public DbSet<Meal> Meals { get; set; }
    public DbSet<MealItem> MealItems { get; set; }
    public DbSet<FoodItem> FoodItems { get; set; }

    // Metrics
    public DbSet<BodyMetric> BodyMetrics { get; set; }

    public DbSet<CoachConversation> CoachConversations { get; set; }
    public DbSet<CoachMessage> CoachMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FitnessAppDbContext).Assembly);
    }
}