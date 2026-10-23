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
}
