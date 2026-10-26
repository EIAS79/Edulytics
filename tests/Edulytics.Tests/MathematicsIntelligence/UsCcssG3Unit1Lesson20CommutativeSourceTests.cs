using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>2021 G3 U01 L20 Stage C contracts — authored, NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit1Lesson20CommutativeSourceTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L20";
    [Theory]
    [InlineData("2×5=10;5×2=10",true)]
    [InlineData("5*2=10;2x5=10",true)]
    [InlineData("2×5=10",false)]
    [InlineData("10",false)]
    [InlineData("2×5=10;2×5=10",false)]
    [InlineData("2×5=10;5×2=11",false)]
    [InlineData("5×2=10;3×5=15",false)]
    public void BothSourceArraysRequireFullSwappedEquations(string answer,bool expected)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="commutative-equation-pair",Expected="2x5=10;5x2=10"
        };
        var feedback=Grade3SourceCompanionCatalogue.Grade(q,answer);
        Assert.Equal(expected,feedback.NumericCorrect);
        Assert.True(feedback.TeacherReview);
    }
    [Fact]
    public void SourceContainsSubtractionWarmupAndSeparateMLR1Explanation()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));
        Assert.NotNull(l);
        Assert.True(l.ReadyForStageD);
        Assert.Equal(new[]{"60","58","58","56"},l.SourceSubtasks.Take(4).Select(x=>x.Expected));
        Assert.Equal(2,l.SourceSubtasks.Count(x=>x.AnswerType=="commutative-equation-pair"));
        Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U01-L20-A1-02" && x.AnswerType=="teacher");
        Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U01-L20-A2-02" && x.AnswerType=="teacher");
        Assert.Contains(l.SourceSubtasks,x=>x.Id=="G3-U01-L20-SYNTH" && x.AnswerType=="teacher");
        Assert.True(Grade3SourceCompanionCatalogue.Grade(
            l.SourceSubtasks.Single(x=>x.Id=="G3-U01-L20-A2-01"),"6×3=18;3×6=18").NumericCorrect);
        Assert.False(Grade3SourceCompanionCatalogue.Grade(
            l.SourceSubtasks.Single(x=>x.Id=="G3-U01-L20-A2-01"),"18").NumericCorrect);
        Assert.True(l.OriginalStudentTaskPdfRead);
    }

    [Fact]
    public void OriginalGroupGeometryAndRichV2CanonicalBridgeMustRemainSourceAligned()
    {
        static string Read(string path)
        {
            DirectoryInfo? dir=new(AppContext.BaseDirectory);
            while(dir is not null)
            {
                var filename=Path.Combine(dir.FullName,
                    path.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(filename))return File.ReadAllText(filename);
                dir=dir.Parent;
            }
            throw new FileNotFoundException(path);
        }
        var view=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l20-2021.rich-lesson-v2.json");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit1Lesson20RichV2.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        var materializer=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        Assert.Contains("two horizontal groups of five",view);
        Assert.Contains("five vertical groups of two",view);
        Assert.Contains("for(var col=0;col<5;col++)",view);
        Assert.Contains("for(var dot=0;dot<18;dot++)",view);
        Assert.Contains("for(var group=0;group<3;group++)",view);
        Assert.Contains("3 × 5 = 15",view);
        Assert.Contains("g3-u01-l20-2021-source-abc-firstparty-20261024-v2",rich);
        Assert.Contains("g3-u01-l20-2021-source-abc-firstparty-20261024-v2",bridge);
        Assert.Contains("Edulytics.Data.Grade3.Unit1Lesson20RichV2",project);
        Assert.Contains("CommonCoreG3Unit1Lesson20RichV2.Apply(lesson)",materializer);
    }

    [Fact]
    public void HumanSituationsAndMLR1PartnerRevisionCannotBeMachineCertified()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        foreach(var id in new[]{
            "G3-U01-L20-A1-02","G3-U01-L20-A2-02","G3-U01-L20-SYNTH"})
        {
            var original=lesson.SourceSubtasks.Single(x=>x.Id==id);
            Assert.Equal("teacher",original.AnswerType);
            foreach(var attempt in new[]{"10","18","2x5=10;5x2=10",""})
            {
                var result=Grade3SourceCompanionCatalogue.Grade(original,attempt);
                Assert.Null(result.NumericCorrect);
                Assert.True(result.TeacherReview);
            }
        }
    }
}
