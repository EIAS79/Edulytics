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
    public void UnrelatedGrade1LessonRetainsPriorVersion()
    {
        var (doc, _, _) = Effective(
            CommonCoreG1SourceGroundedInstructionalCorrections.DataQuestionsLessonCode);
        var untouched = Assert.Single(doc.Lessons,
            l => l.LessonCode == "PED:US-CCSS-MATH:G1:U01:L12");
        Assert.Equal(CommonCoreRichContentCorrections.CorrectionContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, untouched));
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
