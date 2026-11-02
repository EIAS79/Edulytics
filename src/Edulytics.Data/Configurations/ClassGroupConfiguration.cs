using Edulytics.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class ClassGroupConfiguration : IEntityTypeConfiguration<ClassGroup>
{
    public void Configure(EntityTypeBuilder<ClassGroup> b)
    {
        b.ToTable("ClassGroups");
        b.HasKey(x => x.Id);
        b.HasAlternateKey(x => new { x.SchoolId, x.Id });
        b.HasAlternateKey(x => new { x.SchoolId, x.AcademicYearId, x.Id });

        b.Property(x => x.Name).HasMaxLength(150).IsRequired();
        b.Property(x => x.NormalizedName).HasMaxLength(150);
        b.Property(x => x.Code).HasMaxLength(50).IsRequired();
        b.Property(x => x.NormalizedCode).HasMaxLength(50).IsRequired();
        b.Property(x => x.Status).HasConversion<int>();
        b.Property(x => x.RowVersion)
            .IsRequired()
            .IsConcurrencyToken()
            .ValueGeneratedNever();

        // Keep the generated internal class code unique for legacy callers.
        b.HasIndex(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.AcademicProgramId,
                x.NormalizedCode
            })
            .IsUnique();

        // Normal browser identity: class name inside one curriculum adoption.
        b.HasIndex(x => new
            {
                x.SchoolId,
                x.AcademicYearId,
                x.CurriculumAdoptionId,
                x.NormalizedName
            })
            .IsUnique()
            .HasDatabaseName("UX_ClassGroup_CurriculumAdoption_Name")
            .HasFilter(
                "\"CurriculumAdoptionId\" IS NOT NULL AND \"NormalizedName\" IS NOT NULL");

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
        b.HasOne<SchoolCurriculumAdoption>()
            .WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.CurriculumAdoptionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
