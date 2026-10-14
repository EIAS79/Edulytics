using Edulytics.Core.Curriculum;
using Edulytics.Core.Entities;

namespace Edulytics.Data.Seeding;

/// <summary>
/// V9 collective source-purpose root: 31 original geometry-like default lessons
/// reviewed together; repair 28 mismatched still-effective step/mistake fields in
/// one source-bound cohort with 22 mathematical-purpose strategy variants.
/// Preserve the true geometry-classification control and 2 earlier corrections.
/// Source activity excerpts/figures are NOT republished. Not L1-L6 certification.
/// </summary>
public static class CommonCoreB01V9SharedGeometryProcedureCorrections
{
    public const string CorrectedVersion = "usccss-b01-v9-shared-geometry-procedure-28-v1";
    private const string OldSteps = "Step 1: Identify the figure and the attributes named in the task.\nStep 2: Measure, count, or compare sides, angles, corners, or other relevant features.\nStep 3: Use the attributes to classify, construct, or reason about the figure.\nStep 4: Explain which attributes support the result.\nStep 5: Check that the conclusion does not depend only on size or orientation.";
    private const string OldMistakes = "Common mistake: classifying a figure by appearance, size, or orientation. Use its mathematical attributes instead.";
    private sealed record Guard(string Code,string SourceUrl,string RawSha,
        string Outcomes,string MathFamily);
    private static readonly Guard[] Cohort =
    [
        new Guard("PED:US-CCSS-MATH:G2:U06:L03", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-6/lesson-3/preparation.html", "940b693c00be0380fb8449140b2648a3aa593291b5894f89643fcd9f7eb15c91", "CCSS:2.G.A.1|CCSS:2.MD.A.1", "DRAW_TO_SIDE_LENGTHS"),
        new Guard("PED:US-CCSS-MATH:G2:U08:L11", "https://im.kendallhunt.com/k5/teachers/grade-2/unit-8/lesson-11/preparation.html", "030aa078fcfa9ec220f64cee9e5c18b39fbeb418ef855764a35d9f31cd19910e", "CCSS:2.G.A.2|CCSS:2.OA.C.3|CCSS:2.OA.C.4", "RECTANGLE_TILE_ARRAY"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L01", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-1/preparation.html", "ecf441d1960f3c978e50703629e3a9c49f2e795f70a375b3c96ec182bd813dae", "CCSS:3.MD.C.5|CCSS:3.OA.A.1", "INTRO_COMPARE_AREA"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L02", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-2/preparation.html", "2ef0ee20e4479b40b243a8aabaac1b73006d8e25110019fbb0fb6b467268278b", "CCSS:3.MD.C.5", "AREA_EQUAL_SQUARE_TILING"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L03", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-3/preparation.html", "b97b925be05925c9c69eccee2795afa0d44dcd03ee3e67702022b17c3f0502a6", "CCSS:3.MD.C.5|CCSS:3.MD.C.6", "AREA_EQUAL_SQUARE_TILING"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L04", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-4/preparation.html", "88cfe9429d98f2f882ff19325128c2c8c64c85fb1939eb8e36dbe17b66a32231", "CCSS:3.MD.C.6", "AREA_ROWS_AND_COLUMNS"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L06", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-6/preparation.html", "dfb747e5d4ec7742221932cdf25098f420b86576970ec2b3b8c31024817726f5", "CCSS:3.MD.C.6", "AREA_SQUARE_UNIT_SELECTION"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L07", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-7/preparation.html", "18ebf8844412a41cdb7b320da1d27cc87bd11d763b801696269106f8c8b473f6", "CCSS:3.MD.C.6", "AREA_SQUARE_UNIT_SELECTION"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L08", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-8/preparation.html", "464c7453314acf23967c71454cfacb25e7ed45e63715f937dbdcfe26ad245fd6", "CCSS:3.MD.C.7|CCSS:3.OA.B.5", "AREA_ROWS_AND_COLUMNS"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L09", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-9/preparation.html", "f996776931feedfacfda4b8aca8fcfcfe9523e7bed041a4d6de06268cc3f2d6a", "CCSS:3.MD.C.7|CCSS:3.OA.B.5", "AREA_SIDE_LENGTH_PRODUCT"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L10", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-10/preparation.html", "260d82f51de42f7e1f47bcb91bf4f5e1c5d67470e5b0808c279016e0c279c550", "CCSS:3.MD.C.7|CCSS:3.OA.B.5", "AREA_REAL_WORLD_CONTEXT"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L12", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-12/preparation.html", "c4f88a93e04bf93fc6f063c81743f3b2d8192a662bf9a68c68257f2f8849a4c7", "CCSS:3.MD.C.7", "AREA_ADDITIVE_COMPOSITE"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L13", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-13/preparation.html", "bb5bebf143444b70cc93ef5eb09291acae9f7681c6347095962081dd12c986ab", "CCSS:3.MD.C.7|CCSS:3.NBT.A.2", "AREA_ADDITIVE_COMPOSITE"),
        new Guard("PED:US-CCSS-MATH:G3:U02:L14", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-2/lesson-14/preparation.html", "e157145d94d9850e8f0fbf007b2d0d731f951da90125139819e06740deb17a8d", "CCSS:3.MD.C.7", "AREA_MISSING_LENGTH"),
        new Guard("PED:US-CCSS-MATH:G3:U04:L10", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-10/preparation.html", "f99cf628a0fab35040b61a86338e0c1158bd9a709f39d1bf527da95833128732", "CCSS:3.MD.C.7|CCSS:3.OA.C.7", "DISTRIBUTIVE_RECTANGLE_MULTIPLICATION"),
        new Guard("PED:US-CCSS-MATH:G3:U04:L11", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-4/lesson-11/preparation.html", "22c621a374729fce1b200753918143a74e10bfbb7132656d83a87d5ed10bcf09", "CCSS:3.MD.C.7|CCSS:3.OA.C.7", "DISTRIBUTIVE_RECTANGLE_MULTIPLICATION"),
        new Guard("PED:US-CCSS-MATH:G3:U06:L07", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-6/lesson-7/preparation.html", "4a02884cecf813100c414502e37752f990aad5d060d2e9e140d3096f876eef31", "CCSS:3.MD.A.2", "ESTIMATE_LIQUID_VOLUME_L"),
        new Guard("PED:US-CCSS-MATH:G3:U06:L08", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-6/lesson-8/preparation.html", "45a3be1318882ed638f1c99419b1dfb7e36c6c78be7a2e3e6ae87a84225f4132", "CCSS:3.MD.A.2|CCSS:3.OA.C.7", "MEASURE_LIQUID_VOLUME_L"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L06", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-6/preparation.html", "c1231a52553453e0c5568f68fc908b247d53728ef1a65a656cc2bf829221933e", "CCSS:3.MD.D.8", "PERIMETER_BOUNDARY_SUM"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L08", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-8/preparation.html", "bd392237dce9010ed57f3f56a207b8d2c0e6da93f4bd40c400a2c8013a6d534a", "CCSS:3.MD.D.8|CCSS:3.OA.C.7", "PERIMETER_BOUNDARY_SUM"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L09", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-9/preparation.html", "0047b23d01119fd1d150bb172bb4f901d6af5410018e82cdff5887269d819cdc", "CCSS:3.MD.D.8", "PERIMETER_UNKNOWN_SIDE"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L10", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-10/preparation.html", "5c6b02b4288c0545da2dd41af807e06f4c90a78ad244917206f4c951d78a0d72", "CCSS:3.MD.D.8|CCSS:3.OA.C.7|CCSS:3.OA.D.8", "AREA_AND_PERIMETER_SOLVE"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L11", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-11/preparation.html", "fa0d6e53b516b1e649bae609cd5a4915d7c50b0ec21a5a45bf76874a0c8c0f6e", "CCSS:3.MD.D.8|CCSS:3.OA.C.7", "SAME_PERIMETER_DIFFERENT_AREA"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L12", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-12/preparation.html", "b5cfa197ca8006e8ae8435000c8baa576791f4a965a38660a38ab06d7a373388", "CCSS:3.MD.D.8|CCSS:3.OA.C.7", "SAME_AREA_DIFFERENT_PERIMETER"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L13", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-13/preparation.html", "53c872dcf3b0a8a2334445f512c093efbd7cc22783606afd055bc778cc02bab4", "CCSS:3.MD.D.8", "GEOMETRY_PARK_DESIGN"),
        new Guard("PED:US-CCSS-MATH:G3:U07:L14", "https://im.kendallhunt.com/k5/teachers/grade-3/unit-7/lesson-14/preparation.html", "1514f520c67f6a5160bbde18ee34080b2a93e2af9ec65d33256820ba01c79faf", "CCSS:3.G.A.1|CCSS:3.MD.D.8", "GEOMETRY_WAX_PATTERN"),
        new Guard("PED:US-CCSS-MATH:G4:U01:L02", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-1/lesson-2/preparation.html", "d1059ce7e56b5c61d9db3b62d8c8c8d0ddbeaa39f2fcbbcbbdee9b9b8d245aa3", "CCSS:4.OA.B.4", "FACTOR_PAIR_RECTANGLES"),
        new Guard("PED:US-CCSS-MATH:G4:U01:L08", "https://im.kendallhunt.com/k5/teachers/grade-4/unit-1/lesson-8/preparation.html", "aa8fa8c24ac3bba53600725e8f3a92d16e35a644d2783c121cd3183087eb0d5c", "CCSS:4.OA.B.4", "RECTANGLE_ART_FACTOR_MULTIPLE_DESIGN")
    ];
    private static readonly IReadOnlyDictionary<string,Guard> ByCode =
        Cohort.ToDictionary(x=>x.Code,StringComparer.Ordinal);

    public static int ExpectedCount => Cohort.Length;
    public static int FamilyCount => Cohort.Select(x=>x.MathFamily).Distinct().Count();

    private static (string Steps,string Mistakes) ForFamily(string mathFamily) =>
        mathFamily switch
        {
        "DRAW_TO_SIDE_LENGTHS" => ("Step 1: Identify the required polygon and its specified side lengths and corner attributes.\nStep 2: Use a ruler to measure each side precisely, draw and connect segments, then check that the complete figure meets the specifications.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: drawing an approximate outline without measuring the specified lengths. Verify each required side with a consistent scale."),
        "RECTANGLE_TILE_ARRAY" => ("Step 1: Identify the rectangle dimensions, equal square tiles or partially-partitioned grid given in the task.\nStep 2: Complete the equal-sized grid without gaps and count equal rows and columns to represent the total squares.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: counting only a visible portion of a partially partitioned rectangle. Complete the equal square grid before counting."),
        "INTRO_COMPARE_AREA" => ("Step 1: Identify the two plane regions and decide which mathematical features can show the amount of surface each covers.\nStep 2: Cover or compare the regions with same-size units or non-overlapping matching pieces, then justify which has more or equal area.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: comparing boundary lengths or visual orientation instead of how much surface is covered. Compare equal-area units."),
        "AREA_EQUAL_SQUARE_TILING" => ("Step 1: Identify the region and the unit square to use for measuring surface coverage.\nStep 2: Cover every part with equal unit squares, with no gaps or overlaps; count squares once to determine area in square units.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: comparing rectangles by shape instead of total equal unit squares, or leaving empty spaces when tiling."),
        "AREA_ROWS_AND_COLUMNS" => ("Step 1: Identify the equal-sized grid structure or inferred rows and columns in a rectangular region.\nStep 2: Count rows and columns; use rows times squares per row, including unmarked squares when the grid is partial.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: counting boundary squares only or treating unequal rows as a rectangular grid. Verify the full array dimensions."),
        "AREA_SQUARE_UNIT_SELECTION" => ("Step 1: Identify the rectangle and select a square unit appropriate to its size (square inches, centimeters, feet or meters).\nStep 2: Tile or estimate using consistent square units, record area with squared units and explain how the chosen unit affects the count.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: treating centimeters and square centimeters as interchangeable or mixing two unit sizes in one area count."),
        "AREA_SIDE_LENGTH_PRODUCT" => ("Step 1: Identify a rectangle and measure its perpendicular side lengths with the ruler in consistent units.\nStep 2: Compute length times width and label the result in square units; verify that the product counts the implied square array.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: adding length and width to compute area or labeling the area in linear units instead of square units."),
        "AREA_REAL_WORLD_CONTEXT" => ("Step 1: Identify the real surface that needs covering and any given side lengths or component rectangles.\nStep 2: Represent the region, find unit-square coverage or multiply rectangular side lengths, and explain area in square units.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: using a boundary/perimeter calculation when the situation asks for the amount of surface to cover."),
        "AREA_ADDITIVE_COMPOSITE" => ("Step 1: Identify a figure made of rectangular parts and mark a complete partition without overlapping regions.\nStep 2: Find each rectangle's area by rows and columns or multiplication, add disjoint part areas and check the total.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: subtracting or adding side lengths without accounting for each rectangular region, or counting overlapping areas twice."),
        "AREA_MISSING_LENGTH" => ("Step 1: Partition the composite rectilinear shape into rectangles and mark the provided side lengths.\nStep 2: Infer any missing side length from aligned segments, multiply side pairs and add the non-overlapping component areas.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: guessing a missing side from visual size or combining segment lengths that do not share the same direction."),
        "DISTRIBUTIVE_RECTANGLE_MULTIPLICATION" => ("Step 1: Identify the side factors and draw a rectangle representing the total product.\nStep 2: Split one factor into convenient addends, label equal-width subrectangles, then add their areas to verify the distributive multiplication equation.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: omitting a part of the decomposed factor or using inconsistent widths when applying the distributive property."),
        "ESTIMATE_LIQUID_VOLUME_L" => ("Step 1: Identify the container, capacity being considered and the informal or liter units for comparing liquids.\nStep 2: Make a reasonable liter estimate, test by filling or transferring a known-volume unit and revise the estimate.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: estimating how much liquid fits by comparing container heights or shapes only. Check actual filled liter quantities."),
        "MEASURE_LIQUID_VOLUME_L" => ("Step 1: Identify the liquid and the measuring container marked with liters (L).\nStep 2: Predict the volume, carefully measure with consistent 1-liter units or graduated marks, and record the measured volume in L.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: confusing a container's shape with its capacity, or reporting linear dimensions instead of liters."),
        "PERIMETER_BOUNDARY_SUM" => ("Step 1: Trace the full outer boundary of the flat shape and identify each side length.\nStep 2: Measure or use labeled side lengths and add every boundary segment exactly once to find perimeter in linear units.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: finding surface area or counting only some sides instead of adding the entire outside boundary."),
        "PERIMETER_UNKNOWN_SIDE" => ("Step 1: Identify the known perimeter and known boundary segments, including any equal side properties.\nStep 2: Subtract the sum of known side lengths from the full perimeter to find the unknown segment(s), and check by recomputing the boundary.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: computing rectangle area or guessing an unknown boundary from appearance when the perimeter sum is given."),
        "AREA_AND_PERIMETER_SOLVE" => ("Step 1: Read the problem and identify separately whether the question asks for area or for perimeter.\nStep 2: For area, multiply the perpendicular rectangle dimensions in square units; for perimeter, add all outer sides in linear units.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: using the same formula or unit for both perimeter and area; distinguish boundary distance from surface coverage."),
        "SAME_PERIMETER_DIFFERENT_AREA" => ("Step 1: Choose a fixed perimeter and construct several rectangles with side pairs whose doubled sum equals that perimeter.\nStep 2: Find each rectangle's area by multiplying its side lengths, then compare different areas while perimeter stays fixed.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: assuming rectangles with equal perimeter must have equal area; check both 2(length + width) and length × width."),
        "SAME_AREA_DIFFERENT_PERIMETER" => ("Step 1: Choose a fixed area and find different factor pairs that can form rectangle side lengths.\nStep 2: Draw each rectangle, calculate its perimeter by summing all sides and show how perimeters vary with the same area.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: assuming equal areas imply equal perimeters; compare multiple side-length factor pairs."),
        "GEOMETRY_PARK_DESIGN" => ("Step 1: Read the dimensions and constraints of the park-design problem and represent the proposed flat regions.\nStep 2: Identify shape boundaries and required rectangular areas; calculate path/fence perimeter and play-area size where relevant.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: planning a park with dimensions that violate the stated space, or confusing perimeter needed for fencing with area needed for covering."),
        "GEOMETRY_WAX_PATTERN" => ("Step 1: Identify the repeated shapes and symmetry or quadrilateral attributes to be used in a wax-print design.\nStep 2: Create a geometric repeating pattern, justify the shapes' defining attributes and solve any linked measurement or grouping problems.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: treating an irregular outline as a rectangle or rhombus by appearance alone. Check vertices, sides and parallel lines."),
        "FACTOR_PAIR_RECTANGLES" => ("Step 1: Identify a given area in unit squares and list whole-number factor pairs of that area.\nStep 2: Construct rectangles with those side lengths, verify each product and explain why each pair yields the same area.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: treating a factor pair's sum as the rectangle area. Its product counts the unit squares."),
        "RECTANGLE_ART_FACTOR_MULTIPLE_DESIGN" => ("Step 1: Identify the overall rectangular grid size and constraints on the number, size and arrangement of rectangles.\nStep 2: Partition the grid into rectangles with whole-number side lengths, record factor and multiple relationships and verify all area constraints.\nStep 3: Explain the mathematical model or calculation and any needed units.\nStep 4: Verify the result against the original task and the relevant mathematical relationship.", "Mistake: making a design whose rectangles overlap or exceed the grid, or assuming any side lengths multiply to the prescribed area."),
            _ => throw new InvalidOperationException("Unreviewed original geometry root family: "+mathFamily)
        };

    public static bool IsTarget(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson) =>
        doc.PackCode=="US-CCSS-MATH"&&doc.VersionCode=="CCSSM-2010"&&
        ByCode.ContainsKey(lesson.LessonCode);

    private static Guard Verify(CanonicalLessonContentPackLesson l)
    {
        var g=ByCode[l.LessonCode];
        if(l.SourceUrl!=g.SourceUrl||
            !string.Equals(l.CanonicalBodySha256,g.RawSha,StringComparison.OrdinalIgnoreCase)||
            string.Join("|",l.OutcomeCodes)!=g.Outcomes||
            l.Translations.Count!=1||l.Translations[0].CultureCode!="en")
            throw new InvalidOperationException("V9 original geometry shared source/standard/hash drift: "+l.LessonCode);
        return g;
    }

    public static void Apply(CanonicalLessonContentPackDocument doc)
    {
        if(doc.PackCode!="US-CCSS-MATH"||doc.VersionCode!="CCSSM-2010")return;
        foreach(var lesson in doc.Lessons)
        {
            if(!ByCode.ContainsKey(lesson.LessonCode))continue;
            var g=Verify(lesson);
            var t=lesson.Translations[0];
            var (steps,mistakes)=ForFamily(g.MathFamily);
            if(t.StepByStepSolutions==steps&&t.CommonMistakes==mistakes)continue;
            if(t.StepByStepSolutions!=OldSteps||t.CommonMistakes!=OldMistakes)
                throw new InvalidOperationException("V9 unexpected source-shared geometry partial body drift: "+lesson.LessonCode);
            t.StepByStepSolutions=steps;
            t.CommonMistakes=mistakes;
        }
    }

    public static string GetExpectedContentVersion(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string previous) =>
        IsTarget(doc,lesson)?CorrectedVersion:previous;

    public static bool CanUpgradeExisting(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version) =>
        IsTarget(doc,lesson)&&
        (version==CommonCoreRichContentCorrections.CorrectionContentVersion||
         version==CorrectedVersion);

    public static void ValidateExistingBodyBeforeUpgrade(CanonicalLessonContentPackDocument doc,
        CanonicalLessonContentPackLesson lesson,string version,
        IReadOnlyCollection<CurriculumLessonContentTranslation> persisted)
    {
        if(!CanUpgradeExisting(doc,lesson,version))
            throw new InvalidOperationException("V9 geometry procedure predecessor refused: "+lesson.LessonCode);
        var g=Verify(lesson);
        if(persisted.Count==0)return;
        if(persisted.Count!=1||persisted.Single().CultureCode!="en")
            throw new InvalidOperationException("V9 geometry procedure persisted culture drift: "+lesson.LessonCode);
        var t=persisted.Single();
        var effective=lesson.Translations.Single();
        var (newSteps,newMistakes)=ForFamily(g.MathFamily);
        if((t.StepByStepSolutions!=OldSteps||t.CommonMistakes!=OldMistakes)&&
           (t.StepByStepSolutions!=newSteps||t.CommonMistakes!=newMistakes))
            throw new InvalidOperationException("V9 geometry procedure persisted older/new body drift: "+lesson.LessonCode);
        if(t.Title!=effective.Title||t.Explanation!=effective.Explanation||
           t.KeyConceptsAndRules!=effective.KeyConceptsAndRules||
           t.WorkedExamples!=effective.WorkedExamples||
           t.QuickSummary!=effective.QuickSummary)
            throw new InvalidOperationException("V9 geometry procedure unrelated student content drift: "+lesson.LessonCode);
    }
}