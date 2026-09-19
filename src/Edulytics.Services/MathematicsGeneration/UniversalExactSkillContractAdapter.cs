using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Edulytics.Core.AssessmentIntelligence;
using Edulytics.Core.Entities;
using Edulytics.Core.Enums;
using Edulytics.Core.MathematicsGeneration;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.MathematicsGeneration;

internal static class UniversalExactSkillContractAdapter
{
    public static GeneratedMathematicsItem Generate(
        AssessmentBlueprint blueprint,
        MathematicsOutcomeGenerationProfile profile,
        AssessmentItemDifficulty difficulty,
        AssessmentQuestionFamily blueprintFamily,
        AssessmentItemType requestedItemType,
        int seed,
        int index,
        IReadOnlyCollection<string> excluded)
    {
        if (!profile.HasExactSkillContract)
            throw new InvalidOperationException(
                "Exact Mathematics generation requires exact SkillContract metadata.");

        var exactDifficulty = difficulty switch
        {
            AssessmentItemDifficulty.Challenging => ExactSkillQuestionDifficulty.Challenge,
            AssessmentItemDifficulty.Medium => ExactSkillQuestionDifficulty.Stretch,
            _ => ExactSkillQuestionDifficulty.Standard
        };

        var scopeKey = string.Join(
            "|",
            blueprint.CurriculumLevelKey,
            profile.OutcomeCode,
            profile.ExactSkillId,
            index.ToString());

        var question = new ExactSkillContractQuestionEngine()
            .Generate(
                "universal-exact",
                scopeKey,
                profile.ExactQuestionFamilies,
                exactDifficulty,
                1,
                StableInt($"exact|{seed}|{index}", int.MaxValue - 1) + 1,
                excluded)
            .Single();

        var prompt = FormatPrompt(question, requestedItemType, scopeKey);

        var item = new AssessmentItem
        {
            Id = Guid.NewGuid(),
            SchoolId = blueprint.SchoolId,
            CurriculumAdoptionId = blueprint.CurriculumAdoptionId,
            CurriculumPedagogicalLessonId = blueprint.CurriculumPedagogicalLessonId,
            CurriculumTopicId = blueprint.CurriculumTopicId,
            Source = AssessmentItemSource.SystemGenerated,
            ItemType = requestedItemType,
            Difficulty = difficulty,
            Prompt = prompt,
            CorrectAnswer = question.CorrectAnswer,
            Solution = question.Solution,
            GenerationMethod = "exact-skill-contract-v1",
            GenerationFamily = question.Family,
            GenerationParametersJson = JsonSerializer.Serialize(new
            {
                exactSkillId = profile.ExactSkillId,
                questionFamily = question.Family,
                parameters = question.Parameters
            }),
            ExposureFingerprint = question.ExposureFingerprint,
            ValidationMetadataJson = JsonSerializer.Serialize(new
            {
                provider = "edulytics-exact-skill-contract",
                capabilityLevel = "READY_VERIFIED",
                exactSkillId = profile.ExactSkillId,
                questionFamily = question.Family,
                outcomeCode = profile.OutcomeCode,
                curriculumLevelKey = blueprint.CurriculumLevelKey,
                blueprintFamily = blueprintFamily.ToString(),
                solverVerified = true,
                alignmentValidated = true,
                broadFallbackUsed = false,
                gradeAware = true,
                reconstructable = true
            }),
            CreatedAtUtc = DateTime.UtcNow
        };

        var outcomeLink = new AssessmentItemOutcome
        {
            Id = Guid.NewGuid(),
            SchoolId = blueprint.SchoolId,
            AssessmentItemId = item.Id,
            LearningOutcomeId = profile.LearningOutcomeId
        };

        return new GeneratedMathematicsItem(
            item,
            outcomeLink,
            blueprintFamily,
            "universal-exact-skill-contract-v1");
    }

    private static string FormatPrompt(
        ExactSkillGeneratedQuestion question,
        AssessmentItemType itemType,
        string key)
    {
        if (itemType != AssessmentItemType.MultipleChoice)
            return question.Prompt;

        var choices = BuildChoices(question.CorrectAnswer);
        var ordered = choices
            .OrderBy(x => StableInt($"{key}|choice|{x}", int.MaxValue))
            .Take(4)
            .ToArray();
        var labels = new[] { "A", "B", "C", "D" };
        return $"{question.Prompt} {string.Join("  ", ordered.Select((x, i) => $"{labels[i]}) {x}"))}";
    }

    private static IReadOnlyList<string> BuildChoices(string answer)
    {
        var correct = answer.Trim();
        var choices = new HashSet<string>(StringComparer.Ordinal) { correct };

        if (int.TryParse(correct, out var integer))
        {
            for (var delta = 1; choices.Count < 4; delta++)
            {
                choices.Add((integer + delta).ToString());
                choices.Add((integer - delta).ToString());
            }
        }
        else if (TryFraction(correct, out var numerator, out var denominator))
        {
            choices.Add($"{Math.Max(0, numerator - 1)}/{denominator}");
            choices.Add($"{numerator + 1}/{denominator}");
            choices.Add($"{numerator}/{denominator + 1}");
            if (numerator != 0)
                choices.Add($"{denominator}/{numerator}");
        }
        else if (correct is "<" or ">" or "=")
        {
            choices.UnionWith(["<", ">", "=", "cannot determine"]);
        }
        else if (correct is "SSS" or "SAS" or "ASA" or "RHS")
        {
            choices.UnionWith(["SSS", "SAS", "ASA", "RHS"]);
        }
        else
        {
            choices.UnionWith(["none", "insufficient information", "not applicable"]);
        }

        while (choices.Count < 4)
            choices.Add($"option-{choices.Count + 1}");

        return choices.ToArray();
    }

    private static bool TryFraction(
        string value,
        out int numerator,
        out int denominator)
    {
        numerator = 0;
        denominator = 0;
        var parts = value.Split('/');
        return parts.Length == 2 &&
            int.TryParse(parts[0].Trim(), out numerator) &&
            int.TryParse(parts[1].Trim(), out denominator) &&
            denominator != 0;
    }

    private static int StableInt(string key, int maxExclusive)
    {
        if (maxExclusive <= 0)
            throw new InvalidOperationException("Stable range must be positive.");
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(key));
        var value = BitConverter.ToUInt32(hash, 0);
        return (int)(value % (uint)maxExclusive);
    }
}
