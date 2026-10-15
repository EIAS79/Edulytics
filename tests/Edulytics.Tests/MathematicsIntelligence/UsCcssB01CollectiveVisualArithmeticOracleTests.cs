using System.Globalization;
using System.Text.Json;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Data.Seeding;
using Edulytics.Services.LessonContent;

namespace Edulytics.Tests.MathematicsIntelligence;

/// <summary>
/// One whole-batch source-fingerprint-bound visual arithmetic oracle. It probes
/// ACTUALLY composed student Rich V2 for all 500 B01 lessons. Numeric or
/// fractional tick sequences, shaded part bounds and analog clock values must
/// be structurally possible. It does NOT replace human/publisher instructional
/// alignment, pixel-based renderer/a11y acceptance, or L1-L6 proof.
/// </summary>
public sealed class UsCcssB01CollectiveVisualArithmeticOracleTests
{
    [Fact]
    public void AuditActual500RichV2VisualObjectsInOnePass()
    {
        var root = FindRoot();
        using var manifest = JsonDocument.Parse(File.ReadAllText(Path.Combine(root,
            "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json")));
        var entries = manifest.RootElement.GetProperty("lessons").EnumerateArray().ToArray();
        Assert.Equal(500,entries.Length);
        var effective = MathematicsCanonicalLessonContentSeeder.LoadEmbeddedDocuments()
            .Where(d => d.PackCode=="US-CCSS-MATH" && d.VersionCode=="CCSSM-2010")
            .SelectMany(d=>d.Lessons.Select(l=>new {Doc=d,Lesson=l}))
            .ToDictionary(x=>x.Lesson.LessonCode,StringComparer.Ordinal);

        var matrix = new List<object>(500);
        var allFailures = new List<string>();
        var kinds = new Dictionary<string,int>(StringComparer.Ordinal);
        var checkedFractionRows = 0;
        var checkedClockFaces = 0;
        var checkedNumberLines = 0;
        var totalVisuals = 0;

        foreach (var entry in entries)
        {
            var ordinal = entry.GetProperty("ordinal").GetInt32();
            var code = entry.GetProperty("lessonCode").GetString()!;
            var issues = new List<string>();
            var source = effective[code];
            var en = source.Lesson.Translations.Single(x=>x.CultureCode=="en");
            var rich = RichLessonContentV2Registry.Find(code,"en") ??
                RichLessonContentV2RuntimeComposer.TryCompose(
                    code,new CanonicalLessonTranslationRecord(
                        en.CultureCode,en.Title,en.Explanation,en.KeyConceptsAndRules,
                        en.WorkedExamples,en.StepByStepSolutions,en.CommonMistakes,
                        en.QuickSummary));
            if (rich is null)
            {
                issues.Add("NO_STUDENT_RICH_V2");
            }
            else
            {
                foreach (var visual in rich.Visuals)
                {
                    totalVisuals++;
                    var kind = visual.Kind.ToString();
                    kinds[kind] = kinds.GetValueOrDefault(kind)+1;
                    if (string.IsNullOrWhiteSpace(visual.Title) ||
                        string.IsNullOrWhiteSpace(visual.Description))
                        issues.Add("EMPTY_VISUAL_TITLE_OR_INSTRUCTION:"+kind);
                    if (visual.Kind==RichLessonVisualKind.FractionBars)
                    {
                        if (visual.Rows.Count==0)
                            issues.Add("FRACTION_BARS_MISSING_SHADED_PARTS");
                        foreach(var row in visual.Rows)
                        {
                            checkedFractionRows++;
                            if (row.Denominator<=0 || row.Numerator<0 ||
                                row.Numerator>row.Denominator)
                                issues.Add($"FRACTION_BAR_INVALID_SHADED_PARTS:{row.Numerator}/{row.Denominator}");
                        }
                    }
                    if (visual.Kind==RichLessonVisualKind.ClockFace)
                    {
                        checkedClockFaces++;
                        // In G3 U06 L09 the teacher asks for 1:37, NOT only
                        // 5-minute positions. The shared ClockFace Razor SVG
                        // renders 60 equal minute ticks and a continuously
                        // moving hour hand. Reject impossible hand values,
                        // NEVER reject a correct 37-minute reading.
                        if (!ClockItemsAreRepresentable(visual.Items))
                            issues.Add("CLOCK_HANDS_UNREPRESENTABLE_FROM_ITEMS");
                    }
                    if (visual.Kind is RichLessonVisualKind.NumberLine
                        or RichLessonVisualKind.Scale &&
                        visual.Items.Count>=2)
                    {
                        // Only inspect demonstrably numeric or simple rational
                        // ticks; textual categorical axes need different oracles.
                        var ticks = visual.Items.Select(ParseRational).ToArray();
                        if (ticks.All(x=>x.HasValue))
                        {
                            checkedNumberLines++;
                            var intervals = Enumerable.Range(1,ticks.Length-1)
                                .Select(i=>ticks[i]!.Value-ticks[i-1]!.Value).ToArray();
                            if (intervals.Any(x=>x<=0))
                                issues.Add("NUMBER_LINE_NONMONOTONIC_TICK_LABELS");
                            if (intervals.Length>1 && intervals.Any(x=>
                                Math.Abs(x-intervals[0])>0.00000001))
                                issues.Add("NUMBER_LINE_UNEQUAL_VISUAL_TICK_INTERVALS");
                        }
                    }
                }
            }
            foreach(var issue in issues) allFailures.Add(code+": "+issue);
            matrix.Add(new {
                ordinal,lessonCode=code,
                effectiveContentSha256=CanonicalLessonContentMaterializer
                    .ComputeLessonFingerprint(source.Doc,source.Lesson),
                actualVisualCount=rich?.Visuals.Count??0,
                issueCodes=issues.Distinct(StringComparer.Ordinal).ToArray(),
                scope="MATHEMATICALLY_POSSIBLE_VISUAL_STRUCTURE_ONLY",
                academicDecision="NOT_CERTIFIED"
            });
        }

        var output = new {
            kind="B01_500_ACTUAL_RICH_V2_VISUAL_ARITHMETIC_ORACLE",
            expected=500,joined=matrix.Count,totalVisuals,
            checkedFractionRows,checkedClockFaces,checkedNumberLines,
            visualKindCounts=kinds,failures=allFailures,
            lessonRows=matrix,academicDecision="NOT_CERTIFIED",
            caveat="This checks real visual objects and source-body hashes, NOT visual pixels, publisher pedagogy, rendered-browser accessibility or any L1-L6 gate."
        };
        File.WriteAllText(Path.Combine(root,"b01-visual-arithmetic-oracle-500.json"),
            JsonSerializer.Serialize(output,new JsonSerializerOptions{WriteIndented=true})+
            Environment.NewLine);
        Assert.Equal(500,matrix.Count);
        Assert.True(totalVisuals>=500,"Missing visuals in some B01 learner lessons");
        Assert.True(allFailures.Count==0,
            allFailures.Count+" batch visual arithmetic defects: "+
            string.Join(" | ",allFailures.Take(50)));
    }

