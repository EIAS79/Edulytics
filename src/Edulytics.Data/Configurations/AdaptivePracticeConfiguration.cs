using Edulytics.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class AdaptivePracticeSessionConfiguration :
    IEntityTypeConfiguration<AdaptivePracticeSession>
{
    public void Configure(
        EntityTypeBuilder<AdaptivePracticeSession> builder)
    {
        builder.ToTable("AdaptivePracticeSessions");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.CurriculumLevelKey)
            .HasMaxLength(160).IsRequired();
        builder.Property(x => x.PrimarySkillId)
            .HasMaxLength(200).IsRequired();
        builder.Property(x => x.Purpose).HasConversion<int>();
        builder.Property(x => x.Status).HasConversion<int>();
        builder.Property(x => x.EngineVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.PolicyVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.CapabilityVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.FeatureFlagSnapshotJson)
            .HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.StopReason).HasMaxLength(100);
        builder.Property(x => x.RowVersion)
            .IsRequired().IsConcurrencyToken().ValueGeneratedNever();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.StudentProfileId,
            x.StartedAtUtc
        });

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.Status,
            x.StartedAtUtc
        });

        builder.HasOne<School>().WithMany()
            .HasForeignKey(x => x.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<StudentProfile>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.StudentProfileId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SchoolCurriculumAdoption>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.CurriculumAdoptionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<CurriculumPedagogicalLesson>().WithMany()
            .HasForeignKey(x => x.CurriculumPedagogicalLessonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class AdaptiveDecisionSnapshotConfiguration :
    IEntityTypeConfiguration<AdaptiveDecisionSnapshot>
{
    public void Configure(
        EntityTypeBuilder<AdaptiveDecisionSnapshot> builder)
    {
        builder.ToTable("AdaptiveDecisionSnapshots");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.EngineVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.PolicyVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.SkillMasteryBefore).HasPrecision(8, 6);
        builder.Property(x => x.PrerequisiteMasteryBefore).HasPrecision(8, 6);
        builder.Property(x => x.RepresentationFluencyJson)
            .HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.ActiveMisconceptionsJson)
            .HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.SelectedFamily)
            .HasMaxLength(240).IsRequired();
        builder.Property(x => x.SelectedRepresentation)
            .HasMaxLength(120);
        builder.Property(x => x.MisconceptionFocusId)
            .HasMaxLength(200);
        builder.Property(x => x.FreshnessConstraintsJson)
            .HasColumnType("jsonb").IsRequired();
        builder.Property(x => x.DecisionReasonCode)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.DecisionTraceJson)
            .HasColumnType("jsonb").IsRequired();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.SessionId,
            x.Sequence
        }).IsUnique();

        builder.HasOne<AdaptivePracticeSession>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.SessionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class AdaptivePracticeTurnConfiguration :
    IEntityTypeConfiguration<AdaptivePracticeTurn>
{
    public void Configure(
        EntityTypeBuilder<AdaptivePracticeTurn> builder)
    {
        builder.ToTable("AdaptivePracticeTurns");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.SkillId)
            .HasMaxLength(200).IsRequired();
        builder.Property(x => x.QuestionFamily)
            .HasMaxLength(240).IsRequired();
        builder.Property(x => x.Representation)
            .HasMaxLength(120);
        builder.Property(x => x.UiDifficultyBand)
            .HasConversion<int>();
        builder.Property(x => x.MisconceptionFocusId)
            .HasMaxLength(200);
        builder.Property(x => x.SubmittedAnswer)
            .HasMaxLength(2000);
        builder.Property(x => x.Score).HasPrecision(10, 2);
        builder.Property(x => x.Feedback).HasMaxLength(8000);
        builder.Property(x => x.ExposureFingerprint)
            .HasMaxLength(128).IsRequired();
        builder.Property(x => x.SemanticIdentityKey)
            .HasMaxLength(256).IsRequired();
        builder.Property(x => x.RowVersion)
            .IsRequired().IsConcurrencyToken().ValueGeneratedNever();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.SessionId,
            x.Sequence
        }).IsUnique();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.SessionId,
            x.ExposureFingerprint
        }).IsUnique();

        builder.HasOne<AdaptivePracticeSession>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.SessionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AssessmentItem>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentItemId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<AdaptiveDecisionSnapshot>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.DecisionSnapshotId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StudentMisconceptionStateConfiguration :
    IEntityTypeConfiguration<StudentMisconceptionState>
{
    public void Configure(
        EntityTypeBuilder<StudentMisconceptionState> builder)
    {
        builder.ToTable("StudentMisconceptionStates");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.SkillId)
            .HasMaxLength(200).IsRequired();
        builder.Property(x => x.MisconceptionId)
            .HasMaxLength(200).IsRequired();
        builder.Property(x => x.QuestionFamily)
            .HasMaxLength(240);
        builder.Property(x => x.Status).HasConversion<int>();
        builder.Property(x => x.Confidence).HasPrecision(8, 6);
        builder.Property(x => x.EngineVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.RowVersion)
            .IsRequired().IsConcurrencyToken().ValueGeneratedNever();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.StudentProfileId,
            x.CurriculumAdoptionId,
            x.SkillId,
            x.MisconceptionId
        }).IsUnique();

        builder.HasOne<StudentProfile>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.StudentProfileId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<SchoolCurriculumAdoption>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.CurriculumAdoptionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StudentRepresentationFluencyStateConfiguration :
    IEntityTypeConfiguration<StudentRepresentationFluencyState>
{
    public void Configure(
        EntityTypeBuilder<StudentRepresentationFluencyState> builder)
    {
        builder.ToTable("StudentRepresentationFluencyStates");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.SkillId)
            .HasMaxLength(200).IsRequired();
        builder.Property(x => x.Representation)
            .HasMaxLength(120).IsRequired();
        builder.Property(x => x.WeightedFluency)
            .HasPrecision(8, 6);
        builder.Property(x => x.EngineVersion)
            .HasMaxLength(100).IsRequired();
        builder.Property(x => x.RowVersion)
            .IsRequired().IsConcurrencyToken().ValueGeneratedNever();

        builder.HasIndex(x => new
        {
            x.SchoolId,
            x.StudentProfileId,
            x.SkillId,
            x.Representation
        }).IsUnique();

        builder.HasOne<StudentProfile>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.StudentProfileId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id })
            .OnDelete(DeleteBehavior.Restrict);
    }
}
