using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

public sealed class CommonCoreG1U02CompareFiveSourceTests
{
    private static IReadOnlyList<(CanonicalLessonContentPackDocument Doc,
        CanonicalLessonContentPackLesson Lesson,CanonicalLessonContentPackTranslation En)> Targets()=>
        MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(d=>d.Lessons.Where(l=>
                CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.IsTarget(d,l))
                .Select(l=>(Doc:d,Lesson:l,En:l.Translations.Single(t=>t.CultureCode=="en"))))
            .OrderBy(x=>x.Lesson.LessonCode).ToArray();

    [Fact]
    public void AllAndOnlyFiveOriginalFirstEditionLessonsGetCorrectTeachingAndGoals()
    {
        var lesson=Targets();
        Assert.Equal(5,lesson.Count);
        var names=new[]{"Make Them the Same","School Supplies",
            "Compare Favorite Art Supply Data","Compare with Addition and Subtraction",
            "Different Types of Story Problems"};
        for(var i=0;i<5;i++)
        {
            var row=lesson[i];
            Assert.EndsWith($"L{i+11:00}",row.Lesson.LessonCode);
            Assert.Equal(names[i],row.En.Title);
            Assert.Equal("CC BY 4.0",row.Lesson.SourceRights);
            Assert.Equal(CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.CorrectedContentVersion,
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(row.Doc,row.Lesson));
            Assert.Contains("Goal:",row.En.KeyConceptsAndRules);
            Assert.Contains("Step 4:",row.En.StepByStepSolutions);
            Assert.Contains("Example 2:",row.En.WorkedExamples);
            Assert.DoesNotContain("The source task combines",row.En.WorkedExamples);
            Assert.NotEmpty(row.En.QuickSummary);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(
                row.Lesson.LessonCode,new CanonicalLessonTranslationRecord(
                    row.En.CultureCode,row.En.Title,row.En.Explanation,row.En.KeyConceptsAndRules,
                    row.En.WorkedExamples,row.En.StepByStepSolutions,row.En.CommonMistakes,
                    row.En.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
        }
        Assert.Contains("DIFFERENCE",lesson[0].En.Explanation);
        Assert.Contains("school",lesson[1].En.Title,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("chart",lesson[2].En.Explanation,StringComparison.OrdinalIgnoreCase);
        Assert.Contains("addition equation",lesson[3].En.Explanation);
        Assert.Contains("Part Unknown",lesson[4].En.Explanation);
    }

    [Fact]
    public void OriginalArchivedHashesAndFiveLessonScopeRecordedWithoutModernAccessImConflation()
    {
        var fileName="US-CCSS-G1-006.json";
        var folder=new DirectoryInfo(AppContext.BaseDirectory);
        string? path=null;
        while(folder is not null)
        {
            var candidate=Path.Combine(folder.FullName,"artifacts","curriculum-delivery",
                "US-CCSS-MATH","G1","batches",fileName);
            if(File.Exists(candidate)){path=candidate;break;}
            folder=folder.Parent;
        }
        Assert.NotNull(path);
        using var doc=System.Text.Json.JsonDocument.Parse(File.ReadAllText(path!));
        Assert.Equal(5,doc.RootElement.GetProperty("lessonCount").GetInt32());
        var rows=doc.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(5,rows.Length);
        var targets=Targets();
        for(var i=0;i<5;i++)
        {
            var r=rows[i];
            Assert.Equal(i+26,r.GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(targets[i].Lesson.LessonCode,
                r.GetProperty("lessonCode").GetString());
            Assert.Equal(targets[i].Lesson.CanonicalBodySha256,
                r.GetProperty("originalEmbeddedCanonicalBodySha256").GetString());
            Assert.Equal(64,r.GetProperty("originalSourceTeacherPreparationSha256").GetString()!.Length);
            Assert.Equal(64,r.GetProperty("originalSourceTeacherLessonSha256").GetString()!.Length);
            Assert.Contains("excluded",r.GetProperty("sourceEditionConflictExcluded").GetString()!);
        }
    }

    [Fact]
    public void UnexpectedSourceHashMustFailClosed()
    {
        var item=Targets()[0];
        var original=item.Lesson.CanonicalBodySha256;
        try
        {
            item.Lesson.CanonicalBodySha256=new string('0',64);
            Assert.Throws<InvalidOperationException>(()=>
                CommonCoreG1Unit2CompareFiveSourceTeachingCorrections.Apply(item.Doc));
        }
        finally{item.Lesson.CanonicalBodySha256=original;}
    }

    [Fact]
    public void CanonicalGradeBlueprintL14TitleProjectionIsSourceLocked()
    {
        var c="PED:US-CCSS-MATH:G1:U02:L14";
        Assert.Equal("Compare with Addition and Subtraction",
            CommonCoreG1Unit2NextFiveSourceTeachingCorrections
                .ProjectVerifiedPublisherTitle(c,
                    "Compare with Addition and Subtraction PLC Activity"));
        Assert.Throws<InvalidOperationException>(()=>
            CommonCoreG1Unit2NextFiveSourceTeachingCorrections
                .ProjectVerifiedPublisherTitle(c,"unreviewed version"));
    }
    [Fact]
    public void G1006UsesSameSourceComparisonDetailAsG1005WithoutAssumingEditionIsFailure()
    {
        var directory=new DirectoryInfo(AppContext.BaseDirectory);
        while(directory is not null &&
              !File.Exists(Path.Combine(directory.FullName,"Edulytics.sln")))
            directory=directory.Parent;
        Assert.NotNull(directory);

        var root=Path.Combine(directory!.FullName,"artifacts",
            "curriculum-delivery","US-CCSS-MATH","G1","batches");
        using var reviewed=System.Text.Json.JsonDocument.Parse(
            File.ReadAllText(Path.Combine(root,"US-CCSS-G1-006-LOCAL-SOURCE-CROSSWALK.json")));
        using var prior=System.Text.Json.JsonDocument.Parse(
            File.ReadAllText(Path.Combine(root,"US-CCSS-G1-005-LOCAL-SOURCE-CROSSWALK.json")));
        var oldRows=prior.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        var rows=reviewed.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(5,oldRows.Length);
        Assert.Equal(5,rows.Length);
        var expected=Targets();
        var expectedStandards=new[]
        {
            new[]{"1.OA.A.1"},
            new[]{"1.OA.A.1","1.OA.C.5"},
            new[]{"1.MD.C.4","1.OA.A.1"},
            new[]{"1.OA.A.1","1.OA.C.5","1.OA.C.6","1.OA.D.7"},
            new[]{"1.OA.A.1","1.OA.B.4","1.OA.C.6"}
        };
        for(var i=0;i<rows.Length;i++)
        {
            var row=rows[i];
            Assert.Equal(i+26,row.GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(expected[i].Lesson.LessonCode,
                row.GetProperty("lessonCode").GetString());
            Assert.Equal(expectedStandards[i],row.GetProperty(
                "publisherLessonActivityStandardsAddressing").EnumerateArray()
                .Select(x=>x.GetString()!).ToArray());
            Assert.Equal(row.GetProperty("sourcePrepWholeLessonAddressing").EnumerateArray()
                    .Select(x=>x.GetString()!).ToArray(),
                row.GetProperty("publisher2021PreparationStandardsAddressing")
                    .EnumerateArray().Select(x=>x.GetString()!).ToArray());
            Assert.True(row.GetProperty("sourceFilesSha256")
                .GetProperty("studentLesson").ValueKind==
                System.Text.Json.JsonValueKind.Null);
            Assert.Equal("NOT_INDEPENDENTLY_CERTIFIED",
                row.GetProperty("publisherGraphicsInspection").GetString());
            Assert.False(row.GetProperty("classroomActivitiesDigitallyVerified").GetBoolean());
            Assert.NotEmpty(row.GetProperty("originalStudentTask").GetString()!);
            Assert.NotEmpty(row.GetProperty("sourceToLearnerReview").GetString()!);
            Assert.NotEmpty(row.GetProperty("targetSkill").GetString()!);
            var version=row.GetProperty("alternateEditionTaskComparison");
            Assert.Contains(version.GetProperty("classification").GetString(),
                new[]{"SAME_GOAL_REVISED_TASK","PARTIAL_OVERLAP"});
            Assert.Equal(64,version.GetProperty("sha256").GetString()!.Length);
            Assert.Contains("rather than excluded solely",
                version.GetProperty("decision").GetString());
        }
        Assert.Equal("AccessIM v.360",
            rows[2].GetProperty("alternateEditionTaskComparison").GetProperty("edition").GetString());
        Assert.Equal("SAME_GOAL_REVISED_TASK",rows[2].GetProperty(
            "alternateEditionTaskComparison").GetProperty("classification").GetString());
        Assert.Equal("PARTIAL_OVERLAP",rows[3].GetProperty(
            "alternateEditionTaskComparison").GetProperty("classification").GetString());
    }

}
