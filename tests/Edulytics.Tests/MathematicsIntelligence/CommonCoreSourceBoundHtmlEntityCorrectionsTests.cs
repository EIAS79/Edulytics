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
    public void ExactlyNineteenCorrections_LeaveZeroNbspInCommonCoreWorkedExamples()
    {
        var targets = Targets();
        Assert.Equal(19, targets.Count);
        Assert.Equal(19, targets.Select(x => x.Lesson.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());

        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        Assert.Equal(0, documents.Where(x =>
            x.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode)
            .SelectMany(x => x.Lessons)
            .SelectMany(x => x.Translations.Where(t => t.CultureCode == "en"))
            .Count(t => t.WorkedExamples.Contains("&nbsp;", StringComparison.Ordinal)));

        foreach (var (document, lesson) in targets)
        {
            var english = lesson.Translations.Single(x => x.CultureCode == "en");
            Assert.DoesNotContain("&nbsp;", english.WorkedExamples);
            // A V9 source-reviewed shared root fix builds on the existing
            // nbsp display fix for this ONE overlapping identity. Both fixes
            // must still be applied and idempotent, without reversion.
            var expectedEffectiveVersion =
                lesson.LessonCode == "PED:US-CCSS-MATH:G4:U02:L03"
                    ? CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.CorrectedVersion
                    : CommonCoreSourceBoundHtmlEntityCorrections.CorrectedContentVersion;
            Assert.Equal(expectedEffectiveVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    document, lesson));
            var baseline = CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(document, lesson);
            CanonicalLessonContentMaterializer.Materialize(document);
            Assert.Equal(baseline,
                CanonicalLessonContentMaterializer.ComputeLessonFingerprint(
                    document, lesson));
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
