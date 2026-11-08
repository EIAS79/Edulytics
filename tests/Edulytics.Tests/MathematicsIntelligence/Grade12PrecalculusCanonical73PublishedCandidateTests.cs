using Edulytics.Core.Curriculum;
using Edulytics.Core.Enums;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Checks the actual embedded canonical materialization, not only raw RichV2.
/// Academic approval is owned by the programme owner; HTTP and database
/// publication must be checked separately by the isolated HTTPS acceptance.
/// </summary>
public sealed class Grade12PrecalculusCanonical73PublishedCandidateTests
{
    private const string Prefix =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:";

    [Fact]
    public void OwnerApprovedGrade12CandidateHasExactly73CanonicalStudentBodies()
    {
        var packs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var grade12 = packs
            .Where(x => x.Lessons.Any(l =>
                l.LessonCode.StartsWith(Prefix, StringComparison.Ordinal)))
            .ToArray();
        var pack = Assert.Single(grade12);
        Assert.Equal(CanonicalLessonContentStatus.Published, pack.Status);
        Assert.Contains("programme owner", pack.ReviewedBy,
            StringComparison.OrdinalIgnoreCase);
        Assert.Contains("grade12-owner-academic-release-authorization",
            pack.ReviewEvidence, StringComparison.OrdinalIgnoreCase);

        var lessons = pack.Lessons;
        Assert.Equal(73, lessons.Count);
        Assert.Equal(73, lessons.Select(l => l.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());

        var expected = new[] { 7, 4, 9, 8, 4, 3, 6, 8, 8, 5, 7, 4 }
            .SelectMany((count, index) =>
                Enumerable.Range(1, count)
                    .Select(n => Prefix + $"U{index + 1:D2}:L{n:D2}"))
            .ToArray();
        Assert.Equal(expected, lessons.Select(l => l.LessonCode));

        foreach (var lesson in lessons)
        {
            var t = Assert.Single(lesson.Translations);
            Assert.Equal("en", t.CultureCode);
            Assert.NotNull(RichLessonContentV2Registry.Find(lesson.LessonCode, "en"));
            Assert.False(string.IsNullOrWhiteSpace(t.Title));
            Assert.True(t.Explanation.Length >= 40);
            Assert.True(t.WorkedExamples.Length >= 20);
            Assert.True(t.StepByStepSolutions.Length >= 20);
            Assert.True(t.CommonMistakes.Length >= 20);
            Assert.True(t.QuickSummary.Length >= 20);
        }
    }
}
