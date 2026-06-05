using JacksFitnessApp.Domain.Entities.Coach;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Coach;

public class CoachMessageConfiguration : IEntityTypeConfiguration<CoachMessage>
{
    public void Configure(EntityTypeBuilder<CoachMessage> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Role)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(m => m.Content)
            .IsRequired()
            .HasColumnType("nvarchar(max)");
    }
}