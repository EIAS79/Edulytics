using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// G1 newly scoped U03 L24–L27 and U04 L01–L05. These are the independently
/// gradeable numeric COMPONENTS of the original activities. PLC, peer-led
/// centers, learner-authored stories and classroom discussion are NOT
/// automatically certified by these generators.
/// U03 L28 (ordinal 65) intentionally requires a separate teacher-reviewed
/// original student story rubric and has no numeric "authorship" generator.
/// </summary>
public static class UsCcssG1FullGradeStrategyAndTensQuestions
{
    public const string CountOnOrAway="usccss.g1.u03.l24.strategy.count_on_or_away";
    public const string FlexibleSubtract="usccss.g1.u03.l25.strategy.flexible_subtract";
    public const string StoryUnknowns="usccss.g1.u03.l26.story.unknown_positions";
    public const string CenterWithin20="usccss.g1.u03.l27.center.add_subtract";
    public const string CountTens="usccss.g1.u04.l01.count.groups_of_ten";
    public const string MatchTens="usccss.g1.u04.l02.match.tens_equivalence";
    public const string AddRemoveTen="usccss.g1.u04.l03.tens.add_or_remove_one";
    public const string AddRemoveMultipleTens="usccss.g1.u04.l04.tens.add_remove_multiples";
    public const string CenterTens="usccss.g1.u04.l05.center.tens_and_within20";
    public static readonly IReadOnlyList<string> Families=
    [CountOnOrAway,FlexibleSubtract,StoryUnknowns,CenterWithin20,
     CountTens,MatchTens,AddRemoveTen,AddRemoveMultipleTens,CenterTens];

