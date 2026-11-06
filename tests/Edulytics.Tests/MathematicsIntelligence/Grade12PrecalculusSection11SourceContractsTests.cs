using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;
using Edulytics.Core.Mathematics.Visuals;
using Edulytics.Web.Presentation;

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
    public void FirstPartySection11ContentHasSixteenMathematicallyDistinctExamples()
    {
        var lesson = RichLessonContentV2Registry.Find(Code, "en");
        Assert.NotNull(lesson);
        Assert.Equal("Functions and Function Notation", lesson.Title);
        Assert.Equal(16, lesson.WorkedExamples.Count);
        Assert.True(lesson.KeyConcepts.Count >= 8);
        Assert.Equal(7, lesson.Visuals.Count);
        Assert.Contains(lesson.Visuals, visual => visual.Title.Contains("Vertical-line test",
            StringComparison.Ordinal));
        Assert.Contains(lesson.Visuals, visual =>
            visual.Kind == "RatioTable" &&
            visual.Title == "Evaluate and solve from a displayed input-output table" &&
            visual.Rows.Count == 2 &&
            visual.Rows[0].Cells.Count == 4);
        Assert.Contains(lesson.Visuals, visual => visual.Title.Contains("Horizontal-line test",
            StringComparison.Ordinal));
        // Regression for the source exercise 46 error: separate horizontal
        // rays with overlapping x-domains can fail the vertical-line test.
        Assert.Contains(lesson.WorkedExamples, example =>
            example.Title == "Two horizontal rays with overlapping input domains" &&
            example.Answer.Contains("not a function", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(lesson.Visuals, visual =>
            visual.Title == "Vertical-line test of overlapping horizontal rays" &&
            visual.Items.Any(item => item.Contains("0<x<2", StringComparison.Ordinal)));
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
    public void Grade12PartialPracticeIncludesExactFiniteRelationInjectivityFamily()
    {
        Assert.True(LessonPracticeCapabilityResolver.TryResolve(Code, out var contract));
        Assert.NotNull(contract);
        Assert.Equal("READY_VERIFIED", contract.Readiness);
        Assert.Equal(new[] { "supporting.functions.core", "functions.evaluate.exact" },
            contract.SkillIds);
        Assert.Equal(new[]
        {
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionOneToOneFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionDomainRangeFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionAbsoluteTranslateFamily
        }, contract.AllowedQuestionFamilies);
        Assert.DoesNotContain(contract.AllowedQuestionFamilies,
            family => family.Contains("graph", StringComparison.OrdinalIgnoreCase));
        Assert.Equal(1, contract.AllowedQuestionFamilies.Count(
            family => family.Contains("one_to_one", StringComparison.OrdinalIgnoreCase)));
    }

    [Fact]
    public void Grade12ReuseExactPositiveWrongAndInvalidAnswers()
    {
        var relation = new Dictionary<string, int>
        {
            ["x"] = 2, ["a"] = 5, ["b"] = 8, ["repeats"] = 1, ["variant"] = 0
        };
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily, relation, "no"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionRelationFamily, relation, "yes"));
        var notation = new Dictionary<string, int>
        {
            ["initial"] = 20, ["rate"] = 9, ["input"] = 2, ["variant"] = 0
        };
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, notation, "38"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, notation, "37"));
        notation.Remove("rate");
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(
            UsCcssAlg1Unit4FunctionQuestions.FunctionNotationFamily, notation, "38"));
    }

    [Fact]
    public void FiniteRelationOneToOneFamilyGradingIsExactAndFailsClosed()
    {
        const string family = UsCcssAlg1Unit4FunctionQuestions.FunctionOneToOneFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(family));
        var notInjective = new Dictionary<string, int>
        {
            ["x0"] = -1, ["y0"] = 3, ["step"] = 2, ["injective"] = 0
        };
        Assert.Equal("no", UsCcssAlg1Unit4FunctionQuestions.Solve(family, notInjective));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(family, notInjective, "no"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family, notInjective, "yes"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family, notInjective, "invalid"));
        var injective = new Dictionary<string, int>(notInjective)
        {
            ["injective"] = 1
        };
        Assert.Equal("yes", UsCcssAlg1Unit4FunctionQuestions.Solve(family, injective));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(family, injective, "yes"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family, injective, "no"));
        injective["step"] = 0; // Distinct output guarantee removed; fail closed.
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(family, injective, "yes"));
        // This tests ordered-pair injectivity, NOT graphical one-to-one proof.
    }

    [Fact]
    public void Grade12FirstPartyFunctionGraphsRenderWithAccessibleOpenEndpoints()
    {
        var visualSpecs = Grade12PrecalculusSection11Visuals.Build();
        Assert.Equal(4, visualSpecs.Count);
        Assert.All(visualSpecs, MathematicalVisualSpecValidator.Validate);
        var horizontalRays = Assert.IsType<CoordinatePlaneVisualSpec>(visualSpecs[3]);
        Assert.Equal(2, horizontalRays.OpenPoints.Count);
        var svg = AdvancedMathematicsVisualRenderer.RenderSvg(horizontalRays);
        Assert.Equal(2, svg.Split("class='amv-open-point'").Length - 1);
        Assert.Contains("At x=1", horizontalRays.Accessibility.Description,
            StringComparison.Ordinal);
        Assert.Contains("aria-labelledby", svg, StringComparison.Ordinal);
        // Renderable teaching graphs do not certify original figure parity.
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
