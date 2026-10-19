using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM2021 Grade 1 Unit7 17 lessons: gradeable digital geometry, partition
/// concepts, and time recognition only. The full original objectives include
/// hands-on drawing, composing, rotating objects, actual clock manipulation,
/// oral PLC and center activities requiring a teacher/visual review.
/// </summary>
public static class UsCcssG1FullGradeUnit7GeometryTimeQuestions
{
    public static readonly IReadOnlyList<string> Families=
    [
        "usccss.g1.u07.l01.sort_solids_attributes",
        "usccss.g1.u07.l02.compose_solids_components",
        "usccss.g1.u07.l03.sort_flat_shapers",
        "usccss.g1.u07.l04.draw_shape_attribute_check",
        "usccss.g1.u07.l05.defining_triangles",
        "usccss.g1.u07.l06.square_and_rectangle_properties",
        "usccss.g1.u07.l07.compose_flat_shapers",
        "usccss.g1.u07.l08.center_shapes_add_subtract",
        "usccss.g1.u07.l09.equal_halves_fourths",
        "usccss.g1.u07.l10.parts_make_whole",
        "usccss.g1.u07.l11.bigger_piece_halves_fourths",
        "usccss.g1.u07.l12.center_shape_number",
        "usccss.g1.u07.l13.hour_clock",
        "usccss.g1.u07.l14.half_hour_clock",
        "usccss.g1.u07.l15.write_hours_halfhours",
        "usccss.g1.u07.l16.daily_schedule_time",
        "usccss.g1.u07.l17.center_geometry_and_time"
    ];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static int Index(string? family)
    {
        if(family==null)return -1;
        for(int i=0;i<Families.Count;i++)if(Families[i]==family)return i;
        return -1;
    }
    public static bool Supports(string? family)=>Index(family)>=0;
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(Index(family)<0||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode))return false;
        return a>=1&&a<=12&&b>=1&&b<=12&&c==0&&mode>=0&&mode<=3;
    }
    public static Problem Build(string family,Random random)
    {
        int i=Index(family);
        if(i<0)throw new InvalidOperationException("Unsupported G1 Unit7 original-source family");
        int a=random.Next(1,13),b=random.Next(1,13),mode=random.Next(4);
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        string prompt=i switch
        {
            0=>mode switch
            {
                0=>$"Classify solid shape for object #{a}: it has 6 square FLAT faces and 8 corners. Write SHAPE=name.",
                1=>$"Classify solid shape for object #{a}: it has a CURVED continuous surface and no flat face. Write SHAPE=name.",
                2=>$"Classify solid shape for object #{a}: it has two flat circular faces and a curved side. Write SHAPE=name.",
                _=>$"Classify solid shape for object #{a}: one circular base and one point joined by a curved surface. Write SHAPE=name."
            },
            1=>$"A model for building contains {a} cube pieces and {b} rectangular-prism pieces. Count ALL pieces before teacher verifies the physical composition. Write PIECES=number.",
            2=>mode%2==0?$"Sort shape card #{a}: this CLOSED FLAT shape has 3 STRAIGHT sides. Write SHAPE=name.":
                $"Sort shape card #{a}: this CLOSED FLAT shape has 4 equal straight sides and 4 right angles. Write SHAPE=name.",
            3=>mode%2==0?$"Before DRAWING a triangle for card #{a}, list how many straight sides and corners it MUST have. Write SIDES=number;CORNERS=number.":
                $"Before DRAWING a rectangle for card #{a}, list its straight sides and corners. Write SIDES=number;CORNERS=number.",
            4=>$"Triangle card #{a}: three line segments form a CLOSED boundary (rotation {b} degrees is not being tested). How many defining straight SIDES? Write SIDES=number.",
            5=>mode%2==0?$"Shape card #{a}: a square has 4 equal sides and 4 right angles. Is every SQUARE also a RECTANGLE? Write ANSWER=YES/NO.":
                $"Shape card #{a}: a rectangle has 4 right angles but need not have equal sides. Is every RECTANGLE necessarily a SQUARE? Write ANSWER=YES/NO.",
            6=>$"Two equal right triangles can be placed along their diagonal to form a rectangle. Card #{a}, model #{b}. Count the number of triangular COMPONENTS. Write PARTS=number.",
            7=>mode%2==0?$"Center numerical task {a}+{b} (sum within 24, classroom shape center assessed separately). Write RESULT=number.":
                $"Center shape naming card #{a}: a CLOSED flat figure with exactly 3 straight sides. Write SHAPE=name.",
            8=>mode%2==0?$"A rectangle card #{a} is partitioned into 2 EQUAL-area pieces. What is each piece called? Write PART=name;PIECES=number.":
                $"A circle card #{a} is partitioned into 4 EQUAL-area pieces. What is each piece called? Write PART=name;PIECES=number.",
            9=>mode%2==0?$"Two equal halves rejoin to make one WHOLE rectangle; card #{a}. Write PARTS=number;WHOLES=number.":
                $"Four equal fourths rejoin to make one WHOLE circle; card #{a}. Write PARTS=number;WHOLES=number.",
            10=>$"In the SAME shape #{a}, compare one HALF and one FOURTH. Which is larger? Write LARGER=part_name;FOURTHS_PER_HALF=number.",
            11=>mode%2==0?$"Center arithmetic: add {a}+{b} (within 24). Write RESULT=number.":
                $"Center shape card #{a}: 3 straight sides, 3 corners, closed. Write SHAPE=name.",
            12=>$"Clock card #{b}: the MINUTE hand points at 12, and the HOUR hand points at {a}. Tell the time. Write TIME=hour:minutes.",
            13=>mode<=1?
                $"Half-hour clock #{b}: minute hand at SIX, hour hand halfway between {a} and next hour. Write MINUTES=number;HOUR=number.":
                mode==2?
                $"ORIGINAL CARD SORT: Clock #{b} shows the minute hand at 12 and the short hand at {a}. Color it BLUE for o'clock (RED is half past). Write COLOR=BLUE;TIME=hour:minutes;WORDS=hour_oclock. Use a lowercase spelled-out hour word such as seven_oclock, not a digit.":
                $"ORIGINAL CARD SORT: Clock #{b} shows the minute hand at SIX and the short hand halfway past {a}. Color it RED for half past (BLUE is o'clock). Write COLOR=RED;TIME=hour:minutes;WORDS=half_past_hour (e.g. half_past_seven).",
            14=>mode<=1?(mode==0?
                $"Clock card #{b}: hour hand at {a}, minute hand at 12. Write TIME=hour:minutes.":
                $"Clock card #{b}: hour hand past {a}, minute hand at SIX. Write TIME=hour:minutes."):
                $"ORIGINAL COUNT THE MINUTES: Start at 12 on the clock, count clockwise in steps of five until you reach the minute hand on SIX. How many five-minute HOPS and total MINUTES are in half a clock? Write HOPS=number;MINUTES=number;STOP_AT=number.",
            15=>mode%2==0?
                $"Read the analog clock face for the hypothetical READING schedule. The short hand is at {a} and the minute hand is at 12. What time do you see? Write TIME=hour:minutes.":
                $"Read the analog clock face for a hypothetical SNACK time. The short hand is halfway from {a} to the next hour and the long hand points at 6. Write TIME=hour:minutes.",
            16=>mode%2==0?
                $"Grade 1 center numerical task: subtract {b} from {a+b} (under 24). Write RESULT=number.":
                $"Grade 1 WHICH ONE/Can You Draw It center: a closed four-sided flat shape has 4 right angles and its opposite sides are equal. What SHAPE could it be? Write SHAPE=name.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
            "A source-related digital math and shape concept check. Where a first-party SVG is rendered, it illustrates the mathematical shape, equal-area parts, or actual clock-hand positions; the separate numeric or terminology answer does not certify learner drawings, physical solid/flat construction, clock-card manipulation, games, or teacher-led oral reasoning.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 Unit7 parameters");
        int a=p["a"],b=p["b"],mode=p["mode"];
        return Index(family) switch
        {
            0=>$"SHAPE={(mode==0?"CUBE":mode==1?"SPHERE":mode==2?"CYLINDER":"CONE")}",
            1=>$"PIECES={a+b}",
            2=>$"SHAPE={(mode%2==0?"TRIANGLE":"SQUARE")}",
            3=>$"SIDES={(mode%2==0?3:4)};CORNERS={(mode%2==0?3:4)}",
            4=>"SIDES=3",
            5=>$"ANSWER={(mode%2==0?"YES":"NO")}",
            6=>"PARTS=2",
            7=>mode%2==0?$"RESULT={a+b}":"SHAPE=TRIANGLE",
            8=>mode%2==0?"PART=HALF;PIECES=2":"PART=FOURTH;PIECES=4",
            9=>mode%2==0?"PARTS=2;WHOLES=1":"PARTS=4;WHOLES=1",
            10=>"LARGER=HALF;FOURTHS_PER_HALF=2",
            11=>mode%2==0?$"RESULT={a+b}":"SHAPE=TRIANGLE",
            12=>$"TIME={a}:00",
            13=>mode<=1?$"MINUTES=30;HOUR={a}":
                $"COLOR={(mode==2?"BLUE":"RED")};TIME={a}:{(mode==2?"00":"30")};WORDS={(mode==2?HourName(a)+"_oclock":"half_past_"+HourName(a))}",
            14=>mode<=1?$"TIME={a}:{(mode==0?"00":"30")}":"HOPS=6;MINUTES=30;STOP_AT=6",
            15=>$"TIME={a}:{(mode%2==0?"00":"30")}",
            16=>mode%2==0?$"RESULT={a}":"SHAPE=RECTANGLE",
            _=>throw new InvalidOperationException()
        };
    }
    private static string HourName(int h)=>h switch
    {
        1=>"one",2=>"two",3=>"three",4=>"four",5=>"five",6=>"six",
        7=>"seven",8=>"eight",9=>"nine",10=>"ten",11=>"eleven",12=>"twelve",
        _=>throw new InvalidOperationException("Clock hour outside one through twelve")
    };
    private static string Norm(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(response)&&Norm(Solve(family!,p))==Norm(response);
}
