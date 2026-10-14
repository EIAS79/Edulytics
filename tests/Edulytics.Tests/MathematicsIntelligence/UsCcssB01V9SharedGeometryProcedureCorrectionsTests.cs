using System.Text.Json;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Complete original 31-teacher-source shared geometry/area/perimeter/volume
/// procedure cohort, one repair authority, 28 repaired and 3 negative controls.
/// NOT academic lesson certification.
/// </summary>
public sealed class UsCcssB01V9SharedGeometryProcedureCorrectionsTests
{
    private const string Prefix="PED:US-CCSS-MATH:";
    private static string Root()
    {
        DirectoryInfo? d=new(Directory.GetCurrentDirectory());
        while(d is not null)
        {
            if(File.Exists(Path.Combine(d.FullName,
                "artifacts/academic-us-ccss/batches/B01-v9-geometry-template-31-source-purpose-reviewed.json")))
                return d.FullName;
            d=d.Parent;
        }
        throw new FileNotFoundException("V9 whole-cohort teacher sources not available");
    }

    [Fact]
    public void All31SourceGoalsProveExact28SourceBoundMathProcedureRepairsWithThreeControls()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var byCode=docs.SelectMany(doc=>doc.Lessons.Select(lesson=>(Doc:doc,Lesson:lesson)))
            .ToDictionary(x=>x.Lesson.LessonCode,StringComparer.Ordinal);
        using var json=JsonDocument.Parse(File.ReadAllText(Path.Combine(Root(),
            "artifacts/academic-us-ccss/batches/B01-v9-geometry-template-31-source-purpose-reviewed.json")));
        var rows=json.RootElement.GetProperty("rows").EnumerateArray().ToArray();
        Assert.Equal(31,rows.Length);
        Assert.Equal(28,rows.Count(x=>x.GetProperty("needsSharedCauseCorrection").GetBoolean()));
        Assert.Equal(28,CommonCoreB01V9SharedGeometryProcedureCorrections.ExpectedCount);
        Assert.Equal(22,CommonCoreB01V9SharedGeometryProcedureCorrections.FamilyCount);
        foreach(var row in rows)
        {
            var code=row.GetProperty("lessonCode").GetString()!;
            var item=byCode[code];
            var target=row.GetProperty("needsSharedCauseCorrection").GetBoolean();
            Assert.Equal(target,CommonCoreB01V9SharedGeometryProcedureCorrections.IsTarget(
                item.Doc,item.Lesson));
            var steps=Assert.Single(item.Lesson.Translations).StepByStepSolutions;
            if(!target)continue;
            Assert.DoesNotContain("Step 1: Identify the figure and the attributes named in the task.",
                steps);
            Assert.DoesNotContain("classifying a figure by appearance, size, or orientation",
                item.Lesson.Translations.Single().CommonMistakes);
            Assert.Contains("Step 4:",steps);
            Assert.Equal(CommonCoreB01V9SharedGeometryProcedureCorrections.CorrectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.Doc,item.Lesson));
        }
        var control=byCode[Prefix+"G3:U07:L04"];
        Assert.StartsWith("Step 1: Identify the figure and the attributes named in the task.",
            control.Lesson.Translations.Single().StepByStepSolutions);
        foreach(var code in new[]{"G2:U06:L08","G4:U01:L01"})
        {
            Assert.False(CommonCoreB01V9SharedGeometryProcedureCorrections.IsTarget(
                byCode[Prefix+code].Doc,byCode[Prefix+code].Lesson));
        }
    }

    [Theory]
    [InlineData("G2:U06:L03","ruler")]
    [InlineData("G2:U08:L11","grid")]
    [InlineData("G3:U02:L02","unit squares")]
    [InlineData("G3:U02:L09","length times width")]
    [InlineData("G3:U02:L12","rectangle")]
    [InlineData("G3:U04:L11","distributive")]
    [InlineData("G3:U06:L07","liter")]
    [InlineData("G3:U06:L08","liter")]
    [InlineData("G3:U07:L06","perimeter")]
    [InlineData("G3:U07:L09","unknown")]
    [InlineData("G3:U07:L11","perimeter")]
    [InlineData("G3:U07:L12","factor pairs")]
    [InlineData("G4:U01:L02","factor")]
    [InlineData("G4:U01:L08","grid")]
    public void SourceGroundedMathProceduresDescribeActualGoal(string suffix,string concept)
    {
        var lesson=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons)
            .Single(x=>x.LessonCode==Prefix+suffix);
        Assert.Contains(concept,lesson.Translations.Single().StepByStepSolutions,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void NoUnreviewedSourceOrPartialInstructionCanBeAutoUpgraded()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d=>d.Lessons.Any(l=>l.LessonCode==Prefix+"G3:U06:L08"));
        var lesson=doc.Lessons.Single(l=>l.LessonCode==Prefix+"G3:U06:L08");
        Assert.False(CommonCoreB01V9SharedGeometryProcedureCorrections.CanUpgradeExisting(
            doc,lesson,"unapproved-source-version"));
        var originalSha=lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedGeometryProcedureCorrections.Apply(doc));
        }
        finally{lesson.CanonicalBodySha256=originalSha;}
        var t=Assert.Single(lesson.Translations);
        var steps=t.StepByStepSolutions;
        try
        {
            t.StepByStepSolutions+=" unreviewed extra";
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedGeometryProcedureCorrections.Apply(doc));
        }
        finally{t.StepByStepSolutions=steps;}
        CommonCoreB01V9SharedGeometryProcedureCorrections.Apply(doc);
    }

    [Fact]
    public void SharedGeometryCauseIsIdempotentAcrossAllSourceReviewedTargets()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        foreach(var doc in docs)
        {
            var before=doc.Lessons.Select(x=>CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc,x)).ToArray();
            CanonicalLessonContentMaterializer.Materialize(doc);
            Assert.Equal(before,doc.Lessons.Select(x=>CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc,x)).ToArray());
        }
    }
}