using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class PlannedExerciseConfiguration : IEntityTypeConfiguration<PlannedExercise>
{
    public void Configure(EntityTypeBuilder<PlannedExercise> builder)
    {
        builder.HasKey(pe => pe.Id);

        builder.Property(pe => pe.TargetWeight)
            .HasPrecision(6, 2);

        builder.Property(pe => pe.Notes)
            .HasMaxLength(500);
    }
}