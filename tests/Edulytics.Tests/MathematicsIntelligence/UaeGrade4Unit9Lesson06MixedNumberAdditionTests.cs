using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson06MixedNumberAdditionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:06";
    [Fact]
    public void MixedNumberFamiliesGenerateAndVerify()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,family,[family],ExactSkillQuestionDifficulty.Standard,3,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,"-1"));
            }
        }
    }
    [Theory]
    [InlineData(4,2,1,3,2,"5","3","23")]
    [InlineData(8,2,3,1,2,"3","5","29")]
    [InlineData(3,1,2,2,1,"4","0","12")]
    [InlineData(5,3,2,1,1,"4","3","23")]
    public void MixedAdditionCarriesAndKeepsEqualDenominators(int d,int whole1,int numerator1,int whole2,int numerator2,string whole,string fraction,string improper)
    {
        var p=new Dictionary<string,int>
        {
            ["denominator"]=d,["whole1"]=whole1,["whole2"]=whole2,
            ["numerator1"]=numerator1,["numerator2"]=numerator2
        };
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l06.sum_whole",p,whole));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l06.sum_fraction_numerator",p,fraction));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l06.sum_improper_numerator",p,improper));
        Assert.Equal(int.Parse(improper),int.Parse(whole)*d+int.Parse(fraction));
    }
    [Fact]
    public void InvalidFractionNumeratorIsRejected()
    {
        var p=new Dictionary<string,int>{{"denominator",4},{"whole1",1},{"whole2",2},{"numerator1",4},{"numerator2",1}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l06.sum_whole",p,"4"));
    }
    [Fact]
    public void ArabicRichLessonExplainsRegrouping()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("جمع الأعداد الكسرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
