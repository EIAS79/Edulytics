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
    // Unit 1 Lesson 13 uses the same solid-geometry exact infrastructure.
    public const string L13Polyhedra = "usccss.g6.u01.l13.polyhedron_and_net_logic";
    public const string L13Measures = "usccss.g6.u01.l13.source_faces_edges_vertices";
    public const string L14NetArea = "usccss.g6.u01.l14.source_net_surface_area";
    public const string L14NetLogic = "usccss.g6.u01.l14.net_folding_face_logic";
    internal sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family)=>family is Prism or Meaning or L13Polyhedra or L13Measures
       or L14NetArea or L14NetLogic;
    private static int Count(string family)=>family==Prism?14:family==Meaning?8:
        family==L13Polyhedra?15:family==L13Measures?13:
        family==L14NetArea?10:family==L14NetLogic?11:0;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new ArgumentException("Unknown Grade 6 L12 source family",nameof(family));
        ArgumentNullException.ThrowIfNull(random);
        int v=random.Next(Count(family));
        var t=family==Prism?PrismProblem(v):family==Meaning?MeaningProblem(v):
            family==L13Polyhedra?L13PolyhedraProblem(v):
            family==L13Measures?L13MeasuresProblem(v):
            family==L14NetArea?L14NetAreaProblem(v):L14NetLogicProblem(v);
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
        L14NetArea=>v switch
        {
            0=>82m, // Source original14.2 net A 5×6×1 box
            1=>48m, // Source 14.2 net B square base4×4 and four triangular faces b4 h4
            2=>48m, // Source 14.2 net C 3-4-5 right triangular prism length3
            3=>72m, // Original practice2 net 3-4-5 triangular prism length5
            4=>52m, // Original lesson summary 4×3×2 cuboid
            5=>40m, // Separate first-party pyramid b4 h3
            6=>5m, // Triangular prism total faces
            7=>6m, // Rectangular prism total faces
            8=>5m, // Square pyramid total faces
            9=>12m, // Original practice2 P2 two 3-4-5 right triangle end faces
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        L14NetLogic=>v switch
        {
            0=>1m,1=>1m,2=>1m,3=>0m,4=>1m,5=>0m,
            6=>1m,7=>1m,8=>0m,9=>1m,10=>0m,
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        L13Polyhedra=>v switch
        {
            0=>1m,1=>1m,2=>0m,3=>1m,4=>0m,
            5=>0m,6=>1m,7=>0m,8=>0m,9=>1m,
            10=>1m,11=>0m,12=>1m,13=>1m,14=>0m,
            _=>throw new ArgumentOutOfRangeException(nameof(v))
        },
        L13Measures=>v switch
        {
            0=>5m, // Triangular prism faces
            1=>5m, // Square pyramid faces
            2=>4m, // Tetrahedron faces
            3=>6m, // Tetrahedron edges
            4=>4m, // Tetrahedron vertices
            5=>10m, // Octagonal prism faces
            6=>24m, // Octagonal prism edges
            7=>16m, // Octagonal prism vertices
            8=>9m, // Original practice4 6*3/2
            9=>31m, // Original practice5 2*(7*5/2)-2*2
            10=>6m, // Cube faces
            11=>12m, // Cube edges
            12=>8m, // Cube vertices
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
        if(family==Meaning||family==L13Polyhedra||family==L14NetLogic)return answer is ("0" or "1") &&
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

    private static (string Prompt,string Explanation) L14NetAreaProblem(int v)=>v switch
    {
        0=>("ORIGINAL14.2 source net A: rectangular-prism face pairs 5×6 twice, 5×1 twice and 6×1 twice. Find total surface area.","2×(30+5+6)=82 square units."),
        1=>("ORIGINAL14.2 source net B: square pyramid has square base side4 and FOUR triangular sides each base4 perpendicular triangular height4. Find surface area.","Base16 + 4×(4×4÷2)=16+32=48."),
        2=>("ORIGINAL14.2 source net C: right triangular-prism base has legs3 and4, hypotenuse5; prism length3. Three rectangular sides are 3×3, 4×3, 5×3 and two right triangular ends each area6. Find total surface area.","Rectangles9+12+15=36; two triangular ends12; total48."),
        3=>("ORIGINAL practice2 source net: triangular prism length5 with a 3–4–5 right triangle at each end. Three rectangles 3×5,4×5,5×5. Find total surface area.","15+20+25+2×(3×4÷2)=60+12=72."),
        4=>("Original lesson summary: a rectangular prism has face pairs4×2,3×2 and4×3. Find total surface area.","2×(8+6+12)=52."),
        5=>("Separate existing Edulytiks worked example, not publisher net B: square pyramid base side4, four triangular sides each perpendicular face height3. Find surface area.","16+4×(4×3÷2)=40; distinct from original net B whose face height4."),
        6=>("Original14.2 source net C forms a triangular prism with two triangular ends and three rectangles. How many exterior faces?","2+3=5."),
        7=>("Original14.2 source net A is a rectangular prism with three pairs of congruent rectangles. How many outside faces?","Three pairs make6."),
        8=>("Original14.2 source net B forms a square pyramid. How many outside faces?","Square base1 plus triangular sides4 equals5."),
        9=>("Original practice2 right-triangle end face base4 height3: the triangular prism has two congruent triangular ends. Find their COMBINED area.","2×(3×4÷2)=12."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) L14NetLogicProblem(int v)=>v switch
    {
        0=>("Original 14.1: a valid cube net can be folded without overlapping faces, using exactly six square faces. 1=true,0=false.","Six correct squares and complete nonoverlapping adjacency are required."),
        1=>("Original14.2: the UNFOLDED two-dimensional net shows every exterior face, so adding exactly those polygon areas gives surface area. 1=true,0=false.","A valid net exposes all the solid's outer polygons."),
        2=>("Original14.2: temporary unshaded glue/tape FLAPS are not exterior faces when folded. 1=true,0=false.","Only the colored faces contribute to surface area."),
        3=>("Original14.2: if a net has the right six polygons, it MUST fold into a prism even when face adjacency makes overlap. 1=true,0=false.","Matching face inventory alone is insufficient; adjacency must fold."),
        4=>("Original14.1: a triangular prism net has TWO congruent triangles and THREE rectangles. 1=true,0=false.","Prism has triangular parallel bases and3 lateral rectangular faces."),
        5=>("Original14.1: a net of only SIX rectangles can form a triangular prism. 1=true,0=false.","Triangular prism requires two triangular faces."),
        6=>("Original14.2: net A has three pairs of congruent rectangles and may form a rectangular prism. 1=true,0=false.","Rectangular prism net source A has six rectangles of paired dimensions."),
        7=>("Original14.2: net B consists of one square plus four triangles that fold into a square pyramid. 1=true,0=false.","Net B has exactly pyramid base and4 triangular lateral faces."),
        8=>("Original14.2: net C's two triangular faces and three rectangles fold to a RECTANGULAR prism rather than a triangular prism. 1=true,0=false.","Those are the five faces of a triangular prism."),
        9=>("Original practice5: square meters and square inches are valid UNITS for surface area. 1=true,0=false.","Surface area is measured in two-dimensional square units."),
        10=>("Original practice5: meters CUBED describe the total surface area of a polyhedron. 1=true,0=false.","Cubic units describe volume, not surface area."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) L13PolyhedraProblem(int v)=>v switch
    {
        0=>("Original practice1 picture A is a closed rectangular pyramid with all flat polygonal faces. Polyhedron? 1=yes,0=no.","Pyramid is a closed polyhedron."),
        1=>("Original practice1 picture B is a closed many-faced solid whose faces are polygons. Polyhedron? 1=yes,0=no.","Number of faces does not disqualify a fully closed polygon-faced solid."),
        2=>("Original practice1 picture C is a cone with a curved lateral surface. Polyhedron? 1=yes,0=no.","Curved sides are not polygonal faces."),
        3=>("Original practice1 picture D is a closed hexagonal prism. Polyhedron? 1=yes,0=no.","A prism with flat polygon faces is a polyhedron."),
        4=>("Original practice1 picture E is a smooth sphere. Polyhedron? 1=yes,0=no.","A sphere has a curved exterior, not polygonal faces."),
        5=>("Original practice3: Tyler says a SQUARE-BASED prism net is impossible unless ALL six faces are squares. Is his claim correct? 1=yes,0=no.","Two square bases and four connecting rectangles can fold into a square prism that is not a cube."),
        6=>("Original13.2: a prism has two congruent PARALLEL polygonal bases joined by lateral polygons. 1=true,0=false.","The two matching bases characterize a prism."),
        7=>("Original13.2: a pyramid has two identical parallel polygonal bases like a prism. 1=true,0=false.","A pyramid has ONE base and lateral triangles meeting at an apex."),
        8=>("Original13.2: four triangles that touch only at isolated VERTICES automatically form a foldable tetrahedron net. 1=true,0=false.","Complete shared edges are needed for a net."),
        9=>("Original13.2: source net3 cannot fold into Pyramid P because faces would overlap or meet only at vertices. 1=true,0=false.","Net3 is an original explicitly invalid fold arrangement."),
        10=>("Original13.2: a triangular prism has two triangular bases and three rectangles joining them. 1=true,0=false.","Two triangles and three lateral rectangles give five faces."),
        11=>("Original13.1: an OPEN-top box without its closing face is a closed polyhedron enclosing a 3D region. 1=true,0=false.","Open surfaces do not enclose the space as a polyhedron."),
        12=>("Original13.2: source triangular-pyramid net1 is foldable, using four triangles in a connected strip. 1=true,0=false.","A valid four-triangle edge-connected strip may fold as tetrahedron."),
        13=>("Original13.2: source triangular-pyramid net2 folds into a tetrahedron from four triangles arranged as a larger triangle. 1=true,0=false.","Central triangle with three outward triangles is a valid tetrahedron net."),
        14=>("Original13.2: source net3, four triangles touching at vertices but overlapping when folded, is a valid tetrahedron net. 1=true,0=false.","Net3 fails the source folding test."),
        _=>throw new ArgumentOutOfRangeException(nameof(v))
    };

    private static (string Prompt,string Explanation) L13MeasuresProblem(int v)=>v switch
    {
        0=>("First-party model aligned to original13.2: a triangular prism has 2 triangular and3 rectangular faces. How many total faces?","2+3=5 faces."),
        1=>("First-party model aligned to original13.2: a square pyramid has1 square and4 triangular side faces. How many faces?","1+4=5."),
        2=>("Original13.2: a tetrahedron is the polyhedron with the fewest faces. How many triangular faces?","Four triangular faces."),
        3=>("A tetrahedron has four triangular faces meeting edge-to-edge. How many edges does it have?","Six edges, each shared by2 faces."),
        4=>("A tetrahedron has four triangular faces. How many vertices?","Four vertices."),
        5=>("Original practice2 figure is an OCTAGONAL PRISM: two octagon bases and eight rectangles. How many faces?","2+8=10 faces."),
        6=>("Original practice2 octagonal prism: eight top, eight bottom and eight lateral edges. How many edges?","3×8=24 edges."),
        7=>("Original practice2 octagonal prism: two octagonal bases have how many vertices in total?","2×8=16 vertices."),
        8=>("Original practice4 from prior area lesson: all source triangles have base6 units height3 units. Find one area.","6×3÷2=9 square units."),
        9=>("Original practice5: two congruent right triangles each base7 cm and height5 cm touch along their heights; remove an unshaded square2 cm by2 cm. Find shaded combined area.","2×(7×5÷2)-4=35-4=31 cm²."),
        10=>("Original13.2 cube net has six squares. How many faces does its closed cube have?","A cube has6 faces."),
        11=>("Original13.2 model cube: how many edges are there?","Cube edges total12."),
        12=>("Original13.2 model cube: how many vertices does it have?","Cube vertices total8."),
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
