using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>
/// Exact bounded first-party UAE G6 function-table engine. Input/output
/// numerical templates can be reused in later Unit 8 lessons. Free graphs
/// and interpretation remain teacher-reviewed.
/// </summary>
internal static class UaeG6U8FunctionQuestions
{
    internal const string Output="uae.g6.u08.l01.linear_output";
    internal const string Input="uae.g6.u08.l01.linear_input";
    internal const string Division="uae.g6.u08.l01.division_output";
    internal const string Pair="uae.g6.u08.l01.ordered_pair";
    internal const string Classify="uae.g6.u08.l01.classify_variable";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is Output or Input or Division or Pair or Classify;
    private static string Norm(string? text)=>new string((text??"").Where(c=>!char.IsWhiteSpace(c)&&c!='('&&c!=')')
        .Select(c=>c switch {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '٫'=>'.','،'=>',','−' or '–'=>'-',_=>c
        }).ToArray());
    private static bool Expected(string family,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(family)||p is null)return false;
        if(family==Classify)
        {
            if(p.Count!=1||!p.TryGetValue("mode",out int mode)||mode is <0 or >1)return false;
            answer=mode==0?"مستقل":"تابع";return true;
        }
        if(family==Division)
        {
            if(p.Count!=2||!p.TryGetValue("total",out int total)||
               !p.TryGetValue("x",out int x)||total is <1 or >300||
               x is <1 or >100||total%x!=0)return false;
            answer=(total/x).ToString(CultureInfo.InvariantCulture);return true;
        }
        if(p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
           !p.TryGetValue("x",out int input)||a is <1 or >100||b is <-30 or >30||
           input is <0 or >50)return false;
        int output=checked(a*input+b);
        answer=family switch {
            Output=>output.ToString(CultureInfo.InvariantCulture),
            Input=>input.ToString(CultureInfo.InvariantCulture),
            _=>$"{input},{output}"
        };
        return true;
    }
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported function-table family");
        ArgumentNullException.ThrowIfNull(random);
        Dictionary<string,int> p;
        string question;
        if(f==Classify)
        {
            int mode=random.Next(2);p=new(){{"mode",mode}};
            question=mode==0?"في الدالة ص=٧س، ما نوع متغير الساعات س؟ اكتب مستقل أو تابع.":
                "في الدالة ص=٧س، ما نوع متغير المسافة ص الذي يعتمد على الزمن؟ اكتب مستقل أو تابع.";
        }
        else if(f==Division)
        {
            int x=random.Next(1,21),outVal=random.Next(1,16),total=x*outVal;
            p=new(){{"total",total},{"x",x}};
            question=$"توزع {total} قطعة حلوى بالتساوي على {x} أشخاص. ما مخرج قاعدة الدالة ص={total}÷س عند س={x}؟";
        }
        else
        {
            int a=random.Next(1,101),b=random.Next(-15,16),x=random.Next(0,21);
            p=new(){{"a",a},{"b",b},{"x",x}};
            string rule=b==0?$"{a}س":b>0?$"{a}س+{b}":$"{a}س−{-b}";
            int output=a*x+b;
            question=f switch{
                Output=>$"أكمل جدول الدالة ص={rule} عندما س={x}. ما قيمة المخرج ص؟",
                Input=>$"قاعدة الدالة ص={rule} ومخرجها {output}. ما قيمة المدخل س؟",
                _=>$"للدالة ص={rule} والمدخل س={x}، اكتب الزوج المرتب (س،ص)."
            };
        }
        return new Item(f,question,
           "طبّق قاعدة الدالة على المدخل أو اعكس العمليات على المخرج؛ الزوج المرتب يكتب (س،ص).",
           AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        Expected(family,p,out var answer)?answer:throw new InvalidOperationException("Invalid function parameters");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>80||
           !Expected(family,p,out string expected))return false;
        string actual=Norm(response);
        if(family==Classify||family==Pair)return actual==expected;
        return int.TryParse(actual,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int num)&&
            int.TryParse(expected,NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out int val)&&num==val;
    }
}
