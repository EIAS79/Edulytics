using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit2Lesson04EstimateTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:02:04";
    [Fact]
    public void SixEstimateFamiliesSelectedAndIndependentlyVerified()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(6,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=5;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                bad["a"]=9999999;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void HundredAndThousandRoundingUseBothOperands()
    {
        var p=new Dictionary<string,int>{{"a",55481},{"b",2326}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l04.sum_nearest_hundred",p,"٥٧٬٨٠٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l04.sum_nearest_hundred",p,"57807"));
        p["a"]=75542;p["b"]=3225;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l04.diff_nearest_hundred",p,"72300"));
        p["a"]=55599;p["b"]=12782;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l04.diff_nearest_thousand",p,"43000"));
    }
    [Fact]
    public void OriginalArabicEstimateLessonIncludesWorkedVisual()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("تقدير المجاميع والفروق",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.CalculationChain,Assert.Single(l.Visuals).Kind);
    }
}