    private static string OriginalU04L01Bag(int tens)=>tens switch
    {
        1=>"F",2=>"A",3=>"E",4=>"B",5=>"C",6=>"D",
        _=>throw new InvalidOperationException("Unknown 2021 source bag quantity")
    };
    private static string TensWord(int tens)=>tens switch
    {
        1=>"ten",2=>"twenty",3=>"thirty",4=>"forty",5=>"fifty",
        6=>"sixty",7=>"seventy",8=>"eighty",9=>"ninety",
        _=>throw new InvalidOperationException("Unreviewed multiples-of-ten word card")
    };
    private static string TensWordCardLetter(int tens)=>(tens%3) switch
    {
        0=>"A",1=>"B",_=>"C"
    };
    private static string[] TensWordOptions(int tens)
    {
        int correct=tens%3;
        string[] words=new string[3];
        words[correct]=TensWord(tens);
        words[(correct+1)%3]=TensWord(tens==1?9:tens-1);
        words[(correct+2)%3]=TensWord(tens==9?1:tens+1);
        return words;
    }
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null && Families.Contains(family,StringComparer.Ordinal);
    private static string Norm(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("mode",out int mode)||
           mode<0||mode>5 || c!=0)return false;
        return family switch
        {
            CountOnOrAway or FlexibleSubtract=>mode<=3&&a>=11&&a<=20&&b>=1&&b<=9&&b<a,
            StoryUnknowns=>((a>=1&&a<=10&&b>=1&&b<=10&&a+b<=20)||
                (mode==1&&a==6&&b==12))&&
                (mode!=2||(a==6&&b==9)),
            CenterWithin20=>mode<=3&&a>=11&&a<=17&&b>=1&&b<=20-a,
            CountTens=>mode<=3&&a>=1&&a<=6&&b==0&&
                (mode!=0||a==4),
            MatchTens=>mode<=3&&a>=1&&a<=9&&b==0,
            AddRemoveTen=>mode<=3&&b==1&&
                (mode==2?a==9:mode==3?a==1:a>=2&&a<=8),
            AddRemoveMultipleTens=>mode<=3&&a>=2&&a<=7&&b>=1&&
                (mode%2==0?b<=10-a:b<=a)&&
                (mode==0?a==3&&b==2:mode==1?a==7&&b==4:true),
            CenterTens=>(mode<=1&&a>=2&&a<=7&&b>=1&&b<=Math.Min(a,10-a))||
                        (mode>=2&&mode<=3&&a>=11&&a<=17&&b>=1&&b<=20-a)||
                        (mode==4&&a==5&&b==4)||
                        (mode==5&&a>=2&&a<=7&&b>=1&&b<=Math.Min(a,9-a)),
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed G1 source-activity numeric family");
        int mode=family is StoryUnknowns or CenterTens?random.Next(6):random.Next(4);
        int a=0,b=0;
        if(family is CountOnOrAway or FlexibleSubtract){a=random.Next(11,21);b=random.Next(1,Math.Min(9,a-1)+1);}
        else if(family==StoryUnknowns)
        {
            // Original 2021 U03 L26 learner-authored source problem:
            // Elena starts with 6 counters and finishes with 18.
            if(mode==1){a=6;b=12;}
            else if(mode==2){a=6;b=9;} // Original 2021 Han start unknown.
            else{a=random.Next(1,11);b=random.Next(1,11);}
        }
        else if(family==CenterWithin20){a=random.Next(11,18);b=random.Next(1,21-a);}
        else if(family==CountTens){a=mode==0?4:random.Next(1,7);}
        else if(family==MatchTens){a=random.Next(1,10);}
        else if(family==AddRemoveTen)
        {
            a=mode==2?9:mode==3?1:random.Next(2,9);
            b=1;
        }
        else if(family==AddRemoveMultipleTens)
        {
            if(mode==0){a=3;b=2;} // Original 2021 Jada, Bag A 30 and Bag B 2 ten-towers.
            else if(mode==1){a=7;b=4;} // Original 2021 Tyler, 7 ten-towers minus 40.
            else
            {
                a=random.Next(2,8);
                b=random.Next(1,(mode%2==0?10-a:a)+1);
            }
        }
        else if(family==CenterTens)
        {
            if(mode<=1){a=random.Next(2,8);b=random.Next(1,Math.Min(a,10-a)+1);}
            else if(mode<=3){a=random.Next(11,18);b=random.Next(1,21-a);}
            else if(mode==4){a=5;b=4;} // Exact 2021 teacher example, 90 crossed off and 10 free.
            else{a=random.Next(2,8);b=random.Next(1,Math.Min(a,9-a)+1);}
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["a"]=a,["b"]=b,["c"]=0,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("Grade 1 source problem violates value bounds");
        var t=a+b;
        string prompt=family switch
        {
            CountOnOrAway=>mode==2?
                $"Original 2021 Set A: find the unknown in {a} - {b} = □ and write the full subtraction equation. Write UNKNOWN=number;EQUATION=number-number=number.":
                mode==3?
                $"Original 2021 Set B: find the missing addend in □ + {b} = {a}. Use counting ON or taking AWAY to justify your choice. Write UNKNOWN=number;EQUATION=number+number=number.":
                $"Compare counting ON from {b} to {a} with taking AWAY {b} from {a}. Both methods must find the difference. Write DIFFERENCE=number;CHECK=number+number=number.",
            FlexibleSubtract=>$"Choose a subtraction method for {a}-{b}. Check using related addition. Write DIFFERENCE=number;CHECK=number+number=number.",
            StoryUnknowns=>mode switch
            {
                0=>$"{a} birds are here and {b} more arrive. How many birds NOW? Write UNKNOWN=number;EQUATION=number+number=number.",
                1=>a==6&&b==12?
                    "ORIGINAL 2021 STORY: Elena has 6 counters. She gets some more. Now she has 18 counters. How many MORE did Elena get? Write UNKNOWN=number;EQUATION=number+number=number.":
                    $"{a} birds are here; some arrive, and now {t} birds are here. How many ARRIVED? Write UNKNOWN=number;EQUATION=number+number=number.",
                2=>"ORIGINAL 2021 STORY: Han has some pencils. He gets 9 pencils from the art store. Now he has 15 pencils. How many pencils did Han have to START? Write UNKNOWN=number;EQUATION=number+number=number. Explain using objects, drawings, numbers or words.",
                3=>$"{t} birds are here and {b} fly away. How many REMAIN? Write UNKNOWN=number;EQUATION=number-number=number.",
                4=>$"{t} birds are here; some fly away, and {a} remain. How many FLEW AWAY? Write UNKNOWN=number;EQUATION=number-number=number.",
                _=>$"Some birds are here, {b} fly away, and {a} remain. How many were here at START? Write UNKNOWN=number;EQUATION=number-number=number."
            },
            CenterWithin20=>mode%2==0
                ?$"Center numeric task: solve {a}+{b} within 20. Write RESULT=number."
                :$"Center numeric task: solve {a}-{b} within 20. Write RESULT=number.",
            CountTens=>mode switch
            {
                0=>"Original 2021 G1 U04 L01 Activity 1: a source counting bag contains exactly 40 objects. Organize them into COMPLETE groups of ten and count all objects. Write GROUPS=number;TOTAL=number. Choose and describe a physical counting method with your teacher.",
                1=>$"Original 2021 G1 U04 L01 Activity 2 Bag {OriginalU04L01Bag(a)} has exactly {10*a} objects. Organize it in tens and count them. Source bags A=20, B=40, C=50, D=60, E=30, F=10. Write GROUPS=number;TOTAL=number. Partner bag-switching and drawings are teacher reviewed.",
                2=>$"Source-inspired way to organize the collection: count {a} FULL ten-frames, each with ten dots, then explain the total. Write GROUPS=number;TOTAL=number.",
                _=>$"Source-inspired way to organize the collection: count {a} TOWERS of cubes, each with ten cubes, then explain the total. Write GROUPS=number;TOTAL=number."
            },
            MatchTens=>mode switch
            {
                0=>$"Match {a} TOWERS of ten cubes to the numeral for the same amount. Write TENS=number;NUMBER=number.",
                1=>$"Match {a} FULL ten-frames to the numeral for the same amount. Write TENS=number;NUMBER=number.",
                2=>$"Match {a} BAGS, each with ten counters, to the numeral for the same amount. Write TENS=number;NUMBER=number.",
                _=>$"Original 2021 source word-card matching: {a} TEN-RODS (one rod = 10). Which NUMBER-NAME card matches the rods? A: {TensWordOptions(a)[0]}, B: {TensWordOptions(a)[1]}, C: {TensWordOptions(a)[2]}. Choose the card; no spelling required. Write MATCH=A/B/C;TENS=number;NUMBER=number."
            },
            AddRemoveTen=>mode switch
            {
                0=>$"There are {a} ten-cube towers. Add ONE whole tower of ten. Write TENS=number;TOTAL=number.",
                1=>$"There are {a} ten-cube towers. Remove ONE whole tower of ten. Write TENS=number;TOTAL=number.",
                2=>"Original 2021 Unit 4 Lesson 3: SHOW 1 TEN then repeatedly ADD exactly one full ten-tower until you SHOW 9 TENS. Record the EIGHT totals in order AFTER each addition (start at 20 and end at 90). Write TOTALS=number,number,...;LAST=number.",
                _=>"Original 2021 Unit 4 Lesson 3: SHOW 9 TENS then repeatedly TAKE AWAY exactly one full ten-tower until you SHOW 1 TEN. Record the EIGHT totals in order AFTER each subtraction (start at 80 and end at 10). Write TOTALS=number,number,...;LAST=number."
            },
            AddRemoveMultipleTens=>mode switch
            {
                0=>"Original 2021 U04 L04 How Many Are in the Bag? Jada has 30 cubes in Bag A and two TOWERS OF TEN in Bag B. How many cubes altogether? Write TENS=number;TOTAL=number.",
                1=>"Original 2021 U04 L04 How Many Are in the Bag? Tyler has SEVEN TOWERS OF TEN in Bag C and takes away 40 cubes. How many cubes remain? Write TENS=number;TOTAL=number.",
                2=>$"Original 2021 Add and Subtract Tens practice: add {b} tens to {a} tens. Write TENS=number;TOTAL=number.",
                _=>$"Original 2021 Add and Subtract Tens practice: take away {b} tens from {a} tens. Write TENS=number;TOTAL=number."
            },
            CenterTens=>mode switch
            {
                0=>$"Check It Off number cards: add {a*10}+{b*10}. Write RESULT=number.",
                1=>$"Check It Off number cards: subtract {a*10}-{b*10}. Write RESULT=number.",
                2=>$"Earlier center: add {a}+{b} within 20. Write RESULT=number.",
                3=>$"Earlier center: subtract {a}-{b} within 20. Write RESULT=number.",
                4=>"Original 2021 Check It Off: your two number cards show 40 and 50. The 90 box is ALREADY checked, but the 10 box is NOT. Choose ADD or SUBTRACT to check off 10, then write the equation. Write MOVE=ADD/SUBTRACT;COVER=number;EQUATION=expression.",
                _=>$"Check It Off source-inspired new turn: number cards {10*a} and {10*b}; the {10*(a+b)} box is NOT yet checked, while the difference box is already checked. Choose ADD or SUBTRACT to mark the available box, and write its equation. Write MOVE=ADD/SUBTRACT;COVER=number;EQUATION=expression."
            },
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,"Explain correct group-ten, story-equation or within-20 arithmetic. This numeric task does not certify student-authored work, PLC activities or classroom partner games.",AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid scoped G1 strategy/tens instance");
        int a=p["a"],b=p["b"],mode=p["mode"],sum=a+b;
        return family switch
        {
            CountOnOrAway=>mode==2?
                $"UNKNOWN={a-b};EQUATION={a}-{b}={a-b}":
                mode==3?
                $"UNKNOWN={a-b};EQUATION={a-b}+{b}={a}":
                $"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            FlexibleSubtract=>$"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            StoryUnknowns=>mode switch
            {
                0=>$"UNKNOWN={sum};EQUATION={a}+{b}={sum}",
                1=>$"UNKNOWN={b};EQUATION={a}+{b}={sum}",
                2=>$"UNKNOWN={a};EQUATION={a}+{b}={sum}",
                3=>$"UNKNOWN={a};EQUATION={sum}-{b}={a}",
                4=>$"UNKNOWN={b};EQUATION={sum}-{b}={a}",
                _=>$"UNKNOWN={sum};EQUATION={sum}-{b}={a}"
            },
            CenterWithin20=>$"RESULT={(mode%2==0?a+b:a-b)}",
            CountTens=>$"GROUPS={a};TOTAL={10*a}",
            MatchTens=>mode==3
                ?$"MATCH={TensWordCardLetter(a)};TENS={a};NUMBER={10*a}"
                :$"TENS={a};NUMBER={10*a}",
            AddRemoveTen=>mode switch
            {
                0=>$"TENS={a+1};TOTAL={10*(a+1)}",
                1=>$"TENS={a-1};TOTAL={10*(a-1)}",
                2=>"TOTALS=20,30,40,50,60,70,80,90;LAST=90",
                _=>"TOTALS=80,70,60,50,40,30,20,10;LAST=10"
            },
            AddRemoveMultipleTens=>mode%2==0
                ?$"TENS={sum};TOTAL={10*sum}"
                :$"TENS={a-b};TOTAL={10*(a-b)}",
            CenterTens=>mode switch
            {
                0=>$"RESULT={10*sum}",
                1=>$"RESULT={10*(a-b)}",
                2=>$"RESULT={sum}",
                3=>$"RESULT={a-b}",
                4=>"MOVE=SUBTRACT;COVER=10;EQUATION=50-40=10",
                _=>$"MOVE=ADD;COVER={10*sum};EQUATION={10*a}+{10*b}={10*sum}"
            },
            _=>throw new InvalidOperationException()
        };
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(response))return false;
        var e=Norm(expected).Split(';');var v=Norm(response).Split(';');
        if(e.Length!=v.Length)return false;
        var wanted=new Dictionary<string,string>(StringComparer.Ordinal);
        var got=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (parts,target) in new[]{(e,wanted),(v,got)})
            foreach(var part in parts)
            {
                int k=part.IndexOf('=');
                if(k<=0||k==part.Length-1||!target.TryAdd(part[..k],part[(k+1)..]))return false;
            }
        return wanted.Count==got.Count &&
            wanted.All(item=>got.TryGetValue(item.Key,out var val)&&val==item.Value);
    }
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family!,p),response);
}
