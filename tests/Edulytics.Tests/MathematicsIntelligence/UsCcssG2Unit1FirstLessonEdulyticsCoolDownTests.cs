using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C test authoring only. Not executed before Stage D.
/// </summary>
public sealed class UsCcssG2Unit1FirstLessonEdulyticsCoolDownTests
{
    [Fact]
    public void IndependentlyAuthoredExitTicketContainsFourAssessableTasks()
    {
        Assert.Equal("PED:US-CCSS-MATH:G2:U01:L01",
            UsCcssG2Unit1FirstLessonEdulyticsCoolDown.LessonCode);
        Assert.Equal(4,UsCcssG2Unit1FirstLessonEdulyticsCoolDown.Tasks.Count);
        Assert.Equal(new[]{"CD-1","CD-2","CD-3","CD-4"},
            UsCcssG2Unit1FirstLessonEdulyticsCoolDown.Tasks.Select(x=>x.Id));
        Assert.Equal(4,UsCcssG2Unit1FirstLessonEdulyticsCoolDown.ExplanationRubric.Count);
    }

    [Theory]
    [InlineData("CD-1","9",true)]
    [InlineData("CD-1","8",false)]
    [InlineData("CD-2","4",true)]
    [InlineData("CD-2","6",false)]
    [InlineData("CD-2","invalid",false)]
    public void NumericCoolDownTasksUseCorrectWithinTenArithmetic(
        string id,string answer,bool correct)
    {
        var outcome=UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessInteger(id,answer);
        Assert.True(outcome.ScorableByMachine);
        Assert.Equal(correct,outcome.Correct);
    }

    [Fact]
    public void MultipleCorrectExpressionsAndEquivalentFormsAreAccepted()
    {
        var pass=UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessTwoExpressions("CD-3",new[]{"10 - 4","3 + 3"});
        Assert.True(pass.Correct);
        Assert.True(pass.TeacherReviewRequired);
        Assert.True(UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessTwoExpressions("CD-3",new[]{"1 + 5","4 + 2"}).Correct);
        Assert.False(UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessTwoExpressions("CD-3",new[]{"4 + 2","4+2"}).Correct);
        Assert.False(UsCcssG2Unit1FirstLessonEdulyticsCoolDown
            .AssessTwoExpressions("CD-3",new[]{"5 + 2","10 - 4"}).Correct);
    }

    [Fact]
    public void OpenExplanationIsTeacherScoredNotFalselyNumeric()
    {
        var result=UsCcssG2Unit1FirstLessonEdulyticsCoolDown.AssessInteger("CD-4","10");
        Assert.False(result.ScorableByMachine);
        Assert.True(result.TeacherReviewRequired);
    }
}
