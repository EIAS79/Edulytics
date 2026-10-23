using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C authored not executed until Stage D.</summary>
public sealed class UsCcssG2Unit1TwelfthLessonSortAndDisplayTests
{
    [Fact]
    public void ExactlyThreeRealObjectCategoriesNeedTwentyToThirtyItems()
    {
        var valid=UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRealCollection("Blocks","Cubes","Counters","8","12","5");
        Assert.True(valid.Review.Correct);
        Assert.Equal(25,valid.Data!.Total);
        Assert.True(UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRepresentation(valid.Data,"Sorted classroom items","Picture","1",null,null).Correct);
        Assert.True(UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRepresentation(valid.Data,"Sorted classroom items","Bar",null,"0","1").Correct);
    }
    [Theory]
    [InlineData("A","B","C","8","7","4")]
    [InlineData("A","B","C","10","10","11")]
    [InlineData("Same","Same","Different","8","12","5")]
    [InlineData("A","B","C","25","0","0")]
    public void TooFewManyRepeatedOrEmptyCategoriesCannotPass(
        string a,string b,string c,string x,string y,string z)
    {
        Assert.False(UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRealCollection(a,b,c,x,y,z).Review.Correct);
    }
    [Fact]
    public void TwoGraphQuestionsReadTheObservedCountsAndNeedTeacherValidation()
    {
        var collection=UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckRealCollection("Blocks","Cubes","Counters","8","12","5").Data!;
        var q=UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckTwoOriginalQuestions(collection,
                "How many blocks?","How many cubes and counters together?","8","17");
        Assert.True(q.Correct);
        Assert.True(q.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckTwoOriginalQuestions(collection,"How many blocks?","How many blocks?","8","17").Correct);
    }
    [Fact]
    public void MentalSubtractionUsesTen()
    {
        Assert.True(UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckNumberTalk("10","8","10","7").Correct);
        Assert.False(UsCcssG2Unit1TwelfthLessonSortAndDisplay
            .CheckNumberTalk("10","9","10","7").Correct);
        Assert.Equal(5,UsCcssG2Unit1TwelfthLessonSortAndDisplay.TeacherRubric.Count);
    }
}
