using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson08FractionMultiplicationModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:08";
    [Fact]
    public void RepeatedAdditionFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(5,2,3,"6")]
    [InlineData(6,1,4,"4")]
    [InlineData(8,3,2,"6")]
    [InlineData(4,2,4,"8")]
    public void FractionModelCountsUnitFractions(int denominator,int numerator,int times,string expected)
    {
        var p=new Dictionary<string,int>{
            ["denominator"]=denominator,["numerator"]=numerator,["repeatCount"]=times
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l08.model_repeated_sum",p,expected));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l08.model_unit_fraction_count",p,expected));
    }
    [Fact]
    public void InvalidDenominatorRejectsTheProblem()
    {
        var p=new Dictionary<string,int>{{"denominator",1},{"numerator",1},{"repeatCount",3}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l08.model_repeated_sum",p,"3"));
    }
    [Fact]
    public void RichLessonIncludesRepeatedAdditionAndFractionStrips()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: تمثيل الكسور وعمليات الضرب",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
