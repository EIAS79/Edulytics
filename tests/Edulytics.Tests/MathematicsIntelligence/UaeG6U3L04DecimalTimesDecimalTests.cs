using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Grade6 General U03L04 — selected Arabic student book pages 197-204.
// Authored A-B-C without running dotnet or CI manually.
public sealed class UaeG6U3L04DecimalTimesDecimalTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:03:04:STUDENT-AR-128";
    private const string Family="uae.g5.u05.l05.multiply_decimal_factors";
    [Fact]
    public void ArabicSourceLessonUsesReusedDecimalFactorFamily()
    {
        var content=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(content);
        Assert.Equal("ضرب الأعداد العشرية في أعداد عشرية",content!.Title);
        Assert.True(content.WorkedExamples.Count>=10);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.Contains(content.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Contains(Family,contract!.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.True(LessonPracticeInteractionRegistry.TryResolve(Family,out _));
    }

    [Theory]
    [InlineData(3600000,50000,"0.18")]
    [InlineData(112000,7200000,"0.8064")]
    [InlineData(67000,1400000,"0.0938")]
    [InlineData(53000,450000,"0.02385")]
    [InlineData(28450000,11500000,"327.175")]
    [InlineData(14700000,11361000,"167.0067")]
    [InlineData(25040000,3005000,"75.2452")]
    [InlineData(3985300,8032856,"32.0133410168")]
    public void ExactProductsIncludingTenDecimalPlacesAreIndependentlyVerified(int first,int second,string expected)
    {
        var values=new Dictionary<string,int> {
            ["g6DecimalFirstMillionths"]=first,
            ["g6DecimalSecondMillionths"]=second
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,values,expected));
        var arabic=new string(expected.Select(c=>c switch {
            >= '0' and <= '9'=>(char)('٠'+c-'0'),
            '.'=>'٫',_=>c
        }).ToArray());
        Assert.True(ExactSkillContractQuestionEngine.Verify(Family,values,arabic));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,values,"-1"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,values,"1e9"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,values,"abc"));
    }

    [Fact]
    public void InvalidPrecisionOrMissingFactorCannotVerify()
    {
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,
            new Dictionary<string,int>{{"g6DecimalFirstMillionths",3600000}},"0.18"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(Family,
            new Dictionary<string,int>{{"g6DecimalFirstMillionths",-1},
                {"g6DecimalSecondMillionths",50000}},"0"));
    }

    [Fact]
    public void GeneratedArabicDecimalsVerifyAndRetainExactScientificPrecision()
    {
        var engine=new ExactSkillContractQuestionEngine();
        var generated=engine.Generate(Lesson,Family,[Family],ExactSkillQuestionDifficulty.Standard,8,20261114,[]);
        Assert.Equal(8,generated.Count);
        foreach(var item in generated)
        {
            Assert.Contains("احسب حاصل الضرب",item.Prompt);
            Assert.True(item.Parameters.ContainsKey("g6DecimalFirstMillionths"));
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,item.Parameters,item.CorrectAnswer));
        }
        Assert.Equal(8,generated.Select(x=>x.CorrectAnswer+"|"+x.Prompt).Distinct().Count());
    }
}
