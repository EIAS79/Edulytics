using Edulytics.Core.Entities;
using Edulytics.Core.Practice;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class StudentPrivatePracticeRepository(EdulyticsDbContext db)
    : IStudentPrivatePracticeRepository
{
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
}
