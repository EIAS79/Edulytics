using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson03HundredthsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:03";
    [Fact]
    public void HundredthsFamiliesGenerateAndVerify()
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
    [InlineData(0,4,"0.04","4")]
    [InlineData(1,3,"0.13","13")]
    [InlineData(8,3,"0.83","83")]
    [InlineData(0,0,"0.00","0")]
    public void GridHundredthsMatchDecimalNotation(int tenths,int hundredths,string dec,string count)
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",tenths},{"hundredths",hundredths}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l03.hundredths_decimal",p,dec));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l03.hundredths_fraction_numerator",p,count));
    }
    [Fact]
    public void ArabicDecimalDigitsAreAccepted()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",1},{"hundredths",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l03.hundredths_decimal",p,"٠٫١٣"));
    }
    [Fact]
    public void InvalidHundredthsReject()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",1},{"hundredths",10}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l03.hundredths_decimal",p,"0.20"));
    }
    [Fact]
    public void ArabicRichContentModelsHundredths()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الأجزاء من مئة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
