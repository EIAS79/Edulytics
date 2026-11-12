using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson01FactFamiliesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:01";
    [Fact]
    public void FourMultiplicationDivisionFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
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
                invalid["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,invalid,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IndependentMultiplicationDivisionFactsAndArabicDigits()
    {
        var p=new Dictionary<string,int>{{"a",3},{"b",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l01.array_product",p,"١٢"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l01.array_quotient",p,"4"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l01.unknown_factor",p,"٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l01.inverse_fact",p,"4"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l01.array_quotient",p,"3"));
    }
    [Fact]
    public void StudentArabicFactFamilyAndEquationsArePresent()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("العلاقة بين الضرب والقسمة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
