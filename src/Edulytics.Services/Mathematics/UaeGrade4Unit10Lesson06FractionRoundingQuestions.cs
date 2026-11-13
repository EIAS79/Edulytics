using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10Lesson06FractionRoundingQuestions
{
    internal const string NearestBenchmark="uae.g4.u10.l06.nearest_benchmark";
    internal const string ComparedToHalf="uae.g4.u10.l06.compared_to_half";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is NearestBenchmark or ComparedToHalf or G5MixedNumberWhole || G5EstimateFamily(family)||G5ProductEstimate(family);

    // Reuse original G4 benchmark rounding for G5 proper fractions;
    // extend only mixed number rounding to the nearest integer.
    internal const string G5MixedNumberWhole="uae.g5.u08.l01.round_mixed_number_to_whole";
    internal const string G5EstimateMixedSum="uae.g5.u08.l08.estimate_mixed_sum";
    internal const string G5ReasonableSum="uae.g5.u08.l08.check_reasonable_sum";
    internal const string G5EstimateMixedDifference="uae.g5.u08.l09.estimate_mixed_difference";
    internal const string G5EstimateFractionProduct="uae.g5.u09.l02.estimate_fraction_product_quarters";
    internal const string G5EstimateFractionWhole="uae.g5.u09.l02.estimate_whole_fraction_compatible";
    private static bool G5ProductEstimate(string? f)=>f is G5EstimateFractionProduct or G5EstimateFractionWhole;
    private static bool G5EstimateFamily(string? f)=>f is G5EstimateMixedSum or G5ReasonableSum or G5EstimateMixedDifference;
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
        int w1=f==G5EstimateMixedDifference?rng.Next(5,13):rng.Next(1,11);
        int w2=f==G5EstimateMixedDifference?rng.Next(1,Math.Min(9,w1-1)):rng.Next(1,9);
        int d1=rng.Next(3,13),d2=rng.Next(3,13);
        int n1=rng.Next(1,d1),n2=rng.Next(1,d2);
        int first=w1+(2*n1>=d1?1:0),second=w2+(2*n2>=d2?1:0);
        int expected=f==G5EstimateMixedDifference?first-second:first+second;
        int proposed=expected+(rng.Next(2)==0?0:2);
        var p=new Dictionary<string,int>{{"w1",w1},{"n1",n1},{"d1",d1},
            {"w2",w2},{"n2",n2},{"d2",d2},{"proposed",proposed}};
        string prompt=f==G5EstimateMixedDifference
            ?$"قرّب كل عدد كسري ثم قدّر الفرق بين {w1} و{n1}/{d1} و{w2} و{n2}/{d2} إلى أقرب عدد كلي."
            :f==G5EstimateMixedSum
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
          w1 is >=1 and <=12 && w2 is >=1 and <=8 &&
          d1 is >=3 and <=12 && d2 is >=3 and <=12 &&
          n1>=1&&n1<d1&&n2>=1&&n2<d2 &&
          proposed is >=1 and <=22;
    }
    private static string SolveG5Estimation(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Estimation(f,p,out int w1,out int n1,out int d1,
            out int w2,out int n2,out int d2,out int prop))
            throw new InvalidOperationException("Invalid mixed-number estimate parameters.");
        int first=w1+(2*n1>=d1?1:0),second=w2+(2*n2>=d2?1:0);
        int approximate=f==G5EstimateMixedDifference?first-second:first+second;
        int answer=f is G5EstimateMixedSum or G5EstimateMixedDifference ?approximate:(prop==approximate?1:0);
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
        int want=f==G5EstimateMixedDifference?first-second:
                 f==G5EstimateMixedSum?first+second:(prop==first+second?1:0);
        return actual==want;
    }


    // Grade5 estimation reuses the Grade4 nearest-benchmark mechanism.
    // Outputs are machine-checkable number of quarters or compatible units.
    private static int HalfUnitBenchmark(int n,int d)=>4*n<d?0:4*n<3*d?1:2;
    private static Item BuildG5ProductEstimate(string family,Random random)
    {
        if(family==G5EstimateFractionWhole)
        {
            int whole=random.Next(11,40),d=random.Next(2,9),n=random.Next(1,d);
            int compatible=((whole+d/2)/d)*d;
            return new Item(family,$"للتقدير، قرّب {whole} إلى عدد متوافق مع المقام {d}. ما تقدير {n}/{d} من {whole}؟ اكتب القيمة الكلية التقديرية.",
              $"العدد المتوافق هو {compatible}، وأخذ {n}/{d} منه يعني قسمة ثم ضرب.",
              AssessmentItemType.Numeric,new Dictionary<string,int>{{"whole",whole},{"denominator",d},{"numerator",n}});
        }
        int d1=random.Next(3,13),d2=random.Next(3,13);
        int n1=random.Next(1,d1),n2=random.Next(1,d2);
        return new Item(family,
          $"قرّب {n1}/{d1} و{n2}/{d2} كلاً منهما إلى ٠ أو ١/٢ أو ١، ثم قدّر حاصل الضرب. كم ربعًا في الناتج التقريبي؟ اكتب ٠ أو ١ أو ٢ أو ٤.",
          "أقارن كل كسر بالصفر والنصف والواحد وأضرب الكسرين المرجعيين.",
          AssessmentItemType.Numeric,new Dictionary<string,int>{{"n1",n1},{"d1",d1},{"n2",n2},{"d2",d2}});
    }
    private static bool G5ProductValid(string family,IReadOnlyDictionary<string,int> p,
        out int estimate)
    {
        estimate=0;
        if(family==G5EstimateFractionWhole)
        {
            if(p.Count!=3||!p.TryGetValue("whole",out int whole)||
               !p.TryGetValue("denominator",out int d)||
               !p.TryGetValue("numerator",out int n)||
               whole is <11 or >39 || d is <2 or >8 || n<1||n>=d)return false;
            int compatible=((whole+d/2)/d)*d;
            estimate=compatible/d*n;return true;
        }
        if(family!=G5EstimateFractionProduct||p.Count!=4||
           !p.TryGetValue("n1",out int n1)||!p.TryGetValue("d1",out int d1)||
           !p.TryGetValue("n2",out int n2)||!p.TryGetValue("d2",out int d2)||
           d1 is <3 or >12 || d2 is <3 or >12 ||
           n1<1||n1>=d1||n2<1||n2>=d2)return false;
        estimate=HalfUnitBenchmark(n1,d1)*HalfUnitBenchmark(n2,d2);
        return true;
    }
    private static string SolveG5ProductEstimate(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!G5ProductValid(f,p,out int estimate))
            throw new InvalidOperationException("Invalid G5 fractional product estimate.");
        return estimate.ToString(CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5ProductEstimate(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!G5ProductValid(f,p,out int _)||string.IsNullOrWhiteSpace(answer)||
           answer.Length>24)return false;
        string normalized=new(answer.Trim().Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬')
            .Select(ch=>ch switch {>= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
               >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),_=>ch}).ToArray());
        if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int supplied))return false;
        if(f==G5EstimateFractionWhole)
        {
            int d=p["denominator"],whole=p["whole"];
            int left=whole%d,right=d-left;
            int near=whole-left+(left>=right?d:0);
            return supplied==near/d*p["numerator"];
        }
        int n1=p["n1"],d1=p["d1"],n2=p["n2"],d2=p["d2"];
        int first=n1*4<d1?0:n1*4<3*d1?1:2;
        int second=n2*4<d2?0:n2*4<3*d2?1:2;
        return supplied==first*second;
    }

    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported benchmark rounding family");
        if(family==G5MixedNumberWhole)return BuildG5MixedNumberWhole(random);
        if(G5EstimateFamily(family))return BuildG5Estimation(family,random);
        if(G5ProductEstimate(family))return BuildG5ProductEstimate(family,random);
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
        if(G5ProductEstimate(f))return SolveG5ProductEstimate(f,p);
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid benchmark rounding parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(f==G5MixedNumberWhole)return VerifyG5MixedNumberWhole(p,answer);
        if(G5EstimateFamily(f))return VerifyG5Estimation(f,p,answer);
        if(G5ProductEstimate(f))return VerifyG5ProductEstimate(f,p,answer);
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
