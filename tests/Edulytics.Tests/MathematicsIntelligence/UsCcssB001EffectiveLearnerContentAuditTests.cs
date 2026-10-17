using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// B001 effective in-repository learner-body and Rich V2 runtime-composition audit.
/// Not a browser, teacher review, source-license opinion or authenticated DB test.
/// Produces one visible quality/technical triage record for every canonical code.
/// </summary>
public sealed class UsCcssB001EffectiveLearnerContentAuditTests
{
    private static readonly string[] GenericPlaceholders =
    [
        "Step 3: Apply the key idea from the lesson.",
        "Step 4: Explain how the representation supports the result.",
        "Step 1: Identify what needs to be measured."
    ];

    private static string Root()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Edulytics.sln")))
                return dir.FullName;
            dir = dir.Parent;
        }
        throw new InvalidOperationException("Edulytics root not found");
    }

    // The historical B001 catalogue retained activity-name aliases for these three
    // 2021 IM lessons. The actual locally archived teacher/student pages have the
    // following lesson headings. Reconciliation must be exact and source-bound,
    // never a generic waiver of the 500-lesson title-integrity gate.
    private static bool IsVerified2021PublisherTitleCorrection(
        string root, string code, string historicalTitle, string effectiveTitle, string sourceUrl)
    {
        (string Old, string New, string Number)? verified = code switch
        {
            "PED:US-CCSS-MATH:G1:U02:L07" =>
                ("Shake and Spill", "One Part Red, One Part Yellow", "7"),
            "PED:US-CCSS-MATH:G1:U02:L08" =>
                ("Shake, Spill, and Cover", "One Part Red, How Many Yellow?", "8"),
            "PED:US-CCSS-MATH:G1:U02:L09" =>
                ("Compare Story Problems PLC Activity", "What Is the Unknown?", "9"),
            _ => ((string Old, string New, string Number)?)null
        };
        if (verified is null || historicalTitle != verified.Value.Old ||
            effectiveTitle != verified.Value.New ||
            sourceUrl != "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-" +
                verified.Value.Number + "/preparation.html")
            return false;
        using var batch = JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "artifacts", "curriculum-delivery", "US-CCSS-MATH", "G1", "batches",
            "US-CCSS-G1-005.json")));
        var matching = batch.RootElement.GetProperty("lessons").EnumerateArray()
            .Where(x => x.GetProperty("lessonCode").GetString() == code).ToArray();
        return matching.Length == 1 &&
            matching[0].GetProperty("title").GetString() == effectiveTitle &&
            matching[0].GetProperty("previousCatalogueTitle").GetString() == historicalTitle &&
            matching[0].GetProperty("teacherPreparationUrl").GetString() == sourceUrl &&
            matching[0].GetProperty("locallyVerifiedPublisherTitle").GetBoolean();
    }

    [Fact]
    public void EveryCanonicalB001LessonHasSourceLinkedEffectiveTeachingAndCompilableRichContent()
    {
        var root = Root();
        var file = Path.Combine(root, "docs", "curriculum", "batches",
            "US-CCSS-B001-CATALOGUE.json");
        using var manifest = JsonDocument.Parse(File.ReadAllText(file));
        var expected = manifest.RootElement.GetProperty("lessons")
            .EnumerateArray()
            .Select(x => (
                Code: x.GetProperty("lessonCode").GetString()!,
                Source: x.GetProperty("sourceUrl").GetString()!,
                Title: x.GetProperty("title").GetString()!,
                IsHistorical: x.GetProperty("historicalT001").GetBoolean()))
            .ToArray();
        Assert.Equal(500, expected.Length);
        Assert.Equal(500, expected.Select(x => x.Code).Distinct(StringComparer.Ordinal).Count());

        var lookup = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .SelectMany(doc => doc.Lessons.Select(lesson => (Document:doc, Lesson:lesson)))
            .Where(x => expected.Any(e => e.Code == x.Lesson.LessonCode))
            .ToDictionary(x => x.Lesson.LessonCode, StringComparer.Ordinal);

        var rows = new List<object>(500);
        var technicalBlockers = new List<string>();
        var qualityCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        var placeholderCount = 0;
        var placeholderCodes = new List<string>();
        var curatedCount = 0;
        var compiledCount = 0;

        foreach (var (code, source, title, historical) in expected)
        {
            var issues = new List<string>();
            if (!lookup.TryGetValue(code, out var pair))
            {
                rows.Add(new {lessonCode=code,status="MISSING_EMBEDDED_LESSON",
                    issues=new[]{"NO_EFFECTIVE_SOURCE"},academicAcceptance="NOT_GRANTED"});
                technicalBlockers.Add(code + ": missing embedded lesson");
                continue;
            }
            var (document, lesson) = pair;
            if (lesson.SourceUrl != source)
                issues.Add("SOURCE_URL_MISMATCH");
            var english = lesson.Translations.FirstOrDefault(x => x.CultureCode == "en");
            if (english is null)
            {
                issues.Add("ENGLISH_TRANSLATION_MISSING");
                rows.Add(new {lessonCode=code,status="BLOCKED",issues,
                    academicAcceptance="NOT_GRANTED"});
                technicalBlockers.Add(code+": English content missing");
                continue;
            }
            if (english.Title != title &&
                !IsVerified2021PublisherTitleCorrection(root, code, title,
                    english.Title, source))
                issues.Add("TITLE_MISMATCH");
            var instructional = string.Join("\n",english.Explanation,
                english.KeyConceptsAndRules,english.WorkedExamples,
                english.StepByStepSolutions,english.CommonMistakes,english.QuickSummary);
            var generic = GenericPlaceholders.Where(p => instructional.Contains(
                p, StringComparison.OrdinalIgnoreCase)).ToArray();
            if (generic.Length > 0) { placeholderCount++; placeholderCodes.Add(code); }
            var quality = RichLessonContentQualityAudit.Evaluate(document,lesson,english);
            var qualityLabel = quality.OverallQuality.ToString();
            qualityCounts[qualityLabel] = qualityCounts.GetValueOrDefault(qualityLabel)+1;

            var rich = RichLessonContentV2Registry.Find(code,"en");
            string compositionStatus;
            if (rich is not null)
            {
                curatedCount++;
                compositionStatus = "RICH_V2_CURATED";
            }
            else
            {
                try
                {
                    var body = new CanonicalLessonTranslationRecord(
                        english.CultureCode,english.Title,english.Explanation,
                        english.KeyConceptsAndRules,english.WorkedExamples,
                        english.StepByStepSolutions,english.CommonMistakes,english.QuickSummary);
                    rich = RichLessonContentV2RuntimeComposer.TryCompose(code,body);
                    compositionStatus = rich is null ? "RICH_V2_UNAVAILABLE" : "RICH_V2_COMPOSED";
                    if (rich is not null) compiledCount++;
                }
                catch (Exception ex)
                {
                    compositionStatus = "RICH_V2_EXCEPTION";
                    issues.Add("RICH_V2_EXCEPTION_"+ex.GetType().Name);
                }
            }
            if (rich is null) issues.Add("NO_EFFECTIVE_RICH_V2_CONTENT");
            else if (rich.WorkedExamples.Count == 0 ||
                     rich.ExplanationParagraphs.Count == 0 ||
                     rich.SummaryPoints.Count == 0)
                issues.Add("RICH_V2_MISSING_TEACHING_COMPONENT");
            if (issues.Count > 0)
                technicalBlockers.Add(code+": "+string.Join(",",issues));

            rows.Add(new {
                lessonCode=code,title,sourceUrl=source,
                sourceEdition=lesson.SourceEdition,
                effectiveContentVersion=CanonicalLessonContentMaterializer
                    .GetEffectiveContentVersion(document,lesson),
                historicalT001=historical,
                officialOutcomes=lesson.OutcomeCodes,
                qualityHeuristic=qualityLabel,
                qualityFindings=quality.Findings,
                sourceResearchRequired=quality.SourceResearchRequired,
                placeholderPatterns=generic,
                richV2Status=compositionStatus,
                richV2ExampleCount=rich?.WorkedExamples.Count??0,
                richV2ExplanationCount=rich?.ExplanationParagraphs.Count??0,
                technicalStatus=issues.Count==0?"SOURCE_AND_RICH_COMPOSABLE":"TECHNICAL_REVIEW_REQUIRED",
                issues,
                classroomInstructionalAccuracy="NOT_INDEPENDENTLY_REVIEWED",
                realBrowserRendering="NOT_TESTED",
                authenticatedPersistence="NOT_TESTED",
                academicAcceptance="NOT_GRANTED"
            });
        }

        var dest = Path.Combine(root,"artifacts","curriculum-delivery",
            "US-CCSS-MATH","B001");
        Directory.CreateDirectory(dest);
        File.WriteAllText(Path.Combine(dest,"effective-learner-content-triage.json"),
            JsonSerializer.Serialize(new {
                schemaVersion=1,batchId="US-CCSS-B001",
                candidateSha=Environment.GetEnvironmentVariable("GITHUB_SHA")??"local-unpinned",
                testScope="SOURCE_LINKED_EFFECTIVE_IN_REPOSITORY_TEXT_AND_RICH_V2_RUNTIME_COMPOSITION",
                notProofOf="Teacher lesson validation, exact publisher fidelity, licensing, live browser routing, authenticated grading or student data persistence",
                expectedLessons=500,rowsExamined=rows.Count,
                sourceAndRichTechnicalBlockers=technicalBlockers.Count,
                genericPlaceholderLessonCount=placeholderCount,
                richV2CuratedCount=curatedCount,
                richV2CompiledCount=compiledCount,
                qualityHeuristicCounts=qualityCounts,
                internalAcademicAcceptanceCount=0,
                releaseDecision="NO_GO",
                lessons=rows
            },new JsonSerializerOptions {WriteIndented=true})+"\n");

        Console.WriteLine("B001_EFFECTIVE_CONTENT_AUDIT_JSON:"+
            JsonSerializer.Serialize(new {
                expected=500,examined=rows.Count,technicalBlockers=technicalBlockers.Count,
                curated=curatedCount,compiled=compiledCount,
                genericPlaceholderLessonCount=placeholderCount,
                qualityHeuristicCounts=qualityCounts,
                genericByGrade=placeholderCodes.GroupBy(c=>c.Split(':')[2])
                    .ToDictionary(g=>g.Key,g=>g.Count()),
                first30GenericCodes=placeholderCodes.Take(30).ToArray(),
                academicApproved=0
            }));
        Assert.Equal(500,rows.Count);
        Assert.True(technicalBlockers.Count==0,
            $"{technicalBlockers.Count} effective source/Rich V2 technical blockers: "+
            string.Join(" | ",technicalBlockers.Take(40)));
    }
}
