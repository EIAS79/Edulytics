using Edulytics.Core.Entities;
using Edulytics.Core.Practice;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class StudentPrivatePracticeRepository(EdulyticsDbContext db)
    : IStudentPrivatePracticeRepository
{
    public async Task<IReadOnlyList<PrivatePracticeCurriculumOption>> ListCurriculaAsync(
        Guid studentUserId,
        CancellationToken cancellationToken = default)
    {
        var student = await db.StudentProfiles.AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == studentUserId && !x.IsArchived, cancellationToken);
        if (student is null) return [];

        var enrollments = await db.StudentEnrollments.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && x.StudentProfileId == student.Id)
            .ToListAsync(cancellationToken);
        var classIds = enrollments.Select(x => x.ClassGroupId).ToArray();
        var classes = await db.ClassGroups.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && classIds.Contains(x.Id))
            .ToListAsync(cancellationToken);
        var adoptionIds = classes.Where(x => x.CurriculumAdoptionId.HasValue)
            .Select(x => x.CurriculumAdoptionId!.Value).Distinct().ToArray();
        var adoptions = await db.SchoolCurriculumAdoptions.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && x.IsActive && adoptionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var result = new List<PrivatePracticeCurriculumOption>();
        foreach (var enrollment in enrollments)
        {
            var classGroup = classes.FirstOrDefault(x => x.Id == enrollment.ClassGroupId);
            if (classGroup?.CurriculumAdoptionId is not Guid adoptionId) continue;
            var adoption = adoptions.FirstOrDefault(x => x.Id == adoptionId);
            if (adoption is null || string.IsNullOrWhiteSpace(adoption.CurriculumLevelKey)) continue;
            result.Add(new PrivatePracticeCurriculumOption(
                adoption.Id,
                classGroup.Id,
                enrollment.AcademicYearId,
                adoption.CurriculumLevelLabel ?? adoption.CurriculumLevelKey!,
                classGroup.Name));
        }

        return result
            .GroupBy(x => x.CurriculumAdoptionId)
            .Select(x => x.First())
            .OrderBy(x => x.CurriculumLevelLabel)
            .ThenBy(x => x.ClassName)
            .ToArray();
    }

    public async Task<StudentPrivatePracticeContext?> GetContextAsync(
        Guid studentUserId,
        Guid curriculumAdoptionId,
        CancellationToken cancellationToken = default)
    {
        var student = await db.StudentProfiles.AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == studentUserId && !x.IsArchived, cancellationToken);
        if (student is null) return null;

        var adoption = await db.SchoolCurriculumAdoptions.AsNoTracking()
            .SingleOrDefaultAsync(x =>
                x.Id == curriculumAdoptionId &&
                x.SchoolId == student.SchoolId &&
                x.IsActive,
                cancellationToken);
        if (adoption is null || string.IsNullOrWhiteSpace(adoption.CurriculumLevelKey) || !adoption.CurriculumLogicalLevel.HasValue)
            return null;

        var enrollments = await db.StudentEnrollments.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && x.StudentProfileId == student.Id)
            .ToListAsync(cancellationToken);
        var classIds = enrollments.Select(x => x.ClassGroupId).ToArray();
        var classes = await db.ClassGroups.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && classIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        ClassGroup? classGroup = null;
        StudentEnrollment? enrollment = null;
        foreach (var candidate in classes)
        {
            var match = candidate.CurriculumAdoptionId == adoption.Id ||
                (!candidate.CurriculumAdoptionId.HasValue &&
                 candidate.AcademicProgramId == adoption.AcademicProgramId &&
                 candidate.GradeLevelId == adoption.GradeLevelId);
            if (!match) continue;
            var candidateEnrollment = enrollments.FirstOrDefault(x =>
                x.ClassGroupId == candidate.Id &&
                (!adoption.AcademicYearId.HasValue || x.AcademicYearId == adoption.AcademicYearId.Value));
            if (candidateEnrollment is null) continue;
            classGroup = candidate;
            enrollment = candidateEnrollment;
            break;
        }
        if (classGroup is null || enrollment is null) return null;

        // Private practice and the teacher Builder must consume the same authoritative
        // adopted curriculum outcomes. Existing adoptions are repaired idempotently here.
        await OfficialCurriculumOutcomeMaterializer.EnsureAsync(
            db,
            adoption,
            cancellationToken);

        var outcomes = await db.LearningOutcomes.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.AcademicProgramId == adoption.AcademicProgramId &&
                x.GradeLevelId == adoption.GradeLevelId &&
                x.SubjectId == adoption.SubjectId &&
                (x.CurriculumAdoptionId == adoption.Id || x.CurriculumAdoptionId == null))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Code)
            .ToListAsync(cancellationToken);

        var logicalLevel = adoption.CurriculumLogicalLevel.Value;
        var pathway = adoption.CurriculumPathway;
        var lessons = await db.CurriculumPedagogicalLessons.AsNoTracking()
            .Where(x =>
                x.FrameworkVersionId == adoption.FrameworkVersionId &&
                x.LogicalLevelFrom <= logicalLevel && logicalLevel <= x.LogicalLevelTo &&
                (string.IsNullOrWhiteSpace(pathway)
                    ? x.Pathway == null || x.Pathway == ""
                    : x.Pathway == pathway))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
        var lessonIds = lessons.Select(x => x.Id).ToArray();
        var lessonOutcomes = await db.CurriculumPedagogicalLessonOutcomes.AsNoTracking()
            .Where(x => lessonIds.Contains(x.PedagogicalLessonId))
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);

        MathematicsOutcomeSemanticHintLoader.Apply(outcomes, lessons, lessonOutcomes);

        var masteries = await db.StudentOutcomeMasteries.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.StudentProfileId == student.Id &&
                x.ClassGroupId == classGroup.Id &&
                x.SubjectId == adoption.SubjectId)
            .ToListAsync(cancellationToken);

        var exposures = await db.StudentItemExposures.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && x.StudentProfileId == student.Id)
            .OrderByDescending(x => x.ExposedAtUtc)
            .Take(2000)
            .ToListAsync(cancellationToken);

        return new StudentPrivatePracticeContext(
            student, adoption, classGroup, enrollment, outcomes, lessons, lessonOutcomes, masteries, exposures);
    }

    public async Task AddGeneratedAttemptAsync(
        IReadOnlyList<AssessmentItem> items,
        IReadOnlyList<AssessmentItemOutcome> itemOutcomes,
        PracticeAttempt attempt,
        IReadOnlyList<PracticeAttemptItem> attemptItems,
        IReadOnlyList<StudentItemExposure> exposures,
        CancellationToken cancellationToken = default)
    {
        await db.AssessmentItems.AddRangeAsync(items, cancellationToken);
        await db.AssessmentItemOutcomes.AddRangeAsync(itemOutcomes, cancellationToken);
        await db.PracticeAttempts.AddAsync(attempt, cancellationToken);
        await db.PracticeAttemptItems.AddRangeAsync(attemptItems, cancellationToken);
        await db.StudentItemExposures.AddRangeAsync(exposures, cancellationToken);
        await db.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PrivatePracticeAttemptSummary>> ListPrivateAttemptsAsync(
        Guid studentUserId,
        CancellationToken cancellationToken = default)
    {
        var student = await db.StudentProfiles.AsNoTracking()
            .SingleOrDefaultAsync(x => x.UserId == studentUserId && !x.IsArchived, cancellationToken);
        if (student is null) return [];

        return await db.PracticeAttempts.AsNoTracking()
            .Where(x => x.SchoolId == student.SchoolId && x.StudentProfileId == student.Id && x.IsPrivate)
            .OrderByDescending(x => x.StartedAtUtc)
            .Take(100)
            .Select(x => new PrivatePracticeAttemptSummary(
                x.Id, x.CurriculumAdoptionId, x.CurriculumPedagogicalLessonId,
                x.Status, x.StartedAtUtc, x.SubmittedAtUtc, x.Score, x.MaxScore, x.Percentage))
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<PrivatePracticeEvidenceItem>> ListPrivateEvidenceAsync(
        Guid studentUserId,
        CancellationToken cancellationToken = default)
    {
        var student = await db.StudentProfiles.AsNoTracking()
            .SingleOrDefaultAsync(
                x =>
                    x.UserId == studentUserId &&
                    !x.IsArchived,
                cancellationToken);

        if (student is null)
            return [];

        var attempts = await db.PracticeAttempts.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                x.StudentProfileId == student.Id &&
                x.IsPrivate &&
                x.Status == PracticeAttemptStatus.Submitted)
            .OrderByDescending(x => x.SubmittedAtUtc ?? x.StartedAtUtc)
            .Take(100)
            .ToListAsync(cancellationToken);

        if (attempts.Count == 0)
            return [];

        var attemptById = attempts.ToDictionary(x => x.Id);
        var attemptIds = attemptById.Keys.ToArray();

        var attemptItems = await db.PracticeAttemptItems.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                attemptIds.Contains(x.PracticeAttemptId))
            .ToListAsync(cancellationToken);

        if (attemptItems.Count == 0)
            return [];

        var attemptItemById = attemptItems.ToDictionary(x => x.Id);
        var attemptItemIds = attemptItemById.Keys.ToArray();

        var responses = await db.PracticeResponses.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                attemptItemIds.Contains(x.PracticeAttemptItemId))
            .ToListAsync(cancellationToken);

        var itemIds = attemptItems
            .Select(x => x.AssessmentItemId)
            .Distinct()
            .ToArray();

        var items = await db.AssessmentItems.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                itemIds.Contains(x.Id))
            .ToDictionaryAsync(x => x.Id, cancellationToken);

        var outcomeRows = await db.AssessmentItemOutcomes.AsNoTracking()
            .Where(x =>
                x.SchoolId == student.SchoolId &&
                itemIds.Contains(x.AssessmentItemId))
            .ToListAsync(cancellationToken);

        var outcomesByItem = outcomeRows
            .GroupBy(x => x.AssessmentItemId)
            .ToDictionary(
                x => x.Key,
                x => (IReadOnlyList<Guid>)x
                    .Select(row => row.LearningOutcomeId)
                    .Distinct()
                    .ToArray());

        var result = new List<PrivatePracticeEvidenceItem>();

        foreach (var response in responses)
        {
            if (!attemptItemById.TryGetValue(
                    response.PracticeAttemptItemId,
                    out var attemptItem) ||
                !attemptById.TryGetValue(
                    attemptItem.PracticeAttemptId,
                    out var attempt) ||
                !items.TryGetValue(
                    attemptItem.AssessmentItemId,
                    out var item) ||
                attemptItem.MaxScore <= 0m)
            {
                continue;
            }

            result.Add(
                new PrivatePracticeEvidenceItem(
                    attempt.Id,
                    attempt.CurriculumAdoptionId,
                    attempt.CurriculumPedagogicalLessonId,
                    item.Id,
                    item.GenerationFamily,
                    item.GenerationParametersJson,
                    item.ValidationMetadataJson,
                    item.Difficulty,
                    response.IsCorrect,
                    response.Score,
                    attemptItem.MaxScore,
                    response.AnsweredAtUtc,
                    outcomesByItem.TryGetValue(
                        item.Id,
                        out var outcomeIds)
                        ? outcomeIds
                        : []));
        }

        return result
            .OrderByDescending(x => x.AnsweredAtUtc)
            .ThenBy(x => x.AttemptId)
            .ThenBy(x => x.AssessmentItemId)
            .ToArray();
    }
}
