using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG2Unit3YardstickTeachingCorrectionTests
{
    [Fact]
    public void OriginalOptionalYardstickIsSourceGuardedAndDoesNotInventPhysicalMeasurements()
    {
        var target = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d => d.Lessons.Where(l =>
                CommonCoreG2Unit3YardstickTeachingCorrection.IsTarget(d, l))
                .Select(l => (Doc: d, Lesson: l))).Single();
        var english = target.Lesson.Translations.Single(x => x.CultureCode == "en");

        Assert.Contains("3×5", english.Explanation);
        Assert.Contains("36", english.Explanation);
        Assert.Contains("REAL", english.Explanation);
        Assert.Contains("No genuine class desk measurements", english.Explanation);
        Assert.Contains("3+3−5", english.KeyConceptsAndRules);
        Assert.Contains("FICTIONAL", english.WorkedExamples);
        Assert.Equal(CommonCoreG2Unit3YardstickTeachingCorrection.Version,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(target.Doc, target.Lesson));
    }

    [Fact]
    public void UnknownPersistedVersionAndChangedOriginalSourceFailClosed()
    {
        var target = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d => d.Lessons.Where(l =>
                CommonCoreG2Unit3YardstickTeachingCorrection.IsTarget(d, l))
                .Select(l => (Doc: d, Lesson: l))).Single();
        Assert.False(CommonCoreG2Unit3YardstickTeachingCorrection.CanUpgradeExisting(
            target.Doc, target.Lesson, "unapproved-predecessor"));
        var original = target.Lesson.CanonicalBodySha256;
        try
        {
            target.Lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreG2Unit3YardstickTeachingCorrection.Apply(target.Doc));
        }
        finally { target.Lesson.CanonicalBodySha256 = original; }
    }
}
