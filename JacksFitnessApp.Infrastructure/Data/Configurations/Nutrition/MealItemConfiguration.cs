using JacksFitnessApp.Domain.Entities.Nutrition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Nutrition;

public class MealItemConfiguration : IEntityTypeConfiguration<MealItem>
{
    public void Configure(EntityTypeBuilder<MealItem> builder)
    {
        builder.HasKey(mi => mi.Id);

        builder.Property(mi => mi.QuantityGrams)
            .IsRequired()
            .HasPrecision(8, 2);

        builder.HasOne(mi => mi.FoodItem)
            .WithMany()
            .HasForeignKey(mi => mi.FoodItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}