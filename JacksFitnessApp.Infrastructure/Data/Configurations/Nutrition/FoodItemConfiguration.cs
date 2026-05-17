using JacksFitnessApp.Domain.Entities.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Nutrition;

public class FoodItemConfiguration : IEntityTypeConfiguration<FoodItem>
{
    public void Configure(EntityTypeBuilder<FoodItem> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Brand)
            .HasMaxLength(100);

        builder.Property(f => f.Barcode)
            .HasMaxLength(50);

        builder.Property(f => f.CaloriesPer100g)
            .HasPrecision(8, 2);

        builder.Property(f => f.ProteinPer100g)
            .HasPrecision(6, 2);

        builder.Property(f => f.CarbsPer100g)
            .HasPrecision(6, 2);

        builder.Property(f => f.FatPer100g)
            .HasPrecision(6, 2);

        builder.Property(f => f.FibrePer100g)
            .HasPrecision(6, 2);

        builder.HasIndex(f => f.Barcode)
            .IsUnique()
            .HasFilter("[Barcode] IS NOT NULL");
    }
}