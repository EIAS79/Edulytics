using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;

/// <summary>Twenty-four source-specific, mathematically exact and genuinely new numeric families; one shared implementation.</summary>
internal static class UsCcssG2Unit4FinalSixQuestions
{
    private const string Prefix="usccss.g2.u04.l";
    private static readonly IReadOnlySet<string> Families=new HashSet<string>(StringComparer.Ordinal){
        "usccss.g2.u04.l10.subtract_tens_ones",
        "usccss.g2.u04.l10.add_tens_ones",
        "usccss.g2.u04.l10.remaining_after_subtraction",
        "usccss.g2.u04.l10.difference_count_on",
        "usccss.g2.u04.l11.reach_nearest_ten",
        "usccss.g2.u04.l11.subtract_tens_first",
        "usccss.g2.u04.l11.subtract_ones_first",
        "usccss.g2.u04.l11.decompose_to_ten",
        "usccss.g2.u04.l12.unknown_end_add",
        "usccss.g2.u04.l12.unknown_start_subtract",
        "usccss.g2.u04.l12.unknown_jump_add",
        "usccss.g2.u04.l12.unknown_start_add",
        "usccss.g2.u04.l13.train_missing_part",
        "usccss.g2.u04.l13.train_combined_parts",
        "usccss.g2.u04.l13.train_length_difference",
        "usccss.g2.u04.l13.tape_missing_part",
        "usccss.g2.u04.l14.target_line_move",
        "usccss.g2.u04.l14.exact_target_hit",
        "usccss.g2.u04.l14.overshot_target",
        "usccss.g2.u04.l14.captures_to_win",
        "usccss.g2.u04.l15.older_sister_age",
        "usccss.g2.u04.l15.mother_age_jump",
        "usccss.g2.u04.l15.younger_brother_age",
        "usccss.g2.u04.l15.grandmother_age_jump"
    };
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family!=null&&Families.Contains(family);
    private static int Lesson(string family)=>int.Parse(family.Substring(Prefix.Length,2),CultureInfo.InvariantCulture);
    private static string Kind(string family)=>family[(Prefix.Length+3)..];
    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown Grade2 original source-restricted numeric family");
        int lesson=Lesson(family),a=random.Next(10,51),b=random.Next(5,41),step=random.Next(1,11);
        int tens=random.Next(1,6)*10,ones=random.Next(1,10);
        int whole=random.Next(50,101),part=random.Next(11,41);
        int goal=random.Next(20,91),marker=random.Next(0,91),collected=random.Next(0,2);
        int less=random.Next(1,9),older=random.Next(5,30);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["lesson"]=lesson,["a"]=a,["b"]=b,["step"]=step,["tens"]=tens,
            ["ones"]=ones,["whole"]=whole,["part"]=part,["goal"]=goal,
            ["marker"]=marker,["collected"]=collected,["less"]=less,["older"]=older
        };
        var kind=Kind(family);
        int value;string prompt;
        switch(kind)
        {
            case "subtract_tens_ones":
                value=whole-tens-ones;prompt=$"FIRST-PARTY base-ten blocks: remove {tens} and {ones} from {whole}. What is the remainder? Show tens then ones.";break;
            case "add_tens_ones":
                value=a+tens+ones;prompt=$"FIRST-PARTY equal-unit number line: start {a}, jump RIGHT {tens} and RIGHT {ones}. What is the final point?";break;
            case "remaining_after_subtraction":
                value=whole-part;prompt=$"FIRST-PARTY number line: find {whole}−{part}, represent left moves by tens and ones.";break;
            case "difference_count_on":
                value=whole-part;prompt=$"FIRST-PARTY missing addend: {part}+?={whole}. How many unit intervals RIGHT from {part} to {whole}?";break;
            case "reach_nearest_ten":
                value=a%10;prompt=$"FIRST-PARTY subtraction: from {a} move LEFT to nearest lower multiple of ten. How many units left?";break;
            case "subtract_tens_first":
                value=whole-part;prompt=$"FIRST-PARTY subtraction by tens FIRST, then ones: {whole}−{part}.";break;
            case "subtract_ones_first":
                value=whole-part;prompt=$"FIRST-PARTY subtraction with ONES FIRST then tens: {whole}−{part}.";break;
            case "decompose_to_ten":
                value=(a%10);prompt=$"FIRST-PARTY making a ten: to go LEFT from {a} to the next lower multiple of10, how many unit spaces?";break;
            case "unknown_end_add":
                value=a+b;prompt=$"FIRST-PARTY original-style riddle: I start at {a} and jump RIGHT {b}. My endpoint is?";break;
            case "unknown_start_subtract":
                value=a+b;prompt=$"FIRST-PARTY riddle: I start at ?, move LEFT {b} and finish at {a}. What was my start?";break;
            case "unknown_jump_add":
                value=b;prompt=$"FIRST-PARTY riddle: number-line arrow starts at {a}, ends at {a+b}; what is the UNKNOWN jump LENGTH?";break;
            case "unknown_start_add":
                value=a;prompt=$"FIRST-PARTY riddle: ? + {b} = {a+b}. What is the UNKNOWN start?";break;
            case "train_missing_part":
                value=b;prompt=$"FIRST-PARTY cube train grows from {a} to {a+b}. How many new cubes?";break;
            case "train_combined_parts":
                value=a+b;prompt=$"FIRST-PARTY invented train has {a} cubes plus {b} cubes. How many altogether?";break;
            case "train_length_difference":
                value=b;prompt=$"FIRST-PARTY tape diagram whole {a+b}, one part {a}. What is the missing part?";break;
            case "tape_missing_part":
                value=b;prompt=$"FIRST-PARTY number-line arrow from {a} to {a+b} represents unknown change. What length?";break;
            case "target_line_move":
                value=marker+step;prompt=$"FICTIONAL Jump the Line: counter at {marker}, one invented rightward jump of {step}. Where now? Not an original game spin.";break;
            case "exact_target_hit":
                value=marker+step==goal?1:0;prompt=$"FICTIONAL center: at {marker}, jump +{step}, target {goal}. Enter 1 if EXACTLY hit target, else0.";break;
            case "overshot_target":
                value=marker+step>goal?1:0;prompt=$"FICTIONAL center: start {marker}, move +{step}, target {goal}. Enter1 if you PASS target, otherwise0.";break;
            case "captures_to_win":
                value=2-collected;prompt=$"FICTIONAL center already landed exactly on {collected} target(s). How many more exact landings to reach TWO?";break;
            case "older_sister_age":
                value=older+less;prompt=$"FICTIONAL age riddle: child {older} years; sister {less} years older. How old is the sister?";break;
            case "mother_age_jump":
                value=older+less+b;prompt=$"FICTIONAL age chain: sister age {older+less}, mother {b} years older. Mother's age?";break;
            case "younger_brother_age":
                value=older+less;prompt=$"FICTIONAL age chain: mother age {older+less+b}, brother {b} years YOUNGER. What is the brother's age?";break;
            case "grandmother_age_jump":
                value=older+less+b;prompt=$"FICTIONAL age chain: brother age {older+less}, grandma {b} years OLDER. Grandma's age?";break;
            default:throw new InvalidOperationException("Unsupported source-specific numeric family");
        }
        return new(family,prompt,$"Independent Edulytics recomputed result: {value}; no genuine source drawing or classroom record graded.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Bad first-party numeric contract");
        int a=p["a"],b=p["b"],step=p["step"],whole=p["whole"],part=p["part"],
            marker=p["marker"],goal=p["goal"],older=p["older"],less=p["less"];
        int value=Kind(family) switch{
            "subtract_tens_ones"=>whole-p["tens"]-p["ones"],
            "add_tens_ones"=>a+p["tens"]+p["ones"],
            "remaining_after_subtraction" or "difference_count_on" or "subtract_tens_first" or "subtract_ones_first"=>whole-part,
            "reach_nearest_ten" or "decompose_to_ten"=>a%10,
            "unknown_end_add" or "unknown_start_subtract" or "train_combined_parts"=>a+b,
            "unknown_jump_add" or "train_missing_part" or "train_length_difference" or "tape_missing_part"=>b,
            "unknown_start_add"=>a,
            "target_line_move"=>marker+step,
            "exact_target_hit"=>marker+step==goal?1:0,
            "overshot_target"=>marker+step>goal?1:0,
            "captures_to_win"=>2-p["collected"],
            "older_sister_age" or "younger_brother_age"=>older+less,
            "mother_age_jump" or "grandmother_age_jump"=>older+less+b,
            _=>throw new InvalidOperationException("Missing first-party family")
        };
        return value.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)=>
        Valid(family,p)&&string.Equals(response,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family)||p.Count!=13||
           !p.TryGetValue("lesson",out var l)||l!=Lesson(family)||
           !p.TryGetValue("a",out var a)||a<10||a>50||
           !p.TryGetValue("b",out var b)||b<5||b>40||
           !p.TryGetValue("step",out var step)||step<1||step>10||
           !p.TryGetValue("tens",out var tens)||tens<10||tens>50||tens%10!=0||
           !p.TryGetValue("ones",out var ones)||ones<1||ones>9||
           !p.TryGetValue("whole",out var whole)||whole<50||whole>100||
           !p.TryGetValue("part",out var part)||part<11||part>40||
           !p.TryGetValue("goal",out var goal)||goal<20||goal>90||
           !p.TryGetValue("marker",out var marker)||marker<0||marker>90||
           !p.TryGetValue("collected",out var captured)||captured<0||captured>1||
           !p.TryGetValue("less",out var less)||less<1||less>8||
           !p.TryGetValue("older",out var older)||older<5||older>29) return false;
        // Requiring positive number-line remainder and reasonable endpoints enforces Grade2 domains.
        if(whole-tens-ones<0)return false;
        if(a+tens+ones>100)return false;
        if(older+less+b>100)return false;
        return true;
    }
}
