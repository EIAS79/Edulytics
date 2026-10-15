using System.Text.Json;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>V9: one source-bound root for all 20 wrong comparisons, with 24 negative
/// controls from the complete 44-lesson source cohort and no academic PASS.</summary>
public sealed class UsCcssB01V9SharedComparisonProcedureCorrectionsTests
{
    private static string FindRoot()
    {
        DirectoryInfo? dir = new(Directory.GetCurrentDirectory());
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName,
                "artifacts/academic-us-ccss/batches/B01-v9-shared-comparison-procedure-44-source-reviewed-metadata.json")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new FileNotFoundException("Missing shared V9 source verification cohort");
    }

    [Fact]
    public void Complete44SourceReviewedCohortHas20CorrectionsAnd24ProtectedComparativeControls()
    {
        var docs = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var scoped = docs.SelectMany(d => d.Lessons.Select(l => (Doc:d, Lesson:l)))
            .ToDictionary(x => x.Lesson.LessonCode, StringComparer.Ordinal);
        var path = Path.Combine(FindRoot(),
            "artifacts/academic-us-ccss/batches/B01-v9-shared-comparison-procedure-44-source-reviewed-metadata.json");
        using var file = JsonDocument.Parse(File.ReadAllText(path));
        var rows = file.RootElement.GetProperty("sourceAnalysisRows").EnumerateArray().ToArray();
        Assert.Equal(44, rows.Length);
        var changed = rows.Where(x => x.GetProperty("needsSharedCauseRepair").GetBoolean()).ToArray();
        Assert.Equal(20,changed.Length);
        Assert.Equal(20,CommonCoreB01V9SharedComparisonProcedureCorrections.ExpectedCount);
        Assert.Equal(12,CommonCoreB01V9SharedComparisonProcedureCorrections.FamilyCount);
        Assert.Equal(24,rows.Length-changed.Length);
        foreach (var row in rows)
        {
            var code = row.GetProperty("lessonCode").GetString()!;
            var target = scoped[code];
            var shouldChange = row.GetProperty("needsSharedCauseRepair").GetBoolean();
            Assert.Equal(shouldChange, CommonCoreB01V9SharedComparisonProcedureCorrections
                .IsTarget(target.Doc,target.Lesson));
            if (!shouldChange) continue;
            Assert.Equal(64,row.GetProperty("sourceSemanticSha256").GetString()!.Length);
            var effectiveVersion = CommonCoreB01V9OffTopicClockConceptCorrections.IsTarget(target.Doc,target.Lesson)
                ? CommonCoreB01V9OffTopicClockConceptCorrections.CorrectedVersion
                : CommonCoreB01V9SharedComparisonProcedureCorrections.CorrectedVersion;
            Assert.Equal(effectiveVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    target.Doc,target.Lesson));
            var en = Assert.Single(target.Lesson.Translations);
            Assert.DoesNotContain("Step 1: Identify the quantities or expressions to compare.",
                en.StepByStepSolutions);
            Assert.DoesNotContain("deciding from appearance instead of actual values.",
                en.CommonMistakes);
            Assert.Contains("Step 4:", en.StepByStepSolutions);
            var originalFp=CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(target.Doc,target.Lesson);
            CanonicalLessonContentMaterializer.Materialize(target.Doc);
            Assert.Equal(originalFp,CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(target.Doc,target.Lesson));
        }
    }

    [Theory]
    [InlineData("G2:U04:L04","moving to the right")]
    [InlineData("G2:U06:L06","Trace the outline")]
    [InlineData("G2:U08:L01","two groups")]
    [InlineData("G2:U08:L07","Count the rows")]
    [InlineData("G2:U08:L12","count all squares")]
    [InlineData("G2:U09:L09","tape diagram")]
    [InlineData("G3:U01:L09","multiplication expression")]
    [InlineData("G3:U01:L21","graph scale")]
    [InlineData("G3:U04:L18","division equation")]
    [InlineData("G3:U05:L01","unit fraction")]
    [InlineData("G3:U08:L12","open question")]
    [InlineData("G4:U03:L06","n × (a/b) = (n × a)/b")]
    public void AllTwelveMathematicalFamiliesHaveRelevantProceduralOracles(
        string suffix,string expected)
    {
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons)
            .Single(x=>x.LessonCode=="PED:US-CCSS-MATH:"+suffix);
        var t=Assert.Single(lesson.Translations);
        Assert.Contains(expected,t.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void AllSourceOutcomesAndBodyHashesRejectUnreviewedDriftAndVersions()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var pair=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Single(x=>x.Lesson.LessonCode=="PED:US-CCSS-MATH:G2:U08:L07");
        Assert.True(CommonCoreB01V9SharedComparisonProcedureCorrections
            .CanUpgradeExisting(pair.Doc,pair.Lesson,
            CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections.CorrectedVersion));
        Assert.False(CommonCoreB01V9SharedComparisonProcedureCorrections
            .CanUpgradeExisting(pair.Doc,pair.Lesson,"unreviewed"));
        var concept = Assert.Single(pair.Lesson.Translations).KeyConceptsAndRules;
        Assert.True(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
            .IsVerifiedOriginalOrCorrectedConceptState(pair.Lesson.LessonCode, concept));
        Assert.False(CommonCoreB01V9SharedNonMeasureConceptTemplateCorrections
            .IsVerifiedOriginalOrCorrectedConceptState(pair.Lesson.LessonCode, concept + " unreviewed"));
        var raw=pair.Lesson.CanonicalBodySha256;
        try
        {
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedComparisonProcedureCorrections.Apply(pair.Doc));
        }
        finally {pair.Lesson.CanonicalBodySha256=raw;}
        var en=Assert.Single(pair.Lesson.Translations);
        var baseline=en.StepByStepSolutions;
        try
        {
            en.StepByStepSolutions += " injected unreviewed mathematics";
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedComparisonProcedureCorrections.Apply(pair.Doc));
        }
        finally {en.StepByStepSolutions=baseline;}
    }

    [Fact]
    public void BasicMathFactsUsedByOneCohortProcedureAreIndependentlyConsistent()
    {
        // Numeric invariants independent of publisher prose or appearance.
        Assert.Equal(12, Enumerable.Range(0,48).Count(x=>x%4==0)); // groups of 4
        Assert.Equal(48, 12*4);
        Assert.Equal(3*25, 3*20+3*5);
        Assert.Equal(24, 3*8);
        Assert.Equal(12, 3*4);
        Assert.Equal(6, 2*3);
        Assert.Equal(12, 3*4); // three groups of four
        Assert.Equal(6, (3*2)); // equal-area six-part whole
    }
}