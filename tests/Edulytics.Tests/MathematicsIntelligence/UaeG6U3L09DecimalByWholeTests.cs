using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U3L09DecimalByWholeTests
{
    const string L="PED:UAE-MOE-MATH:L6:GENERAL:03:09:STUDENT-AR-128";
    const string Exact="uae.g5.u05.l11.exact_decimal_by_whole";
    const string Rounded="uae.g5.u05.l11.round_decimal_quotient_hundredth";
    [Fact] public void ArabicSourceLessonKeepsExactAndRoundedContractsDistinct()
    {
        var l=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(l);
        Assert.True(l!.WorkedExamples.Count>=8);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        Assert.Contains(Exact,c.AllowedQuestionFamilies);
        Assert.Contains(Rounded,c.AllowedQuestionFamilies);
    }
    [Theory]
    [InlineData(6800,2,"3.4")]
    [InlineData(7700,14,"0.55")]
    [InlineData(3490,4,"0.8725")]
    [InlineData(55080,17,"3.24")]
    [InlineData(9480,15,"0.632")]
    [InlineData(12320,22,"0.56")]
    [InlineData(69904,34,"2.056")]
    [InlineData(3600,4,"0.9")]
    [InlineData(118500,5,"23.7")]
    [InlineData(55200,46,"1.2")]
    public void ExactQuotientsRetainAllFractionalPlaces(int n,int d,string expected)
    {
        var p=new Dictionary<string,int>{{"g6DividendThousandths",n},{"g6WholeDivisor",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Exact,p,expected));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Exact,p,"-1"));
    }
    [Theory]
    [InlineData(6740,3,"2.25")]
    [InlineData(7240,7,"1.03")]
    [InlineData(336752,31,"10.86")]
    [InlineData(42560,22,"1.93")]
    [InlineData(39400,3,"13.13")]
    [InlineData(10900,6,"1.82")]
    [InlineData(27400,11,"2.49")]
    [InlineData(12340,7,"1.76")]
    [InlineData(80400,19,"4.23")]
    [InlineData(35820,16,"2.24")]
    public void RepeatingQuotientsRoundOnlyToCentsWhenRequested(int n,int d,string rounded)
    {
        var p=new Dictionary<string,int>{{"g6DividendThousandths",n},{"g6WholeDivisor",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(Rounded,p,rounded));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Exact,p,rounded));
    }
    [Fact] public void GeneratorReturnsSourceSpecificArabicQuestionsUsingExistingEngine()
    {
        var e=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{Exact,Rounded})
        {
            var qs=e.Generate(L,f,[f],ExactSkillQuestionDifficulty.Standard,5,20261114,[]);
            Assert.Equal(5,qs.Count);
            foreach(var q in qs)
            {
                Assert.Contains("احسب",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
            }
        }
    }
}
