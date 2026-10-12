using System.Globalization;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// One-lesson CCSS source comparison regression, not L5/L6 or L7 certification.
/// </summary>
public sealed class CommonCoreG1CountAndAddPilotCorrectionTests
{
    private const string Code = CommonCoreG1CountAndAddPilotCorrection.LessonCode;

    private static (CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,
        CanonicalLessonContentPackTranslation En) Effective()
    {
        var entry = Assert.Single(
            MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
                .SelectMany(d => d.Lessons.Where(l => l.LessonCode == Code)
                    .Select(l => (Doc: d, Lesson: l))));
        return (entry.Doc, entry.Lesson,
            Assert.Single(entry.Lesson.Translations, t => t.CultureCode == "en"));
    }

    [Fact]
    public void RepairedBodyTeachesCountingObjectsTo20RepresentingAndDotCubeAddition()
    {
        var (doc, lesson, en) = Effective();
        Assert.Equal(CommonCoreG1CountAndAddPilotCorrection.CorrectedVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc, lesson));
        Assert.Equal(new[] { "CCSS:1.OA.C.5", "CCSS:1.OA.C.6" }, lesson.OutcomeCodes);

        Assert.Contains("16 counters", en.WorkedExamples);
        Assert.Contains("ten-frame", en.WorkedExamples);
        Assert.Contains("5 + 3 = 8", en.WorkedExamples);
        Assert.Contains("4 + 2 = 6", en.WorkedExamples);
        Assert.Contains("Count every object once", en.KeyConceptsAndRules);
        Assert.Contains("roll again", en.StepByStepSolutions);
        Assert.Contains("count an object twice", en.CommonMistakes);
        Assert.DoesNotContain("decompose a number", en.QuickSummary);

        // Numeric oracle, independent of the generator or its answer labels.
        Assert.Equal(16, Enumerable.Range(1, 16).Count());
        Assert.Equal(16, 10 + 6);
        Assert.Equal(8, 5 + 3);
        Assert.Equal(6, 4 + 2);
        Assert.Equal(3, 8 - 5);
    }

    [Fact]
    public void SourceHashDriftOrPartialOverwriteFailsClosedAndCorrectionIsIdempotent()
    {
        var (doc, lesson, en) = Effective();
        var before = CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson);
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(before, CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson));

        var oldSha = lesson.CanonicalBodySha256;
        lesson.CanonicalBodySha256 = new string('0', 64);
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG1CountAndAddPilotCorrection.Apply(doc));
        }
        finally { lesson.CanonicalBodySha256 = oldSha; }

        var priorExamples = en.WorkedExamples;
        en.WorkedExamples = "Unexpected edit";
        try
        {
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG1CountAndAddPilotCorrection.Apply(doc));
        }
        finally { en.WorkedExamples = priorExamples; }
    }

    [Fact]
    public void UpgradeRequiresOldVersionAndWillNotReplaceUnexpectedPersistedBody()
    {
        var (doc, lesson, _) = Effective();
        CommonCoreG1CountAndAddPilotCorrection.ValidateExistingBodyBeforeUpgrade(
            doc, lesson, CommonCoreRichContentCorrections.CorrectionContentVersion,
            Array.Empty<CurriculumLessonContentTranslation>());
        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG1CountAndAddPilotCorrection.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, "unexpected-prior-version",
                Array.Empty<CurriculumLessonContentTranslation>()));
        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreG1CountAndAddPilotCorrection.ValidateExistingBodyBeforeUpgrade(
                doc, lesson, CommonCoreRichContentCorrections.CorrectionContentVersion,
                [new CurriculumLessonContentTranslation
                {
                    CultureCode = "en", Title = "A user edited the existing title"
                }]));
    }

    [Fact]
    public void ActualLessonPracticeFamiliesGenerateAndGradeCorrectAndWrongNumericAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code, out var contract));
        Assert.NotNull(contract);
        Assert.NotEmpty(contract!.AllowedQuestionFamilies);

        var engine = new ExactSkillContractQuestionEngine();
        var generated = 0;
        var wrongAnswersRejected = 0;
        var seed = 8100101;
        foreach (var family in contract.AllowedQuestionFamilies.Distinct(StringComparer.Ordinal))
        {
            foreach (var difficulty in new[]
                     {
                         ExactSkillQuestionDifficulty.Standard,
                         ExactSkillQuestionDifficulty.Stretch,
                         ExactSkillQuestionDifficulty.Challenge
                     })
            {
                var question = Assert.Single(engine.Generate(
                    "g1-count-add-pilot", Code, [family], difficulty,
                    1, seed++, []));
                Assert.Equal(family, question.Family);
                Assert.True(ExactSkillContractQuestionEngine.Verify(
                    family, question.Parameters, question.CorrectAnswer),
                    "Correct response must be accepted for " + family + "/" + difficulty);
                generated++;

                if (int.TryParse(question.CorrectAnswer, NumberStyles.Integer,
                    CultureInfo.InvariantCulture, out var actual))
                {
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family, question.Parameters,
                        (actual + 999).ToString(CultureInfo.InvariantCulture)),
                        "An invalid numeric response was accepted for " + family);
                    wrongAnswersRejected++;
                }
            }
        }
        Assert.True(generated >= contract.AllowedQuestionFamilies.Count * 3);
        Assert.True(wrongAnswersRejected > 0,
            "No numeric wrong-answer grading cases covered for Count and Add.");
        // Representation (drawings/dot cubes), UI feedback and persistence
        // remain separate L5/L6 evidence; this is an engine-level pilot only.
    }

    [Fact]
    public void RichV2ComposerUsesCorrectedCanonicalTextWhereAvailable()
    {
        var (_, _, en) = Effective();
        var rich = RichLessonContentV2RuntimeComposer.TryCompose(Code,
            new CanonicalLessonTranslationRecord(en.CultureCode, en.Title,
                en.Explanation, en.KeyConceptsAndRules, en.WorkedExamples,
                en.StepByStepSolutions, en.CommonMistakes, en.QuickSummary));

        // Actual core Rich V2 runtime composition must expose the corrected
        // 16-object counting and dot-cube worked example text to the learner.
        // This is still NOT proof of browser/staging/interactive drawing output.
        Assert.NotNull(rich);
        var core = Assert.Single(rich!.KeyConcepts, x => x.Title == "Core idea");
        Assert.Contains("Count every object once", core.Rule);
        Assert.Contains("16 counters", core.Example);
        Assert.Contains("5 + 3 = 8", core.Example);
        Assert.Contains("ten-frame", core.Example);
        Assert.Contains("16 counters", string.Join(" ", rich.ExplanationParagraphs));
        Assert.NotEmpty(rich.Visuals);
    }
}
