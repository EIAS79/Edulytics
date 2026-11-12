using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson05PropertiesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:05";
    [Fact]
    public void SixArithmeticPropertyFamiliesGenerateAndVerify()
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
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-999"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var altered=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                altered["a"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,altered,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void ArabicDigitsAndZeroDivisionRulesAreNotConfused()
    {
        var p=new Dictionary<string,int>{{"a",7},{"b",4}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.commutative_missing_factor",p,"٤"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.identity",p,"7"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.zero",p,"٠"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.divide_by_one",p,"٧"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.divide_by_self",p,"١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.zero_dividend",p,"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l05.zero",p,"7"));
    }
    [Fact]
    public void ArabicPropertiesAndDivisionRulesArePresent()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Equal("خصائص الضرب وقواعد القسمة",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(rich.Visuals).Kind);
    }
}
