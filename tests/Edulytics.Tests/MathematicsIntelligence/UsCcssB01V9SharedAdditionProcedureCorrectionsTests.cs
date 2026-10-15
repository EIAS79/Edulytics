using System.Text.Json;
using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// One reviewed 25-source addition-template cohort: 21 substantive
/// mathematical-purpose corrections, 3 unchanged control lessons and 1 previously-corrected control.
/// The source review is not a complete L1-L6 academic certification.
/// </summary>
public sealed class UsCcssB01V9SharedAdditionProcedureCorrectionsTests
{
    private const string Prefix="PED:US-CCSS-MATH:";
    private static string Root()
    {
        DirectoryInfo? d=new(Directory.GetCurrentDirectory());
        while(d is not null)
        {
            if(File.Exists(Path.Combine(d.FullName,
                "artifacts/academic-us-ccss/batches/B01-v9-addition-template-25-source-goal-activity-reviewed.json")))
                return d.FullName;
            d=d.Parent;
        }
        throw new FileNotFoundException("V9 addition source cohort evidence unavailable");
    }

    [Fact]
    public void All25OfficialSourceGoalsAndActivitiesBind22CorrectionsAndThreeControls()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var byCode=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .ToDictionary(x=>x.Lesson.LessonCode,StringComparer.Ordinal);
        using var json=JsonDocument.Parse(File.ReadAllText(Path.Combine(Root(),
            "artifacts/academic-us-ccss/batches/B01-v9-addition-template-25-source-goal-activity-reviewed.json")));
        var cohort=json.RootElement.GetProperty("cohortRows").EnumerateArray().ToArray();
        var newlyVerified=json.RootElement.GetProperty("rows").EnumerateArray().ToArray();
        Assert.Equal(25,cohort.Length);
        Assert.Equal(21,newlyVerified.Length);
        Assert.Equal(21,cohort.Count(r=>r.GetProperty("needsSharedRootCorrection").GetBoolean()));
        Assert.Equal(4,cohort.Count(r=>r.GetProperty("alreadyCountedInPrevious113").GetBoolean()));
        Assert.Equal(21,CommonCoreB01V9SharedAdditionProcedureCorrections.ExpectedCount);
        Assert.Equal(21,CommonCoreB01V9SharedAdditionProcedureCorrections.MathFamilyCount);
        Assert.Equal(25,cohort.Select(r=>r.GetProperty("lessonCode").GetString()).Distinct().Count());
        foreach(var row in cohort)
        {
            var code=row.GetProperty("lessonCode").GetString()!;
            var item=byCode[code];
            var correction=row.GetProperty("needsSharedRootCorrection").GetBoolean();
            Assert.Equal(correction,CommonCoreB01V9SharedAdditionProcedureCorrections.IsTarget(
                item.Doc,item.Lesson));
            Assert.Equal(item.Lesson.SourceUrl,row.GetProperty("originalTeacherSourceUrl").GetString());
            Assert.Equal(item.Lesson.CanonicalBodySha256,row.GetProperty("canonicalOriginalBodySha256").GetString());
            Assert.True(row.GetProperty("sourceGoalReviewed").GetBoolean());
            Assert.True(row.GetProperty("originalTeacherActivityFetched").GetBoolean());
            Assert.False(row.GetProperty("academicCertifiedByThisReview").GetBoolean());
            var t=Assert.Single(item.Lesson.Translations);
            if(correction)
            {
                Assert.DoesNotContain("Identify the quantities being combined",t.StepByStepSolutions);
                Assert.Contains("Step 4:",t.StepByStepSolutions);
                Assert.StartsWith("Mistake:",t.CommonMistakes);
                var effectiveVersion = CommonCoreB01V9OffTopicClockConceptCorrections.IsTarget(item.Doc,item.Lesson)
                    ? CommonCoreB01V9OffTopicClockConceptCorrections.CorrectedVersion
                    : CommonCoreB01V9SharedAdditionProcedureCorrections.CorrectedVersion;
                Assert.Equal(effectiveVersion,
                    CanonicalLessonContentMaterializer.GetEffectiveContentVersion(item.Doc,item.Lesson));
            }
            else
            {
                if(code==Prefix+"G3:U07:L02")
                    Assert.DoesNotContain("Identify the quantities being combined",t.StepByStepSolutions);
                else
                    Assert.StartsWith("Step 1: Identify the quantities being combined",t.StepByStepSolutions);
            }
        }
        foreach(var row in newlyVerified)
        {
            var code=row.GetProperty("lessonCode").GetString()!;
            Assert.Contains(cohort,r=>r.GetProperty("lessonCode").GetString()==code &&
                !r.GetProperty("alreadyCountedInPrevious113").GetBoolean());
        }
    }

    [Theory]
    [InlineData("G2:U06:L15","nickel")]
    [InlineData("G2:U06:L16","quarter")]
    [InlineData("G2:U06:L21","pattern")]
    [InlineData("G2:U07:L09","hundred")]
    [InlineData("G2:U08:L04","equal addends")]
    [InlineData("G2:U08:L09","array")]
    [InlineData("G3:U01:L15","multiplication")]
    [InlineData("G3:U03:L05","algorithms")]
    [InlineData("G3:U03:L20","second operation")]
    [InlineData("G3:U03:L21","round")]
    [InlineData("G4:U01:L05","multiples")]
    [InlineData("G4:U03:L07","denominator")]
    [InlineData("G4:U03:L08","number line")]
    [InlineData("G4:U03:L16","hundredths")]
    [InlineData("G4:U03:L17","whole")]
    [InlineData("G4:U03:L18","associative")]
    public void CorrectedStepsTargetActualOriginalTeacherMathPurpose(string suffix,string term)
    {
        var t=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons)
            .Single(l=>l.LessonCode==Prefix+suffix).Translations.Single();
        Assert.Contains(term,t.StepByStepSolutions,StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void SourceTamperingAndPartialProcedureDriftFailClosed()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(d=>d.Lessons.Any(l=>l.LessonCode==Prefix+"G4:U03:L17"));
        var lesson=doc.Lessons.Single(l=>l.LessonCode==Prefix+"G4:U03:L17");
        Assert.False(CommonCoreB01V9SharedAdditionProcedureCorrections.CanUpgradeExisting(
            doc,lesson,"unapproved-unknown-version"));
        var sha=lesson.CanonicalBodySha256;
        try
        {
            lesson.CanonicalBodySha256=new string('a',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedAdditionProcedureCorrections.Apply(doc));
        }
        finally { lesson.CanonicalBodySha256=sha; }
        var t=lesson.Translations.Single();
        var steps=t.StepByStepSolutions;
        try
        {
            t.StepByStepSolutions+=" UNVERIFIED_INSTRUCTION";
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9SharedAdditionProcedureCorrections.Apply(doc));
        }
        finally { t.StepByStepSolutions=steps; }
        CommonCoreB01V9SharedAdditionProcedureCorrections.Apply(doc);
    }

    [Fact]
    public void AdditionRootIsIdempotentAndNeverTouchesUnrelatedPacks()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        foreach(var doc in docs)
        {
            var before=doc.Lessons.Select(l=>CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc,l)).ToArray();
            CommonCoreB01V9SharedAdditionProcedureCorrections.Apply(doc);
            Assert.Equal(before,doc.Lessons.Select(l=>CanonicalLessonContentMaterializer
                .ComputeLessonFingerprint(doc,l)).ToArray());
        }
    }
}
