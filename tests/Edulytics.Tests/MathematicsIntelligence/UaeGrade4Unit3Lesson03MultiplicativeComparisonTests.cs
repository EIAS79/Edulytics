using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson03MultiplicativeComparisonTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:03";
    [Fact]
    public void ThreeComparisonFamiliesAreRoutedWithIndependentVerifier()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(3,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            for(int seed=1;seed<=6;seed++)
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,1,seed,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-1"));
                Assert.Equal(2,q.Parameters.Count);
                Assert.DoesNotContain("result",q.Parameters.Keys);
                var bad=q.Parameters.ToDictionary(x=>x.Key,x=>x.Value);
                bad["multiple"]=0;
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,bad,q.CorrectAnswer));
            }
        }
    }
    [Fact]
    public void IndependentTimesAsManyAndDivision()
    {
        var p=new Dictionary<string,int>{{"baseNumber",7},{"multiple",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.times_as_many",p,"٢١"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.unknown_comparison_base",p,"7"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.unknown_multiplier",p,"٣"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.times_as_many",p,"10"));
    }
    [Fact]
    public void ArabicLessonHasModelsAndExamples()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("الضرب في صورة مقارنة",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
