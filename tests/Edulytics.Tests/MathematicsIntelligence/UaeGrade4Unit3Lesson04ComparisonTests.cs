using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeGrade4Unit3Lesson04ComparisonTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L4:COMMON:03:04";
    [Fact]
    public void ReusesFourProvenAdditiveAndMultiplicativeComparisonFamilies()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.Equal(4,contract!.AllowedQuestionFamilies.Count);
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in contract.AllowedQuestionFamilies)
        {
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(f,out var ui));
            Assert.NotNull(ui);
            foreach(var q in engine.Generate(Lesson,f,[f],
                ExactSkillQuestionDifficulty.Standard,2,20261111,[]))
            {
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
                Assert.False(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,"-15"));
                Assert.DoesNotContain("result",q.Parameters.Keys);
            }
        }
    }
    [Fact]
    public void ComparisonOperationsAreNotInterchangeable()
    {
        var additive=new Dictionary<string,int>{{"a",1900},{"b",600}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u02.l08.bar_compare",additive,"١٬٣٠٠"));
        var multiplicative=new Dictionary<string,int>{{"baseNumber",6},{"multiple",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.times_as_many",multiplicative,"١٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.unknown_comparison_base",multiplicative,"6"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g4.u03.l03.times_as_many",multiplicative,"9"));
    }
    [Fact]
    public void SourceMatchedArabicLessonHasBothComparisonMethods()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("المقارنة لحل المسائل",l!.Title);
        Assert.True(l.WorkedExamples.Count>=3);
        Assert.Equal(RichLessonVisualKind.EquationSet,Assert.Single(l.Visuals).Kind);
    }
}
