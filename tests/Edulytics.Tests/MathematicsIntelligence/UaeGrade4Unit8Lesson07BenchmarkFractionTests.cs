using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit8Lesson07BenchmarkFractionTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:08:07";
    [Fact]
    public void BenchmarkFamiliesGenerateVerifyAndRejectIncorrect()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
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
                Assert.DoesNotContain("???",q.Prompt);
            }
        }
    }
    [Theory]
    [InlineData(1,6,"0")]
    [InlineData(3,6,"1")]
    [InlineData(5,8,"2")]
    [InlineData(3,8,"0")]
    public void ClassifyFractionsAroundOneHalf(int numerator,int denominator,string expected)
    {
        var p=new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l07.compare_with_half",p,expected));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l07.benchmark_order",p,expected));
    }
    [Fact]
    public void MalformedFractionsCannotBeVerified()
    {
        var p=new Dictionary<string,int>{{"numerator",5},{"denominator",0}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u08.l07.compare_with_half",p,"1"));
    }
    [Fact]
    public void ArabicRichContentIsValidAndSourceMapped()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("استخدام الكسور المعيارية للمقارنة والترتيب",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
