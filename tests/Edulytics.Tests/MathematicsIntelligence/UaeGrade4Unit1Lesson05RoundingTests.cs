using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit1Lesson05RoundingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:01:05";

    [Fact]
    public void SixDifferentPlaceValueRoundingFamiliesAreLessonSelected()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(6,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],
                ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-999"));
                Assert.Single(q.Parameters);
                Assert.DoesNotContain("result",q.Parameters.Keys);
            }
        }
    }

    [Fact]
    public void RoundAtMidpointsAndCarryBoundaries()
    {
        var p=new Dictionary<string,int>{{"n",637482}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(
           "uae.g4.u01.l05.round_nearest_ten_thousand",p,"٦٤٠٬٠٠٠"));
        p["n"]=274350;
        Assert.True(ExactSkillContractQuestionEngine.Verify(
           "uae.g4.u01.l05.round_nearest_thousand",p,"274000"));
        p["n"]=999500;
        Assert.True(ExactSkillContractQuestionEngine.Verify(
           "uae.g4.u01.l05.round_nearest_thousand",p,"1000000"));
        p["n"]=9999999;
        Assert.True(ExactSkillContractQuestionEngine.Verify(
           "uae.g4.u01.l05.round_nearest_million",p,"10000000"));
        p["n"]=-1;
        Assert.False(ExactSkillContractQuestionEngine.Verify(
           "uae.g4.u01.l05.round_nearest_million",p,"0"));
    }

    [Fact]
    public void ArabicRichVisualAndWorkedExamplesAvailable()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("استخدام القيمة المكانية للتقريب",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.NumberLine,Assert.Single(l.Visuals).Kind);
    }
}
