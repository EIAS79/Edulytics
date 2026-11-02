using Edulytics.Core.Entities;
using Edulytics.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class PersonalSubscriptionConfiguration
    : IEntityTypeConfiguration<PersonalSubscription>
{
    public void Configure(EntityTypeBuilder<PersonalSubscription> builder)
    {
        builder.ToTable("PersonalSubscriptions");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.PlanType).HasConversion<int>().IsRequired();
        builder.Property(x => x.Status).HasConversion<int>().IsRequired();

        builder.Property(x => x.FrameworkCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FrameworkName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FrameworkVersionName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CurriculumLevelKey).HasMaxLength(160).IsRequired();
        builder.Property(x => x.CurriculumLevelLabel).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CurriculumPathway).HasMaxLength(160);
        builder.Property(x => x.SubjectCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SubjectName).HasMaxLength(200).IsRequired();

        builder.Property(x => x.BaseAmount).HasPrecision(14, 2).IsRequired();
        builder.Property(x => x.DiscountPercent).HasPrecision(5, 2).IsRequired();
        builder.Property(x => x.PaidAmount).HasPrecision(14, 2).IsRequired();
        builder.Property(x => x.CurrencyCode).HasMaxLength(3).IsRequired();

        builder.Property(x => x.PaymentProvider).HasMaxLength(80).IsRequired();
        builder.Property(x => x.ExternalCustomerReference).HasMaxLength(256);
        builder.Property(x => x.ExternalPaymentReference).HasMaxLength(256);
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();
        builder.Property(x => x.RowVersion)
            .IsRequired()
            .IsConcurrencyToken()
            .ValueGeneratedNever();

        builder.HasIndex(x => new { x.StudentUserId, x.Status, x.EndsAtUtc });
        builder.HasIndex(x => x.ExternalPaymentReference);

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CurriculumFrameworkVersion>()
            .WithMany()
            .HasForeignKey(x => x.FrameworkVersionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
