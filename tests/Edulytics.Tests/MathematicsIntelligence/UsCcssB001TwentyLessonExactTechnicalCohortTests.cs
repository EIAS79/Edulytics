using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Data.Seeding;
using Edulytics.Services.Mathematics;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// B001 first-20 exact-code P4 TECHNICAL smoke test. Source lesson teaching,
/// student activities, self-grader math and online UAT are not "accepted" by
/// passing this test. This deliberately does not create P5 sign-off.
/// </summary>
public sealed class UsCcssB001TwentyLessonExactTechnicalCohortTests
{
    private static readonly string[] RequiredConcepts =
    [
        "count|add",             // G1 U01 L01: counting collections, dot cubes
        "dot|expression",        // L02: matching dot cards and missing dots
        "count|1 or 2",          // L03: counting on, game center
        "subtract|1 or 2",       // L04: subtract by 1 or 2
        "addition|subtraction",  // L05: mixed strategies
        "pair|10",               // L06: ten partners / center
        "sort|tool",             // L07: tool sorting
        "shape|sort",            // L08: shapes grouped by properties
        "survey|data",           // L09: classroom vote / table
        "center|add",            // L10: choices & number fluency
        "survey|pet",            // L11: class pet categories
        "data|count",            // L12: read a representation
        "question|data",         // L13: ask answerable questions
        "sort|display",          // L14: categories / center choice
        "survey|animal",         // L15: data collection
        "expression|story",      // G1 U02 L01: write expression
        "equation|story",        // L02: represent complete equation
        "unknown|change",        // L03: missing addend
        "result|change",         // L04: distinguish result/change unknown
        "story|center"           // L05: picture-story center
    ];
    private static DirectoryInfo RepositoryRoot()
    {
        DirectoryInfo? dir=new(AppContext.BaseDirectory);
        while(dir is not null && !File.Exists(Path.Combine(dir.FullName,"Edulytics.sln")))
            dir=dir.Parent;
        Assert.NotNull(dir);
        return dir!;
    }

