using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE Arabic Grade6 General source PDF80-87. Written for Stage D, not manually run.
public sealed class UaeGrade6Unit1Lesson06RatioTablesTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:06:STUDENT-AR-128";
    [Fact] public void ArabicRatioTablesReuseExactKernelAndOriginalRichVisuals()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("جداول النسب",l!.Title);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.RatioTable);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Equal(2,contract!.AllowedQuestionFamilies.Count);
    }
    [Fact] public void EquivalentRatioAndTotalPartitionRemainIndependentlyVerifiable()
    {
        const string ratio="ratio.unit_rate.equivalent_ratio";
        var p=new Dictionary<string,int>{{"baseFirst",6},{"baseSecond",1},{"targetSecond",5}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(ratio,p,"30"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(ratio,p,"25"));
        const string split="ratio.unit_rate.divide_total";
        var q=new Dictionary<string,int>{{"firstPart",2},{"secondPart",3},{"total",30},{"askedPart",3}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(split,q,"18"));
    }
    [Fact] public void ArabicRatioTablesHaveRealArabicQuestionsFromExactEngine()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var family in new[]{"ratio.unit_rate.equivalent_ratio","ratio.unit_rate.divide_total"})
        {
            Assert.True(LessonPracticeInteractionRegistry.TryResolve(family,out _));
            foreach(var q in engine.Generate(Lesson,Lesson,[family],ExactSkillQuestionDifficulty.Standard,3,20261114,[]))
            {
                Assert.Contains("نسب",q.Prompt);
                Assert.True(ExactSkillContractQuestionEngine.Verify(family,q.Parameters,q.CorrectAnswer));
            }
        }
    }
}
