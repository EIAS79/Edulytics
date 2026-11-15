using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U2L05ExtremePercentTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:05:STUDENT-AR-128";
    private const string P="uae.g6.u02.l05.percent_extreme_to_decimal";
    private const string D="uae.g6.u02.l05.decimal_extreme_to_percent";
    [Fact] public void ArabicRichAndTwoNewFamiliesUseExistingEngine()
    {
        var lesson=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(lesson);
        Assert.True(lesson!.WorkedExamples.Count>=5);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
        }
    }
    [Theory]
    [InlineData(36,"0.00036")]
    [InlineData(200,"0.002")]
    [InlineData(170000,"1.7")]
    [InlineData(250000,"2.5")]
    public void ExtremePercentToDecimalIsExact(int scaled,string answer)
    {
        var p=new Dictionary<string,int>{{"scaledPercent",scaled}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(P,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(P,p,"-1"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(P,p,
            new string(answer.Select(c=>c is >= '0' and <= '9'?(char)('٠'+c-'0'):c).ToArray()).Replace('.','٫')));
    }
    [Theory]
    [InlineData(16,"0.16")]
    [InlineData(75,"0.75")]
    [InlineData(16800,"168")]
    [InlineData(21000,"210")]
    public void ExtremeDecimalToPercentIsExact(int scaled,string answer)
    {
        var p=new Dictionary<string,int>{{"scaledDecimal",scaled}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(D,p,answer));
        Assert.False(ExactSkillContractQuestionEngine.Verify(D,p,"-1"));
    }
    [Fact] public void ExistingGeneratorAndIndependentVerifierAgree()
    {
        var generator=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{P,D})
            foreach(var item in generator.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,item.Parameters,item.CorrectAnswer));
    }
}
