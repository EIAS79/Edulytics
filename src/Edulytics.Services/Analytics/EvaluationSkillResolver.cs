using System.Text.Json;
using Edulytics.Core.Analytics;
using Edulytics.Core.Entities;
using Edulytics.Core.Mathematics.Assessment;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Core.Mathematics.Skills;

namespace Edulytics.Services.Analytics;

internal static class EvaluationSkillResolver
{
    public static IReadOnlyList<EvaluationSkillDescriptor> ResolveExpected(
        LearningOutcome outcome)
    {
        ArgumentNullException.ThrowIfNull(outcome);

        if (Stage19AssessmentSkillContracts.TryResolve(
                outcome.Code,
                out var assessmentContract) &&
            assessmentContract is not null)
        {
            return [Descriptor(
                assessmentContract.SkillId,
                EvaluationSkillResolutionKind.ExactSkillContract)];
        }

        if (OfficialOutcomePracticeMapRegistry.TryResolve(
                outcome.Code,
                out var officialMap) &&
            officialMap is not null &&
            SupportingPracticeTargetRuleRegistry.TryGetById(
                officialMap.TargetRuleId,
                out var officialRule) &&
            officialRule is not null)
        {
            return [Descriptor(
                officialRule.SkillId,
                EvaluationSkillResolutionKind.ReviewedOutcomeMapping)];
        }

        if (PolishOutcomePracticeMapRegistry.TryResolve(
                outcome.Code,
                out var polishMap) &&
            polishMap is not null)
        {
            var descriptors = polishMap.TargetRules
                .Select(x => x.SkillId)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .Select(x => Descriptor(
                    x,
                    EvaluationSkillResolutionKind.ReviewedOutcomeMapping))
                .ToArray();

            if (descriptors.Length > 0)
                return descriptors;
        }

        return
        [
            new EvaluationSkillDescriptor(
                $"outcome:{outcome.Code}",
                string.IsNullOrWhiteSpace(outcome.Description)
                    ? outcome.Code
                    : outcome.Description.Trim(),
                EvaluationSkillResolutionKind.OutcomeProxy,
                [])
        ];
    }

    public static IReadOnlyList<EvaluationSkillDescriptor> ResolveForEvidence(
        LearningOutcome outcome,
        AssessmentItem? item,
        CurriculumPedagogicalLesson? lesson)
    {
        ArgumentNullException.ThrowIfNull(outcome);

        if (TryReadExactSkillId(item, out var exactSkillId))
        {
            return
            [
                Descriptor(
                    exactSkillId,
                    EvaluationSkillResolutionKind.ExactSkillContract)
            ];
        }

        if (lesson is not null &&
            Stage18PracticeSkillContracts.TryResolve(
                lesson.Code,
                out var practiceContract) &&
            practiceContract is not null)
        {
            return
            [
                Descriptor(
                    practiceContract.SkillId,
                    EvaluationSkillResolutionKind.ExactSkillContract)
            ];
        }

        if (lesson is not null &&
            SupportingPracticeTargetRuleRegistry.TryResolveReviewedOfficialTitle(
                lesson.Code,
                lesson.Title,
                out var reviewedRule) &&
            reviewedRule is not null)
        {
            return
            [
                Descriptor(
                    reviewedRule.SkillId,
                    EvaluationSkillResolutionKind.ReviewedOutcomeMapping)
            ];
        }

        return ResolveExpected(outcome);
    }

    private static EvaluationSkillDescriptor Descriptor(
        string skillId,
        EvaluationSkillResolutionKind resolutionKind)
    {
        var key = skillId.Trim();

        if (MathematicsSkillMetadataRegistry.TryResolve(
                key,
                out var metadata) &&
            metadata is not null)
        {
            return new EvaluationSkillDescriptor(
                metadata.Id,
                metadata.CanonicalName,
                resolutionKind,
                metadata.Prerequisites);
        }

        return new EvaluationSkillDescriptor(
            key,
            key,
            resolutionKind,
            []);
    }

    private static bool TryReadExactSkillId(
        AssessmentItem? item,
        out string skillId)
    {
        skillId = string.Empty;
        if (item is null)
            return false;

        return TryRead(
                   item.GenerationParametersJson,
                   out skillId) ||
               TryRead(
                   item.ValidationMetadataJson,
                   out skillId);
    }

    private static bool TryRead(
        string? json,
        out string skillId)
    {
        skillId = string.Empty;
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            foreach (var propertyName in new[]
                     {
                         "exactSkillId",
                         "skillId",
                         "skillContract"
                     })
            {
                if (!root.TryGetProperty(
                        propertyName,
                        out var property) ||
                    property.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                var value = property.GetString()?.Trim();
                if (string.IsNullOrWhiteSpace(value))
                    continue;

                skillId = value;
                return true;
            }
        }
        catch (JsonException)
        {
            // Historical/teacher-created items may have no reconstructable
            // generation metadata. Evaluation then falls back to reviewed
            // outcome/lesson mappings rather than guessing.
        }

        return false;
    }
}
