using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Source-goal-specific gradeable digital math for ALL G1 Unit 5 lessons
/// (ordinals 89–102). Shared first-party solver, 14 lesson-specific contracts.
/// Physical centers, PLC oral explanation, actual Food Drive and teacher
/// observations remain separate and are never certified by these answers.
/// </summary>
public static class UsCcssG1FullGradeUnit5AdditionQuestions
{
    private enum Kind { TensOrOnes, NoComposeTwoDigits, MakeNewTen, ComposingTwoDigits, Flexible }
    private sealed record Family(string Code,Kind Kind,string LessonCode);
    private static readonly Family[] Specs =
    [
        new("usccss.g1.u05.l01.tens_or_ones_no_compose",Kind.TensOrOnes,"L01"),
        new("usccss.g1.u05.l02.explain_no_compose_equations",Kind.NoComposeTwoDigits,"L02"),
        new("usccss.g1.u05.l03.write_no_compose_equations",Kind.NoComposeTwoDigits,"L03"),
        new("usccss.g1.u05.l04.center_no_compose",Kind.NoComposeTwoDigits,"L04"),
        new("usccss.g1.u05.l05.make_new_ten",Kind.MakeNewTen,"L05"),
        new("usccss.g1.u05.l06.explain_make_ten_equations",Kind.MakeNewTen,"L06"),
        new("usccss.g1.u05.l07.detect_new_ten",Kind.Flexible,"L07"),
        new("usccss.g1.u05.l08.center_add_within100",Kind.Flexible,"L08"),
        new("usccss.g1.u05.l09.two_digit_composing",Kind.ComposingTwoDigits,"L09"),
        new("usccss.g1.u05.l10.tens_tens_ones_ones",Kind.ComposingTwoDigits,"L10"),
        new("usccss.g1.u05.l11.explain_addition_method",Kind.ComposingTwoDigits,"L11"),
        new("usccss.g1.u05.l12.write_addition_equations",Kind.ComposingTwoDigits,"L12"),
        new("usccss.g1.u05.l13.center_add_within100",Kind.Flexible,"L13"),
        new("usccss.g1.u05.l14.optional_food_drive_numeric",Kind.ComposingTwoDigits,"L14")
    ];
    public static readonly IReadOnlyList<string> Families=Specs.Select(x=>x.Code).ToArray();
    private static readonly int[] FoodPupils=[18,24,13,30];
    private static readonly int[] FoodGradeDayOne=[18,51,23,13,39,40,8,29,30];
    private static string Pupil(int n)=>n switch
    {
        18=>"Lin",24=>"Priya",13=>"Han",30=>"Tyler",
        _=>throw new InvalidOperationException("Unverified food-drive pupil")
    };
    private static string SchoolGrade(int n)=>n switch
    {
        18=>"Kindergarten",51=>"1st grade",23=>"2nd grade",
        13=>"3rd grade",39=>"4th grade",40=>"5th grade",
        8=>"6th grade",29=>"7th grade",30=>"8th grade",
        _=>throw new InvalidOperationException("Unverified source school-grade")
    };
    private static bool FoodDriveValid(int a,int b,int mode)=>
        a!=b&&(mode<=1?
            FoodPupils.Contains(a)&&FoodPupils.Contains(b):
            FoodGradeDayOne.Contains(a)&&FoodGradeDayOne.Contains(b)
            &&(mode!=3||a!=13&&b!=13&&a+b+13<=100));
    private static string FoodDriveSolve(int a,int b,int mode)=>mode switch
    {
        0=>$"PAIR={a+b};ALL=85",
        1=>$"FIRST_PAIR={a+b};SECOND_PAIR={85-a-b};ALL=85",
        2=>$"TOTAL={a+b};PACK={(a+b>=35&&a+b<=65?"YES":"NO")}",
        3=>$"TOTAL={a+b+13};PACK={(a+b+13>=35&&a+b+13<=65?"YES":"NO")}",
        _=>throw new InvalidOperationException("Unreviewed Food Drive variant")
    };
    private static Problem FoodDriveBuild(string family,Random random,int mode)
    {
        for(int attempt=0;attempt<400;attempt++)
        {
            var available=mode<=1?FoodPupils:FoodGradeDayOne;
            int a=available[random.Next(available.Length)];
            int b=available[random.Next(available.Length)];
            if(!FoodDriveValid(a,b,mode))continue;
            var p=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
            string question=mode switch
            {
                0=>$"Original Food Drive student table: Lin 18, Priya 24, Han 13, Tyler 30 cans. How many cans did {Pupil(a)} and {Pupil(b)} collect together? How many did all FOUR collect? Write PAIR=number;ALL=number.",
                1=>$"Original Food Drive student table: Lin 18, Priya 24, Han 13, Tyler 30 cans. Partner A/B: Group {Pupil(a)} and {Pupil(b)} into one pair and the OTHER two students into a second pair. How many does each pair have, and how many cans did all four collect? Write FIRST_PAIR=number;SECOND_PAIR=number;ALL=number.",
                2=>$"Original Food Drive Day 1 grade table: {SchoolGrade(a)} collected {a} cans and {SchoolGrade(b)} collected {b} cans. A box must hold between 35 and 65 cans INCLUSIVE from exactly TWO grades. Calculate the total and whether these two grades qualify. Write TOTAL=number;PACK=YES/NO.",
                _=>$"Original Food Drive optional challenge: {SchoolGrade(a)} collected {a}, {SchoolGrade(b)} collected {b}, and 3rd grade collected 13 cans. Could these THREE distinct grades share ONE box requiring 35 to 65 cans INCLUSIVE? Write TOTAL=number;PACK=YES/NO."
            };
            return new(family,question,
                "These are copied numerical quantities only from the source 2021 student tables. Demonstrating math grading does not claim the children actually collected cans, packed boxes or discussed alternatives.",
                AssessmentItemType.ShortAnswer,p);
        }
        throw new InvalidOperationException("Cannot sample source-true Food Drive group combination");
    }

