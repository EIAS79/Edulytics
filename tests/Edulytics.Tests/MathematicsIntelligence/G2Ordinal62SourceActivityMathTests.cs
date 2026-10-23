using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal62SourceActivityMathTests
{
    [Fact]
    public void OriginalSourceArithmeticStillRequiresRealTeacherEvidence()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.Source(10,"43","11","34","78");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.Source(10,"999","11","34","78").Correct);
        Assert.NotEmpty(UsCcssG2Unit4FinalSixSourcePractice.TeacherRubrics[10]);
    }
    [Fact]
    public void IndependentPracticeDoesNotCertifyClassroomArtwork()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(10,"34","47","14","18");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.FirstParty(10,"999","47","14","18").Correct);
    }
}
