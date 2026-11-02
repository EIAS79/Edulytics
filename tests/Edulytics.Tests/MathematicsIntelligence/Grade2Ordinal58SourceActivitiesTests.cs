using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Ordinal58SourceActivitiesTests
{
    [Fact]
    public void OriginalActivityHasHumanReviewBoundary()
    {
        var result=UsCcssG2Unit4NextThreeActivities.Source(6,"40","38","38","28");
        Assert.True(result.TeacherReviewNeeded);
        Assert.True(result.Correct);
        var invalid=UsCcssG2Unit4NextThreeActivities.Source(6,"999","999","999","999");
        Assert.False(invalid.Correct);
    }
    [Fact]
    public void IndependentlyAuthoredNumericalChecksCannotCertifyRealStudents()
    {
        var result=UsCcssG2Unit4NextThreeActivities.Independent(6,"38","1","2","10");
        Assert.True(result.Correct);
        Assert.False(result.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4NextThreeActivities.Independent(6,"999","1","2","10").Correct);
        Assert.NotEmpty(UsCcssG2Unit4NextThreeActivities.TeacherRubrics[6]);
    }
}
