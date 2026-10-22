using Edulytics.Core.Mathematics.Practice;
namespace Edulytics.Tests.MathematicsIntelligence;
public sealed class G2Ordinal65SourceActivityMathTests
{
    [Fact]
    public void OriginalSourceArithmeticStillRequiresRealTeacherEvidence()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.Source(13,"9","18","76","4");
        Assert.True(r.Correct);
        Assert.True(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.Source(13,"999","18","76","4").Correct);
        Assert.NotEmpty(UsCcssG2Unit4FinalSixSourcePractice.TeacherRubrics[13]);
    }
    [Fact]
    public void IndependentPracticeDoesNotCertifyClassroomArtwork()
    {
        var r=UsCcssG2Unit4FinalSixSourcePractice.FirstParty(13,"21","62","24","49");
        Assert.True(r.Correct);
        Assert.False(r.TeacherReviewNeeded);
        Assert.False(UsCcssG2Unit4FinalSixSourcePractice.FirstParty(13,"999","62","24","49").Correct);
    }
}