    public sealed record Problem(string Family,string Prompt,string Explanation,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    private static Family? Get(string? f)=>Specs.SingleOrDefault(x=>x.Code==f);
    public static bool Supports(string? family)=>Get(family)!=null;
    public static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        var spec=Get(family);
        if(spec==null||p.Count!=3||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("mode",out int mode)||mode<0||mode>3)
            return false;
        if(spec.LessonCode=="L14")return FoodDriveValid(a,b,mode);
        int ones=a%10+b%10,sum=a+b;
        return spec.Kind switch
        {
            Kind.TensOrOnes=>a>=20&&a<=89&&sum<=99&&
                ((b>=1&&b<=9&&ones<10)||
                 (b>=10&&b<=30&&b%10==0)),
            Kind.NoComposeTwoDigits=>a>=10&&a<=89&&b>=10&&b<=89&&sum<=99&&ones<10,
            Kind.MakeNewTen=>a>=20&&a<=89&&b>=1&&b<=9&&sum<=99&&ones>=10,
            Kind.ComposingTwoDigits=>a>=10&&a<=89&&b>=10&&b<=89&&sum<=100&&ones>=10,
            Kind.Flexible=>a>=20&&a<=89&&b>=1&&b<=(spec.LessonCode=="L07"?9:10)&&sum<=99&&
                (spec.LessonCode!="L07"||mode<=1||
                 (mode==2?a%10>=1&&b>=10-a%10:
                          a%10<=8&&b<10-a%10)),
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        var spec=Get(family)??throw new InvalidOperationException("Unreviewed G1 Unit 5 Practice family");
        int a=0,b=0;
        int mode=random.Next(4);
        if(spec.LessonCode=="L14")return FoodDriveBuild(family,random,mode);
        // Generate genuine operand choices satisfying this source-goal family.
        for(int tries=0;tries<1000;tries++)
        {
            a=spec.Kind switch
            {
                Kind.NoComposeTwoDigits or Kind.ComposingTwoDigits=>random.Next(10,90),
                _=>random.Next(20,90)
            };
            b=spec.Kind switch
            {
                Kind.NoComposeTwoDigits or Kind.ComposingTwoDigits=>random.Next(10,90),
                Kind.TensOrOnes when mode%2==1=>random.Next(1,4)*10,
                Kind.Flexible when spec.LessonCode=="L07"&&mode==2=>
                    random.Next(Math.Min(9,10-a%10),10),
                Kind.Flexible when spec.LessonCode=="L07"&&mode==3=>
                    random.Next(1,Math.Max(2,10-a%10)),
                Kind.Flexible=>random.Next(1,spec.LessonCode=="L07"?10:11),
                _=>random.Next(1,10)
            };
            var candidate=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
            if(Valid(family,candidate))
            {
                // Only source-permitted valid operands enter the verified prompt.
                string activity=spec.LessonCode switch
                {
                    "L02" or "L06" or "L11"=>"Interpret and explain the true equations.",
                    "L03" or "L07" or "L12"=>"Write equations that preserve the sum at every step.",
                    "L04" or "L08" or "L13"=>"Solve the digital number task; physical center turns are teacher-led.",
                    "L14"=>"Hypothetical food-drive groups only; no actual donation or class activity is certified.",
                    _=>"Use a drawing, ten-frame, or tens-and-ones model."
                };
                string operation=spec.LessonCode=="L14"
                    ?$"Two hypothetical food-drive boxes contain {a} and {b} items."
                    :$"Add {a}+{b} within 100.";
                if(spec.LessonCode=="L07"&&mode>=2)
                {
                    string condition=mode==2?"WOULD make a new ten":"would NOT make a new ten";
                    string promptMissing=
                        $"Original 2021 OPEN-CHOICE Missing Numbers task: {a}+□. " +
                        $"What positive ONE-DIGIT number COULD be put in the box that {condition}? " +
                        "Choose ANY valid missing value and show the actual sum. " +
                        "Write MISSING=number;NEW_TEN=YES/NO;TOTAL=number. " +
                        "Use place-value reasoning; teacher reviews children's own explanations.";
                    return new(family,promptMissing,
                        "Source allows several mathematically valid additions: the verifier " +
                        "accepts any correct one-digit choice meeting the stated make-ten " +
                        "constraint, not only the generator's sample answer. Teacher-led " +
                        "written and two-digit extensions remain independent classroom work.",
                        AssessmentItemType.ShortAnswer,candidate);
                }
                string prompt=spec.Kind switch
                {
                    Kind.MakeNewTen=>$"{activity} {operation} To form the next ten, split the smaller addend. Write MOVE=number;REST=number;TOTAL=number.",
                    Kind.ComposingTwoDigits=>$"{activity} {operation} Combine tens and ones, exchanging ten ones for a new ten. Write TENS=number;ONES=number;TOTAL=number.",
                    _=>$"{activity} {operation} Check if ten ones compose a new ten. Write NEW_TEN=YES/NO;TOTAL=number."
                };
                return new(family,prompt,
                    "Source-goal within-100 arithmetic: make ten when required, preserve the original addends, and verify both the intermediate number of tens and the total.",
                    AssessmentItemType.ShortAnswer,candidate);
            }
        }
        throw new InvalidOperationException("Unable to sample a valid G1 Unit 5 source-skill problem");
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G1 U05 source-scoped operands");
        var spec=Get(family)!;int a=p["a"],b=p["b"],sum=a+b;
        if(spec.LessonCode=="L14")return FoodDriveSolve(a,b,p["mode"]);
        if(spec.LessonCode=="L07"&&p["mode"]>=2)
            return $"MISSING={b};NEW_TEN={(p["mode"]==2?"YES":"NO")};TOTAL={sum}";
        return spec.Kind switch
        {
            Kind.MakeNewTen=>$"MOVE={10-a%10};REST={b-(10-a%10)};TOTAL={sum}",
            Kind.ComposingTwoDigits=>$"TENS={sum/10};ONES={sum%10};TOTAL={sum}",
            _=>$"NEW_TEN={(a%10+b%10>=10?"YES":"NO")};TOTAL={sum}"
        };
    }
    private static string Norm(string? s)=>string.Concat((s??"").Where(c=>!char.IsWhiteSpace(c))).ToUpperInvariant();
    public static bool GradePersistedAnswer(string? family,string? expected,string? submitted)
    {
        if(!Supports(family)||string.IsNullOrWhiteSpace(expected)||string.IsNullOrWhiteSpace(submitted))return false;
        var e=Norm(expected).Split(';');var r=Norm(submitted).Split(';');
        if(e.Length!=r.Length)return false;
        var em=new Dictionary<string,string>(StringComparer.Ordinal);
        var rm=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var (items,map) in new[]{(e,em),(r,rm)})
            foreach(var item in items)
            {
                int at=item.IndexOf('=');
                if(at<=0||at==item.Length-1||!map.TryAdd(item[..at],item[(at+1)..]))return false;
            }
        return em.Count==rm.Count&&em.All(x=>rm.TryGetValue(x.Key,out var value)&&value==x.Value);
    }
    private static bool VerifyOpenMissingAddend(IReadOnlyDictionary<string,int> p,string? answer)
    {
        var values=Norm(answer).Split(';');
        if(values.Length!=3)return false;
        var fields=new Dictionary<string,string>(StringComparer.Ordinal);
        foreach(var part in values)
        {
            int pos=part.IndexOf('=');
            if(pos<=0||pos==part.Length-1||
               !fields.TryAdd(part[..pos],part[(pos+1)..]))return false;
        }
        if(fields.Count!=3||
           !fields.TryGetValue("MISSING",out var digitText)||
           !fields.TryGetValue("NEW_TEN",out var tenText)||
           !fields.TryGetValue("TOTAL",out var totalText)||
           !int.TryParse(digitText,out var digit)||digit<1||digit>9||
           !int.TryParse(totalText,out var total)||total!=p["a"]+digit||total>99)
            return false;
        bool makesNewTen=p["a"]%10+digit>=10;
        return makesNewTen==(p["mode"]==2) &&
               tenText==(makesNewTen?"YES":"NO");
    }

    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? submitted)=>
        Valid(family,p)&&
        (family==Specs[6].Code&&p["mode"]>=2?
            VerifyOpenMissingAddend(p,submitted):
            GradePersistedAnswer(family,Solve(family!,p),submitted));
}
