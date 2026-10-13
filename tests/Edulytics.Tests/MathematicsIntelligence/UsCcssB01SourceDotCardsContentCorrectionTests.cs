using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// G1 U01 L02 is repaired inside the single B01 500-lesson source-linked
/// pipeline. Source pack remains unchanged and L1-L6 are not self-certified.
/// </summary>
public sealed class UsCcssB01SourceDotCardsContentCorrectionTests
{
    private const string Code = CommonCoreG1DotCardMatchingCorrection.LessonCode;

    [Fact]
    public void All500MaterializedBodiesIncludeTheSourceBoundActivityCorrection()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode == "US-CCSS-MATH" &&
                d.VersionCode == "CCSSM-2010").ToArray();

        var lesson = docs.SelectMany(d => d.Lessons.Select(l => (d,l)))
            .Single(x => x.l.LessonCode == Code);
        var en = Assert.Single(lesson.l.Translations.Where(t => t.CultureCode == "en"));
        Assert.Equal(CommonCoreG1DotCardMatchingCorrection.CorrectedVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                lesson.d,lesson.l));
        Assert.Contains("dot groups",en.KeyConceptsAndRules,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Match dot pictures",en.WorkedExamples,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Draw the missing dots",en.WorkedExamples,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Check It Off",en.WorkedExamples);
        Assert.Contains("5 + 2 = 7",en.WorkedExamples);
        Assert.Contains("cards 0 through 5",en.CommonMistakes);
        Assert.Equal(
            "https://im.kendallhunt.com/k5/teachers/grade-1/unit-1/lesson-2/preparation.html",
            lesson.l.SourceUrl);
        Assert.Equal("7d8c9aa59717d264f80522bcf07c5385b5f332b2808b66c5c75fb19672c70e33",
            lesson.l.CanonicalBodySha256);

        var before = CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
            lesson.d,lesson.l);
        CommonCoreG1DotCardMatchingCorrection.Apply(lesson.d);
        var after = CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
            lesson.d,lesson.l);
        Assert.Equal(before,after);
        Assert.True(CommonCoreG1DotCardMatchingCorrection.CanUpgradeExisting(
            lesson.d,lesson.l,"common-core-rich-content-v1"));
        Assert.False(CommonCoreG1DotCardMatchingCorrection.CanUpgradeExisting(
            lesson.d,lesson.l,"unexpected-content-version"));
    }

    [Fact]
    public void OriginalPriorGrade1PilotLessonIdentityAndVersionArePreserved()
    {
        var doc = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.PackCode == "US-CCSS-MATH" &&
                d.VersionCode == "CCSSM-2010" &&
                d.Lessons.Any(l=>l.LessonCode == Code));
        var original = doc.Lessons.Single(l =>
            l.LessonCode == "PED:US-CCSS-MATH:G1:U01:L01");
        Assert.Equal("us-ccss-g1-u01-l01-count-collections-dots-v1",
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                doc,original));
        var neighboring = doc.Lessons.Single(l =>
            l.LessonCode == "PED:US-CCSS-MATH:G1:U01:L03");
        Assert.False(CommonCoreG1DotCardMatchingCorrection.IsTarget(doc,neighboring));
    }
}
