using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Verifies the complete 23-lesson V9 cause-level cohort and its four
/// family-specific corrections without sequential per-lesson certification.
/// </summary>
public sealed class UsCcssB01V9SharedNonMeasureConceptTemplateCorrectionsTests
{
    private const string Phrase = "Measurement units must be equal in size and placed or interpreted without gaps";
    private const string Prefix = "PED:US-CCSS-MATH:";
    private static IReadOnlyList<CanonicalLessonContentPackDocument> Documents() =>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();

    [Fact]
    public void OneSourceVerifiedSharedRootRemovesIrrelevantRulerConceptsFromExactly23Lessons()
    {
        var docs=Documents();
        var targets=docs.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.IsTarget(d,l))
            .Select(l=>(Document:d,Lesson:l))).ToArray();
        Assert.Equal(23,CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.ExpectedCount);
        Assert.Equal(23,targets.Length);
        Assert.Equal(23,targets.Select(x=>x.Lesson.LessonCode).Distinct(StringComparer.Ordinal).Count());
        foreach(var (doc,lesson) in targets)
        {
            var expectedEffectiveVersion =
                lesson.LessonCode == Prefix + "G2:U08:L07"
                    ? CommonCoreB01V9SharedComparisonProcedureCorrections.CorrectedVersion
                    : CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.CorrectedVersion;
            Assert.Equal(expectedEffectiveVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,lesson));
            var en=Assert.Single(lesson.Translations);
            Assert.DoesNotContain(Phrase,en.KeyConceptsAndRules);
            Assert.StartsWith("Key ideas:",en.KeyConceptsAndRules);
            Assert.Contains("- ",en.KeyConceptsAndRules);
        }
    }

    [Fact]
    public void FourInvalidMeasurementProceduresAreReplacedByCorrectMathFamilySharedRules()
    {
        var docs=Documents();
        var sources=docs.SelectMany(d=>d.Lessons).ToDictionary(x=>x.LessonCode,
            StringComparer.Ordinal);
        foreach(var code in new[] {"G2:U07:L07","G2:U07:L14","G4:U03:L03","G4:U03:L05"})
        {
            var en=Assert.Single(sources[Prefix+code].Translations);
            Assert.DoesNotContain("Step 1: Identify what needs to be measured.",en.StepByStepSolutions);
            Assert.DoesNotContain("leaving gaps, overlapping units",en.CommonMistakes);
        }
        Assert.Contains("compose 10 ones into 1 ten",sources[Prefix+"G2:U07:L07"]
            .Translations.Single().StepByStepSolutions);
        Assert.Contains("decompose 1 hundred as 10 tens",sources[Prefix+"G2:U07:L14"]
            .Translations.Single().StepByStepSolutions);
        Assert.Contains("n × (1/b) = n/b",sources[Prefix+"G4:U03:L03"]
            .Translations.Single().StepByStepSolutions);
        Assert.Contains("n × (a/b) = (n × a)/b",sources[Prefix+"G4:U03:L05"]
            .Translations.Single().StepByStepSolutions);
    }

    [Fact]
    public void RealLengthAndNumberLineLessonsRemainUntouchedByNonMeasureSharedRoot()
    {
        var docs=Documents();
        foreach(var code in new[]{"G2:U03:L02","G2:U03:L03","G3:U06:L01",
            "G3:U06:L03","G2:U04:L01","G3:U05:L05"})
        {
            var pair=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
                .Single(x=>x.Lesson.LessonCode==Prefix+code);
            Assert.False(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
                .IsTarget(pair.Doc,pair.Lesson));
            Assert.NotEqual(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
                .CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    pair.Doc,pair.Lesson));
        }
    }

    [Fact]
    public void RepeatedApplicationIsIdempotentAndSourceOrPartialBodyDriftFailsClosed()
    {
        var docs=Documents();
        var doc=docs.Single(d=>d.Lessons.Any(l=>l.LessonCode==Prefix+"G2:U07:L07"));
        var lesson=doc.Lessons.Single(l=>l.LessonCode==Prefix+"G2:U07:L07");
        var shaBefore=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson);
        CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.Apply(doc);
        Assert.Equal(shaBefore,
            CanonicalLessonContentMaterializer.ComputeLessonFingerprint(doc,lesson));
        var originalSha=lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.Apply(doc));
        }
        finally {lesson.CanonicalBodySha256=originalSha;}
        var en=Assert.Single(lesson.Translations);
        var saved=en.KeyConceptsAndRules;
        try
        {
            en.KeyConceptsAndRules += " injected drift";
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.Apply(doc));
        }
        finally {en.KeyConceptsAndRules=saved;}
    }

    [Fact]
    public void ProductionVersionPredecessorsCannotUpgradeArbitraryLessons()
    {
        var docs=Documents();
        var special=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Single(x=>x.Lesson.LessonCode==Prefix+"G4:U02:L03");
        Assert.True(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
            .CanUpgradeExisting(special.Doc,special.Lesson,
                CommonCoreSourceBoundHtmlEntityCorrections.CorrectedContentVersion));
        var other=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Single(x=>x.Lesson.LessonCode==Prefix+"G2:U05:L03");
        Assert.False(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
            .CanUpgradeExisting(other.Doc,other.Lesson,
                CommonCoreSourceBoundHtmlEntityCorrections.CorrectedContentVersion));
        Assert.False(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
            .CanUpgradeExisting(other.Doc,other.Lesson,"unreviewed-version"));
    }
}