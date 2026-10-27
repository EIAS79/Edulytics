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

    // Historical B001 labels and the 2021 Grade 3 first-party authored heading
    // can differ. Only exact, immutable source/authoring pairs are admitted; this
    // is technical render consistency, never publisher-title certification.
    private static bool IsSourceBoundGrade3FirstPartyTitle(
        string root, string code, string oldTitle, string effectiveTitle, string sourceUrl)
    {
        var path=Path.Combine(root,"artifacts","curriculum-delivery",
            "US-CCSS-MATH","G3","stage-d-source-bound-display-title-reconciliation.json");
        var manifestPath=Path.Combine(root,"artifacts","curriculum-delivery",
            "US-CCSS-MATH","G3","grade-manifest.json");
        if(!File.Exists(path)||!File.Exists(manifestPath))return false;
        using var aliases=JsonDocument.Parse(File.ReadAllText(path));
        var list=aliases.RootElement.GetProperty("entries").EnumerateArray().ToArray();
        if(list.Length!=30||list.Select(x=>x.GetProperty("lessonCode").GetString())
            .Distinct(StringComparer.Ordinal).Count()!=30)return false;
        var matches=list.Where(x=>x.GetProperty("lessonCode").GetString()==code).ToArray();
        if(matches.Length!=1)return false;
        var row=matches[0];
        if(row.GetProperty("historicalCatalogueTitle").GetString()!=oldTitle ||
           row.GetProperty("firstPartyDisplayTitle").GetString()!=effectiveTitle ||
           row.GetProperty("sourceUrl").GetString()!=sourceUrl)return false;
        using var manifest=JsonDocument.Parse(File.ReadAllText(manifestPath));
        var lesson=manifest.RootElement.GetProperty("lessons").EnumerateArray()
            .Where(x=>x.GetProperty("lessonCode").GetString()==code).ToArray();
        if(lesson.Length!=1||
           lesson[0].GetProperty("title").GetString()!=oldTitle ||
           lesson[0].GetProperty("sourceTeacherUrl").GetString()!=sourceUrl ||
           lesson[0].GetProperty("originalSemanticSha256").GetString()!=
             row.GetProperty("originalSemanticSha256").GetString())return false;
        var authored=Path.Combine(root,row.GetProperty("richV2Path").GetString()!
            .Replace('/',Path.DirectorySeparatorChar));
        var checkpoint=Path.Combine(root,row.GetProperty("authoringCandidatePath").GetString()!
            .Replace('/',Path.DirectorySeparatorChar));
        if(!File.Exists(authored)||!File.Exists(checkpoint))return false;
        using var authoredJson=JsonDocument.Parse(File.ReadAllText(authored));
        var items=authoredJson.RootElement.GetProperty("Lessons");
        if(items.GetArrayLength()!=1||items[0].GetProperty("LessonCode").GetString()!=code||
           items[0].GetProperty("Title").GetString()!=effectiveTitle)return false;
        using var candidate=JsonDocument.Parse(File.ReadAllText(checkpoint));
        return candidate.RootElement.GetProperty("lessonCode").GetString()==code &&
            candidate.RootElement.GetProperty("authoringState").GetString()=="READY_FOR_STAGE_D" &&
            candidate.RootElement.GetProperty("readyForStageD").GetBoolean();
    }

    // The historical B001 catalogue retained activity-name aliases for these three
    // 2021 IM lessons. The actual locally archived teacher/student pages have the
    // following lesson headings. Reconciliation must be exact and source-bound,
    // never a generic waiver of the 500-lesson title-integrity gate.
    private static bool IsVerified2021PublisherTitleCorrection(
        string root, string code, string historicalTitle, string effectiveTitle, string sourceUrl)
    {
        if (code.StartsWith("PED:US-CCSS-MATH:G3:",StringComparison.Ordinal))
            return IsSourceBoundGrade3FirstPartyTitle(root,code,historicalTitle,
                effectiveTitle,sourceUrl);

        // Grade 4 2021 unit indices use a "PLC Activity" display suffix on 23
        // indexed lesson titles (13 historical B001 aliases, 10 later blueprint aliases). The individual teacher
        // preparation pages use the lesson heading without that suffix. The alias
        // registry is exact-code, exact-title, URL and recorded-source-hash pinned;
        // it does not certify publisher parity, academic approval or image rights.
        if (code.StartsWith("PED:US-CCSS-MATH:G4:",StringComparison.Ordinal))
        {
            var aliasPath=Path.Combine(root,"artifacts","curriculum-delivery",
                "US-CCSS-MATH","G4","g4-b001-index-display-title-aliases.json");
            if (!File.Exists(aliasPath))return false;
            using var aliases=JsonDocument.Parse(File.ReadAllText(aliasPath));
            var entries=aliases.RootElement.GetProperty("entries").EnumerateArray().ToArray();
            if(entries.Length!=23 || entries.Select(x=>x.GetProperty("lessonCode").GetString())
                   .Distinct(StringComparer.Ordinal).Count()!=23)return false;
            var g4AliasMatches=entries.Where(x=>x.GetProperty("lessonCode").GetString()==code).ToArray();
            if(g4AliasMatches.Length!=1)return false;
            var row=g4AliasMatches[0];
            if(row.GetProperty("historicalCatalogueTitle").GetString()!=historicalTitle ||
               row.GetProperty("firstPartyDisplayTitle").GetString()!=effectiveTitle ||
               row.GetProperty("sourceUrl").GetString()!=sourceUrl ||
               historicalTitle!=effectiveTitle+" PLC Activity")return false;
            var unit=code.Substring(code.IndexOf(":U",StringComparison.Ordinal)+2,2).TrimStart('0');
            var lessonNumber=code.Substring(code.LastIndexOf(":L",StringComparison.Ordinal)+2).TrimStart('0');
            if(sourceUrl!="https://im.kendallhunt.com/k5/teachers/grade-4/unit-"+
               unit+"/lesson-"+lessonNumber+"/preparation.html")return false;
            var packPath=Path.Combine(root,"src","Edulytics.Core","Curriculum",
                "LessonContent","Packs","us-ccss-math-g4-phase29-v1.lesson-content-pack.json");
            using var sourcePack=JsonDocument.Parse(File.ReadAllText(packPath));
            var sourceMatches=sourcePack.RootElement.GetProperty("lessons").EnumerateArray()
                .Where(x=>x.GetProperty("lessonCode").GetString()==code).ToArray();
            return sourceMatches.Length==1 &&
                sourceMatches[0].GetProperty("sourceSha256").GetString()==
                    row.GetProperty("recordedSourceSha256").GetString() &&
                sourceMatches[0].GetProperty("sourceUrl").GetString()==sourceUrl;
        }

        // Three further Grade 1 catalogue aliases have exact, source-SHA-pinned
        // first-party corrections. Keep the historic B001 labels unchanged.
        // Accept only these precise old/new titles, official lesson URLs,
        // and the matching reviewed correction entry, not arbitrary title drift.
        (string Old, string New, string TeacherSha, int Unit, int Lesson)? extra = code switch
        {
            "PED:US-CCSS-MATH:G1:U03:L24" =>
                ("Relate Counting to Addition and Subtraction PLC Activity",
                 "Relate Counting to Addition and Subtraction",
                 "3fd5d2c854c79d76d2678582f6cf6d3285a8004ed7e608a62fe54d2777be9761",3,24),
            "PED:US-CCSS-MATH:G1:U04:L04" =>
                ("More Addition and Subtraction with Tens PLC Activity",
                 "More Addition and Subtraction with Tens",
                 "aea5b61803df4c4b9df6fd46f89e9413a5bb48d0ef33c3e8178327ec9e22b5c7",4,4),
            "PED:US-CCSS-MATH:G1:U04:L08" =>
                ("Different Representations of Tens and Ones PLC Activity",
                 "Different Representations of Tens and Ones",
                 "f3fa805d6421b2677b3a20200f257432271cbfee1e3b9c1ec6d34b8ba855d4da",4,8),
            _ => null
        };
        if(extra.HasValue)
        {
            var expected = extra.Value;
            var correctUrl = "https://im.kendallhunt.com/k5/teachers/grade-1/unit-" +
                expected.Unit + "/lesson-" + expected.Lesson + "/preparation.html";
            if(historicalTitle!=expected.Old || effectiveTitle!=expected.New ||
               sourceUrl!=correctUrl)return false;
            var correctionPath=Path.Combine(root,"src","Edulytics.Data","Seeding",
                "CommonCoreG1FullGradeStartingContentCorrections.cs");
            var reviewed=File.ReadAllText(correctionPath);
            var recordStart="new(@\"" + code + "\",@\"" + sourceUrl + "\",@\"" +
                expected.TeacherSha + "\"";
            var at=reviewed.IndexOf(recordStart,StringComparison.Ordinal);
            if(at<0)return false;
            var next=reviewed.IndexOf("\n        new(@\"",at+recordStart.Length,
                StringComparison.Ordinal);
            var sourceRow=reviewed[at..(next<0?reviewed.Length:next)];
            return sourceRow.Contains("@\""+effectiveTitle+"\"",
                StringComparison.Ordinal);
        }
        (string Old, string New, string Number)? verified = code switch
        {
            "PED:US-CCSS-MATH:G1:U02:L07" =>
                ("Shake and Spill", "One Part Red, One Part Yellow", "7"),
            "PED:US-CCSS-MATH:G1:U02:L08" =>
                ("Shake, Spill, and Cover", "One Part Red, How Many Yellow?", "8"),
            "PED:US-CCSS-MATH:G1:U02:L09" =>
                ("Compare Story Problems PLC Activity", "What Is the Unknown?", "9"),
            "PED:US-CCSS-MATH:G1:U02:L14" =>
                ("Compare with Addition and Subtraction PLC Activity",
                 "Compare with Addition and Subtraction", "14"),
            "PED:US-CCSS-MATH:G1:U02:L19" =>
                ("Story Problems and Equations PLC Activity",
                 "Story Problems and Equations", "19"),
            _ => ((string Old, string New, string Number)?)null
        };
        if (verified is null || historicalTitle != verified.Value.Old ||
            effectiveTitle != verified.Value.New ||
            sourceUrl != "https://im.kendallhunt.com/k5/teachers/grade-1/unit-2/lesson-" +
                verified.Value.Number + "/preparation.html")
            return false;
        var nextBatch=code=="PED:US-CCSS-MATH:G1:U02:L14";
        var thirdBatch=code=="PED:US-CCSS-MATH:G1:U02:L19";
        using var batch = JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "artifacts", "curriculum-delivery", "US-CCSS-MATH", "G1", "batches",
            thirdBatch?"US-CCSS-G1-007.json":
                nextBatch?"US-CCSS-G1-006.json":"US-CCSS-G1-005.json")));
        var matching = batch.RootElement.GetProperty("lessons").EnumerateArray()
            .Where(x => x.GetProperty("lessonCode").GetString() == code).ToArray();
        if(matching.Length!=1)return false;
        var target=matching[0];
        if(thirdBatch)
            return batch.RootElement.GetProperty("lessonCount").GetInt32()==5 &&
                target.GetProperty("originalLessonTitle").GetString()==effectiveTitle &&
                target.GetProperty("previousCatalogTitle").GetString()==historicalTitle &&
                target.GetProperty("sourceTeacherPreparationUrl").GetString()==sourceUrl &&
                target.GetProperty("originalSourceTeacherPreparationSha256").GetString()==
                    "2a1017cd2f32bb0573a862379760b2c817c684c49d7beba60558f677bc70cec7";
        if(nextBatch)
            return batch.RootElement.GetProperty("lessonCount").GetInt32()==5 &&
                target.GetProperty("source2021LessonTitle").GetString()==effectiveTitle &&
                target.GetProperty("oldCatalogTitle").GetString()==historicalTitle &&
                target.GetProperty("originalSourceTeacherPreparationUrl").GetString()==sourceUrl &&
                target.GetProperty("originalSourceTeacherPreparationSha256").GetString()==
                    "18a8baef801cf09e27f5d6e64283a4287c274425db159bd240b1bd74cc27a904";
        return target.GetProperty("title").GetString() == effectiveTitle &&
            target.GetProperty("previousCatalogueTitle").GetString() == historicalTitle &&
            target.GetProperty("teacherPreparationUrl").GetString() == sourceUrl &&
            target.GetProperty("locallyVerifiedPublisherTitle").GetBoolean();
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
