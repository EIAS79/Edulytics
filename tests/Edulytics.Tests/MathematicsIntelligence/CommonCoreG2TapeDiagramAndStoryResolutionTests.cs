using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG2TapeDiagramAndStoryResolutionTests
{
    private static (CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English)
        Effective(string code)
    {
        var entry = Assert.Single(MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons
                .Where(lesson => lesson.LessonCode == code)
                .Select(lesson => (Doc: doc, Lesson: lesson))));

        return (entry.Doc, entry.Lesson,
            Assert.Single(entry.Lesson.Translations, t => t.CultureCode == "en"));
    }

    [Theory]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramLessonCode,
        CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramContentVersion)]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.SolveStoriesLessonCode,
        CommonCoreG2TapeDiagramAndStoryResolutionCorrection.SolveStoriesContentVersion)]
    public void CorrectsOnlySourceAnchoredGrade2Content_AndKeepsArithmeticValid(
        string code, string version)
    {
        var (doc, lesson, english) = Effective(code);
        Assert.Equal(new[] { "CCSS:2.NBT.B.5", "CCSS:2.OA.A.1" },
            lesson.OutcomeCodes);
        Assert.Equal(version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson));

        Assert.DoesNotContain("Equivalent algebraic transformations",
            english.KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Apply the key idea from the lesson",
            english.StepByStepSolutions, StringComparison.OrdinalIgnoreCase);
        Assert.True(english.WorkedExamples.Length > 400);
        Assert.True(english.StepByStepSolutions.Contains("Step 5:", StringComparison.Ordinal));

        if (code == CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramLessonCode)
        {
            Assert.Contains("part-whole", english.KeyConceptsAndRules,
                StringComparison.OrdinalIgnoreCase);
            Assert.Contains("47 - 19 = 28", english.WorkedExamples);
            Assert.Contains("23 + 17 + 50 = 90", english.WorkedExamples);
            Assert.Contains("63 - 26 = 37", english.WorkedExamples);
            Assert.Equal(28, 47 - 19);
            Assert.Equal(90, 23 + 17 + 50);
            Assert.Equal(37, 63 - 26);
        }
        else
        {
            Assert.Contains("number lines", english.KeyConceptsAndRules,
                StringComparison.OrdinalIgnoreCase);
            Assert.Contains("38 - 14 = 24", english.WorkedExamples);
            Assert.Contains("72 - 20 = 52", english.WorkedExamples);
            Assert.Contains("43 + 18 = 61", english.WorkedExamples);
            Assert.Equal(24, 38 - 14);
            Assert.Equal(46, 72 - 26);
            Assert.Equal(61, 43 + 18);
        }
    }

    [Theory]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramLessonCode)]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.SolveStoriesLessonCode)]
    public void CorrectionsAreIdempotent_AndRejectUnexpectedSourceAndProse(string code)
    {
        var (doc, lesson, english) = Effective(code);
        var originalHash = CanonicalLessonContentMaterializer
            .ComputeLessonFingerprint(doc, lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(originalHash,
            CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson));

        var originalSource = lesson.CanonicalBodySha256;
        lesson.CanonicalBodySha256 = new string('0', 64);
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2TapeDiagramAndStoryResolutionCorrection.Apply(doc));
        }
        finally { lesson.CanonicalBodySha256 = originalSource; }

        var originalSteps = english.StepByStepSolutions;
        english.StepByStepSolutions = "Unauthorized silent content change";
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2TapeDiagramAndStoryResolutionCorrection.Apply(doc));
        }
        finally { english.StepByStepSolutions = originalSteps; }
    }

    [Theory]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramLessonCode)]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.SolveStoriesLessonCode)]
    public void HistoricalStorageGuardsRejectUnexpectedVersionAndBody(string code)
    {
        var (doc, lesson, _) = Effective(code);
        CommonCoreG2TapeDiagramAndStoryResolutionCorrection
            .ValidateExistingBodyBeforeUpgrade(doc, lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion,
                Array.Empty<CurriculumLessonContentTranslation>());

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2TapeDiagramAndStoryResolutionCorrection
                .ValidateExistingBodyBeforeUpgrade(doc, lesson,
                    "unrecognized-legacy-version",
                    Array.Empty<CurriculumLessonContentTranslation>()));

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2TapeDiagramAndStoryResolutionCorrection
                .ValidateExistingBodyBeforeUpgrade(doc, lesson,
                    CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation
                    {
                        CultureCode = "en", Title = "Unknown changed title"
                    }]));
    }

    [Theory]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramLessonCode)]
    [InlineData(CommonCoreG2TapeDiagramAndStoryResolutionCorrection.SolveStoriesLessonCode)]
    public void RichV2UsesEffectiveCorrectedConceptAndExamples(string code)
    {
        var (_, lesson, english) = Effective(code);
        var rich = RichLessonContentV2RuntimeComposer.TryCompose(code,
            new CanonicalLessonTranslationRecord(
                english.CultureCode, english.Title, english.Explanation,
                english.KeyConceptsAndRules, english.WorkedExamples,
                english.StepByStepSolutions, english.CommonMistakes,
                english.QuickSummary));
        Assert.NotNull(rich);
        Assert.Contains("Example 1", rich!.KeyConcepts[0].Example,
            StringComparison.Ordinal);
        Assert.DoesNotContain("Equivalent algebraic transformations",
            rich.KeyConcepts[0].Rule, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UnrelatedGrade2LessonAndEarlierStoryWritingRepairAreNotAltered()
    {
        var (doc, _, _) = Effective(
            CommonCoreG2TapeDiagramAndStoryResolutionCorrection.TapeDiagramLessonCode);

        var lesson12 = Assert.Single(doc.Lessons,
            x => x.LessonCode == CommonCoreG2StoryProblemsSourceCorrection.LessonCode);

        Assert.Equal(CommonCoreG2StoryProblemsSourceCorrection.CorrectedContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson12));

        var lesson10 = Assert.Single(doc.Lessons,
            x => x.LessonCode == "PED:US-CCSS-MATH:G2:U09:L10");

        Assert.Equal(CommonCoreRichContentCorrections.CorrectionContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson10));
    }
}
