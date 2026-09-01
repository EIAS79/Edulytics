from pathlib import Path

ROOT = Path(__file__).resolve().parents[2]


def write(rel: str, text: str) -> None:
    path = ROOT / rel
    path.parent.mkdir(parents=True, exist_ok=True)
    path.write_text(text.rstrip() + "\n", encoding="utf-8")
    print(f"WRITE {rel}")


def replace_once(rel: str, old: str, new: str) -> None:
    path = ROOT / rel
    text = path.read_text(encoding="utf-8")
    if new in text:
        print(f"SKIP {rel}: already patched")
        return
    if old not in text:
        raise SystemExit(f"PATCH ANCHOR MISSING: {rel}: {old[:80]!r}")
    path.write_text(text.replace(old, new, 1), encoding="utf-8")
    print(f"PATCH {rel}")


write("src/Edulytics.Core/Enums/PracticeEnums.cs", r'''
namespace Edulytics.Core.Enums;

public enum AssessmentItemSource
{
    TeacherCreated = 1,
    SystemGenerated = 2
}

public enum AssessmentItemType
{
    Numeric = 1,
    ShortAnswer = 2,
    MultipleChoice = 3
}

public enum AssessmentItemDifficulty
{
    Easy = 1,
    Medium = 2,
    Challenging = 3
}

public enum PracticeAttemptStatus
{
    InProgress = 1,
    Submitted = 2
}

public enum LearningEvidenceType
{
    Practice = 1
}
''')

write("src/Edulytics.Core/Entities/PracticeEntities.cs", r'''
using Edulytics.Core.Enums;
using Edulytics.Core.Interfaces;

namespace Edulytics.Core.Entities;

public sealed class AssessmentItem : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid CurriculumAdoptionId { get; set; }
    public Guid? CurriculumPedagogicalLessonId { get; set; }
    public Guid? CurriculumTopicId { get; set; }
    public AssessmentItemSource Source { get; set; }
    public AssessmentItemType ItemType { get; set; }
    public AssessmentItemDifficulty Difficulty { get; set; }
    public string Prompt { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string Solution { get; set; } = string.Empty;
    public Guid? CreatedByUserId { get; set; }
    public string? GenerationMethod { get; set; }
    public string? GenerationFamily { get; set; }
    public string? GenerationParametersJson { get; set; }
    public string ExposureFingerprint { get; set; } = string.Empty;
    public string? ValidationMetadataJson { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = [];
}

public sealed class AssessmentItemOutcome : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public Guid LearningOutcomeId { get; set; }
}

public sealed class PracticeAttempt : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid CurriculumAdoptionId { get; set; }
    public Guid? CurriculumPedagogicalLessonId { get; set; }
    public PracticeAttemptStatus Status { get; set; }
    public DateTime StartedAtUtc { get; set; }
    public DateTime? SubmittedAtUtc { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public decimal Percentage { get; set; }
    public byte[] RowVersion { get; set; } = [];
}

public sealed class PracticeAttemptItem : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid PracticeAttemptId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public int Order { get; set; }
    public decimal MaxScore { get; set; }
}

public sealed class PracticeResponse : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid PracticeAttemptItemId { get; set; }
    public string Answer { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public decimal Score { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public DateTime AnsweredAtUtc { get; set; }
}

public sealed class LearningEvidence : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid LearningOutcomeId { get; set; }
    public Guid PracticeAttemptId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public LearningEvidenceType EvidenceType { get; set; }
    public AssessmentItemDifficulty Difficulty { get; set; }
    public bool IsCorrect { get; set; }
    public decimal Score { get; set; }
    public decimal MaxScore { get; set; }
    public DateTime OccurredAtUtc { get; set; }
}

public sealed class StudentItemExposure : ISchoolScoped
{
    public Guid Id { get; set; }
    public Guid SchoolId { get; set; }
    public Guid StudentProfileId { get; set; }
    public Guid AssessmentItemId { get; set; }
    public string ExposureFingerprint { get; set; } = string.Empty;
    public DateTime ExposedAtUtc { get; set; }
}
''')

write("src/Edulytics.Core/Practice/IPracticeRepository.cs", r'''
using Edulytics.Core.Entities;

namespace Edulytics.Core.Practice;

public interface IPracticeRepository
{
    Task<StudentProfile?> FindStudentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<bool> IsEnrolledInAdoptionAsync(
        Guid schoolId,
        Guid studentProfileId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssessmentItem>> ListItemsAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<AssessmentItem>> GetItemsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyDictionary<Guid, IReadOnlyList<Guid>>> GetOutcomeIdsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default);

    Task<bool> OutcomesBelongToAdoptionAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        IReadOnlyCollection<Guid> outcomeIds,
        CancellationToken cancellationToken = default);

    Task AddAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<PracticeAttemptItem> items,
        IReadOnlyCollection<StudentItemExposure> exposures,
        CancellationToken cancellationToken = default);

    Task<PracticeAttempt?> GetAttemptAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PracticeAttemptItem>> GetAttemptItemsAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<PracticeResponse>> GetResponsesAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task SaveResponseAsync(
        PracticeResponse response,
        CancellationToken cancellationToken = default);

    Task CompleteAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<LearningEvidence> evidence,
        CancellationToken cancellationToken = default);
}
''')

