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
        CommonCoreG2SourceGroundedFoundationsCorrections.Apply(document);
        CommonCoreG2Unit1TenTwentySourceTeachingCorrections.Apply(document);
        CommonCoreG2Unit1HowCloseSourceTeachingCorrection.Apply(document);
        CommonCoreG2Unit1AddWithin50SourceTeachingCorrection.Apply(document);
        CommonCoreG2Unit1CentersAndRealDataCorrections.Apply(document);
        CommonCoreG2Unit1PictureGraphSourceTeachingCorrection.Apply(document);
        CommonCoreG2Unit1BarGraphsSourceTeachingCorrection.Apply(document);
        CommonCoreG2Unit1ConstructGraphsTeachingCorrection.Apply(document);
        CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection.Apply(document);
        CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection.Apply(document);
        CommonCoreG2Unit1BarCompareTeachingCorrection.Apply(document);
        CommonCoreG2Unit1TapeDiagramTeachingCorrection.Apply(document);
        CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection.Apply(document);
        CommonCoreG2Unit1AllCompareProblemsTeachingCorrection.Apply(document);
        CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.Apply(document);
        CommonCoreG2Unit1ClassSurveysTeachingCorrection.Apply(document);
        CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.Apply(document);
        CommonCoreG2Unit2UnknownAddendTeachingCorrection.Apply(document);
        CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.Apply(document);
        CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.Apply(document);
        CommonCoreG2Unit2DecomposeTenTeachingCorrection.Apply(document);
        CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.Apply(document);
        CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.Apply(document);
        CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.Apply(document);
        CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.Apply(document);
        CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.Apply(document);
        CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.Apply(document);
        CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.Apply(document);
        CommonCoreG2Unit2StoryEquationsTeachingCorrection.Apply(document);
        CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.Apply(document);
        CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.Apply(document);
        CommonCoreG2Unit2MarketInventoryTeachingCorrection.Apply(document);
        CommonCoreG2Unit3StandardUnitsTeachingCorrection.Apply(document);
        CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.Apply(document);
        CommonCoreG2Unit3BuildRulerTeachingCorrection.Apply(document);
        CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.Apply(document);
        CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.Apply(document);
        CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.Apply(document);
        CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.Apply(document);
        CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.Apply(document);
        CommonCoreG2Unit3FeetFishTapeTeachingCorrection.Apply(document);
        CommonCoreG2Unit3TornTapeTeachingCorrection.Apply(document);
        CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.Apply(document);
        CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.Apply(document);
        CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.Apply(document);
        CommonCoreG2HundredsPlaceValueSourceCorrections.Apply(document);
        CommonCoreG1Unit2StorySourceCorrections.Apply(document);
        CommonCoreB001SourceGoalMismatchCorrections.Apply(document);
        CommonCoreG1Unit2Part2SourceTeachingCorrections.Apply(document);
        CommonCoreG1Unit2NextFiveSourceTeachingCorrections.Apply(document);
        CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.Apply(document);
        CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.Apply(document);
        CommonCoreG1NextSourceFiveCorrections.Apply(document);
        CommonCoreG1SourceFiveNextCorrections.Apply(document);
        CommonCoreG1TenAndTeenSourceFiveCorrections.Apply(document);
        CommonCoreG1FullGradeStartingContentCorrections.Apply(document);
        CommonCoreG1FullGradeFinalUnit4Corrections.Apply(document);
        CommonCoreG1FullGradeUnit5ContentCorrections.Apply(document);
        CommonCoreG1FullGradeUnit6To8ContentCorrections.Apply(document);
        CommonCoreSourceBoundHtmlEntityCorrections.Apply(document);
        CommonCoreT001MixedAddSubtractContentCorrection.Apply(document);
        CommonCoreT001FindThePairContentCorrection.Apply(document);

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

        var g2 = CommonCoreG2SourceGroundedFoundationsCorrections
            .GetExpectedContentVersion(document, lesson, g1Pilot);

        var g2TenTwenty = CommonCoreG2Unit1TenTwentySourceTeachingCorrections
            .GetExpectedContentVersion(document, lesson, g2);
        var g2HowClose = CommonCoreG2Unit1HowCloseSourceTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2TenTwenty);
        var g2Add50 = CommonCoreG2Unit1AddWithin50SourceTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2HowClose);
        var g2Centers = CommonCoreG2Unit1CentersAndRealDataCorrections
            .GetExpectedContentVersion(document, lesson, g2Add50);
        var g2Picture = CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Centers);
        var g2Bar = CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Picture);
        var g2Construct = CommonCoreG2Unit1ConstructGraphsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Bar);
        var g2Questions = CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Construct);
        var g2Centers2 = CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
            .GetExpectedContentVersion(document, lesson, g2Questions);
        var g2Compare = CommonCoreG2Unit1BarCompareTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Centers2);
        var g2Tape = CommonCoreG2Unit1TapeDiagramTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Compare);
        var g2Positions = CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Tape);
        var g2AllPositions = CommonCoreG2Unit1AllCompareProblemsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Positions);
        var g2Collections = CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2AllPositions);
        var g2Survey = CommonCoreG2Unit1ClassSurveysTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Collections);
        var g2U02Compare = CommonCoreG2Unit2AddSubtractCompareTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Survey);
        var g2U02Missing = CommonCoreG2Unit2UnknownAddendTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2U02Compare);
        var g2Zoo = CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2U02Missing);
        var g2Center = CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Zoo);
        var g2Decompose = CommonCoreG2Unit2DecomposeTenTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Center);
        var g2Target = CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Decompose);
        var g2TwoDigits = CommonCoreG2Unit2TwoDigitTradeTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Target);
        var g2Different = CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2TwoDigits);
        var g2Mixed = CommonCoreG2Unit2MixedAddSubtractTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Different);
        var g2Center2 = CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Mixed);
        var g2Seeds = CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Center2);
        var g2Mancala = CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Seeds);
        var g2Equations = CommonCoreG2Unit2StoryEquationsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Mancala);
        var g2TwoStep = CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Equations);
        var g2TapeCenter = CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2TwoStep);
        var g2Market = CommonCoreG2Unit2MarketInventoryTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2TapeCenter);
        var g2MeasureUnits = CommonCoreG2Unit3StandardUnitsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Market);
        var g2Bearded = CommonCoreG2Unit3BeardedDragonCmTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2MeasureUnits);
        var g2Ruler = CommonCoreG2Unit3BuildRulerTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Bearded);
        var g2EstimateCm = CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Ruler);
        var g2Meters = CommonCoreG2Unit3MetersReptileTapeTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2EstimateCm);
        var g2Reptiles = CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Meters);
        var g2Stage2 = CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Reptiles);
        var g2Inch = CommonCoreG2Unit3WhatIsAnInchTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Stage2);
        var g2Feet = CommonCoreG2Unit3FeetFishTapeTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Inch);
        var g2Torn = CommonCoreG2Unit3TornTapeTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Feet);
        var g2Ribbon = CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Torn);
        var g2Bracelets = CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Ribbon);
        var g2TargetMeasure = CommonCoreG2Unit3TargetMeasurementsTeachingCorrection
            .GetExpectedContentVersion(document, lesson, g2Bracelets);
        var g2Hundreds = CommonCoreG2HundredsPlaceValueSourceCorrections
            .GetExpectedContentVersion(document, lesson, g2TargetMeasure);
        var g1Stories = CommonCoreG1Unit2StorySourceCorrections
            .GetExpectedContentVersion(document, lesson, g2Hundreds);
        var b001Goals = CommonCoreB001SourceGoalMismatchCorrections
            .GetExpectedContentVersion(document, lesson, g1Stories);
        var g1Part2 = CommonCoreG1Unit2Part2SourceTeachingCorrections
            .GetExpectedContentVersion(document, lesson, b001Goals);
        var g1Next = CommonCoreG1Unit2NextFiveSourceTeachingCorrections
            .GetExpectedContentVersion(document, lesson, g1Part2);
        var compared=CommonCoreG1Unit2CompareFiveSourceTeachingCorrections
            .GetExpectedContentVersion(document,lesson,g1Next);
        var stories=CommonCoreG1Unit2EquationsAndStoriesSourceCorrections
            .GetExpectedContentVersion(document,lesson,compared);
        var next=CommonCoreG1NextSourceFiveCorrections
            .GetExpectedContentVersion(document,lesson,stories);
        var g1009=CommonCoreG1SourceFiveNextCorrections
            .GetExpectedContentVersion(document,lesson,next);
        var g1010=CommonCoreG1TenAndTeenSourceFiveCorrections
            .GetExpectedContentVersion(document,lesson,g1009);
        var g1FullGrade = CommonCoreG1FullGradeStartingContentCorrections
            .GetExpectedContentVersion(document, lesson, g1010);
        var g1EndUnit4 = CommonCoreG1FullGradeFinalUnit4Corrections
            .GetExpectedContentVersion(document,lesson,g1FullGrade);
        var g1Unit5 = CommonCoreG1FullGradeUnit5ContentCorrections
            .GetExpectedContentVersion(document,lesson,g1EndUnit4);
        var g1Unit6To8 = CommonCoreG1FullGradeUnit6To8ContentCorrections
            .GetExpectedContentVersion(document,lesson,g1Unit5);
        var html = CommonCoreSourceBoundHtmlEntityCorrections
            .GetExpectedContentVersion(document, lesson, g1Unit6To8);
        var mixed = CommonCoreT001MixedAddSubtractContentCorrection
            .GetExpectedContentVersion(document, lesson, html);
        return CommonCoreT001FindThePairContentCorrection
            .GetExpectedContentVersion(document, lesson, mixed);
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
        CommonCoreG2SourceGroundedFoundationsCorrections
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1TenTwentySourceTeachingCorrections
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1HowCloseSourceTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1AddWithin50SourceTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1CentersAndRealDataCorrections
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1ConstructGraphsTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1BarCompareTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1TapeDiagramTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1AllCompareProblemsTeachingCorrection
            .IsTarget(document, lesson) ||
        CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit1ClassSurveysTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2UnknownAddendTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2DecomposeTenTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2StoryEquationsTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit2MarketInventoryTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3StandardUnitsTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3BuildRulerTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3FeetFishTapeTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3TornTapeTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.IsTarget(document, lesson) ||
        CommonCoreG2HundredsPlaceValueSourceCorrections.IsTarget(document, lesson) ||
        CommonCoreG1Unit2StorySourceCorrections.IsTarget(document, lesson) ||
        CommonCoreB001SourceGoalMismatchCorrections.IsTarget(document, lesson) ||
        CommonCoreG1Unit2Part2SourceTeachingCorrections.IsTarget(document, lesson) ||
        CommonCoreG1Unit2NextFiveSourceTeachingCorrections.IsTarget(document, lesson) ||
        CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.IsTarget(document, lesson) ||
        CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.IsTarget(document, lesson) ||
        CommonCoreG1NextSourceFiveCorrections.IsTarget(document, lesson) ||
        CommonCoreG1SourceFiveNextCorrections.IsTarget(document, lesson) ||
        CommonCoreG1TenAndTeenSourceFiveCorrections.IsTarget(document, lesson) ||
        CommonCoreG1FullGradeStartingContentCorrections.IsTarget(document, lesson) ||
        CommonCoreG1FullGradeFinalUnit4Corrections.IsTarget(document, lesson) ||
        CommonCoreG1FullGradeUnit5ContentCorrections.IsTarget(document, lesson) ||
        CommonCoreG1FullGradeUnit6To8ContentCorrections.IsTarget(document, lesson) ||
        CommonCoreSourceBoundHtmlEntityCorrections
            .IsTarget(document, lesson) ||
        CommonCoreT001MixedAddSubtractContentCorrection.IsTarget(document, lesson) ||
        CommonCoreT001FindThePairContentCorrection.IsTarget(document, lesson);

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
        CommonCoreG2SourceGroundedFoundationsCorrections
            .CanUpgradeExisting(
                document,
                lesson,
                existingContentVersion) ||
        CommonCoreG2Unit1TenTwentySourceTeachingCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1HowCloseSourceTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1AddWithin50SourceTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1CentersAndRealDataCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1PictureGraphSourceTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1BarGraphsSourceTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1ConstructGraphsTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1QuestionsAboutDataTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1OptionalSortAndDisplaySourceCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1BarCompareTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1TapeDiagramTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1ThreeUnknownPositionsTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1AllCompareProblemsTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1CountingCollectionsCenterTeachingCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit1ClassSurveysTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2AddSubtractCompareTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2UnknownAddendTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2ZooStoryProblemsTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2CaptureAndFiveCentersTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2DecomposeTenTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2TargetNumbersStage4TeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2TwoDigitTradeTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2DifferentWaysDecomposeTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2MixedAddSubtractTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2CenterDay2TargetStage5TeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2SeedsStoryTypesTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2MancalaTapeDiagramsTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2StoryEquationsTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2TwoStepSeedStoriesTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2TapeStoriesCenterTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit2MarketInventoryTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3StandardUnitsTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3BeardedDragonCmTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3BuildRulerTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3EstimateMeasureCmTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3MetersReptileTapeTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3ReptileCompareStoryTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3EstimateMeasureStage2TeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3WhatIsAnInchTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3FeetFishTapeTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3TornTapeTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3SareeRibbonStoriesTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3FriendshipBraceletsTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2Unit3TargetMeasurementsTeachingCorrection.CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG2HundredsPlaceValueSourceCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1Unit2StorySourceCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreB001SourceGoalMismatchCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1Unit2Part2SourceTeachingCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1Unit2NextFiveSourceTeachingCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1Unit2CompareFiveSourceTeachingCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1Unit2EquationsAndStoriesSourceCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1NextSourceFiveCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1SourceFiveNextCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1TenAndTeenSourceFiveCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1FullGradeStartingContentCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1FullGradeFinalUnit4Corrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1FullGradeUnit5ContentCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreG1FullGradeUnit6To8ContentCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreSourceBoundHtmlEntityCorrections
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreT001MixedAddSubtractContentCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion) ||
        CommonCoreT001FindThePairContentCorrection
            .CanUpgradeExisting(document, lesson, existingContentVersion);

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