    internal static bool ClockItemsAreRepresentable(IReadOnlyList<string> items)
    {
        if (items.Count < 2 ||
            !int.TryParse(items[0],out var hour) ||
            !int.TryParse(items[1],out var minute))
            return false;
        return hour is >= 1 and <= 12 &&
            minute is >= 0 and <= 59;
    }

    [Theory]
    [InlineData(1,37,true)]
    [InlineData(1,35,true)]
    [InlineData(12,59,true)]
    [InlineData(6,0,true)]
    [InlineData(0,37,false)]
    [InlineData(13,37,false)]
    [InlineData(1,60,false)]
    [InlineData(1,-1,false)]
    public void EveryRealMinuteCanBeRepresentedButInvalidClockValuesCannot(
        int hour,int minute,bool valid)
    {
        Assert.Equal(valid,ClockItemsAreRepresentable(
            new [] {hour.ToString(CultureInfo.InvariantCulture),
                minute.ToString(CultureInfo.InvariantCulture)}));
    }

    [Fact]
    public void ClockRejectsMissingOrNonNumericHandData()
    {
        Assert.False(ClockItemsAreRepresentable(Array.Empty<string>()));
        Assert.False(ClockItemsAreRepresentable(new []{"1"}));
        Assert.False(ClockItemsAreRepresentable(new []{"1","1:37"}));
        Assert.False(ClockItemsAreRepresentable(new []{"hour","37"}));
    }

    private static double? ParseRational(string raw)
    {
        if (double.TryParse(raw,NumberStyles.Float,
            CultureInfo.InvariantCulture,out var numeric))
            return double.IsFinite(numeric)?numeric:null;
        var parts=raw.Split('/');
        if (parts.Length==2 &&
            double.TryParse(parts[0],NumberStyles.Float,CultureInfo.InvariantCulture,
                out var n) &&
            double.TryParse(parts[1],NumberStyles.Float,CultureInfo.InvariantCulture,
                out var d) && d!=0)
            return n/d;
        return null;
    }

    private static string FindRoot()
    {
        DirectoryInfo? dir=new(Directory.GetCurrentDirectory());
        while(dir is not null)
        {
            if(File.Exists(Path.Combine(dir.FullName,
                "artifacts/academic-us-ccss/batches/B01-lesson-manifest.json")))
                return dir.FullName;
            dir=dir.Parent;
        }
        throw new FileNotFoundException("B01 whole-batch manifest missing");
    }
}
