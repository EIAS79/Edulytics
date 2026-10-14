using System.Text.Json;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Source-bound V9 subtraction template root: all 55 original goals reviewed,
/// 16 exact purpose-mismatch corrections, remaining 39 not auto-overridden.
/// Not final L1-L6/eight-dimension certification.
/// </summary>
public sealed class UsCcssB01V9SharedSubtractionProcedureCorrectionsTests
{
    private const string Prefix="PED:US-CCSS-MATH:";
    private static string Root()
    {
        DirectoryInfo? dir=new(Directory.GetCurrentDirectory());
        while(dir is not null)
        {
            if(File.Exists(Path.Combine(dir.FullName,
                "artifacts/academic-us-ccss/batches/B01-v9-shared-subtraction-procedure-16-source-bound-repair.json")))
                return dir.FullName;
            dir=dir.Parent;
        }
        throw new FileNotFoundException("V9 shared subtraction source review missing");
    }

    [Fact]
    public void All55OriginalTeachingPurposesAllowExactly16TargetedAnd39UnmodifiedLessons()
    {
        using var review=JsonDocument.Parse(File.ReadAllText(Path.Combine(Root(),
            "artifacts/academic-us-ccss/batches/B01-v9-subtraction-55-source-goal-activity-purpose-reviewed.json")));
        using var correction=JsonDocument.Parse(File.ReadAllText(Path.Combine(Root(),
            "artifacts/academic-us-ccss/batches/B01-v9-shared-subtraction-procedure-16-source-bound-repair.json")));
        var all=review.RootElement.GetProperty("sourceReviewedRows").EnumerateArray().ToArray();
        var targets=correction.RootElement.GetProperty("rows").EnumerateArray().ToArray();
        Assert.Equal(55,all.Length);
        Assert.Equal(16,targets.Length);
        Assert.Equal(16,CommonCoreB01V9SharedSubtractionProcedureCorrections.ExpectedCount);
        Assert.Equal(16,CommonCoreB01V9SharedSubtractionProcedureCorrections.StrategyCount);
        Assert.Equal("BATCH_NO-GO",correction.RootElement.GetProperty("academicDecision").GetString());

        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var byCode=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .ToDictionary(x=>x.Lesson.LessonCode,StringComparer.Ordinal);
        Assert.All(all,row=>
        {
            var code=row.GetProperty("lessonCode").GetString()!;
            var match=targets.Any(x=>x.GetProperty("lessonCode").GetString()==code);
            var item=byCode[code];
            Assert.Equal(match,CommonCoreB01V9SharedSubtractionProcedureCorrections.IsTarget(item.Doc,item.Lesson));
            var t=Assert.Single(item.Lesson.Translations);
            if(match)
            {
                Assert.DoesNotContain("Step 1: Identify the whole and the part being removed or compared.",t.StepByStepSolutions);
                Assert.Contains("Step 4:",t.StepByStepSolutions);
                Assert.StartsWith("Mistake:",t.CommonMistakes);
                Assert.Equal(CommonCoreB01V9SharedSubtractionProcedureCorrections.CorrectedVersion,
                    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.Doc,item.Lesson));
            }
        });
        foreach(var target in targets)
        {
            var code=target.GetProperty("lessonCode").GetString()!;
            var row=Assert.Single(all,x=>x.GetProperty("lessonCode").GetString()==code);
            var lesson=byCode[code].Lesson;
            Assert.Equal(lesson.SourceUrl,target.GetProperty("originalTeacherSourceUrl").GetString());
            Assert.Equal(lesson.CanonicalBodySha256,target.GetProperty("learnerRawBodySha256").GetString());
            Assert.Equal(row.GetProperty("sourceSemanticSha256").GetString(),
                target.GetProperty("sourceSemanticSha256").GetString());
            Assert.True(target.GetProperty("sourceGoalAndActivityPurposeChecked").GetBoolean());
            Assert.True(target.GetProperty("notFullL1toL6").GetBoolean());
        }
    }

    [Theory]
    [InlineData("G2:U01:L14","tape diagram")]
    [InlineData("G2:U03:L18","yardstick")]
    [InlineData("G2:U04:L06","number line")]
    [InlineData("G2:U04:L07","rightward")]
    [InlineData("G2:U04:L09","count backward")]
    [InlineData("G2:U04:L10","base-ten")]
    [InlineData("G2:U04:L11","exchange")]
    [InlineData("G2:U05:L07","three-digit")]
    [InlineData("G2:U06:L17","100 cents")]
    [InlineData("G2:U06:L18","prices")]
    [InlineData("G3:U08:L15","Number Talk")]
    [InlineData("G4:U03:L09","fraction")]
    [InlineData("G4:U03:L11","decompose")]
    [InlineData("G4:U04:L18","column")]
    [InlineData("G4:U04:L20","standard algorithm")]
    public void MathsInstructionMatchesOriginalTeacherGoal(string suffix,string concept)
    {
        var t=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons).Single(x=>x.LessonCode==Prefix+suffix)
            .Translations.Single();
        Assert.Contains(concept,t.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RejectUnverifiedSourceBodyAndPartialProcedureChanges()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d=>d.Lessons.Any(x=>x.LessonCode==Prefix+"G4:U03:L09"));
        var l=doc.Lessons.Single(x=>x.LessonCode==Prefix+"G4:U03:L09");
        Assert.False(CommonCoreB01V9SharedSubtractionProcedureCorrections.CanUpgradeExisting(
            doc,l,"unapproved-version"));
        var before=l.CanonicalBodySha256;
        try
        {
            l.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedSubtractionProcedureCorrections.Apply(doc));
        }
        finally {l.CanonicalBodySha256=before;}
        var steps=l.Translations.Single().StepByStepSolutions;
        try
        {
            l.Translations.Single().StepByStepSolutions+=" MALFORMED";
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedSubtractionProcedureCorrections.Apply(doc));
        }
        finally {l.Translations.Single().StepByStepSolutions=steps;}
    }

    [Fact]
    public void OneSharedRootCanBeReappliedWithoutChangingAnyOtherLesson()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        foreach(var doc in docs)
        {
            var fingerprints=doc.Lessons.Select(x=>CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc,x)).ToArray();
            CommonCoreB01V9SharedSubtractionProcedureCorrections.Apply(doc);
            Assert.Equal(fingerprints,doc.Lessons.Select(x=>CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc,x)).ToArray());
        }
    }
}
