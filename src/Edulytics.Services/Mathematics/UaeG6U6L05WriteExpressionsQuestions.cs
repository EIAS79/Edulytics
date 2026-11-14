using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Bounded exact symbolic expressions from Grade 6 bar-model prompts.
/// Complex rewrites, drawing and explanations remain teacher reviewed.</summary>
internal static class UaeG6U6L05WriteExpressionsQuestions
{
    internal const string More="uae.g6.u06.l05.more_than",Less="uae.g6.u06.l05.less_than",Part="uae.g6.u06.l05.part_of";
    internal const string Repeated="uae.g6.u06.l05.repeated",GroupsPlus="uae.g6.u06.l05.groups_plus",Type="uae.g6.u06.l05.operation_type";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is More or Less or Part or Repeated or GroupsPlus or Type;
    private static string Normalize(string? s)
    {
        if(string.IsNullOrWhiteSpace(s))return "";
        return new string(s.Where(ch=>!char.IsWhiteSpace(ch)&&ch!='('&&ch!=')')
            .Select(ch=>ch switch{
                >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
                >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
                '−' or '–'=>'-','×' or '·'=>'*','÷'=>'/',_=>ch}).ToArray());
    }
    private static bool Expected(string family,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(family)||p is null)return false;
        if(family==Type)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >3)return false;
            answer=mode switch{0=>"مجموع",1=>"فرق",2=>"حاصل ضرب",_=>"حاصل قسمة"};return true;
        }
        if(family==Part)
        {
            if(p.Count!=1||!p.TryGetValue("d",out int d)||d is <2 or >12)return false;
            answer=$"س/{d}";return true;
        }
        if(family==GroupsPlus)
        {
            if(p.Count!=2||!p.TryGetValue("times",out int times)||
               !p.TryGetValue("add",out int add)||times is <2 or >6||add is <1 or >25)return false;
            answer=$"{times}س+{add}";return true;
        }
        if(family==Repeated)
        {
            if(p.Count!=1||!p.TryGetValue("times",out int times)||times is <2 or >6)return false;
            answer=$"{times}س";return true;
        }
        if(p.Count!=1||!p.TryGetValue("n",out int n)||n is <1 or >50)return false;
        answer=family==More?$"س+{n}":$"س-{n}";
        return true;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown word expression family");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string prompt;
        if(family==Type)
        {
            int mode=random.Next(4);p=new(){{"mode",mode}};
            string expression=mode switch{0=>"س+٨",1=>"س−٨",2=>"٥س",_=>"س÷٣"};
            prompt=$"ما العملية الرئيسية في التعبير {expression}؟ اكتب مجموع أو فرق أو حاصل ضرب أو حاصل قسمة.";
        }
        else if(family==Part)
        {
            int d=random.Next(2,13);p=new(){{"d",d}};
            prompt=$"كمية مقدارها س، ما تعبير قسمتها على {d}؟ اكتب التعبير بالحرف س.";
        }
        else if(family==GroupsPlus)
        {
            int t=random.Next(2,7),add=random.Next(1,26);
            p=new(){{"times",t},{"add",add}};
            prompt=$"رسم شريطي فيه {t} أقسام، كل قسم س، ثم {add} وحدات إضافية. اكتب التعبير الجبري.";
        }
        else if(family==Repeated)
        {
            int t=random.Next(2,7);p=new(){{"times",t}};
            prompt=$"عدد المجموعات المتساوية {t}، وفي كل مجموعة س عناصر. اكتب التعبير الجبري المختصر.";
        }
        else
        {
            int n=random.Next(1,51);p=new(){{"n",n}};
            prompt=family==More?$"عدد أكبر من س بمقدار {n}. اكتب التعبير.":
                $"عدد أقل من س بمقدار {n}. اكتب التعبير.";
        }
        return new Item(family,prompt,"طابق الشرائط مع الكمية المجهولة والثابتة قبل كتابة الرموز.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        Expected(family,p,out string ans)?ans:throw new InvalidOperationException("Invalid symbolic parameters.");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>80||
           !Expected(family,p,out string expected))return false;
        string norm=Normalize(response),canonical=Normalize(expected);
        if(norm==canonical)return true;
        if(family==More&&p.TryGetValue("n",out int n))return norm==$"{n}+س";
        if(family==Part&&p.TryGetValue("d",out int d))return norm==$"1/{d}*س";
        if(family is Repeated or GroupsPlus)
        {
            int t=p["times"];
            if(family==Repeated)return norm==$"{t}*س"||norm==$"س*{t}";
            int a=p["add"];
            return norm==$"{t}*س+{a}"||norm==$"س*{t}+{a}";
        }
        return false;
    }
}
