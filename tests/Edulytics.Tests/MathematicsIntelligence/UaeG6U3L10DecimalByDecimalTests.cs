using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L10DecimalByDecimalTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:10:STUDENT-AR-128";
    const string F="uae.g5.u05.l13.exact_decimal_divide";
    [Fact] public void ArabicLessonAndExistingDecimalFamiliesAreRegistered()
    {
        var lesson=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(lesson);
        Assert.True(lesson!.WorkedExamples.Count>=10);
        Assert.Contains(lesson.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
        }
    }
    [Theory]
    [InlineData(36000000,9000000,"4")]
    [InlineData(36000000,900000,"40")]
    [InlineData(36000000,90000,"400")]
    [InlineData(36000000,9000,"4000")]
    [InlineData(36000000,900,"40000")]
    [InlineData(3600000,900000,"4")]
    [InlineData(360000,90000,"4")]
    [InlineData(36000,9000,"4")]
    [InlineData(3600,900,"4")]
    [InlineData(1710000,900000,"1.9")]
    [InlineData(2640000,600000,"4.4")]
    [InlineData(52000000,400000,"130")]
    [InlineData(9000,180000,"0.05")]
    [InlineData(11200000,70000,"160")]
    [InlineData(5600000,14000,"400")]
    [InlineData(6240000,200000000,"0.0312")]
    [InlineData(400000,25000000,"0.016")]
    [InlineData(680000,3400000,"0.2")]
    [InlineData(3690000,300000,"12.3")]
    [InlineData(33800,1300000,"0.026")]
    [InlineData(167280,3400000,"0.0492")]
    [InlineData(6300,7000,"0.9")]
    public void ExactDecimalDivisionPersistsSixPlacePrecision(int n,int d,string expected)
    {
        var p=new Dictionary<string,int>{{"g6DividendMillionths",n},{"g6DivisorMillionths",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,p,"-1"));
    }
    [Fact] public void DivideByZeroAndInexactRepeatingQuotientAreNotAcceptedAsExact()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,
            new Dictionary<string,int>{{"g6DividendMillionths",6300},{"g6DivisorMillionths",0}},"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,
            new Dictionary<string,int>{{"g6DividendMillionths",127400000},{"g6DivisorMillionths",40400000}},"3.2"));
    }
    [Fact] public void SourceScopedArabicGeneratedQuestionsUseExistingKernel()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{F,"uae.g5.u05.l13.contextual_decimal_unit_rate"})
        {
            var questions=e.Generate(L,f,[f],ExactSkillQuestionDifficulty.Standard,6,20261114,[]);
            Assert.Equal(6,questions.Count);
            foreach(var q in questions)
            {
                Assert.Contains("احسب",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
            }
        }
    }
}
