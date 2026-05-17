using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class ProgrammeDayConfiguration : IEntityTypeConfiguration<ProgrammeDay>
{
    public void Configure(EntityTypeBuilder<ProgrammeDay> builder)
    {
        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(d => d.PlannedExercises)
            .WithOne(pe => pe.ProgrammeDay)
            .HasForeignKey(pe => pe.ProgrammeDayId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(d => d.WorkoutSessions)
            .WithOne(ws => ws.ProgrammeDay)
            .HasForeignKey(ws => ws.ProgrammeDayId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}