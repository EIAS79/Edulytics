using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson10ImproperFractionsQuestions
{
    internal const string Whole="uae.g4.u08.l10.improper_whole";
    internal const string Remainder="uae.g4.u08.l10.improper_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Whole or Remainder || IsG5DivisionFraction(f);

 // UAE Grade 5 U07 L01: division of whole quantities as fractional shares.
 // Reuse G4 improper/mixed fraction solver and verifier infrastructure.
 internal const string G5AsFraction="uae.g5.u07.l01.division_as_fraction";
 internal const string G5WholePart="uae.g5.u07.l01.quotient_whole_part";
 internal const string G5Remainder="uae.g5.u07.l01.quotient_remainder";
 private static bool IsG5DivisionFraction(string? f)=>f is G5AsFraction or G5WholePart or G5Remainder;
 private static Item BuildG5DivisionFraction(string f,Random random)
 {
    int d=random.Next(2,11),n=random.Next(1,d*3+1);
    string prompt=f switch
    {
      G5AsFraction=>$"تقاسم {d} طلاب {n} وحدات بالتساوي. اكتب نصيب الطالب الواحد على صورة كسر (بسط/مقام).",
      G5WholePart=>$"تقاسم {d} طلاب {n} وحدات بالتساوي. كم وحدة كاملة يحصل عليها الطالب الواحد؟",
      _=>$"عند قسمة {n} على {d}، ما الباقي بعد توزيع الوحدات الكاملة بالتساوي؟"
    };
    return new Item(f,prompt,
        "نمثل عدد الوحدات بالبسط وعدد المجموعات بالمقام؛ خارج القسمة يعطي الكل والباقي يبقى جزءًا كسريًا.",
        f==G5AsFraction?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
        new Dictionary<string,int>{{"n",n},{"d",d}});
 }
 private static bool ValidG5DivisionFraction(string? f,IReadOnlyDictionary<string,int> p,
    out int n,out int d)
 {
    n=d=0;
    return IsG5DivisionFraction(f)&&p.Count==2&&
      p.TryGetValue("n",out n)&&p.TryGetValue("d",out d)&&
      d is >=2 and <=10 && n>0&&n<=3*d;
 }
 private static string SolveG5DivisionFraction(string f,IReadOnlyDictionary<string,int> p)
 {
    if(!ValidG5DivisionFraction(f,p,out int n,out int d))
       throw new InvalidOperationException("Invalid G5 division/fraction parameters");
    return f==G5AsFraction?$"{n}/{d}":
           (f==G5WholePart?n/d:n%d).ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5DivisionFraction(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
    if(!ValidG5DivisionFraction(f,p,out int n,out int d)||
       string.IsNullOrWhiteSpace(answer)||answer.Length>32)return false;
    string normalized=new(answer.Trim()
       .Select(ch=>ch switch
       {
         >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
         >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
         '⁄'=>'/',
         _=>ch
       }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not '٬').ToArray());
    if(f==G5AsFraction)
    {
       string[] parts=normalized.Split('/');
       return parts.Length==2 &&
         int.TryParse(parts[0],NumberStyles.None,CultureInfo.InvariantCulture,out int a)&&
         int.TryParse(parts[1],NumberStyles.None,CultureInfo.InvariantCulture,out int b)&&
         b>0 && (long)a*d==(long)n*b;
    }
    return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&
       actual==(f==G5WholePart?n/d:n%d);
 }

    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported fraction family");
        if(IsG5DivisionFraction(f))return BuildG5DivisionFraction(f,random);
        int denominator=random.Next(2,11),whole=random.Next(1,6),remainder=random.Next(0,denominator);
        int numerator=whole*denominator+remainder;
        var parameters=new Dictionary<string,int>{{"improper",numerator},{"denominator",denominator}};
        string prompt=f==Whole?
            $"حوّل الكسر المعتل {numerator}/{denominator} إلى عدد كسري. ما عدد الوحدات الكاملة؟":
            $"حوّل الكسر المعتل {numerator}/{denominator} إلى عدد كسري. ما بسط الجزء الكسري المتبقي بعد القسمة؟";
        return new Item(f,prompt,"اقسم البسط على المقام؛ خارج القسمة وحدات كاملة والباقي بسط الكسر.",
            AssessmentItemType.Numeric,parameters);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("improper",out int n)||
           !p.TryGetValue("denominator",out int d)||d is <2 or >10||n<d||n>6*d)return false;
        answer=f==Whole?n/d:n%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5DivisionFraction(f))return SolveG5DivisionFraction(f,p);
        if(!Valid(f,p,out int a))throw new InvalidOperationException("Invalid improper fraction parameters");
        return a.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5DivisionFraction(f))return VerifyG5DivisionFraction(f,p,answer);
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
