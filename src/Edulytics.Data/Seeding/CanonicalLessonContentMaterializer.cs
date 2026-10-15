using System.Security.Cryptography;
using System.Text;
using Edulytics.Core.Curriculum;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Single authority for the effective learner-facing canonical lesson body.
/// Audits, persistence and Production parity checks must materialize content
/// through this class so reviewed corrections cannot exist only in memory in
/// one path while a different body is served to students.
/// </summary>
public static class CanonicalLessonContentMaterializer
{
    public static void Materialize(
        CanonicalLessonContentPackDocument document)
    {
        CambridgePrimaryStage6LessonContentCorrections
            .ApplyApprovedCorrections(document);
        CambridgeOfficialMappingContentCorrections
            .ApplyApprovedCorrections(document);
        SupportingLessonPracticeContentCorrections
            .ApplyApprovedCorrections(document);
        OfficialLessonPracticeContentCorrections
            .ApplyApprovedCorrections(document);
        PolishLessonPracticeContentCorrections
            .ApplyApprovedCorrections(document);
        CommonCoreG4MultiplesSourceCorrection.Apply(document);
        CommonCoreG2StoryProblemsSourceCorrection.Apply(document);
        CommonCoreG2TapeDiagramAndStoryResolutionCorrection.Apply(document);
        CommonCoreG1SourceGroundedInstructionalCorrections.Apply(document);
        CommonCoreG1CountAndAddPilotCorrection.Apply(document);
        CommonCoreG1DotCardMatchingCorrection.Apply(document);
        CommonCoreG2SourceGroundedFoundationsCorrections.Apply(document);
        CommonCoreSourceBoundHtmlEntityCorrections.Apply(document);
        CommonCoreB01FractionPurposeCorrections.Apply(document);
        CommonCoreB01PlaceValuePurposeCorrection.Apply(document);
        CommonCoreB01UnitAndPlaceValuePurposeCorrections.Apply(document);
        CommonCoreB01NonLengthRulerTemplateCorrections.Apply(document);
        CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.Apply(document);
        CommonCoreB01V9LearnerVoiceSharedRootCorrection.Apply(document);
        CommonCoreB01V9SharedComparisonProcedureCorrections.Apply(document);
        CommonCoreB01V9SharedGeometryProcedureCorrections.Apply(document);
        CommonCoreB01V9SharedAdditionProcedureCorrections.Apply(document);
        CommonCoreB01V9SharedSubtractionProcedureCorrections.Apply(document);
        CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.Apply(document);
        CommonCoreB01V9OffTopicClockConceptCorrections.Apply(document);
        CommonCoreT001FindThePairContentCorrection.Apply(document);
        CommonCoreT001MixedAddSubtractContentCorrection.Apply(document);

        ValidateUnicode(document);
        CanonicalLessonContentPackContract.Validate(document);
    }

