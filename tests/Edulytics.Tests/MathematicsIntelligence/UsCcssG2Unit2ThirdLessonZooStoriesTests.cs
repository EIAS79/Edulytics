using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class UsCcssG2Unit2ThirdLessonZooStoriesTests
{
    [Theory]
    [InlineData("21","21+34=55",true)]
    [InlineData("21","55-34=21",true)]
    [InlineData("20","21+34=55",false)]
    [InlineData("21","34+55=89",false)]
    public void StartUnknownBusAndEquationAreBothChecked(string answer,string equation,bool correct)
    {
        var result=UsCcssG2Unit2ThirdLessonZooStories.CheckBus(answer,equation);
        Assert.Equal(correct,result.Correct);
        Assert.True(result.TeacherReviewNeeded);
    }
    [Fact]
    public void AllThreeZooStructuresAndIndependentExitCheckedExactly()
    {
        Assert.True(UsCcssG2Unit2ThirdLessonZooStories
            .CheckOtherStories("42","31","54").Correct);
        Assert.False(UsCcssG2Unit2ThirdLessonZooStories
            .CheckOtherStories("42","31","44").Correct);
        Assert.True(UsCcssG2Unit2ThirdLessonZooStories
            .CheckCoolDown("42","42","43").Correct);
        Assert.False(UsCcssG2Unit2ThirdLessonZooStories
            .CheckCoolDown("42","41","43").Correct);
        Assert.True(UsCcssG2Unit2ThirdLessonZooStories.TeacherRubric.Count>=6);
    }
}
