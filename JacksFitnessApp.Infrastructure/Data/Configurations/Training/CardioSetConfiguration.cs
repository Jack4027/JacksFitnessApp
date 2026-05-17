using JacksFitnessApp.Domain.Entities.Training;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Training;

public class CardioSetConfiguration : IEntityTypeConfiguration<CardioSet>
{
    public void Configure(EntityTypeBuilder<CardioSet> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.DistanceKm)
            .HasPrecision(8, 3);

        builder.Property(s => s.Notes)
            .HasMaxLength(500);
    }
}