using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// UAE G4 U03 L08: evaluate whether a numerical claim is reasonable, 1=yes 0=no.
internal static class UaeGrade4Unit3Lesson08ReasonablenessQuestions
{
    internal const string SumEstimate="uae.g4.u03.l08.sum_estimate";
    internal const string SavingsGoal="uae.g4.u03.l08.savings_feasible";
    internal const string TravelDistance="uae.g4.u03.l08.travel_feasible";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is SumEstimate or SavingsGoal or TravelDistance;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown reasonable answer family");
        int a=rng.Next(100,500),b=rng.Next(100,500),c=rng.Next(1,6);
        string text=f switch
        {
            SumEstimate=>$"المجموع {a} + {b}. هل التقدير لأقرب مئة ({(int)Math.Round((a+b)/100.0,MidpointRounding.AwayFromZero)*100+(c%2==0?0:300)}) معقول؟ اكتب ١ لنعم و٠ للا.",
            SavingsGoal=>$"يدخر شخص {a} درهم أسبوعيًا لمدة {c} أسابيع. هل يستطيع شراء غرض ثمنه {b*c} دراهم؟ اكتب ١ لنعم و٠ للا.",
            _=>$"يسير القطار {a} كيلومترًا في الساعة. هل يمكنه قطع {b*c} كيلومترًا في {c} ساعات؟ اكتب ١ لنعم و٠ للا."
        };
        return new Item(f,text,
            "أقدّر النتيجة، وأقارن المبلغ أو المسافة بالحد المطلوب، ثم أجيب ١ إذا كانت العبارة صحيحة و٠ إن كانت خاطئة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int n)
    {
        n=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||
           !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
           a is <100 or >=500||b is <100 or >=500||c is <1 or >=6)return false;
        n=f switch {SumEstimate=>c%2==0?1:0,SavingsGoal=>a>=b?1:0,_=>a>=b?1:0};
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid reasonableness");
        return n.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int n)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(ch=>ch!=','&&ch!='٬'&&ch!=' '&&ch!='\u00a0')
            .Select(ch=>ch is >= '٠' and <= '٩'?(char)('0'+ch-'٠'):
                        ch is >= '۰' and <= '۹'?(char)('0'+ch-'۰'):ch).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==n;
    }
}
