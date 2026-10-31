using System;
using System.Collections.Generic;
using System.Linq;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// ALG1 L01 selected Practice wiring only. AUTHORED_NOT_RUN.
/// Coder5's existing paper, rubric, source checks and P3 key tests remain unchanged.
/// Persisted grading equivalence is intentionally pending separate PracticeService lease.
/// </summary>
public sealed class UsCcssAlg1U01L01SelectedPracticeRouteTests
{
    private const string Lesson = "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L01";
    private const string Family = UsCcssAlg1U01L01StatisticalSelectionQuestions.Family;

    [Fact]
    public void SelectedLessonContractIsExactAndOtherAlgebraOneLessonsRemainUnsupported()
    {
        Assert.True(LessonPracticeContractRegistry.TryResolve(Lesson, out var contract));
        Assert.NotNull(contract);
        Assert.Equal(Lesson, contract.LessonCode);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Single(contract.AllowedQuestionFamilies);
        Assert.Equal(Family, contract.AllowedQuestionFamilies[0]);
        Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(Family));
        Assert.False(LessonPracticeContractRegistry.TryResolve(
            "PED:US-CCSS-MATH:HS:TRAD:ALG1:IM-1E:U01:L02", out _));
    }

    [Fact]
    public void ExactGeneratedP3IsSixChoiceAndTeacherReviewBoundaryIsNonRevealing()
    {
        var questions = new ExactSkillContractQuestionEngine().Generate(
            "stage18", Lesson, [Family], ExactSkillQuestionDifficulty.Standard, 4, 31031, []);
        Assert.Equal(4, questions.Count);
        Assert.Equal(4, questions.Select(q => q.ExposureFingerprint).Distinct().Count());
        foreach (var q in questions)
        {
            Assert.Equal(Family, q.Family);
            Assert.Equal("A,C,E", q.CorrectAnswer);
            Assert.True(ExactSkillContractQuestionEngine.Verify(Family, q.Parameters, q.CorrectAnswer));
            Assert.Contains("A.", q.Prompt);
            Assert.Contains("F.", q.Prompt);
            Assert.True(q.Prompt.Contains("commas", StringComparison.OrdinalIgnoreCase));
            Assert.False(q.Prompt.Contains("A,C,E", StringComparison.OrdinalIgnoreCase));
            Assert.False(q.Solution.Contains("A,C,E", StringComparison.OrdinalIgnoreCase));
            Assert.True(q.Prompt.Contains("teacher", StringComparison.OrdinalIgnoreCase));
        }
    }

    [Theory]
    [InlineData("A,C,E")]
    [InlineData("e, a, c")]
    [InlineData(" C , e , A ")]
    public void SourceSetAcceptedWithoutDuplicateChoiceIds(string response)
    {
        var p = new Dictionary<string,int> { ["classSize"] = 22, ["window"] = 0, ["category"] = 1 };
        Assert.True(UsCcssAlg1U01L01StatisticalSelectionQuestions.Verify(Family, p, response));
        Assert.True(UsCcssAlg1U01L01StatisticalSelectionQuestions.GradePersistedAnswer("A,C,E", response));
    }

    [Theory]
    [InlineData("")]
    [InlineData("A,C")]
    [InlineData("A,C,E,F")]
    [InlineData("A,A,C,E")]
    [InlineData("A,C,G")]
    [InlineData("A C E")]
    [InlineData("B,C,E")]
    [InlineData("A,,E")]
    public void IncompleteExtraDuplicateWrongAndMalformedSetsRejected(string response)
    {
        var p = new Dictionary<string,int> { ["classSize"] = 22, ["window"] = 1, ["category"] = 0 };
        Assert.False(UsCcssAlg1U01L01StatisticalSelectionQuestions.Verify(Family, p, response));
        Assert.False(UsCcssAlg1U01L01StatisticalSelectionQuestions.GradePersistedAnswer("A,C,E", response));
    }

    [Fact]
    public void ForeignFamilyAndBadParametersFailClosed()
    {
        var p = new Dictionary<string,int> { ["classSize"] = 22, ["window"] = 2, ["category"] = 2 };
        Assert.False(UsCcssAlg1U01L01StatisticalSelectionQuestions.Verify("unapproved.family", p, "A,C,E"));
        Assert.False(UsCcssAlg1U01L01StatisticalSelectionQuestions.Verify(
            Family, new Dictionary<string,int> { ["classSize"] = 0, ["window"] = 2, ["category"] = 2 }, "A,C,E"));
        Assert.False(UsCcssAlg1U01L01StatisticalSelectionQuestions.GradePersistedAnswer("B,C,E", "A,C,E"));
    }
}
