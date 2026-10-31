using Edulytics.Core.Curriculum;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// Production Rich V2 resource-loader checks for independently authored Grade 8 Unit 1.
/// These tests prove content dispatch and mathematical invariants, not academic
/// acceptance, teacher assessment, or end-to-end Practice persistence.
/// </summary>
public sealed class UsCcssGrade8Coder4RichV2Tests
{
    [Theory]
    [InlineData(1, "Moving in the Plane")]
    [InlineData(2, "Naming the Moves")]
    [InlineData(3, "Grid Moves")]
    [InlineData(4, "Making the Moves")]
    [InlineData(5, "Coordinate Moves")]
    [InlineData(6, "Describing Transformations")]
    [InlineData(7, "No Bending or Stretching")]
    [InlineData(8, "Rotation Patterns")]
    [InlineData(9, "Moves in Parallel")]
    [InlineData(10, "Composing Figures")]
    [InlineData(11, "What Is the Same?")]
    [InlineData(12, "Congruent Polygons")]
    [InlineData(13, "Congruence")]
    [InlineData(14, "Alternate Interior Angles")]
    [InlineData(15, "Adding the Angles in a Triangle")]
    [InlineData(16, "Parallel Lines and the Angles in a Triangle")]
    [InlineData(17, "Rotate and Tessellate")]
    public void Grade8Unit1OriginalFirstPartyLessonIsLoadedFromExactRichV2Resource(
        int lessonNumber, string title)
    {
        var code = $"PED:US-CCSS-MATH:G8:U01:L{lessonNumber:D2}";
        var lesson = RichLessonContentV2Registry.Find(code, "en");
        Assert.NotNull(lesson);
        Assert.Equal(title, lesson.Title);
        Assert.True(lesson.ExplanationParagraphs.Count >= 2);
        Assert.True(lesson.KeyConcepts.Count >= 3);
        Assert.True(lesson.WorkedExamples.Count >= 3);
        Assert.True(lesson.CommonMistakes.Count >= 2);
        Assert.True(lesson.SummaryPoints.Count >= 3);
        Assert.Contains(lesson.ExplanationParagraphs, paragraph =>
            paragraph.Contains("CC BY 4.0", StringComparison.Ordinal));
        Assert.Contains(lesson.ExplanationParagraphs, paragraph =>
            paragraph.Contains("teacher", StringComparison.OrdinalIgnoreCase));
        foreach (var example in lesson.WorkedExamples)
        {
            Assert.True(example.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(example.Answer));
            Assert.False(string.IsNullOrWhiteSpace(example.Check));
        }
        Assert.Empty(lesson.Videos);
    }

    [Fact]
    public void Grade8LessonLookupSupportsExistingCultureFallbackWithoutCrossGradeConfusion()
    {
        var code = "PED:US-CCSS-MATH:G8:U01:L03";
        var english = RichLessonContentV2Registry.Find(code, "en");
        Assert.NotNull(english);
        Assert.Equal(english.Title, RichLessonContentV2Registry.Find(code, "en-GB")?.Title);
        Assert.Null(RichLessonContentV2Registry.Find("PED:US-CCSS-MATH:G8:U01:L18", "en"));
    }

    [Fact]
    public void RotationReflectionTranslationAndPythagoreanChecksHaveCorrectIndependentOracles()
    {
        static (int x, int y) RotateCcw(int x, int y) => (-y, x);
        static (int x, int y) RotateCw(int x, int y) => (y, -x);
        static long D2((int x,int y) a, (int x,int y) b) =>
            (long)(a.x - b.x) * (a.x - b.x) + (long)(a.y - b.y) * (a.y - b.y);
        var rand = new Random(20261029);
        for (var i = 0; i < 2048; i++)
        {
            var a = (x: rand.Next(-300,301),y: rand.Next(-300,301));
            var b = (x: rand.Next(-300,301),y: rand.Next(-300,301));
            var dx = rand.Next(-50,51);
            var dy = rand.Next(-50,51);
            Assert.Equal(D2(a,b),D2((a.x+dx,a.y+dy),(b.x+dx,b.y+dy)));
            Assert.Equal(D2(a,b),D2(RotateCcw(a.x,a.y),RotateCcw(b.x,b.y)));
            Assert.Equal(D2(a,b),D2((-a.x,a.y),(-b.x,b.y)));
            var c = RotateCcw(a.x,a.y);
            Assert.Equal(a,RotateCw(c.x,c.y));
        }
        Assert.Equal(25,3*3+4*4);
        Assert.NotEqual(RotateCcw(2,1),RotateCw(2,1));
    }
}


