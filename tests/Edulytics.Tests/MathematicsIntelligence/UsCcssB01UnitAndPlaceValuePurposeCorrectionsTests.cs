using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Two teacher-preparation verified source-goal repairs for a common
/// incorrectly reused ruler-measurement template. These assertions do NOT
/// certify the 500 lessons or replace independent mathematical evaluation.
/// </summary>
public sealed class UsCcssB01UnitAndPlaceValuePurposeCorrectionsTests
{
    private const string WeightCode = "PED:US-CCSS-MATH:G3:U06:L06";
    private const string TenThousandCode = "PED:US-CCSS-MATH:G4:U04:L06";
    private static IReadOnlyList<CanonicalLessonContentPackDocument> Documents() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();

    [Fact]
    public void OnlyTwoVerifiedLessonsAreTargetsAndNeitherContainsRulerTemplate()
    {
        var docs = Documents();
        var matches = docs.SelectMany(d=>d.Lessons
            .Where(l => CommonCoreB01UnitAndPlaceValuePurposeCorrections.IsTarget(d,l))
            .Select(l => (d,l))).ToArray();
        Assert.Equal(2,matches.Length);
        Assert.Equal(new[]{WeightCode,TenThousandCode},
            matches.Select(x=>x.l.LessonCode).OrderBy(x=>x,StringComparer.Ordinal).ToArray());
        foreach(var (document,lesson) in matches)
        {
            var t=Assert.Single(lesson.Translations);
            Assert.Equal("en",t.CultureCode);
            Assert.Equal(CommonCoreB01UnitAndPlaceValuePurposeCorrections.CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(document,lesson));
            Assert.DoesNotContain("Choose the required unit, align equal units",t.WorkedExamples);
            Assert.DoesNotContain("Identify what needs to be measured",t.StepByStepSolutions);
            Assert.DoesNotContain("leaving gaps, overlapping units",t.CommonMistakes);
            Assert.DoesNotContain("Measurement units must be equal in size",t.KeyConceptsAndRules);
        }
    }

    [Fact]
    public void GradeThreeWeightsUseGramsKilogramsAndIndependentlyCheckedComparisons()
    {
        var lesson=Documents().SelectMany(d=>d.Lessons)
            .Single(l=>l.LessonCode==WeightCode);
        var t=Assert.Single(lesson.Translations);
        Assert.Equal("CCSS:3.MD.A.2",Assert.Single(lesson.OutcomeCodes));
        Assert.Contains("280 g - 250 g = 30 g",t.WorkedExamples);
        Assert.Contains("1 kg = 1,000 g",t.WorkedExamples);
        Assert.Contains("1,000 g - 650 g = 350 g",t.WorkedExamples);
        Assert.Contains("weighing scale",t.CommonMistakes);
        Assert.DoesNotContain("Pixabay",t.WorkedExamples);
        Assert.DoesNotContain("Pexels",t.WorkedExamples);
    }

    [Fact]
    public void GradeFourTenThousandsUseActualPlaceValueRatherThanLengths()
    {
        var lesson=Documents().SelectMany(d=>d.Lessons)
            .Single(l=>l.LessonCode==TenThousandCode);
        var t=Assert.Single(lesson.Translations);
        Assert.Equal("CCSS:4.NBT.A.1",Assert.Single(lesson.OutcomeCodes));
        Assert.Contains("10 × 1,000 = 10,000",t.KeyConceptsAndRules);
        Assert.Contains("1 × 10,000",t.WorkedExamples);
        Assert.Contains("ten-thousands place",t.StepByStepSolutions);
        Assert.Contains("Ten hundreds make 1,000",t.CommonMistakes);
    }

    [Fact]
    public void RepairsAreIdempotentButUnexpectedPublisherFingerprintOrPartialBodyFailClosed()
    {
        foreach (var code in new[]{WeightCode,TenThousandCode})
        {
            var doc=Documents().Single(d=>d.Lessons.Any(l=>l.LessonCode==code));
            var l=doc.Lessons.Single(l=>l.LessonCode==code);
            var fp=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,l);
            CommonCoreB01UnitAndPlaceValuePurposeCorrections.Apply(doc);
            Assert.Equal(fp,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,l));
            var oldSha=l.CanonicalBodySha256;
            try
            {
                l.CanonicalBodySha256=new string('0',64);
                Assert.Throws<InvalidOperationException>(() =>
                    CommonCoreB01UnitAndPlaceValuePurposeCorrections.Apply(doc));
            }
            finally {l.CanonicalBodySha256=oldSha;}
            var t=Assert.Single(l.Translations);
            var old=t.CommonMistakes;
            try
            {
                t.CommonMistakes += " Unreviewed mutation.";
                Assert.Throws<InvalidOperationException>(() =>
                    CommonCoreB01UnitAndPlaceValuePurposeCorrections.Apply(doc));
            }
            finally {t.CommonMistakes=old;}
        }
    }
}