write("src/Edulytics.Data/Configurations/PracticeConfiguration.cs", r'''
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
        builder.Property(x => x.Score).HasPrecision(10, 2);
        builder.Property(x => x.MaxScore).HasPrecision(10, 2);
        builder.Property(x => x.Percentage).HasPrecision(7, 2);
        builder.Property(x => x.RowVersion).IsRequired().IsConcurrencyToken().ValueGeneratedNever();
        builder.HasIndex(x => new { x.SchoolId, x.StudentProfileId, x.StartedAtUtc });
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
''')

write("src/Edulytics.Data/Repositories/PracticeRepository.cs", r'''
using Edulytics.Core.Entities;
using Edulytics.Core.Practice;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class PracticeRepository(EdulyticsDbContext context) : IPracticeRepository
{
    public Task<StudentProfile?> FindStudentByUserIdAsync(Guid userId, CancellationToken cancellationToken = default) =>
        context.StudentProfiles.AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == userId && !x.IsArchived, cancellationToken);

    public Task<bool> IsEnrolledInAdoptionAsync(
        Guid schoolId,
        Guid studentProfileId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default) =>
        context.StudentEnrollments.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && x.StudentProfileId == studentProfileId)
            .Join(
                context.ClassGroups.AsNoTracking().Where(x => x.SchoolId == schoolId),
                enrollment => enrollment.ClassGroupId,
                classGroup => classGroup.Id,
                (_, classGroup) => classGroup)
            .AnyAsync(x => x.CurriculumAdoptionId == curriculumAdoptionId, cancellationToken);

    public async Task<IReadOnlyList<AssessmentItem>> ListItemsAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default) =>
        await context.AssessmentItems.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.CurriculumAdoptionId == curriculumAdoptionId &&
                (!lessonId.HasValue || x.CurriculumPedagogicalLessonId == lessonId))
            .OrderBy(x => x.Difficulty)
            .ThenBy(x => x.CreatedAtUtc)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<AssessmentItem>> GetItemsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default) =>
        await context.AssessmentItems.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && itemIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyDictionary<Guid, IReadOnlyList<Guid>>> GetOutcomeIdsAsync(
        Guid schoolId,
        IReadOnlyCollection<Guid> itemIds,
        CancellationToken cancellationToken = default)
    {
        var rows = await context.AssessmentItemOutcomes.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && itemIds.Contains(x.AssessmentItemId))
            .Select(x => new { x.AssessmentItemId, x.LearningOutcomeId })
            .ToListAsync(cancellationToken);

        return rows
            .GroupBy(x => x.AssessmentItemId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<Guid>)x.Select(y => y.LearningOutcomeId).Distinct().ToArray());
    }

    public async Task<bool> OutcomesBelongToAdoptionAsync(
        Guid schoolId,
        Guid curriculumAdoptionId,
        IReadOnlyCollection<Guid> outcomeIds,
        CancellationToken cancellationToken = default)
    {
        var distinct = outcomeIds.Distinct().ToArray();
        if (distinct.Length == 0)
        {
            return false;
        }

        var count = await context.LearningOutcomes.AsNoTracking()
            .CountAsync(
                x =>
                    x.SchoolId == schoolId &&
                    x.CurriculumAdoptionId == curriculumAdoptionId &&
                    distinct.Contains(x.Id),
                cancellationToken);

        return count == distinct.Length;
    }

    public async Task AddAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<PracticeAttemptItem> items,
        IReadOnlyCollection<StudentItemExposure> exposures,
        CancellationToken cancellationToken = default)
    {
        context.PracticeAttempts.Add(attempt);
        context.PracticeAttemptItems.AddRange(items);
        context.StudentItemExposures.AddRange(exposures);
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task<PracticeAttempt?> GetAttemptAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default) =>
        context.PracticeAttempts.SingleOrDefaultAsync(
            x => x.SchoolId == schoolId && x.Id == attemptId,
            cancellationToken);

    public async Task<IReadOnlyList<PracticeAttemptItem>> GetAttemptItemsAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default) =>
        await context.PracticeAttemptItems.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && x.PracticeAttemptId == attemptId)
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<PracticeResponse>> GetResponsesAsync(
        Guid schoolId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        var itemIds = context.PracticeAttemptItems.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && x.PracticeAttemptId == attemptId)
            .Select(x => x.Id);

        return await context.PracticeResponses.AsNoTracking()
            .Where(x => x.SchoolId == schoolId && itemIds.Contains(x.PracticeAttemptItemId))
            .ToListAsync(cancellationToken);
    }

    public async Task SaveResponseAsync(
        PracticeResponse response,
        CancellationToken cancellationToken = default)
    {
        var existing = await context.PracticeResponses.SingleOrDefaultAsync(
            x => x.SchoolId == response.SchoolId && x.PracticeAttemptItemId == response.PracticeAttemptItemId,
            cancellationToken);

        if (existing is null)
        {
            context.PracticeResponses.Add(response);
        }
        else
        {
            existing.Answer = response.Answer;
            existing.IsCorrect = response.IsCorrect;
            existing.Score = response.Score;
            existing.Feedback = response.Feedback;
            existing.AnsweredAtUtc = response.AnsweredAtUtc;
        }

        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task CompleteAttemptAsync(
        PracticeAttempt attempt,
        IReadOnlyCollection<LearningEvidence> evidence,
        CancellationToken cancellationToken = default)
    {
        context.LearningEvidence.AddRange(evidence);
        await context.SaveChangesAsync(cancellationToken);
    }
}
''')

