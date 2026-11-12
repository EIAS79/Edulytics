using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson05SimplestFormTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:05";
    [Fact]
    public void ThreeSimplificationFamiliesGenerateAndVerify()
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
    [InlineData(8,12,"2","3")]
    [InlineData(6,12,"1","2")]
    [InlineData(5,8,"5","8")]
    public void ReducedNumeratorAndDenominatorMatchGcd(int n,int d,string nResult,string dResult)
    {
        var p=new Dictionary<string,int>{{"numerator",n},{"denominator",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l05.reduced_numerator",p,nResult));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l05.reduced_denominator",p,dResult));
    }
    [Theory]
    [InlineData(8,12,"0")]
    [InlineData(5,8,"1")]
    public void DeterminesWhetherFractionIsAlreadySimplest(int n,int d,string expected)
    {
        var p=new Dictionary<string,int>{{"numerator",n},{"denominator",d}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l05.is_simplest",p,expected));
    }
    [Fact]
    public void InvalidFractionsCannotBeChecked()
    {
        var p=new Dictionary<string,int>{{"numerator",5},{"denominator",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l05.is_simplest",p,"0"));
    }
    [Fact]
    public void RichArabicLessonHasGcdAndSimplificationExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("أبسط صورة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
