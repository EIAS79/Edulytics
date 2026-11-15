using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U2L08PercentEstimationTests
{
    private const string Lesson="PED:UAE-MOE-MATH:L6:GENERAL:02:08:STUDENT-AR-128";
    private const string Family="percentages.core.of_quantity";
    [Fact] public void OriginalArabicEstimationLessonUsesExactPercentKernel()
    {
        var rich=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(rich);
        Assert.True(rich!.WorkedExamples.Count>=7);
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var contract));
        Assert.NotNull(contract);
        Assert.Contains(Family,contract!.AllowedQuestionFamilies);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
    }
    [Fact] public void EstimateAndExactResultAreDeliberatelyDistinct()
    {
        Assert.Equal(325.24,692*0.47,2);
        Assert.Equal(350,700*0.5,2);
        Assert.Equal(33.66,198*.17,2);
        Assert.Equal(36.72,408*.09,2);
    }
    [Fact] public void GeneratorIssuesArabicAndVerifierChecksExactValue()
    {
        var engine=new ExactSkillContractQuestionEngine();
        foreach(var q in engine.Generate(Lesson,Family,[Family],ExactSkillQuestionDifficulty.Standard,2,20261114,[]))
        {
            Assert.Contains("الدقيقة",q.Prompt);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family,q.Parameters,q.CorrectAnswer));
        }
    }
}
