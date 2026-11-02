using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Stage C source contract authored, intentionally NOT EXECUTED.</summary>
public sealed class UsCcssG3Unit1Lesson19UnknownProductTests
{
    private const string Code="PED:US-CCSS-MATH:G3:U01:L19";
    [Theory]
    [InlineData("7x5=□",true)]
    [InlineData("5×7=?",true)]
    [InlineData("35=7×□",false)]
    [InlineData("7×5=35",false)]
    [InlineData("35",false)]
    [InlineData("7+5=?",false)]
    [InlineData("7×6=?",false)]
    public void CrayonTaskRequiresSymbolicUnknownProduct(string value,bool expected)
    {
        var q=new Grade3OriginalQuestion {
            AnswerType="multiplication-unknown-equation",Expected="7x5=□"
        };
        var f=Grade3SourceCompanionCatalogue.Grade(q,value);
        Assert.Equal(expected,f.NumericCorrect);
        Assert.True(f.TeacherReview);
    }
    [Fact]
    public void FiveCrayonRowsAndBothCoconutGrovesRequireSymbolAndSolution()
    {
        Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var lesson));
        Assert.NotNull(lesson);
        Assert.True(lesson.ReadyForStageD);
        Assert.Equal(12,lesson.SourceSubtasks.Count);
        foreach(var pair in new[]{("G3-U01-L19-A1-01","7x5=□","35","35"),("G3-U01-L19-A2-01","5x9=□","5x9=45","45"),("G3-U01-L19-A2-02","2x4=□","2x4=8","8")})
        {
            var symbolic=lesson.SourceSubtasks.Single(q=>q.Id==pair.Item1+"-SYMBOL");
            var numeric=lesson.SourceSubtasks.Single(q=>q.Id==pair.Item1);
            Assert.Equal("multiplication-unknown-equation",symbolic.AnswerType);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(symbolic,pair.Item2).NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(numeric,pair.Item3).NumericCorrect);
            Assert.True(Grade3SourceCompanionCatalogue.Grade(numeric,pair.Item3).TeacherReview);
            if(numeric.AnswerType=="multiplication-equation")
                Assert.False(Grade3SourceCompanionCatalogue.Grade(numeric,pair.Item4).NumericCorrect);
            Assert.False(Grade3SourceCompanionCatalogue.Grade(symbolic,pair.Item3).NumericCorrect);
        }
        Assert.Equal("teacher",lesson.SourceSubtasks.Single(
            q=>q.Id=="G3-U01-L19-A1-COCRAFT").AnswerType);
        Assert.Equal(new[]{"20","18","16","14"},
            lesson.SourceSubtasks.Take(4).Select(q=>q.Expected));
        Assert.True(lesson.OriginalStudentTaskPdfRead);
    }

    [Fact]
    public void CanonicalRichV2AndMLR5DisclosureAreFirstPartyAuthoringOnly()
    {
        static string Read(string path)
        {
            DirectoryInfo? root=new(AppContext.BaseDirectory);
            while(root is not null)
            {
                var full=Path.Combine(root.FullName,path.Replace('/',Path.DirectorySeparatorChar));
                if(File.Exists(full))return File.ReadAllText(full);
                root=root.Parent;
            }
            throw new FileNotFoundException(path);
        }
        var rich=Read("src/Edulytics.Core/Curriculum/LessonContent/RichV2/us-ccss-g3-u01-l19-2021.rich-lesson-v2.json");
        var razor=Read("src/Edulytics.Web/Views/Grade3SourceCompanion/Index.cshtml");
        var bridge=Read("src/Edulytics.Data/Seeding/CommonCoreG3Unit1Lesson19RichV2.cs");
        var corrector=Read("src/Edulytics.Data/Seeding/CommonCoreG3SourceTeachingCorrections.cs");
        var project=Read("src/Edulytics.Data/Edulytics.Data.csproj");
        Assert.Contains("g3-u01-l19-2021-first-party-source-abc-20261024-v2",rich);
        Assert.Contains("g3-u01-l19-2021-first-party-source-abc-20261024-v2",bridge);
        Assert.Contains("CommonCoreG3Unit1Lesson19RichV2.Apply(lesson)",corrector);
        Assert.Contains("Edulytics.Data.Grade3.Unit1Lesson19RichV2",project);
        Assert.Contains("Phase 1 — source MLR5 Co-craft Questions",razor);
        Assert.Contains("<details>",razor);
        Assert.Contains("for(var crayon=0;crayon<35;crayon++)",razor);
        Assert.Contains("for(var tree=0;tree<45;tree++)",razor);
        Assert.Contains("for(var tree=0;tree<8;tree++)",razor);
    }
}
