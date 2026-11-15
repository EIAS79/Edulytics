using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L05PowersOfTenLabTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:05:STUDENT-AR-128";
    const string F="uae.g5.u05.l06.decimal_times_power_ten";
    [Fact] public void ArabicLabIsRegisteredWithExactExistingFamily()
    {
        var lesson=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(lesson);
        Assert.Equal("مختبر الاستكشاف: الضرب بقوى العشرة",lesson!.Title);
        Assert.Contains(lesson.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var contract));
        Assert.Contains(F,contract!.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(F));
    }
    [Theory]
    [InlineData(28600000,6,"28600000")]
    [InlineData(12400000,-1,"1.24")]
    [InlineData(12400000,-2,"0.124")]
    [InlineData(12400000,-3,"0.0124")]
    [InlineData(12400000,-4,"0.00124")]
    [InlineData(1240000,7,"12400000")]
    [InlineData(4950000,1,"49.5")]
    [InlineData(13600000,3,"13600")]
    [InlineData(2350000,-2,"0.0235")]
    public void ExactPositiveAndNegativePowersVerifyWithoutRounding(int baseMillionths,int exponent,string expected)
    {
        var args=new Dictionary<string,int> {["g6PowerBaseMillionths"]=baseMillionths,["g6PowerExponent"]=exponent};
        Assert.True(ExactSkillContractQuestionEngine.Verify(F,args,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,args,"999999999"));
    }
    [Fact] public void IllegalPowersAndMissingBaseAreRejected()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,
            new Dictionary<string,int>{{"g6PowerBaseMillionths",12400000},{"g6PowerExponent",-5}},"0"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(F,
            new Dictionary<string,int>{{"g6PowerExponent",3}},"1240"));
    }
    [Fact] public void ArabicGeneratedPowerOfTenQuestionsRemainVerifiable()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var qs=engine.Generate(L,F,[F],ExactSkillQuestionDifficulty.Standard,6,20261114,[]);
        Assert.Equal(6,qs.Count);
        foreach(var q in qs)
        {
            Assert.Contains("قوى",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(F,q.Parameters,q.CorrectAnswer));
        }
    }
}
