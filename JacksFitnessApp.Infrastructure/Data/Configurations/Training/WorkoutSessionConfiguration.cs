using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class WorkoutSessionConfiguration : IEntityTypeConfiguration<WorkoutSession>
{
    public void Configure(EntityTypeBuilder<WorkoutSession> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.UserId)
            .IsRequired();

        builder.Property(s => s.Date)
            .IsRequired();

        builder.Property(s => s.Notes)
            .HasMaxLength(1000);

        builder.HasMany(s => s.Sets)
            .WithOne(ws => ws.WorkoutSession)
            .HasForeignKey(ws => ws.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(s => s.CardioSets)
            .WithOne(cs => cs.WorkoutSession)
            .HasForeignKey(cs => cs.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}