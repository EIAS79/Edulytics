using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U9L07SemanticTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:09:07:STUDENT-AR-128";
    [Fact] public void OriginalArabicLessonAndContractExist()
    {
        var x=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(x);
        Assert.Equal(8,x!.WorkedExamples.Count);
        Assert.True(x.Visuals.Count>=3);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal(1,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void ExactVerificationAndWrongAnswer()
    {
        var p=new Dictionary<string,int>{{"x",0},{"y",0},{"w",4},{"h",3}};
        const string f="uae.g6.u09.l09.coordinate_polygon_area";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"١٢"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"13"));
    }
}