    private static void ValidateUnicode(
        CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            foreach (var translation in lesson.Translations)
            {
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "Title",
                    translation.Title);
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "Explanation",
                    translation.Explanation);
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "KeyConceptsAndRules",
                    translation.KeyConceptsAndRules);
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "WorkedExamples",
                    translation.WorkedExamples);
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "StepByStepSolutions",
                    translation.StepByStepSolutions);
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "CommonMistakes",
                    translation.CommonMistakes);
                ValidateField(
                    lesson.LessonCode,
                    translation.CultureCode,
                    "QuickSummary",
                    translation.QuickSummary);
            }
        }
    }

    private static void ValidateField(
        string lessonCode,
        string cultureCode,
        string field,
        string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var current = value[index];

            if (char.IsHighSurrogate(current))
            {
                if (index + 1 < value.Length &&
                    char.IsLowSurrogate(value[index + 1]))
                {
                    index++;
                    continue;
                }

                throw InvalidUnicode(
                    lessonCode,
                    cultureCode,
                    field,
                    index,
                    current);
            }

            if (char.IsLowSurrogate(current))
            {
                throw InvalidUnicode(
                    lessonCode,
                    cultureCode,
                    field,
                    index,
                    current);
            }
        }
    }

    private static InvalidOperationException InvalidUnicode(
        string lessonCode,
        string cultureCode,
        string field,
        int index,
        char value) =>
        new(
            $"Invalid UTF-16 learner content: {lessonCode}/{cultureCode}/" +
            $"{field}@{index}:U+{(int)value:X4}.");

    public static string GetEffectiveContentVersion(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        var stage6 =
            CambridgePrimaryStage6LessonContentCorrections
                .GetExpectedContentVersion(document, lesson);

        var officialMapping =
            CambridgeOfficialMappingContentCorrections
                .GetExpectedContentVersion(
                    document,
                    lesson,
                    stage6);

        var supporting =
            SupportingLessonPracticeContentCorrections
                .GetExpectedContentVersion(
                    document,
                    lesson,
                    CambridgeReviewedExampleContentCorrections
                        .GetExpectedContentVersion(
                            document,
                            lesson,
                            officialMapping));

        var official =
            OfficialLessonPracticeContentCorrections
                .GetExpectedContentVersion(
                    document,
                    lesson,
                    supporting);

        var polish =
            PolishLessonPracticeContentCorrections
                .GetExpectedContentVersion(
                    document,
                    lesson,
                    official);

        var commonCoreRich = CommonCoreRichContentCorrections
            .GetExpectedContentVersion(document, lesson, polish);

        var g4 = CommonCoreG4MultiplesSourceCorrection
            .GetExpectedContentVersion(document, lesson, commonCoreRich);

        var g2Story = CommonCoreG2StoryProblemsSourceCorrection
            .GetExpectedContentVersion(document, lesson, g4);

        var g2Tape = CommonCoreG2TapeDiagramAndStoryResolutionCorrection
            .GetExpectedContentVersion(document, lesson, g2Story);

        var g1 = CommonCoreG1SourceGroundedInstructionalCorrections
            .GetExpectedContentVersion(document, lesson, g2Tape);

        var g1Pilot = CommonCoreG1CountAndAddPilotCorrection
            .GetExpectedContentVersion(document, lesson, g1);

        var dotMatching = CommonCoreG1DotCardMatchingCorrection
            .GetExpectedContentVersion(document, lesson, g1Pilot);
        var g2 = CommonCoreG2SourceGroundedFoundationsCorrections
            .GetExpectedContentVersion(document, lesson, dotMatching);

        var html = CommonCoreSourceBoundHtmlEntityCorrections
            .GetExpectedContentVersion(document, lesson, g2);
        var fractions = CommonCoreB01FractionPurposeCorrections
            .GetExpectedContentVersion(document, lesson, html);
        var placeValue = CommonCoreB01PlaceValuePurposeCorrection
            .GetExpectedContentVersion(document, lesson, fractions);
        var weightOrTenThousand = CommonCoreB01UnitAndPlaceValuePurposeCorrections
            .GetExpectedContentVersion(document, lesson, placeValue);
        var v9Three = CommonCoreB01NonLengthRulerTemplateCorrections
            .GetExpectedContentVersion(document, lesson, weightOrTenThousand);
        var nonMeasure = CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.GetExpectedContentVersion(document, lesson, v9Three);
        var learnerVoice=CommonCoreB01V9LearnerVoiceSharedRootCorrection.GetExpectedContentVersion(document, lesson, nonMeasure);
        var comparisons = CommonCoreB01V9SharedComparisonProcedureCorrections.GetExpectedContentVersion(document, lesson, learnerVoice);
        var geometry = CommonCoreB01V9SharedGeometryProcedureCorrections.GetExpectedContentVersion(document, lesson, comparisons);
        var addition=CommonCoreB01V9SharedAdditionProcedureCorrections.GetExpectedContentVersion(document, lesson, geometry);
        var subtraction=CommonCoreB01V9SharedSubtractionProcedureCorrections.GetExpectedContentVersion(document, lesson, addition);
        var geometryConcept=CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.GetExpectedContentVersion(document, lesson, subtraction);
        var offTopic=CommonCoreB01V9OffTopicClockConceptCorrections.GetExpectedContentVersion(document,lesson,geometryConcept);
        var pair=CommonCoreT001FindThePairContentCorrection.GetExpectedContentVersion(document,lesson,offTopic);
        return CommonCoreT001MixedAddSubtractContentCorrection.GetExpectedContentVersion(document,lesson,pair);
    }

    public static bool IsReviewedCorrectionTarget(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson) =>
        CambridgePrimaryStage6LessonContentCorrections
            .IsTarget(document, lesson) ||
        CambridgeOfficialMappingContentCorrections
            .IsTarget(document, lesson) ||
        CambridgeReviewedExampleContentCorrections
            .IsTarget(document, lesson) ||
        SupportingLessonPracticeContentCorrections
            .IsTarget(document, lesson) ||
        OfficialLessonPracticeContentCorrections
            .IsTarget(document, lesson) ||
        PolishLessonPracticeContentCorrections
            .IsTarget(document, lesson) ||
        CommonCoreRichContentCorrections
            .IsTarget(document, lesson) ||
        CommonCoreG4MultiplesSourceCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2StoryProblemsSourceCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2TapeDiagramAndStoryResolutionCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG1SourceGroundedInstructionalCorrections
            .IsTarget(document, lesson) ||
        CommonCoreG1CountAndAddPilotCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG1DotCardMatchingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2SourceGroundedFoundationsCorrections
            .IsTarget(document, lesson) ||
        CommonCoreSourceBoundHtmlEntityCorrections
            .IsTarget(document, lesson) ||
        CommonCoreB01FractionPurposeCorrections
            .IsTarget(document, lesson) ||
        CommonCoreB01PlaceValuePurposeCorrection.IsTarget(document,lesson) ||
        CommonCoreB01UnitAndPlaceValuePurposeCorrections.IsTarget(document,lesson) ||
        CommonCoreB01NonLengthRulerTemplateCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9LearnerVoiceSharedRootCorrection.IsTarget(document,lesson) ||
        CommonCoreB01V9SharedComparisonProcedureCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9SharedGeometryProcedureCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9SharedAdditionProcedureCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9SharedSubtractionProcedureCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.IsTarget(document,lesson) ||
        CommonCoreB01V9OffTopicClockConceptCorrections.IsTarget(document,lesson) ||
        CommonCoreT001FindThePairContentCorrection.IsTarget(document,lesson) ||
        CommonCoreT001MixedAddSubtractContentCorrection.IsTarget(document,lesson);

    public static bool CanUpgradeExisting(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson,
        string existingContentVersion) =>
        CambridgePrimaryStage6LessonContentCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CambridgeOfficialMappingContentCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CambridgeReviewedExampleContentCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        SupportingLessonPracticeContentCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        OfficialLessonPracticeContentCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        PolishLessonPracticeContentCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreRichContentCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreG4MultiplesSourceCorrection
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreG2StoryProblemsSourceCorrection
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreG2TapeDiagramAndStoryResolutionCorrection
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreG1SourceGroundedInstructionalCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreG1CountAndAddPilotCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1DotCardMatchingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2SourceGroundedFoundationsCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreSourceBoundHtmlEntityCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreB01FractionPurposeCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreB01PlaceValuePurposeCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreB01UnitAndPlaceValuePurposeCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreB01NonLengthRulerTemplateCorrections.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9LearnerVoiceSharedRootCorrection.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9SharedComparisonProcedureCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9SharedGeometryProcedureCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9SharedAdditionProcedureCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9SharedSubtractionProcedureCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreB01V9OffTopicClockConceptCorrections.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreT001FindThePairContentCorrection.CanUpgradeExisting(document,lesson,existingContentVersion) ||
        CommonCoreT001MixedAddSubtractContentCorrection.CanUpgradeExisting(document,lesson,existingContentVersion);

    public static string ComputeLessonFingerprint(
        CanonicalLessonContentPackDocument document,
        CanonicalLessonContentPackLesson lesson)
    {
        var builder = new StringBuilder();
        builder.Append(document.PackCode).Append('\n');
        builder.Append(document.VersionCode).Append('\n');
        builder.Append(lesson.LessonCode).Append('\n');
        builder.Append(GetEffectiveContentVersion(document, lesson)).Append('\n');

        foreach (var translation in lesson.Translations
                     .OrderBy(x => x.CultureCode, StringComparer.Ordinal))
        {
            builder
                .Append(translation.CultureCode).Append('\n')
                .Append(translation.Title).Append('\n')
                .Append(translation.Explanation).Append('\n')
                .Append(translation.KeyConceptsAndRules).Append('\n')
                .Append(translation.WorkedExamples).Append('\n')
                .Append(translation.StepByStepSolutions).Append('\n')
                .Append(translation.CommonMistakes).Append('\n')
                .Append(translation.QuickSummary).Append('\n');
        }

        return Convert.ToHexString(
            SHA256.HashData(
                Encoding.UTF8.GetBytes(builder.ToString())))
            .ToLowerInvariant();
    }
}
