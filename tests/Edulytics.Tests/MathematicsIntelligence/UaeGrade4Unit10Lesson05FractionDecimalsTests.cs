using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson05FractionDecimalsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:05";
    [Fact]
    public void EquivalentFractionDecimalFamiliesGenerateAndVerify()
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
    [InlineData(7,0,"70","0.70")]
    [InlineData(4,3,"43","0.43")]
    [InlineData(0,5,"5","0.05")]
    public void EquivalentTenthsAndHundredthsMatchDecimal(int tenths,int hundredths,string hundredCount,string dec)
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",tenths},{"hundredths",hundredths}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l04.decimal_to_hundredths",p,hundredCount));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l03.hundredths_decimal",p,dec));
    }
    [Fact]
    public void InvalidPlaceValueIsRejected()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",12},{"hundredths",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l03.hundredths_decimal",p,"1.21"));
    }
    [Fact]
    public void ArabicContentIncludesFractionDecimalConversions()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الكسور والأعداد العشرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
