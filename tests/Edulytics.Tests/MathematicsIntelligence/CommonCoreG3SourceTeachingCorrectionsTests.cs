using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authoring-stage regression contracts only; Stage D / CI not run.</summary>
public sealed class CommonCoreG3SourceTeachingCorrectionsTests
{
    private static CanonicalLessonContentPackDocument Grade3Document() => new()
    {
        PackCode = CommonCoreG3SourceTeachingCorrections.PackCode,
        VersionCode = "CCSSM-2010",
        ContentVersion = CommonCoreG3SourceTeachingCorrections.BaseContentVersion
    };

    [Fact]
    public void EverySourceAlignedGrade3LessonHasAnEmbeddedVersionedRule()
    {
        Assert.Equal(143, CommonCoreG3SourceTeachingCorrections.CorrectionCount);
    }

    [Fact]
    public void ExactFirstEditionSourceHashIsRequiredAndUpgradeIsConservative()
    {
        var document = Grade3Document();
        var lesson = new CanonicalLessonContentPackLesson
        {
            LessonCode = "PED:US-CCSS-MATH:G3:U01:L09",
            SourceEdition = "1st Edition (2021)",
            CanonicalBodySha256 =
                "6c907b7f1945d2b123f490edb6995a800fe142ee682231097b0f2433cd7eb2d4"
        };

        Assert.True(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Equal(CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion,
            CommonCoreG3SourceTeachingCorrections.GetExpectedContentVersion(
                document, lesson, document.ContentVersion));
        Assert.True(CommonCoreG3SourceTeachingCorrections.CanUpgradeExisting(
            document, lesson, CommonCoreG3SourceTeachingCorrections.BaseContentVersion));
        Assert.False(CommonCoreG3SourceTeachingCorrections.CanUpgradeExisting(
            document, lesson, "unknown-or-preexisting-translation-v0"));

        lesson.CanonicalBodySha256 = new string('0', 64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.False(CommonCoreG3SourceTeachingCorrections.CanUpgradeExisting(
            document, lesson, CommonCoreG3SourceTeachingCorrections.BaseContentVersion));
    }

    [Fact]
    public void DifferentGradesAndUnapprovedPackVersionsAreNeverTargeted()
    {
        var document=Grade3Document();
        var lesson=new CanonicalLessonContentPackLesson
        {
            LessonCode="PED:US-CCSS-MATH:G2:U01:L09",
            SourceEdition="1st Edition (2021)",
            CanonicalBodySha256="6c907b7f1945d2b123f490edb6995a800fe142ee682231097b0f2433cd7eb2d4"
        };
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));

        lesson.LessonCode="PED:US-CCSS-MATH:G3:U01:L09";
        document.ContentVersion="unreviewed-replacement";
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document,lesson));
        document.ContentVersion=CommonCoreG3SourceTeachingCorrections.BaseContentVersion;
        document.PackCode="CAMBRIDGE-INTL-MATH";
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document,lesson));
    }
    [Fact]
    public void Lesson1CanonicalAuthoringExplainsEveryGraphRevealBeforeGrading()
    {
        // The full embedded 2021 G3 pack is the guarded baseline. This test
        // checks the authored A-B-C candidate only; it is NOT Stage D success
        // and must not be executed during source-authoring.
        var assembly = typeof(CanonicalLessonContentPackDocument).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(name =>
            name.EndsWith("us-ccss-math-g3-phase29-v1.lesson-content-pack.json",
                StringComparison.Ordinal));
        using var stream = assembly.GetManifestResourceStream(resource)
            ?? throw new InvalidOperationException("Missing Grade 3 source pack.");
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(
            stream, options) ?? throw new InvalidOperationException("Invalid pack.");
        Assert.Equal(143, document.Lessons.Count);

        CommonCoreG3SourceTeachingCorrections.Apply(document);
        var lesson = document.Lessons.Single(l =>
            l.LessonCode == "PED:US-CCSS-MATH:G3:U01:L01");
        var english = lesson.Translations.Single(t => t.CultureCode == "en");

        Assert.Contains("4, 3, 1, 2, 7 and 6", english.Explanation);
        Assert.Contains("First, study", english.Explanation);
        Assert.Contains("Next, reveal the title", english.Explanation);
        Assert.Contains("Finally, read", english.Explanation);
        Assert.Contains("Walk 4, Bike 3, Skateboard 1, Car 2", english.Explanation);
        Assert.Contains("from 0 to 8 in steps of 1", english.Explanation);
        Assert.Contains("two " +
            "answerable questions", english.WorkedExamples);
        Assert.Contains("teacher review", english.Explanation);
        Assert.Contains("context", english.QuickSummary);
        Assert.Equal(
            CommonCoreG3SourceTeachingCorrections.CorrectionContentVersion,
            CommonCoreG3SourceTeachingCorrections.GetExpectedContentVersion(
                document, lesson, document.ContentVersion));

        // A public equivalent is allowed, but original restricted assessment
        // prompts and first-party numeric answer keys must not be introduced.
        Assert.DoesNotContain("G3-U01-L01-EDU-CD-COUNT", english.WorkedExamples);
    }

    [Fact]
    public void SeparateGrade1AndGrade2DocumentsBypassTheGrade3FullSetGuard()
    {
        var document=Grade3Document();
        document.Lessons.Add(new CanonicalLessonContentPackLesson {
            LessonCode="PED:US-CCSS-MATH:G1:U01:L01"
        });
        document.Lessons.Add(new CanonicalLessonContentPackLesson {
            LessonCode="PED:US-CCSS-MATH:G2:U01:L01"
        });
        CommonCoreG3SourceTeachingCorrections.Apply(document);
        Assert.Equal(2,document.Lessons.Count);
    }

}
