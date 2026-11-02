
using Edulytics.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Edulytics.Data.Configurations;

public sealed class AssessmentItemConfiguration : IEntityTypeConfiguration<AssessmentItem>
{
    public void Configure(EntityTypeBuilder<AssessmentItem> builder)
    {
        builder.ToTable("AssessmentItems");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.Source).HasConversion<int>();
        builder.Property(x => x.ItemType).HasConversion<int>();
        builder.Property(x => x.Difficulty).HasConversion<int>();
        builder.Property(x => x.Prompt).HasMaxLength(4000).IsRequired();
        builder.Property(x => x.CorrectAnswer).HasMaxLength(1000).IsRequired();
        builder.Property(x => x.Solution).HasMaxLength(8000).IsRequired();
        builder.Property(x => x.GenerationMethod).HasMaxLength(100);
        builder.Property(x => x.GenerationFamily).HasMaxLength(200);
        builder.Property(x => x.GenerationParametersJson).HasColumnType("jsonb");
        builder.Property(x => x.ExposureFingerprint).HasMaxLength(128).IsRequired();
        builder.Property(x => x.ValidationMetadataJson).HasColumnType("jsonb");
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.RowVersion).IsRequired().IsConcurrencyToken().ValueGeneratedNever();
        builder.HasIndex(x => new { x.SchoolId, x.CurriculumAdoptionId, x.CurriculumPedagogicalLessonId });
        builder.HasIndex(x => new { x.SchoolId, x.ExposureFingerprint });
        builder.HasOne<School>().WithMany().HasForeignKey(x => x.SchoolId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<SchoolCurriculumAdoption>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.CurriculumAdoptionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CurriculumPedagogicalLesson>().WithMany()
            .HasForeignKey(x => x.CurriculumPedagogicalLessonId).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CurriculumTopic>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.CurriculumTopicId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class AssessmentItemOutcomeConfiguration : IEntityTypeConfiguration<AssessmentItemOutcome>
{
    public void Configure(EntityTypeBuilder<AssessmentItemOutcome> builder)
    {
        builder.ToTable("AssessmentItemOutcomes");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.HasIndex(x => new { x.SchoolId, x.AssessmentItemId, x.LearningOutcomeId }).IsUnique();
        builder.HasOne<AssessmentItem>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentItemId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<LearningOutcome>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.LearningOutcomeId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PracticeAttemptConfiguration : IEntityTypeConfiguration<PracticeAttempt>
{
    public void Configure(EntityTypeBuilder<PracticeAttempt> builder)
    {
        builder.ToTable("PracticeAttempts");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.Status).HasConversion<int>();
        builder.Property(x => x.IsPrivate).IsRequired().HasDefaultValue(false);
        builder.Property(x => x.Score).HasPrecision(10, 2);
        builder.Property(x => x.MaxScore).HasPrecision(10, 2);
        builder.Property(x => x.Percentage).HasPrecision(7, 2);
        builder.Property(x => x.RowVersion).IsRequired().IsConcurrencyToken().ValueGeneratedNever();
        builder.HasIndex(x => new { x.SchoolId, x.StudentProfileId, x.StartedAtUtc });
        builder.HasIndex(x => new { x.SchoolId, x.IsPrivate, x.StudentProfileId, x.SubmittedAtUtc });
        builder.HasOne<StudentProfile>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.StudentProfileId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<SchoolCurriculumAdoption>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.CurriculumAdoptionId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<CurriculumPedagogicalLesson>().WithMany()
            .HasForeignKey(x => x.CurriculumPedagogicalLessonId).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PracticeAttemptItemConfiguration : IEntityTypeConfiguration<PracticeAttemptItem>
{
    public void Configure(EntityTypeBuilder<PracticeAttemptItem> builder)
    {
        builder.ToTable("PracticeAttemptItems");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.MaxScore).HasPrecision(10, 2);
        builder.HasIndex(x => new { x.SchoolId, x.PracticeAttemptId, x.Order }).IsUnique();
        builder.HasIndex(x => new { x.SchoolId, x.PracticeAttemptId, x.AssessmentItemId }).IsUnique();
        builder.HasOne<PracticeAttempt>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.PracticeAttemptId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<AssessmentItem>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentItemId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class PracticeResponseConfiguration : IEntityTypeConfiguration<PracticeResponse>
{
    public void Configure(EntityTypeBuilder<PracticeResponse> builder)
    {
        builder.ToTable("PracticeResponses");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.Answer).HasMaxLength(2000).IsRequired();
        builder.Property(x => x.Score).HasPrecision(10, 2);
        builder.Property(x => x.Feedback).HasMaxLength(8000).IsRequired();
        builder.HasIndex(x => new { x.SchoolId, x.PracticeAttemptItemId }).IsUnique();
        builder.HasOne<PracticeAttemptItem>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.PracticeAttemptItemId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class LearningEvidenceConfiguration : IEntityTypeConfiguration<LearningEvidence>
{
    public void Configure(EntityTypeBuilder<LearningEvidence> builder)
    {
        builder.ToTable("LearningEvidence");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.EvidenceType).HasConversion<int>();
        builder.Property(x => x.Difficulty).HasConversion<int>();
        builder.Property(x => x.Score).HasPrecision(10, 2);
        builder.Property(x => x.MaxScore).HasPrecision(10, 2);
        builder.HasIndex(x => new { x.SchoolId, x.StudentProfileId, x.LearningOutcomeId, x.OccurredAtUtc });
        builder.HasIndex(x => new { x.SchoolId, x.PracticeAttemptId, x.AssessmentItemId, x.LearningOutcomeId }).IsUnique();
        builder.HasOne<StudentProfile>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.StudentProfileId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<LearningOutcome>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.LearningOutcomeId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<PracticeAttempt>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.PracticeAttemptId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<AssessmentItem>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentItemId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
    }
}

public sealed class StudentItemExposureConfiguration : IEntityTypeConfiguration<StudentItemExposure>
{
    public void Configure(EntityTypeBuilder<StudentItemExposure> builder)
    {
        builder.ToTable("StudentItemExposures");
        builder.HasKey(x => x.Id);
        builder.HasAlternateKey(x => new { x.SchoolId, x.Id });
        builder.Property(x => x.ExposureFingerprint).HasMaxLength(128).IsRequired();
        builder.HasIndex(x => new { x.SchoolId, x.StudentProfileId, x.ExposureFingerprint, x.ExposedAtUtc });
        builder.HasOne<StudentProfile>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.StudentProfileId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<AssessmentItem>().WithMany()
            .HasForeignKey(x => new { x.SchoolId, x.AssessmentItemId })
            .HasPrincipalKey(x => new { x.SchoolId, x.Id }).OnDelete(DeleteBehavior.Restrict);
    }
}
