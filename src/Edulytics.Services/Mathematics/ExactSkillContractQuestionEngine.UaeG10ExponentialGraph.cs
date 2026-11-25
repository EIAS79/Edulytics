using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade10 General G10-U05L01 from owner Applied 2022–2023 Volume2,
/// source printed pages294–300. Exponential growth/decay graph features.
/// Bounded integer exact-probes only; fractional table and real-world plotting
/// remain original authored material and teacher-assessed in Stage B.
/// </summary>
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10ExponentialGraph(
        string family,Random random,ExactSkillQuestionDifficulty difficulty)
    {
        int b=random.Next(2,6),coeff=random.Next(1,5);
        int direction=random.Next(0,2); // 1 growth, 0 decay
        int h=random.Next(-2,3),k=random.Next(-4,5);
        int step=random.Next(0,3);
        // A small exact answer subset: decay a=coefficient*b^2 so
        // a*b^-n remains an integer for n<=2; growth a=coefficient.
        int a=direction==1?coeff:coeff*b*b;
        int probe=h+step;
        string factor=direction==1?$"{b}":"(1/"+b.ToString(CultureInfo.InvariantCulture)+")";
        string func=$"د(س)={a}×{factor}^(س−({h}))+({k})";
        string prompt=family switch
        {
            UaeG10ExponentialGraphResponse.Value=>
                $"للدالة الأسية {func}، أوجد د({probe}) واكتب القيمة الصحيحة بعد التعويض.",
            UaeG10ExponentialGraphResponse.Growth=>
                $"للدالة الأسية {func}، هل هي دالة نمو أسي عندما المعامل الموجب ثابت؟ أجب ١ للنمو أو ٠ للاضمحلال.",
            UaeG10ExponentialGraphResponse.Asymptote=>
                $"للدالة الأسية {func}، ما قيمة ص التي يقترب منها التمثيل البياني أفقيًا دون بلوغها؟",
            _=>$"للدالة الأسية {func}، أوجد د({h}) عند الإزاحة الأفقية هـ={h}، وهي قيمة بداية المضاعف قبل انتقال الخط البياني."
        };
        return Problem(family,prompt,
            "للصيغة د(س)=أ×ب^(س−هـ)+ك: حدد النمو من ب>١ أو الاضمحلال من ٠<ب<١، والمقارب الأفقي ص=ك، والتعويض يُحسب بأسس صحيحة وحدها مع مراعاة الإزاحة.",
            AssessmentItemType.Numeric,("base",b),("coeff",coeff),
            ("direction",direction),("h",h),("k",k),("step",step));
    }
}
internal static class UaeG10ExponentialGraphResponse
{
    internal const string Value="uae.g10.general.u05.l01.value_at_x";
    internal const string Growth="uae.g10.general.u05.l01.is_growth";
    internal const string Asymptote="uae.g10.general.u05.l01.horizontal_asymptote";
    internal const string ShiftAnchor="uae.g10.general.u05.l01.value_at_shift";
    internal static bool Supports(string? f) =>
        f is Value or Growth or Asymptote or ShiftAnchor;

    private static bool Expected(string family,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(family)||p.Count!=6||
            !p.ContainsKey("base")||!p.ContainsKey("coeff")||
            !p.ContainsKey("direction")||!p.ContainsKey("h")||
            !p.ContainsKey("k")||!p.ContainsKey("step"))return false;
        int b=p["base"],c=p["coeff"],dir=p["direction"],h=p["h"],k=p["k"],n=p["step"];
        if(b is < 2 or > 5||c is < 1 or > 4||dir is not (0 or 1)||
           h is < -2 or > 2||k is < -4 or > 4||n is < 0 or > 2)return false;
        int a=dir==1?c:c*b*b;
        int y=dir==1?a*IntPow(b,n)+k:a/IntPow(b,n)+k;
        value=family switch
        {
            Value=>y,
            Growth=>dir,
            Asymptote=>k,
            _=>a+k
        };
        return true;
    }
    private static int IntPow(int b,int n)
    {
        int r=1;for(int i=0;i<n;i++)r*=b;return r;
    }
    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Expected(family,p,out int value))
            throw new InvalidOperationException("Invalid bounded G10 exponential graph parameters.");
        return value.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Expected(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string raw=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-',_=>ch
        }));
        return int.TryParse(raw,NumberStyles.AllowLeadingSign,
            CultureInfo.InvariantCulture,out int result)&&result==expected;
    }
}
