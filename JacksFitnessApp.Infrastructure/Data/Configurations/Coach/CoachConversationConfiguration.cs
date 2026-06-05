using JacksFitnessApp.Domain.Entities.Coach;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JacksFitnessApp.Infrastructure.Data.Configurations.Coach;

public class CoachConversationConfiguration : IEntityTypeConfiguration<CoachConversation>
{
    public void Configure(EntityTypeBuilder<CoachConversation> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.UserId)
            .IsRequired()
            .HasMaxLength(450);

        builder.Property(c => c.Title)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne<IdentityUser>()
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Messages)
            .WithOne(m => m.Conversation)
            .HasForeignKey(m => m.ConversationId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}