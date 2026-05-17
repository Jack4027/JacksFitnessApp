using JacksFitnessApp.Domain.Entities.Metrics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Metrics;

public class BodyMetricConfiguration : IEntityTypeConfiguration<BodyMetric>
{
    public void Configure(EntityTypeBuilder<BodyMetric> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.UserId)
            .IsRequired();

        builder.Property(b => b.WeightKg)
            .HasPrecision(5, 2);

        builder.Property(b => b.BodyFatPercentage)
            .HasPrecision(4, 1);

        builder.Property(b => b.MuscleMassKg)
            .HasPrecision(5, 2);

        builder.Property(b => b.ChestCm)
            .HasPrecision(5, 1);

        builder.Property(b => b.WaistCm)
            .HasPrecision(5, 1);

        builder.Property(b => b.HipsCm)
            .HasPrecision(5, 1);

        builder.Property(b => b.BicepCm)
            .HasPrecision(5, 1);

        builder.Property(b => b.ThighCm)
            .HasPrecision(5, 1);

        builder.Property(b => b.Notes)
            .HasMaxLength(1000);

        builder.HasIndex(b => new { b.UserId, b.Date });
    }
}