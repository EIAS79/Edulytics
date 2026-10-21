using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Original IM2021 G1 U06 L01–L17 digital mathematics only. Real measurement
/// with equal units, correct placement/no gaps, manipulation, teacher PLC,
/// drawing and optional personally-authored stories need independent evidence.
/// </summary>
public static class UsCcssG1FullGradeUnit6MeasurementQuestions
{
    public static readonly IReadOnlyList<string> Families=
    [
        "usccss.g1.u06.l01.compare_lengths",
        "usccss.g1.u06.l02.indirect_length_comparison",
        "usccss.g1.u06.l03.choose_indirect_comparison",
        "usccss.g1.u06.l04.center_add_compare",
        "usccss.g1.u06.l05.measure_connecting_cubes",
        "usccss.g1.u06.l06.measure_paperclips",
        "usccss.g1.u06.l07.unit_length_matters",
        "usccss.g1.u06.l08.count_to110_cube_groups",
        "usccss.g1.u06.l09.write_number_to120",
        "usccss.g1.u06.l10.center_measure_and_add",
        "usccss.g1.u06.l11.compare_shoe_lengths",
        "usccss.g1.u06.l12.compare_measurements",
        "usccss.g1.u06.l13.take_from_unknown",
        "usccss.g1.u06.l14.match_story_equation",
        "usccss.g1.u06.l15.write_story_equations",
        "usccss.g1.u06.l16.center_mixed_math",
        "usccss.g1.u06.l17.optional_compare_stories"
    ];
    private static readonly (int a,int b)[] Lesson2OriginalWarmup =
    [
        (35,20),(35,25),(30,45),(37,45)
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
        int i=Index(family);
        if(i<0||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode)||
           mode<0||mode>(i==1?7:i==5?7:i==6?4:i==7?4:i==9?8:i==10?7:i==11?5:i==12?4:i==13?9:i==14?4:i==15?9:3)||
           (i!=0&&c!=0&&!(i==10&&mode==5)))return false;
        if(i==1&&mode>=4)
        {
            var source=Lesson2OriginalWarmup[mode-4];
            return a==source.a&&b==source.b&&c==0;
        }
        return i switch
        {
            0=>a>=4&&a<=20&&b>=1&&b<=20&&
                (mode%2==0?c==0:(b>=4&&c>=4&&c<=20&&a!=b&&a!=c&&b!=c)),
            1 or 2=>a>=4&&a<=20&&b>=1&&b<=20,
            10=>mode switch
            {
                4=>a==9&&b==7&&c==0,          // Clare + Han: 16
                5=>a==7&&b==9&&c==4,          // Kiran + brothers: 20
                6=>a==13&&b==8&&c==0,         // Diego vs father: 5 longer
                7=>a==8&&b==17&&c==0,         // Jada + Elena: missing 9
                _=>a>=4&&a<=20&&b>=1&&b<=20&&c==0 // Existing generic comparison stays
            },
            // Original U06 L12 targets Compare Bigger Unknown / Smaller Unknown,
            // NOT Difference Unknown from two already known lengths.
            11=>mode switch
            {
                4=>a==14&&b==4&&c==0,  // Original Han 14, Priya 4 shorter
                5=>a==10&&b==4&&c==0,  // Original Priya 10, Han 4 longer
                _=>a>=4&&a<=20&&b>=1&&b<=9&&(mode%2==0?b<a:a+b<=20)
            },
            3=>a>=11&&a<=65&&b>=1&&b<=20&&a+b<=100,
            15=>mode switch
            {
                4=>a>=101&&a<=120&&b==0,
                5=>a==65&&b==21,
                6=>a==65&&b==20,
                7=>a==85&&b==1,
                8 or 9=>a==65&&b==21,
                _=>a>=11&&a<=65&&b>=1&&b<=20&&a+b<=100
            },
            9=>mode switch
            {
                4=>a>=5&&a<=20&&b>=1&&b<=25&&Math.Abs(a-b)<=4,
                5=>a==24&&b==54,
                6=>a==24&&b==64,
                7=>a==30&&b==75,
                8=>a==32&&b==77,
                _=>a>=11&&a<=65&&b>=1&&b<=20&&a+b<=100
            },
            4 or 5=>a>=2&&a<=20&&b==0,
            6=>b==2&&(mode==4?(a==6||a==12):a>=2&&a<=14),
            7=>b==0&&(mode==4?a>=101&&a<=110:a>=20&&a<=110),
            8=>a>=95&&a<=120&&b==0,
            12=>mode==4?a==20&&b==9&&c==0:
                a>=5&&a<=20&&b>=1&&b<=a&&c==0,
            13=>mode switch
            {
                4=>a==11&&b==8&&c==0, // Jada: 8 + unknown = 11
                5=>a==19&&b==11&&c==0, // Kiran: 19 - unknown = 11
                6=>a==16&&b==10&&c==0, // Han and Lin: 10 fewer
                7=>a==14&&b==9&&c==0, // Elena and Andre: nine more than five
                8=>a==16&&b==6&&c==0, // Noah and Tyler: 16 vs 6
                9=>a==14&&b==9&&c==0, // Clare: nine given, five left
                _=>a>=5&&a<=20&&b>=1&&b<=a&&c==0
            },
            14=>mode==4?a==19&&b==12&&c==0:
                a>=5&&a<=20&&b>=1&&b<=a&&c==0,
            16=>a>=5&&a<=20&&b>=1&&b<=a&&c==0,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        int i=Index(family);
        if(i<0)throw new InvalidOperationException("Unreviewed G1 Unit6 source-skill family");
        int mode=random.Next(i==1?8:i==5?8:i==9?9:i==6||i==7||i==14?5:i==10?8:i==11?6:i==12?5:i==13?10:i==15?10:4),a=0,b=0,c=0;
        if(i is 0 or 1 or 2 or 10)
        {
            // Preserve independent source-comparison lengths. Previously these
            // operands were overwritten by the fallback branch, making strip
            // A almost always at least as long as B in lessons 1-3 and 11.
            a=random.Next(4,21);
            b=random.Next(1,21);
            if(i==0&&mode%2==1)
            {
                // Original U06 L01 also orders THREE distinct objects.
                do{b=random.Next(4,21);}while(b==a);
                do{c=random.Next(4,21);}while(c==a||c==b);
            }
        }
        else if(i==11)
        {
            // Stable first-grade lengths that exercise both unknown positions.
            a=mode%2==0?random.Next(7,21):random.Next(4,17);
            b=mode%2==0?random.Next(1,Math.Min(10,a)):
                           random.Next(1,Math.Min(10,21-a));
        }
        else if(i is 3 or 9 or 15)
        {
            if(i==9&&mode==4)
            {
                a=random.Next(5,21);
                b=Math.Clamp(a+random.Next(-4,5),1,25);
            }
            else {a=random.Next(11,66);b=random.Next(1,Math.Min(20,100-a)+1);}
        }
        else if(i is 4 or 5){a=random.Next(2,21);}
        else if(i==6){a=mode==4?(random.Next(2)==0?6:12):random.Next(2,15);b=2;}
        else if(i==7){a=mode==4?random.Next(101,111):random.Next(20,111);}
        else if(i==8){a=random.Next(95,121);}
        else {a=random.Next(5,21);b=random.Next(1,a+1);}
        if(i==10&&mode>=4)
        {
            (a,b,c)=mode switch
            {
                4=>(9,7,0),
                5=>(7,9,4),
                6=>(13,8,0),
                7=>(8,17,0),
                _=>throw new InvalidOperationException("Unknown original shoe story")
            };
        }
        if(i==12&&mode==4)(a,b,c)=(20,9,0);
        if(i==14&&mode==4)(a,b,c)=(19,12,0);
        if(i==15&&mode==4)(a,b,c)=(random.Next(101,121),0,0);
        if(i==15&&mode==5)(a,b,c)=(65,21,0);
        if(i==15&&mode>=6)
            (a,b,c)=mode switch
            {
                6=>(65,20,0),7=>(85,1,0),8=>(65,21,0),9=>(65,21,0),
                _=>throw new InvalidOperationException("Invalid source number talk")
            };
        if(i==9&&mode>=5)
            (a,b,c)=mode switch
            {
                5=>(24,54,0),6=>(24,64,0),
                7=>(30,75,0),8=>(32,77,0),
                _=>throw new InvalidOperationException("Invalid original source true/false")
            };
        if(i==11&&mode>=4)
            (a,b,c)=mode==4?(14,4,0):(10,4,0);
        if(i==13&&mode>=4)
        {
            (a,b,c)=mode switch
            {
                4=>(11,8,0),5=>(19,11,0),6=>(16,10,0),
                7=>(14,9,0),8=>(16,6,0),9=>(14,9,0),
                _=>throw new InvalidOperationException("Unreviewed original source story")
            };
        }
        if(i==1&&mode>=4)
        {
            (a,b)=Lesson2OriginalWarmup[mode-4];
            c=0;
        }
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode}};
        if(!Valid(family,p))throw new InvalidOperationException("Unreviewed G1 Unit6 source operands");
        string prompt=i switch
        {
            0=>mode%2==0?
                $"Draw two FIRST-PARTY equal-unit strips starting at the SAME endpoint: red has {a} cube-length units and blue has {b}. Compare them. Write LONGER=RED/BLUE/EQUAL;DIFFERENCE=number. The learner-made drawing needs teacher review.":
                $"Three strips A, B, and C share the SAME start line and measure {a}, {b}, and {c} equal cube-length units. Original student Activity 2: order three objects {(mode==1?"from SHORTEST to LONGEST":"from LONGEST to SHORTEST")}. Write ORDER=A,B,C (the letters in their correct order).",
            1=>mode>=4?
                $"ORIGINAL 2021 GRADE 1 UNIT 6 LESSON 2 MENTAL WARM-UP: Add {a}+{b}. "+
                "Write RESULT=number; explain mentally how adding tens/ones changes the value.":
                $"FIRST-PARTY INDIRECT LENGTH MODEL: Without placing objects side by side, "+
                $"a single unchanged reference string represents object A as {a} equal "+
                $"units and object B as {b} of the SAME units. Which is longer? "+
                "Write LONGER=A/B/EQUAL. Student's actual chosen pencil/objects "+
                "and real desk side versus leg string comparison are teacher-assessed.",
            2=>mode switch
            {
                0=>$"Two classroom objects cannot be laid together. One flexible SAME reference unit gives lengths {a} and {b}. Choose the longer one. Write LONGER=A/B/EQUAL.",
                1=>$"SOURCE-INSPIRED WALK TO SCHOOL: Mai's route measures {a} and Clare's measures {b} of the SAME reference units. The two paths cannot be laid together: use the SAME movable reference string to compare indirectly. Who has the SHORTER route? Write SHORTER=MAI/CLARE/EQUAL.",
                2=>$"SOURCE-INSPIRED WILL IT FIT: Teacher's desk WIDTH is {b} units; doorway OPENING is {a} of the SAME reference units. Using a common comparison string for both, will the desk fit widthwise? Write FITS=YES/NO.",
                _=>$"SOURCE-INSPIRED WILL IT FIT: Student desk WIDTH is {b} units; doorway OPENING is {a} of the SAME reference units. Using a common comparison string for both, will the desk fit widthwise? Write FITS=YES/NO."
            },
            3=>mode%2==0?$"Center addition: compute {a}+{b} within 100. Write RESULT=number.":
                $"Center expression comparison within 20: compare {a%8+1}+{b%8+1} with {a/10%8+1}+{b/10%8+1}. Write RELATION=GREATER/LESS/EQUAL.",
            4=>$"A line is tiled by {a} EQUAL connecting cubes, placed edge to edge with NO gaps or overlap. How many cube-length units? Write LENGTH=number.",
            5=>mode switch
            {
                4=>$"SOURCE-INSPIRED PAPER CLIP CHECK: {a} equally sized clips cover a tape strip from its starting endpoint to the other endpoint, with no gaps and no overlap. Is this a VALID length measurement? Write VALID=YES/NO.",
                5=>$"SOURCE-INSPIRED PAPER CLIP CHECK: {a} equally sized clips are arranged along a tape strip, but there is an uncovered GAP between two adjacent clips. Is this a VALID length measurement? Write VALID=YES/NO.",
                6=>$"SOURCE-INSPIRED PAPER CLIP CHECK: {a} equally sized clips are arranged along a tape strip, but two adjacent clips OVERLAP. Is this a VALID length measurement? Write VALID=YES/NO.",
                7=>$"SOURCE-INSPIRED PAPER CLIP CHECK: {a} paper clips appear end to end on a tape strip, but the clips have MIXED LENGTHS. Can the raw count be reported as a valid measurement in one identical paper-clip unit? Write VALID=YES/NO.",
                _=>$"A pencil is exactly covered by {a} identical paper clips, end to end, without gaps or overlaps. Record the measurement. Write LENGTH=number."
            },
            6=>mode==4
                ?$"SOURCE-INSPIRED THREE-UNIT COMPARISON (FIRST-PARTY MODEL): The SAME strip is {a} one-cube-length units. A medium measuring tool spans 2 such units; a long tool spans 3. Without leaving gaps, find ALL THREE correct counts for the same unchanged strip. Write SMALL_UNITS=number;MEDIUM_UNITS=number;LARGE_UNITS=number. This is not a claim to have measured Jeison's real foot."
                :$"One strip is {a} long units. A small unit is HALF the length of one long unit. How many small units cover the SAME strip? Write SMALL_UNITS=number.",
            7=>mode==4?
                $"SOURCE-GROUNDED GROUPS UP TO 110: A FIRST-PARTY equal-unit strip measures {a} unit cubes, MORE THAN 100 cubes. Count full tens and remaining ones, and verify the total without guessing an original source diagram. Write TENS=number;ONES=number;TOTAL=number.":
                $"A long object measures {a} identical cube units. Organize the count into tens and ones. Write TENS=number;ONES=number;TOTAL=number.",
            8=>$"Write the number represented by {a/10} complete TENS and {a%10} leftover ONES. Grade 1 range is up to 120. Write NUMBER=number.",
            9=>mode>=5?mode switch
                {
                    5=>"ORIGINAL 2021 NUMBER TALK TRUE/FALSE: Is 24+30=44? Write TRUE=YES/NO;ACTUAL=number.",
                    6=>"ORIGINAL 2021 NUMBER TALK TRUE/FALSE: Is 24+40=64? Write TRUE=YES/NO;ACTUAL=number.",
                    7=>"ORIGINAL 2021 NUMBER TALK TRUE/FALSE: Is 30+45=75? Write TRUE=YES/NO;ACTUAL=number.",
                    _=>"ORIGINAL 2021 NUMBER TALK TRUE/FALSE: Is 32+45=76? Write TRUE=YES/NO;ACTUAL=number."
                }:
                mode==4?
                $"SOURCE-GROUNDED ESTIMATE THEN MEASURE (FIRST-PARTY MODEL): BEFORE counting, a pupil estimated {b} equal cube units. The hypothetical strip actually measures {a} equal cube units without gaps. Record BOTH values, the absolute difference, and whether the estimate was HIGH, LOW or EXACT. Write ACTUAL=number;ESTIMATE=number;DIFFERENCE=number;DIRECTION=HIGH/LOW/EXACT. Real classroom measuring and chosen units remain teacher-observed.":
                mode%2==0?$"Center arithmetic: add {a}+{b} within 100. Write RESULT=number.":
                $"Center numeric measuring task: {a} same-sized cubes arranged without gaps. Write LENGTH=number.",
            10=>mode switch
            {
                4=>"ORIGINAL 2021 SHOE STORY: Clare's shoe is 9 equal cubes long, Han's shoe is 7. How long are the TWO shoes TOGETHER? Write TOTAL=number.",
                5=>"ORIGINAL 2021 SHOE STORY: Kiran's shoe is 7 cubes, his older brother's shoe 9 and younger brother's 4. What is the combined length of ALL THREE? Write TOTAL=number.",
                6=>"ORIGINAL 2021 SHOE STORY: Diego's shoe is 8 cubes and his father's 13 cubes. How many cubes LONGER is his father's shoe than Diego's? Write DIFFERENCE=number.",
                7=>"ORIGINAL 2021 SHOE STORY: Jada's shoe is 8 cubes, and Jada's and Elena's shoes TOGETHER measure 17 cubes. How long is Elena's shoe? Write ELENA=number.",
                _=>$"Hypothetical two shoes are {a} and {b} identical cube-length units long. How many units LONGER is the longer shoe? Write DIFFERENCE=number."
            },
            11=>mode==4?
                "ORIGINAL 2021 BRACELET: Han's bracelet is 14 equal cube units. Priya's is FOUR units SHORTER. Write LENGTH=number;DIFFERENCE=number. Teacher reviews physical bar model.":
                mode==5?
                "ORIGINAL 2021 BRACELET: Priya's bracelet is 10 equal cube units. Han's is FOUR units LONGER. Write LENGTH=number;DIFFERENCE=number. Teacher reviews the comparison diagram.":
                mode%2==0?
                $"Han's friendship bracelet is {a} connecting cubes long. Priya's bracelet is {b} cubes FEWER than Han's. How long is PRIYA's bracelet? Write LENGTH=number;DIFFERENCE=number.":
                $"Priya's friendship bracelet is {a} connecting cubes long. Han's bracelet is {b} cubes MORE than Priya's. How long is HAN's bracelet? Write LENGTH=number;DIFFERENCE=number.",
            12=>mode==4?
                "ORIGINAL ELENA BEADS: Elena took 9 beads OUT of her bracelet and 11 beads are LEFT. How many beads were there TO START? Write START=number.":
                mode%3==0?$"Take From: {a} marbles were present; {b} are taken away. Write REMAINING=number.":
                mode%3==1?$"Take From: {a} marbles were present, {a-b} remain. How many were taken? Write REMOVED=number.":
                $"Take From: Some marbles were present; {b} leave, {a-b} remain. Write START=number.",
            13=>mode switch
            {
                0=>$"Match the equation to a story: {a} cubes, {b} leave, {a-b} remain. Write OP=ADD/SUBTRACT;TOTAL=number.",
                1=>$"Story: {a-b} cubes, {b} join, {a} in all. Write OP=ADD/SUBTRACT;TOTAL=number.",
                2=>$"Original source: CIRCLE TWO equations matching this Take From story. There are {a} cubes, {b} are taken away, and {a-b} remain. A: {a}-{b}={a-b}. B: {a}+{b}={a-b}. C: {a-b}+{b}={a}. Write MATCH=first_letter,second_letter;TOTAL=number (the TWO correct letters in alphabetical order, followed by the whole).",
                3=>$"Original source: CIRCLE TWO equations matching this Put Together story. There are {a-b} cubes and {b} more join, making {a} in all. A: {a}+{b}={a}. B: {a-b}+{b}={a}. C: {a}-{a-b}={b}. Write MATCH=first_letter,second_letter;TOTAL=number (the TWO correct letters in alphabetical order, followed by the whole).",
                4=>"SOURCE-GROUNDED JADA: Jada has 8 people pictures and 11 total pictures. How many are ANIMAL pictures? Circle TWO matching equations: A: 8+□=11. B: 8+11=□. C: 11-8=□. Write MATCH=first_letter,second_letter;UNKNOWN=number.",
                5=>"SOURCE-GROUNDED KIRAN: Kiran has 19 pictures and gives some away; 11 remain. How many did he GIVE AWAY? Circle TWO matching equations: A: 19-□=11. B: 19+11=□. C: 19-11=□. Write MATCH=first_letter,second_letter;UNKNOWN=number.",
                6=>"SOURCE-GROUNDED HAN AND LIN: Han has 16 stamps; Lin has 10 FEWER. How many stamps does Lin have? Circle TWO matching equations: A: 10+□=16. B: 16-10=10. C: 16-10=□. Write MATCH=first_letter,second_letter;UNKNOWN=number.",
                7=>"SOURCE-GROUNDED ELENA AND ANDRE: Elena has 9 MORE stickers than Andre, who has 5. How many does Elena have? Circle TWO matching equations: A: 9+5=14. B: 9-5=14. C: 5+9=14. Write MATCH=first_letter,second_letter;TOTAL=number.",
                8=>"SOURCE-GROUNDED NOAH AND TYLER: Noah has 6 stamps and Tyler has 16. How many MORE does Tyler have? Circle TWO matching equations: A: 6+□=16. B: 16-6=16. C: 16-6=□. Write MATCH=first_letter,second_letter;UNKNOWN=number.",
                9=>"SOURCE-GROUNDED CLARE: Clare gives away 9 stickers and has 5 LEFT. How many did she START with? Circle TWO matching equations: A: 5+9=□. B: 9-5=14. C: □-9=5. Write MATCH=first_letter,second_letter;UNKNOWN=number.",
                _=>throw new InvalidOperationException("Unreviewed original story mode")
            },
            14=>mode==4?
                "ORIGINAL CLARE ORIGAMI SOURCE TASK: Clare gave 12 paper cranes AWAY and has 7 LEFT. The START is unknown. WRITE the source story equation with the correct missing-start relationship, then solve it. Write START=number;EQUATION=number-number=number. This tests mathematical equality, not teacher-observed pupil writing or gallery work.":
                mode%2==0?$"Write a true equation for {a} birds, {b} fly away. Write EQUATION=number-number=number.":
                $"Write an addition equation for {a-b} children, {b} join to make {a}. Write EQUATION=number+number=number.",
            15=>mode switch
            {
                4=>$"SOURCE-GROUNDED COUNTING COLLECTIONS STAGE 3 (FIRST-PARTY NUMERIC MODEL): A collection has {a/10} complete groups of TEN objects and {a%10} loose objects. Write its total (up to 120) and both components. Write TENS=number;ONES=number;TOTAL=number. This does not claim a pupil estimated or physically counted a real classroom collection.",
                5=>"ORIGINAL 2021 NUMBER TALK: Mentally compare the results of 65+20+1 and 65+21. Both expressions should equal the same number. Write FIRST=number;SECOND=number;EQUAL=YES/NO.",
                6=>"ORIGINAL 2021 NUMBER TALK: Find 65+20. Write RESULT=number.",
                7=>"ORIGINAL 2021 NUMBER TALK: Find 85+1. Write RESULT=number.",
                8=>"ORIGINAL 2021 NUMBER TALK: Find 65+20+1. Write RESULT=number.",
                9=>"ORIGINAL 2021 NUMBER TALK: Find 65+21. Write RESULT=number.",
                _=>mode%2==0?$"Center addition within 100: {a}+{b}. Write RESULT=number.":
                    $"Center hypothetical measurement: {a} equal cubes without gaps. Write LENGTH=number."
            },
            16=>$"Compare story: one group has {a} tulips and another has {a-b}. How many MORE are in the first? Write DIFFERENCE=number.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
          "This digital subquestion checks a mathematical number/equation. Real endpoint alignment, choosing a comparison tool, same-unit iteration without gaps, actual shoe measurement, teacher discussion and optional investigations are separately assessed.",
          AssessmentItemType.ShortAnswer,p);
    }
    private static string Relation(int a,int b,string first,string second)=>a>b?first:a<b?second:"EQUAL";
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Out of scope G1 U06 params");
        int i=Index(family),a=p["a"],b=p["b"],mode=p["mode"];
        return i switch
        {
            0=>mode%2==0?
                $"LONGER={Relation(a,b,"RED","BLUE")};DIFFERENCE={Math.Abs(a-b)}":
                "ORDER="+string.Join(",",
                    (mode==1
                        ?new[]{(Length:a,Label:"A"),(Length:b,Label:"B"),(Length:p["c"],Label:"C")}.OrderBy(x=>x.Length)
                        :new[]{(Length:a,Label:"A"),(Length:b,Label:"B"),(Length:p["c"],Label:"C")}.OrderByDescending(x=>x.Length))
                    .Select(x=>x.Label)),
            1=>mode>=4?$"RESULT={a+b}":$"LONGER={Relation(a,b,"A","B")}",
            2=>mode switch
            {
                0=>$"LONGER={Relation(a,b,"A","B")}",
                1=>$"SHORTER={Relation(a,b,"CLARE","MAI")}",
                _=>$"FITS={(b<=a?"YES":"NO")}"
            },
            3=>mode%2==0?$"RESULT={a+b}":$"RELATION={Relation(a%8+b%8+2,a/10%8+b/10%8+2,"GREATER","LESS")}",
            4=>$"LENGTH={a}",
            5=>mode switch
            {
                4=>"VALID=YES",
                5 or 6 or 7=>"VALID=NO",
                _=>$"LENGTH={a}"
            },
            6=>mode==4?
                $"SMALL_UNITS={a};MEDIUM_UNITS={a/2};LARGE_UNITS={a/3}":
                $"SMALL_UNITS={a*2}",
            7=>$"TENS={a/10};ONES={a%10};TOTAL={a}",
            8=>$"NUMBER={a}",
            9=>mode>=5?
                $"TRUE={(mode is 6 or 7?"YES":"NO")};ACTUAL={b}":
                mode==4?
                $"ACTUAL={a};ESTIMATE={b};DIFFERENCE={Math.Abs(a-b)};DIRECTION={(b==a?"EXACT":b>a?"HIGH":"LOW")}":
                mode%2==0?$"RESULT={a+b}":$"LENGTH={a}",
            10=>mode switch
            {
                4=>$"TOTAL={a+b}",
                5=>$"TOTAL={a+b+p["c"]}",
                6=>$"DIFFERENCE={a-b}",
                7=>$"ELENA={b-a}",
                _=>$"DIFFERENCE={Math.Abs(a-b)}"
            },
            11=>$"LENGTH={(mode%2==0?a-b:a+b)};DIFFERENCE={b}",
            12=>mode==4?"START=20":mode%3==0?$"REMAINING={a-b}":
                mode%3==1?$"REMOVED={b}":$"START={a}",
            13=>mode switch
            {
                0=>$"OP=SUBTRACT;TOTAL={a-b}",
                1=>$"OP=ADD;TOTAL={a}",
                2=>$"MATCH=A,C;TOTAL={a}",
                3=>$"MATCH=B,C;TOTAL={a}",
                4=>"MATCH=A,C;UNKNOWN=3",
                5=>"MATCH=A,C;UNKNOWN=8",
                6=>"MATCH=A,C;UNKNOWN=6",
                7=>"MATCH=A,C;TOTAL=14",
                8=>"MATCH=A,C;UNKNOWN=10",
                9=>"MATCH=A,C;UNKNOWN=14",
                _=>throw new InvalidOperationException("Unreviewed original story mode")
            },
            14=>mode==4?
                "START=19;EQUATION=19-12=7":
                mode%2==0?$"EQUATION={a}-{b}={a-b}":$"EQUATION={a-b}+{b}={a}",
            15=>mode switch
            {
                4=>$"TENS={a/10};ONES={a%10};TOTAL={a}",
                5=>"FIRST=86;SECOND=86;EQUAL=YES",
                6=>"RESULT=85",
                7 or 8 or 9=>"RESULT=86",
                _=>mode%2==0?$"RESULT={a+b}":$"LENGTH={a}"
            },
            16=>$"DIFFERENCE={b}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string Norm(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? candidate)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(candidate)&&Norm(Solve(family!,p))==Norm(candidate);
}