write("src/Edulytics.Services/Practice/PracticeContracts.cs", r'''
using Edulytics.Core.Enums;

namespace Edulytics.Services.Practice;

public enum PracticeErrorCode
{
    StudentNotFound,
    AccessDenied,
    NotEnrolled,
    Required,
    ItemNotFound,
    ItemScopeMismatch,
    ItemMissingOutcome,
    OutcomeScopeMismatch,
    AttemptNotFound,
    AttemptNotInProgress,
    AttemptIncomplete,
    ResponseItemMismatch,
    InvalidAnswer
}

public sealed record PracticeQueryResult<T>(T? Value, PracticeErrorCode? Error) where T : class
{
    public static PracticeQueryResult<T> Success(T value) => new(value, null);
    public static PracticeQueryResult<T> Failure(PracticeErrorCode error) => new(null, error);
}

public sealed record PracticeCommandResult(bool Succeeded, PracticeErrorCode? Error, Guid? EntityId = null)
{
    public static PracticeCommandResult Success(Guid? id = null) => new(true, null, id);
    public static PracticeCommandResult Failure(PracticeErrorCode error) => new(false, error);
}

public sealed record PracticeItemSummary(
    Guid Id,
    AssessmentItemType ItemType,
    AssessmentItemDifficulty Difficulty,
    string Prompt,
    Guid? LessonId,
    IReadOnlyList<Guid> OutcomeIds);

public sealed record PracticeAttemptQuestion(
    Guid AttemptItemId,
    Guid AssessmentItemId,
    int Order,
    AssessmentItemType ItemType,
    AssessmentItemDifficulty Difficulty,
    string Prompt,
    bool Answered,
    bool? IsCorrect,
    string? Feedback);

public sealed record PracticeAttemptDetails(
    Guid AttemptId,
    PracticeAttemptStatus Status,
    DateTime StartedAtUtc,
    DateTime? SubmittedAtUtc,
    decimal Score,
    decimal MaxScore,
    decimal Percentage,
    IReadOnlyList<PracticeAttemptQuestion> Questions);

public sealed record PracticeFeedback(
    Guid AttemptItemId,
    bool IsCorrect,
    decimal Score,
    string Solution);
''')

write("src/Edulytics.Services/Practice/IPracticeService.cs", r'''
namespace Edulytics.Services.Practice;

public interface IPracticeService
{
    Task<PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>> ListAvailableAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default);

    Task<PracticeCommandResult> StartAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        IReadOnlyList<Guid> itemIds,
        CancellationToken cancellationToken = default);

    Task<PracticeQueryResult<PracticeAttemptDetails>> GetAttemptAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default);

    Task<PracticeQueryResult<PracticeFeedback>> AnswerAsync(
        Guid studentUserId,
        Guid attemptId,
        Guid attemptItemId,
        string answer,
        CancellationToken cancellationToken = default);

    Task<PracticeQueryResult<PracticeAttemptDetails>> SubmitAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default);
}
''')

