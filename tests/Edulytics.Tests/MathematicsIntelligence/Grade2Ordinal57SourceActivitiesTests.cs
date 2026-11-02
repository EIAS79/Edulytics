using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade2Ordinal57SourceActivitiesTests
{
    [Fact]
    public void OriginalActivityHasHumanReviewBoundary()
    {
        var result=UsCcssG2Unit4NextThreeActivities.Source(5,"10","18","30","19");
        Assert.True(result.TeacherReviewNeeded);
        Assert.Null(result.Correct);
        var invalid=UsCcssG2Unit4NextThreeActivities.Source(5,"999","999","999","999");
        Assert.Null(invalid.Correct);
    }
    [Fact]
    public void IndependentlyAuthoredNumericalChecksCannotCertifyRealStudents()
    {
        var result=UsCcssG2Unit4NextThreeActivities.Independent(5,"20","4","9","2");
        Assert.True(result.Correct);
        Assert.False(result.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4NextThreeActivities.Independent(5,"999","4","9","2").Correct);
        Assert.NotEmpty(UsCcssG2Unit4NextThreeActivities.TeacherRubrics[5]);
    }
}
