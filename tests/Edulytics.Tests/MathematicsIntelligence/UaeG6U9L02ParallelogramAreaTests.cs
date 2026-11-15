using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U9L02ParallelogramAreaTests
{
    private const string L = "PED:UAE-MOE-MATH:L6:GENERAL:09:02:STUDENT-AR-128";
    [Fact] public void HasOriginalArabicRichLessonAndPractice()
    {
        var lesson=RichLessonContentV2Registry.Find(L,"ar");
        Assert.NotNull(lesson);
        Assert.Equal(8,lesson!.WorkedExamples.Count);
        Assert.Contains(lesson.Visuals,v=>v.Kind==RichLessonVisualKind.ShapeDecomposition);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(L,out var c));
        Assert.Equal(3,c!.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Fact] public void ArabicAreaAndHeightAreVerifiedAndWrongAnswersRejected()
    {
        var p=new Dictionary<string,int>{{"b",6},{"h",8}};
        const string f="uae.g6.u09.l02.parallelogram_area";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"٤٨"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,p,"48"));
        Assert.False(ExactSkillContractQuestionEngine.Verify(f,p,"٤٩"));
        var q=new Dictionary<string,int>{{"b",9},{"area",45}};
        Assert.True(ExactSkillContractQuestionEngine.Verify("uae.g6.u09.l02.parallelogram_height",q,"٥"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u09.l02.parallelogram_height",q,"٧"));
        Assert.False(ExactSkillContractQuestionEngine.Verify("uae.g6.u09.l02.parallelogram_height",new Dictionary<string,int>{{"b",0},{"area",45}},"0"));
    }
}
