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
}
