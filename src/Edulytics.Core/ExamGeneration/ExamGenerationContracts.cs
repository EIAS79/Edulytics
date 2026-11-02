using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.MathematicsGeneration;

namespace Edulytics.Core.ExamGeneration;

public enum GeneratedExamKind
{
    FormalTeacherAssessment = 1,
    StudentPersonalTest = 2
}

public enum GeneratedExamStatus
{
    Draft = 1,
    Reviewed = 2,
    Approved = 3,
    Published = 4,
    ReadyToStart = 5
}

public sealed record FormalExamContext(
    Guid TeacherUserId,
    Guid ClassGroupId,
    Guid SubjectId,
    Guid AcademicYearId,
    Guid TermId,
    DateOnly AssessmentDate);

public sealed record GeneratedExamQuestion(
    int Order,
    decimal MaxScore,
    GeneratedMathematicsItem GeneratedItem);

public sealed record ExamGenerationRequest(
    string Title,
    AssessmentBlueprint Blueprint,
    MathematicsGenerationBatch GeneratedItems,
    FormalExamContext? FormalContext = null);

public sealed record GeneratedExamDraft(
    Guid Id,
    GeneratedExamKind Kind,
    GeneratedExamStatus Status,
    string Title,
    AssessmentBlueprint Blueprint,
    FormalExamContext? FormalContext,
    IReadOnlyList<GeneratedExamQuestion> Questions,
    IReadOnlyList<string> AuditTrail,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);

public sealed record FormalAssessmentMaterialization(
    Assessment Assessment,
    IReadOnlyList<AssessmentQuestion> Questions,
    IReadOnlyList<QuestionLearningOutcome> OutcomeMappings,
    IReadOnlyList<AssessmentItem> AssessmentItems,
    IReadOnlyList<AssessmentItemOutcome> ItemOutcomeMappings);
