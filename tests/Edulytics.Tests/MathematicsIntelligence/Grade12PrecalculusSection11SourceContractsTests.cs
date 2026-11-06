using Edulytics.Core.Curriculum;
using Edulytics.Services.Mathematics;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Stage D test specification, authored during G12 A-B-C but intentionally NOT RUN.
/// A complete source parity / media / grade12 adoption assertion will be added only
/// when all original 92 exercises, figure tasks, and source subparts are reconciled.
/// </summary>
public sealed class Grade12PrecalculusSection11SourceContractsTests
{
    private const string Code =
        "PED:US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017:U01:L01";

    [Fact]
    public void IndependentPrecalculusBlueprintIsSourceOrderedAndNotGrade11()
    {
        var blueprint = Assert.Single(PedagogicalLessonBlueprintRegistry.LoadEmbeddedDocuments(),
            b => b.BlueprintCode == "US-CCSS-MATH:HS:ADV:PRECALC:OPENSTAX-2017");
        Assert.Equal(2, blueprint.SchemaVersion);
        Assert.Equal("PRECALC", blueprint.CourseCode);
        Assert.Equal("Advanced Precalculus", blueprint.Pathway);
        Assert.Equal(13, blueprint.LogicalLevelFrom);
        Assert.Equal(13, blueprint.LogicalLevelTo);
        Assert.Equal(12, blueprint.Units.Count);
        Assert.Equal(73, blueprint.Lessons.Count);
        Assert.Equal(73, blueprint.Lessons.Select(x => x.SourceLessonCode).Distinct().Count());
        Assert.Equal(Code, blueprint.Lessons[0].LessonCode);
        Assert.Equal("Functions and Function Notation", blueprint.Lessons[0].Title);
        Assert.EndsWith(":U12:L04", blueprint.Lessons[^1].LessonCode,
            StringComparison.Ordinal);
        Assert.Equal(73, blueprint.Lessons.Select(x => x.LessonCode).Distinct().Count());
        Assert.Equal(2, blueprint.Lessons[0].FormalTargets.Count);
        Assert.All(blueprint.Lessons.Skip(1), lesson =>
            Assert.Empty(lesson.FormalTargets)); // Fail-closed: other standards not yet source-reviewed.
    }

    [Fact]
    public void FirstPartySection11ContentHasFifteenDistinctMathematicalExamples()
    {
        var lesson = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(lesson);
        Assert.Equal("Functions and Function Notation", lesson.Title);
        Assert.Equal(15, lesson.WorkedExamples.Count);
        Assert.True(lesson.KeyConcepts.Count >= 8);
        Assert.Equal(5, lesson.Visuals.Count);
        Assert.Contains(lesson.Visuals, visual => visual.Title.Contains("Vertical-line test",
            StringComparison.Ordinal));
        Assert.Contains(lesson.Visuals, visual => visual.Title.Contains("Horizontal-line test",
            StringComparison.Ordinal));
        Assert.All(lesson.WorkedExamples, example =>
        {
            Assert.True(example.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(example.Question));
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        });
    }

    [Fact]
    public void ExistingFunctionQuestionFamiliesAreReusedOnlyForTheirActualSkills()
    {
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(
            UsCcssAlg1Unit4FunctionQuestions.FunctionDomainRangeFamily));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(
            UsCcssAlg1Unit4FunctionQuestions.FunctionAbsoluteTranslateFamily));

        // A generated numeric notation question is NOT proof of graph
        // classification or a manually checked line-test drawing.
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Supports(
            "g12.prectrig.graph.vertical_horizontal_line_teacher_review"));
    }

    [Fact]
    public void IndependentWorkedExamplesHaveCorrectMathGoldenValues()
    {
        Assert.Equal(6, 2 * 2 + 3 * 2 - 4);
        Assert.Equal(24, 4 * 4 + 2 * 4);
        Assert.Equal(3, (-3) * (-3) + 2 * (-3));
        Assert.Equal(3, 1 * 1 + 2 * 1);
        Assert.Equal(4, (-1) * (-1) - 2 * (-1) + 1);
        Assert.Equal(0.0, 2 * 3 + 6 * (2 - 3.0 / 3) - 12);
    }
}
