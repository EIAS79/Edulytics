using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Grade 1 U04 L21 source math checks. Test authoring only; CI/D are deferred.
/// </summary>
public sealed class UsCcssG1Lesson86Original2021ComparisonsTests
{
    private const string Family="usccss.g1.u04.l21.compare_nonstandard_forms";

    [Fact]
    public void SourceSixPublicRepresentationComparisonsAndMentalWarmupAreExact()
    {
        foreach(var (mode,a,b,answer,wrong) in new[]{
            (7,38,28,"SYMBOL=>","SYMBOL=<"),
            (8,35,35,"SYMBOL==","SYMBOL=>"),
            (9,99,89,"SYMBOL=>","SYMBOL=<"),
            (10,10,6,"RESULT=16","RESULT=15"),
            (11,9,6,"RESULT=15","RESULT=16"),
            (12,10,7,"RESULT=17","RESULT=15"),
            (13,8,7,"RESULT=15","RESULT=17")
        })
        {
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
            Assert.Equal(answer,UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,p));
            Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,answer));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,wrong));
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Verify(Family,p,answer+";FAKE=1"));
            p["a"]=a+1;
            Assert.False(UsCcssG1FullGradeUnit4FinalQuestions.Valid(Family,p));
        }
    }

    [Fact]
    public void OriginalWarmupAndComparisonModesArePresentWithoutBreakingOlderModes()
    {
        var questions=Enumerable.Range(0,1600)
            .Select(i=>UsCcssG1FullGradeUnit4FinalQuestions.Build(
                Family,new Random(i*29+17))).ToArray();
        foreach(int mode in Enumerable.Range(0,14))
            Assert.Contains(questions,q=>q.Parameters["mode"]==mode);
        Assert.All(questions,q=>Assert.True(UsCcssG1FullGradeUnit4FinalQuestions.Verify(
            Family,q.Parameters,UsCcssG1FullGradeUnit4FinalQuestions.Solve(Family,q.Parameters))));
    }

    [Fact]
    public void LearnerExplanationContainsPublicSourceAndPredecessorUpgradeIsExplicit()
    {
        var doc=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Single(x=>x.Lessons.Any(l=>l.LessonCode=="PED:US-CCSS-MATH:G1:U04:L21"));
        var row=doc.Lessons.Single(x=>x.LessonCode=="PED:US-CCSS-MATH:G1:U04:L21");
        CanonicalLessonContentMaterializer.Materialize(doc);
        Assert.Equal(CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersVersion,
            CanonicalLessonContentMaterializer.GetEffectiveContentVersion(doc,row));
        Assert.True(CommonCoreG1SourceGroundedInstructionalCorrections.CanUpgradeExisting(
            doc,row,CommonCoreG1SourceGroundedInstructionalCorrections.CompareRepresentedNumbersVersionV1));
        var body=row.Translations.Single(x=>x.CultureCode=="en").WorkedExamples;
        foreach(var math in new[]{"10+6=16","9+6=15","10+7=17","8+7=15",
            "38>28","32=32","35<40","52>42","35=35","99>89"})
            Assert.Contains(math,body);
    }
}
