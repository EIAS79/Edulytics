using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UaeG6U9L01ParallelogramAreaLabTests
{
    private const string Lesson = "PED:UAE-MOE-MATH:L6:GENERAL:09:01:STUDENT-AR-128";
    [Fact] public void ArabicLabHasExamplesAndVisuals()
    {
        var l=RichLessonContentV2Registry.Find(Lesson,"ar");
        Assert.NotNull(l);
        Assert.Equal(8,l!.WorkedExamples.Count);
        Assert.Contains(l.Visuals,v=>v.Kind==RichLessonVisualKind.ShapeDecomposition);
        Assert.Contains(l.WorkedExamples,e=>e.Answer=="٤٥ سم²");
    }
    [Fact] public void ReusesExistingEngineAndRetainsSourceGate()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Lesson,out var c));
        Assert.Equal("READY_VERIFIED",c!.Readiness);
        Assert.Equal(3,c.AllowedQuestionFamilies.Count);
        foreach(var f in c.AllowedQuestionFamilies)
            Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(f));
    }
    [Theory]
    [InlineData(6,4,24)][InlineData(7,3,21)][InlineData(9,5,45)]
    [InlineData(3,2,6)][InlineData(5,4,20)][InlineData(12,3,36)]
    [InlineData(8,5,40)][InlineData(10,6,60)]
    public void BaseTimesPerpendicularHeight(int b,int h,int expected)
        => Assert.Equal(expected,b*h);
}
