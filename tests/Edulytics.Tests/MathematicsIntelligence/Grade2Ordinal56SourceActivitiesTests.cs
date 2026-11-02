using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Ordinal56SourceActivitiesTests
{
    [Fact]
    public void OriginalActivityHasHumanReviewBoundary()
    {
        var result=UsCcssG2Unit4NextThreeActivities.Source(4,"30","25","20","10");
        Assert.True(result.TeacherReviewNeeded);
        Assert.True(result.Correct);
        var invalid=UsCcssG2Unit4NextThreeActivities.Source(4,"999","999","999","999");
        Assert.False(invalid.Correct);
    }
    [Fact]
    public void IndependentlyAuthoredNumericalChecksCannotCertifyRealStudents()
    {
        var result=UsCcssG2Unit4NextThreeActivities.Independent(4,"11","10","52","17");
        Assert.True(result.Correct);
        Assert.False(result.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4NextThreeActivities.Independent(4,"999","10","52","17").Correct);
        Assert.NotEmpty(UsCcssG2Unit4NextThreeActivities.TeacherRubrics[4]);
    }
}
