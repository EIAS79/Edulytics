using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U2L09PercentBarTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:09:STUDENT-AR-128";
    [Fact] public void OriginalArabicLabReusesExactPercentEngine()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=5);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.NotNull(c);
        Assert.Contains("percentages.core.of_quantity",c!.AllowedQuestionFamilies);
    }
    [Fact] public void BarPartsComputeExactlyAndCheckRemainder()
    {
        const string f="percentages.core.of_quantity";
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,new Dictionary<string,int>{{"percent",20},{"quantity",180}},"36"));
        Assert.True(ExactSkillContractQuestionEngine.Verify(f,new Dictionary<string,int>{{"percent",25},{"quantity",32}},"8"));
        Assert.Equal(180,36+18+126);
    }
}
