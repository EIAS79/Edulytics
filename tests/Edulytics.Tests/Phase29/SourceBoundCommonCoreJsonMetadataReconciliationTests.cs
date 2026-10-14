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
    public void MetadataOnlyStartupSelector_ExposesExactV9SourceBoundRootCohorts()
    {
        var documents = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var chosen = documents.SelectMany(d => d.Lessons
                .Where(l => MathematicsCanonicalLessonContentSeeder
                    .IsSourceBoundJsonMetadataCandidate(d, l))
                .Select(l => (Doc: d, Lesson: l)))
            .ToArray();

        // Source-bound corrections counted as a SET: of the 23 V9 root members, G4 U02 L03 was already selected by the HTML display fix; only 22 identities are new.
        const int expectedSourceBoundCount = 31 + 3 + 7 + 6 + 6 + 1 + 12 + 1 + 2 + 3 + 22 + 1 + 19 + 28 + 18 + 16 + 5; // reviewed additions + vetted subtraction + 5 independent shared concept fixes
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
        Assert.Contains("PED:US-CCSS-MATH:G1:U04:L21", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L02", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L05", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L02", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L14", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L15", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L16", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L08", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L09", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U03:L28", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L11", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L06", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L08", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L07", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U01:L11", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U06:L03", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L13", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L04", selected);
        Assert.Contains("PED:US-CCSS-MATH:G1:U07:L07", selected);
        Assert.Contains(CommonCoreG1CountAndAddPilotCorrection.LessonCode, selected);
        Assert.Contains("PED:US-CCSS-MATH:G2:U05:L01", selected);
        Assert.Contains("PED:US-CCSS-MATH:G3:U07:L02", selected);
        Assert.Contains("PED:US-CCSS-MATH:G4:U04:L19", selected);
        var sharedCohort = documents.SelectMany(d => d.Lessons.Where(l =>
            CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.IsTarget(d,l)))
            .Select(l => l.LessonCode).ToArray();
        Assert.Equal(23, sharedCohort.Length);
        Assert.Equal(23, sharedCohort.Distinct(StringComparer.Ordinal).Count());
        Assert.All(sharedCohort, code => Assert.Contains(code, selected));
        var studentVoice = documents.SelectMany(d => d.Lessons.Where(l =>
            CommonCoreB01V9LearnerVoiceSharedRootCorrection.IsTarget(d,l))).Select(l => l.LessonCode).ToArray();
        Assert.Equal(2, studentVoice.Length);
        Assert.All(studentVoice, code => Assert.Contains(code, selected));
        var familyProcedures=documents.SelectMany(d => d.Lessons.Where(l =>
            CommonCoreB01V9SharedComparisonProcedureCorrections.IsTarget(d,l))).Select(l => l.LessonCode).ToArray();
        Assert.Equal(20, familyProcedures.Length);
        Assert.Equal(20, familyProcedures.Distinct(StringComparer.Ordinal).Count());
        Assert.All(familyProcedures, code => Assert.Contains(code, selected));
        var geometryProcedures=documents.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreB01V9SharedGeometryProcedureCorrections.IsTarget(d,l))).Select(l=>l.LessonCode).ToArray();
        Assert.Equal(28,geometryProcedures.Length);
        Assert.Equal(28,geometryProcedures.Distinct(StringComparer.Ordinal).Count());
        Assert.All(geometryProcedures,code=>Assert.Contains(code,selected));
        var verifiedAdditionProcedures=documents.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreB01V9SharedAdditionProcedureCorrections.IsTarget(d,l))).Select(l=>l.LessonCode).ToArray();
        Assert.Equal(21,verifiedAdditionProcedures.Length);
        Assert.All(verifiedAdditionProcedures,code=>Assert.Contains(code,selected));
        var subtractionProcedures=documents.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreB01V9SharedSubtractionProcedureCorrections.IsTarget(d,l))).Select(l=>l.LessonCode).ToArray();
        Assert.Equal(16,subtractionProcedures.Length);
        Assert.All(subtractionProcedures,code=>Assert.Contains(code,selected));
        var unrelatedGeometry=documents.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreB01V9UnrelatedGeometryConceptRuleCorrections.IsTarget(d,l))).Select(l=>l.LessonCode).ToArray();
        Assert.Equal(5,unrelatedGeometry.Length);
        Assert.All(unrelatedGeometry,code=>Assert.Contains(code,selected));

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
