using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authoring-only original Unit 4 L22 source math/teacher game rules; CI deferred.</summary>
public sealed class UsCcssG1Lesson87MysteryNumberSourceTests
{
    private const string Family="usccss.g1.u04.l22.center_place_value";

    [Fact]
    public void OriginalTrueFalseTensWarmupPreservesExactTruthValues()
    {
        foreach(var (mode,a,b,correct,wrong) in new[]{
            (4,92,92,"TRUE=YES;LEFT=92;RIGHT=92","TRUE=NO;LEFT=92;RIGHT=92"),
            (5,92,92,"TRUE=NO;LEFT=92;RIGHT=92","TRUE=YES;LEFT=92;RIGHT=92"),
            (6,33,43,"TRUE=YES;LEFT=33;RIGHT=43","TRUE=NO;LEFT=33;RIGHT=43")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
            Assert.Equal(correct,UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,correct));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,wrong));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,correct+";EXTRA=1"));
            p["a"]++;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        }
    }

    [Fact]
    public void StudentTeachingCapturesAllThreeCenterChoicesAndScoringConditions()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L22"));
        CanonicalLessonContentMaterializer.Materialize(doc);
        var row=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U04:L22");
        Assert.Equal(CommonCoreG1FullGradeUnit5ContentCorrections.Unit4Lesson22SourceVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
        Assert.True(CommonCoreG1FullGradeUnit5ContentCorrections.CanUpgradeExisting(
            doc,row,"us-ccss-g1-u04-l22-exact-three-comparisons-v3"));
        var body=row.Translations.Single(x=>x.CultureCode=="en");
        foreach(var phrase in new[]{"two number cards","five rounds","lowest score wins",
            "Mystery Number, Get Your Numbers in Order and Greatest of Them All"})
            Assert.Contains(phrase,body.Explanation+" "+body.StepByStepSolutions);
    }
}
