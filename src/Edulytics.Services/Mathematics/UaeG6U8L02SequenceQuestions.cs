using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact bounded arithmetic and geometric sequence rules for UAE G6.
/// Explaining the observed pattern and nonnumeric representations is teacher assessed.</summary>
internal static class UaeG6U8L02SequenceQuestions
{
    internal const string Arithmetic="uae.g6.u08.l02.arithmetic_nth";
    internal const string Geometric="uae.g6.u08.l02.geometric_nth";
    internal const string Classify="uae.g6.u08.l02.classify";
    internal const string Decimal="uae.g6.u08.l02.decimal_arithmetic";
    internal const string Linear="uae.g6.u08.l02.linear_rule";
    internal sealed record Item(string Family,string Prompt,string Solution,
       AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Arithmetic or Geometric or Classify or Decimal or Linear;
    private static string Norm(string? s)=>new string((s??"").Where(c=>!char.IsWhiteSpace(c))
      .Select(c=>c switch{
        >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
        '٫'=>'.','−' or '–'=>'-',_=>c}).ToArray());
    private static bool Expected(string f,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(f)||p is null)return false;
        if(f==Classify)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >1)return false;
            result=mode==0?"حسابية":"هندسية";return true;
        }
        if(f==Geometric)
        {
            if(p.Count!=3||!p.TryGetValue("first",out int first)||
                !p.TryGetValue("ratio",out int ratio)||!p.TryGetValue("n",out int n)||
                first is <1 or >8||ratio is <2 or >3||n is <1 or >7)return false;
            int value=first;for(int i=1;i<n;i++)value=checked(value*ratio);
            result=value.ToString(CultureInfo.InvariantCulture);return true;
        }
        if(f==Decimal)
        {
            if(p.Count!=3||!p.TryGetValue("first10",out int a)||
               !p.TryGetValue("diff10",out int d)||!p.TryGetValue("n",out int n)||
               a is <-100 or >1000||d is <-100 or >100||d==0||n is <1 or >20)return false;
            result=((a+(n-1)*d)/10m).ToString("0.#",CultureInfo.InvariantCulture);return true;
        }
        if(p.Count!=3||!p.TryGetValue("first",out int firstVal)||
           !p.TryGetValue("delta",out int diff)||!p.TryGetValue("n",out int index)||
           firstVal is <-100 or >1000||diff is <-20 or >30||
           index is <1 or >100)return false;
        int answer=f==Arithmetic?firstVal+(index-1)*diff:diff*index+firstVal;
        result=answer.ToString(CultureInfo.InvariantCulture);return true;
    }
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported sequence");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string prompt;
        if(f==Classify)
        {
            int mode=random.Next(2);p=new(){{"mode",mode}};
            prompt=mode==0?"هل المتتالية ٧،١٤،٢١،٢٨ حسابية أم هندسية؟":
            "هل المتتالية ٢،٤،٨،١٦ حسابية أم هندسية؟";
        }
        else if(f==Geometric)
        {
            int first=random.Next(1,9),ratio=random.Next(2,4),n=random.Next(2,8);
            p=new(){{"first",first},{"ratio",ratio},{"n",n}};
            prompt=$"متتالية هندسية حدها الأول {first} ونسبتها {ratio}. أوجد الحد رقم {n}.";
        }
        else if(f==Decimal)
        {
            int first=random.Next(1,101),d=random.Next(1,31)*(random.Next(2)==0?1:-1),n=random.Next(2,21);
            p=new(){{"first10",first},{"diff10",d},{"n",n}};
            prompt=$"متتالية حسابية تبدأ بـ{(first/10m).ToString("0.#",CultureInfo.InvariantCulture)} ويضاف {(d/10m).ToString("0.#",CultureInfo.InvariantCulture)} كل مرة. ما الحد رقم {n}؟";
        }
        else
        {
            int first=random.Next(1,101),diff=random.Next(1,21),n=random.Next(2,51);
            p=new(){{"first",first},{"delta",diff},{"n",n}};
            prompt=f==Arithmetic?$"متتالية حسابية حدها الأول {first} والفرق الثابت {diff}. ما الحد رقم {n}؟":
                 $"جد قيمة الدالة {diff}ن+{first} عندما ن={n}. اكتب الناتج.";
        }
        return new Item(f,prompt,
             "الميزة الثابتة في الحسابية هي الفرق، وفي الهندسية هي النسبة، وحد الرتبة يحسب بحسب القاعدة.",
             AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        Expected(f,p,out var answer)?answer:throw new InvalidOperationException("Invalid sequence parameters.");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>70||
           !Expected(f,p,out string expected))return false;
        string actual=Norm(response);
        if(f==Classify)return actual==expected;
        return decimal.TryParse(actual,NumberStyles.Number,CultureInfo.InvariantCulture,out var a)&&
            decimal.TryParse(expected,NumberStyles.Number,CultureInfo.InvariantCulture,out var b)&&a==b;
    }
}
