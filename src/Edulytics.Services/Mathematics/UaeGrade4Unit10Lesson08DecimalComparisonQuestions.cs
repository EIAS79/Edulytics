using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10Lesson08DecimalComparisonQuestions
{
    internal const string Compare="uae.g4.u10.l08.decimal_compare";
    internal const string Larger="uae.g4.u10.l08.larger_hundredths";
    internal const string G5Compare="uae.g5.u01.l07.compare_decimal_to_thousandths";
    internal const string G5Greater="uae.g5.u01.l07.select_greater_decimal";
    internal const string G5OrderAsc="uae.g5.u01.l08.order_decimals_ascending";
    internal const string G5OrderDesc="uae.g5.u01.l08.order_decimals_descending";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Compare or Larger or G5Compare or G5Greater or G5OrderAsc or G5OrderDesc;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported decimal comparison");
        if (family is G5Compare or G5Greater) return BuildG5(family,random);
        if (family is G5OrderAsc or G5OrderDesc) return BuildG5Ordering(family,random);
        int a=random.Next(0,100),b=random.Next(0,100);
        if(a==b)b=(b+1)%100;
        var p=new Dictionary<string,int>{{"firstHundredths",a},{"secondHundredths",b}};
        string x=(a/100m).ToString("0.00",CultureInfo.InvariantCulture);
        string y=(b/100m).ToString("0.00",CultureInfo.InvariantCulture);
        string prompt=family==Compare?
            $"قارن {x} و{y}: اكتب ٠ إذا الأول أصغر، و١ إذا تساويا، و٢ إذا الأول أكبر.":
            $"أي العددين أكبر: {x} أو {y}؟ اكتب عدد أجزاء المئة في العدد الأكبر.";
        return new Item(family,prompt,"قارن الأعشار ثم أجزاء المئة، أو استخدم نموذج مئة مربع.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(family)||p.Count!=2||
           !p.TryGetValue("firstHundredths",out int a)||!p.TryGetValue("secondHundredths",out int b)||
           a is <0 or >99||b is <0 or >99)return false;
        result=family==Compare?(a<b?0:a==b?1:2):Math.Max(a,b);
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(family is G5Compare or G5Greater) return SolveG5(family,p);
        if(family is G5OrderAsc or G5OrderDesc) return SolveG5Ordering(family,p);
        if(!Valid(family,p,out int answer))throw new InvalidOperationException("Invalid decimal comparison");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(family is G5Compare or G5Greater) return VerifyG5(family,p,answer);
        if(family is G5OrderAsc or G5OrderDesc) return VerifyG5Ordering(family,p,answer);
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
    // Extend the reviewed Grade 4 comparison implementation without changing G4 families.
    // Every decimal is represented by its exact integer number of thousandths.
    private static Item BuildG5(string family,Random random)
    {
        int a=random.Next(0,300_001),b=random.Next(0,300_001);
        if(family==G5Compare&&random.Next(7)==0)b=a;
        if(family==G5Greater&&a==b)b=(a==300_000?299_999:a+1);
        var p=new Dictionary<string,int>
        { ["firstThousandths"]=a,["secondThousandths"]=b };
        string first=(a/1000m).ToString("0.000",CultureInfo.InvariantCulture);
        string second=(b/1000m).ToString("0.000",CultureInfo.InvariantCulture);
        string question=family==G5Compare
            ? $"ضع > أو < أو = بين العددين: {first} □ {second}."
            : $"ما العدد الأكبر: {first} أم {second}؟ اكتب العدد العشري.";
        return new Item(family,question,
            "ساوِ عدد الخانات العشرية بالأصفار، ثم قارن المنازل من اليسار.",
            family==G5Compare?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,p);
    }

    private static bool ValidG5(string family,IReadOnlyDictionary<string,int> p,
        out int a,out int b)
    {
        a=b=0;
        return family is G5Compare or G5Greater&&p.Count==2&&
            p.TryGetValue("firstThousandths",out a)&&
            p.TryGetValue("secondThousandths",out b)&&
            a is >=0 and <=300_000&&b is >=0 and <=300_000&&
            (family==G5Compare||a!=b);
    }

    private static string SolveG5(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5(family,p,out int a,out int b))
            throw new InvalidOperationException("Invalid G5 decimal comparison.");
        return family==G5Compare
            ? a>b?">":a<b?"<":"="
            : (Math.Max(a,b)/1000m).ToString("0.000",CultureInfo.InvariantCulture);
    }

    private static bool VerifyG5(string family,
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5(family,p,out int a,out int b)||string.IsNullOrWhiteSpace(answer))
            return false;
        if(family==G5Compare)return answer.Trim()==(a<b?"<":a>b?">":"=");
        string normalized=new(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',
            _=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00A0').ToArray());
        if(normalized.Length>20||normalized.Count(x=>x=='.')>1||
           !normalized.All(x=>x is >= '0' and <= '9' or '.'))return false;
        return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal result)&&
            result*1000m==(a>b?a:b);
    }
    // Four-number ordering is an extension of the existing exact decimal
    // comparison engine; old Grade 4 questions are unchanged.
    private static Item BuildG5Ordering(string family,Random random)
    {
        var chosen=new HashSet<int>();
        while(chosen.Count<4)chosen.Add(random.Next(0,300_001));
        int[] a=chosen.ToArray();
        var p=new Dictionary<string,int>
        { ["a"]=a[0],["b"]=a[1],["c"]=a[2],["d"]=a[3] };
        string direction=family==G5OrderAsc?"الأصغر إلى الأكبر":"الأكبر إلى الأصغر";
        string text=string.Join("، ",a.Select(v=>(v/1000m).ToString("0.000",CultureInfo.InvariantCulture)));
        return new Item(family,
            $"رتب الأعداد العشرية الآتية من {direction}؛ افصل الأعداد بفاصلة منقوطة: {text}.",
            "حاذِ المنازل حتى أجزاء الألف، ثم رتب القيم.",
            AssessmentItemType.ShortAnswer,p);
    }
    private static bool ValidG5Ordering(string family,
        IReadOnlyDictionary<string,int> p,out int[] values)
    {
        values=[];
        if(family is not (G5OrderAsc or G5OrderDesc)||p.Count!=4||
           !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("c",out int c)||!p.TryGetValue("d",out int d))return false;
        int[] arr=[a,b,c,d];
        if(arr.Any(x=>x is <0 or >300_000)||arr.Distinct().Count()!=4)return false;
        values=arr;
        return true;
    }
    private static string SolveG5Ordering(string family,
        IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5Ordering(family,p,out int[] values))
            throw new InvalidOperationException("Invalid G5 decimal order parameters.");
        Array.Sort(values);
        if(family==G5OrderDesc)Array.Reverse(values);
        return string.Join(";",values.Select(v=>(v/1000m).ToString("0.000",CultureInfo.InvariantCulture)));
    }
    private static bool VerifyG5Ordering(string family,
        IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5Ordering(family,p,out int[] originals)||string.IsNullOrWhiteSpace(answer))
            return false;
        var parts=answer.Split(';',StringSplitOptions.TrimEntries);
        if(parts.Length!=4)return false;
        var values=new int[4];
        for(int i=0;i<parts.Length;i++)
        {
            string normalized=new(parts[i].Select(ch=>ch switch
            {
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '٫'=>'.',
                _=>ch
            }).Where(ch=>ch is not ' ' and not '٬' and not '\u00A0').ToArray());
            if(normalized.Length>20||normalized.Count(ch=>ch=='.')>1||
               !normalized.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
            if(!decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,out decimal number))return false;
            decimal scaled=number*1000m;
            if(scaled!=decimal.Truncate(scaled)||scaled<0||scaled>300_000)return false;
            values[i]=(int)scaled;
        }
        var independent=family==G5OrderAsc
            ? originals.OrderBy(v=>v).ToArray()
            : originals.OrderByDescending(v=>v).ToArray();
        return values.SequenceEqual(independent);
    }
}
