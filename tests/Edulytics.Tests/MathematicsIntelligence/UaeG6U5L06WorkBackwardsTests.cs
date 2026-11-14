using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U5L06WorkBackwardsTests
{
    const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:05:06:STUDENT-AR-128",T="uae.g6.u05.l06.work_backward_temperature",S="uae.g6.u05.l06.work_backward_schedule";
    [Fact] public void FourStepArabicInquiryHasExactNumericSubskillsAndTeacherReasoning()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(10,l!.WorkedExamples.Count);
        Assert.All(l.WorkedExamples,e=>Assert.True(e.Steps.Count>=3));
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.CalculationChain);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void ReverseTemperatureHandlesNegativeAndPositiveInitialValue()
    {
        var p=new Dictionary<string,int>{{"final",-6},{"change",-14}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(T,p,"8"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(T,p,"-20"));
        var q=new Dictionary<string,int>{{"final",4},{"change",9}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(T,q,"−٥"));
        var invalid=new Dictionary<string,int>{{"final",100},{"change",9}};
        Assert.False(ExactSkillContractQuestionEngine.Verify(T,invalid,"91"));
    }
    [Fact] public void ReverseScheduleCountsAllStepsAndRejectsInvalidParameters()
    {
        var p=new Dictionary<string,int>{{"endMinute",510},{"travel",20},{"food",25},{"prepare",15},{"message",10}};
        Assert.True(ExactSkillContractQuestionEngine.Verify(S,p,"07:20"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(S,p,"٠٧:٢٠"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(S,p,"07:30"));
        p["endMinute"]=1000;
        Assert.False(ExactSkillContractQuestionEngine.Verify(S,p,"07:20"));
    }
}
