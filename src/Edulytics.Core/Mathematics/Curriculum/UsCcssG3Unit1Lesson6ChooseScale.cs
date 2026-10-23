namespace Edulytics.Core.Mathematics.Curriculum;

/// <summary>
/// G3 U01 L06 source numerical data from offline IM K5 2021 student-facing
/// teacher HTML. New graph drawings are Edulytics authored, not publisher art.
/// Original 2021 pattern-block SVG was geometrically audited; we author new
/// first-party artwork while retaining the source's exact categorical quantities.
/// </summary>
public static class UsCcssG3Unit1Lesson6ChooseScale
{
    public const string LessonCode = "PED:US-CCSS-MATH:G3:U01:L06";
    public sealed record CategoryCount(string Category, int Count);
    public static readonly IReadOnlyList<CategoryCount> SourcePets =
        Array.AsReadOnly(new []{
            new CategoryCount("Cat",16),new CategoryCount("Dog",13),
            new CategoryCount("Guinea Pig",6),new CategoryCount("Goldfish",10),
            new CategoryCount("Python",30)
        });
    public static readonly IReadOnlyList<CategoryCount> SourceSeasons =
        Array.AsReadOnly(new []{
            new CategoryCount("Winter",24),new CategoryCount("Spring",13),
            new CategoryCount("Summer",40),new CategoryCount("Fall",22)
        });
    public static readonly IReadOnlyList<CategoryCount> SourcePatternBlocks =
        Array.AsReadOnly(new []{
            // The original 2021 first-edition pattern-block SVG's visible filled geometries:
            // 27 green triangle paths contain one exact duplicate SVG path (26 distinct
            // visible triangles), 5 orange squares, 15 red trapezoids, 10 yellow hexagons.
            // SVG URL: https://cms-assets.illustrativemathematics.org/7053kvgtyqg9pibtx7gto33ajaea
            new CategoryCount("Triangle",26),new CategoryCount("Square",5),
            new CategoryCount("Trapezoid",15),new CategoryCount("Hexagon",10)
        });
    // A NEW Edulytics drawn collection, different positions and graphics than the
    // publisher original. The exact category quantities are preserved, not the art.
    public static readonly IReadOnlyList<string> FirstPartyPatternShapeOrder =
        Array.AsReadOnly(Enumerable.Range(0,26)
            .SelectMany(i=>SourcePatternBlocks.Where(row=>i<row.Count)
                .Select(row=>row.Category)).ToArray());
    public static bool TryGradePatternCount(string? category,int answer,out bool correct)
    {
        correct=false;
        var source=SourcePatternBlocks.SingleOrDefault(row=>
            string.Equals(row.Category,category,StringComparison.OrdinalIgnoreCase));
        if(source is null)return false;
        correct=source.Count==answer;
        return true;
    }
    public static readonly IReadOnlyList<string> PatternCategories =
        Array.AsReadOnly(new []{"Triangle","Square","Trapezoid","Hexagon"});
    public static readonly IReadOnlyList<CategoryCount> IndependentCoolDown =
        Array.AsReadOnly(new []{
            new CategoryCount("Music Club",19),new CategoryCount("Science Club",26),
            new CategoryCount("Art Club",15)
        });

    public static int GraphMaximum(int largest, int step)
    {
        if(largest < 0 || largest > 200 || step is not (2 or 5 or 10))
            throw new ArgumentOutOfRangeException(nameof(largest));
        return Math.Max(step,((largest+step-1)/step)*step);
    }
    public static bool TryValidatePatternCounts(IReadOnlyList<CategoryCount>? rows,out string note)
    {
        note="Four source pattern-block categories, with teacher-confirmed counts, are required.";
        if(rows is null || rows.Count!=4)return false;
        if(rows.Any(x=>x.Count<0||x.Count>100)||rows.Sum(x=>x.Count) is < 1 or > 200)
        { note="Pattern-block counts must be nonnegative with a combined total of 1–200.";return false; }
        if(rows.GroupBy(x=>x.Category,StringComparer.OrdinalIgnoreCase).Count()!=4
            || PatternCategories.Any(cat=>!rows.Any(x=>string.Equals(cat,x.Category,StringComparison.OrdinalIgnoreCase))))
        { note="All four nonduplicate source categories are required.";return false; }
        note="These four category names and counts are structurally valid; the source SVG quantities are checked separately, and a teacher still evaluates the drawn bars.";
        return true;
    }
    public static bool TryGradeNumeric(string? id,int answer,out bool correct)
    {
        correct=false;
        int? expected=id switch{
            "L06-SOURCE-PATTERN-TRIANGLE"=>26,
            "L06-SOURCE-PATTERN-SQUARE"=>5,
            "L06-SOURCE-PATTERN-TRAPEZOID"=>15,
            "L06-SOURCE-PATTERN-HEXAGON"=>10,
            "L06-SOURCE-SEASON-BAR-WINTER"=>24,
            "L06-SOURCE-SEASON-BAR-SPRING"=>13,
            "L06-SOURCE-SEASON-BAR-SUMMER"=>40,
            "L06-SOURCE-SEASON-BAR-FALL"=>22,
            "L06-SOURCE-PET-CAT"=>16,
            "L06-SOURCE-PET-PYTHON"=>30,
            "L06-SOURCE-PET-TOTAL"=>75,
            "L06-SOURCE-SEASONS-SPRING"=>13,
            "L06-SOURCE-SEASONS-TOTAL"=>99,
            "L06-SOURCE-SEASONS-SUMMER-MINUS-SPRING"=>27,
            "L06-EDU-CD-MUSIC"=>19,
            "L06-EDU-CD-SCIENCE"=>26,
            "L06-EDU-CD-DIFFERENCE"=>7,
            _=>null
        };
        if(expected is null)return false;
        correct=answer==expected;
        return true;
    }
    public static bool RequiresTeacherReview(string key)=>key is
        "pattern-figure-provenance" or "scale-justification" or
        "pattern-bar-graph" or "seasons-bar-graph" or "source-cooldown-scale" or
        "source-cooldown-learning" or
        "student-bar-graph" or "peer-review" or "reflection-cooldown"
            or "math-community";
}
