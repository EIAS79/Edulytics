using System.Text.Json;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// B01 source-verified five-lesson off-topic shared-geometry concept repair:
/// original SHA, CCSS standards, teacher source and learner preservation guards.
/// Does not award academic L1-L6 passes.
/// </summary>
public sealed class UsCcssB01V9UnrelatedGeometryConceptRuleCorrectionsTests
{
    private const string Prefix = "PED:US-CCSS-MATH:";
    private const string BadRule =
        "Use mathematical attributes such as sides, angles, parallelism, or length";

    private static readonly string[] Codes =
    [
        Prefix + "G3:U04:L03",
        Prefix + "G3:U04:L12",
        Prefix + "G3:U05:L10",
        Prefix + "G3:U08:L01",
        Prefix + "G3:U08:L05"
    ];

    private static string Root()
    {
        DirectoryInfo? directory = new(Directory.GetCurrentDirectory());
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName,
                "artifacts/academic-us-ccss/batches/B01-v9-geometry-concept-contamination-five-source-verified.json")))
                return directory.FullName;
            directory = directory.Parent;
        }
        throw new FileNotFoundException("V9 off-topic geometry source evidence missing.");
    }

    [Fact]
    public void ExactlyFiveSourceVerifiedRepairsRemoveUnrelatedGeometryClassification()
    {
        using var file = JsonDocument.Parse(File.ReadAllText(Path.Combine(
            Root(), "artifacts/academic-us-ccss/batches/B01-v9-geometry-concept-contamination-five-source-verified.json")));
        var evidence = file.RootElement.GetProperty("rows").EnumerateArray().ToArray();
        Assert.Equal(5, evidence.Length);
        Assert.Equal(5, CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.ExpectedCount);
        Assert.Equal("BATCH_NO-GO", file.RootElement.GetProperty("academicDecision").GetString());

        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var all = docs.SelectMany(d => d.Lessons.Select(l => (Doc: d, Lesson: l)))
            .ToDictionary(x => x.Lesson.LessonCode, StringComparer.Ordinal);
        var actual = all.Values.Where(x =>
            CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.IsTarget(x.Doc, x.Lesson))
            .Select(x => x.Lesson.LessonCode).ToArray();
        Assert.Equal(Codes.OrderBy(x => x), actual.OrderBy(x => x));

        foreach (var item in evidence)
        {
            var code = item.GetProperty("lessonCode").GetString()!;
            var pair = all[code];
            Assert.Equal(pair.Lesson.SourceUrl,
                item.GetProperty("originalTeacherSourceUrl").GetString());
            Assert.Equal(pair.Lesson.CanonicalBodySha256,
                item.GetProperty("canonicalOriginalLearnerBodySha256").GetString());
            Assert.Equal(pair.Lesson.OutcomeCodes,
                item.GetProperty("outcomeCodes").EnumerateArray().Select(x => x.GetString()!).ToArray());
            Assert.Equal(CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(pair.Doc, pair.Lesson));

            var t = Assert.Single(pair.Lesson.Translations);
            Assert.DoesNotContain(BadRule, t.KeyConceptsAndRules);
            Assert.Contains(item.GetProperty("independentlyAuthoredReplacement").GetString()!,
                t.KeyConceptsAndRules);
            Assert.False(item.GetProperty("fullAcademicCertified").GetBoolean());
            Assert.DoesNotContain("geometry concept", t.Explanation, StringComparison.OrdinalIgnoreCase);
        }
    }

    [Theory]
    [InlineData("G3:U04:L03", "unknown")]
    [InlineData("G3:U04:L12", "12 tens")]
    [InlineData("G3:U05:L10", "same amount")]
    [InlineData("G3:U08:L01", "unit-fraction")]
    [InlineData("G3:U08:L05", "item price")]
    public void NewConceptMatchesTheActualOriginalLessonPurpose(string suffix, string term)
    {
        var t = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(x => x.Lessons)
            .Single(x => x.LessonCode == Prefix + suffix)
            .Translations.Single();
        Assert.Contains(term, t.KeyConceptsAndRules, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void UnrelatedRealGeometryLessonsAndEveryOtherB01IdentityRemainUntouched()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var before = docs.SelectMany(d => d.Lessons.Select(l => (d, l)))
            .ToDictionary(x => x.l.LessonCode, x =>
                CanonicalLessonContentMaterializer.ComputeLessonFingerprint(x.d, x.l));
        foreach (var doc in docs)
            CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.Apply(doc);
        var after = docs.SelectMany(d => d.Lessons.Select(l => (d, l)))
            .ToDictionary(x => x.l.LessonCode, x =>
                CanonicalLessonContentMaterializer.ComputeLessonFingerprint(x.d, x.l));
        Assert.Equal(before, after);
        Assert.Contains(Prefix + "G3:U07:L02", after.Keys);
        Assert.DoesNotContain(Prefix + "G3:U07:L02", Codes);
    }

    [Fact]
    public void SHAAndPartialBodyDriftCannotBypassTheReviewGuards()
    {
        var doc = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d => d.Lessons.Any(l => l.LessonCode == Prefix + "G3:U04:L12"));
        var lesson = doc.Lessons.Single(l => l.LessonCode == Prefix + "G3:U04:L12");
        Assert.False(CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections
            .CanUpgradeExisting(doc, lesson, "unreviewed-arbitrary-predecessor"));

        var prior = lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.Apply(doc));
        }
        finally { lesson.CanonicalBodySha256 = prior; }

        var t = lesson.Translations.Single();
        var text = t.KeyConceptsAndRules;
        try
        {
            t.KeyConceptsAndRules += " INJECTED_UNVERIFIED_CONCEPT";
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.Apply(doc));
        }
        finally { t.KeyConceptsAndRules = text; }
    }
}