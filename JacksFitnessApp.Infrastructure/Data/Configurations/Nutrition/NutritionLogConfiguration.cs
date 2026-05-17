using JacksFitnessApp.Domain.Entities.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Nutrition;

public class NutritionLogConfiguration : IEntityTypeConfiguration<NutritionLog>
{
    public void Configure(EntityTypeBuilder<NutritionLog> builder)
    {
        builder.HasKey(n => n.Id);

        builder.Property(n => n.UserId)
            .IsRequired();

        builder.Property(n => n.Notes)
            .HasMaxLength(1000);

        builder.HasIndex(n => new { n.UserId, n.Date })
            .IsUnique();

        builder.HasMany(n => n.Meals)
            .WithOne(m => m.NutritionLog)
            .HasForeignKey(m => m.NutritionLogId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}