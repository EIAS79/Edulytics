using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;
using System.Text.Json;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1TenAndTeenFiveTests
{
    private static readonly string[] Codes=[
        "PED:US-CCSS-MATH:G1:U03:L09",
        "PED:US-CCSS-MATH:G1:U03:L10",
        "PED:US-CCSS-MATH:G1:U03:L11",
        "PED:US-CCSS-MATH:G1:U03:L12",
        "PED:US-CCSS-MATH:G1:U03:L13"];
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
      CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)> Targets()=>
      MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
        .SelectMany(doc=>doc.Lessons.Where(l=>
          CommonCoreG1TenAndTeenSourceFiveCorrections.IsTarget(doc,l))
          .Select(l=>(Doc:doc,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
        .OrderBy(x=>x.Lesson.LessonCode).ToArray();

    [Fact]
    public void CorrectedSourceBodiesPreserveFiveOfficialLessonsAndRichInstruction()
    {
        var rows=Targets();
        Assert.Equal(5,rows.Count);
        Assert.Equal(Codes,rows.Select(x=>x.Lesson.LessonCode));
        foreach(var row in rows)
        {
            Assert.Equal("CC BY 4.0",row.Lesson.SourceRights);
            Assert.Equal(CommonCoreG1TenAndTeenSourceFiveCorrections.Version,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            Assert.Contains("Goal:",row.En.KeyConceptsAndRules);
            Assert.Contains("Example 2:",row.En.WorkedExamples);
            Assert.Contains("Step 5:",row.En.StepByStepSolutions);
            Assert.DoesNotContain("The source task combines",row.En.WorkedExamples);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(
                row.Lesson.LessonCode,new CanonicalLessonTranslationRecord(
                    row.En.CultureCode,row.En.Title,row.En.Explanation,row.En.KeyConceptsAndRules,
                    row.En.WorkedExamples,row.En.StepByStepSolutions,row.En.CommonMistakes,
                    row.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        Assert.Contains("ten-frame",rows[0].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("missing start",rows[1].En.KeyConceptsAndRules,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rock",rows[2].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Addition and Subtraction Equations with Teen Numbers",rows[3].En.Explanation);
        Assert.Contains("18+2=20",rows[2].En.Explanation);
        Assert.Contains("second complete ten",rows[2].En.WorkedExamples);
        Assert.Contains("Shake and Spill",rows[3].En.Explanation);
        Assert.Contains("Change Unknown",rows[4].En.Explanation);
    }

    [Fact]
    public void CrosswalkCoversAllFiveExactSourcePdfsGoalsAndUntouchedStandards()
    {
        var folder=new DirectoryInfo(AppContext.BaseDirectory);
        while(folder is not null&&!File.Exists(Path.Combine(folder.FullName,"Edulytics.sln")))
            folder=folder.Parent;
        Assert.NotNull(folder);
        var path=Path.Combine(folder!.FullName,"artifacts","curriculum-delivery",
            "US-CCSS-MATH","G1","batches","US-CCSS-G1-010-LOCAL-SOURCE-CROSSWALK.json");
        using var source=JsonDocument.Parse(File.ReadAllText(path));
        Assert.Equal("EDULYTICS-GRADE-FIRST-5-V1",
            source.RootElement.GetProperty("planId").GetString());
        Assert.Equal("PENDING",
            source.RootElement.GetProperty("ownerProductionUat").GetString());
        var rows=source.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(5,rows.Length);
        var materialized=Targets();
        for(int i=0;i<5;i++)
        {
            Assert.Equal(46+i,rows[i].GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(Codes[i],rows[i].GetProperty("lessonCode").GetString());
            Assert.Equal("1.3."+(i+9),rows[i].GetProperty("sourceLessonCode").GetString());
            Assert.Equal(64,rows[i].GetProperty("original2021StudentTaskPdfSha256").GetString()!.Length);
            Assert.Equal(64,rows[i].GetProperty("originalTeacherPreparationSha256").GetString()!.Length);
            Assert.Equal(64,rows[i].GetProperty("originalTeacherLessonSha256").GetString()!.Length);
            Assert.Equal(materialized[i].Lesson.OutcomeCodes,
                rows[i].GetProperty("officialOutcomeCodesUnchanged")
                       .EnumerateArray().Select(x=>x.GetString()!).ToArray());
            var goals=rows[i].GetProperty("sourceGoals").EnumerateArray().ToArray();
            Assert.Equal(2,goals.Length);
            foreach(var goal in goals)
            {
                Assert.True(goal.GetProperty("sourceGoalRequirement").GetString()!.Length>=35);
                Assert.Contains("Original 2021",goal.GetProperty("evidenceScope").GetString());
                Assert.StartsWith("LOCAL_IMPLEMENTED_PENDING_EXACT_HEAD_CI",
                    goal.GetProperty("assessmentStatus").GetString());
            }
        }
    }
}