write("src/Edulytics.Services/Practice/PracticeService.cs", r'''
using System.Globalization;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.Practice;

namespace Edulytics.Services.Practice;

public sealed class PracticeService(IPracticeRepository repository) : IPracticeService
{
    private const int MaxItemsPerAttempt = 50;

    public async Task<PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>> ListAvailableAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        Guid? lessonId,
        CancellationToken cancellationToken = default)
    {
        var student = await repository.FindStudentByUserIdAsync(studentUserId, cancellationToken);
        if (student is null)
        {
            return PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>.Failure(PracticeErrorCode.StudentNotFound);
        }

        if (!await repository.IsEnrolledInAdoptionAsync(
                student.SchoolId,
                student.Id,
                curriculumAdoptionId,
                cancellationToken))
        {
            return PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>.Failure(PracticeErrorCode.NotEnrolled);
        }

        var items = await repository.ListItemsAsync(student.SchoolId, curriculumAdoptionId, lessonId, cancellationToken);
        var mappings = await repository.GetOutcomeIdsAsync(student.SchoolId, items.Select(x => x.Id).ToArray(), cancellationToken);

        var result = items
            .Where(x => mappings.TryGetValue(x.Id, out var outcomeIds) && outcomeIds.Count > 0)
            .Select(x => new PracticeItemSummary(
                x.Id,
                x.ItemType,
                x.Difficulty,
                x.Prompt,
                x.CurriculumPedagogicalLessonId,
                mappings[x.Id]))
            .ToArray();

        return PracticeQueryResult<IReadOnlyList<PracticeItemSummary>>.Success(result);
    }

    public async Task<PracticeCommandResult> StartAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        IReadOnlyList<Guid> itemIds,
        CancellationToken cancellationToken = default)
    {
        if (itemIds.Count is < 1 or > MaxItemsPerAttempt || itemIds.Distinct().Count() != itemIds.Count)
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.Required);
        }

        var student = await repository.FindStudentByUserIdAsync(studentUserId, cancellationToken);
        if (student is null)
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.StudentNotFound);
        }

        if (!await repository.IsEnrolledInAdoptionAsync(
                student.SchoolId,
                student.Id,
                curriculumAdoptionId,
                cancellationToken))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.NotEnrolled);
        }

        var items = await repository.GetItemsAsync(student.SchoolId, itemIds, cancellationToken);
        if (items.Count != itemIds.Count)
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.ItemNotFound);
        }

        if (items.Any(x => x.CurriculumAdoptionId != curriculumAdoptionId))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.ItemScopeMismatch);
        }

        var mappings = await repository.GetOutcomeIdsAsync(student.SchoolId, itemIds, cancellationToken);
        if (items.Any(x => !mappings.TryGetValue(x.Id, out var outcomes) || outcomes.Count == 0))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.ItemMissingOutcome);
        }

        var allOutcomeIds = mappings.Values.SelectMany(x => x).Distinct().ToArray();
        if (!await repository.OutcomesBelongToAdoptionAsync(
                student.SchoolId,
                curriculumAdoptionId,
                allOutcomeIds,
                cancellationToken))
        {
            return PracticeCommandResult.Failure(PracticeErrorCode.OutcomeScopeMismatch);
        }

        var now = DateTime.UtcNow;
        var attempt = new PracticeAttempt
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            StudentProfileId = student.Id,
            CurriculumAdoptionId = curriculumAdoptionId,
            CurriculumPedagogicalLessonId = SingleLessonOrNull(items),
            Status = PracticeAttemptStatus.InProgress,
            StartedAtUtc = now,
            Score = 0,
            MaxScore = itemIds.Count,
            Percentage = 0,
            RowVersion = []
        };

        var byId = items.ToDictionary(x => x.Id);
        var attemptItems = itemIds.Select((id, index) => new PracticeAttemptItem
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            PracticeAttemptId = attempt.Id,
            AssessmentItemId = id,
            Order = index + 1,
            MaxScore = 1m
        }).ToArray();

        var exposures = itemIds.Select(id => new StudentItemExposure
        {
            Id = Guid.NewGuid(),
            SchoolId = student.SchoolId,
            StudentProfileId = student.Id,
            AssessmentItemId = id,
            ExposureFingerprint = byId[id].ExposureFingerprint,
            ExposedAtUtc = now
        }).ToArray();

        await repository.AddAttemptAsync(attempt, attemptItems, exposures, cancellationToken);
        return PracticeCommandResult.Success(attempt.Id);
    }

    public async Task<PracticeQueryResult<PracticeAttemptDetails>> GetAttemptAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        var access = await GetOwnedAttemptAsync(studentUserId, attemptId, cancellationToken);
        if (access.Error.HasValue)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Failure(access.Error.Value);
        }

        return PracticeQueryResult<PracticeAttemptDetails>.Success(
            await BuildDetailsAsync(access.Student!, access.Attempt!, cancellationToken));
    }

    public async Task<PracticeQueryResult<PracticeFeedback>> AnswerAsync(
        Guid studentUserId,
        Guid attemptId,
        Guid attemptItemId,
        string answer,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(answer) || answer.Trim().Length > 2000)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.InvalidAnswer);
        }

        var access = await GetOwnedAttemptAsync(studentUserId, attemptId, cancellationToken);
        if (access.Error.HasValue)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(access.Error.Value);
        }

        var attempt = access.Attempt!;
        if (attempt.Status != PracticeAttemptStatus.InProgress)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.AttemptNotInProgress);
        }

        var attemptItems = await repository.GetAttemptItemsAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var attemptItem = attemptItems.SingleOrDefault(x => x.Id == attemptItemId);
        if (attemptItem is null)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.ResponseItemMismatch);
        }

        var itemList = await repository.GetItemsAsync(attempt.SchoolId, [attemptItem.AssessmentItemId], cancellationToken);
        var item = itemList.SingleOrDefault();
        if (item is null)
        {
            return PracticeQueryResult<PracticeFeedback>.Failure(PracticeErrorCode.ItemNotFound);
        }

        var correct = AnswersMatch(answer, item.CorrectAnswer);
        var score = correct ? attemptItem.MaxScore : 0m;
        var response = new PracticeResponse
        {
            Id = Guid.NewGuid(),
            SchoolId = attempt.SchoolId,
            PracticeAttemptItemId = attemptItem.Id,
            Answer = answer.Trim(),
            IsCorrect = correct,
            Score = score,
            Feedback = item.Solution,
            AnsweredAtUtc = DateTime.UtcNow
        };

        await repository.SaveResponseAsync(response, cancellationToken);
        return PracticeQueryResult<PracticeFeedback>.Success(
            new PracticeFeedback(attemptItem.Id, correct, score, item.Solution));
    }

    public async Task<PracticeQueryResult<PracticeAttemptDetails>> SubmitAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken = default)
    {
        var access = await GetOwnedAttemptAsync(studentUserId, attemptId, cancellationToken);
        if (access.Error.HasValue)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Failure(access.Error.Value);
        }

        var attempt = access.Attempt!;
        if (attempt.Status == PracticeAttemptStatus.Submitted)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Success(
                await BuildDetailsAsync(access.Student!, attempt, cancellationToken));
        }

        var attemptItems = await repository.GetAttemptItemsAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var responses = await repository.GetResponsesAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        if (attemptItems.Count == 0 || responses.Count != attemptItems.Count)
        {
            return PracticeQueryResult<PracticeAttemptDetails>.Failure(PracticeErrorCode.AttemptIncomplete);
        }

        var items = await repository.GetItemsAsync(
            attempt.SchoolId,
            attemptItems.Select(x => x.AssessmentItemId).ToArray(),
            cancellationToken);
        var itemById = items.ToDictionary(x => x.Id);
        var mappings = await repository.GetOutcomeIdsAsync(
            attempt.SchoolId,
            items.Select(x => x.Id).ToArray(),
            cancellationToken);
        var responseByAttemptItemId = responses.ToDictionary(x => x.PracticeAttemptItemId);

        var now = DateTime.UtcNow;
        var evidence = new List<LearningEvidence>();
        foreach (var attemptItem in attemptItems)
        {
            var item = itemById[attemptItem.AssessmentItemId];
            var response = responseByAttemptItemId[attemptItem.Id];
            foreach (var outcomeId in mappings[item.Id])
            {
                evidence.Add(new LearningEvidence
                {
                    Id = Guid.NewGuid(),
                    SchoolId = attempt.SchoolId,
                    StudentProfileId = attempt.StudentProfileId,
                    LearningOutcomeId = outcomeId,
                    PracticeAttemptId = attempt.Id,
                    AssessmentItemId = item.Id,
                    EvidenceType = LearningEvidenceType.Practice,
                    Difficulty = item.Difficulty,
                    IsCorrect = response.IsCorrect,
                    Score = response.Score,
                    MaxScore = attemptItem.MaxScore,
                    OccurredAtUtc = now
                });
            }
        }

        attempt.Score = responses.Sum(x => x.Score);
        attempt.MaxScore = attemptItems.Sum(x => x.MaxScore);
        attempt.Percentage = attempt.MaxScore == 0m
            ? 0m
            : Math.Round(attempt.Score / attempt.MaxScore * 100m, 2, MidpointRounding.AwayFromZero);
        attempt.Status = PracticeAttemptStatus.Submitted;
        attempt.SubmittedAtUtc = now;

        await repository.CompleteAttemptAsync(attempt, evidence, cancellationToken);
        return PracticeQueryResult<PracticeAttemptDetails>.Success(
            await BuildDetailsAsync(access.Student!, attempt, cancellationToken));
    }

    private async Task<PracticeAttemptDetails> BuildDetailsAsync(
        StudentProfile student,
        PracticeAttempt attempt,
        CancellationToken cancellationToken)
    {
        var attemptItems = await repository.GetAttemptItemsAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var items = await repository.GetItemsAsync(
            attempt.SchoolId,
            attemptItems.Select(x => x.AssessmentItemId).ToArray(),
            cancellationToken);
        var itemById = items.ToDictionary(x => x.Id);
        var responses = await repository.GetResponsesAsync(attempt.SchoolId, attempt.Id, cancellationToken);
        var responseByAttemptItem = responses.ToDictionary(x => x.PracticeAttemptItemId);

        var questions = attemptItems.Select(link =>
        {
            var item = itemById[link.AssessmentItemId];
            responseByAttemptItem.TryGetValue(link.Id, out var response);
            return new PracticeAttemptQuestion(
                link.Id,
                item.Id,
                link.Order,
                item.ItemType,
                item.Difficulty,
                item.Prompt,
                response is not null,
                response?.IsCorrect,
                response?.Feedback);
        }).ToArray();

        return new PracticeAttemptDetails(
            attempt.Id,
            attempt.Status,
            attempt.StartedAtUtc,
            attempt.SubmittedAtUtc,
            attempt.Score,
            attempt.MaxScore,
            attempt.Percentage,
            questions);
    }

    private async Task<(StudentProfile? Student, PracticeAttempt? Attempt, PracticeErrorCode? Error)> GetOwnedAttemptAsync(
        Guid studentUserId,
        Guid attemptId,
        CancellationToken cancellationToken)
    {
        var student = await repository.FindStudentByUserIdAsync(studentUserId, cancellationToken);
        if (student is null)
        {
            return (null, null, PracticeErrorCode.StudentNotFound);
        }

        var attempt = await repository.GetAttemptAsync(student.SchoolId, attemptId, cancellationToken);
        if (attempt is null)
        {
            return (student, null, PracticeErrorCode.AttemptNotFound);
        }

        if (attempt.StudentProfileId != student.Id)
        {
            return (student, null, PracticeErrorCode.AccessDenied);
        }

        return (student, attempt, null);
    }

    private static Guid? SingleLessonOrNull(IReadOnlyList<AssessmentItem> items)
    {
        var values = items.Select(x => x.CurriculumPedagogicalLessonId).Distinct().ToArray();
        return values.Length == 1 ? values[0] : null;
    }

    internal static bool AnswersMatch(string submitted, string expected)
    {
        var left = submitted.Trim();
        var right = expected.Trim();
        if (string.Equals(left, right, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        static bool TryNumber(string value, out decimal number) =>
            decimal.TryParse(
                value.Replace(',', '.'),
                NumberStyles.Number | NumberStyles.AllowLeadingSign,
                CultureInfo.InvariantCulture,
                out number);

        return TryNumber(left, out var leftNumber) &&
               TryNumber(right, out var rightNumber) &&
               leftNumber == rightNumber;
    }
}
''')

