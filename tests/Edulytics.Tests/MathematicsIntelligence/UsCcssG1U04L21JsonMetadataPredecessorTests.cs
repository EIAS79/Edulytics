using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Regression for the production JSON-only U04:L21 v1 to v2 metadata migration.
/// Source, math and authoring tests remain separately immutable.
/// </summary>
public sealed class UsCcssG1U04L21JsonMetadataPredecessorTests
{
    [Fact]
    public void OnlyApprovedLessonAndExactVersionPairWithNoPersistedProseMayUpgrade()
    {
        var doc = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x => x.Lessons.Any(l => l.LessonCode ==
                CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersLessonCode));
        var lesson = doc.Lessons.Single(x => x.LessonCode ==
            CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersLessonCode);
        var unrelated = doc.Lessons.First(x => x.LessonCode != lesson.LessonCode);
        var v1 = CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersVersionV1;
        var v2 = CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersVersion;
        bool Allowed(string before, string after, bool empty, bool unrelatedLesson = false) =>
            MathematicsCanonicalLessonContentSeeder.IsGuardedG1U04L21JsonMetadataPredecessor(
                doc, unrelatedLesson ? unrelated : lesson, before, after, empty);

        Assert.True(Allowed(v1, v2, empty:true));
        Assert.False(Allowed(v1, v2, empty:false));
        Assert.False(Allowed("unknown-production-version", v2, empty:true));
        Assert.False(Allowed(v1, "unknown-new-version", empty:true));
        Assert.False(Allowed(v2, v2, empty:true));
        Assert.False(Allowed(v1, v2, empty:true, unrelatedLesson:true));

        // Source-specific body overwrite remains forbidden even with v1 metadata.
        Assert.False(CommonCoreG1SourceGroundedInstructionalCorrections
            .CanUpgradeExisting(doc, lesson, v1));
    }
}
