using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson07FactorsMultiplesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:07";
    [Fact]
    public void ThreeFamilyKindsGenerateAndValidateIndependentAnswers()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-20"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var wrong=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                wrong["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,wrong,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IsFactorIncludesPositiveAndNegativeCases()
    {
        var p=new Dictionary<string,int>{{"a",7},{"b",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l07.is_factor",p,"١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l07.nth_multiple",p,"28"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l07.factor_pair",p,"٤"));
        p["b"]=5;
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l07.is_factor",p,"٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l07.is_factor",p,"1"));
    }
    [Fact]
    public void ArabicBookContentAndSourceExamplesExist()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("العوامل والمضاعفات",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
