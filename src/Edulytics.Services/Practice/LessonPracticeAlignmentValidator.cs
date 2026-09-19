using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

public sealed record LessonPracticeAlignmentResult(
    bool IsAligned,
    string ReasonCode);

/// <summary>
/// Independent lesson-alignment gate for exact Practice.
/// Mathematical correctness alone is insufficient: the generated family must be
/// explicitly authorized by the lesson's Practice contract.
/// </summary>
public static class LessonPracticeAlignmentValidator
{
    public static LessonPracticeAlignmentResult Validate(
        Stage18PracticeSkillContract contract,
        ExactSkillGeneratedQuestion question)
    {
        ArgumentNullException.ThrowIfNull(contract);
        ArgumentNullException.ThrowIfNull(question);

        if (string.IsNullOrWhiteSpace(contract.SkillId))
            return new(false, "MissingSkillId");

        if (!contract.AllowedQuestionFamilies.Contains(
                question.Family,
                StringComparer.Ordinal))
        {
            return new(false, "QuestionFamilyNotAllowed");
        }

        if (!ExactSkillContractQuestionEngine.Verify(
                question.Family,
                question.Parameters,
                question.CorrectAnswer))
        {
            return new(false, "IndependentMathematicsVerificationFailed");
        }

        return new(true, "ExactLessonContractAligned");
    }

    public static LessonPracticeAlignmentResult ValidatePersisted(
        Stage18PracticeSkillContract contract,
        string? generationFamily,
        IReadOnlyDictionary<string, int> parameters,
        string? correctAnswer)
    {
        if (string.IsNullOrWhiteSpace(generationFamily) ||
            string.IsNullOrWhiteSpace(correctAnswer))
        {
            return new(false, "PersistedItemMetadataIncomplete");
        }

        if (!contract.AllowedQuestionFamilies.Contains(
                generationFamily,
                StringComparer.Ordinal))
        {
            return new(false, "QuestionFamilyNotAllowed");
        }

        return ExactSkillContractQuestionEngine.Verify(
                generationFamily,
                parameters,
                correctAnswer)
            ? new(true, "ExactLessonContractAligned")
            : new(false, "IndependentMathematicsVerificationFailed");
    }
}