write("src/Edulytics.Web/Extensions/PracticeRegistrationExtensions.cs", r'''
using Edulytics.Core.Practice;
using Edulytics.Data.Repositories;
using Edulytics.Services.Practice;

namespace Edulytics.Web.Extensions;

public static class PracticeRegistrationExtensions
{
    public static IServiceCollection AddPracticePhase30(this IServiceCollection services)
    {
        services.AddScoped<IPracticeRepository, PracticeRepository>();
        services.AddScoped<IPracticeService, PracticeService>();
        return services;
    }
}
''')

replace_once(
    "src/Edulytics.Data/Contexts/EdulyticsDbContext.cs",
    "    public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();\n",
    "    public DbSet<StudentAnswer> StudentAnswers => Set<StudentAnswer>();\n"
    "    public DbSet<AssessmentItem> AssessmentItems => Set<AssessmentItem>();\n"
    "    public DbSet<AssessmentItemOutcome> AssessmentItemOutcomes => Set<AssessmentItemOutcome>();\n"
    "    public DbSet<PracticeAttempt> PracticeAttempts => Set<PracticeAttempt>();\n"
    "    public DbSet<PracticeAttemptItem> PracticeAttemptItems => Set<PracticeAttemptItem>();\n"
    "    public DbSet<PracticeResponse> PracticeResponses => Set<PracticeResponse>();\n"
    "    public DbSet<LearningEvidence> LearningEvidence => Set<LearningEvidence>();\n"
    "    public DbSet<StudentItemExposure> StudentItemExposures => Set<StudentItemExposure>();\n")

