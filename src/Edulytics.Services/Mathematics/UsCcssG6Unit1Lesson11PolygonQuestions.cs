using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original OUR/IM 6-8 Grade 6 U01:L11: polygon / nonpolygon
/// classification and area by triangles, trapezoids and decomposition.
/// Mathematics scored here is numeric only; drawing, reasoning and why-not
/// polygons are human-assessed in the first-party source workmat.
/// </summary>
internal static class UsCcssG6Unit1Lesson11PolygonQuestions
{
    public const string Classification = "usccss.g6.u01.l11.polygon_classification";
    public const string Area = "usccss.g6.u01.l11.source_polygon_area";
    public const string G5Shape = "uae.g5.u12.l01.polygon_or_nonpolygon";
    public const string G5Sides = "uae.g5.u12.l01.polygon_side_count";
    public const string G5Vertices = "uae.g5.u12.l01.polygon_vertex_count";
    public const string G5Regular = "uae.g5.u12.l01.regular_polygon";
    internal sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) => family is Classification or Area or G5Shape or G5Sides or G5Vertices or G5Regular;
    private static int Count(string family) =>
        family == Classification ? 10 : family == Area ? 12 :
        family == G5Shape ? 8 : family == G5Sides || family == G5Vertices ? 6 :
        family == G5Regular ? 8 : 0;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new ArgumentException("Unsupported original G6 polygon family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int v=random.Next(Count(family));
        var x=family switch
        {
            Classification=>ClassificationProblem(v),
            Area=>AreaProblem(v),
            _=>G5PolygonProblem(family,v)
        };
        string teacherReview=family is G5Shape or G5Sides or G5Vertices or G5Regular
            ? " يراجع المعلم رسم الطالب وحدود الشكل وتبرير التصنيف باللغة العربية."
            : " Teacher separately checks polygon outline, valid triangulation, " +
              "the student's original drawing and explanation.";
        return new Problem(family,x.Prompt,x.Explanation+teacherReview,
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",v}});
    }

    private static decimal Answer(string family,int v)=>family switch
    {
        Classification=>v switch
        {
            0=>1m, // Original practice1 A straight closed arrow
            1=>0m, // B curved rounded rectangle
            2=>1m, // C simple concave star
            3=>0m, // D open triangle
            4=>0m, // E curved heart
            5=>0m, // F three-dimensional prism (not one plane polygon)
            6=>1m, // Polygon is its boundary; enclosed region has area
            7=>0m, // Meeting at vertex not automatically one polygon
            8=>1m, // All simple polygons triangulable in lesson reasoning
            9=>0m, // Self-crossing path not a simple polygon
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        G5Shape=>v is 0 or 3 or 5 or 7 ? 1m : 0m,
        G5Sides or G5Vertices=>v switch {0=>3m,1=>4m,2=>5m,3=>6m,4=>7m,5=>8m,_=>throw new ArgumentOutOfRangeException(nameof(v))},
        G5Regular=>v is 0 or 2 or 4 or 6 ? 1m : 0m,
        Area=>v switch
        {
            0=>12m, // Original 11.3 quadrilateral A
            1=>18m, // Original 11.3 quadrilateral B
            2=>28m, // Original 11.3 quadrilateral C
            3=>14m, // Original 11.3 quadrilateral D
            4=>15m, // Original 11.3 quadrilateral E
            5=>18m, // Original 11.3 quadrilateral F
            6=>18m, // Original practice2 trapezoid bases8,4 h3
            7=>93.6m, // Original practice3 regular hex side6 and full height10.4
            8=>11m, // Original practice4 triangle base11 h2
            9=>40m, // Original 11.4 source pinwheel (source grid geometry)
            10=>12m, // Separate first-party independent pinwheel example
            11=>32m, // Edulytiks first-party Cool-down b10,6 height4
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        _=>throw new ArgumentException("Unknown original source family",nameof(family))
    };

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p)) throw new ArgumentException("Invalid source polygon variant",nameof(p));
        return Answer(family,p["variant"]).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? text)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(text)||text!=text.Trim())
            return false;
        var expected=Answer(family,p!["variant"]);
        if(family is G5Shape or G5Sides or G5Vertices or G5Regular)
        {
            string normalized=new(text.Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                _=>ch
            }).ToArray());
            return normalized.Length<=16 && int.TryParse(normalized,NumberStyles.None,
                CultureInfo.InvariantCulture,out int g5Integer) && g5Integer==expected;
        }
        if(family==Classification)
            return text is ("0" or "1") &&
                text==expected.ToString(CultureInfo.InvariantCulture);
        var parts=text.Split('/');
        if(parts.Length==2)
        {
            if(!long.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out long n)
                ||!long.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out long d)
                ||n<0||d<=0||n>1000000||d>1000000)
                return false;
            return n == expected*d;
        }
        return parts.Length==1 &&
            decimal.TryParse(text,NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,out decimal number) &&
            number>=0 && number==expected;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family) && p is {Count:1} && p.TryGetValue("variant",out int v) &&
        v>=0 && v<Count(family!);

    private static (string Prompt,string Explanation) G5PolygonProblem(string family,int v)
    {
        if(family==G5Shape)
        {
            string[] descriptions=[
                "حد مستو مغلق من خمس قطع مستقيمة لا تتقاطع إلا عند رؤوسها",
                "إطار بيضاوي مغلق ذو خط منحني",
                "ثلاث قطع مستقيمة متصلة طرفًا بطرف ولكن يوجد فراغ بين البداية والنهاية",
                "مضلع مقعر مغلق من ست قطع مستقيمة لا يتقاطع حدّه",
                "مسار رباعي مستقيم مغلق لكن ضلعين غير متجاورين يتقاطعان",
                "حد مغلق يتكون من أربع قطع مستقيمة متصلة بلا تقاطع",
                "حد مغلق فيه ضلع مستقيم واحد وثلاثة أقواس منحنية",
                "شكل ثماني الأضلاع مغلق لا تتقاطع أضلاعه إلا عند رؤوسها"
            ];
            return ($"الوصف: {descriptions[v]}. هل يُعد مضلعًا بسيطًا؟ اكتب ١ نعم أو ٠ لا.",
                "المضلع المستوي حد مغلق من قطع مستقيمة متصلة دون تقاطع بين غير المتجاور.");
        }
        if(family==G5Sides || family==G5Vertices)
        {
            string[] names=["مثلث","شكل رباعي","شكل خماسي الأضلاع","شكل سداسي الأضلاع","شكل سباعي الأضلاع","شكل ثماني الأضلاع"];
            return ($"لديك {names[v]} منتظم أو غير منتظم. ما عدد {(family==G5Sides?"الأضلاع":"الرؤوس")}؟",
                "في كل مضلع عدد الأضلاع يساوي عدد الرؤوس، ولا يتعلق الأمر بانتظامه.");
        }
        string[] rules=[
            "مربع جميع أضلاعه متساوية وزواياه الأربع قائمة",
            "مستطيل طوله أكبر من عرضه وزواياه الأربع قائمة",
            "سداسي جميع أضلاعه متساوية وزواياه الداخلية الست متساوية",
            "معين أضلاعه متساوية لكن زواياه متبادلة ٦٠° و١٢٠°",
            "مثلث متساوي الأضلاع وزواياه الثلاث متساوية",
            "مثلث متساوي الساقين ضلعاه فقط متساويان",
            "ثماني الأضلاع جميع أضلاعه متساوية وجميع زواياه متساوية",
            "خماسي له أضلاع متساوية لكن ليست جميع زواياه متساوية"
        ];
        return ($"الشكل: {rules[v]}. هل هو مضلع منتظم؟ اكتب ١ نعم أو ٠ لا.",
            "الانتظام يشترط تساوي الأضلاع جميعها وتساوي الزوايا الداخلية جميعها.");
    }
    private static (string Prompt,string Explanation) ClassificationProblem(int v) => v switch
    {
        0=>("Original practice1 figure A is a closed arrow whose boundary uses straight, noncrossing line segments. Is it a plane polygon? 1=yes,0=no.","The concave arrow is a polygon with straight closed boundary."),
        1=>("Original practice1 figure B looks like a rectangle but has rounded upper corners. Polygon? 1=yes,0=no.","Curved edges mean its boundary is not solely straight segments."),
        2=>("Original practice1 figure C is a closed non-self-crossing five-pointed star OUTLINE, not crossing diagonals. Polygon? 1=yes,0=no.","A concave simple polygon may have a star-shaped outline."),
        3=>("Original practice1 figure D looks like a triangle with one side disconnected at its corner. Polygon? 1=yes,0=no.","Open boundaries do not form polygons."),
        4=>("Original practice1 figure E is a smoothly curved heart outline. Polygon? 1=yes,0=no.","Its boundary includes curves, not only straight segments."),
        5=>("Original practice1 figure F depicts a three-dimensional rectangular prism, not one flat closed polygonal boundary. Is the WHOLE figure one plane polygon? 1=yes,0=no.","A polyhedron is not a single plane polygon; its individual faces may be polygons."),
        6=>("Original 11.2: the mathematical polygon refers to a closed straight-sided boundary, while its AREA describes the enclosed region. 1=true,0=false.","Boundary and the region it encloses are distinguished."),
        7=>("Two disjoint triangles that touch only at one vertex automatically form one simple polygon boundary. 1=true,0=false.","A union meeting at one point need not be a single simple polygon."),
        8=>("Original lesson conclusion: any simple polygon can be decomposed into triangles to reason about its enclosed area. 1=true,0=false.","Triangulation supports the source's general area strategy."),
        9=>("A line path that crosses itself before closing counts as a simple polygon with noncrossing boundary. 1=true,0=false.","A simple polygon boundary must not self-intersect."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) AreaProblem(int v) => v switch
    {
        0=>("Original 11.3 quadrilateral A grid vertices (2,3),(5,1),(6,3),(5,7). Find enclosed area in square grid units.","Split at horizontal y3: two triangles area4 and8, total12."),
        1=>("Original 11.3 quadrilateral B grid vertices (9,1),(13,2),(13,8),(9,4). Find enclosed area.","Two vertical parallel sides lengths3 and6, separation4; area(3+6)×4÷2=18."),
        2=>("Original 11.3 quadrilateral C trapezoid has parallel horizontal sides4 and10 and height4. Find enclosed area.","(4+10)×4÷2 = 28."),
        3=>("Original 11.3 quadrilateral D has horizontal diagonal length4, top perpendicular height2 and lower height5. Find its area.","(4×2÷2)+(4×5÷2)=4+10=14."),
        4=>("Original 11.3 quadrilateral E has parallel horizontal sides8 and2 with corresponding height3. Find area.","(8+2)×3÷2 = 15."),
        5=>("Original 11.3 quadrilateral F has perpendicular diamond diagonals6 units each. Find area.","Split into four equal right triangles: 6×6÷2=18."),
        6=>("Original practice2 trapezoid has parallel bases8 and4 units, perpendicular height3. Find its area.","Pair two identical trapezoids to make a parallelogram base12, height3. One area12×3÷2=18."),
        7=>("Original practice3 regular hexagon side6 in and vertical total height10.4 in. Decompose into six congruent triangles, each base6 and height5.2. Find hexagon area in square inches.","6 × (6×5.2÷2) = 93.6 in². Or central rectangle plus two caps."),
        8=>("Original practice4 triangle has a horizontal base11 units and its opposite vertex2 units below that base. Find area.","11×2÷2 = 11 square units."),
        9=>("Original activity11.4 pinwheel grid: a central tilted square encloses20 square units and four non-overlapping triangular blades have area5 each. Find total original SOURCE pinwheel area.","20+4×5 = 40. Decompose source grid; do not use the separate independently authored 12-square-unit illustration."),
        10=>("Edulytiks first-party INDEPENDENT teaching example, not the original source pinwheel: central 2×2 square and four non-overlapping triangular wings each base2,height2. Find total.","Central4, each wing2, so independent illustration totals12."),
        11=>("Edulytiks independent Cool-down, NOT publisher 11.5: trapezoid parallel bases10 cm and6 cm, height4 cm. Find area and draw a triangulation.","(10+6)×4÷2 = 32 cm². Teacher grades accurate construction and explanation."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };
}
