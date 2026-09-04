using Edulytics.Core.Assessments;
using Edulytics.Core.Entities;
using Edulytics.Core.Interfaces;
using Edulytics.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Edulytics.Data.Repositories;

public sealed class AssessmentBuilderRepository(EdulyticsDbContext db) : IAssessmentBuilderRepository
{
    public async Task<AssessmentBuilderPersistenceContext?> GetContextAsync(
        Guid schoolId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var assessment = await db.Assessments
            .SingleOrDefaultAsync(x => x.SchoolId == schoolId && x.Id == assessmentId, cancellationToken);
        if (assessment is null)
            return null;

        var classGroup = await db.ClassGroups.AsNoTracking()
            .SingleOrDefaultAsync(x => x.SchoolId == schoolId && x.Id == assessment.ClassGroupId, cancellationToken);
        if (classGroup is null)
            return null;

        SchoolCurriculumAdoption? adoption = null;
        if (classGroup.CurriculumAdoptionId.HasValue)
        {
            adoption = await db.SchoolCurriculumAdoptions.AsNoTracking()
                .SingleOrDefaultAsync(
                    x => x.SchoolId == schoolId && x.Id == classGroup.CurriculumAdoptionId.Value && x.IsActive,
                    cancellationToken);
        }

        adoption ??= await db.SchoolCurriculumAdoptions.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.IsActive &&
                x.AcademicProgramId == classGroup.AcademicProgramId &&
                x.GradeLevelId == classGroup.GradeLevelId &&
                x.SubjectId == assessment.SubjectId &&
                (x.AcademicYearId == assessment.AcademicYearId || x.AcademicYearId == null))
            .OrderByDescending(x => x.AcademicYearId.HasValue)
            .ThenByDescending(x => x.IsPrimary)
            .FirstOrDefaultAsync(cancellationToken);

        var questions = await db.AssessmentQuestions
            .Where(x => x.SchoolId == schoolId && x.AssessmentId == assessmentId)
            .OrderBy(x => x.Order)
            .ToListAsync(cancellationToken);
        var questionIds = questions.Select(x => x.Id).ToArray();

        var items = await db.AssessmentItems
            .Where(x => x.SchoolId == schoolId && questionIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var questionMappings = await db.QuestionLearningOutcomes
            .Where(x => x.SchoolId == schoolId && questionIds.Contains(x.AssessmentQuestionId))
            .ToListAsync(cancellationToken);

        var itemMappings = await db.AssessmentItemOutcomes
            .Where(x => x.SchoolId == schoolId && questionIds.Contains(x.AssessmentItemId))
            .ToListAsync(cancellationToken);

        var adoptionId = adoption?.Id;
        var outcomes = await db.LearningOutcomes.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.AcademicProgramId == classGroup.AcademicProgramId &&
                x.GradeLevelId == classGroup.GradeLevelId &&
                x.SubjectId == assessment.SubjectId &&
                (!adoptionId.HasValue || x.CurriculumAdoptionId == adoptionId.Value || x.CurriculumAdoptionId == null))
            .OrderBy(x => x.Order)
            .ThenBy(x => x.Code)
            .ToListAsync(cancellationToken);

        var summaries = await db.ClassOutcomeSummaries.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.AcademicYearId == assessment.AcademicYearId &&
                x.ClassGroupId == assessment.ClassGroupId &&
                x.SubjectId == assessment.SubjectId)
            .ToListAsync(cancellationToken);

        return new AssessmentBuilderPersistenceContext(
            assessment,
            classGroup,
            adoption,
            questions,
            items,
            questionMappings,
            itemMappings,
            outcomes,
            summaries);
    }

    public async Task<IReadOnlyList<StudentProfile>> ListTargetStudentsAsync(
        Guid schoolId,
        Guid assessmentId,
        CancellationToken cancellationToken = default)
    {
        var assessment = await db.Assessments.AsNoTracking()
            .SingleOrDefaultAsync(x => x.SchoolId == schoolId && x.Id == assessmentId, cancellationToken);
        if (assessment is null)
            return [];

        var studentIds = await db.StudentEnrollments.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                x.AcademicYearId == assessment.AcademicYearId &&
                x.ClassGroupId == assessment.ClassGroupId)
            .Select(x => x.StudentProfileId)
            .Distinct()
            .ToArrayAsync(cancellationToken);

        return await db.StudentProfiles.AsNoTracking()
            .Where(x =>
                x.SchoolId == schoolId &&
                studentIds.Contains(x.Id) &&
                !x.IsArchived &&
                x.Status == Core.Enums.AcademicStructureStatus.Active)
            .OrderBy(x => x.DisplayName)
            .ThenBy(x => x.StudentNumber)
            .ToListAsync(cancellationToken);
    }

    public void AddBundle(AssessmentBuilderQuestionBundle bundle)
    {
        db.AssessmentQuestions.Add(bundle.Question);
        db.AssessmentItems.Add(bundle.Item);
        db.QuestionLearningOutcomes.AddRange(bundle.QuestionOutcomeMappings);
        db.AssessmentItemOutcomes.AddRange(bundle.ItemOutcomeMappings);
    }

    public void RemoveQuestionBundle(
        AssessmentQuestion question,
        AssessmentItem? item,
        IReadOnlyCollection<QuestionLearningOutcome> questionMappings,
        IReadOnlyCollection<AssessmentItemOutcome> itemMappings)
    {
        db.QuestionLearningOutcomes.RemoveRange(questionMappings);
        db.AssessmentItemOutcomes.RemoveRange(itemMappings);
        if (item is not null)
            db.AssessmentItems.Remove(item);
        db.AssessmentQuestions.Remove(question);
    }

    public async Task<AssessmentPersistenceResult> SaveAsync(
        Assessment assessment,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default)
    {
        db.Entry(assessment).Property(x => x.RowVersion).OriginalValue = expectedRowVersion;
        try
        {
            await db.SaveChangesAsync(cancellationToken);
            return AssessmentPersistenceResult.Success();
        }
        catch (DbUpdateConcurrencyException)
        {
            return AssessmentPersistenceResult.Failure(AssessmentPersistenceError.Conflict);
        }
        catch (DbUpdateException)
        {
            return AssessmentPersistenceResult.Failure(AssessmentPersistenceError.Constraint);
        }
    }
}
