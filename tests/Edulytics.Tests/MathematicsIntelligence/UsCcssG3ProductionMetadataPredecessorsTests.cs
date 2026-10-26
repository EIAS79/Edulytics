using System;
using System.Linq;
using Edulytics.Data.Seeding;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Read-only observed production metadata on 2026-10-26: 141 Grade 3 rows
/// have common-core-rich-content-v1 and exactly U01:L12/U01:L15 have the
/// prior B001 source-goal repair version. Only the latter TWO are selected
/// by the existing 294-entry metadata-only upgrade inventory. Never modify
/// saved learner/teacher translations or claim academic source parity.
/// </summary>
public sealed class UsCcssG3ProductionMetadataPredecessorsTests
{
    private const string OldB001 =
        "us-ccss-b001-original-source-goals-repaired-v1";

    [Fact]
    public void OnlyTwoObservedGrade3B001RowsAllowSourceBoundMetadataOnlyUpgrade()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var allGrade3 = documents.SelectMany(d =>
                d.Lessons.Select(l => (Document: d, Lesson: l)))
            .Where(x => x.Lesson.LessonCode.StartsWith(
                "PED:US-CCSS-MATH:G3:", StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(143, allGrade3.Length);
        Assert.Equal(143, allGrade3.Select(x => x.Lesson.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());

        var codes = new[]
        {
            "PED:US-CCSS-MATH:G3:U01:L12",
            "PED:US-CCSS-MATH:G3:U01:L15"
        };
        foreach (var code in codes)
        {
            var (document, lesson) = allGrade3
                .Single(x => x.Lesson.LessonCode == code);
            Assert.True(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
            Assert.True(MathematicsCanonicalLessonContentSeeder
                .IsSourceBoundJsonMetadataCandidate(document, lesson));
            var expected = CanonicalLessonContentMaterializer
                .GetEffectiveContentVersion(document, lesson);
            Assert.Equal(
                CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion,
                expected);

            bool Guard(string before, string after, bool withoutPersistedProse) =>
                MathematicsCanonicalLessonContentSeeder
                    .IsGuardedG3KnownProductionJsonMetadataPredecessor(
                        document, lesson, before, after, withoutPersistedProse);

            Assert.True(Guard(OldB001, expected, true));
            Assert.False(Guard(OldB001, expected, false));
            Assert.False(Guard("common-core-rich-content-v1", expected, true));
            Assert.False(Guard("unverified-content-version", expected, true));
            Assert.False(Guard(OldB001, "unreviewed-next-version", true));
            Assert.False(Guard(expected, expected, true));
            Assert.False(CanonicalLessonContentMaterializer
                .CanUpgradeExisting(document, lesson, OldB001));

            var hash = lesson.CanonicalBodySha256;
            lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.False(Guard(OldB001, expected, true));
            lesson.CanonicalBodySha256 = hash;

            var edition = lesson.SourceEdition;
            lesson.SourceEdition = "unreviewed-edition";
            Assert.False(Guard(OldB001, expected, true));
            lesson.SourceEdition = edition;

            var version = document.VersionCode;
            document.VersionCode = "unverified-framework";
            Assert.False(Guard(OldB001, expected, true));
            document.VersionCode = version;
        }

        var other = allGrade3.Single(x =>
            x.Lesson.LessonCode == "PED:US-CCSS-MATH:G3:U01:L11");
        Assert.False(MathematicsCanonicalLessonContentSeeder
            .IsGuardedG3KnownProductionJsonMetadataPredecessor(
                other.Document, other.Lesson, OldB001,
                CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion,
                verifiedNoPersistedTranslations: true));

        var grade2 = documents.SelectMany(d =>
                d.Lessons.Select(l => (Document: d, Lesson: l)))
            .Single(x => x.Lesson.LessonCode == "PED:US-CCSS-MATH:G2:U02:L01");
        Assert.False(MathematicsCanonicalLessonContentSeeder
            .IsGuardedG3KnownProductionJsonMetadataPredecessor(
                grade2.Document, grade2.Lesson, OldB001,
                CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion,
                verifiedNoPersistedTranslations: true));
    }
}
