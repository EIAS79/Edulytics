using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Model-based subtraction equation writing and inverse addition.
/// Student diagrams and original stories require teacher review.</summary>
internal static class UaeG6U7L04SubtractionLabQuestions
{
    internal const string Write="uae.g6.u07.l04.write_subtraction_model";
    internal const string Inverse="uae.g6.u07.l04.inverse_addition_statement";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Write or Inverse;
    private static string Norm(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
         >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
         >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
         '−' or '–'=>'-',_=>ch}).ToArray());
    private static bool Expected(string f,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(f)||p is null||p.Count!=2||
           !p.TryGetValue("removed",out int removed)||!p.TryGetValue("remaining",out int remain)||
           removed is <1 or >50||remain is <1 or >100)return false;
        answer=f==Write?$"س-{removed}={remain}":$"س={remain}+{removed}";
        return true;
    }
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported subtraction model");
        ArgumentNullException.ThrowIfNull(random);
        int removed=random.Next(1,51),remaining=random.Next(1,101);
        Dictionary<string,int> p=new(){{"removed",removed},{"remaining",remaining}};
        string prompt=f==Write?
            $"كان في البداية س من العناصر، أُزيل {removed} منها فتبقى {remaining}. اكتب معادلة الطرح باستخدام س.":
            $"حوّل المعادلة س−{removed}={remaining} إلى معادلة جمع مكافئة بكتابة س=المتبقي+المزال.";
        return new Item(f,prompt,"أحدد الكل والأجزاء في الرسم الشريطي وأحوّل الفرق إلى جمع للتحقق.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        Expected(f,p,out var answer)?answer:throw new InvalidOperationException("Invalid subtraction model");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>90||
           !Expected(f,p,out var expected))return false;
        string actual=Norm(response);
        if(actual==expected)return true;
        if(f==Inverse)
        {
            int removed=p["removed"],remain=p["remaining"];
            return actual==$"س={removed}+{remain}"||actual==$"{remain}+{removed}=س"||
                 actual==$"س={remain+removed}";
        }
        return false;
    }
}
