using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson04FractionDecimalModelsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:04";
    [Fact]
    public void FractionDecimalFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
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
    [InlineData(7,0,"70","7")]
    [InlineData(4,0,"40","4")]
    [InlineData(0,0,"0","0")]
    public void EquivalentTenthsAndHundredthsMatch(int tenths,int hundredths,string hundredCount,string tenthCount)
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",tenths},{"hundredths",hundredths}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l04.decimal_to_hundredths",p,hundredCount));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l04.decimal_to_tenths",p,tenthCount));
    }
    [Fact]
    public void HundredthFractionCannotBeMisreadAsTenths()
    {
        var p=new Dictionary<string,int>{{"whole",0},{"tenths",7},{"hundredths",1}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l04.decimal_to_hundredths",p,"71"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l04.decimal_to_tenths",p,"7"));
    }
    [Fact]
    public void ArabicRichContentIncludesFractionsAndDecimalModels()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("نشاط عملي: تمثيل الكسور والأعداد العشرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
