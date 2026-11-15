using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U2L10PrecisePercentTests
{
    private const string L="PED:UAE-MOE-MATH:L6:GENERAL:02:10:STUDENT-AR-128";
    private const string F="percentages.core.of_quantity";
    [Fact] public void ArabicSourceHasPrecisionModelsAndReusedFamily()
    {
        var rich=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=7);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.NotNull(c);
        Assert.Contains(F,c!.AllowedQuestionFamilies);
    }
    [Theory]
    [InlineData(14500,320,"464")]
    [InlineData(22000,65,"143")]
    [InlineData(25,58,"0.145")]
    [InlineData(3,1500,"0.45")]
    [InlineData(46,80,"0.368")]
    public void PrecisionVerifierChecksMoreThanHundredAndLessThanOne(int scaled,int amount,string expected)
    {
        var p=new Dictionary<string,int>{{"scaledPercentHundredths",scaled},{"quantity",amount}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"-1"));
    }
    [Fact] public void ScopedGeneratorRetainsIndependentExactVerifier()
    {
        var g=new ExactSkillContractQuestionEngine();
        foreach(var q in g.Generate(L,F,[F],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
            Assert.True(ExactSkillContractQuestionEngine.Verify(F,q.Parameters,q.CorrectAnswer));
    }
}
