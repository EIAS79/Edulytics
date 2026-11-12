using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson09FractionMultiplicationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:09";
    [Fact]
    public void MultiplicationFamiliesGenerateVerifyAndRejectWrongAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
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
    [InlineData(3,2,4,"8","2","2")]
    [InlineData(4,3,3,"9","2","1")]
    [InlineData(8,1,5,"5","0","5")]
    [InlineData(5,2,6,"12","2","2")]
    public void MultiplicationPreservesFractionDenominator(int denominator,int numerator,int times,string product,string whole,string remainder)
    {
        var p=new Dictionary<string,int>{{"denominator",denominator},{"numerator",numerator},{"repeatCount",times}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_numerator",p,product));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_whole",p,whole));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_remainder",p,remainder));
        Assert.Equal(int.Parse(product),int.Parse(whole)*denominator+int.Parse(remainder));
    }
    [Fact]
    public void CannotUseZeroDenominator()
    {
        var p=new Dictionary<string,int>{{"denominator",0},{"numerator",2},{"repeatCount",3}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l09.product_numerator",p,"6"));
    }
    [Fact]
    public void ArabicRichLessonContainsEstimationAndConversion()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("ضرب الكسور في الأعداد الكلية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
