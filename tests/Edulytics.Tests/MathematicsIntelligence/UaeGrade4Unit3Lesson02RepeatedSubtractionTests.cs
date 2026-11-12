using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson02RepeatedSubtractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:02";
    [Fact]
    public void RepeatedSubtractionFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["divisor"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void DivisionAndReverseMultiplicationUseSameFactors()
    {
        var p=new Dictionary<string,int>{{"divisor",4},{"quotient",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l02.count_subtraction_jumps",p,"٣"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l02.equal_groups_quotient",p,"3"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l02.reconstruct_dividend",p,"١٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l02.count_subtraction_jumps",p,"4"));
    }
    [Fact]
    public void ArabicLessonAndVisualShowRepeatedSubtraction()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("علاقة القسمة بالطرح",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.NumberLine,Assert.Single(l.Visuals).Kind);
    }
}
