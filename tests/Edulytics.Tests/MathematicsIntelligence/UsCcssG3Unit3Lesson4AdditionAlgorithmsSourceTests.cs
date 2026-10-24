using System.Text.Json;
using System.Text.Json.Serialization;
using Edulytics.Core.Curriculum;
using Edulytics.Data.Seeding;
using Edulytics.Web.Curriculum.Grade3;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>Authoring-only G3 U03 L04 tests; no runtime or Stage D execution.</summary>
public sealed class UsCcssG3Unit3Lesson4AdditionAlgorithmsSourceTests
{
    const string Code="PED:US-CCSS-MATH:G3:U03:L04";
    static Grade3CompanionLesson Lesson(){Assert.True(Grade3SourceCompanionCatalogue.TryGet(Code,out var l));return Assert.IsType<Grade3CompanionLesson>(l);}
    static Grade3OriginalQuestion Task(string suffix)=>Lesson().SourceSubtasks.Single(q=>q.Id.EndsWith(suffix,StringComparison.Ordinal));

    [Theory]
    [InlineData("WU-A","247",true)]
    [InlineData("WU-B","247",true)]
    [InlineData("WU-C","257",true)]
    [InlineData("WU-C","247",false)]
    [InlineData("WU-D","247",true)]
    [InlineData("A1-362-354","716",true)]
    [InlineData("A1-362-354","706",false)]
    [InlineData("A2-SUM1","706",true)]
    [InlineData("A2-SUM2","525",true)]
    [InlineData("A2-SUM3","927",true)]
    [InlineData("A2-SUM3","827",false)]
    [InlineData("A2-SUM2","415",false)]
    [InlineData("A2-SUM3","927.0",false)]
    [InlineData("A2-SUM3","",false)]
    public void PreservesSourceAdditionNumericsAndWrongCases(string suffix,string value,bool expected)
    {
        var r=Grade3SourceCompanionCatalogue.Grade(Task(suffix),value);
        Assert.Equal(expected,r.NumericCorrect);
        Assert.True(r.TeacherReview);
    }
    [Fact]
    public void OpenDiscussionAndCritiqueCannotBeAutoCertified()
    {
        Assert.False(Lesson().ReadyForStageD);
        Assert.False(Lesson().OriginalStudentTaskPdfRead);
        Assert.Equal(14,Lesson().SourceSubtasks.Count);
        foreach(var q in Lesson().SourceSubtasks.Where(q=>q.AnswerType=="teacher"))
        {
            var result=Grade3SourceCompanionCatalogue.Grade(q,"927");
            Assert.Null(result.NumericCorrect);
            Assert.True(result.TeacherReview);
        }
    }
    [Fact]
    public void MaterializedTeachingExplainsOriginalTwoAlgorithmsAndComposition()
    {
        var assembly=typeof(CanonicalLessonContentPackDocument).Assembly;
        var r=assembly.GetManifestResourceNames().Single(n=>n.EndsWith("us-ccss-math-g3-phase29-v1.lesson-content-pack.json",StringComparison.Ordinal));
        using var stream=assembly.GetManifestResourceStream(r)!;
        var opts=new JsonSerializerOptions(JsonSerializerDefaults.Web);
        opts.Converters.Add(new JsonStringEnumConverter());
        var d=JsonSerializer.Deserialize<CanonicalLessonContentPackDocument>(stream,opts)!;
        var lesson=d.Lessons.Single(x=>x.LessonCode==Code);
        CommonCoreG3SourceTeachingCorrections.Apply(d);
        var en=lesson.Translations.Single(x=>x.CultureCode=="en");
        foreach(var anchor in new[]{"A=200+30+17","C=200+47+10","362+354=716","600+110+6","6+110+600","475+231=706","136+389=525","670+257=927","MLR7","MLR3"})
            Assert.Contains(anchor,en.Explanation);
        lesson.CanonicalBodySha256=new string('0',64);
        Assert.False(CommonCoreG3SourceTeachingCorrections.IsTarget(d,lesson));
    }
}
