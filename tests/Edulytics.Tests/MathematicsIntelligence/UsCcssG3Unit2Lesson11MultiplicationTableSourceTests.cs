using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Source-specific authoring contracts for deferred Stage D. NOT RUN.</summary>
public sealed class UsCcssG3Unit2Lesson11MultiplicationTableSourceTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U02:L11";
    private static Grade3CompanionLesson Lesson()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code, out var lesson));
        return Assert.IsType<Grade3CompanionLesson>(lesson);
    }

    [Fact]
    public void AllTableBoundedFactorPairsMatchAnIndependentAreaOracle()
    {
        var q = Lesson().SourceBasedCheck!;
        Assert.Equal("rectangle-dimensions", q.AnswerType);
        Assert.Null(q.Expected);
        Assert.Equal(24, q.DimensionConstraint!.TargetArea);
        Assert.Equal(9, q.DimensionConstraint.MaximumSide);
        var accepted = new List<string>();
        for (var rows = 1; rows <= 12; rows++)
        for (var columns = 1; columns <= 12; columns++)
        {
            // Source constants are independent of the question configuration.
            var wanted = rows <= 9 && columns <= 9 && rows * columns == 24;
            var answer = $"{rows},{columns}";
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Equal(wanted, result.NumericCorrect);
            Assert.True(result.TeacherReview);
            if (result.NumericCorrect == true) accepted.Add(answer);
        }
        Assert.Equal(new[] { "3,8", "4,6", "6,4", "8,3" }, accepted);
    }

    [Theory]
    [InlineData(" 3 , 8 ", true)]
    [InlineData("24", false)]
    [InlineData("2,12", false)]
    [InlineData("1,24", false)]
    [InlineData("0,24", false)]
    [InlineData("-3,-8", false)]
    [InlineData("3.0,8", false)]
    [InlineData("3/1,8", false)]
    [InlineData("3x8", false)]
    [InlineData("3,8,24", false)]
    [InlineData("3,8 trailing", false)]
    [InlineData("999999999999999,8", false)]
    [InlineData("", false)]
    public void DimensionInputRequiresBothWholeNumberSidesWithinTheTable(string answer, bool correct)
    {
        var result = Grade3SourceCompanionCatalogue.Grade(Lesson().SourceBasedCheck!, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void MissingOrInvalidRectangleConfigurationCannotAwardCredit()
    {
        foreach (var rule in new Grade3DimensionConstraint?[] {
            null, new() { TargetArea = 0, MaximumSide = 9 },
            new() { TargetArea = 24, MaximumSide = 0 },
            new() { TargetArea = 24, MaximumSide = 1000 },
            new() { TargetArea = 1000001, MaximumSide = 999 } })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(
                new() { AnswerType = "rectangle-dimensions", DimensionConstraint = rule }, "3,8");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }

    [Fact]
    public void GrowingArraysHaveDistinctTotalsAndTeacherOnlyReasoning()
    {
        var lesson = Lesson();
        Assert.Equal(12, lesson.SourceSubtasks.Count);
        var counts = new[] { 8, 12, 16 };
        for (var i = 0; i < counts.Length; i++)
        {
            var q = lesson.SourceSubtasks.Single(q => q.Id.EndsWith($"WU-{i + 1}"));
            Assert.True(Grade3SourceCompanionCatalogue.Grade(q, counts[i].ToString()).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(q, (counts[i] + 1).ToString()).NumericCorrect);
        }
        foreach (var q in lesson.SourceSubtasks.Where(q => q.AnswerType == "teacher"))
        foreach (var answer in new[] { "24", "3,8", "15", "" })
        {
            var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
        Assert.Contains("every cell", lesson.TeacherReviewNote);
        Assert.Contains("lower-right", lesson.TeacherReviewNote);
        Assert.True(lesson.OriginalStudentTaskPdfRead);
        Assert.True(lesson.ReadyForStageD); // Authored A–C, not runtime acceptance.
    }

    [Theory]
    [InlineData("3x5=15;5x3=15", true)]
    [InlineData("15=5×3;15=3×5", true)]
    [InlineData("15", false)]
    [InlineData("3x5=15;3x5=15", false)]
    [InlineData("3x5=15;5x3=16", false)]
    public void ACheckedPatternRequiresTwoCompleteReversedFactorEquations(string answer, bool correct)
    {
        var q = Lesson().SourceSubtasks.Single(q => q.Id.EndsWith("A2-COMMUTE"));
        var result = Grade3SourceCompanionCatalogue.Grade(q, answer);
        Assert.Equal(correct, result.NumericCorrect);
        Assert.True(result.TeacherReview);
    }

    [Fact]
    public void PracticeAndIndependentCoolDownHaveDifferentRectanglesAndProducts()
    {
        var lesson = Lesson();
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice!, "24").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.IndependentPractice!, "28").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown!, "28").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown!, "24").NumericCorrect);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.OriginalEdulyticsCoolDown!, "28").TeacherReview);
    }

    [Fact]
    public void CanonicalMaterializationRepairsGenericDataDisplayTeachingAndPreservesGuards()
    {
        var assembly = typeof(CanonicalLessonContentPackDocument).Assembly;
        var resource = assembly.GetManifestResourceNames().Single(n => n.EndsWith(
            "us-ccss-math-g3-phase29-v1.lesson-content-pack.json", StringComparison.Ordinal));
        using var stream = assembly.GetManifestResourceStream(resource)!;
        var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var document = JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream, options)!;
        var lesson = document.Lessons.Single(l => l.LessonCode == Code);
        Assert.True(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        CommonCoreG3SourceTeachingCorrections.Apply(document);
        var en = lesson.Translations.Single(t => t.CultureCode == "en");
        Assert.Contains("upper-left DATA square", en.WorkedExamples);
        Assert.Contains("3 by 8", en.WorkedExamples);
        Assert.Contains("4 by 6", en.WorkedExamples);
        Assert.Contains("12 + 4 = 16", en.StepByStepSolutions);
        Assert.Contains("Completing every cell is not required", en.Explanation);
        Assert.Contains("MLR8", en.Explanation);
        Assert.Contains("28 square units", en.WorkedExamples);
        Assert.DoesNotContain("Read the title, labels, categories, and scale", en.StepByStepSolutions);
        lesson.SourceEdition = "2nd Edition";
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(document, lesson));
        Assert.Throws<InvalidOperationException>(() => CommonCoreG3SourceTeachingCorrections.Apply(document));
    }
}
