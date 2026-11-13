using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10Lesson06FractionRoundingQuestions
{
    internal const string NearestBenchmark="uae.g4.u10.l06.nearest_benchmark";
    internal const string ComparedToHalf="uae.g4.u10.l06.compared_to_half";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is NearestBenchmark or ComparedToHalf or G5MixedNumberWhole;

    // Reuse original G4 benchmark rounding for G5 proper fractions;
    // extend only mixed number rounding to the nearest integer.
    internal const string G5MixedNumberWhole="uae.g5.u08.l01.round_mixed_number_to_whole";
    private static Item BuildG5MixedNumberWhole(Random random)
    {
        int w=random.Next(1,13),d=random.Next(4,17),n=random.Next(1,d);
        return new Item(G5MixedNumberWhole,
            $"قرّب العدد الكسري {w} و{n}/{d} إلى أقرب عدد كلي، مع التقريب لأعلى عند المنتصف.",
            "قارِن الكسر بالنصف؛ إذا بلغ النصف أو تجاوزه أضف واحدًا إلى العدد الصحيح.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"whole",w},{"numerator",n},{"denominator",d}});
    }
    private static bool ValidG5MixedNumberWhole(IReadOnlyDictionary<string,int> p,
        out int w,out int n,out int d)
    {
        w=n=d=0;
        return p.Count==3&&p.TryGetValue("whole",out w)&&
            p.TryGetValue("numerator",out n)&&p.TryGetValue("denominator",out d)&&
            w is >=1 and <=12&&d is >=4 and <=16&&n>=1&&n<d;
    }
    private static string SolveG5MixedNumberWhole(IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5MixedNumberWhole(p,out int w,out int n,out int d))
            throw new InvalidOperationException("Invalid G5 mixed number rounding");
        return (w+(2*n>=d?1:0)).ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5MixedNumberWhole(IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5MixedNumberWhole(p,out int w,out int n,out int d)||
            string.IsNullOrWhiteSpace(answer)||answer.Length>20)return false;
        string s=new(answer.Trim().Where(c=>!char.IsWhiteSpace(c)&&c is not ',' and not '٬')
            .Select(c=>c switch{
                >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c}).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int v)&&
            v==(n<d-n?w:w+1);
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported benchmark rounding family");
        if(family==G5MixedNumberWhole)return BuildG5MixedNumberWhole(random);
        int d=random.Next(4,17),n=random.Next(1,d);
        var p=new Dictionary<string,int>{{"numerator",n},{"denominator",d}};
        string prompt=family==NearestBenchmark?
            $"قرب الكسر {n}/{d} إلى المرجع الأقرب: اكتب ٠ للصفر، و١ للنصف، و٢ للواحد. عند التعادل اختر المرجع الأكبر.":
            $"قارن الكسر {n}/{d} بالنصف: اكتب ٠ لأصغر من النصف، و١ للتساوي، و٢ لأكبر من النصف.";
        return new Item(family,prompt,"حدد موضع الكسر على خط الأعداد وقارن مسافته من الصفر والنصف والواحد.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("numerator",out int n)||
           !p.TryGetValue("denominator",out int d)||d is <4 or >16||n<1||n>=d)return false;
        if(f==ComparedToHalf)
            result=2*n<d?0:2*n==d?1:2;
        else
            result=4*n<d?0:4*n<3*d?1:2;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(f==G5MixedNumberWhole)return SolveG5MixedNumberWhole(p);
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid benchmark rounding parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(f==G5MixedNumberWhole)return VerifyG5MixedNumberWhole(p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
