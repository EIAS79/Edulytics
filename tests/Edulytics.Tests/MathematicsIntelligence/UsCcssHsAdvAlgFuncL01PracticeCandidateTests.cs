using Edulytics.Services.Mathematics;
using Xunit;
using Candidate = Edulytics.Services.Mathematics.UsCcssHsAdvAlgFuncL01PracticeCandidate;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// L01 source-task question/assessment candidate only: fixtures AUTHORED_NOT_RUN.
/// No assertion here certifies selected Practice, teacher persistence, or Stage D.
/// </summary>
public sealed class UsCcssHsAdvAlgFuncL01PracticeCandidateTests
{
    [Theory]
    [InlineData(0, "M01")]
    [InlineData(1, "M02")]
    [InlineData(2, "M03")]
    [InlineData(3, "M04")]
    [InlineData(4, "M05")]
    [InlineData(5, "M06")]
    [InlineData(6, "M07")]
    [InlineData(7, "M08")]
    [InlineData(8, "M09")]
    [InlineData(9, "M10")]
    [InlineData(10, "M11")]
    [InlineData(11, "SOURCE_PAINTING")]
    [InlineData(12, "SOURCE_TASK6")]
    [InlineData(13, "SOURCE_TASK1A")]
    [InlineData(14, "SOURCE_TASK1B")]
    [InlineData(15, "M01")]
    public void DeterministicQuestionBankPreservesThirteenCasesAndAddsOriginalSourceProducts(
        int index, string expectedCaseId)
    {
        Assert.True(Candidate.TryGenerate(Candidate.LessonCode, index, out var question));
        Assert.NotNull(question);
        Assert.Equal(Candidate.LessonCode, question!.LessonCode);
        Assert.Equal(expectedCaseId, question.CaseId);
        Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
        Assert.False(string.IsNullOrWhiteSpace(question.ExpectedResponseFormat));
        Assert.Equal(expectedCaseId is "M05" or "M11", question.RequiresTeacherReview);
    }

    [Fact]
    public void WrongLessonNegativeIndexAndUnregisteredCasesFailClosed()
    {
        Assert.False(Candidate.TryGenerate(Candidate.LessonCode, -1, out _));
        Assert.False(Candidate.TryGenerate("PED:US-CCSS-MATH:HS:FOURTH:ADV-ALG-FUNC:U01:L02", 0, out _));
        Assert.False(Candidate.TryGenerate(null, 0, out _));
        Assert.False(Candidate.TryGetQuestion(Candidate.LessonCode, "M12", out _));
        Assert.False(Candidate.TryGetQuestion(Candidate.LessonCode, "m01", out _));
        Assert.False(Candidate.TryGetQuestion("PED:US-CCSS-MATH:G1:U01:L01", "M01", out _));
        Assert.Equal(Candidate.Disposition.WrongLessonOrCase,
            Candidate.Assess("PED:US-CCSS-MATH:G1:U01:L01", "M01",
                "A=[[1,1,1],[2,-1,1],[1,2,-1]];b=[6,3,2]").Status);
        Assert.Equal(Candidate.Disposition.WrongLessonOrCase,
            Candidate.Assess(Candidate.LessonCode, "M12", "[1,2]").Status);
    }

    [Theory]
    [InlineData("M01", "A=[[1,1,1],[2,-1,1],[1,2,-1]];b=[6,3,2]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M02", "A^-1=[[1,-1],[-1,2]];x=[2,1]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M03", "A^-1=[[-1/7,3/7,2/7],[3/7,-2/7,1/7],[5/7,-1/7,-3/7]];x=[1,2,3]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M06", "inconsistent", Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M07", "singular", Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M08", "dimension_mismatch", Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M09", "[2,-1,1]", Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("M10", "not_square", Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_PAINTING",
        "A^-1=[[0.4,0.2,-0.4],[-0.1,0.2,0.1],[0.2,-0.4,0.8]];x=[12,2,6]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_PAINTING",
        "A^-1=[[0.4,0.2,-0.4],[-0.1,0.2,0.1],[0.2,-0.4,0.8]];x=[12,6,2]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK6",
        "A^-1=[[-5/3,2/3,4/3],[1/3,-1/3,1/3],[1,0,-1]];x=[2,4,3]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_TASK6",
        "A^-1=[[-5/3,2/3,4/3],[1/3,-1/3,1/3],[1,0,-1]];x=[2,3,4]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK1A", "[[2,0,1],[1,4,0],[0,2,1]]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_TASK1A", "[[4/2,0,1],[1,4,0],[0,2,1]]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_TASK1A", "[[1,0,0],[0,1,0],[0,0,1]]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK1A", "[[2,0,1],[1,4,0]]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK1B", "[[1,0,0],[0,1,0],[0,0,1]]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_TASK1B", "[[1.0,0,0],[0,1,0],[0,0,1]]",
        Candidate.Disposition.CorrectClosedAnswer)]
    [InlineData("SOURCE_TASK1B", "[[2,0,1],[1,4,0],[0,2,1]]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK1B", "[[1,0,0],[0,1,0],[0,0,1/0]]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK1B", "[[1 0,0],[0,1,0],[0,0,1]]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("SOURCE_TASK1B", "[[1,0,0],[0,1,0],[0,0,1]];x=[1,2,3]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("M02", "x=[2,1]", Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("M07", "invertible", Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("M03", "A^-1=[[1,0],[0,1]];x=[1,2,3]",
        Candidate.Disposition.IncorrectOrMalformed)]
    [InlineData("M05", "x=2-t,y=t", Candidate.Disposition.TeacherReviewRequired)]
    [InlineData("M11", "R2 <- R2 - 2R1 keeps the solutions",
        Candidate.Disposition.TeacherReviewRequired)]
    [InlineData("M01", "Ax=b with A rows (1,1,1), (2,-1,1), (1,2,-1) and b column (6,3,2)",
        Candidate.Disposition.TeacherReviewRequired)]
    [InlineData("M03", "det(A)=7; A^-1=(1/7)*[[-1,3,2],[3,-2,1],[5,-1,-3]]; x=[1,2,3]",
        Candidate.Disposition.TeacherReviewRequired)]
    public void AssessmentDelegatesToExistingExactOracleWithoutScoringProofs(
        string caseId, string answer, Candidate.Disposition expected)
    {
        var assessment = Candidate.Assess(Candidate.LessonCode, caseId, answer);
        Assert.Equal(expected, assessment.Status);
        Assert.False(string.IsNullOrWhiteSpace(assessment.Feedback));
    }

    [Fact]
    public void TeacherProofsGetNoAutomaticSuccessAndInvalidInputIsRejected()
    {
        Assert.Equal(Candidate.Disposition.TeacherReviewRequired,
            Candidate.Assess(Candidate.LessonCode, "M05", "only one solution (2,0)").Status);
        Assert.Equal(Candidate.Disposition.TeacherReviewRequired,
            Candidate.Assess(Candidate.LessonCode, "M11", "R2 <- R2 -").Status);
        Assert.Equal(Candidate.Disposition.IncorrectOrMalformed,
            Candidate.Assess(Candidate.LessonCode, "M11", "<script>bad</script>").Status);
        Assert.Equal(Candidate.Disposition.IncorrectOrMalformed,
            Candidate.Assess(Candidate.LessonCode, "M03", null).Status);
    }
}
