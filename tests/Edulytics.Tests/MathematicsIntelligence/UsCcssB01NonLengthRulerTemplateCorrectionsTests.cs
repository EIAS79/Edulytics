using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-checked non-length cohort from the 500-way V9 shared-root-cause audit.
/// These checks do not imply that B01 is academically certified.
/// </summary>
public sealed class UsCcssB01NonLengthRulerTemplateCorrectionsTests
{
    private const string G2 = "PED:US-CCSS-MATH:G2:U05:L01";
    private const string G3 = "PED:US-CCSS-MATH:G3:U07:L02";
    private const string G4 = "PED:US-CCSS-MATH:G4:U04:L19";
    private static IReadOnlyList<CanonicalLessonContentPackDocument> Documents() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();

    [Fact]
    public void ExactlyThreeSourceVerifiedNonLengthLessonsAreCorrectedWithoutChangingActualLengthLessons()
    {
        var docs = Documents();
        var targets = docs.SelectMany(d => d.Lessons
            .Where(l => CommonCoreB01NonLengthRulerTemplateCorrections.IsTarget(d, l))
            .Select(l => (d, l))).ToArray();
        Assert.Equal(new[] { G2, G3, G4 }, targets.Select(x => x.l.LessonCode)
            .OrderBy(x => x, StringComparer.Ordinal).ToArray());
        foreach (var (document, lesson) in targets)
        {
            var t = Assert.Single(lesson.Translations);
            Assert.Equal(CommonCoreB01NonLengthRulerTemplateCorrections.CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(document, lesson));
            Assert.DoesNotContain("Measurement units must be equal in size", t.KeyConceptsAndRules);
            Assert.DoesNotContain("Identify what needs to be measured", t.StepByStepSolutions);
            Assert.DoesNotContain("align equal units from the endpoint", t.WorkedExamples);
            Assert.DoesNotContain("leaving gaps, overlapping units", t.CommonMistakes);
        }
        var actualLength = docs.SelectMany(d => d.Lessons.Select(l => (d, l)))
            .Single(x => x.l.LessonCode == "PED:US-CCSS-MATH:G2:U03:L03");
        Assert.False(CommonCoreB01NonLengthRulerTemplateCorrections.IsTarget(actualLength.d, actualLength.l));
    }

    [Fact]
    public void MathematicalObjectivesAndWorkedAnswersMatchTheirSourceGoals()
    {
        var lessons = Documents().SelectMany(d => d.Lessons).ToDictionary(l => l.LessonCode);
        var hundreds = Assert.Single(lessons[G2].Translations);
        Assert.Contains("10 tens", hundreds.KeyConceptsAndRules);
        Assert.Contains("70 + 30 = 100", hundreds.WorkedExamples);
        Assert.Contains("10 × 10 = 100", hundreds.WorkedExamples);
        var shapes = Assert.Single(lessons[G3].Translations);
        Assert.Contains("three sides", shapes.WorkedExamples);
        Assert.Contains("square", shapes.CommonMistakes);
        Assert.DoesNotContain("Find the total using an efficient strategy", shapes.StepByStepSolutions);
        var regrouping = Assert.Single(lessons[G4].Translations);
        Assert.Contains("Compose 10 ones as 1 ten", regrouping.KeyConceptsAndRules);
        Assert.Contains("8,299+1,111=9,410", regrouping.WorkedExamples);
        Assert.Contains("5,203-2,786=2,417", regrouping.WorkedExamples);
    }

    [Fact]
    public void FixIsIdempotentAndRejectsSourceHashAndPartialBodyDrift()
    {
        foreach (var code in new[] { G2, G3, G4 })
        {
            var doc = Documents().Single(d => d.Lessons.Any(l => l.LessonCode == code));
            var lesson = doc.Lessons.Single(l => l.LessonCode == code);
            var before = CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson);
            CommonCoreB01NonLengthRulerTemplateCorrections.Apply(doc);
            Assert.Equal(before, CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc, lesson));
            var realSha = lesson.CanonicalBodySha256;
            try
            {
                lesson.CanonicalBodySha256 = new string('0', 64);
                Assert.Throws<InvalidOperationException>(() =>
                    CommonCoreB01NonLengthRulerTemplateCorrections.Apply(doc));
            }
            finally { lesson.CanonicalBodySha256 = realSha; }
            var t = Assert.Single(lesson.Translations);
            var saved = t.CommonMistakes;
            try
            {
                t.CommonMistakes += " Injected partial drift.";
                Assert.Throws<InvalidOperationException>(() =>
                    CommonCoreB01NonLengthRulerTemplateCorrections.Apply(doc));
            }
            finally { t.CommonMistakes = saved; }
        }
    }
}