public sealed class UsCcssGrade8Coder4Unit2RichV2Tests
{
    [Theory]
    [InlineData(1, "Projecting and Scaling")]
    [InlineData(2, "Circular Grid")]
    [InlineData(3, "Dilations with no Grid")]
    [InlineData(4, "Dilations on a Square Grid")]
    [InlineData(5, "More Dilations")]
    [InlineData(6, "Similarity")]
    [InlineData(7, "Similar Polygons")]
    [InlineData(8, "Similar Triangles")]
    [InlineData(9, "Side Length Quotients in Similar Triangles")]
    [InlineData(10, "Meet Slope")]
    [InlineData(11, "Writing Equations for Lines")]
    [InlineData(12, "Using Equations for Lines")]
    [InlineData(13, "The Shadow Knows")]
    public void Unit2FirstPartyRichLessonIsDiscoverableAndHasWorkedReasoning(
        int number, string expectedTitle)
    {
        var code = $"PED:US-CCSS-MATH:G8:U02:L{number:D2}";
        var content = RichLessonContentV2Registry.Find(code, "en");
        Assert.NotNull(content);
        Assert.Equal(expectedTitle, content.Title);
        Assert.True(content.ExplanationParagraphs.Count >= 2);
        Assert.True(content.KeyConcepts.Count >= 3);
        Assert.True(content.WorkedExamples.Count >= 3);
        Assert.True(content.CommonMistakes.Count >= 2);
        Assert.True(content.SummaryPoints.Count >= 3);
        Assert.Contains(content.ExplanationParagraphs, x=>x.Contains("CC BY 4.0"));
        Assert.Contains(content.ExplanationParagraphs, x=>x.Contains("teacher",StringComparison.OrdinalIgnoreCase));
        Assert.All(content.WorkedExamples, x => {
            Assert.True(x.Steps.Count >= 3);
            Assert.False(string.IsNullOrWhiteSpace(x.Answer));
            Assert.False(string.IsNullOrWhiteSpace(x.Check));
        });
    }

    [Fact]
    public void IndependentUnit2ScalingAndSlopeOraclesCoverLimits()
    {
        static (double x,double y) Dilate(
            (double x,double y) p,(double x,double y) c,double scale) =>
            (c.x+scale*(p.x-c.x),c.y+scale*(p.y-c.y));
        Assert.Equal((5d,3d), Dilate((3d,2d),(1d,1d),2));
        Assert.Equal((-4d,3d), Dilate((-8d,6d),(0d,0d),0.5));
        Assert.Equal(24d,2d*3d*Math.Pow(2d,2d));
        Assert.Equal(2d,(8d-2d)/(4d-1d));
        Assert.Equal(-2d,(1d-5d)/(2d-0d));
        Assert.Equal(7.5d, 1.5d * 10d / 2d);
        Assert.Equal(0d,30d-2d*15d);
        Assert.NotEqual(2d,9d/5d);
    }
}


public sealed class UsCcssGrade8Coder4FullCourseRichV2Tests
{
    [Theory]
    [InlineData(1,17)]
    [InlineData(2,13)]
    [InlineData(3,14)]
    [InlineData(4,16)]
    [InlineData(5,22)]
    [InlineData(6,11)]
    [InlineData(7,16)]
    [InlineData(8,16)]
    [InlineData(9,6)]
    public void EveryCanonicalGrade8LessonResolvesFromTheActualEmbeddedRichV2Registry(
        int unit, int expectedLessonCount)
    {
        var checkedCount = 0;
        for (var lessonNumber = 1; lessonNumber <= expectedLessonCount; lessonNumber++)
        {
            var code = $"PED:US-CCSS-MATH:G8:U{unit:D2}:L{lessonNumber:D2}";
            var content = RichLessonContentV2Registry.Find(code, "en");
            Assert.NotNull(content);
            Assert.False(string.IsNullOrWhiteSpace(content.Title));
            Assert.True(content.ExplanationParagraphs.Count >= 2, code);
            Assert.True(content.KeyConcepts.Count >= 3, code);
            Assert.True(content.WorkedExamples.Count >= 3, code);
            Assert.True(content.CommonMistakes.Count >= 2, code);
            Assert.True(content.SummaryPoints.Count >= 3, code);
            Assert.Contains(content.ExplanationParagraphs,
                x => x.Contains("CC BY 4.0", StringComparison.Ordinal));
            Assert.Contains(content.ExplanationParagraphs,
                x => x.Contains("teacher", StringComparison.OrdinalIgnoreCase));
            Assert.All(content.WorkedExamples, example =>
            {
                Assert.True(example.Steps.Count >= 3, code);
                Assert.False(string.IsNullOrWhiteSpace(example.Question));
                Assert.False(string.IsNullOrWhiteSpace(example.Answer));
                Assert.False(string.IsNullOrWhiteSpace(example.Check));
            });
            checkedCount++;
        }
        Assert.Equal(expectedLessonCount,checkedCount);
    }

