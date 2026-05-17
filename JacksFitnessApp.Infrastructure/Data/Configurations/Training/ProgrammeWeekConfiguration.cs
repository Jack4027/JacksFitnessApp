using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class ProgrammeWeekConfiguration : IEntityTypeConfiguration<ProgrammeWeek>
{
    public void Configure(EntityTypeBuilder<ProgrammeWeek> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.WeekNumber)
            .IsRequired();

        builder.Property(w => w.Notes)
            .HasMaxLength(500);

        builder.HasMany(w => w.Days)
            .WithOne(d => d.ProgrammeWeek)
            .HasForeignKey(d => d.ProgrammeWeekId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}