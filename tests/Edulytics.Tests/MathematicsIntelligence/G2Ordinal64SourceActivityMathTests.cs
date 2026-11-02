using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal64SourceActivityMathTests
{
    [Fact]
    public void OriginalSourceArithmeticStillRequiresRealTeacherEvidence()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.Source(12,"32","53","38","49");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.Source(12,"999","53","38","49").Correct);
        Assert.NotEmpty(UsCcssG2Unit4FinalSixSourcePractice.TeacherRubrics[12]);
    }
    [Fact]
    public void IndependentPracticeDoesNotCertifyClassroomArtwork()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(12,"33","66","38","58");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.FirstParty(12,"999","66","38","58").Correct);
    }
}