replace_once(
    "src/Edulytics.Data/Contexts/EdulyticsDbContext.cs",
    "        builder.ApplyConfiguration(new StudentAnswerConfiguration());\n",
    "        builder.ApplyConfiguration(new StudentAnswerConfiguration());\n"
    "        builder.ApplyConfiguration(new AssessmentItemConfiguration());\n"
    "        builder.ApplyConfiguration(new AssessmentItemOutcomeConfiguration());\n"
    "        builder.ApplyConfiguration(new PracticeAttemptConfiguration());\n"
    "        builder.ApplyConfiguration(new PracticeAttemptItemConfiguration());\n"
    "        builder.ApplyConfiguration(new PracticeResponseConfiguration());\n"
    "        builder.ApplyConfiguration(new LearningEvidenceConfiguration());\n"
    "        builder.ApplyConfiguration(new StudentItemExposureConfiguration());\n")

replace_once(
    "src/Edulytics.Web/Program.cs",
    "builder.Services\n    .AddLessonContentPhase29();\n",
    "builder.Services\n    .AddLessonContentPhase29();\n\n"
    "builder.Services\n    .AddPracticePhase30();\n")

write("tests/Edulytics.Tests/Phase30/Phase30PracticeEngineTests.cs", r'''
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Data.Contexts;
using Edulytics.Data.Repositories;
using Edulytics.Services.Practice;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Tests.Phase30;

public sealed class Phase30PracticeEngineTests
{
    [Fact]
    public void NumericAnswerComparison_IsCultureTolerantAndDeterministic()
    {
        Assert.True(PracticeService.AnswersMatch(" 2,50 ", "2.5"));
        Assert.True(PracticeService.AnswersMatch("X = 7", "x = 7"));
        Assert.False(PracticeService.AnswersMatch("7", "8"));
    }

    [Fact]
    public async Task PracticeAttempt_RecordsExposureResponseAndOutcomeEvidence_WithoutOfficialGrade()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var service = new PracticeService(new PracticeRepository(context));

        var start = await service.StartAsync(
            fixture.StudentUserId,
            fixture.AdoptionId,
            [fixture.ItemId]);

        Assert.True(start.Succeeded);
        var attemptId = Assert.IsType<Guid>(start.EntityId);

        var details = await service.GetAttemptAsync(fixture.StudentUserId, attemptId);
        Assert.NotNull(details.Value);
        var question = Assert.Single(details.Value!.Questions);

        var feedback = await service.AnswerAsync(
            fixture.StudentUserId,
            attemptId,
            question.AttemptItemId,
            "6");

        Assert.NotNull(feedback.Value);
        Assert.True(feedback.Value!.IsCorrect);
        Assert.Equal("Divide both sides by 3.", feedback.Value.Solution);

        var submitted = await service.SubmitAsync(fixture.StudentUserId, attemptId);
        Assert.NotNull(submitted.Value);
        Assert.Equal(PracticeAttemptStatus.Submitted, submitted.Value!.Status);
        Assert.Equal(100m, submitted.Value.Percentage);

        var evidence = await context.LearningEvidence.SingleAsync();
        Assert.Equal(fixture.OutcomeId, evidence.LearningOutcomeId);
        Assert.Equal(LearningEvidenceType.Practice, evidence.EvidenceType);
        Assert.Equal(AssessmentItemDifficulty.Medium, evidence.Difficulty);
        Assert.True(evidence.IsCorrect);

        var exposure = await context.StudentItemExposures.SingleAsync();
        Assert.Equal("linear-equation-family:a1:b0:c6", exposure.ExposureFingerprint);

        Assert.Empty(await context.AssessmentResults.ToListAsync());
    }

    [Fact]
    public async Task PracticeAttempt_RejectsItemsFromAnotherCurriculumAdoption()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var foreignItem = await AddItemAsync(
            context,
            fixture.SchoolId,
            Guid.NewGuid(),
            fixture.OutcomeId,
            "foreign-fingerprint");

        var service = new PracticeService(new PracticeRepository(context));
        var result = await service.StartAsync(
            fixture.StudentUserId,
            fixture.AdoptionId,
            [foreignItem.Id]);

        Assert.False(result.Succeeded);
        Assert.Equal(PracticeErrorCode.ItemScopeMismatch, result.Error);
        Assert.Empty(await context.PracticeAttempts.ToListAsync());
    }

    [Fact]
    public async Task Student_CannotReadAnotherStudentsPracticeAttempt()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var service = new PracticeService(new PracticeRepository(context));
        var start = await service.StartAsync(fixture.StudentUserId, fixture.AdoptionId, [fixture.ItemId]);
        var attemptId = Assert.IsType<Guid>(start.EntityId);

        var otherUser = Guid.NewGuid();
        var otherStudent = NewStudent(fixture.SchoolId, otherUser, "S-OTHER");
        context.StudentProfiles.Add(otherStudent);
        context.StudentEnrollments.Add(new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = fixture.SchoolId,
            StudentProfileId = otherStudent.Id,
            ClassGroupId = fixture.ClassGroupId,
            AcademicYearId = fixture.AcademicYearId,
            EnrolledAtUtc = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        var result = await service.GetAttemptAsync(otherUser, attemptId);
        Assert.Null(result.Value);
        Assert.Equal(PracticeErrorCode.AccessDenied, result.Error);
    }

    [Fact]
    public async Task SubmittedAttempt_IsIdempotentAndDoesNotDuplicateEvidence()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var service = new PracticeService(new PracticeRepository(context));
        var start = await service.StartAsync(fixture.StudentUserId, fixture.AdoptionId, [fixture.ItemId]);
        var attemptId = Assert.IsType<Guid>(start.EntityId);
        var details = (await service.GetAttemptAsync(fixture.StudentUserId, attemptId)).Value!;
        await service.AnswerAsync(fixture.StudentUserId, attemptId, details.Questions[0].AttemptItemId, "6");

        var first = await service.SubmitAsync(fixture.StudentUserId, attemptId);
        var second = await service.SubmitAsync(fixture.StudentUserId, attemptId);

        Assert.NotNull(first.Value);
        Assert.NotNull(second.Value);
        Assert.Equal(1, await context.LearningEvidence.CountAsync());
    }

    [Fact]
    public async Task AssessmentItem_PreservesReconstructionAndValidationMetadata()
    {
        await using var context = CreateContext();
        var fixture = await SeedAsync(context);
        var item = await context.AssessmentItems.SingleAsync(x => x.Id == fixture.ItemId);

        Assert.Equal(AssessmentItemSource.SystemGenerated, item.Source);
        Assert.Equal(AssessmentItemType.Numeric, item.ItemType);
        Assert.Equal(AssessmentItemDifficulty.Medium, item.Difficulty);
        Assert.Equal("3x = 18. Find x.", item.Prompt);
        Assert.Equal("6", item.CorrectAnswer);
        Assert.Equal("Divide both sides by 3.", item.Solution);
        Assert.Equal("native-template-v1", item.GenerationMethod);
        Assert.Equal("linear-equation-family", item.GenerationFamily);
        Assert.Contains("coefficient", item.GenerationParametersJson);
        Assert.Contains("validated", item.ValidationMetadataJson);
    }

    private static EdulyticsDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<EdulyticsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString("N"))
            .Options;
        return new EdulyticsDbContext(options);
    }

    private static async Task<Fixture> SeedAsync(EdulyticsDbContext context)
    {
        var schoolId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var student = NewStudent(schoolId, userId, "S-001");
        var academicYearId = Guid.NewGuid();
        var classGroupId = Guid.NewGuid();
        var adoptionId = Guid.NewGuid();
        var outcomeId = Guid.NewGuid();

        context.StudentProfiles.Add(student);
        context.ClassGroups.Add(new ClassGroup
        {
            Id = classGroupId,
            SchoolId = schoolId,
            AcademicYearId = academicYearId,
            AcademicProgramId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            CurriculumAdoptionId = adoptionId,
            Name = "Class A",
            NormalizedName = "CLASS A",
            Code = "A",
            NormalizedCode = "A",
            Status = AcademicStructureStatus.Active,
            RowVersion = []
        });
        context.StudentEnrollments.Add(new StudentEnrollment
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            StudentProfileId = student.Id,
            ClassGroupId = classGroupId,
            AcademicYearId = academicYearId,
            EnrolledAtUtc = DateTime.UtcNow
        });
        context.LearningOutcomes.Add(new LearningOutcome
        {
            Id = outcomeId,
            SchoolId = schoolId,
            AcademicProgramId = Guid.NewGuid(),
            FrameworkVersionId = Guid.NewGuid(),
            SubjectId = Guid.NewGuid(),
            GradeLevelId = Guid.NewGuid(),
            CurriculumAdoptionId = adoptionId,
            TopicId = Guid.NewGuid(),
            Code = "M.TEST.1",
            Description = "Solve a linear equation.",
            Weight = 1,
            Order = 1
        });
        await context.SaveChangesAsync();

        var item = await AddItemAsync(
            context,
            schoolId,
            adoptionId,
            outcomeId,
            "linear-equation-family:a1:b0:c6");

        return new Fixture(
            schoolId,
            userId,
            student.Id,
            academicYearId,
            classGroupId,
            adoptionId,
            outcomeId,
            item.Id);
    }

    private static StudentProfile NewStudent(Guid schoolId, Guid userId, string number) => new()
    {
        Id = Guid.NewGuid(),
        SchoolId = schoolId,
        UserId = userId,
        StudentNumber = number,
        NormalizedStudentNumber = number,
        FirstName = "Test",
        LastName = "Student",
        DisplayName = "Test Student",
        Status = AcademicStructureStatus.Active,
        CreatedAtUtc = DateTime.UtcNow,
        UpdatedAtUtc = DateTime.UtcNow,
        RowVersion = []
    };

    private static async Task<AssessmentItem> AddItemAsync(
        EdulyticsDbContext context,
        Guid schoolId,
        Guid adoptionId,
        Guid outcomeId,
        string fingerprint)
    {
        var item = new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            CurriculumAdoptionId = adoptionId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = AssessmentItemType.Numeric,
            Difficulty = AssessmentItemDifficulty.Medium,
            Prompt = "3x = 18. Find x.",
            CorrectAnswer = "6",
            Solution = "Divide both sides by 3.",
            GenerationMethod = "native-template-v1",
            GenerationFamily = "linear-equation-family",
            GenerationParametersJson = "{\"coefficient\":3,\"result\":18}",
            ExposureFingerprint = fingerprint,
            ValidationMetadataJson = "{\"validated\":true}",
            CreatedAtUtc = DateTime.UtcNow,
            RowVersion = []
        };
        context.AssessmentItems.Add(item);
        context.AssessmentItemOutcomes.Add(new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = schoolId,
            AssessmentItemId = item.Id,
            LearningOutcomeId = outcomeId
        });
        await context.SaveChangesAsync();
        return item;
    }

    private sealed record Fixture(
        Guid SchoolId,
        Guid StudentUserId,
        Guid StudentProfileId,
        Guid AcademicYearId,
        Guid ClassGroupId,
        Guid AdoptionId,
        Guid OutcomeId,
        Guid ItemId);
}
''')

