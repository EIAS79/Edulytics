using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Twelve independently source-purpose-checked fraction lessons had a common
/// incorrect generic measurement template. Tests cover their single guarded
/// materialization boundary; does not assert academic certification.
/// </summary>
public sealed class UsCcssB01FractionPurposeCorrectionsTests
{
    private static readonly string[] TargetLessons =
    [
        "PED:US-CCSS-MATH:G3:U05:L02",
        "PED:US-CCSS-MATH:G3:U05:L03",
        "PED:US-CCSS-MATH:G3:U05:L04",
        "PED:US-CCSS-MATH:G3:U05:L06",
        "PED:US-CCSS-MATH:G3:U05:L07",
        "PED:US-CCSS-MATH:G3:U05:L08",
        "PED:US-CCSS-MATH:G3:U05:L09",
        "PED:US-CCSS-MATH:G4:U02:L01",
        "PED:US-CCSS-MATH:G4:U02:L02",
        "PED:US-CCSS-MATH:G4:U03:L01",
        "PED:US-CCSS-MATH:G4:U03:L02",
        "PED:US-CCSS-MATH:G4:U03:L04"
    ];

    [Fact]
    public void AllTwelvePurposeAlignedCorrectionsAppearInRealEffectiveLearnerBodies()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode == "US-CCSS-MATH" && d.VersionCode == "CCSSM-2010")
            .ToArray();
        var all = documents.SelectMany(d => d.Lessons.Select(l => (Document: d, Lesson: l)))
            .ToArray();
        var targeted = all.Where(x =>
            CommonCoreB01FractionPurposeCorrections.IsTarget(x.Document, x.Lesson)).ToArray();
        Assert.Equal(12, targeted.Length);
        Assert.Equal(TargetLessons.OrderBy(x => x), targeted
            .Select(x => x.Lesson.LessonCode).OrderBy(x => x));
        foreach (var (document, lesson) in targeted)
        {
            Assert.Equal(CommonCoreB01FractionPurposeCorrections.CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(document, lesson));
            var t = Assert.Single(lesson.Translations);
            Assert.Equal("en", t.CultureCode);
            Assert.True(t.KeyConceptsAndRules.Contains("fraction", StringComparison.OrdinalIgnoreCase) ||
                t.KeyConceptsAndRules.Contains("1/b", StringComparison.Ordinal) ||
                t.KeyConceptsAndRules.Contains("a/b", StringComparison.Ordinal),
                "Missing source-goal fraction concept for " + lesson.LessonCode);
            Assert.True(t.StepByStepSolutions.Contains("fraction", StringComparison.OrdinalIgnoreCase) ||
                t.StepByStepSolutions.Contains("1/b", StringComparison.Ordinal) ||
                t.StepByStepSolutions.Contains("a/b", StringComparison.Ordinal),
                "Missing fraction procedure for " + lesson.LessonCode);
            Assert.Contains("Mistake:", t.CommonMistakes);
            Assert.DoesNotContain("Step 1: Identify what needs to be measured",
                t.StepByStepSolutions);
            Assert.DoesNotContain("Common mistake: using unequal units, leaving gaps",
                t.CommonMistakes);
            Assert.DoesNotContain("Use mathematical attributes such as sides, angles",
                t.KeyConceptsAndRules);
            Assert.DoesNotContain("Measurement units must be equal in size and placed",
                t.KeyConceptsAndRules);
            Assert.DoesNotContain("Approach: Choose the required unit, align equal units",
                t.WorkedExamples);
        }
    }

    [Fact]
    public void SourceGuardedRepairIsIdempotentAndRejectsTamperedSourceFingerprint()
    {
        var document = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.PackCode == "US-CCSS-MATH" &&
                d.VersionCode == "CCSSM-2010" &&
                d.Lessons.Any(x => x.LessonCode ==
                    "PED:US-CCSS-MATH:G3:U05:L04"));
        var lesson = document.Lessons.Single(x =>
            x.LessonCode == "PED:US-CCSS-MATH:G3:U05:L04");
        var before = CanonicalLessonContentMaterializer.ComputeLessonFingerprint(document,lesson);
        CommonCoreB01FractionPurposeCorrections.Apply(document);
        Assert.Equal(before,
            CanonicalLessonContentMaterializer.ComputeLessonFingerprint(document,lesson));
        var original = lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreB01FractionPurposeCorrections.Apply(document));
        }
        finally
        {
            lesson.CanonicalBodySha256 = original;
        }
    }

    [Fact]
    public void SourceGuardedRepairRejectsPartialLearnerBodyDrift()
    {
        var document = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.PackCode == "US-CCSS-MATH" &&
                d.VersionCode == "CCSSM-2010" &&
                d.Lessons.Any(x => x.LessonCode ==
                    "PED:US-CCSS-MATH:G4:U03:L04"));
        var lesson = document.Lessons.Single(x =>
            x.LessonCode == "PED:US-CCSS-MATH:G4:U03:L04");
        var body = Assert.Single(lesson.Translations);
        var old = body.StepByStepSolutions;
        try
        {
            body.StepByStepSolutions += " Unreviewed change.";
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreB01FractionPurposeCorrections.Apply(document));
        }
        finally
        {
            body.StepByStepSolutions = old;
        }
    }

    [Fact]
    public void UnrelatedLessonsAndCurriculaDoNotMatchTheRepair()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        foreach(var d in documents)
        foreach(var l in d.Lessons)
            if(!TargetLessons.Contains(l.LessonCode, StringComparer.Ordinal))
                Assert.False(CommonCoreB01FractionPurposeCorrections.IsTarget(d,l));
    }
}
