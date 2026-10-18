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
        int ones=a%10+b%10,sum=a+b;
        return spec.Kind switch
        {
            Kind.TensOrOnes=>a>=20&&a<=89&&sum<=99&&
                ((b>=1&&b<=9&&ones<10)||
                 (b>=10&&b<=30&&b%10==0)),
            Kind.NoComposeTwoDigits=>a>=10&&a<=89&&b>=10&&b<=89&&sum<=99&&ones<10,
            Kind.MakeNewTen=>a>=20&&a<=89&&b>=1&&b<=9&&sum<=99&&ones>=10,
            Kind.ComposingTwoDigits=>a>=10&&a<=89&&b>=10&&b<=89&&sum<=100&&ones>=10,
            Kind.Flexible=>a>=20&&a<=89&&b>=1&&b<=(spec.LessonCode=="L07"?9:10)&&sum<=99,
            _=>false
        };
    }
    public static Problem Build(string family,Random random)
    {
        var spec=Get(family)??throw new InvalidOperationException("Unreviewed G1 Unit 5 Practice family");
        int a=0,b=0,mode=random.Next(4);
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
                Kind.Flexible=>random.Next(1,spec.LessonCode=="L07"?10:11),
                _=>random.Next(1,10)
            };
            var candidate=new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode}};
            if(Valid(family,candidate))
            {
                var sum=a+b,composes=(a%10+b%10)>=10;
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
    public static bool Verify(string? family,IReadOnlyDictionary<string,int> p,string? submitted)=>
        Valid(family,p)&&GradePersistedAnswer(family,Solve(family!,p),submitted);
}
