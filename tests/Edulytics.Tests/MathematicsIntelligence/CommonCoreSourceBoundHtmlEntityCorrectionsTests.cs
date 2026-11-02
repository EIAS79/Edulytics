using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Machine-verifiable technical display repair, not academic approval.</summary>
public sealed class CommonCoreSourceBoundHtmlEntityCorrectionsTests
{
    private static List<(CanonicalLessonContentPackDocument Document,
        CanonicalLessonContentPackLesson Lesson)> Targets() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(document => document.Lessons
                .Where(lesson => CommonCoreSourceBoundHtmlEntityCorrections
                    .IsTarget(document, lesson))
                .Select(lesson => (Document: document, Lesson: lesson)))
            .ToList();

    [Fact]
    public void RemainingEightLegacyRepairsAndEightGrade5SourceRichSupersessionsAreIsolated()
    {
        var targets = Targets();
        // 19 original entries: 3 superseded by G2 authoring, and 8 Grade 5
        // lessons have migrated to the independently authored Grade 5 source
        // pack. Only the remaining eight legacy phase30 repairs apply.
        Assert.Equal(8, targets.Count);
        Assert.Equal(8, targets.Select(x => x.Lesson.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());

        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var grade5SourceCodes = CommonCoreSourceBoundHtmlEntityCorrections
            .TargetLessonCodes
            .Where(code => code.StartsWith("PED:US-CCSS-MATH:G5:", StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(8, grade5SourceCodes.Length);
        foreach (var code in grade5SourceCodes)
        {
            Assert.DoesNotContain(targets, x => x.Lesson.LessonCode == code);
            // Do not hide an old HTML repair by losing its replacement.
            // The source-specific Grade 5 Rich V2 item must still exist.
            Assert.NotNull(RichLessonContentV2Registry.Find(code, "en"));
        }
        Assert.Equal(0, documents.Where(x =>
            x.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode)
            .SelectMany(x => x.Lessons)
            .SelectMany(x => x.Translations.Where(t => t.CultureCode == "en"))
            .Count(t => t.WorkedExamples.Contains("&nbsp;", StringComparison.Ordinal)));

        foreach (var (document, lesson) in targets)
        {
            var english = lesson.Translations.Single(x => x.CultureCode == "en");
            Assert.DoesNotContain("&nbsp;", english.WorkedExamples);
            Assert.Equal(
                CommonCoreSourceBoundHtmlEntityCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    document, lesson));
            var baseline = CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(document, lesson);
            CanonicalLessonContentMaterializer.Materialize(document);
            Assert.Equal(baseline,
                CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
                    document, lesson));
        }

        // Three original G2 lessons now have independently source-SHA-reviewed
        // rich worked examples, so old nbsp-only repair must NOT overwrite them.
        var superseded=new[]{
            "PED:US-CCSS-MATH:G2:U04:L03",
            "PED:US-CCSS-MATH:G2:U06:L19",
            "PED:US-CCSS-MATH:G2:U09:L04"
        };
        foreach(var code in superseded)
        {
            Assert.DoesNotContain(targets,x=>x.Lesson.LessonCode==code);
            var row=documents.SelectMany(d=>d.Lessons.Where(l=>l.LessonCode==code)
                         .Select(l=>(Document:d,Lesson:l))).Single();
            Assert.DoesNotContain("&nbsp;",row.Lesson.Translations.Single(t=>t.CultureCode=="en").WorkedExamples);
            Assert.True(
                CommonCoreG2Unit4UnlabeledTicksTeachingCorrection.IsTarget(row.Document,row.Lesson) ||
                CommonCoreG2Unit6SourceCorrections.IsTarget(row.Document,row.Lesson) ||
                CommonCoreG2Unit9SourceCorrections.IsTarget(row.Document,row.Lesson));
        }
        Assert.DoesNotContain(targets, x =>
            x.Lesson.LessonCode == CommonCoreG4MultiplesSourceCorrection.LessonCode);
    }

    [Fact]
    public void ModifiedSourceOrUnexpectedProseCannotBeSilentlyRewritten()
    {
        var (document, lesson) = Targets()[0];
        var original = lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256 = new string('0', 64);
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreSourceBoundHtmlEntityCorrections.Apply(document));
        }
        finally
        {
            lesson.CanonicalBodySha256 = original;
        }

        var english = lesson.Translations.Single(x => x.CultureCode == "en");
        var previous = english.WorkedExamples;
        try
        {
            english.WorkedExamples += " UNREVIEWED_TEXT";
            Assert.Throws<InvalidOperationException>(() =>
                CommonCoreSourceBoundHtmlEntityCorrections.Apply(document));
        }
        finally
        {
            english.WorkedExamples = previous;
        }

        Assert.True(CommonCoreSourceBoundHtmlEntityCorrections
            .CanUpgradeExisting(document, lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion));
        Assert.False(CommonCoreSourceBoundHtmlEntityCorrections
            .CanUpgradeExisting(document, lesson, "unknown-version"));

        CommonCoreSourceBoundHtmlEntityCorrections
            .ValidateExistingBodyBeforeUpgrade(document, lesson,
                CommonCoreRichContentCorrections.CorrectionContentVersion,
                Array.Empty<CurriculumLessonContentTranslation>());

        Assert.Throws<InvalidOperationException>(() =>
            CommonCoreSourceBoundHtmlEntityCorrections
                .ValidateExistingBodyBeforeUpgrade(document, lesson,
                    CommonCoreRichContentCorrections.CorrectionContentVersion,
                    [new CurriculumLessonContentTranslation
                    {
                        CultureCode = "en",
                        Title = "UNRECOGNIZED",
                        WorkedExamples = "UNRECOGNIZED"
                    }]));
    }

    [Fact]
    public void OtherSourceEditionsAndNonTargetsAreExcluded()
    {
        var (document, lesson) = Targets()[0];
        var originalVersion = document.ContentVersion;
        try
        {
            document.ContentVersion = "other-source-edition";
            Assert.False(CommonCoreSourceBoundHtmlEntityCorrections.IsTarget(
                document, lesson));
            var originalWorked = lesson.Translations.Single(x => x.CultureCode == "en")
                .WorkedExamples;
            CommonCoreSourceBoundHtmlEntityCorrections.Apply(document);
            Assert.Equal(originalWorked,
                lesson.Translations.Single(x => x.CultureCode == "en")
                    .WorkedExamples);
        }
        finally
        {
            document.ContentVersion = originalVersion;
        }

        var other = document.Lessons.First(x =>
            x.LessonCode != lesson.LessonCode);
        Assert.False(CommonCoreSourceBoundHtmlEntityCorrections.IsTarget(
            document, other));
    }
}
