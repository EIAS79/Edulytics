using Edulytics.Core.Curriculum;
using Edulytics.Data.Repositories;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.Phase29;

/// <summary>
/// Guards the previous release's seven source-bound corrections plus the five
/// new Grade 2 candidates, without claiming human certification. Only these
/// lessons may have JSON production metadata versions reconciled on startup.
/// </summary>
public sealed class SourceBoundCommonCoreJsonMetadataReconciliationTests
{
    [Fact]
    public void MetadataOnlyStartupSelector_ExposesExactCurrentSourceBoundLessons()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var chosen = documents.SelectMany(d => d.Lessons
                .Where(l => MathematicsCanonicalLessonContentSeeder
                    .IsSourceBoundJsonMetadataCandidate(d, l))
                .Select(l => (Doc: d, Lesson: l)))
            .ToArray();

        // Previous 56 source-bound lessons plus exact source-SHA-guarded Grade 1 B03 L12/L14.
        const int expectedSourceBoundCount = 31 + 3 + 7 + 6 + 6 + 1 + 2 + 2 + 3 + 3 + 4 + 2 + 5 + 5 + 5 + 5 + 5 + 5 + 28;
        // Exact next five source-bound IM2021 G1 Unit 2 lesson rows, not a broad catalogue expansion.
        Assert.Equal(expectedSourceBoundCount, chosen.Length);
        Assert.Equal(expectedSourceBoundCount, chosen.Select(x => x.Lesson.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());
        Assert.All(chosen, x =>
        {
            Assert.Equal(MathematicsCurriculumPackRegistry.CommonCoreCode,
                x.Doc.PackCode);
            Assert.Equal("CCSSM-2010", x.Doc.VersionCode);
            Assert.NotEqual(CommonCoreRichContentCorrections.CorrectionContentVersion,
                CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(x.Doc, x.Lesson));
            Assert.True(x.Lesson.CanonicalBodySha256.Length == 64);
        });

        var selected = chosen.Select(x => x.Lesson.LessonCode)
            .ToHashSet(StringComparer.Ordinal);
        foreach(var code in CommonCoreG1Unit2NextFiveSourceTeachingCorrections.TargetLessonCodes)
            Assert.Contains(code,selected);
        // V1 G1-006: five source-verified, SHA-locked U02 L11-L15 only.
        for(var n=11;n<=15;n++)
            Assert.Contains($"PED:US-CCSS-MATH:G1:U02:L{n:00}",selected);
        // V1 G1-007: strict source-bound five lessons with unchanged grade inventory.
        for(var n=16;n<=20;n++)
            Assert.Contains($"PED:US-CCSS-MATH:G1:U02:L{n:00}",selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U04:L21", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L02", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L05", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L02", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L14", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L15", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L16", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L08", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L09", selected);
        for (var n=14;n<=27;n++)
            Assert.Contains($"PED:US-CCSS-MATH:G1:U03:L{n:00}",selected);
        foreach(var n in new[]{1,2,4,5,6,7,8,9,10,11,12,13,14,15})
            Assert.Contains($"PED:US-CCSS-MATH:G1:U04:L{n:00}",selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U03:L28", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L11", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L06", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L08", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L07", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L11", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L12", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L14", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L03", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L13", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L04", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L07", selected);
        Assert.Contains(CommonCoreG1CountAndAddPilotCorrection.LessonCode, selected);
        Assert.Contains(CommonCoreT001MixedAddSubtractContentCorrection.LessonCode, selected);
        Assert.Contains(CommonCoreT001FindThePairContentCorrection.LessonCode, selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L04", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L10", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L17", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L13", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U04:L03", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U01:L01", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U06:L07", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U06:L08", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U06:L09", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U09:L05", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U09:L11", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U09:L12", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U09:L13", selected);
        foreach (var code in CommonCoreG2HundredsPlaceValueSourceCorrections.TargetLessonCodes)
            Assert.Contains(code, selected);
        Assert.Contains("PED:US-CCSS-MATH:G4:U01:L01", selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U04:L03", selected);
        Assert.Contains("PED:US-CCSS-MATH:G4:U07:L14", selected);
        Assert.Contains("PED:US-CCSS-MATH:G5:U08:L09", selected);

        var nonTarget = documents
            .Single(x => x.PackCode == MathematicsCurriculumPackRegistry.CommonCoreCode &&
                x.Lessons.Any(l => l.LessonCode == "PED:US-CCSS-MATH:G2:U01:L02"));
        Assert.False(MathematicsCanonicalLessonContentSeeder
            .IsSourceBoundJsonMetadataCandidate(nonTarget,
                nonTarget.Lessons.Single(l =>
                    l.LessonCode == "PED:US-CCSS-MATH:G2:U01:L02")));
    }

    [Fact]
    public void JsonIndex_AndDatabaseVersionContract_NeedMetadataSynchronization()
    {
        var index = new EmbeddedCanonicalLessonContentIndex();
        Assert.True(index.TryGet("PED:US-CCSS-MATH:G2:U06:L08", out var content));
        Assert.Equal(
            CommonCoreG2SourceGroundedFoundationsCorrections.CorrectedContentVersion,
            content.ContentVersion);
    }
}
