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
           mode<0||mode>7 || (mode==7 && family!=FlexibleSubtract) ||
            (mode==6 && family!=CountOnOrAway && family!=FlexibleSubtract) ||
            (c!=0 && family!=CenterWithin20))return false;
        return family switch
        {
            CountOnOrAway=>a>=11&&a<=20&&b>=1&&b<a&&mode<=6&&
                (mode!=4||((a==12&&b==7)||(a==18&&b==5)||
                           (a==14&&b==6)||(a==15&&b==12)||(a==13&&b==4)))&&
                (mode!=5||((a==18&&b==5)||(a==13&&b==6)||(a==20&&b==15)))&&
                (mode!=6||((a==16&&b==3)||(a==17&&b==8)||(a==18&&b==15))),
            FlexibleSubtract=>mode<=7&&a>=11&&a<=20&&b>=1&&b<a&&
                (mode!=2||(a==12&&b==7))&&
                (mode!=4||((a==20&&b==15)||(a==19&&b==3)||(a==13&&b==5)||
                           (a==18&&b==9)||(a==17&&b==15)))&&
                (mode!=5||(a==15&&b==8))&&
                (mode!=6||(a==17&&b==9))&&
                (mode!=7||(a==16&&b==7)),
            StoryUnknowns=>((a>=1&&a<=10&&b>=1&&b<=10&&a+b<=20)||
                ((mode==1||mode==4)&&a==6&&b==12))&&
                (mode!=2||(a==6&&b==9))&&
                (mode!=4||((a==6&&b==12)||(a==9&&b==6))),
            CenterWithin20=>mode==0?
                (a>=1&&a<=10&&b>=1&&b<=10&&c==0&&a+b<=20):
                mode==2?
                (a>=1&&a<=9&&b>=1&&b<=9&&c>=1&&c<=10&&a+b+c<=20):
                (mode==1||mode==3)&&a>=11&&a<=17&&b>=1&&b<=20-a&&c==0,
            CountTens=>mode<=3&&a>=1&&a<=6&&b==0&&
                (mode!=0||a==4),
            MatchTens=>mode<=3&&a>=1&&a<=9&&b==0,
            AddRemoveTen=>mode<=3&&b==1&&
                (mode==2?a==9:mode==3?a==1:a>=2&&a<=8),
            AddRemoveMultipleTens=>mode<=3&&a>=2&&a<=7&&b>=1&&
                (mode%2==0?b<=10-a:b<=a)&&
                (mode==0?a==3&&b==2:mode==1?a==7&&b==4:true),
            CenterTens=>(mode==0&&a>=2&&a<=7&&b>=1&&b<=Math.Min(a,9-a))||
                        (mode==1&&a>=2&&a<=7&&b>=1&&b<=a)||
                        (mode>=2&&mode<=3&&a>=11&&a<=17&&b>=1&&b<=20-a)||
                        (mode==4&&a==5&&b==4)||
                        (mode==5&&a>=2&&a<=7&&b>=1&&b<=Math.Min(a,9-a)),
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed G1 source-activity numeric family");
        int mode=family==FlexibleSubtract?random.Next(8):
            (family==CountOnOrAway?random.Next(7):
             (family==StoryUnknowns||family==CenterTens?random.Next(6):random.Next(4)));
        int a=0,b=0,c=0;
        if(family is CountOnOrAway or FlexibleSubtract)
        {
            if(family==CountOnOrAway&&mode==4)
            {
                // Original 2021 Unit 3 Lesson 24 Student Facing Set A: ALL five exact equations.
                var tasks=new[]{(12,7),(18,5),(14,6),(15,12),(13,4)};
                (a,b)=tasks[random.Next(tasks.Length)];
            }
            else if(family==CountOnOrAway&&mode==5)
            {
                // Original Set B: 5+□=18; 13=□+6; 20=15+□.
                var tasks=new[]{(18,5),(13,6),(20,15)};
                (a,b)=tasks[random.Next(tasks.Length)];
            }
            else if(family==CountOnOrAway&&mode==6)
            {
                // Original Activity 1 Diego/Tyler: 16−3;17−8;18−15.
                var tasks=new[]{(16,3),(17,8),(18,15)};
                (a,b)=tasks[random.Next(tasks.Length)];
            }
            else if(family==FlexibleSubtract&&mode==4)
            {
                // Original 2021 Student Facing Activity 1: exactly five subtraction expressions.
                var tasks=new[]{(20,15),(19,3),(13,5),(18,9),(17,15)};
                (a,b)=tasks[random.Next(tasks.Length)];
            }
            else if(family==FlexibleSubtract&&mode==5)
            {a=15;b=8;} // Original Clare story: 8+□=15.
            else if(family==FlexibleSubtract&&mode==6)
            {a=17;b=9;} // Original Kiran story: 17−□=9.
            else if(family==FlexibleSubtract&&mode==7)
            {a=16;b=7;} // Independently authored Edulytics exit check, not publisher 14-9 synthesis.
            else if(family==FlexibleSubtract&&mode==2){a=12;b=7;} // Original 2021 pencil story: 12−7.
            else
            {
                a=random.Next(11,21);
                b=random.Next(1,a);
            }
        }
        else if(family==StoryUnknowns)
        {
            // Original 2021 U03 L26 learner-authored source problem:
            // Elena starts with 6 counters and finishes with 18.
            if(mode==1){a=6;b=12;}
            else if(mode==2){a=6;b=9;} // Original Han unknown START.
            else if(mode==4)
            {
                if(random.Next(2)==0){a=6;b=12;} // Elena: 18 minus missing number leaves 6.
                else{a=9;b=6;} // Han: 15 minus missing number leaves 9.
            }
            else{a=random.Next(1,11);b=random.Next(1,11);}
        }
        else if(family==CenterWithin20)
        {
            if(mode==0)
            {
                a=random.Next(1,11);b=random.Next(1,11);
            }
            else if(mode==2)
            {
                a=random.Next(1,10);b=random.Next(1,10);
                c=random.Next(1,Math.Min(10,20-a-b)+1);
            }
            else{a=random.Next(11,18);b=random.Next(1,21-a);}
        }
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
            if(mode==0){a=random.Next(2,8);b=random.Next(1,Math.Min(a,9-a)+1);}
            else if(mode==1){a=random.Next(2,8);b=random.Next(1,a+1);}
            else if(mode<=3){a=random.Next(11,18);b=random.Next(1,21-a);}
            else if(mode==4){a=5;b=4;} // Exact 2021 teacher example, 90 crossed off and 10 free.
            else{a=random.Next(2,8);b=random.Next(1,Math.Min(a,9-a)+1);}
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
            {["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("Grade 1 source problem violates value bounds");
        var t=a+b;
        string prompt=family switch
        {
            CountOnOrAway=>mode==4?
                (a==14?
                    "ORIGINAL 2021 SET A, equation 3: Find the value of the BOX in □ = 14-6. Write UNKNOWN=number;EQUATION=number=number-number. Explain a counting-on or taking-away method with your teacher.":
                    $"ORIGINAL 2021 SET A: Find the number in {a}-{b}=□. Write UNKNOWN=number;EQUATION=number-number=number. Explain your thinking to a teacher.") :
                mode==5?
                (a==18?
                    "ORIGINAL 2021 SET B, equation 1: 5+□=18. Write UNKNOWN=number;EQUATION=number+number=number. Explain how you found the missing addend.":
                 a==13?
                    "ORIGINAL 2021 SET B, equation 2: 13=□+6. Write UNKNOWN=number;EQUATION=number=number+number. Explain the missing first addend.":
                    "ORIGINAL 2021 SET B, equation 3: 20=15+□. Write UNKNOWN=number;EQUATION=number=number+number. Explain how counting on helps.") :
                mode==6?
                $"ORIGINAL 2021 DIEGO AND TYLER TASK: Find {a}-{b} using BOTH methods. Diego takes away {b} from {a}; Tyler counts on from {b} to {a}. Write DIFFERENCE=number;CHECK=number+number=number. Show both strategies with a teacher; digital checking cannot verify a drawing.":
                mode==2?
                $"EDULYTICS ADAPTED SET A: Find the unknown in {a}-{b}=□. Write UNKNOWN=number;EQUATION=number-number=number.":
                mode==3?
                $"EDULYTICS ADAPTED SET B: Find the missing first addend in □+{b}={a}. Write UNKNOWN=number;EQUATION=number+number=number.":
                $"Compare counting ON from {b} to {a} with taking AWAY {b} from {a}. Both methods must find the difference. Write DIFFERENCE=number;CHECK=number+number=number.",
            FlexibleSubtract=>mode==2?
                "ORIGINAL 2021 STUDENT FACING: There are 12 pencils on the table. The teacher picks up 7 pencils. How many are still on the table? Choose YOUR subtraction method, explain with counters, drawings, numbers or words, and write DIFFERENCE=number;CHECK=number+number=number.":
                mode==4?
                $"ORIGINAL 2021 STUDENT FACING: For {a}-{b}, choose an efficient method: count on, take away, or use ten. Show thinking with drawings, numbers, or words. Write DIFFERENCE=number;CHECK=number+number=number. Actual method explanation requires teacher review.":
                mode==5?
                "ORIGINAL 2021 CLARE STORY: Clare has 8 glue sticks from one table. After she collects more from another table, she has 15. How many MORE did she get? Complete 8+□=15. Write UNKNOWN=number;EQUATION=number+number=number. Explain your method with your teacher.":
                mode==6?
                "ORIGINAL 2021 KIRAN STORY: Kiran has 17 crayons, gives some away, and has 9 left. How many did he GIVE AWAY? Complete 17−□=9. Write UNKNOWN=number;EQUATION=number-number=number. Explain your strategy.":
                mode==7?
                "EDULYTICS ORIGINAL EXIT CHECK: Show how you could work out 16−7 using any correct strategy. How many remain? Check your answer by addition. Write DIFFERENCE=number;CHECK=number+number=number. If you choose a drawing or words, explain to your teacher: the digital numeric fields do not certify the drawing or your reasoning.":
                $"Choose a subtraction method for {a}-{b}. Check using related addition. Write DIFFERENCE=number;CHECK=number+number=number.",
            StoryUnknowns=>mode switch
            {
                0=>$"{a} birds are here and {b} more arrive. How many birds NOW? Write UNKNOWN=number;EQUATION=number+number=number.",
                1=>a==6&&b==12?
                    "ORIGINAL 2021 STORY: Elena has 6 counters. She gets some more. Now she has 18 counters. How many MORE did Elena get? Write UNKNOWN=number;EQUATION=number+number=number.":
                    $"{a} birds are here; some arrive, and now {t} birds are here. How many ARRIVED? Write UNKNOWN=number;EQUATION=number+number=number.",
                2=>"ORIGINAL 2021 STORY: Han has some pencils. He gets 9 pencils from the art store. Now he has 15 pencils. How many pencils did Han have to START? Write UNKNOWN=number;EQUATION=number+number=number. Explain using objects, drawings, numbers or words.",
                3=>$"{t} birds are here, {b} fly away. How many REMAIN? Show thinking and write UNKNOWN=number;EQUATION=number-number=number.",
                4=>a==6&&b==12?
                    "ORIGINAL 2021 STORY: Elena has 18 counters. She gets rid of SOME and now has 6 counters. How many did Elena get rid of? Show thinking and write UNKNOWN=number;EQUATION=number-number=number.":
                    "ORIGINAL 2021 STORY: Han has 15 pencils, gives SOME pencils to friends and now has 9. How many did he GIVE AWAY? Show thinking and write UNKNOWN=number;EQUATION=number-number=number.",
                _=>$"Some birds are here, {b} fly away, and {a} remain. How many were here at START? Write UNKNOWN=number;EQUATION=number-number=number."
            },
            CenterWithin20=>mode==0?
                $"ORIGINAL 2021 HOW CLOSE? Subtract from 20: select TWO number cards {a}, {b} from a teacher-led four-card hand. Subtract both from 20 to approach zero. Write RESULT=number;EQUATION=20-number-number=number. Partner comparison and winning points are teacher observed.":
                mode==2?
                $"ORIGINAL 2021 HOW CLOSE? Subtract from 20: select THREE number cards {a}, {b}, {c} from a teacher-led four-card hand. Subtract all three from 20 to approach zero. Write RESULT=number;EQUATION=20-number-number-number=number. The real card game remains teacher-led.":
                mode==1?
                $"COMPARE center: evaluate the source-style addition expression {a}+{b} within 20, then compare it to another partner's expression under teacher supervision. Write RESULT=number.":
                $"COMPARE center: evaluate the source-style subtraction expression {a}-{b} within 20, then compare it to another partner's expression under teacher supervision. Write RESULT=number.",
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
        int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"],sum=a+b;
        return family switch
        {
            CountOnOrAway=>mode==4?
                (a==14?
                    "UNKNOWN=8;EQUATION=8=14-6":
                    $"UNKNOWN={a-b};EQUATION={a}-{b}={a-b}") :
                mode==5?
                (a==18?"UNKNOWN=13;EQUATION=5+13=18":
                 a==13?"UNKNOWN=7;EQUATION=13=7+6":
                 "UNKNOWN=5;EQUATION=20=15+5") :
                mode==2?
                $"UNKNOWN={a-b};EQUATION={a}-{b}={a-b}":
                mode==3?
                $"UNKNOWN={a-b};EQUATION={a-b}+{b}={a}":
                $"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            FlexibleSubtract=>mode==5?
                "UNKNOWN=7;EQUATION=8+7=15":
                mode==6?
                "UNKNOWN=8;EQUATION=17-8=9":
                $"DIFFERENCE={a-b};CHECK={a-b}+{b}={a}",
            StoryUnknowns=>mode switch
            {
                0=>$"UNKNOWN={sum};EQUATION={a}+{b}={sum}",
                1=>$"UNKNOWN={b};EQUATION={a}+{b}={sum}",
                2=>$"UNKNOWN={a};EQUATION={a}+{b}={sum}",
                3=>$"UNKNOWN={a};EQUATION={sum}-{b}={a}",
                4=>$"UNKNOWN={b};EQUATION={sum}-{b}={a}",
                _=>$"UNKNOWN={sum};EQUATION={sum}-{b}={a}"
            },
            CenterWithin20=>mode==0?
                $"RESULT={20-a-b};EQUATION=20-{a}-{b}={20-a-b}":
                mode==2?
                $"RESULT={20-a-b-c};EQUATION=20-{a}-{b}-{c}={20-a-b-c}":
                $"RESULT={(mode==1?a+b:a-b)}",
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
