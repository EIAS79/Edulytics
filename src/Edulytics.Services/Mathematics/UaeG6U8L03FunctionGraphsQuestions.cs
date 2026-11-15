using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Bounded source-scoped UAE G6 Unit 8 function equations from coordinate plots.
/// Freehand graphs, justification and model design require teacher review.</summary>
internal static class UaeG6U8L03FunctionGraphsQuestions
{
    internal const string Equation="uae.g6.u08.l03.equation_from_line";
    internal const string Linear="uae.g6.u08.l03.linear_vs_square";
    internal const string Intercept="uae.g6.u08.l03.y_intercept";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Equation or Linear or Intercept;
    private static string Normalize(string? text)=>
       new string((text??"").Where(c=>!char.IsWhiteSpace(c)).Select(c=>c switch {
        >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
        '−' or '–'=>'-','٫'=>'.','×'=>'*',_=>c}).ToArray());
    private static bool TryExpected(string f,IReadOnlyDictionary<string,int>? p,out string result)
    {
        result="";
        if(!Supports(f)||p is null)return false;
        if(f==Linear)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >1)return false;
            result=mode==0?"خطية":"غير خطية";return true;
        }
        if(p.Count!=2||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
            a is <1 or >20||b is <0 or >30)return false;
        result=f==Intercept?b.ToString(CultureInfo.InvariantCulture):
              b==0?$"ص={a}س":$"ص={a}س+{b}";
        return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported graph family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string question;
        if(f==Linear)
        {
            int mode=r.Next(2);p=new(){{"mode",mode}};
            question=mode==0?
                "هل النقاط (١،٣)،(٢،٦)،(٣،٩) التي تقع على مستقيم تمثل دالة خطية أم غير خطية؟":
                "هل نقاط مساحة المربع (١،١)،(٢،٤)،(٣،٩) تمثل دالة خطية أم غير خطية؟";
        }
        else
        {
            int a=r.Next(1,21),b=r.Next(0,31);p=new(){{"a",a},{"b",b}};
            question=f==Equation?
                $"دالة خطية تمر بالنقطتين (٠،{b}) و(١،{a+b}). اكتب معادلتها على صورة ص=أ س+ب.":
                $"الدالة ص={a}س+{b}، ما المقطع الرأسي عندما س=٠؟ اكتب العدد.";
        }
        return new Item(f,question,
            "الميل زيادة المخرج لكل وحدة مدخل، والمقطع هو قيمة المخرج عندما المدخل صفر.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryExpected(f,p,out var result)?result:throw new InvalidOperationException("Invalid graph parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>100||
           !TryExpected(f,p,out var expected))return false;
        string got=Normalize(response),wanted=Normalize(expected);
        if(got==wanted)return true;
        if(f==Intercept)
            return int.TryParse(got,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&
                   int.TryParse(expected,NumberStyles.Integer,CultureInfo.InvariantCulture,out int t)&&n==t;
        if(f==Equation)
        {
            int a=p["a"],b=p["b"];
            return got==(b==0?$"y={a}x":$"y={a}x+{b}")||
                   got==(b==0?$"ص={a}*س":$"ص={a}*س+{b}");
        }
        return false;
    }
}
