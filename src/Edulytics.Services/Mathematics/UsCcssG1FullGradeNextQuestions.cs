using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-goal-scoped Grade 1 new lessons U03 L19–L23 (ordinals 56–60).
/// Automated numeric portions only; classroom partner strategies, cards,
/// How Close? and Five in a Row require real teacher facilitation.
/// </summary>
public static class UsCcssG1FullGradeNextQuestions
{
    public const string FlexibleAdd="usccss.g1.u03.l19.flexible.make_ten";
    public const string ZooThree="usccss.g1.u03.l20.zoo.three_addends";
    public const string CenterTwoThree="usccss.g1.u03.l21.center.two_three_addends";
    public const string TeenSubtract="usccss.g1.u03.l22.teen.subtract_cross_ten";
    public const string TenSubtract="usccss.g1.u03.l23.ten.subtract_in_steps";
    public static readonly IReadOnlyList<string> Families=[FlexibleAdd,ZooThree,CenterTwoThree,TeenSubtract,TenSubtract];
    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is not null&&Families.Contains(family,StringComparer.Ordinal);
    private static string Normalize(string? value)=>
        string.Concat((value??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=4||
           !p.TryGetValue("a",out var a)||!p.TryGetValue("b",out var b)||
           !p.TryGetValue("c",out var c)||!p.TryGetValue("mode",out var mode)||
           mode<0||(mode>3&&(family!=ZooThree||mode!=4)))return false;
        return family switch
        {
            FlexibleAdd=>a>=6&&a<=9&&b>=11-a&&b<=10&&c==0&&
                (mode!=2||(a==8&&b==7)),
            ZooThree=>a>=1&&a<=8&&b>=1&&b<=8&&c>=1&&c<=8&&a+b+c<=20&&
                (mode!=2||(a==8&&b==7&&c==5))&&
                (mode!=3||(a==3&&b==6&&c==7))&&
                (mode!=4||(a==8&&b==4&&c==7)), 
            CenterTwoThree=>mode==3?
                (a>=0&&a<=10&&b>=7&&b<=9&&c==0&&a+b<=20):
                (a>=0&&a<=9&&b>=0&&b<=9&&
                 (mode==1?c>=0&&c<=9:c==0)&&a+b+c<=20),
            TeenSubtract=>a>=11&&a<=19&&b>=0&&b<=9&&c==0&&
                (mode!=3||(a==16&&b==7)),
            TenSubtract=>a>=11&&a<=19&&b>=0&&b<=9&&c==0&&
                (mode!=2||(a==15&&b==8)),
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unreviewed G1 L56–60 Practice family");
        int mode=random.Next(family==ZooThree?5:4),a=0,b=0,c=0;
        if(family==FlexibleAdd)
        {
            if(mode==2){a=8;b=7;} // Original 2021 Lin/Han/Kiran 8+7 task.
            else{a=random.Next(6,10);b=random.Next(11-a,11);}
        }
        else if(family==ZooThree)
        {
            if(mode==2){a=8;b=7;c=5;} // Original reptile story.
            else if(mode==3){a=3;b=6;c=7;} // Original bird exhibit story.
            else if(mode==4){a=8;b=4;c=7;} // 2021 Kendall Hunt primate cool-down task independently quoted by EdReports' 2021 review.
            else if(mode==0 && random.Next(2)==0)
            {a=8;b=4;c=3;} // Original large cats: lions, tigers, cheetahs.
            else if(mode==1 && random.Next(2)==0)
            {a=8;b=7;c=4;} // Original petting zoo: goats, sheep, pigs.
            else
            {
                a=random.Next(1,9);b=random.Next(1,9);
                c=random.Next(1,Math.Min(8,20-a-b)+1);
            }
        }
        else if(family==CenterTwoThree)
        {
            if(mode==3)
            {
                // Original 2021 Five in a Row: numeral card + 7, 8 or 9.
                a=random.Next(0,11);b=random.Next(7,10);c=0;
            }
            else
            {
                // How Close? Add to 20: two- or three-card combination.
                a=random.Next(0,10);b=random.Next(0,10);
                c=mode==1?random.Next(0,Math.Min(9,20-a-b)+1):0;
            }
        }
        else if(family==TeenSubtract)
        {
            if(mode==3){a=16;b=7;} // Original Elena's 16-crayon story.
            else{a=random.Next(11,20);b=random.Next(0,10);} // 2021 teen cards 11–19; subtraction cards 0–9.
        }
        else
        {
            if(mode==2){a=15;b=8;} // Original Diego's number-card subtraction.
            else{a=random.Next(11,20);b=random.Next(0,10);} // Source 2021 digit cards 0–9 (same card game as prior lesson).
        }
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {["a"]=a,["b"]=b,["c"]=c,["mode"]=mode};
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 L56–60 parameters");
        string prompt=family switch
        {
            FlexibleAdd=>mode==2?
                "Original 2021 Student Facing: Lin, Han and Kiran each solve 8+7. Compare the THREE mathematically equivalent methods: Lin uses 8+2+5, Han uses 7+7+1, Kiran uses 8+8-1. Write LIN=expression=value;HAN=expression=value;KIRAN=expression=value;TOTAL=number. Draw or explain why each method works with your teacher.":
                $"Compare addition methods for {a}+{b} within 20. Make a ten by moving counters from the second addend. Write MOVE=number;REST=number;TOTAL=number.",
            ZooThree=>mode==4?
                "ORIGINAL 2021 COOL-DOWN (corroborated in EdReports' review of Kendall Hunt Grade 1): Jada counts 8 monkeys, 4 gorillas and 7 orangutans. How many primates are there? Write TOTAL=number;EQUATION=number+number+number=number. Show drawings, numbers or words to your teacher; digital grading checks only the equation.":
                mode==2?
                $"ORIGINAL 2021 STUDENT STORY: Jada sees 8 snakes, 7 iguanas and 5 frogs at the reptile exhibit. How many reptiles? Count all THREE groups and write the matching equation. Write TOTAL=number;EQUATION=number+number+number=number.":
                mode==3?
                "ORIGINAL 2021 STUDENT STORY: In the bird exhibit Jada sees 3 herons, 6 hawks and 7 hummingbirds. How many birds did Jada see? Show thinking using drawings, counters, numbers or words. Write TOTAL=number;EQUATION=number+number+number=number.":
                mode==0 && a==8 && b==4 && c==3?
                "ORIGINAL 2021 LARGE CATS: Jada sees 8 lions, 4 tigers and 3 cheetahs. How many large cats are there? Show all groups and write TOTAL=number;EQUATION=number+number+number=number.":
                mode==1 && a==8 && b==7 && c==4?
                "ORIGINAL 2021 PETTING ZOO: Jada pets 8 goats, 7 sheep and 4 pigs. How many animals did she pet? Show all groups and write TOTAL=number;EQUATION=number+number+number=number.":
                mode%2==0?
                $"At a zoo, there are {a} zebras, {b} penguins and {c} monkeys. All three groups count. Write TOTAL=number;EQUATION=number+number+number=number.":
                $"At a zoo there are {a} lions, {b} birds and {c} seals. Use ANY correct addition method for all three groups. Write TOTAL=number;EQUATION=number+number+number=number.",
            CenterTwoThree=>mode==3
                ?$"Original 2021 FIVE IN A ROW: choose a numeral card {a}; choose to add 7, 8 or 9 (chosen {b}). Calculate {a}+{b}, and write TOTAL=number;EQUATION=number+number=number. The actual card/board/partner game remains teacher-led."
                :mode==1
                ?$"How Close? Add to 20: choose THREE cards {a}, {b} and {c} and find how close the sum is to 20. Write TOTAL=number;EQUATION=number+number+number=number. Comparing hands and assigning a winner requires a teacher-led game."
                :$"How Close? Add to 20: choose TWO cards {a} and {b} and find how close their sum is to 20. Write TOTAL=number;EQUATION=number+number=number.",
            TeenSubtract=>mode==3?
                "ORIGINAL 2021 STUDENT FACING: Elena has 16 crayons and gives 7 crayons to Diego. How many crayons remain? Show your thinking with drawings, numbers, or words. For this digital method, write REACH10=number;REST=number;DIFFERENCE=number;EQUATION=number-number=number. Original classroom drawings, posters and alternative strategies require teacher review.":
                b<a-10?
                $"Original 2021 NUMBER CARD SUBTRACTION numeric subskill: start with teen number {a} and subtract digit card {b} (0 through 9). Subtract from the ones WITHOUT crossing ten; forcing a cross-ten split would be wrong. Write DIFFERENCE=number;EQUATION=number-number=number. Physical cards and strategy drawings stay teacher-led.":
                $"Take From: start with {a} counters and remove {b}. First reach ten, then remove the rest. Write REACH10=number;REST=number;DIFFERENCE=number;EQUATION=number-number=number. Original 2021 number cards range from 0 through 9; card draws and drawings stay teacher-led.",
            TenSubtract=>mode==2?
                "ORIGINAL 2021 STUDENT FACING: Diego starts Number Card Subtraction with 15, then picks an 8. Show 15 on ten-frames, subtract 8 using a ten, and write the matching EQUATION. Write TO10=number;REST=number;DIFFERENCE=number;EQUATION=number-number=number. Explain your drawing to a teacher.":
                b<a-10?
                $"ORIGINAL NUMBER CARD SUBTRACTION WITH TEN-FRAMES: show teen card {a} and remove digit card {b} (0 through 9), WITHOUT crossing ten. Do not invent a negative remainder. Write DIFFERENCE=number;EQUATION=number-number=number. The physical cards and explanatory drawings remain teacher-led.":
                $"Use ten as a landmark for {a}-{b}. First remove the extra ones to reach ten, then subtract the rest. Write TO10=number;REST=number;DIFFERENCE=number;EQUATION=number-number=number. Source 2021 physical cards and teacher-led ten-frame drawings are not auto-assessed.",
            _=>throw new InvalidOperationException()
        };
        return new(family,prompt,
            "Represent the quantities with counters or ten-frames and check equivalent computations. Teacher-led center participation is not digitally certified.",
            AssessmentItemType.ShortAnswer,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Unreviewed or impossible math instance");
        int a=p["a"],b=p["b"],c=p["c"],mode=p["mode"];
        return family switch
        {
            FlexibleAdd=>mode==2?
                "LIN=8+2+5=15;HAN=7+7+1=15;KIRAN=8+8-1=15;TOTAL=15":
                $"MOVE={10-a};REST={b-(10-a)};TOTAL={a+b}",
            ZooThree=>$"TOTAL={a+b+c};EQUATION={a}+{b}+{c}={a+b+c}",
            CenterTwoThree=>mode==1
                ?$"TOTAL={a+b+c};EQUATION={a}+{b}+{c}={a+b+c}"
                :$"TOTAL={a+b};EQUATION={a}+{b}={a+b}",
            TeenSubtract=>b<a-10
                ?$"DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}"
                :$"REACH10={a-10};REST={b-(a-10)};DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}",
            TenSubtract=>b<a-10
                ?$"DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}"
                :$"TO10={a-10};REST={b-(a-10)};DIFFERENCE={a-b};EQUATION={a}-{b}={a-b}",
            _=>throw new InvalidOperationException()
        };
    }
    public static bool GradePersistedAnswer(string? family,string? expected,string? response)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(response))
            return false;
        var e=Normalize(expected).Split(';');var r=Normalize(response).Split(';');
        if(e.Length!=r.Length)return false;
        var left=new Dictionary<string,string>(StringComparer.Ordinal);
        var right=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (parts,map) in new[]{(e,left),(r,right)})
            foreach(var part in parts)
            {
                int pos=part.IndexOf('=');
                if(pos<=0||pos==part.Length-1||!map.TryAdd(part[..pos],part[(pos+1)..]))
                    return false;
            }
        if(left.Count!=right.Count)return false;
        return left.All(item=>right.TryGetValue(item.Key,out var value)&&value==item.Value);
    }
    // Original 2021 three-group zoo and center stories accept regrouping and
    // commutativity, not one arbitrary textual addend order. Check the exact
    // story quantities so an unrelated expression with the same sum fails.
    private static bool VerifySourceGroupEquation(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        var parts=Normalize(response).Split(';');
        if(parts.Length!=2)return false;
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var part in parts)
        {
            var cut=part.IndexOf('=');
            if(cut<=0||cut==part.Length-1||
                !fields.TryAdd(part[..cut],part[(cut+1)..]))return false;
        }
        if(fields.Count!=2||!fields.TryGetValue("TOTAL",out var totalText)||
           !fields.TryGetValue("EQUATION",out var equation)||
           !int.TryParse(totalText,out var reportedTotal))return false;
        var requiredTotal=p["a"]+p["b"]+p["c"];
        if(reportedTotal!=requiredTotal)return false;
        var equality=equation.Split('=');
        if(equality.Length!=2||
           !int.TryParse(equality[1],out var result)||result!=requiredTotal)return false;
        var operands=equality[0].Split('+');
        // A selected zero-card is still a third card in the How Close mode=1 task.
        // Do not infer operand COUNT from its numeric VALUE.
        var threeGroups=family==ZooThree||(family==CenterTwoThree&&p["mode"]==1);
        var sourceOperands=threeGroups
            ?new[]{p["a"],p["b"],p["c"]}
            :new[]{p["a"],p["b"]};
        if(operands.Length!=sourceOperands.Length)return false;
        var parsed=new int[operands.Length];
        for(int i=0;i<operands.Length;i++)
            if(!int.TryParse(operands[i],out parsed[i]))return false;
        Array.Sort(parsed);
        Array.Sort(sourceOperands);
        return parsed.SequenceEqual(sourceOperands);
    }

    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p) &&
        (family is ZooThree or CenterTwoThree?
            VerifySourceGroupEquation(family!,p,response):
            GradePersistedAnswer(family,Solve(family!,p),response));
}
