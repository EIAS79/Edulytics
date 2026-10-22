using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2EighthLessonDifferentWaysTests
{
    [Fact]
    public void SourceMentalFactsAndLinClareNumericalEquivalenceVerified()
    {
        var warm=UsCcssG2Unit2EighthLessonDifferentWays.NumberTalk("38","48","18","18");
        Assert.True(warm.Correct);Assert.True(warm.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2EighthLessonDifferentWays.NumberTalk("38","48","18","28").Correct);
        Assert.True(UsCcssG2Unit2EighthLessonDifferentWays.LinClare("15","21","11").Correct);
        Assert.False(UsCcssG2Unit2EighthLessonDifferentWays.LinClare("15","71","11").Correct);
    }
    [Fact]
    public void AndrePracticeAndIndependentExitHaveSeparateTeacherRubric()
    {
        Assert.True(UsCcssG2Unit2EighthLessonDifferentWays.AndreAndPractice(
            "37","16","45","18").Correct);
        Assert.False(UsCcssG2Unit2EighthLessonDifferentWays.AndreAndPractice(
            "37","16","55","18").Correct);
        var exit=UsCcssG2Unit2EighthLessonDifferentWays.Exit("26","26","36");
        Assert.True(exit.Correct);Assert.False(exit.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit2EighthLessonDifferentWays.Exit("26","27","36").Correct);
        Assert.True(UsCcssG2Unit2EighthLessonDifferentWays.TeacherRubric.Count>=6);
    }
}
