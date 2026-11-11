using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson02TenthsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:02";
    [Fact]
    public void TenthsFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in c.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(3,"0.3","3")]
    [InlineData(7,"0.7","7")]
    [InlineData(8,"0.8","8")]
    public void TenthsMatchFractions(int n,string decimalAnswer,string count)
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",n},{"hundredths",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l02.tenths_decimal",p,decimalAnswer));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l02.tenths_count",p,count));
    }
    [Fact]
    public void ArabicDecimalPointAndArabicDigitsAreAccepted()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",7},{"hundredths",0}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l02.tenths_decimal",p,"٠٫٧"));
    }
    [Fact]
    public void HundredthsAreRejectedFromTenthsFamily()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",7},{"hundredths",2}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l02.tenths_decimal",p,"0.72"));
    }
    [Fact]
    public void ArabicRichLessonIncludesTenthsModels()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأعشار (الأجزاء من عشرة)",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