    [Fact]
    public void FirstTwentyHaveRealOriginalSourceActivityReferencesAndEffectiveRichTeaching()
    {
        var root=RepositoryRoot().FullName;
        using var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "docs","curriculum","batches","US-CCSS-B001-20-INTAKE.json")));
        using var source=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "docs","curriculum","batches",
            "US-CCSS-B001-2021-SOURCE-PEDAGOGY-PACKETS-1-OF-2.json")));
        var lessons=manifest.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        var sources=source.RootElement.GetProperty("lessons").EnumerateArray()
            .ToDictionary(x=>x.GetProperty("code").GetString()!,StringComparer.Ordinal);
        Assert.Equal(20,lessons.Length);
        Assert.Equal(20,RequiredConcepts.Length);
        var actual=MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc=>doc.Lessons.Select(lesson=>(doc,lesson)))
            .Where(x=>lessons.Any(id=>id.GetProperty("lessonCode").GetString()==x.lesson.LessonCode))
            .ToDictionary(x=>x.lesson.LessonCode,StringComparer.Ordinal);
        Assert.Equal(20,actual.Count);
        for(var ordinal=1;ordinal<=20;ordinal++)
        {
            var row=lessons[ordinal-1];
            var code=row.GetProperty("lessonCode").GetString()!;
            var canonical=actual[code];
            var lesson=canonical.lesson;
            Assert.Equal(ordinal,row.GetProperty("gradeOrdinal").GetInt32());
            Assert.Equal(ordinal<=15,row.GetProperty("historicalPreviouslyShipped").GetBoolean());
            Assert.Equal(ordinal, sources[code].GetProperty("ordinal").GetInt32());
            var sourceRow=sources[code];
            Assert.False(string.IsNullOrWhiteSpace(
                sourceRow.GetProperty("teacherPurposeExcerpt").GetString()));
            Assert.False(string.IsNullOrWhiteSpace(
                sourceRow.GetProperty("teacherLearningGoalsExcerpt").GetString()));
            var originalActivities=sourceRow.GetProperty("originalActivities").EnumerateArray().ToArray();
            Assert.NotEmpty(originalActivities);
            Assert.Contains(originalActivities,
                activity=>!string.IsNullOrWhiteSpace(activity.GetProperty("name").GetString()));
            Assert.StartsWith("https://im.kendallhunt.com/k5/teachers/grade-1/",
                lesson.SourceUrl);
            var effective=lesson.Translations.Single(x=>x.CultureCode=="en");
            Assert.False(string.IsNullOrWhiteSpace(effective.Explanation));
            Assert.False(string.IsNullOrWhiteSpace(effective.WorkedExamples));
            Assert.False(string.IsNullOrWhiteSpace(effective.StepByStepSolutions));
            Assert.False(string.IsNullOrWhiteSpace(effective.CommonMistakes));
            Assert.False(string.IsNullOrWhiteSpace(effective.QuickSummary));
            var text=(effective.Explanation+" "+effective.KeyConceptsAndRules+" "+
                effective.WorkedExamples+" "+effective.StepByStepSolutions+" "+
                effective.QuickSummary).ToLowerInvariant();
            foreach(var term in RequiredConcepts[ordinal-1].Split('|'))
                Assert.Contains(term,text);
            var rich=RichLessonContentV2RuntimeComposer.TryCompose(code,
                new CanonicalLessonTranslationRecord(
                    effective.CultureCode,effective.Title,effective.Explanation,
                    effective.KeyConceptsAndRules,effective.WorkedExamples,
                    effective.StepByStepSolutions,effective.CommonMistakes,
                    effective.QuickSummary));
            Assert.NotNull(rich);
            Assert.NotEmpty(rich!.WorkedExamples);
            Assert.False(string.IsNullOrWhiteSpace(
                CanonicalLessonContentMaterializer.GetEffectiveContentVersion(
                    canonical.doc,lesson)));
        }
    }

    [Fact]
    public void EachRealFirstTwentyPracticeFamilyIsExplicitlyAllowedAndHasBoundedGeneration()
    {
        using var manifest=JsonDocument.Parse(File.ReadAllText(Path.Combine(
            RepositoryRoot().FullName,"docs","curriculum","batches",
            "US-CCSS-B001-20-INTAKE.json")));
        var codes=manifest.RootElement.GetProperty("lessons").EnumerateArray()
            .Select(x=>x.GetProperty("lessonCode").GetString()!).ToArray();
        var engine=new ExactSkillContractQuestionEngine();
        var checkedCases=0;
        for(var i=0;i<codes.Length;i++)
        {
            var code=codes[i];
            Assert.True(LessonPracticeCapabilityResolver.TryResolve(code,out var practice),code);
            Assert.NotNull(practice);
            Assert.Equal("READY_VERIFIED",practice!.Readiness);
            Assert.NotEmpty(practice.AllowedQuestionFamilies);
            Assert.Equal(practice.AllowedQuestionFamilies.Distinct(StringComparer.Ordinal).Count(),
                practice.AllowedQuestionFamilies.Count);
            foreach(var family in practice.AllowedQuestionFamilies)
            {
                Assert.True(ExactSkillContractQuestionEngine.SupportsFamily(family),code+" "+family);
                for(var seed=0;seed<3;seed++)
                {
                    var q=Assert.Single(engine.Generate(
                        "b001-20-independent-technical-sampling",code,[family],
                        ExactSkillQuestionDifficulty.Standard,1,20261017+(i*31+seed),[]));
                    Assert.Equal(family,q.Family);
                    Assert.False(string.IsNullOrWhiteSpace(q.Prompt));
                    Assert.False(string.IsNullOrWhiteSpace(q.CorrectAnswer));
                    Assert.True(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,q.CorrectAnswer));
                    Assert.False(ExactSkillContractQuestionEngine.Verify(
                        family,q.Parameters,"__NOT_A_VALID_GRADE_1_ANSWER__"));
                    checkedCases++;
                }
            }
        }
        Assert.True(checkedCases>=60,"Too few source-exact P4 technical test cases");
    }

    [Fact]
    public void TechnicalSourceAndPracticeChecksCannotMintAcademicP5Approval()
    {
        var root=RepositoryRoot().FullName;
        using var intake=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "docs","curriculum","batches","US-CCSS-B001-20-INTAKE.json")));
        using var backlog=JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "docs","curriculum","batches","US-CCSS-B001-20-P3-ACADEMIC-GAP-LEDGER.json")));
        Assert.Equal("EDULYTICS-US-CCSS-20-V2",
            intake.RootElement.GetProperty("planId").GetString());
        Assert.Equal(0,intake.RootElement.GetProperty("acceptedUnderCurrent20Policy").GetInt32());
        Assert.Equal(0,backlog.RootElement.GetProperty("counts")
            .GetProperty("academicAcceptedUnderV2_20").GetInt32());
        Assert.Equal("NO_GO",backlog.RootElement.GetProperty("counts")
            .GetProperty("releaseDecision").GetString());
        Assert.Equal(20,backlog.RootElement.GetProperty("lessons").GetArrayLength());
    }
}
