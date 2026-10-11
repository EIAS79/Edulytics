using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG2StoryProblemsSourceCorrectionTests
{
    private static (CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation English) GetEffectiveLesson()
    {
        var match = Assert.Single(MathematicsCanonicalLessonContentSeeder
            .LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons
                .Where(lesson => lesson.LessonCode ==
                    CommonCoreG2StoryProblemsSourceCorrection.LessonCode)
                .Select(lesson => (document, lesson))));

        return (match.document, match.lesson,
            Assert.Single(match.lesson.Translations, x => x.CultureCode == "en"));
    }

    [Fact]
    public void SourceGroundedStoryWriting_UsesGrade2QuestionsInsteadOfAlgebraicTransformations()
    {
        var (doc, lesson, english) = GetEffectiveLesson();
        Assert.Equal("CCSS:2.OA.A.1", Assert.Single(lesson.OutcomeCodes));
        Assert.Equal(CommonCoreG2StoryProblemsSourceCorrection.CorrectedContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson));

        Assert.Contains("story problem", english.KeyConceptsAndRules,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("18 + 13 = 31", english.WorkedExamples);
        Assert.Contains("18 + 7 = 25", english.WorkedExamples);
        Assert.Contains("64 - 35 = 29", english.WorkedExamples);
        Assert.Contains("31 - 12 = 19", english.WorkedExamples);
        Assert.Contains("Re-read the story", english.StepByStepSolutions);
        Assert.DoesNotContain("algebraic transformations", english.KeyConceptsAndRules,
            StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("graphs, or tables", english.StepByStepSolutions,
            StringComparison.OrdinalIgnoreCase);

        Assert.Equal(31, 18 + 13);
        Assert.Equal(25, 18 + 7);
        Assert.Equal(29, 64 - 35);
        Assert.Equal(19, 31 - 12);
    }

    [Fact]
    public void GuardAndIdempotence_RejectSourceDriftAndPreserveUnrelatedGrade2Lesson()
    {
        var (doc, lesson, english) = GetEffectiveLesson();
        var fingerprint = CanonicalLessonContentMaterializer
            .ComputeLessonFingerprint(doc, lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(fingerprint,
            CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson));

        var sibling = Assert.Single(doc.Lessons,
            x => x.LessonCode == "PED:US-CCSS-MATH:G2:U09:L11");
        Assert.Equal(CommonCoreRichContentCorrections.CorrectionContentVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, sibling));
        Assert.False(CommonCoreG2StoryProblemsSourceCorrection.IsTarget(doc, sibling));

        var originalSha = lesson.CanonicalBodySha256;
        lesson.CanonicalBodySha256 = new string('0', 64);
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2StoryProblemsSourceCorrection.Apply(doc));
        }
        finally
        {
            lesson.CanonicalBodySha256 = originalSha;
        }

        var originalSteps = english.StepByStepSolutions;
        english.StepByStepSolutions = "Unexpected replacement";
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2StoryProblemsSourceCorrection.Apply(doc));
        }
        finally
        {
            english.StepByStepSolutions = originalSteps;
        }
    }

    [Fact]
    public void PersistedUpgrade_FailsClosedOnVersionCultureAndBodyDrift()
    {
        var (doc, lesson, _) = GetEffectiveLesson();

        CommonCoreG2StoryProblemsSourceCorrection.ValidateExistingBodyBeforeUpgrade(
            doc, lesson, CommonCoreRichContentCorrections.CorrectionContentVersion,
            Array.Empty<CurriculumLessonContentTranslation>());

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2StoryProblemsSourceCorrection.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, "unexpected-previous-version",
                Array.Empty<CurriculumLessonContentTranslation>()));

        var wrong = new CurriculumLessonContentTranslation
        {
            CultureCode = "en",
            Title = "Changed story text",
            Explanation = "Different content"
        };
        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG2StoryProblemsSourceCorrection.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, CommonCoreRichContentCorrections.CorrectionContentVersion,
                [wrong]));
    }

    [Fact]
    public void EffectiveRichV2Content_UsesCorrectedStoryWritingRules()
    {
        var (_, lesson, english) = GetEffectiveLesson();
        var rich = RichLessonContentV2RuntimeComposer.TryCompose(
            lesson.LessonCode,
            new CanonicalLessonTranslationRecord(
                english.CultureCode, english.Title, english.Explanation,
                english.KeyConceptsAndRules, english.WorkedExamples,
                english.StepByStepSolutions, english.CommonMistakes, english.QuickSummary));
        Assert.NotNull(rich);
        Assert.Contains("18 + 13 = 31",
            rich!.KeyConcepts[0].Example, StringComparison.Ordinal);
        Assert.DoesNotContain("Equivalent algebraic transformations",
            rich.KeyConcepts[0].Rule, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("unknown", rich.KeyConcepts[0].Rule,
            StringComparison.OrdinalIgnoreCase);
    }
}
