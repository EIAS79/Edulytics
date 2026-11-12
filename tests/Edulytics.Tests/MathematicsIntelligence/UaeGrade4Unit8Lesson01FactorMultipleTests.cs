using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson01FactorMultipleTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:01";
    [Fact]
    public void FactorMultipleFamiliesGenerateVerifyAndRejectIncorrectAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
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
    [InlineData(4,6,24)]
    [InlineData(3,12,36)]
    [InlineData(6,8,48)]
    public void FactorPairsProduceExpectedProduct(int a,int b,int number)
        =>Assert.Equal(number,a*b);
    [Theory]
    [InlineData(72,8,true)]
    [InlineData(74,6,false)]
    [InlineData(64,4,true)]
    [InlineData(86,4,false)]
    public void MultipleRequiresZeroDivisionRemainder(int number,int factor,bool expected)
        =>Assert.Equal(expected,number%factor==0);
    [Fact]
    public void ArabicRichCoverageContainsWorkedArraysAndMultiples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("العوامل والمضاعفات",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.True(l.CommonMistakes.Count>=2);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
