using Edulytics.Core.Entities;
using Edulytics.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class PersonalPaymentTransactionConfiguration
    : IEntityTypeConfiguration<PersonalPaymentTransaction>
{
    public void Configure(EntityTypeBuilder<PersonalPaymentTransaction> builder)
    {
        builder.ToTable("PersonalPaymentTransactions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Provider).HasMaxLength(80).IsRequired();
        builder.Property(x => x.ProviderCheckoutSessionId).HasMaxLength(256).IsRequired();
        builder.Property(x => x.ProviderPaymentId).HasMaxLength(256);
        builder.Property(x => x.ProviderEventId).HasMaxLength(256);
        builder.Property(x => x.Amount).HasPrecision(14, 2).IsRequired();
        builder.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();
        builder.Property(x => x.RowVersion)
            .IsRequired()
            .IsConcurrencyToken()
            .ValueGeneratedNever();

        builder.HasIndex(x => new { x.StudentUserId, x.CreatedAtUtc });
        builder.HasIndex(x => new { x.Provider, x.ProviderCheckoutSessionId }).IsUnique();
        builder.HasIndex(x => new { x.Provider, x.ProviderEventId })
            .IsUnique()
            .HasFilter("\"ProviderEventId\" IS NOT NULL");

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PersonalSubscription>()
            .WithMany()
            .HasForeignKey(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
