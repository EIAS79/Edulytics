using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;
namespace Edulytics.Tests.MathematicsIntelligence;
/// <summary>Grade 3 Unit 3 Lesson 5 authoring contracts, NOT RUN.</summary>
public sealed class UsCcssG3Unit3Lesson5AdditionAlgorithmSourceTests
{
    const string Code="PED:US-CCSS-MATH:G3:U03:L05";
    static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
    static Grade3OriginalQuestion Task(string s)=>Lesson().SourceSubtasks.Single(q=>q.Id.EndsWith(s,StringComparison.Ordinal));

    [Theory]
    [InlineData("WU-LEFT","199",true)]
    [InlineData("WU-RIGHT","201",true)]
    [InlineData("WU-LEFT","201",false)]
    [InlineData("A1-SUM","598",true)]
    [InlineData("A1-SUM","597",false)]
    [InlineData("A2-SUM","547",true)]
    [InlineData("A2-SUM","447",false)]
    [InlineData("A2-1","526",true)]
    [InlineData("A2-2","892",true)]
    [InlineData("A2-3","552",true)]
    [InlineData("A2-3","542",false)]
    [InlineData("A2-4","901",true)]
    [InlineData("A2-4","801",false)]
    [InlineData("A2-4","901.0",false)]
    public void NumericOraclesAndComposingMistakes(string id,string answer,bool expected)
    {
        var r=Grade3SourceCompanionCatalogue.Grade(Task(id),answer);
        Assert.Equal(expected,r.NumericCorrect);Assert.True(r.TeacherReview);
    }
    [Fact]
    public void HumanAlgorithmComparisonsRemainUnscoredByNumericChecker()
    {
        Assert.False(Lesson().ReadyForStageD);
        Assert.False(Lesson().OriginalStudentTaskPdfRead);
        Assert.Equal(14,Lesson().SourceSubtasks.Count);
        foreach(var task in Lesson().SourceSubtasks.Where(x=>x.AnswerType=="teacher"))
        {
            Assert.Null(Grade3SourceCompanionCatalogue.Grade(task,"547").NumericCorrect);
        }
    }
    [Fact]
    public void MaterializedRichBodyExplainsBothMethodsAndThreeSourceContexts()
    {
        var a=typeof(CanonicalLessonContentPackDocument).Assembly;
        var r=a.GetManifestResourceNames().Single(x=>x.EndsWith("us-ccss-math-g3-phase29-v1.lesson-content-pack.json",StringComparison.Ordinal));
        using var stream=a.GetManifestResourceStream(r)!;
        var options=new JsonSerializerOptions(JsonSerializerDefaults.Web);
        options.Converters.Add(new JsonStringEnumConverter());
        var pack=JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream,options)!;
        var lesson=pack.Lessons.Single(x=>x.LessonCode==Code);
        CommonCoreG3SourceTeachingCorrections.Apply(pack);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        foreach(var text in new[]{"199 and 201","367+231=598","365+182=547","14 tens","354+198=552","525+376=901","MLR8"})
            Assert.Contains(text,en.Explanation);
        lesson.CanonicalBodySha256=new string('0',64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(pack,lesson));
    }
}
