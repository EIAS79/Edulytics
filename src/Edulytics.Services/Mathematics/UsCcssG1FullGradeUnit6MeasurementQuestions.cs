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
           mode<0||mode>3||c!=0)return false;
        return i switch
        {
            0 or 1 or 2 or 10 or 11=>a>=4&&a<=20&&b>=1&&b<=20,
            3 or 9 or 15=>a>=11&&a<=65&&b>=1&&b<=20&&a+b<=100,
            4 or 5=>a>=2&&a<=20&&b==0,
            6=>a>=2&&a<=14&&b==2,
            7=>a>=20&&a<=110&&b==0,
            8=>a>=100&&a<=120&&b==0,
            12 or 13 or 14 or 16=>a>=5&&a<=20&&b>=1&&b<=a&&c==0,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        int i=Index(family);
        if(i<0)throw new InvalidOperationException("Unreviewed G1 Unit6 source-skill family");
        int mode=random.Next(4),a=0,b=0;
        if(i is 0 or 1 or 2 or 10 or 11){a=random.Next(4,21);b=random.Next(1,21);}
        else if(i is 3 or 9 or 15){a=random.Next(11,66);b=random.Next(1,Math.Min(20,100-a)+1);}
        else if(i is 4 or 5){a=random.Next(2,21);}
        else if(i==6){a=random.Next(2,15);b=2;}
        else if(i==7){a=random.Next(20,111);}
        else if(i==8){a=random.Next(100,121);}
        else {a=random.Next(5,21);b=random.Next(1,a+1);}
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",0},{"mode",mode}};
        if(!Valid(family,p))throw new InvalidOperationException("Unreviewed G1 Unit6 source operands");
        string prompt=i switch
        {
            0=>$"Two drawn strips start at the SAME line. The red strip has {a} equal cube lengths; blue has {b}. State LONGER=RED/BLUE/EQUAL;DIFFERENCE=number.",
            1=>$"Without moving two separate strips, a SAME reference strip of {a} cubes matches strip A and another of {b} cubes matches B. Which is longer? Write LONGER=A/B/EQUAL.",
            2=>$"Two classroom objects cannot be laid together. A third measuring strip gives lengths {a} and {b} in SAME size units. Choose the longer one. Write LONGER=A/B/EQUAL.",
            3=>mode%2==0?$"Center addition: compute {a}+{b} within 100. Write RESULT=number.":
                $"Center expression comparison within 20: compare {a%8+1}+{b%8+1} with {a/10%8+1}+{b/10%8+1}. Write RELATION=GREATER/LESS/EQUAL.",
            4=>$"A line is tiled by {a} EQUAL connecting cubes, placed edge to edge with NO gaps or overlap. How many cube-length units? Write LENGTH=number.",
            5=>$"A pencil is exactly covered by {a} identical paper clips, end to end, without gaps or overlaps. Record the measurement. Write LENGTH=number.",
            6=>$"One strip is {a} long units. A small unit is HALF the length of one long unit. How many small units cover the SAME strip? Write SMALL_UNITS=number.",
            7=>$"A long object measures {a} identical cube units. Organize the count into tens and ones. Write TENS=number;ONES=number;TOTAL=number.",
            8=>$"Write the number represented by {a/10} complete TENS and {a%10} leftover ONES. Grade 1 range is up to 120. Write NUMBER=number.",
            9=>mode%2==0?$"Center arithmetic: add {a}+{b} within 100. Write RESULT=number.":
                $"Center numeric measuring task: {a} same-sized cubes arranged without gaps. Write LENGTH=number.",
            10=>$"Two shoes are {a} and {b} identical cube-length units long. How many units longer is the longer shoe? Write DIFFERENCE=number.",
            11=>$"Two ribbons measure {a} and {b} of the SAME units. Find their length difference. Write DIFFERENCE=number.",
            12=>mode%3==0?$"Take From: {a} marbles were present; {b} are taken away. Write REMAINING=number.":
                mode%3==1?$"Take From: {a} marbles were present, {a-b} remain. How many were taken? Write REMOVED=number.":
                $"Take From: Some marbles were present; {b} leave, {a-b} remain. Write START=number.",
            13=>mode%2==0?$"Match the equation to a story: {a} cubes, {b} leave, {a-b} remain. Write OP=ADD/SUBTRACT;TOTAL=number.":
                $"Story: {a-b} cubes, {b} join, {a} in all. Write OP=ADD/SUBTRACT;TOTAL=number.",
            14=>mode%2==0?$"Write a true equation for {a} birds, {b} fly away. Write EQUATION=number-number=number.":
                $"Write an addition equation for {a-b} children, {b} join to make {a}. Write EQUATION=number+number=number.",
            15=>mode%2==0?$"Center addition within 100: {a}+{b}. Write RESULT=number.":
                $"Center hypothetical measurement: {a} equal cubes without gaps. Write LENGTH=number.",
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
            0=>$"LONGER={Relation(a,b,"RED","BLUE")};DIFFERENCE={Math.Abs(a-b)}",
            1 or 2=>$"LONGER={Relation(a,b,"A","B")}",
            3=>mode%2==0?$"RESULT={a+b}":$"RELATION={Relation(a%8+b%8+2,a/10%8+b/10%8+2,"GREATER","LESS")}",
            4 or 5=>$"LENGTH={a}",
            6=>$"SMALL_UNITS={a*2}",
            7=>$"TENS={a/10};ONES={a%10};TOTAL={a}",
            8=>$"NUMBER={a}",
            9=>mode%2==0?$"RESULT={a+b}":$"LENGTH={a}",
            10 or 11=>$"DIFFERENCE={Math.Abs(a-b)}",
            12=>mode%3==0?$"REMAINING={a-b}":mode%3==1?$"REMOVED={b}":$"START={a}",
            13=>mode%2==0?$"OP=SUBTRACT;TOTAL={a-b}":$"OP=ADD;TOTAL={a}",
            14=>mode%2==0?$"EQUATION={a}-{b}={a-b}":$"EQUATION={a-b}+{b}={a}",
            15=>mode%2==0?$"RESULT={a+b}":$"LENGTH={a}",
            16=>$"DIFFERENCE={b}",
            _=>throw new InvalidOperationException()
        };
    }
    private static string Norm(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? candidate)=>
        Valid(family,p)&&!string.IsNullOrWhiteSpace(candidate)&&Norm(Solve(family!,p))==Norm(candidate);
}
