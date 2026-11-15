using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U9L06SemanticTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:09:06:STUDENT-AR-128";
    [Fact] public void OriginalArabicLessonAndContractExist()
    {
        var x=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(x);
        Assert.Equal(8,x!.WorkedExamples.Count);
        Assert.True(x.Visuals.Count>=3);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(2,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void ExactVerificationAndWrongAnswer()
    {
        var p=new Dictionary<string,int>{{"a",8},{"b",6},{"h",4}};
        const string f="uae.g6.u09.l06.trapezoid_area";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٢٨"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"29"));
    }
}
