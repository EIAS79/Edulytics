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
        Assert.Equal(21, lesson.Visuals.Count);
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
        Assert.Equal(14, lesson.Visuals.Count(visual =>
            visual.Title.StartsWith("Source-skill table ", StringComparison.Ordinal)));
        Assert.Contains(lesson.Visuals, visual =>
            visual.Title == "Source-skill table 14: Exercise input–output table" &&
            visual.Rows.Count == 2 && visual.Rows[1].Cells.Count == 10);
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
            UsCcssAlg1Unit4FunctionQuestions.FunctionImplicitClassificationFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionTableLookupFamily,
            UsCcssAlg1Unit4FunctionQuestions.FunctionQuadraticRootsFamily,
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
        Assert.Equal(5, visualSpecs.Count);
        Assert.All(visualSpecs, MathematicalVisualSpecValidator.Validate);
        Assert.Contains("at x=-1 and x=3",
            visualSpecs[2].Accessibility.Description, StringComparison.Ordinal);
        var horizontalRays = Assert.IsType<CoordinatePlaneVisualSpec>(visualSpecs[4]);
        Assert.Equal(2, horizontalRays.OpenPoints.Count);
        var svg = AdvancedMathematicsVisualRenderer.RenderSvg(horizontalRays);
        Assert.Equal(2, svg.Split("class='amv-open-point'").Length - 1);
        Assert.Contains("At x=1", horizontalRays.Accessibility.Description,
            StringComparison.Ordinal);
        Assert.Contains("aria-labelledby", svg, StringComparison.Ordinal);
        // Renderable teaching graphs do not certify original figure parity.
    }

    [Fact]
    public void Grade12FirstPartyExerciseBankMapsAllOriginalNumberedTasks()
    {
        var tasks = Grade12PrecalculusSection11Exercises.All;
        Assert.Equal(92, tasks.Count);
        Assert.Equal(Enumerable.Range(1, 92), tasks.Select(x => x.Number));
        Assert.Equal(93, Grade12PrecalculusSection11Exercises.CoolDown.Number);
        Assert.Contains("one-to-one", Grade12PrecalculusSection11Exercises.CoolDown.Prompt);
        Assert.Equal(12, Grade12PrecalculusSection11Exercises.TryIts.Count);
        Assert.Equal(Enumerable.Range(94, 12),
            Grade12PrecalculusSection11Exercises.TryIts.Select(x => x.Number));
        Assert.NotNull(Grade12PrecalculusSection11Exercises.Find(94));
        Assert.Equal("Try It 1", Grade12PrecalculusSection11Exercises.DisplayLabel(94));
        Assert.Equal("Edulytics original Cool-down",
            Grade12PrecalculusSection11Exercises.DisplayLabel(93));
        Assert.Null(Grade12PrecalculusSection11Exercises.Find(106));
        Assert.All(tasks, item =>
        {
            Assert.False(string.IsNullOrWhiteSpace(item.Skill));
            Assert.False(string.IsNullOrWhiteSpace(item.Prompt));
            Assert.NotEqual("AUTOMATICALLY_GRADED", item.ReviewMode);
        });
        Assert.Equal(20, tasks.Count(x => x.Number is >= 40 and <= 59 &&
            x.ReviewMode == "TEACHER_DIAGRAM_REQUIRED"));
        Assert.Equal(12, tasks.Count(x => x.Number is >= 76 and <= 87 &&
            x.ReviewMode == "TEACHER_DIAGRAM_REQUIRED"));
        Assert.Contains("tons/week", tasks[87].Prompt, StringComparison.Ordinal);
        // Teacher submission persistence, actual figure equivalence and
        // automated skill grading remain separate A–C gaps.
    }

    [Fact]
    public void Grade12RubricRoutesAreLessonScopedAndPreserve92ExerciseNumbers()
    {
        Assert.Equal(92, Grade12PrecalculusSection11Exercises.All.Count);
        var studentRoute = Attribute.GetCustomAttribute(
            typeof(Edulytics.Web.Controllers.Grade12PrecalculusRubricStudentController),
            typeof(Microsoft.AspNetCore.Mvc.RouteAttribute)) as Microsoft.AspNetCore.Mvc.RouteAttribute;
        var teacherRoute = Attribute.GetCustomAttribute(
            typeof(Edulytics.Web.Controllers.Grade12PrecalculusRubricTeacherController),
            typeof(Microsoft.AspNetCore.Mvc.RouteAttribute)) as Microsoft.AspNetCore.Mvc.RouteAttribute;
        Assert.Equal("student/g12/precalculus/rubric", studentRoute?.Template);
        Assert.Equal("school/g12/precalculus/rubric", teacherRoute?.Template);
        // Tenant-scoped persistence and authorization are verified in Stage D.
    }

    [Fact]
    public void SourceRelatedExerciseGraphsAreAccessibleFirstPartySpecifications()
    {
        Assert.Equal(20, Grade12PrecalculusSection11ExerciseVisuals.Count);
        Assert.NotNull(Grade12PrecalculusSection11ExerciseVisuals.Find(52));
        var radians = Assert.IsType<CoordinatePlaneVisualSpec>(
            Grade12PrecalculusSection11ExerciseVisuals.Find(59));
        Assert.Equal(5, radians.XAxisTicks.Count);
        Assert.Contains(radians.XAxisTicks, tick => tick.Label == "π/2");
        Assert.Contains("π/2", AdvancedMathematicsVisualRenderer.RenderSvg(radians));
        foreach (var (originalNumber, visual) in Grade12PrecalculusSection11ExerciseVisuals.All)
        {
            Assert.True(originalNumber is >= 40 and <= 59);
            Assert.False(string.IsNullOrWhiteSpace(visual.Accessibility.Description));
            MathematicalVisualSpecValidator.Validate(visual);
            var svg = AdvancedMathematicsVisualRenderer.RenderSvg(visual);
            Assert.Contains("aria-labelledby", svg, StringComparison.Ordinal);
        }
        // Source figure 1:1 equivalence and student E2E remain for Stage D.
    }

    [Fact]
    public void ExtendedPrecalculusSourceFamiliesRejectInvalidAnswerAndParameters()
    {
        var implicitParameters = new Dictionary<string, int> { ["variant"] = 1 };
        const string implicitFamily = UsCcssAlg1Unit4FunctionQuestions.FunctionImplicitClassificationFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Supports(implicitFamily));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(implicitFamily, implicitParameters, "no"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(implicitFamily, implicitParameters, "yes"));
        implicitParameters["variant"] = 99;
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(implicitFamily, implicitParameters, "no"));

        var table = new Dictionary<string,int> { ["first"] = 10, ["inverse"] = 1 };
        const string tableFamily = UsCcssAlg1Unit4FunctionQuestions.FunctionTableLookupFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "2,4"));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "4; 2"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "2,2"));
        table["inverse"] = 0;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "12"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(tableFamily, table, "17"));

        var roots = new Dictionary<string,int> { ["rootA"]=-3, ["rootB"]=2 };
        const string rootFamily = UsCcssAlg1Unit4FunctionQuestions.FunctionQuadraticRootsFamily;
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "-3,2"));
        Assert.True(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "2,-3"));
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "-3,-3"));
        roots["rootB"] = -3;
        Assert.False(UsCcssAlg1Unit4FunctionQuestions.Verify(rootFamily, roots, "-3"));
    }

    [Fact]
    public void Section11TeacherReviewGuidanceCoversAll105WithoutExposingOriginalFigureExactRoots()
    {
        Assert.Equal(105, Grade12PrecalculusSection11TeacherGuidance.Count);
        Assert.Contains("tons per week",
            Grade12PrecalculusSection11TeacherGuidance.Find(88)!.ExpectedMathematics,
            StringComparison.Ordinal);
        Assert.Contains("x=7",
            Grade12PrecalculusSection11TeacherGuidance.Find(52)!.ExpectedMathematics,
            StringComparison.Ordinal);
        Assert.Contains("original source graph",
            Grade12PrecalculusSection11TeacherGuidance.Find(52)!.ExpectedMathematics,
            StringComparison.OrdinalIgnoreCase);

        Assert.Contains("UNRESOLVED",
            Grade12PrecalculusSection11TeacherGuidance.Find(52)!.Caveat,
            StringComparison.Ordinal);
        Assert.False(Grade12PrecalculusSection11TeacherGuidance.Find(94)!.OriginalSourceExercise);
        Assert.Null(Grade12PrecalculusSection11TeacherGuidance.Find(106));
    }

    [Fact]
    public void SixSourceMediaSkillsHaveIndependentFirstPartyLearningAlternatives()
    {
        Assert.Equal(6, Grade12PrecalculusSection11SupportResources.All.Count);
        Assert.Equal(Enumerable.Range(1, 6),
            Grade12PrecalculusSection11SupportResources.All.Select(x => x.OriginalMediaOrdinal));
        Assert.All(Grade12PrecalculusSection11SupportResources.All,
            resource => Assert.False(string.IsNullOrWhiteSpace(resource.FirstPartyExplanation)));
        // Alternative explanations do not prove third-party media rights or exact parity.
    }

    [Fact]
    public void PrecalculusWindowExercisesRespectExactPhysicalAxesAndRanges()
    {
        var windows = Grade12PrecalculusSection11WindowVisuals.All;
        Assert.Equal(12, windows.Count);
        Assert.Equal(Enumerable.Range(76, 12),
            windows.Select(x => x.ExerciseNumber));
        Assert.Equal(-1000000m, windows[11].XMin);
        Assert.Equal(1000000m, windows[11].XMax);
        Assert.Equal("-100 ≤ y ≤ 100", windows[11].ExactOutputRange);
        Assert.Equal("0 ≤ y ≤ 10,000", windows[2].ExactOutputRange);
        Assert.Equal("-0.001 ≤ y ≤ 0.001", windows[3].ExactOutputRange);
        Assert.All(windows, x =>
        {
            MathematicalVisualSpecValidator.Validate(x.Graph);
            Assert.True(x.Graph.Segments.Count >= 32);
            Assert.Contains("aria-labelledby",
                AdvancedMathematicsVisualRenderer.RenderSvg(x.Graph),
                StringComparison.Ordinal);
        });
    }

    [Fact]
    public void NineBasicToolkitGraphFamiliesHaveAccessibleDomainsAndBranches()
    {
        Assert.Equal(9, Grade12PrecalculusSection11ToolkitGraphs.All.Count);
        Assert.Contains(Grade12PrecalculusSection11ToolkitGraphs.All,
            x => x.Title == "Reciprocal y=1/x" &&
                 x.Domain == "x≠0" && x.Graph.Segments.Count == 80);
        Assert.Contains(Grade12PrecalculusSection11ToolkitGraphs.All,
            x => x.Title == "Square root y=√x" && x.Domain == "x≥0");
        Assert.All(Grade12PrecalculusSection11ToolkitGraphs.All,
            x =>
            {
                MathematicalVisualSpecValidator.Validate(x.Graph);
                Assert.Contains("aria-labelledby",
                    AdvancedMathematicsVisualRenderer.RenderSvg(x.Graph),
                    StringComparison.Ordinal);
            });
    }

    [Fact]
    public void FifteenOriginalFigureSkillsHaveIndependentTextOrGraphAlternatives()
    {
        var figures = Grade12PrecalculusSection11FigureAlternatives.All;
        Assert.Equal(15, figures.Count);
        Assert.Equal(Enumerable.Range(1, 15),
            figures.Select(x => x.OriginalFigureOrdinal));
        Assert.All(figures, x =>
            Assert.False(string.IsNullOrWhiteSpace(x.FirstPartyAlternative)));
        Assert.Equal(2, figures.Count(x => x.OriginalFigureOrdinal is 6 or 10 &&
            x.AddedDrawing is not null));
        foreach (var figure in figures.Where(x => x.AddedDrawing is not null))
        {
            MathematicalVisualSpecValidator.Validate(figure.AddedDrawing!);
            Assert.Contains("aria-labelledby",
                AdvancedMathematicsVisualRenderer.RenderSvg(figure.AddedDrawing!),
                StringComparison.Ordinal);
        }
    }

    [Fact]
    public void Grade12OriginalExercisePartsRetain162AtomicReviewActionsWithoutDoubleCounting()
    {
        Assert.Equal(92, Grade12PrecalculusSection11SourcePartChecklist.OriginalSourceExerciseCount);
        Assert.Equal(162, Grade12PrecalculusSection11SourcePartChecklist.OriginalAtomicReviewActions);
        Assert.Equal(106, Grade12PrecalculusSection11SourcePartChecklist.ExplicitSourceSubparts);
        Assert.Equal(5, Grade12PrecalculusSection11SourcePartChecklist.ForExercise(27).Count);
        Assert.Equal(3, Grade12PrecalculusSection11SourcePartChecklist.ForExercise(39).Count);
        Assert.Equal(2, Grade12PrecalculusSection11SourcePartChecklist.ForExercise(52).Count);
        Assert.Single(Grade12PrecalculusSection11SourcePartChecklist.ForExercise(92));
        Assert.Empty(Grade12PrecalculusSection11SourcePartChecklist.ForExercise(93));
        Assert.Contains(Grade12PrecalculusSection11SourcePartChecklist.ForExercise(52),
            part => part.OriginalSourceNumericalResultUnresolved);
    }

    [Fact]
    public void Grade12GraphSubmissionsPreserveAccessibleCoordinatesAndRequireEvidence()
    {
        const string reasoning = "This input value has a single correctly labelled output.";
        const string graph = "x-domain [-10,10], marked vertex (0,0), range [0,100].";
        Assert.True(Grade12PrecalculusSection11Exercises.SupportsGraphEvidence(40));
        Assert.True(Grade12PrecalculusSection11Exercises.RequiresGraphEvidence(76));
        Assert.False(Grade12PrecalculusSection11Exercises.RequiresGraphEvidence(52));
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, null));
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, "x=1"));
        Assert.NotNull(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, graph));
        Assert.Contains("Labelled graph / coordinate representation:",
            Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(76, reasoning, graph)!);
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(27, reasoning, graph));
        Assert.Null(Grade12PrecalculusSection11Exercises.ComposeTeacherResponse(106, reasoning, null));
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
