using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit10Lesson06FractionRoundingTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:10:06";
    [Fact]
    public void BenchmarkRoundingFamiliesGenerateVerify()
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
    [InlineData(1,10,"0","0")]
    [InlineData(6,10,"1","2")]
    [InlineData(9,10,"2","2")]
    [InlineData(4,8,"1","1")]
    public void FractionComparedWithBenchmarkAndHalf(int numerator,int denominator,string rounded,string halfRelation)
    {
        var p=new Dictionary<string,int>{{"numerator",numerator},{"denominator",denominator}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l06.nearest_benchmark",p,rounded));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l06.compared_to_half",p,halfRelation));
    }
    [Fact]
    public void InvalidFractionIsRejected()
    {
        var p=new Dictionary<string,int>{{"numerator",10},{"denominator",10}};
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u10.l06.nearest_benchmark",p,"2"));
    }
    [Fact]
    public void RichArabicLessonTeachesNumberLineBenchmarks()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("تقريب الكسور",l!.Title);
        Assert.True(l.WorkedExamples.Count>=4);
        Assert.True(l.KeyConcepts.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
