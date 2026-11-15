using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L02EstimatedDecimalProductsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:03:02:STUDENT-AR-128";
    private const string Family="uae.g5.u05.l05.multiply_decimal_factors";
    [Fact] public void ArabicLessonReusesExistingDecimalProductFamily()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=6);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Contains(Family,c!.AllowedQuestionFamilies);
    }
    [Theory]
    [InlineData(87,28,1,1,"27")]
    [InlineData(426,372,10,10,"1600")]
    [InlineData(693,65,10,1,"490")]
    public void SpecifiedRoundingRuleHasIndependentVerification(int a,int b,int pa,int pb,string expected)
    {
        var p=new Dictionary<string,int>{{"g6EstimateProduct",1},{"aTenths",a},{"bTenths",b},{"placeA",pa},{"placeB",pb}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,p,"-1"));
    }
    [Fact] public void GeneratedEstimationsRemainValid()
    {
        var g=new ExactSkillContractQuestionEngine();
        foreach(var q in g.Generate(Lesson,Family,[Family],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,q.Parameters,q.CorrectAnswer));
    }
}
