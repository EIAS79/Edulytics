using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit9Lesson02LikeFractionAdditionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:09:02";
    [Fact]
    public void LikeFractionAdditionFamiliesGenerateAndVerify()
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
    [InlineData(5,1,8,"6","3","4")]
    [InlineData(3,4,12,"7","7","12")]
    [InlineData(7,4,8,"11","11","8")]
    [InlineData(2,3,10,"5","1","2")]
    public void AdditionAndSimplificationMatchCorrectFractions(int a,int b,int denominator,string sum,string reducedNum,string reducedDen)
    {
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"denominator",denominator}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l02.sum_numerator",p,sum));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l02.simplified_sum_numerator",p,reducedNum));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l02.simplified_sum_denominator",p,reducedDen));
    }
    [Fact]
    public void RejectsMismatchedParameterDomains()
    {
        var p=new Dictionary<string,int>{{"a",8},{"b",1},{"denominator",8}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u09.l02.sum_numerator",p,"9"));
    }
    [Fact]
    public void RichArabicLessonHasLikeFractionExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("جمع الكسور المتشابهة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