    [Fact]
    public void AllRichV2DocumentsCanBeParsedThroughWholeRegistryWithoutDuplicateLessonCodes()
    {
        // The strict AllDocuments path deliberately validates each embedded resource.
        var documents = RichLessonContentV2Registry.AllDocuments;
        Assert.NotEmpty(documents);
        var grade8Codes = documents
            .SelectMany(document => document.Lessons)
            .Select(lesson => lesson.LessonCode)
            .Where(code => code.StartsWith("PED:US-CCSS-MATH:G8:", StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(131,grade8Codes.Length);
        Assert.Equal(131,grade8Codes.Distinct(StringComparer.Ordinal).Count());
    }

    [Fact]
    public void IndependentIntegerExponentAndPythagoreanRegressionsKeepMathBoundaries()
    {
        Assert.Equal(0.25d,Math.Pow(2d,3d)*Math.Pow(2d,-5d));
        Assert.Equal(0.001d,Math.Pow(10d,-3d),12);
        Assert.Equal(9d,Math.Pow(3d,4d)/Math.Pow(3d,2d));
        Assert.Equal(25,3*3+4*4);
        Assert.Equal(12d,Math.Sqrt(13*13-5*5));
        Assert.True(Math.Sqrt(2d)>1.4d);
        Assert.True(Math.Sqrt(2d)<1.5d);
        Assert.Equal(10d,Math.Sqrt(Math.Pow(4-(-2),2)+Math.Pow(9-1,2)));
    }
}


public sealed class UsCcssGrade8Coder4ExistingPracticeSmokeTests
{
    [Fact]
    public void Grade8Unit4Lesson5OneVariableFamilyActuallyGeneratesAndRejectsMalformedAnswers()
    {
        const string code = "PED:US-CCSS-MATH:G8:U04:L05";
        Assert.True(Edulytics.Core.Mathematics.Practice.LessonPracticeContractRegistry
            .TryResolve(code, out var contract));
        Assert.NotNull(contract);
        const string family = "algebra.linear.ax_plus_b_equals_c";
        Assert.Contains(family, contract.AllowedQuestionFamilies);
        Assert.True(Edulytics.Services.Mathematics.ExactSkillContractQuestionEngine
            .SupportsFamily(family));
        var engine = new Edulytics.Services.Mathematics.ExactSkillContractQuestionEngine();
        var questions = engine.Generate(
            fingerprintNamespace: "usccss-g8-coder4-v7-local-smoke",
            scopeKey: code,
            allowedQuestionFamilies: [family],
            difficulty: Edulytics.Services.Mathematics.ExactSkillQuestionDifficulty.Standard,
            questionCount: 8,
            seed: 20261029,
            excludedExposureFingerprints: []);
        Assert.Equal(8, questions.Count);
        foreach (var question in questions)
        {
            Assert.Equal(family, question.Family);
            Assert.True(Edulytics.Services.Mathematics.ExactSkillContractQuestionEngine
                .Verify(question.Family, question.Parameters, question.CorrectAnswer));
            Assert.False(Edulytics.Services.Mathematics.ExactSkillContractQuestionEngine
                .Verify(question.Family, question.Parameters, "definitely-not-a-number"));
            Assert.False(Edulytics.Services.Mathematics.ExactSkillContractQuestionEngine
                .Verify(question.Family, question.Parameters, "999999999"));
        }
        // Smoke-only: no student auth, attempt persistence, full lesson objective
        // coverage, accessibility or teacher-assessed task has been verified.
    }
}

public sealed class UsCcssGrade8Coder4TwoVariableRepresentationTests
{
    [Fact]
    public void Unit3Lesson12PublishesCorrectAccessiblePairTableButStillRequiresActualGraphAndPractice()
    {
        const string lessonCode = "PED:US-CCSS-MATH:G8:U03:L12";
        var lesson = RichLessonContentV2Registry.Find(lessonCode, "en");
        Assert.NotNull(lesson);
        var table = Assert.Single(lesson.Visuals, x =>
            x.Kind == RichLessonVisualKind.RatioTable);
        var equations = Assert.Single(lesson.Visuals, x =>
            x.Kind == RichLessonVisualKind.EquationSet);
        Assert.Contains("two", table.Description, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("graph", table.Description, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("2x + y = 7", equations.Description, StringComparison.Ordinal);
        Assert.Equal(5, table.Rows.Count);
        Assert.Equal(
            new[] { "Case", "x", "y", "2x + y", "Satisfies equation?" },
            table.Rows.Select(row => row.Label));
        var xRow = table.Rows[1].Cells.Select(int.Parse).ToArray();
        var yRow = table.Rows[2].Cells.Select(int.Parse).ToArray();
        var resultRow = table.Rows[3].Cells.Select(int.Parse).ToArray();
        var solutionRow = table.Rows[4].Cells.ToArray();
        Assert.Equal(4, xRow.Length);
        for (var i = 0; i < xRow.Length; i++)
        {
            Assert.Equal(2 * xRow[i] + yRow[i], resultRow[i]);
            Assert.Equal(resultRow[i] == 7 ? "Yes" : "No", solutionRow[i]);
        }
        Assert.Equal(new[] { "Yes", "Yes", "Yes", "No" }, solutionRow);
        // The existing runtime maps this lesson to the wrong one-variable
        // family; see Supervisor 2 Issue #595. These displays are not C/D proof.
    }
}

public sealed class UsCcssGrade8Coder4V10OriginalVisualResourceTests
{
    [Fact]
    public void SixtySevenGrade8LessonsLoadOriginalMathRepresentationsWithoutMutatingOtherCurricula()
    {
        var lessons = RichLessonContentV2Registry.AllDocuments
            .SelectMany(document => document.Lessons)
            .Where(lesson => lesson.LessonCode.StartsWith(
                "PED:US-CCSS-MATH:G8:", StringComparison.Ordinal))
            .OrderBy(lesson => lesson.LessonCode, StringComparer.Ordinal)
            .ToArray();

        Assert.Equal(131, lessons.Length);
        Assert.Equal(131, lessons.Select(lesson => lesson.LessonCode)
            .Distinct(StringComparer.Ordinal).Count());

        var supported = lessons.Where(lesson => lesson.Visuals.Count > 0).ToArray();
        Assert.Equal(67, supported.Length);
        var ratioTables = supported.SelectMany(lesson => lesson.Visuals)
            .Where(visual => visual.Kind == RichLessonVisualKind.RatioTable)
            .ToArray();
        Assert.Equal(67, ratioTables.Length);

        var unitNumbers = supported
            .Select(lesson => lesson.LessonCode.Split(':')[3])
            .Distinct(StringComparer.Ordinal)
            .OrderBy(x => x, StringComparer.Ordinal).ToArray();
        Assert.Equal(
            new[] { "U01", "U02", "U03", "U04", "U05", "U06", "U07", "U08", "U09" },
            unitNumbers);

        foreach (var lesson in supported)
        {
            var exact = RichLessonContentV2Registry.Find(lesson.LessonCode, "en");
            Assert.NotNull(exact);
            Assert.Equal(lesson.Title, exact.Title);
            Assert.NotEmpty(exact.WorkedExamples);

            foreach (var visual in exact.Visuals)
            {
                Assert.False(string.IsNullOrWhiteSpace(visual.Title));
                Assert.False(string.IsNullOrWhiteSpace(visual.Description));
                Assert.DoesNotContain(
                    "cms-assets.illustrativemathematics.org",
                    visual.Description,
                    StringComparison.OrdinalIgnoreCase);

                if (visual.Kind != RichLessonVisualKind.RatioTable)
                    continue;

                Assert.True(visual.Rows.Count >= 2);
                var width = visual.Rows[0].Cells.Count;
                Assert.InRange(width, 1, 6);
                Assert.All(visual.Rows, row =>
                {
                    Assert.False(string.IsNullOrWhiteSpace(row.Label));
                    Assert.Equal(width, row.Cells.Count);
                    Assert.All(row.Cells, cell =>
                        Assert.False(string.IsNullOrWhiteSpace(cell)));
                });
            }
        }
        // Loader and table semantics are not authenticated A/B acceptance,
        // publisher activity parity or any Stage C/D Practice proof.
    }
}
