using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage A–C authored tests only; NOT executed.</summary>
public sealed class UsCcssG3Unit1Lesson16PhysicalArraysTests
{
    private const string Code = "PED:US-CCSS-MATH:G3:U01:L16";

    [Fact]
    public void SourceTasksRequirePhysicalTeacherJudgmentNotJustCorrectArithmetic()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.Equal(7,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,q =>
        {
            Assert.Equal("teacher",q.AnswerType);
            var feedback=Grade3SourceCompanionCatalogue.Grade(q,"24");
            Assert.Null(feedback.NumericCorrect);
            Assert.True(feedback.TeacherReview);
        });
        Assert.Equal("teacher",lesson.IndependentPractice!.AnswerType);
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(
            lesson.OriginalEdulyticsCoolDown,"24").NumericCorrect);
        Assert.False(lesson.ReadyForStageD);
    }

    [Fact]
    public void SourceSpecificRichV2PreservesTwentyFourCubeNonExhaustiveInstruction()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var content=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("12 eggs",content);
        Assert.Contains("four circles",content);
        Assert.Contains("six groups of five",content);
        Assert.Contains("60 connecting cubes",content);
        Assert.Contains("all 20 cubes",content);
        Assert.Contains("all 24 cubes",content);
        Assert.Contains("NOT require finding every possible factor pair",content);
        Assert.Contains(rich.CommonMistakes,q=>q.Mistake.Contains("Requiring all possible arrays"));
        Assert.Contains(rich.SummaryPoints,p=>p.Contains("human-judged"));
    }

    [Fact]
    public void AuthoringBridgeAndOptionalNumericCheckDoNotMarkPhysicalConstructionCorrect()
    {
        // Code-level contract. Intentionally NOT EXECUTED under Stage A–C policy.
        static string ReadSource(string path)
        {
            DirectoryInfo? current = new(AppContext.BaseDirectory);
            while (current is not null)
            {
                var candidate = Path.Combine(current.FullName,
                    path.Replace('/', Path.DirectorySeparatorChar));
                if (File.Exists(candidate)) return File.ReadAllText(candidate);
                current = current.Parent;
            }
            throw new FileNotFoundException(path);
        }

        var rich=ReadSource("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l16-2021.rich-lesson-v2.json");
        var materializer=ReadSource("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var bridge=ReadSource("src/Edulytics.Data/Seeding/CommonCoreG3Unit1Lesson16RichV2.cs");
        var project=ReadSource("src/Edulytics.Data/Edulytics.Data.csproj");
        var controller=ReadSource("src/Edulytics.Web/Controllers/Grade3SourceCompanionController.cs");
        Assert.Contains("g3-u01-l16-2021-first-party-source-abc-authoring-20261024-v2",rich);
        Assert.Contains("CommonCoreG3Unit1Lesson16RichV2.Apply(lesson)",materializer);
        Assert.Contains("Edulytics.Data.Grade3.Unit1Lesson16RichV2",project);
        Assert.Contains("g3-u01-l16-2021-first-party-source-abc-authoring-20261024-v2",bridge);
        Assert.Contains("lesson.LessonNumber == 16",controller);
        Assert.Contains("numeric correctness is not",controller,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("60 or more connecting cubes for EACH PAIR",rich);
        Assert.Contains("MLR8 Discussion Supports",rich);
        Assert.Contains("Five in a Row: Multiplication Stage 1",rich);
    }

    [Fact]
    public void SourceSubtasksHaveNoMachinePassEvenForCorrectNumericalTotals()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        foreach (var answer in new[] {"20","24","30","0","not an array"})
        {
            Assert.All(lesson.SourceSubtasks,q =>
            {
                var result = Grade3SourceCompanionCatalogue.Grade(q,answer);
                Assert.Null(result.NumericCorrect);
                Assert.True(result.TeacherReview);
            });
        }
        // Correctly calculating 6 x 5 = 30 alone is an optional supplement.
        Assert.NotNull(lesson.SourceBasedCheck);
        Assert.True(Grade3SourceCompanionCatalogue.Grade(lesson.SourceBasedCheck!,"30").TeacherReview);
        Assert.False(lesson.ReadyForStageD);
        Assert.False(lesson.OriginalStudentTaskPdfRead);
    }
}
