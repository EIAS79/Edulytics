using Edulytics.Core.Mathematics.Curriculum;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage C authored-but-NOT-RUN contracts for exact G3 U01 L02 class-dependent
/// questions. Never equate numeric passing checks with real survey provenance.
/// </summary>
public sealed class UsCcssG3Unit1Lesson2ClassSurveyCandidateTests
{
    private static readonly UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount[] ExampleSchoolResponses =
    [
        new("Walk", 6), new("Bike", 3), new("Van", 1),
        new("Bus", 5), new("Car", 2), new("Train", 1)
    ];

    [Fact]
    public void RealClassDataMustBeProvidedRatherThanCopiedFromLesson1()
    {
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            null, out _));
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
            null, "Walk", "Bike", 3, out _));
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            ExampleSchoolResponses, out var classes));
        Assert.Equal(6, classes!["Walk"]);
        Assert.Equal(18, classes.Values.Sum());
    }

    [Theory]
    [InlineData("Walk", "Bike", 3)]
    [InlineData("Bus", "Car", 3)]
    [InlineData("Walk", "Walk", -1)]
    public void OneStepComparisonIsGradedOrRejectedByScope(
        string first, string second, int difference)
    {
        var eligible = UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
            ExampleSchoolResponses, first, second, difference, out var correct);
        if (first == second)
        {
            Assert.False(eligible);
            Assert.False(correct);
        }
        else
        {
            Assert.True(eligible);
            Assert.True(correct);
            Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
                ExampleSchoolResponses, first, second, difference + 1, out var wrong));
            Assert.False(wrong);
        }
    }

    [Fact]
    public void TwoStepClassComparisonAcceptsCorrectAndRejectsIncorrect()
    {
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeTwoStep(
            ExampleSchoolResponses, "Walk", "Bike", "Van", 8, out var correct));
        Assert.True(correct);
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeTwoStep(
            ExampleSchoolResponses, "Walk", "Bike", "Van", 7, out var wrong));
        Assert.False(wrong);
    }

    [Fact]
    public void InvalidClassGraphCannotBeAutoGraded()
    {
        UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount[] duplicates =
            [new("Walk", 3), new("walk", 5)];
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            duplicates, out _));
        UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount[] invalid =
            [new("Walk", -1), new("Bike", 3)];
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            invalid, out _));
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
            ExampleSchoolResponses, "Horse", "Bus", 5, out _));
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeTwoStep(
            ExampleSchoolResponses, "Walk", "Walk", "Bus", 7, out _));
    }

    [Theory]
    [InlineData("collect-real-sticky-note-survey")]
    [InlineData("construct-class-picture-graph")]
    [InlineData("construct-class-bar-graph")]
    [InlineData("explain-true-false-statement")]
    [InlineData("justify-with-partner")]
    [InlineData("revise-math-community-norms")]
    [InlineData("cool-down-open-response")]
    public void TeacherObservedTasksAreNotAutoGraded(string task)
    {
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.RequiresTeacherJudgment(task));
    }
    [Fact]
    public void ARealClassMayHaveThirtyLearnersButNumericProblemsRemainWithinTwenty()
    {
        UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount[] classOfThirty =
        [
            new("Bike", 3), new("Walk", 14), new("Van", 2),
            new("Bus", 5), new("Car", 4), new("Train", 2)
        ];
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            classOfThirty, out var values));
        Assert.Equal(30, values!.Values.Sum());
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
            classOfThirty, "Bus", "Car", 1, out var correct));
        Assert.True(correct);
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeTwoStep(
            classOfThirty, "Walk", "Bus", "Bike", 16, out _));
    }

    [Fact]
    public void AnOverFortySurveyDoesNotProduceAnAutomaticNumericPass()
    {
        UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount[] impossible =
            [new("Walk", 21), new("Bike", 21)];
        Assert.False(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(
            impossible, out _));
    }

    [Fact]
    public void AdaptedTeacherCategoriesCanBeUsedInsteadOfFixedSourceWords()
    {
        UsCcssG3Unit1Lesson2ClassSurveyCandidate.ClassroomCount[] rows =
        [
            new("Bicycle",5), new("Walk",6), new("Scooter",2),
            new("Bus",4), new("Car",3), new("Train",1)
        ];
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGetValidatedCounts(rows,out var counts));
        Assert.Equal(21,counts!.Values.Sum());
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeDifference(
            rows,"Scooter","Bus",2,out var one));
        Assert.True(one);
        Assert.True(UsCcssG3Unit1Lesson2ClassSurveyCandidate.TryGradeTwoStep(
            rows,"Scooter","Bus","Walk",0,out var two));
        Assert.True(two);
    }

}
