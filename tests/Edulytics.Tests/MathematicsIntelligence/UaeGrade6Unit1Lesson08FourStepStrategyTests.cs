using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;

// UAE G6 General source PDF96-98. Tests authored only; not run manually.
public sealed class UaeGrade6Unit1Lesson08FourStepStrategyTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:01:08:STUDENT-AR-128";
    [Fact] public void SourceInquiryHasAllFourOriginalStepsAndTeacherReasoning()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal("استقصاء حل المسائل: خطة الخطوات الأربع",l!.Title);
        Assert.Contains(l.Visuals,x=>x.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(l.WorkedExamples.Count>=5);
        Assert.True(l.KeyConcepts.Count>=4);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Single(contract!.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily("ratio.unit_rate.direct"));
    }
    [Fact] public void EqualGroupsAreExactAndLinearProgressionIsNotMisclassified()
    {
        var p=new Dictionary<string,int>{{"total",180},{"quantity",15}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",p,"12"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",p,"15"));
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var q in engine.Generate(Lesson,Lesson,["ratio.unit_rate.direct"],
            ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
        {
            Assert.Contains("وحدة",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify("ratio.unit_rate.direct",q.Parameters,q.CorrectAnswer));
        }
    }
}
