using Edulytics.Data.Seeding;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>V9 single cohort-level learner-facing voice repair; no certification.</summary>
public sealed class UsCcssB01V9LearnerVoiceSharedRootCorrectionTests
{
    private const string Prefix = "PED:US-CCSS-MATH:G1:U01:";
    [Fact]
    public void BothSourceReviewedLessonsHaveConsistentLearnerVoiceAndPreserveMathematics()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var cohort=docs.SelectMany(d=>d.Lessons.Where(l=>
            CommonCoreB01V9LearnerVoiceSharedRootCorrection.IsTarget(d,l))
            .Select(l=>(Document:d,Lesson:l))).ToArray();
        Assert.Equal(2,cohort.Length);
        Assert.Equal(new[]{Prefix+"L04",Prefix+"L05"},
            cohort.Select(x=>x.Lesson.LessonCode).OrderBy(s=>s,StringComparer.Ordinal).ToArray());
        foreach(var (d,l) in cohort)
        {
            var expectedVersion=l.LessonCode==Prefix+"L05"
                ? CommonCoreT001MixedAddSubtractContentCorrection.CorrectedVersion
                : CommonCoreB01V9LearnerVoiceSharedRootCorrection.CorrectedVersion;
            Assert.Equal(expectedVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(d,l));
            var t=Assert.Single(l.Translations);
            Assert.DoesNotContain("support them in adding",t.Explanation);
            Assert.DoesNotContain("they are observing",t.Explanation);
            Assert.DoesNotContain("as they learn a new stage",t.Explanation);
            Assert.Contains("you",t.Explanation,StringComparison.OrdinalIgnoreCase);
            var fp=CanonicalLessonContentMaterializer.ComputeLessonFingerprint(d,l);
            CanonicalLessonContentMaterializer.Materialize(d);
            Assert.Equal(fp,CanonicalLessonContentMaterializer.ComputeLessonFingerprint(d,l));
        }
        var subtraction=Assert.Single(cohort.Single(x=>x.Lesson.LessonCode==Prefix+"L04").Lesson.Translations);
        Assert.Contains("subtract 1 or 2",subtraction.Explanation);
        Assert.Contains("6 - 2 = 4",subtraction.WorkedExamples);
        var mixed=Assert.Single(cohort.Single(x=>x.Lesson.LessonCode==Prefix+"L05").Lesson.Translations);
        Assert.Contains("add and subtract within 10",mixed.Explanation);
        Assert.Contains("3 + 2 = 5",mixed.WorkedExamples);
        Assert.Contains("6 - 2 = 4",mixed.WorkedExamples);
    }
    [Fact]
    public void StaleSourceOrCorruptProseCannotBeRewrittenOrPromoted()
    {
        var docs=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments();
        var pair=docs.SelectMany(d=>d.Lessons.Select(l=>(Doc:d,Lesson:l)))
            .Single(x=>x.Lesson.LessonCode==Prefix+"L05");
        var original=pair.Lesson.CanonicalBodySha256;
        try{
            pair.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9LearnerVoiceSharedRootCorrection.Apply(pair.Doc));
        }
        finally{pair.Lesson.CanonicalBodySha256=original;}
        var t=Assert.Single(pair.Lesson.Translations);
        var baseline=t.Explanation;
        try{
            t.Explanation += " UNREVIEWED";
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreB01V9LearnerVoiceSharedRootCorrection.Apply(pair.Doc));
        }
        finally{t.Explanation=baseline;}
        Assert.False(CommonCoreB01V9LearnerVoiceSharedRootCorrection.CanUpgradeExisting(
            pair.Doc,pair.Lesson,"arbitrary-unverified-version"));
    }
}