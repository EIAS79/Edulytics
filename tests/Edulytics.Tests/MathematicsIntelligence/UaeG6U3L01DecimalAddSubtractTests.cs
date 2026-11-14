using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L01DecimalAddSubtractTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:03:01:STUDENT-AR-128";
    [Fact] public void ArabicLessonReusesExistingDecimalFamilies()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=6);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
    }
    [Theory]
    [InlineData("uae.g4.u11.l06.column_sum",23100,5800,"28.9")]
    [InlineData("uae.g4.u11.l06.column_sum",22360,17030,"39.39")]
    [InlineData("uae.g4.u11.l10.column_difference",5774,2371,"3.403")]
    [InlineData("uae.g4.u11.l10.column_difference",6000,4780,"1.22")]
    public void ThousandthPrecisionUsesIndependentVerifier(string family,int a,int b,string expected)
    {
        var p=new Dictionary<string,int>{{"g6Thousandths",1},{"aThousandths",a},{"bThousandths",b}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(family,p,"999"));
    }
    [Fact] public void OriginalArabicProblemGenerationRemainsExact()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{"uae.g4.u11.l06.column_sum","uae.g4.u11.l10.column_difference"})
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
    }
}
