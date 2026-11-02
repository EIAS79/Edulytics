using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Original source Grade2 L16 assertions written during C, unrun until D.</summary>
public sealed class UsCcssG2Unit1SixteenthLessonComparePracticeTests
{
    [Theory]
    [InlineData("LibraryBooks","Larger","Andre","37","29+8=37",true)]
    [InlineData("LibraryPages","Smaller","Andre","25","45-20=25",true)]
    [InlineData("ThreeReads1","Smaller","Noah","17","27-10=17",true)]
    [InlineData("ThreeReads2","Larger","Jada","55","25+30=55",true)]
    [InlineData("ThreeReads3","Difference","Jada","27","47-20=27",true)]
    [InlineData("ThreeReads4","Larger","Noah","42","28+14=42",true)]
    [InlineData("LibraryBooks","Smaller","Andre","37","29+8=37",false)]
    [InlineData("LibraryBooks","Larger","Andre","21","29-8=21",false)]
    public void EachOriginalStoryUsesCorrectUnknownPositionAndRelatedEquation(
        string id,string unknown,string bigger,string answer,string equation,bool expected)
    {
        var r=UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckOriginalProblem(id,unknown,bigger,answer,equation);
        Assert.Equal(expected,r.Correct);
        Assert.True(r.TeacherReviewNeeded);
    }

    [Fact]
    public void MakeTenWarmupAndOriginalEdulyticsCoolDownAreExact()
    {
        Assert.True(UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckWarmup("1","1","0").Correct);
        Assert.False(UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckWarmup("1","1","1").Correct);
        Assert.True(UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckNewEdulyticsCoolDown("54","22","22").Correct);
        Assert.False(UsCcssG2Unit1SixteenthLessonComparePractice
            .CheckNewEdulyticsCoolDown("54","22","23").Correct);
        Assert.Equal(6,UsCcssG2Unit1SixteenthLessonComparePractice.OriginalSourceStories.Count);
        Assert.Equal(5,UsCcssG2Unit1SixteenthLessonComparePractice.TeacherRubric.Count);
    }
}
