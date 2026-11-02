using Edulytics.Core.Assessments;
using Edulytics.Core.Entities;

namespace Edulytics.Core.Interfaces;

public interface IAssessmentBuilderRepository
{
    Task<AssessmentBuilderPersistenceContext?> GetContextAsync(
        Guid schoolId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<StudentProfile>> ListTargetStudentsAsync(
        Guid schoolId,
        Guid assessmentId,
        CancellationToken cancellationToken = default);

    void AddBundle(AssessmentBuilderQuestionBundle bundle);

    void ReplaceOutcomeMappings(
        IReadOnlyCollection<QuestionLearningOutcome> currentQuestionMappings,
        IReadOnlyCollection<AssessmentItemOutcome> currentItemMappings,
        IReadOnlyCollection<QuestionLearningOutcome> replacementQuestionMappings,
        IReadOnlyCollection<AssessmentItemOutcome> replacementItemMappings);

    void RemoveQuestionBundle(
        AssessmentQuestion question,
        AssessmentItem? item,
        IReadOnlyCollection<QuestionLearningOutcome> questionMappings,
        IReadOnlyCollection<AssessmentItemOutcome> itemMappings);

    Task<AssessmentPersistenceResult> SaveAsync(
        Assessment assessment,
        byte[] expectedRowVersion,
        CancellationToken cancellationToken = default);
}
