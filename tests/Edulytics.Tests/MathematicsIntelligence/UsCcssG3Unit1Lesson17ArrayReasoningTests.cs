using Edulytics.Core.Curriculum;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authored Stage C contract only; tests intentionally NOT RUN.</summary>
public sealed class UsCcssG3Unit1Lesson17ArrayReasoningTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L17";

    [Fact]
    public void SourceCardSortWodbAndDrawingsNeverReceiveNumericAutoApproval()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.Equal(6,lesson.SourceSubtasks.Count);
        Assert.All(lesson.SourceSubtasks,task=>
        {
            Assert.Equal("teacher",task.AnswerType);
            var feedback=Grade3SourceCompanionCatalogue.Grade(task,"16");
            Assert.Null(feedback.NumericCorrect);
            Assert.True(feedback.TeacherReview);
        });
        Assert.Equal("teacher",lesson.OriginalEdulyticsCoolDown!.AnswerType);
        Assert.Null(Grade3SourceCompanionCatalogue.Grade(
            lesson.OriginalEdulyticsCoolDown,"16").NumericCorrect);
    }

    [Fact]
    public void SourceSpecificRichV2CoversHumanGeometryRatherThanOnlyProducts()
    {
        var rich=RichLessonContentV2Registry.Find(Code,"en");
        Assert.NotNull(rich);
        var body=string.Join(" ",rich.ExplanationParagraphs);
        Assert.Contains("15 dots",body);
        Assert.Contains("16",body);
        Assert.Contains("MLR1 Stronger and Clearer Each Time",body);
        Assert.Contains("four groups of three",body);
        Assert.Contains("incomplete column",body);
        Assert.Contains("multiple arrays",body);
        Assert.Contains(rich.CommonMistakes,q=>q.Mistake.Contains("automatic pass"));
        Assert.Contains(rich.SummaryPoints,q=>q.Contains("teacher-assessed"));
    }

    [Fact]
    public void FirstPartyTwelveCardDeckTracks2021BlacklineMathButNotOriginalArt()
    {
        // Static authored-source test; NOT EXECUTED until Stage D.
        static string Read(string path)
        {
            DirectoryInfo? root = new(AppContext.BaseDirectory);
            while(root is not null)
            {
                var full=Path.Combine(root.FullName,path.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(full))return File.ReadAllText(full);
                root=root.Parent;
            }
            throw new FileNotFoundException(path);
        }
        var view=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l17-2021.rich-lesson-v2.json");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var materializer=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit1Lesson17RichV2.cs");
        Assert.Contains("var firstPartyCards",view);
        Assert.Contains("A–J (25)",view);
        Assert.Contains("B–K (14)",view);
        Assert.Contains("C–G (12)",view);
        Assert.Contains("D–H (20)",view);
        Assert.Contains("E–F (30)",view);
        Assert.Contains("I–L (12)",view);
        Assert.Contains("New A — 15",view);
        Assert.Contains("New B — 15",view);
        Assert.Contains("New C — 15",view);
        Assert.Contains("New D — 16",view);
        Assert.Contains("for(var dot=0;dot<16;dot++)",view);
        Assert.Contains("g3-u01-l17-2021-first-party-abc-authoring-20261024-v2",rich);
        Assert.Contains("CommonCoreG3Unit1Lesson17RichV2.Apply(lesson)",materializer);
        Assert.Contains("Edulytics.Data.Grade3.Unit1Lesson17RichV2",project);
        Assert.Contains("g3-u01-l17-2021-first-party-abc-authoring-20261024-v2",bridge);
    }

    [Theory]
    [InlineData("A-J",25)]
    [InlineData("B-K",14)]
    [InlineData("C-G",12)]
    [InlineData("D-H",20)]
    [InlineData("E-F",30)]
    [InlineData("I-L",12)]
    public void OriginalSourceCardsAreTeacherReviewedAcrossSixDifferentMatches(
       string originalSourcePair,int count)
    {
        Assert.InRange(count,12,30);
        Assert.Equal(3,originalSourcePair.Length);
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        var activity=lesson!.SourceSubtasks.Single(q=>q.Id.EndsWith("A1-MATCH",StringComparison.Ordinal));
        // Even a mathematically correct pair is NOT an automatic source-grade.
        var result=Grade3SourceCompanionCatalogue.Grade(activity,originalSourcePair);
        Assert.Null(result.NumericCorrect);
        Assert.True(result.TeacherReview);
        var wrong=Grade3SourceCompanionCatalogue.Grade(activity,"A-B");
        Assert.Null(wrong.NumericCorrect);
        Assert.True(wrong.TeacherReview);
    }
}
