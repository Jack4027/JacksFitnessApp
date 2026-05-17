using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class ExerciseConfiguration : IEntityTypeConfiguration<Exercise>
{
    public void Configure(EntityTypeBuilder<Exercise> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.Type)
            .IsRequired();

        builder.Property(e => e.Category)
            .IsRequired();

        builder.Property(e => e.Equipment)
            .IsRequired();

        builder.Property(e => e.IsCustom)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasMany(e => e.WorkoutSets)
            .WithOne(ws => ws.Exercise)
            .HasForeignKey(ws => ws.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.CardioSets)
            .WithOne(cs => cs.Exercise)
            .HasForeignKey(cs => cs.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.PlannedExercises)
            .WithOne(pe => pe.Exercise)
            .HasForeignKey(pe => pe.ExerciseId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}