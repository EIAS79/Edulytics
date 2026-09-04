using Edulytics.Core.Entities;
using Edulytics.Core.Enums;

namespace Edulytics.Core.Assessments;

public enum AssessmentBuilderDifficulty
{
    AtClassLevel = 1,
    Stretch = 2,
    Challenge = 3
}

public enum AssessmentBuilderQuestionStatus
{
    Draft = 1,
    Approved = 2,
    Legacy = 3,
    Edited = 4
}

public sealed record AssessmentBuilderPersistenceContext(
    Assessment Assessment,
    ClassGroup ClassGroup,
    SchoolCurriculumAdoption? CurriculumAdoption,
    IReadOnlyList<AssessmentQuestion> Questions,
    IReadOnlyList<AssessmentItem> Items,
    IReadOnlyList<QuestionLearningOutcome> QuestionOutcomeMappings,
    IReadOnlyList<AssessmentItemOutcome> ItemOutcomeMappings,
    IReadOnlyList<LearningOutcome> LearningOutcomes,
    IReadOnlyList<ClassOutcomeSummary> ClassOutcomeSummaries);

public sealed record AssessmentBuilderQuestionBundle(
    AssessmentQuestion Question,
    AssessmentItem Item,
    IReadOnlyList<QuestionLearningOutcome> QuestionOutcomeMappings,
    IReadOnlyList<AssessmentItemOutcome> ItemOutcomeMappings);
