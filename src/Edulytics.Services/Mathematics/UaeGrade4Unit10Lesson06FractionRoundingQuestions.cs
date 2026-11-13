using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10Lesson06FractionRoundingQuestions
{
    internal const string NearestBenchmark="uae.g4.u10.l06.nearest_benchmark";
    internal const string ComparedToHalf="uae.g4.u10.l06.compared_to_half";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is NearestBenchmark or ComparedToHalf or G5MixedNumberWhole || G5EstimateFamily(family);

    // Reuse original G4 benchmark rounding for G5 proper fractions;
    // extend only mixed number rounding to the nearest integer.
    internal const string G5MixedNumberWhole="uae.g5.u08.l01.round_mixed_number_to_whole";
    internal const string G5EstimateMixedSum="uae.g5.u08.l08.estimate_mixed_sum";
    internal const string G5ReasonableSum="uae.g5.u08.l08.check_reasonable_sum";
    private static bool G5EstimateFamily(string? f)=>f is G5EstimateMixedSum or G5ReasonableSum;
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


    // Reuse G4 benchmark rounding; extend to estimate sums of two mixed numbers.
    private static Item BuildG5Estimation(string f,Random rng)
    {
        int w1=rng.Next(1,11),w2=rng.Next(1,9);
        int d1=rng.Next(3,13),d2=rng.Next(3,13);
        int n1=rng.Next(1,d1),n2=rng.Next(1,d2);
        int expected=w1+(2*n1>=d1?1:0)+w2+(2*n2>=d2?1:0);
        int proposed=expected+(rng.Next(2)==0?0:2);
        var p=new Dictionary<string,int>{{"w1",w1},{"n1",n1},{"d1",d1},
            {"w2",w2},{"n2",n2},{"d2",d2},{"proposed",proposed}};
        string prompt=f==G5EstimateMixedSum
            ?$"قرّب كل عدد كسري إلى أقرب عدد كلي ثم قدّر مجموع {w1} و{n1}/{d1} و{w2} و{n2}/{d2}. ما الناتج التقريبي؟"
            :$"بعد تقريب {w1} و{n1}/{d1} مع {w2} و{n2}/{d2} إلى أقرب عدد كلي لكل عدد، هل التقدير {proposed} منطقي؟ اكتب ١ لنعم أو ٠ للا.";
        return new Item(f,prompt,"أقارن كل جزء كسري بالنصف، ثم أجمع التقريبين وأتحقق من معقولية التقدير.",
            AssessmentItemType.Numeric,p);
    }
    private static bool ValidG5Estimation(string f,IReadOnlyDictionary<string,int> p,
        out int w1,out int n1,out int d1,out int w2,out int n2,out int d2,out int proposed)
    {
        w1=n1=d1=w2=n2=d2=proposed=0;
        return G5EstimateFamily(f)&&p.Count==7&&
          p.TryGetValue("w1",out w1)&&p.TryGetValue("n1",out n1)&&
          p.TryGetValue("d1",out d1)&&p.TryGetValue("w2",out w2)&&
          p.TryGetValue("n2",out n2)&&p.TryGetValue("d2",out d2)&&
          p.TryGetValue("proposed",out proposed)&&
          w1 is >=1 and <=10 && w2 is >=1 and <=8 &&
          d1 is >=3 and <=12 && d2 is >=3 and <=12 &&
          n1>=1&&n1<d1&&n2>=1&&n2<d2 &&
          proposed is >=2 and <=22;
    }
    private static string SolveG5Estimation(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Estimation(f,p,out int w1,out int n1,out int d1,
            out int w2,out int n2,out int d2,out int prop))
            throw new InvalidOperationException("Invalid mixed-number estimate parameters.");
        int approximate=w1+(2*n1>=d1?1:0)+w2+(2*n2>=d2?1:0);
        int answer=f==G5EstimateMixedSum?approximate:(prop==approximate?1:0);
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5Estimation(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(!ValidG5Estimation(f,p,out int w1,out int n1,out int d1,
            out int w2,out int n2,out int d2,out int prop)||string.IsNullOrWhiteSpace(response)||
            response.Length>30)return false;
        string normalized=new(response.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
          .Select(ch=>ch switch{>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
              >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),_=>ch}).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual))return false;
        // Independent benchmark: compare each numerator to the missing piece of its denominator.
        int first=w1+(n1>=d1-n1?1:0),second=w2+(n2>=d2-n2?1:0);
        int want=f==G5EstimateMixedSum?first+second:(prop==first+second?1:0);
        return actual==want;
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported benchmark rounding family");
        if(family==G5MixedNumberWhole)return BuildG5MixedNumberWhole(random);
        if(G5EstimateFamily(family))return BuildG5Estimation(family,random);
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
        if(G5EstimateFamily(f))return SolveG5Estimation(f,p);
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid benchmark rounding parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(f==G5MixedNumberWhole)return VerifyG5MixedNumberWhole(p,answer);
        if(G5EstimateFamily(f))return VerifyG5Estimation(f,p,answer);
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
