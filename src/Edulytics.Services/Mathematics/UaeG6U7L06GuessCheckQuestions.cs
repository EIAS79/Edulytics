using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact objectively-checkable portions of the UAE G6 guess/check/revise inquiry.
/// Nonunique open problems, strategy justifications and student-created tables are teacher-assessed.</summary>
internal static class UaeG6U7L06GuessCheckQuestions
{
    internal const string Notes="uae.g6.u07.l06.banknotes",Comics="uae.g6.u07.l06.comic_sets",
        Quiz="uae.g6.u07.l06.quiz_points",Savings="uae.g6.u07.l06.monthly_savings",Operators="uae.g6.u07.l06.operator_check";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Notes or Comics or Quiz or Savings or Operators;
    private static string Norm(string? s)=>new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
       .Select(ch=>ch switch{
          >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
          >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
          '×' or '·'=>'*','÷'=>'/','٫'=>'.',_=>ch}).ToArray());
    private static bool Expected(string f,IReadOnlyDictionary<string,int>? p,out string answer)
    {
        answer="";
        if(!Supports(f)||p is null)return false;
        if(f==Comics||f==Quiz||f==Operators)
        {
            if(p.Count!=0)return false;
            answer=f==Comics?"٢،٢":f==Quiz?"٣،٣":"3*4+6/1";
            return true;
        }
        if(f==Notes)
        {
            if(p.Count!=2||!p.TryGetValue("n",out int n)||
               !p.TryGetValue("twenties",out int t)||n is <2 or >18||t is <1 or >17||t>=n)return false;
            answer=t.ToString(CultureInfo.InvariantCulture);return true;
        }
        if(p.Count!=3||!p.TryGetValue("initial",out int start)||
           !p.TryGetValue("month",out int monthly)||!p.TryGetValue("months",out int months)||
           start is <0 or >1000||monthly is <1 or >100||months is <1 or >24)return false;
        answer=months.ToString(CultureInfo.InvariantCulture);return true;
    }
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported guess-check family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string question;
        if(f==Notes)
        {
            int n=r.Next(2,19),twenties=r.Next(1,n);
            p=new(){{"n",n},{"twenties",twenties}};
            question=$"مع شخص {n} أوراق نقدية، بعضها ٢٠ درهمًا وبعضها ١٠ دراهم، ومجموعها {20*twenties+10*(n-twenties)} درهمًا. كم ورقة من فئة ٢٠؟ جرّب وتحقق.";
        }
        else if(f==Comics)
        {
            p=new();
            question="اشترى طالب ١٦ قصة في مجموعات: كل مجموعة جديدة ٥ قصص وكل مستعملة ٣ قصص، مع وجود النوعين. كم مجموعة جديدة وكم مجموعة مستعملة؟ اكتب العددين بهذا الترتيب مفصولين بفاصلة عربية.";
        }
        else if(f==Quiz)
        {
            p=new();
            question="اختبار فيه ٦ أسئلة، بعضها بنقطتين وبعضها بأربع نقاط. مجموع النقاط ١٨. كم سؤالًا من كل نوع؟ اكتب عدد أسئلة النقطتين ثم الأربع نقاط.";
        }
        else if(f==Operators)
        {
            p=new();
            question="أكمل التعبير ٣ □ ٤ □ ٦ □ ١ = ١٨ باستخدام × ثم + ثم ÷، مرة لكل رمز. اكتب التعبير كاملًا دون مساواة.";
        }
        else
        {
            int start=r.Next(0,1001),monthly=r.Next(1,101),months=r.Next(1,25);
            p=new(){{"initial",start},{"month",monthly},{"months",months}};
            question=$"المدخرات {start} درهمًا، يزيدها {monthly} درهمًا في الشهر. كم شهرًا للوصول إلى {start+monthly*months} درهمًا؟";
        }
        return new Item(f,question,
            "اختر تخمينًا يلتزم بجميع القيود، ثم اعمل جدولًا أو تحقق بالحساب وراجع التخمين.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        Expected(f,p,out var answer)?answer:throw new InvalidOperationException("Invalid guess-check params");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>80||
           !Expected(f,p,out string expected))return false;
        string actual=Norm(response);
        if(f is Comics or Quiz)
        {
            actual=actual.Replace(",","،");
            return actual==expected;
        }
        if(f==Operators)return actual==expected;
        return actual==expected;
    }
}
