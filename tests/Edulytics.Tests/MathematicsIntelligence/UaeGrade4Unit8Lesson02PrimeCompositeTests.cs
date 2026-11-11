using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson02PrimeCompositeTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:02";

    [Fact]
    public void PrimeCompositeFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,5,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(0,"2")]
    [InlineData(1,"2")]
    [InlineData(13,"1")]
    [InlineData(18,"0")]
    public void PrimeCompositeAndNeitherUseCorrectClassification(int number,string expected)
    {
        var parameters=new Dictionary<string,int>{{"n",number}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l02.prime_composite_classification",parameters,expected));
    }
    [Theory]
    [InlineData(18,"6")]
    [InlineData(13,"2")]
    public void FactorCountMatchesPositiveDivisors(int number,string expected)
    {
        var parameters=new Dictionary<string,int>{{"n",number}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l02.positive_factor_count",parameters,expected));
    }
    [Fact]
    public void ArabicRichSourceContentIsRegistered()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأعداد الأولية وغير الأولية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
