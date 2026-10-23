using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C contracts are written, deliberately not executed.</summary>
public sealed class Grade3SourceCompanionGradingTests
{
    [Fact]
    public void Only135StageCOriginalCompanionCandidatesRegistered()
    {
        Assert.Equal(135, Grade3SourceCompanionCatalogue.Codes.Count);
        Assert.False(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U01:L08", out _));
        Assert.True(Grade3SourceCompanionCatalogue.TryGet("PED:US-CCSS-MATH:G3:U01:L09", out var l));
        Assert.Equal(9,l!.Ordinal);
        Assert.False(l.ReadyForStageD);
    }

    [Theory]
    [InlineData("integer","12","12","13",true)]
    [InlineData("integer","5","5","6",true)]
    [InlineData("comparison",">",">","<",true)]
    [InlineData("comparison","=","=","<",true)]
    [InlineData("rational","2/4","1/2","3/4",true)]
    [InlineData("rational","7/4","14/8","1/4",true)]
    public void EligibleNumbersAndEquivalentFractionsAreCheckedWithoutCertifyingReasoning(
        string kind,string expected,string good,string bad,bool review)
    {
        var q = new Grade3OriginalQuestion{Prompt="Test",AnswerType=kind,Expected=expected};
        var yes=Grade3SourceCompanionCatalogue.Grade(q,good);
        var no=Grade3SourceCompanionCatalogue.Grade(q,bad);
        Assert.True(yes.NumericCorrect);
        Assert.False(no.NumericCorrect);
        Assert.Equal(review,yes.TeacherReview);
        Assert.True(no.TeacherReview);
    }
    [Fact]
    public void TeacherOnlyTasksNeverYieldNumericPasses()
    {
        var q=new Grade3OriginalQuestion{AnswerType="teacher",Prompt="Draw a graph."};
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(q,"10").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(q,"10").TeacherReview);
    }
    [Fact]
    public void FractionParserRejectsZeroDenominatorAndOutOfScopeNumbers()
    {
        var q=new Grade3OriginalQuestion{AnswerType="rational",Expected="3/4"};
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"1/0").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(q,"100000000/4").NumericCorrect);
    }
    [Theory]
    [InlineData(9,6)]
    [InlineData(10,6)]
    [InlineData(11,12)]
    [InlineData(12,15)]
    [InlineData(13,10)]
    [InlineData(14,9)]
    [InlineData(15,3)]
    [InlineData(16,30)]
    [InlineData(17,15)]
    [InlineData(18,15)]
    [InlineData(19,35)]
    [InlineData(20,10)]
    public void SourceAlignedNumericalCheckDoesNotProveOriginalDiagramParity(int number, int expected)
    {
        var lessonCode=$"PED:US-CCSS-MATH:G3:U01:L{number:00}";
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(lessonCode,out var entry));
        Assert.NotNull(entry!.SourceBasedCheck);
        Assert.True(entry.ReviewedStudentFacingText);
        Assert.False(entry.SourceBasedCheckProvenance!.FigurePixelsInspected);
        var yes=Grade3SourceCompanionCatalogue.Grade(entry.SourceBasedCheck!, expected.ToString());
        var no=Grade3SourceCompanionCatalogue.Grade(entry.SourceBasedCheck!, (expected+1).ToString());
        Assert.True(yes.NumericCorrect);
        Assert.False(no.NumericCorrect);
        Assert.True(yes.TeacherReview);
    }

    [Fact]
    public void EverySourceReviewedCompanionRetainsThreeScopedContractsAndTeacherBoundary()
    {
        Assert.Equal(135, Grade3SourceCompanionCatalogue.Codes.Count);
        foreach (var code in Grade3SourceCompanionCatalogue.Codes)
        {
            Assert.True(Grade3SourceCompanionCatalogue.TryGet(code, out var lesson));
            Assert.NotNull(lesson);
            Assert.NotNull(lesson!.SourceBasedCheck);
            Assert.NotNull(lesson.IndependentPractice);
            Assert.NotNull(lesson.OriginalEdulyticsCoolDown);
            Assert.NotNull(lesson.SourceBasedCheckProvenance);
            Assert.False(lesson.SourceBasedCheckProvenance!.FigurePixelsInspected);
            Assert.True(lesson.ReviewedStudentFacingText);
            Assert.False(lesson.ReadyForStageD);

            foreach (var question in new[] { lesson.SourceBasedCheck!,
                       lesson.IndependentPractice!, lesson.OriginalEdulyticsCoolDown! })
            {
                Assert.False(string.IsNullOrWhiteSpace(question.Prompt));
                Assert.Contains(question.AnswerType, new[]
                    { "teacher", "integer", "rational", "comparison" });
                if (question.AnswerType == "teacher")
                {
                    Assert.Null(question.Expected);
                    var review = Grade3SourceCompanionCatalogue.Grade(question, "42");
                    Assert.Null(review.NumericCorrect);
                    Assert.True(review.TeacherReview);
                    continue;
                }

                Assert.False(string.IsNullOrWhiteSpace(question.Expected));
                var valid=Grade3SourceCompanionCatalogue.Grade(question,question.Expected);
                Assert.True(valid.NumericCorrect);
                Assert.True(valid.TeacherReview);
                var malformed=Grade3SourceCompanionCatalogue.Grade(question,"not-an-answer");
                Assert.False(malformed.NumericCorrect);
                Assert.True(malformed.TeacherReview);

                string wrong=question.AnswerType switch
                {
                    "integer" => (int.Parse(question.Expected!) + 1).ToString(),
                    "rational" => "1/0",
                    "comparison" => question.Expected == ">" ? "<" : ">",
                    _ => throw new InvalidOperationException("Unexpected type")
                };
                var rejected=Grade3SourceCompanionCatalogue.Grade(question,wrong);
                Assert.False(rejected.NumericCorrect);
                Assert.True(rejected.TeacherReview);
            }
        }
    }

    [Fact]
    public void NoSourceAlignedQuestionForEarlierDedicatedLessonsIsDuplicatedInGenericBank()
    {
        for (var lessonNumber=1; lessonNumber<=8; lessonNumber++)
        {
            Assert.False(Grade3SourceCompanionCatalogue.TryGet(
                $"PED:US-CCSS-MATH:G3:U01:L{lessonNumber:00}",out _));
        }
    }

}
