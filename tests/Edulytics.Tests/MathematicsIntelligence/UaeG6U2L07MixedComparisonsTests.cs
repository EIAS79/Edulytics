using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U2L07MixedComparisonsTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:07:STUDENT-AR-128";
    [Fact] public void ArabicLessonAndExistingExactFamiliesAreBound()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.Equal("مقارنة وترتيب الكسور والأعداد العشرية والنسب المئوية",rich!.Title);
        Assert.True(rich.WorkedExamples.Count>=6);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
        foreach(var f in contract.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void MixedFractionDecimalCorrectnessIsExact()
    {
        const string f="fractions.compare.unlike.common_denominator";
        var p=new Dictionary<string,int>{{"n1",3},{"d1",4},{"n2",70},{"d2",100}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,">"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"<"));
    }
    [Fact] public void ScopedArabicGenerationUsesIndependentVerifier()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var f in new[]{"fractions.compare.unlike.common_denominator","fractions.compare.unlike.order_three"})
            foreach(var q in engine.Generate(Lesson,f,[f],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
            {
                Assert.True(q.Prompt.Contains("قارن")||q.Prompt.Contains("رتّب"));
                Assert.True(ExactSkillContractQuestionEngine.Verify(f,q.Parameters,q.CorrectAnswer));
            }
    }
}
