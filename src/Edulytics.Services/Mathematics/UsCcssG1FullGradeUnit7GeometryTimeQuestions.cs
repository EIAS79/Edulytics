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
        if(Index(family)==15&&mode>=4)
            return c==0&&(mode switch
            {
                4=>a==8&&b==32,
                5=>a==8&&b==33,
                6=>a==8&&b==38,
                7=>a==8&&b==48,
                8=>a==6&&b==12,
                _=>false
            });
        return a>=1&&a<=12&&b>=1&&b<=12&&c==0&&mode>=0&&
            mode<=(Index(family)==5?6:Index(family)==8?5:((Index(family)==1||Index(family)==2||Index(family)==4||Index(family)==6||Index(family)==7||Index(family)==13)?4:3))&&
            (Index(family)!=1||mode!=4||(a>=2&&a<=4&&b>=2&&b<=3))&&
            (Index(family)!=13||mode!=4||a<=10)&&
            (Index(family)!=12||mode!=3||a!=b);
    }
    public static Problem Build(string family,Random random)
    {
        int i=Index(family);
        if(i<0)throw new InvalidOperationException("Unsupported G1 Unit7 original-source family");
        int a=random.Next(1,13),b=random.Next(1,13),mode=random.Next(i==5?7:i==8?6:i==15?9:i==1||i==2||i==4||i==6||i==7||i==13?5:4);
        if(i==1&&mode==4){a=random.Next(2,5);b=random.Next(2,4);}
        if(i==13&&mode==4)a=random.Next(1,11);
        if(i==12&&mode==3&&b==a)b=a==12?1:a+1;
        if(i==15&&mode>=4)(a,b)=mode switch
        {
            4=>(8,32),5=>(8,33),6=>(8,38),7=>(8,48),
            8=>(6,12),
            _=>throw new InvalidOperationException("Unreviewed source clock/warmup case")
        };
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
            1=>mode==4?
                $"SOURCE-INSPIRED COMPOSE A SOLID (FIRST-PARTY MODEL): Build a rectangular prism using TWO identical layers of equal cubes. Each layer has {b} rows, with {a} cubes in EACH row, all touching face to face. How many unit cubes are needed for ONE layer and for BOTH layers together? Write PER_LAYER=number;LAYERS=number;TOTAL_CUBES=number. Real student construction and source-publisher shape figures are teacher-reviewed.":
                $"A model for building contains {a} cube pieces and {b} rectangular-prism pieces. Count ALL pieces before teacher verifies the physical composition. Write PIECES=number.",
            2=>mode==4?
                $"SOURCE-INSPIRED FLAT-SHAPE RE-SORT, CARD SET #{a}: Four FIRST-PARTY cards depict a closed TRIANGLE, a SQUARE, a non-square RECTANGLE and a CIRCLE. FIRST sort into straight-only boundaries versus curved boundaries. NEXT sort straight-only cards again by THREE or FOUR straight sides. Write CURVED=shape;THREE_SIDES=shape;FOUR_SIDES=shape,shape (square before rectangle, using uppercase names). Teacher observes students' own original card categories and explanations separately.":
                mode%2==0?$"Sort shape card #{a}: this CLOSED FLAT shape has 3 STRAIGHT sides. Write SHAPE=name.":
                $"Sort shape card #{a}: this CLOSED FLAT shape has 4 equal straight sides and 4 right angles. Write SHAPE=name.",
            3=>mode%2==0?$"Before DRAWING a triangle for card #{a}, list how many straight sides and corners it MUST have. Write SIDES=number;CORNERS=number.":
                $"Before DRAWING a rectangle for card #{a}, list its straight sides and corners. Write SIDES=number;CORNERS=number.",
            4=>mode==4?
                $"SOURCE-INSPIRED TRIANGLE AND NON-TRIANGLE REASONING, FIRST-PARTY CARD SET #{a}: Cards A, B, and C each have exactly three straight segments meeting at endpoints to enclose a region (one is rotated and one has a narrow angle). Card D has three straight segments but does NOT close; E is a four-sided square; F has two straight sides and one CURVED side. Identify the three valid TRIANGLES and all three NONTRIANGLES using the defining CLOSED THREE STRAIGHT SIDES rule. Write TRIANGLES=letters;NONTRIANGLES=letters (alphabetical). Actual student creation of THREE triangles and THREE non-triangles is a separate original teacher-reviewed task.":
                $"Triangle card #{a}: three line segments form a CLOSED boundary (rotation {b} degrees is not being tested). How many defining straight SIDES? Write SIDES=number.",
            5=>mode switch
            {
                4=>$"SOURCE-INSPIRED CLASSIFICATION: FIRST-PARTY card #{a} has FOUR equal straight sides AND FOUR right angles. Is it a RECTANGLE? Is it a SQUARE? Write RECTANGLE=YES/NO;SQUARE=YES/NO. Genuine original learner-drawn five rectangles, three nonrectangles, five squares, and three nonsquares require separate teacher review.",
                5=>$"SOURCE-INSPIRED CLASSIFICATION: FIRST-PARTY card #{a} has FOUR right angles, opposite sides equal, but two neighboring sides have DIFFERENT lengths. Is it a RECTANGLE? Is it a SQUARE? Write RECTANGLE=YES/NO;SQUARE=YES/NO.",
                6=>$"SOURCE-INSPIRED CLASSIFICATION: FIRST-PARTY card #{a} has FOUR equal straight sides, but its angles are slanted and NONE is a RIGHT angle. Is it a RECTANGLE? Is it a SQUARE? Write RECTANGLE=YES/NO;SQUARE=YES/NO.",
                _=>mode%2==0?$"Shape card #{a}: a square has 4 equal sides and 4 right angles. Is every SQUARE also a RECTANGLE? Write ANSWER=YES/NO.":
                    $"Shape card #{a}: a rectangle has 4 right angles but need not have equal sides. Is every RECTANGLE necessarily a SQUARE? Write ANSWER=YES/NO."
            },
            6=>mode==4?
                $"SOURCE-INSPIRED SAME-OUTLINE REBUILD (FIRST-PARTY MODEL #{a}): An identical regular HEXAGON silhouette is covered completely by SIX equal pattern-block triangles with no gaps. Rebuild the SAME silhouette using TWO trapezoid blocks; each trapezoid equals THREE of the small unit triangles. Report the TWO different block counts and whether the covered AREA and OUTLINE are unchanged. Write TRIANGLES=number;TRAPEZOIDS=number;SAME_OUTLINE=YES/NO. Students' original actual block construction, tracing and teacher checks cannot be inferred from this arithmetic model.":
                $"Two equal right triangles can be placed along their diagonal to form a rectangle. Card #{a}, model #{b}. Count the number of triangular COMPONENTS. Write PARTS=number.",
            7=>mode==4?
                $"SOURCE-INSPIRED MATCH MINE (FIRST-PARTY SPATIAL MODEL #{a}): A partner describes a three-dimensional construction made from TWO equal cubes touching face to face on the bottom row, plus ONE equal cube stacked directly on the LEFT cube. Count how many cubes are in the bottom and top layers and in the whole arrangement. Write BOTTOM=number;TOP=number;TOTAL_CUBES=number. Teacher checks actual partner explanations and physical recreation; a numeric result cannot certify it.":
                mode%2==0?$"Center numerical task {a}+{b} (sum within 24, classroom shape center assessed separately). Write RESULT=number.":
                $"Center shape naming card #{a}: a CLOSED flat figure with exactly 3 straight sides. Write SHAPE=name.",
            8=>mode==4?
                $"ORIGINAL EQUAL PIECES SOURCE-INSPIRED MATHEMATICS: FIRST-PARTY circle #{a} and square #{b} are BOTH divided into exactly TWO EQUAL-AREA parts. Name the part and the piece count for EACH whole. Write CIRCLE_PART=HALF;CIRCLE_PIECES=number;SQUARE_PART=HALF;SQUARE_PIECES=number. Physical cutting, folding and children's explanations remain teacher-reviewed.":
                mode==5?
                $"ORIGINAL EQUAL PIECES SOURCE-INSPIRED MATHEMATICS: FIRST-PARTY circle #{a} and square #{b} are BOTH divided into FOUR EQUAL-AREA parts. Name the part and piece count for EACH whole. Write CIRCLE_PART=FOURTH;CIRCLE_PIECES=number;SQUARE_PART=FOURTH;SQUARE_PIECES=number. Unequal pieces do NOT count as fourths; actual student folding remains teacher-reviewed.":
                mode%2==0?$"A rectangle card #{a} is partitioned into 2 EQUAL-area pieces. What is each piece called? Write PART=name;PIECES=number.":
                $"A circle card #{a} is partitioned into 4 EQUAL-area pieces. What is each piece called? Write PART=name;PIECES=number.",
            9=>mode<=1?(mode==0?
                $"Two equal halves rejoin to make one WHOLE rectangle; card #{a}. Write PARTS=number;WHOLES=number.":
                $"Four equal fourths rejoin to make one WHOLE circle; card #{a}. Write PARTS=number;WHOLES=number."):
                mode==2?
                $"SOURCE-INSPIRED SHADED CARD SORT, FIRST-PARTY MODEL #{a}: A rectangle is 4 unit squares WIDE and 2 unit squares HIGH. A vertical cut 1 unit from the left splits it into two regions, one 1-by-2 and one 3-by-2. Compare their AREAS to decide whether they are equal halves. Write SORT=EQUAL/UNEQUAL;PIECES=number. No publisher image is assumed.":
                $"SOURCE-INSPIRED SHADED CARD SORT, FIRST-PARTY MODEL #{a}: A rectangle is 4 unit squares WIDE and 2 unit squares HIGH. Cut it into 2 equal-width columns and 2 equal-height rows, giving FOUR regions each 2-by-1 unit squares. Compare their AREAS. Write SORT=EQUAL/UNEQUAL;PIECES=number. Real source cards and pupil sorting are teacher-reviewed.",
            10=>$"In the SAME shape #{a}, compare one HALF and one FOURTH. Which is larger? Write LARGER=part_name;FOURTHS_PER_HALF=number.",
            11=>mode%2==0?$"Center arithmetic: add {a}+{b} (within 24). Write RESULT=number.":
                $"Center shape card #{a}: 3 straight sides, 3 corners, closed. Write SHAPE=name.",
            12=>mode<=1?
                $"Clock card #{b}: the MINUTE hand points at 12, and the HOUR hand points at {a}. Tell the time. Write TIME=hour:minutes.":
                mode==2?
                $"ORIGINAL CONNECT CLOCKS: The analog clock shows two hands, and a second digital card displays {a}:00. Do these clocks show the SAME time? Write SAME=YES/NO;TIME=hour:minutes.":
                $"ORIGINAL CONNECT CLOCKS: The analog clock shows two hands, and a second digital card displays {b}:00. Do these clocks show the SAME time? Write SAME=YES/NO;TIME=hour:minutes.",
            13=>mode==4?
                $"SOURCE-INSPIRED ORDER THREE CLOCKS, FIRST-PARTY TEXT MODEL: Clock A shows {a}:30, Clock B shows {a+1}:00 and Clock C shows {a}:00, all within the SAME 12-hour half-day with no clock wrap. Arrange these THREE clocks from earliest to latest, comparing o'clock with half past. Write ORDER=comma-separated clock letters. The teacher verifies original publisher clock cards and real student ordering separately.":
                mode<=1?
                $"Half-hour clock #{b}: minute hand at SIX, hour hand halfway between {a} and next hour. Write MINUTES=number;HOUR=number.":
                mode==2?
                $"ORIGINAL CARD SORT: Clock #{b} shows the minute hand at 12 and the short hand at {a}. Color it BLUE for o'clock (RED is half past). Write COLOR=BLUE;TIME=hour:minutes;WORDS=hour_oclock. Use a lowercase spelled-out hour word such as seven_oclock, not a digit.":
                $"ORIGINAL CARD SORT: Clock #{b} shows the minute hand at SIX and the short hand halfway past {a}. Color it RED for half past (BLUE is o'clock). Write COLOR=RED;TIME=hour:minutes;WORDS=half_past_hour (e.g. half_past_seven).",
            14=>mode<=1?(mode==0?
                $"Clock card #{b}: hour hand at {a}, minute hand at 12. Write TIME=hour:minutes.":
                $"Clock card #{b}: hour hand past {a}, minute hand at SIX. Write TIME=hour:minutes."):
                $"ORIGINAL COUNT THE MINUTES: Start at 12 on the clock, count clockwise in steps of five until you reach the minute hand on SIX. How many five-minute HOPS and total MINUTES are in half a clock? Write HOPS=number;MINUTES=number;STOP_AT=number.",
            15=>mode>=4?mode switch
                {
                    4=>"ORIGINAL 2021 NUMBER TALK: Find 8+32. Write RESULT=number.",
                    5=>"ORIGINAL 2021 NUMBER TALK: Find 8+33. Write RESULT=number.",
                    6=>"ORIGINAL 2021 NUMBER TALK: Find 8+38. Write RESULT=number.",
                    7=>"ORIGINAL 2021 NUMBER TALK: Find 8+48. Write RESULT=number.",
                    _=>"ORIGINAL 2021 DIFFICULT CLOCK: Diego says this clock is 6:00. Priya says it is 12:30. The long minute hand points at 12 and the short hour hand points at 6. Who is correct? Write TIME=hour:minutes;CORRECT=first_name. Teacher reviews the learner's explanation and source figure."
                }:
                mode%2==0?
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
            1=>mode==4?$"PER_LAYER={a*b};LAYERS=2;TOTAL_CUBES={2*a*b}":
                $"PIECES={a+b}",
            2=>mode==4?"CURVED=CIRCLE;THREE_SIDES=TRIANGLE;FOUR_SIDES=SQUARE,RECTANGLE":
                $"SHAPE={(mode%2==0?"TRIANGLE":"SQUARE")}",
            3=>$"SIDES={(mode%2==0?3:4)};CORNERS={(mode%2==0?3:4)}",
            4=>mode==4?"TRIANGLES=A,B,C;NONTRIANGLES=D,E,F":"SIDES=3",
            5=>mode switch
            {
                4=>"RECTANGLE=YES;SQUARE=YES",
                5=>"RECTANGLE=YES;SQUARE=NO",
                6=>"RECTANGLE=NO;SQUARE=NO",
                _=>$"ANSWER={(mode%2==0?"YES":"NO")}"
            },
            6=>mode==4?"TRIANGLES=6;TRAPEZOIDS=2;SAME_OUTLINE=YES":"PARTS=2",
            7=>mode==4?"BOTTOM=2;TOP=1;TOTAL_CUBES=3":
                mode%2==0?$"RESULT={a+b}":"SHAPE=TRIANGLE",
            8=>mode==4?"CIRCLE_PART=HALF;CIRCLE_PIECES=2;SQUARE_PART=HALF;SQUARE_PIECES=2":
                mode==5?"CIRCLE_PART=FOURTH;CIRCLE_PIECES=4;SQUARE_PART=FOURTH;SQUARE_PIECES=4":
                mode%2==0?"PART=HALF;PIECES=2":"PART=FOURTH;PIECES=4",
            9=>mode<=1?(mode==0?"PARTS=2;WHOLES=1":"PARTS=4;WHOLES=1"):
                (mode==2?"SORT=UNEQUAL;PIECES=2":"SORT=EQUAL;PIECES=4"),
            10=>"LARGER=HALF;FOURTHS_PER_HALF=2",
            11=>mode%2==0?$"RESULT={a+b}":"SHAPE=TRIANGLE",
            12=>mode<=1?$"TIME={a}:00":$"SAME={(mode==2?"YES":"NO")};TIME={a}:00",
            13=>mode==4?"ORDER=C,A,B":
                mode<=1?$"MINUTES=30;HOUR={a}":
                $"COLOR={(mode==2?"BLUE":"RED")};TIME={a}:{(mode==2?"00":"30")};WORDS={(mode==2?HourName(a)+"_oclock":"half_past_"+HourName(a))}",
            14=>mode<=1?$"TIME={a}:{(mode==0?"00":"30")}":"HOPS=6;MINUTES=30;STOP_AT=6",
            15=>mode switch
            {
                4=>"RESULT=40",
                5=>"RESULT=41",
                6=>"RESULT=46",
                7=>"RESULT=56",
                8=>"TIME=6:00;CORRECT=DIEGO",
                _=>$"TIME={a}:{(mode%2==0?"00":"30")}"
            },
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
