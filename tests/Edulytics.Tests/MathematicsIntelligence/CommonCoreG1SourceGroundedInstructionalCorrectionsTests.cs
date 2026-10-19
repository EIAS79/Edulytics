using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1SourceGroundedInstructionalCorrectionsTests
{
    private static (CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English) Effective(string lessonCode)
    {
        var match = Assert.Single(MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons
                .Where(lesson => lesson.LessonCode == lessonCode)
                .Select(lesson => (Doc: doc, Lesson: lesson))));

        return (match.Doc, match.Lesson,
            Assert.Single(match.Lesson.Translations, x => x.CultureCode == "en"));
    }

    [Theory]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode,
        CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsVersion)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.JungleSurveyLessonCode,
        CommonCoreG1SourceGroundedInstructionalCorrections.JungleSurveyVersion)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.TensLessonCode,
        CommonCoreG1SourceGroundedInstructionalCorrections.TensVersion)]
    public void EffectiveLessonUsesSourceGroundedAgeAppropriateInstructions(
        string code, string version)
    {
        var (doc, lesson, en) = Effective(code);
        Assert.Equal(version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson));

        Assert.Contains("Step 5:", en.StepByStepSolutions);
        Assert.DoesNotContain("1 − 4 = -3", en.WorkedExamples);
        Assert.DoesNotContain("2 − 3 = -1", en.WorkedExamples);
        Assert.DoesNotContain("source task compares or removes", en.WorkedExamples,
            StringComparison.OrdinalIgnoreCase);

        if (code == CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode)
        {
            Assert.Contains("information shown in a picture graph", en.KeyConceptsAndRules,
                StringComparison.OrdinalIgnoreCase);
            Assert.Contains("7 - 3 = 4", en.WorkedExamples);
            Assert.Contains("7 + 5 + 3 = 15", en.WorkedExamples);
            Assert.Equal(new[] { "CCSS:1.MD.C.4", "CCSS:1.OA.C.5", "CCSS:1.OA.C.6" },
                lesson.OutcomeCodes);
        }
        else if (code == CommonCoreG1SourceGroundedInstructionalCorrections.JungleSurveyLessonCode)
        {
            Assert.Contains("survey", en.KeyConceptsAndRules,
                StringComparison.OrdinalIgnoreCase);
            Assert.Contains("4 + 3 + 2 = 9", en.WorkedExamples);
            Assert.Contains("4 - 2 = 2", en.WorkedExamples);
            Assert.Equal("CCSS:1.MD.C.4", Assert.Single(lesson.OutcomeCodes));
        }
        else
        {
            Assert.Contains("towers of ten", en.KeyConceptsAndRules,
                StringComparison.OrdinalIgnoreCase);
            Assert.Contains("60 - 10 = 50", en.WorkedExamples);
            Assert.Contains("20 + 10 = 30", en.WorkedExamples);
            Assert.Equal(7, lesson.OutcomeCodes.Count);
        }
    }

    [Theory]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.JungleSurveyLessonCode)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.TensLessonCode)]
    public void CorrectionIdempotentAndRefusesSourceOrPartialTextDrift(string code)
    {
        var (doc, lesson, en) = Effective(code);
        var before = CanonicalLessonContentMaterializer
            .ComputeLessonFingerprint(doc, lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(before,
            CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson));

        var originalSha = lesson.CanonicalBodySha256;
        lesson.CanonicalBodySha256 = new string('0', 64);
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG1SourceGroundedInstructionalCorrections.Apply(doc));
        }
        finally { lesson.CanonicalBodySha256 = originalSha; }

        var originalExamples = en.WorkedExamples;
        en.WorkedExamples = "Unexpected partial overwrite";
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG1SourceGroundedInstructionalCorrections.Apply(doc));
        }
        finally { en.WorkedExamples = originalExamples; }
    }

    [Theory]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.JungleSurveyLessonCode)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.TensLessonCode)]
    public void StoredCorrectionRequiresExactPriorVersionAndKnownBody(string code)
    {
        var (doc, lesson, _) = Effective(code);
        CommonCoreG1SourceGroundedInstructionalCorrections.ValidateExistingBodyBeforeUpgrade(
            doc, lesson, CommonCoreRichContentCorrections.CorrectionContentVersion,
            Array.Empty<CurriculumLessonContentTranslation>());

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG1SourceGroundedInstructionalCorrections.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, "surprising-old-version",
                Array.Empty<CurriculumLessonContentTranslation>()));

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG1SourceGroundedInstructionalCorrections.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, CommonCoreRichContentCorrections.CorrectionContentVersion,
                [new CurriculumLessonContentTranslation
                {
                    CultureCode = "en", Title = "User-edited current content"
                }]));
    }

    [Theory]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.JungleSurveyLessonCode)]
    [InlineData(CommonCoreG1SourceGroundedInstructionalCorrections.TensLessonCode)]
    public void RichV2UsesEffectiveSourceGroundedMathNotOriginalBadExamples(string code)
    {
        var (_, _, en) = Effective(code);
        var rich = RichLessonContentV2RuntimeComposer.TryCompose(code,
            new CanonicalLessonTranslationRecord(
                en.CultureCode, en.Title, en.Explanation, en.KeyConceptsAndRules,
                en.WorkedExamples, en.StepByStepSolutions, en.CommonMistakes,
                en.QuickSummary));
        Assert.NotNull(rich);
        Assert.DoesNotContain("1 − 4 = -3", rich!.KeyConcepts[0].Example);
        Assert.DoesNotContain("2 − 3 = -1", rich.KeyConcepts[0].Example);
    }

    [Fact]
    public void Original2021U06L02LearnerWorkRequiresFourPencilComparisonsAndUnchangedDeskString()
    {
        const string code=CommonCoreG1SourceGroundedInstructionalCorrections.CompareLengthsIndirectlyLessonCode;
        var (doc,lesson,en)=Effective(code);
        Assert.Equal(CommonCoreG1SourceGroundedInstructionalCorrections.CompareLengthsIndirectlyVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
        Assert.Contains("TWO classroom objects longer",en.KeyConceptsAndRules);
        Assert.Contains("TWO shorter",en.KeyConceptsAndRules);
        Assert.Contains("FOUR complete comparison sentences",en.StepByStepSolutions);
        Assert.Contains("desk's side",en.StepByStepSolutions);
        Assert.Contains("one piece of string",en.StepByStepSolutions);
        Assert.Contains("Draw or write an explanation",en.StepByStepSolutions);
        Assert.Contains("35+20=55",en.WorkedExamples);
        Assert.Contains("37+45=82",en.WorkedExamples);
        Assert.Contains("cannot be invented",en.CommonMistakes);
        var fingerprint=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(fingerprint,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson));
    }

    [Fact]
    public void Original2021U07L06RequiresBothRectangleAndSquareDrawingSets()
    {
        const string code=CommonCoreG1SourceGroundedInstructionalCorrections.RectanglesSquaresAttributesLessonCode;
        var (doc,lesson,en)=Effective(code);
        Assert.Equal(CommonCoreG1SourceGroundedInstructionalCorrections.RectanglesSquaresAttributesVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
        Assert.Contains("DRAW FIVE rectangles and THREE shapes",en.KeyConceptsAndRules);
        Assert.Contains("DRAW FIVE squares and THREE shapes",en.KeyConceptsAndRules);
        Assert.Contains("TWO distinct sets of eight",en.KeyConceptsAndRules);
        Assert.Contains("FIVE different valid rectangles",en.StepByStepSolutions);
        Assert.Contains("THREE figures that are NOT rectangles",en.StepByStepSolutions);
        Assert.Contains("FIVE squares and THREE figures that are NOT squares",en.StepByStepSolutions);
        Assert.Contains("A non-square rectangle is a valid RECTANGLE nonexample of a SQUARE",en.WorkedExamples);
        Assert.Contains("57+42=99",en.WorkedExamples);
        Assert.Contains("SECOND source task",en.CommonMistakes);
        var before=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(before,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson));
    }

    [Fact]
    public void OriginalU07L15ClockTasksPreserveDrawingAndOpenChoiceHourHand()
    {
        const string code=CommonCoreG1SourceGroundedInstructionalCorrections.WriteClockTimesLessonCode;
        var (doc,lesson,en)=Effective(code);
        Assert.Equal(CommonCoreG1SourceGroundedInstructionalCorrections.WriteClockTimesVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
        Assert.Contains("DRAW the MINUTE HAND",en.KeyConceptsAndRules);
        Assert.Contains("ONLY a minute hand shown",en.KeyConceptsAndRules);
        Assert.Contains("OTHER valid hour choices",en.KeyConceptsAndRules);
        Assert.Contains("minute hand at the 6",en.WorkedExamples);
        Assert.Contains("2:30",en.WorkedExamples);
        Assert.Contains("7:30",en.WorkedExamples);
        Assert.Contains("POSSIBLE matching hour hand",en.StepByStepSolutions);
        Assert.Contains("Find at least one OTHER hour-hand position",en.StepByStepSolutions);
        Assert.Contains("DRAW-MINUTE-HAND tasks",en.CommonMistakes);
        var current=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(current,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson));
    }

    [Fact]
    public void NextFiveGrade1SourceScopeAndPreviousBatchContentVersionsRemainIsolated()
    {
        var (doc, _, _) = Effective(
            CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode);
        var newBatch = Assert.Single(doc.Lessons,
            l => l.LessonCode == "PED:US-CCSS-MATH:G1:U02:L16");
        Assert.False(CommonCoreG1Unit2Part2SourceTeachingCorrections.IsTarget(doc,newBatch));
        Assert.False(CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.IsTarget(doc,newBatch));
        Assert.True(CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.IsTarget(doc,newBatch));
        Assert.Equal(CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.CorrectedContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,newBatch));
        var olderBatch=Assert.Single(doc.Lessons,
            l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U02:L11");
        Assert.False(CommonCoreG1Unit2EquationsAndStoriesSourceCorrections.IsTarget(doc,olderBatch));
        Assert.Equal(CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.CorrectedContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,olderBatch));
    }

    [Fact]
    public void IndependentArithmeticAssertionsForRepairedExamples()
    {
        Assert.Equal(4, 7 - 3);
        Assert.Equal(15, 7 + 5 + 3);
        Assert.Equal(9, 4 + 3 + 2);
        Assert.Equal(2, 4 - 2);
        Assert.Equal(30, 20 + 10);
        Assert.Equal(50, 60 - 10);
        Assert.Equal(70, 80 - 10);
    }
}
