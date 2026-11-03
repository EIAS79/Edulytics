using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original OUR/IM 6-8 Grade6 U01:L12, independent surface area arithmetic
/// and hidden-face reasoning. Learner-built unit cube prisms, nets, sticky note
/// estimates and explanatory sketches require teacher assessment.
/// </summary>
internal static class UsCcssG6Unit1Lesson12SurfaceAreaQuestions
{
    public const string Prism = "usccss.g6.u01.l12.prism_exposed_face_area";
    public const string Meaning = "usccss.g6.u01.l12.surface_area_meaning";
    internal sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family)=>family is Prism or Meaning;
    private static int Count(string family)=>family==Prism?14:family==Meaning?8:0;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new ArgumentException("Unknown Grade 6 L12 source family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int v=random.Next(Count(family));
        var t=family==Prism?PrismProblem(v):MeaningProblem(v);
        return new Problem(family,t.Prompt,t.Explanation+
            " Teacher checks the actually built 3D figure, all hidden faces, " +
            "orientation, units and learner's written reasoning separately.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"variant",v}});
    }

    private static decimal Answer(string family,int v)=>family switch
    {
        Prism=>v switch
        {
            0=>720m, // Original 12.2 cabinet 24x12x6 bottom excluded
            1=>1008m, // Original 12.2 cabinet all six faces
            2=>32m, // Original 12.3 2x2x3 prism
            3=>40m, // Source alternative 1x2x6 same 12 unit cubes
            4=>38m, // Source alternative 1x3x4 same 12 unit cubes
            5=>50m, // Source alternative 1x1x12 same 12 unit cubes
            6=>118m, // Independent preexisting Edulytiks cabinet 8x5x3 no bottom
            7=>158m, // Independent preexisting closed cabinet 8x5x3
            8=>64m, // Original practice P1 2x4x4 rectangular prism
            9=>22m, // Original P3 5 cubes L shaped, 4 adjacencies
            10=>22m, // Original P3 5 cubes plus shaped, 4 adjacencies
            11=>88m, // Original practice P4 4x2x6 prism
            12=>52m, // Original source lesson summary 2x3x4 prism
            13=>62m, // Owner independent Cool-down, source prep 2x3x5 prism
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        Meaning=>v switch
        {
            0=>1m, // surface area sum of exposed outer unit square faces
            1=>0m, // volume same implies surface area same false
            2=>1m, // 5-sided cabinet covering excludes inaccessible bottom
            3=>0m, // count internal shared cube faces as exposed false
            4=>1m, // original P2 outside-square-feet description appropriate
            5=>0m, // original P2 horizontal inside area not surface area
            6=>1m, // 5-cube L and plus both 22 exposed faces
            7=>0m, // Figure L necessarily has greater SA than plus figure
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        _=>throw new ArgumentException("Unsupported surface-area family",nameof(family))
    };

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new ArgumentException("Invalid source variant",nameof(p));
        return Answer(family,p["variant"]).ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int>? p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer!=answer.Trim())
            return false;
        var expected=Answer(family,p!["variant"]);
        if(family==Meaning)return answer is ("0" or "1") &&
            answer==expected.ToString(CultureInfo.InvariantCulture);
        var parts=answer.Split('/');
        if(parts.Length==2)
        {
            if(!long.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out long n)
                ||!long.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out long d)
                ||n<0||d<=0||n>1000000||d>1000000)return false;
            return n == expected*d;
        }
        return parts.Length==1 &&
           decimal.TryParse(answer,NumberStyles.AllowDecimalPoint,
               CultureInfo.InvariantCulture,out decimal number) &&
           number>=0 && number==expected;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p)=>
        Supports(family)&&p is {Count:1}&&p.TryGetValue("variant",out int v)&&
        v>=0&&v<Count(family!);

    private static (string Prompt,string Explanation) PrismProblem(int v)=>v switch
    {
        0=>("ORIGINAL source12.2 filing cabinet: 24 sticky-note widths long,12 deep,6 high. Cover top+front+back+two sides, NOT the bottom. How many unit square notes?","Top288, front and back288, two sides144, total720. This is not the independent 8-by-5-by-3 example."),
        1=>("ORIGINAL source12.2 cabinet is 24 by12 by6 sticky-note units. If ALL SIX faces including bottom were covered, find the closed prism surface area.","6-face sum2×(24×12+24×6+12×6)=1008."),
        2=>("ORIGINAL source12.3 12 unit cubes form a 2-by-2-by-3 prism. Find all exposed face area in square units.","2×(2×2+2×3+2×3)=32; include hidden bottom and back."),
        3=>("Original source 12.3 skill: 12 cubes form a 1-by-2-by-6 prism. What is its total surface area?","2×(1×2+1×6+2×6)=40."),
        4=>("Source 12.3 alternative arrangement of 12 cubes: a 1-by-3-by-4 prism. Find surface area.","2×(1×3+1×4+3×4)=38."),
        5=>("Source 12.3 alternate 12 cubes in a 1-by-1-by-12 straight prism. Find surface area.","2×(1+12+12)=50."),
        6=>("EDULYTICS independent example, NOT the original source cabinet: 8-by-5-by-3 note widths, cover five faces except bottom. Find unit square notes.","Top40, front/back48 and sides30, total118."),
        7=>("EDULYTICS independent example: completely closed 8-by-5-by-3 rectangular cabinet, cover all six faces. Find surface area.","2×(8×5+8×3+5×3)=158."),
        8=>("Original practice1 figure shows a 2-by-4-by-4 rectangular prism made of unit cubes. Find its surface area; choose among16,32,48,64.","2×(2×4+2×4+4×4)=64, NOT32."),
        9=>("Original practice3 Figure A consists of five unit cubes arranged as a 3-tall vertical column and 3-long bottom row (one shared corner cube). Four face adjacencies. Find exposed unit-square faces.","5×6 - 2×4 = 22, since each shared pair hides two faces."),
        10=>("Original practice3 Figure B consists of five unit cubes in a planar plus-sign cross. Four face adjacencies. Find exposed square faces.","5×6 - 2×4 = 22; neither arrangement has greater surface area."),
        11=>("Original practice4: rectangular prism height4, width2, length6. Find surface area in square units.","2×(4×2+4×6+2×6)=88."),
        12=>("Original source lesson summary rectangular prism edges2cm,3cm,4cm. Find surface area in cm².","2×(6+8+12)=52 square cm."),
        13=>("Edulytiks independent Cool-down, NOT protected original12.4: a 2-by-3-by-5 prism (as in source teacher-prep materials) is built from unit cubes. Find all exposed square faces.","2×(2×3+2×5+3×5)=62. Teacher checks student model and all six faces."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) MeaningProblem(int v)=>v switch
    {
        0=>("Original12.2: the surface area of a closed rectangular prism is the sum of the AREAS of all its outside faces measured in square units. 1=true,0=false.","Unit squares cover external faces without gaps/overlaps."),
        1=>("Original12.3: two prisms assembled from the SAME number of unit cubes MUST have identical surface area. 1=true,0=false.","12-cube 2×2×3 has32; 1×2×6 has40."),
        2=>("Original12.2: when putting notes on a cabinet standing on the floor, the inaccessible bottom is excluded FROM THE COVERING TASK, though a closed prism still has a bottom face. 1=true,0=false.","This source task counts the five reachable outside faces."),
        3=>("Original12.3: surfaces where adjacent unit cubes touch each other are still EXPOSED outer faces. 1=true,0=false.","Internal mating faces are not exposed."),
        4=>("Original practice2 treasure trunk: the NUMBER OF SQUARE FEET covering ALL OUTSIDE faces describes its surface area. 1=true,0=false.","Outside face coverage has two-dimensional square units."),
        5=>("Original practice2 treasure trunk: the number of SQUARE INCHES of just the horizontal INSIDE floor is the whole OUTSIDE surface area. 1=true,0=false.","One interior planar area cannot describe all exterior faces."),
        6=>("Original practice3: five-cube L and plus shapes each have four face-sharing pairs, so both have exposed area22 square units. 1=true,0=false.","30 original cube faces less8 hidden faces =22 each."),
        7=>("Original practice3: Figure A L shape must have strictly GREATER surface area than Figure B five-cube plus shape. 1=true,0=false.","Both have22 exposed unit squares, so neither has greater surface area."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };
}
