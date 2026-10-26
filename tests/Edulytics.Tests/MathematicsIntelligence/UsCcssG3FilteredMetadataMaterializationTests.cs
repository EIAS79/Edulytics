using System;
using System.Linq;
using Edulytics.Data.Seeding;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Production regression for the partial Grade 3 metadata-only filter:
/// canonical Grade 3 is materialized once at all-143 scope and must not
/// be re-materialized after source-bound seeding filters it to two rows.
/// </summary>
public sealed class UsCcssG3FilteredMetadataMaterializationTests
{
    [Fact]
    public void FullGrade3IsMaterializedBeforeTwoSourceBoundB001RowsAreFiltered()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var complete = documents.Single(d => d.Lessons.Any(l =>
            l.LessonCode == "PED:US-CCSS-MATH:G3:U01:L12"));
        var allGrade3 = complete.Lessons
            .Where(l => l.LessonCode.StartsWith(
                "PED:US-CCSS-MATH:G3:", StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(143, allGrade3.Length);

        var selected = allGrade3.Where(l =>
            MathematicsCanonicalLessonContentSeeder.IsSourceBoundJsonMetadataCandidate(
                complete, l)).ToArray();
        Assert.Equal(
            new[] {
                "PED:US-CCSS-MATH:G3:U01:L12",
                "PED:US-CCSS-MATH:G3:U01:L15"
            }, selected.Select(l => l.LessonCode).ToArray());

        foreach (var lesson in selected)
        {
            Assert.True(CommonCoreG3SourceTeachingCorrections.IsTarget(complete, lesson));
            Assert.Equal(
                CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    complete, lesson));
        }

        // This is the original bug: whole-grade Apply validates 143 original
        // Grade 3 source rows, correctly throwing on a filtered 2-lesson pack.
        // Production must preserve that guard and seed these PRE-materialized
        // source-bound rows without applying the full-grade transform twice.
        var filteredPack = new Edulytics.Core.Curriculum.CanonicalLessonContentPackDocument
        {
            PackCode = complete.PackCode,
            VersionCode = complete.VersionCode,
            ContentVersion = complete.ContentVersion,
            Lessons = selected.ToList()
        };
        var exception = Assert.Throws<InvalidOperationException>(() =>
            CanonicalLessonContentMaterializer.Materialize(filteredPack));
        Assert.Contains("Expected the full Grade 3 2021 set of 143 lessons",
            exception.Message);
        // Fail-closed behavior still holds for incomplete unvalidated packs.
    }
}
