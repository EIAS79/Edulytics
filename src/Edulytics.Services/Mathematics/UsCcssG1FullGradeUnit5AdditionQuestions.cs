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
        4=>"MIN_BOXES=4",
        _=>throw new InvalidOperationException("Unreviewed Food Drive variant")
    };
    private static Problem FoodDriveBuild(string family,Random random,int mode)
    {
        if(mode==4)
        {
            // The original 2021 final question asks for the fewest boxes across ALL grades.
            // Four boxes are possible only when the optional three-grade grouping is allowed;
            // children's own grouping proof remains a teacher-reviewed paper activity.
            string[] phrasings =
            [
                "The original Food Drive Day 1 totals for all nine grades are 18, 51, 23, 13, 39, 40, 8, 29, 30.",
                "Food Drive minimum-box challenge: Kindergarten 18, first 51, second 23, third 13, fourth 39, fifth 40, sixth 8, seventh 29, eighth 30.",
                "From the original Food Drive nine-grade table, the grade totals are 18, 51, 23, 13, 39, 40, 8, 29, 30.",
                "The original grade-level Food Drive collected 251 cans in nine intact grade groups: 18, 51, 23, 13, 39, 40, 8, 29, 30."
            ];
            return new(family,
                phrasings[random.Next(phrasings.Length)]+" If every box has 35–65 cans and the OPTIONAL three-grade combination is allowed, what is the least number of boxes that can hold all cans, using each grade group exactly once? Write MIN_BOXES=number. Explain a four-box grouping to your teacher; digital scoring checks only the minimum number.",
                "The total is 251 cans. Three 65-can boxes hold only 195. A four-box witness is 18+39+8=65, 51+13=64, 23+40=63, 29+30=59; each grade is used once. A strict two-grade-per-box restriction would NOT permit this four-box witness.",
                AssessmentItemType.ShortAnswer,
                new Dictionary<string,int>{{"a",0},{"b",0},{"mode",4}});
        }
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

    private static readonly (int a,int b)[] Lesson1OriginalEquations =
    [
        (43,5),(43,50),(51,3),(51,30),(2,75),
        (20,75),(93,6),(60,28),(5,74),
        (34,5),(34,50),(4,45) // Public 2021 teacher Priya and Mai/Andre comparisons.
    ];
    private static readonly (int a,int b)[] Lesson2OriginalEquations =
    [
        (23,45),(63,25)
    ];
    private static readonly (int a,int b)[] Lesson3OriginalEquations =
    [
        (13,21),(13,31),(23,31),(33,41),(32,54),(23,74)
    ];
    private static readonly (int a,int b)[] Lesson8OriginalWarmup =
    [
        (68,2),(3,68),(5,45),(45,7)
    ];
    private static readonly (int a,int b)[] Lesson9OriginalWarmup =
    [
        (38,2),(40,3),(38,5),(38,15)
    ];
    private static readonly (int a,int b)[] Lesson10OriginalEquations =
    [
        (28,56),(27,44)
    ];
    private static readonly (int a,int b)[] Lesson11OriginalStories =
    [
        (37,25),(23,68)
    ];
    private static readonly (int a,int b)[] Lesson12OriginalEquations =
    [
        (48,15),(57,36)
    ];
    private static (int a,int b)? Exact2021Equation(string lesson,int mode)
    {
        var examples=lesson switch
        {
            "L01"=>Lesson1OriginalEquations,
            "L02"=>Lesson2OriginalEquations,
            "L03"=>Lesson3OriginalEquations,
            "L08"=>Lesson8OriginalWarmup,
            "L09"=>Lesson9OriginalWarmup,
            "L10"=>Lesson10OriginalEquations,
            "L11"=>Lesson11OriginalStories,
            "L12"=>Lesson12OriginalEquations,
            _=>null
        };
        return examples is not null&&mode>=4&&mode<examples.Length+4
            ?examples[mode-4]:null;
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
           !p.TryGetValue("mode",out int mode)||mode<0||
           mode>(spec.LessonCode=="L01"?15:spec.LessonCode=="L02"?6:spec.LessonCode=="L03"?10:spec.LessonCode=="L07"?5:
           spec.LessonCode is "L08" or "L09"?7:
           spec.LessonCode is "L10" or "L11" or "L12"?5:
           spec.LessonCode=="L14"?4:3))
            return false;
        if(spec.LessonCode=="L14")return mode==4 ? a==0&&b==0 : FoodDriveValid(a,b,mode);
        // Independent first-party equation proxy for the two distinct source
        // student strategies (not an automated judgment of publisher drawings).
        if(spec.LessonCode=="L02"&&mode==6)return a==63&&b==25;
        if(spec.LessonCode=="L03"&&mode==10)return a==32&&b==54;
        if(mode>=4&&(spec.LessonCode is "L01" or "L02" or "L03" or "L08" or "L09" or "L10" or "L11" or "L12"))
        {
            var source=Exact2021Equation(spec.LessonCode,mode);
            return source.HasValue&&a==source.Value.a&&b==source.Value.b;
        }
        int ones=a%10+b%10,sum=a+b;
        return spec.Kind switch
        {
            Kind.TensOrOnes=>a>=20&&a<=89&&sum<=99&&
                ((b>=1&&b<=9&&ones<10)||
                 (b>=10&&b<=30&&b%10==0)),
            Kind.NoComposeTwoDigits=>a>=10&&a<=89&&b>=10&&b<=89&&sum<=99&&ones<10,
            Kind.MakeNewTen=>a>=20&&a<=89&&b>=1&&b<=9&&sum<=99&&ones>=10&&
                (spec.LessonCode!="L05"||
                    (mode!=1||(a==37&&b==7))&&
                    (mode!=2||(a==47&&b==8))&&
                    (mode!=3||(a==26&&b==8)))&&
                (spec.LessonCode!="L06"||
                    (mode!=2||(a==34&&b==9))&&
                    (mode!=3||(a==68&&b==6))),
            Kind.ComposingTwoDigits=>a>=10&&a<=89&&b>=10&&b<=89&&sum<=100&&ones>=10,
            Kind.Flexible=>spec.LessonCode=="L07"&&mode>=2?
                (mode==2?a==32&&b>=8&&b<=9:
                 mode==3?a==16&&b>=0&&b<=3:
                 mode==4?a==8&&b>=10&&b<=91&&b%10>=2:
                         a==8&&b>=10&&b<=91&&b%10<=1):
                a>=20&&a<=89&&b>=1&&b<=10&&sum<=99,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        var spec=Get(family)??throw new InvalidOperationException("Unreviewed G1 Unit 5 Practice family");
        int a=0,b=0;
        int mode=random.Next(spec.LessonCode=="L01"?16:spec.LessonCode=="L02"?7:spec.LessonCode=="L03"?11:spec.LessonCode=="L07"?6:
            spec.LessonCode is "L08" or "L09"?8:
            spec.LessonCode is "L10" or "L11" or "L12"?6:
            spec.LessonCode=="L14"?5:4);
        if(spec.LessonCode=="L14")return FoodDriveBuild(family,random,mode);
        if(spec.LessonCode=="L02"&&mode==6)
        {
            var p=new Dictionary<string,int>{{"a",63},{"b",25},{"mode",6}};
            if(!Valid(family,p))throw new InvalidOperationException("Lesson 90 source strategy parameters drift");
            return new(family,
                "EDULYTICS FIRST-PARTY TWO-STRATEGY CHECK, aligned to original 2021 student 63+25: " +
                "Method A combines 60+20 then 3+5. Method B adds 20 to 63, then 5. " +
                "Complete each true intermediate result: ELENA_TENS=number;ELENA_ONES=number;" +
                "ANDRE_FIRST=number;ANDRE_SECOND=number;TOTAL=number. " +
                "These are authored equations, NOT a claimed reconstruction or automated grading of the publisher drawings.",
                "Both methods give 88; teacher checks whether the student's drawings or verbal reasoning show place value.",
                AssessmentItemType.ShortAnswer,p);
        }
        if(spec.LessonCode=="L03"&&mode==10)
        {
            var p=new Dictionary<string,int>{{"a",32},{"b",54},{"mode",10}};
            if(!Valid(family,p))throw new InvalidOperationException("Lesson 91 original math drift");
            return new(family,
                "EDULYTICS FIRST-PARTY METHOD WRITING, from original 2021 Lin and Han source example 32+54: " +
                "Lin combines the tens 30+50, then ones 2+4. Han starts from 54, adds 30, and then adds 2. " +
                "Write LIN_TENS=number;LIN_ONES=number;HAN_FIRST=number;HAN_SECOND=number;TOTAL=number. " +
                "The actual pupil-created equations, drawings and explanations need teacher review.",
                "Lin: 30+50=80 and 2+4=6; Han: 54+30=84 and 84+2=86; both get 86.",
                AssessmentItemType.ShortAnswer,p);
        }
        var original=Exact2021Equation(spec.LessonCode,mode);
        if(original.HasValue)
        {
            (a,b)=original.Value;
            var sourceP=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
            if(!Valid(family,sourceP))throw new InvalidOperationException("Invalid source exact addition operands");
            return new(family,
                spec.LessonCode=="L01"&&mode>=13?
                $"ORIGINAL 2021 U05 L01 TEACHER DISCUSSION: Find {a}+{b} by place value; distinguish ones from tens. Write NEW_TEN=NO;TOTAL=number. Oral source discussion remains teacher-assessed.":
                spec.LessonCode=="L01"?
                $"ORIGINAL 2021 U05 L01 STUDENT EQUATION: Add {a}+{b} using tens or ones without making a new ten. Write NEW_TEN=NO;TOTAL=number. Teacher reviews the base-ten drawing.":
                spec.LessonCode is "L08" or "L09"?
                $"ORIGINAL 2021 U05 {spec.LessonCode} NUMBER TALK: Add {a}+{b}. Did the ones make a NEW TEN? Write NEW_TEN=YES/NO;TOTAL=number. Teacher reviews the mental explanation.":
                spec.LessonCode is "L02" or "L03"?
                $"ORIGINAL 2021 U05 {spec.LessonCode} STUDENT EQUATION: Explain why {a}+{b} is true by adding tens and ones separately. Write TENS=total_tens_value;ONES=ones_value;TOTAL=sum. Partner method explanations and drawings require teacher review.":
                $"ORIGINAL 2021 U05 {spec.LessonCode} STUDENT EQUATION/STORY: Add {a}+{b} with a new ten. Write TENS=number_of_tens;ONES=remaining_ones;TOTAL=number. Teacher reviews the addition strategy.",
                "First-party source-aligned numeric comparison; the original publisher picture and pupil oral explanation are separately teacher assessed.",
                AssessmentItemType.ShortAnswer,sourceP);
        }
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
            if(spec.LessonCode=="L05")
            {
                if(mode==1){a=37;b=7;}
                if(mode==2){a=47;b=8;}
                if(mode==3){a=26;b=8;}
            }
            if(spec.LessonCode=="L06")
            {
                if(mode==2){a=34;b=9;}
                if(mode==3){a=68;b=6;}
            }
            if(spec.LessonCode=="L07"&&mode>=2)
            {
                // Exact 2021 Student Facing image-alt source quantities:
                // 32 + □ one-digit with new ten, 16 + □ without,
                // 8 + □ two-digit with/without new ten.
                if(mode==2){a=32;b=random.Next(8,10);}
                else if(mode==3){a=16;b=random.Next(0,4);}
                else if(mode==4)
                {
                    a=8;
                    do{b=random.Next(10,92);}while(b%10<2);
                }
                else
                {
                    a=8;
                    do{b=random.Next(10,92);}while(b%10>1);
                }
            }
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
                    bool yes=mode==2||mode==4;
                    string condition=yes?"WOULD make a new ten":"would NOT make a new ten";
                    string size=mode<=3?"ONE-DIGIT (0 to 9)":"TWO-DIGIT (10 to 91)";
                    string promptMissing=
                        $"ORIGINAL 2021 STUDENT FACING — Lin's water-smudged number: {a}+□. " +
                        $"What {size} number COULD she have written that {condition}? " +
                        "Choose ANY mathematically valid missing value, show the actual sum, " +
                        "and write MISSING=number;NEW_TEN=YES/NO;TOTAL=number. " +
                        "Teacher reviews personal drawings or equations and explains why several answers work.";
                    return new(family,promptMissing,
                        "Source allows several mathematically valid additions: the verifier " +
                        "accepts any correct one-digit choice meeting the stated make-ten " +
                        "constraint, not only the generator's sample answer. Teacher-led " +
                        "written and two-digit extensions remain independent classroom work.",
                        AssessmentItemType.ShortAnswer,candidate);
                }
                string prompt=spec.Kind switch
                {
                    Kind.MakeNewTen=>spec.LessonCode=="L05"&&mode==2?
                        "ORIGINAL 2021 STUDENT: Find 8+47. Make the next ten using 47+3+5=55. Show the source drawing or equation to a teacher. Write MOVE=number;REST=number;TOTAL=number.":
                    spec.LessonCode=="L05"&&mode==1?
                        "ORIGINAL 2021 FIELD TRIP STORY: 37 dollars plus 7 more; how much money did Tyler's teacher collect? Write MOVE=number;REST=number;TOTAL=number.":
                    spec.LessonCode=="L05"&&mode==3?
                        "ORIGINAL 2021 CLASSROOM STORY: 8 students sit at tables and 26 sit on the rug. How many students? Write MOVE=number;REST=number;TOTAL=number.":
                    spec.LessonCode=="L06"&&mode==2?
                        "ORIGINAL 2021 STUDENT: Elena and Andre both find 34+9. Elena starts with 34+6=40 and then adds 3. Andre starts with 9+1=10 and then adds 33. Complete BOTH true methods. Write MOVE=number;REST=number;TOTAL=number;ELENA=expression;ANDRE=expression. Teacher reviews personal drawings/explanations.":
                    spec.LessonCode=="L06"&&mode==3?
                        "ORIGINAL 2021 STUDENT: Find 6+68 by making ten. Show the equation and explain to your teacher. Write MOVE=number;REST=number;TOTAL=number.":
                    $"{activity} {operation} To form the next ten, split the smaller addend. Write MOVE=number;REST=number;TOTAL=number.",
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
        if(spec.LessonCode=="L02"&&p["mode"]==6)
            return "ELENA_TENS=80;ELENA_ONES=8;ANDRE_FIRST=83;ANDRE_SECOND=88;TOTAL=88";
        if(spec.LessonCode=="L03"&&p["mode"]==10)
            return "LIN_TENS=80;LIN_ONES=6;HAN_FIRST=84;HAN_SECOND=86;TOTAL=86";
        if(p["mode"]>=4&&(spec.LessonCode is "L02" or "L03"))
            return $"TENS={(a/10+b/10)*10};ONES={a%10+b%10};TOTAL={sum}";
        if(p["mode"]>=4&&(spec.LessonCode is "L08" or "L09"))
            return $"NEW_TEN={(a%10+b%10>=10?"YES":"NO")};TOTAL={sum}";
        if(spec.LessonCode=="L06"&&p["mode"]==2)
            return "MOVE=6;REST=3;TOTAL=43;ELENA=34+6+3=43;ANDRE=9+1+33=43";
        if(spec.LessonCode=="L07"&&p["mode"]>=2)
            return $"MISSING={b};NEW_TEN={(a%10+b%10>=10?"YES":"NO")};TOTAL={sum}";
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
           !int.TryParse(digitText,out var digit)||
           !int.TryParse(totalText,out var total)||total!=p["a"]+digit||total>99)
            return false;
        int mode=p["mode"];
        bool oneDigit=mode==2||mode==3;
        if(oneDigit?(digit<0||digit>9):(digit<10||digit>91))return false;
        bool makesNewTen=p["a"]%10+digit%10>=10;
        return makesNewTen==(mode==2||mode==4)&&
               tenText==(makesNewTen?"YES":"NO");
    }

    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? submitted)=>
        Valid(family,p)&&
        (family==Specs[6].Code&&p["mode"]>=2?
            VerifyOpenMissingAddend(p,submitted):
            GradePersistedAnswer(family,Solve(family!,p),submitted));
}
