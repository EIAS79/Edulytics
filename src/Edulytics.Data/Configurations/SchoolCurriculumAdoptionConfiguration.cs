using Edulytics.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class SchoolCurriculumAdoptionConfiguration : IEntityTypeConfiguration<SchoolCurriculumAdoption>
{
    public void Configure(EntityTypeBuilder<SchoolCurriculumAdoption> b)
    {
        b.ToTable("SchoolCurriculumAdoptions");
        b.HasKey(x => x.Id);
        b.HasAlternateKey(x => new { x.SchoolId, x.Id });

        b.Property(x => x.CurriculumLevelKey).HasMaxLength(200);
        b.Property(x => x.CurriculumLevelLabel).HasMaxLength(240);
        b.Property(x => x.CurriculumStage).HasMaxLength(160);
        b.Property(x => x.CurriculumPathway).HasMaxLength(240);
        b.Property(x => x.CreatedAtUtc).IsRequired();
        b.Property(x => x.UpdatedAtUtc).IsRequired();
        b.Property(x => x.RowVersion)
            .IsRequired()
            .IsConcurrencyToken()
            .ValueGeneratedNever();

        // Explicit identity indexes are authoritative for new writes.
        b.HasIndex(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.AcademicProgramId,
                x.CurriculumLevelKey,
                x.SubjectId,
                x.FrameworkVersionId
            })
            .IsUnique()
            .AreNullsDistinct(false)
            .HasDatabaseName("UX_CurriculumAdoption_ExplicitLevel")
            .HasFilter("\"CurriculumLevelKey\" IS NOT NULL");

        b.HasIndex(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.AcademicProgramId,
                x.CurriculumLevelKey,
                x.SubjectId
            })
            .IsUnique()
            .AreNullsDistinct(false)
            .HasDatabaseName("UX_CurriculumAdoption_PrimaryExplicitLevel")
            .HasFilter("\"IsPrimary\" = TRUE AND \"CurriculumLevelKey\" IS NOT NULL");

        // Legacy scopes remain valid only while they have not been deterministically
        // resolved to an explicit curriculum level.
        b.HasIndex(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.AcademicProgramId,
                x.GradeLevelId,
                x.SubjectId,
                x.FrameworkVersionId
            })
            .IsUnique()
            .AreNullsDistinct(false)
            .HasDatabaseName("UX_CurriculumAdoption_LegacyScope")
            .HasFilter("\"CurriculumLevelKey\" IS NULL");

        b.HasIndex(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.AcademicProgramId,
                x.GradeLevelId,
                x.SubjectId
            })
            .IsUnique()
            .AreNullsDistinct(false)
            .HasDatabaseName("UX_CurriculumAdoption_PrimaryLegacyScope")
            .HasFilter("\"IsPrimary\" = TRUE AND \"CurriculumLevelKey\" IS NULL");

        b.ToTable(
            table => table.HasCheckConstraint(
                "CK_CurriculumAdoption_LogicalLevel",
                "\"CurriculumLogicalLevel\" IS NULL OR (\"CurriculumLogicalLevel\" BETWEEN 1 AND 13)"));

        b.HasOne<School>()
            .WithMany()
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne<AcademicYear>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AcademicYearId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne<AcademicProgram>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AcademicProgramId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne<GradeLevel>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.GradeLevelId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne<Subject>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.SubjectId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne<CurriculumFrameworkVersion>()
            .WithMany()
            .HasForeignKey(x => x.FrameworkVersionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
