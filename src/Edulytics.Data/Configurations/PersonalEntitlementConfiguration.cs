using Edulytics.Core.Entities;
using Edulytics.Data.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class PersonalEntitlementConfiguration
    : IEntityTypeConfiguration<PersonalEntitlement>
{
    public void Configure(EntityTypeBuilder<PersonalEntitlement> builder)
    {
        builder.ToTable("PersonalEntitlements");
        builder.HasKey(x => x.Id);

        builder.Property(x => x.FrameworkCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.FrameworkName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.FrameworkVersionName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CurriculumLevelKey).HasMaxLength(160).IsRequired();
        builder.Property(x => x.CurriculumLevelLabel).HasMaxLength(200).IsRequired();
        builder.Property(x => x.CurriculumPathway).HasMaxLength(160);
        builder.Property(x => x.SubjectCode).HasMaxLength(100).IsRequired();
        builder.Property(x => x.SubjectName).HasMaxLength(200).IsRequired();
        builder.Property(x => x.IsActive).IsRequired();
        builder.Property(x => x.StartsAtUtc).IsRequired();
        builder.Property(x => x.EndsAtUtc).IsRequired();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.UpdatedAtUtc).IsRequired();
        builder.Property(x => x.RowVersion)
            .IsRequired()
            .IsConcurrencyToken()
            .ValueGeneratedNever();

        builder.HasIndex(x => new
        {
            x.StudentUserId,
            x.IsActive,
            x.StartsAtUtc,
            x.EndsAtUtc
        });

        builder.HasIndex(x => x.SubscriptionId).IsUnique();

        builder.HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(x => x.StudentUserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<PersonalSubscription>()
            .WithOne()
            .HasForeignKey<PersonalEntitlement>(x => x.SubscriptionId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CurriculumFrameworkVersion>()
            .WithMany()
            .HasForeignKey(x => x.FrameworkVersionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
