using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson07MixedSubtractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:07";
    [Fact]
    public void MixedSubtractionFamiliesGenerateVerifyAndRejectInvalidAnswers()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
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
    [InlineData(4,4,1,1,3,"2","2","10")]
    [InlineData(8,5,7,3,2,"2","5","21")]
    [InlineData(3,3,1,1,2,"1","2","5")]
    [InlineData(5,6,2,2,4,"3","3","18")]
    public void MixedDifferenceSupportsRegrouping(int denominator,int whole1,int numerator1,int whole2,int numerator2,string whole,string fraction,string improper)
    {
        var p=new Dictionary<string,int>{
            ["denominator"]=denominator,["whole1"]=whole1,["numerator1"]=numerator1,
            ["whole2"]=whole2,["numerator2"]=numerator2};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l07.difference_whole",p,whole));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l07.difference_fraction_numerator",p,fraction));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l07.difference_improper_numerator",p,improper));
        Assert.Equal(int.Parse(improper),int.Parse(whole)*denominator+int.Parse(fraction));
    }
    [Fact]
    public void InvalidOrderOfWholeNumbersIsRejected()
    {
        var p=new Dictionary<string,int>{{"denominator",4},{"whole1",2},{"numerator1",1},{"whole2",3},{"numerator2",2}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l07.difference_whole",p,"-1"));
    }
    [Fact]
    public void RichArabicLessonExplainsBorrowing()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("طرح الأعداد الكسرية",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
