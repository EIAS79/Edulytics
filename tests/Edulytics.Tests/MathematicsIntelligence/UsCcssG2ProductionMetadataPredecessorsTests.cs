using System;
using System.Linq;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Exact 2026-10-23 SELECT-only inventory of legacy G2 ReadFromJson metadata
/// on Khalid's production Neon branch. No mutation of persisted pupil data or
/// independent academic attestation is authorized by this guard.
/// </summary>
public sealed class UsCcssG2ProductionMetadataPredecessorsTests
{
    private static readonly (string Code, string OldVersion)[] LegacyRows =
    [
        ("PED:US-CCSS-MATH:G2:U02:L01", "us-ccss-b001-original-source-goals-repaired-v1"),
        ("PED:US-CCSS-MATH:G2:U03:L12", "us-ccss-b001-original-source-goals-repaired-v1"),
        ("PED:US-CCSS-MATH:G2:U04:L03", "us-ccss-source-bound-nbsp-display-v1"),
        ("PED:US-CCSS-MATH:G2:U06:L07", "us-ccss-g2-source-grounded-foundations-v1"),
        ("PED:US-CCSS-MATH:G2:U06:L08", "us-ccss-g2-source-grounded-foundations-v1"),
        ("PED:US-CCSS-MATH:G2:U06:L09", "us-ccss-g2-source-grounded-foundations-v1"),
        ("PED:US-CCSS-MATH:G2:U06:L19", "us-ccss-source-bound-nbsp-display-v1"),
        ("PED:US-CCSS-MATH:G2:U09:L04", "us-ccss-source-bound-nbsp-display-v1"),
        ("PED:US-CCSS-MATH:G2:U09:L05", "us-ccss-g2-source-grounded-foundations-v1"),
        ("PED:US-CCSS-MATH:G2:U09:L11", "us-ccss-g2-u09-l11-tape-diagram-relationships-correction-v1"),
        ("PED:US-CCSS-MATH:G2:U09:L12", "us-ccss-g2-u09-l12-story-writing-source-correction-v1"),
        ("PED:US-CCSS-MATH:G2:U09:L13", "us-ccss-g2-u09-l13-solve-represent-story-correction-v1"),
    ];

    // Four other non-generic stored versions were observed in the same
    // SELECT-only production inventory. They must already be current or
    // supported by their existing source-specific migration contracts.
    private static readonly (string Code, string OldVersion)[] ExistingPriorRows =
    [
        ("PED:US-CCSS-MATH:G2:U01:L01", "us-ccss-g2-source-grounded-foundations-v1"),
        ("PED:US-CCSS-MATH:G2:U05:L01", "us-ccss-g2-u05-hundreds-source-goal-v1"),
        ("PED:US-CCSS-MATH:G2:U05:L02", "us-ccss-g2-u05-hundreds-source-goal-v1"),
        ("PED:US-CCSS-MATH:G2:U05:L03", "us-ccss-g2-u05-hundreds-source-goal-v1"),
    ];

    [Fact]
    public void RemainingObservedNonGenericG2MetadataIsAlreadyCurrentOrSourceGuarded()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        foreach (var (code, before) in ExistingPriorRows)
        {
            var (document, lesson) = docs
                .SelectMany(d => d.Lessons.Select(l => (Document: d, Lesson: l)))
                .Single(x => x.Lesson.LessonCode == code);
            var after = CanonicalLessonContentMaterializer
                .GetEffectiveContentVersion(document, lesson);
            Assert.True(after == before ||
                CanonicalLessonContentMaterializer.CanUpgradeExisting(
                    document, lesson, before),
                $"Unrecognized non-generic production metadata predecessor for {code}: {before} -> {after}");
        }
    }

    [Fact]
    public void OnlyExactPriorSourceMetadataOnDocumentedG2LessonsMayAdvance()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var candidateCodes = LegacyRows.Select(x => x.Code).ToHashSet(StringComparer.Ordinal);
        var rows = docs.SelectMany(d => d.Lessons.Select(l => (Document: d, Lesson: l)))
            .Where(x => candidateCodes.Contains(x.Lesson.LessonCode)).ToArray();
        Assert.Equal(LegacyRows.Length, rows.Length);
        foreach (var (code, oldVersion) in LegacyRows)
        {
            var (document, lesson) = rows.Single(x => x.Lesson.LessonCode == code);
            var expected = CanonicalLessonContentMaterializer
                .GetEffectiveContentVersion(document, lesson);
            bool Guard(string previous, string next, bool persistedProseIsAbsent) =>
                MathematicsCanonicalLessonContentSeeder
                    .IsGuardedG2KnownProductionJsonMetadataPredecessor(
                        document, lesson, previous, next, persistedProseIsAbsent);

            // Historical version == current version needs no migration and
            // deliberately does not qualify for a redundant upgrade.
            Assert.Equal(oldVersion != expected,
                Guard(oldVersion, expected, persistedProseIsAbsent: true));
            Assert.False(Guard(oldVersion, expected, persistedProseIsAbsent: false));
            Assert.False(Guard("unknown-production-content-version", expected, true));
            Assert.False(Guard(oldVersion, "unknown-target-content-version", true));
            Assert.False(Guard(expected, expected, true));
        }
        var outsider = docs.SelectMany(d => d.Lessons.Select(l => (Document: d, Lesson: l)))
            .Single(x => x.Lesson.LessonCode == "PED:US-CCSS-MATH:G2:U05:L04");
        Assert.False(MathematicsCanonicalLessonContentSeeder
            .IsGuardedG2KnownProductionJsonMetadataPredecessor(
                outsider.Document, outsider.Lesson,
                "us-ccss-b001-original-source-goals-repaired-v1",
                "any-content-version", verifiedNoPersistedTranslations: true));
    }
}
