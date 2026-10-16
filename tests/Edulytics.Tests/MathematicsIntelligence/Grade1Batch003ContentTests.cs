using Edulytics.Core.Curriculum;
using System.Reflection;
using Edulytics.Core.Lessons;
using Edulytics.Services.LessonContent;
using Xunit;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class Grade1Batch003ContentTests
{
    private const string Prefix="PED:US-CCSS-MATH:G1:U01:L";

    private static RichLessonContentV2Lesson Empty(string code)=>new()
    {
        LessonCode=code,CultureCode="en",Title="source-verified Grade 1 mini-lesson",
        ExplanationParagraphs=[],KeyConcepts=[],WorkedExamples=[],
        CommonMistakes=[],SummaryPoints=[],Visuals=[],Videos=[]
    };

    [Fact]
    public void FiveSourcePurposeLessonsReuseExistingRichV2LayoutAndTeachGradeOneCorrectly()
    {
        var append=typeof(RichLessonContentV2RuntimeComposer).GetMethod(
            "AddG1003LearnerTeaching",BindingFlags.NonPublic|BindingFlags.Static);
        Assert.NotNull(append);
        for(var i=11;i<=15;i++)
        {
            var lesson=Empty(Prefix+i.ToString("D2"));
            append!.Invoke(null,[lesson.LessonCode,lesson]);
            Assert.StartsWith("Learning goal:",lesson.ExplanationParagraphs[0]);
            Assert.Equal(2,lesson.WorkedExamples.Count);
            Assert.All(lesson.WorkedExamples,x=>
            {
                Assert.NotEmpty(x.Steps);
                Assert.False(string.IsNullOrWhiteSpace(x.Answer));
                Assert.False(string.IsNullOrWhiteSpace(x.Check));
            });
            Assert.NotEmpty(lesson.CommonMistakes[0].Correction);
            Assert.NotEmpty(lesson.SummaryPoints);
        }
    }

    [Fact]
    public void DataQuestionAndOptionalAnimalSurveyNeverExposeUnrelatedNegativeWorkedAnswers()
    {
        var append=typeof(RichLessonContentV2RuntimeComposer).GetMethod(
            "AddG1003LearnerTeaching",BindingFlags.NonPublic|BindingFlags.Static)!;
        foreach(var i in new[]{13,15})
        {
            var lesson=Empty(Prefix+i.ToString("D2"));
            append.Invoke(null,[lesson.LessonCode,lesson]);
            var all=string.Join(" ",lesson.WorkedExamples.Select(x=>
                x.Question+" "+x.Answer+" "+string.Join(" ",x.Steps)));
            Assert.DoesNotContain("= -3",all);
            Assert.DoesNotContain("= -4",all);
        }
        var survey=Empty(Prefix+"15");
        append.Invoke(null,[survey.LessonCode,survey]);
        Assert.Contains("Optional project",survey.ExplanationParagraphs[0]);
        Assert.Contains("real survey",survey.ExplanationParagraphs[1]);
        Assert.Contains("fictional",survey.WorkedExamples[0].Check);
        var asking=Empty(Prefix+"13");
        append.Invoke(null,[asking.LessonCode,asking]);
        Assert.Contains("Ask questions",asking.ExplanationParagraphs[0]);
        Assert.Contains("individual names",asking.WorkedExamples[1].Check,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ScopingLeavesOriginalTenAndNextGradeUntouched()
    {
        var append=typeof(RichLessonContentV2RuntimeComposer).GetMethod(
            "AddG1003LearnerTeaching",BindingFlags.NonPublic|BindingFlags.Static)!;
        foreach(var code in new[]{Prefix+"10",Prefix+"16","PED:US-CCSS-MATH:G2:U01:L11"})
        {
            var lesson=Empty(code);
            append.Invoke(null,[code,lesson]);
            Assert.Empty(lesson.WorkedExamples);
        }
    }
}
