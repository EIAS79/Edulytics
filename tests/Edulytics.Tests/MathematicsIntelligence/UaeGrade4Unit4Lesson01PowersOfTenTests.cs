using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit4Lesson01PowersOfTenTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:04:01";
    [Fact]
    public void ThreeSourceMatchedFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=4;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(3,q.Parameters.Count);
                var invalid=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                invalid["zeros"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void MultiplesAndUnknownFactorsUsePlaceValue()
    {
        var p=new Dictionary<string,int>{{"factor",8},{"baseDigit",2},{"zeros",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l01.multiply_power_of_ten",p,"١٦٬٠٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l01.unknown_factor_power_ten",p,"٢٠٠٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l01.compare_powers_ten",p,"١"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u04.l01.multiply_power_of_ten",p,"1600"));
    }
    [Fact]
    public void ArabicLessonContainsPowersOfTenWorkedExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Contains("١٠٠",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
