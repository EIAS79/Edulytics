using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal63SourceActivityMathTests
{
    [Fact]
    public void OriginalSourceArithmeticStillRequiresRealTeacherEvidence()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.Source(11,"26","35","46","45");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.Source(11,"999","35","46","45").Correct);
        Assert.NotEmpty(UsCcssG2Unit4FinalSixSourcePractice.TeacherRubrics[11]);
    }
    [Fact]
    public void IndependentPracticeDoesNotCertifyClassroomArtwork()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(11,"24","47","38","53");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.FirstParty(11,"999","47","38","53").Correct);
    }
}
