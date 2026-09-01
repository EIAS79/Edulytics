
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
