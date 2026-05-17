using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class WorkoutSetConfiguration : IEntityTypeConfiguration<WorkoutSet>
{
    public void Configure(EntityTypeBuilder<WorkoutSet> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.WeightKg)
            .HasPrecision(6, 2);

        builder.Property(s => s.IsPersonalRecord)
            .HasDefaultValue(false);

        builder.Property(s => s.Notes)
            .HasMaxLength(500);
    }
}