write("docs/PHASE_30_IMPLEMENTATION.md", r'''
# Phase 30 — Practice & Assessment Item Engine

## Scope

Phase 30 introduces the durable assessment-item and student-practice evidence foundation required by Phases 31–36.
It deliberately does **not** implement mastery scoring, assessment-blueprint intelligence, dynamic question generation,
adaptive testing, or a traditional Question Bank.

## Data model

- `AssessmentItem`: exact reconstructable item content and validation/generation metadata.
- `AssessmentItemOutcome`: explicit Learning Outcome mapping.
- `PracticeAttempt` / `PracticeAttemptItem`: non-grade student practice lifecycle.
- `PracticeResponse`: exact student answer, deterministic correctness, score and solution feedback.
- `LearningEvidence`: raw Outcome-level evidence for the future deterministic mastery engine.
- `StudentItemExposure`: exposure fingerprint history for later duplicate/reassessment exclusion.

## Security / product invariants

- Practice is never an Official Grade and does not create `AssessmentResult` rows.
- Student identity is resolved from `StudentProfile.UserId` server-side.
- Curriculum access is derived through `StudentEnrollment -> ClassGroup -> CurriculumAdoptionId`.
- Items and Outcome mappings must match the student's school and curriculum adoption.
- Attempts are student-owned and cross-student reads are denied.
- Submitted attempts are idempotent and do not duplicate evidence.
- Every used item remains reconstructable.

## Deferred by design

- Mastery calculation: Phase 31.
- Assessment blueprint intelligence: Phase 32.
- Dynamic mathematics generation: Phase 33.
- Full teacher/student exam generation: Phase 34.
- Adaptive/diagnostic behavior: Phase 35.
- Equivalent reassessment: Phase 36.
''')

print("PHASE30 PATCH COMPLETE")
