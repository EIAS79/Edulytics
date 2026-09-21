using System.Text.RegularExpressions;
using Edulytics.Core.Enums;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.MathematicsGeneration;

public enum MathematicsCurriculumDifficultyStage
{
    EarlyPrimary = 1,
    Primary = 2,
    LowerSecondary = 3,
    UpperSecondary = 4
}

public sealed record GradeAwareExactDifficultyDecision(
    MathematicsCurriculumDifficultyStage Stage,
    int? NumericLevel,
    ExactSkillQuestionDifficulty EffectiveDifficulty);

/// <summary>
/// Converts the user-facing Easy/Medium/Challenging band into a bounded exact
/// generation scale that is appropriate for the curriculum level. The visible
/// difficulty label is preserved; only the internal parameter complexity is
/// calibrated. Unknown level keys fail closed.
/// </summary>
public static partial class GradeAwareExactDifficultyPolicy
{
    [GeneratedRegex(@"(?:^|[:_\-\s])(?:G|L|S|GRADE|YEAR|STAGE|LEVEL)\s*(\d{1,2})(?:$|[:_\-\s])", RegexOptions.IgnoreCase)]
    private static partial Regex NumericLevelRegex();

    public static GradeAwareExactDifficultyDecision Resolve(
        string curriculumLevelKey,
        AssessmentItemDifficulty requestedDifficulty)
    {
        if (string.IsNullOrWhiteSpace(curriculumLevelKey))
            throw new InvalidOperationException(
                "Grade-aware Mathematics difficulty requires a curriculum level key.");

        var key = curriculumLevelKey.Trim();
        if (Regex.IsMatch(key, @"(?:^|[:_\-\s])HS(?:$|[:_\-\s])", RegexOptions.IgnoreCase))
        {
            return new GradeAwareExactDifficultyDecision(
                MathematicsCurriculumDifficultyStage.UpperSecondary,
                null,
                Map(MathematicsCurriculumDifficultyStage.UpperSecondary, requestedDifficulty));
        }

        var match = NumericLevelRegex().Match(key);
        if (!match.Success || !int.TryParse(match.Groups[1].Value, out var level) || level is < 1 or > 13)
        {
            throw new InvalidOperationException(
                $"Unsupported curriculum level key for grade-aware Mathematics difficulty: {curriculumLevelKey}");
        }

        var stage = level switch
        {
            <= 2 => MathematicsCurriculumDifficultyStage.EarlyPrimary,
            <= 6 => MathematicsCurriculumDifficultyStage.Primary,
            <= 9 => MathematicsCurriculumDifficultyStage.LowerSecondary,
            _ => MathematicsCurriculumDifficultyStage.UpperSecondary
        };

        return new GradeAwareExactDifficultyDecision(
            stage,
            level,
            Map(stage, requestedDifficulty));
    }

    private static ExactSkillQuestionDifficulty Map(
        MathematicsCurriculumDifficultyStage stage,
        AssessmentItemDifficulty requestedDifficulty) =>
        (stage, requestedDifficulty) switch
        {
            (_, AssessmentItemDifficulty.Easy) =>
                ExactSkillQuestionDifficulty.Standard,

            (MathematicsCurriculumDifficultyStage.EarlyPrimary, AssessmentItemDifficulty.Medium) =>
                ExactSkillQuestionDifficulty.Standard,
            (MathematicsCurriculumDifficultyStage.EarlyPrimary, AssessmentItemDifficulty.Challenging) =>
                ExactSkillQuestionDifficulty.Stretch,

            (MathematicsCurriculumDifficultyStage.Primary, AssessmentItemDifficulty.Medium) =>
                ExactSkillQuestionDifficulty.Stretch,
            (MathematicsCurriculumDifficultyStage.Primary, AssessmentItemDifficulty.Challenging) =>
                ExactSkillQuestionDifficulty.Challenge,

            (_, AssessmentItemDifficulty.Medium) =>
                ExactSkillQuestionDifficulty.Stretch,
            (_, AssessmentItemDifficulty.Challenging) =>
                ExactSkillQuestionDifficulty.Challenge,

            _ => throw new InvalidOperationException("Unsupported Mathematics difficulty band.")
        };
}
