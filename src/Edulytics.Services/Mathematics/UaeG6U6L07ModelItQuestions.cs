using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact numeric subskills of Grade 6 teacher-led model-it inquiry.
/// Constructing drawings and original real-world explanations is not auto-graded.</summary>
internal static class UaeG6U6L07ModelItQuestions
{
    internal const string Seats="uae.g6.u06.l07.table_seats",Stairs="uae.g6.u06.l07.staircase_squares",
        Savings="uae.g6.u06.l07.savings_after_weeks",Teams="uae.g6.u06.l07.equal_team_size",Votes="uae.g6.u06.l07.fractional_votes";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Seats or Stairs or Savings or Teams or Votes;
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out decimal answer)
    {
        answer=0m;
        if(!Supports(family)||p is null)return false;
        if(family==Seats)
        {
            if(p.Count!=1||!p.TryGetValue("tables",out int n)||n is <1 or >20)return false;
            answer=4*n+2;return true;
        }
        if(family==Stairs)
        {
            if(p.Count!=1||!p.TryGetValue("step",out int n)||n is <1 or >25)return false;
            answer=n*(n+1)/2m;return true;
        }
        if(family==Savings)
        {
            if(p.Count!=3||!p.TryGetValue("initialCents",out int init)||
                !p.TryGetValue("weekCents",out int weekly)||!p.TryGetValue("weeks",out int weeks)||
                init is <0 or >10000||weekly is <1 or >5000||weeks is <1 or >52)return false;
            answer=(init+(decimal)weekly*weeks)/100m;return true;
        }
        if(family==Teams)
        {
            if(p.Count!=2||!p.TryGetValue("people",out int people)||
                !p.TryGetValue("teams",out int n)||people is <4 or >120||
                n is <2 or >12||people%n!=0)return false;
            answer=people/n;return true;
        }
        if(p.Count!=3||!p.TryGetValue("people",out int total)||
           !p.TryGetValue("numerator",out int num)||!p.TryGetValue("denominator",out int den)||
           total is <4 or >120||num is <1 or >7||den is <2 or >8||
           num>=den||(total*num)%den!=0)return false;
        answer=total*num/den;return true;
    }
    private static string Normalize(string? s) => new string((s??"").Where(ch=>!char.IsWhiteSpace(ch))
        .Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.','−' or '–'=>'-',_=>ch}).ToArray());
    internal static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported inquiry family");
        ArgumentNullException.ThrowIfNull(r);
        Dictionary<string,int> p;
        string prompt;
        if(family==Seats)
        {
            int n=r.Next(1,21);p=new(){{"tables",n}};
            prompt=$"عند وصل {n} طاولات مستطيلة في صف، النموذج الأول يتسع لـ٦ ويضيف كل جدول تالٍ ٤ أماكن. كم مقعدًا في النمط؟";
        }
        else if(family==Stairs)
        {
            int n=r.Next(1,26);p=new(){{"step",n}};
            prompt=$"نمط مربعات السلم يحوي في الأشكال الأولى ١ ثم ٣ ثم ٦ مربعات، وكل شكل يضيف صفًا متزايدًا. كم مربعًا في الشكل رقم {n}؟";
        }
        else if(family==Savings)
        {
            int init=r.Next(0,101)*100+r.Next(0,100),week=r.Next(1,51)*100+r.Next(0,100);
            int weeks=r.Next(1,53);
            p=new(){{"initialCents",init},{"weekCents",week},{"weeks",weeks}};
            prompt=$"رصيد أولي {(init/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهم، وتضاف {(week/100m).ToString("0.00",CultureInfo.InvariantCulture)} درهم كل أسبوع. كم الرصيد بعد {weeks} أسبوعًا؟";
        }
        else if(family==Teams)
        {
            int n=r.Next(2,13),size=r.Next(2,11);
            p=new(){{"people",n*size},{"teams",n}};
            prompt=$"وزع {n*size} طالبًا على {n} فرق متساوية باستعمال نموذج مربعات. كم طالبًا في كل فريق؟";
        }
        else
        {
            int den=r.Next(2,9),num=r.Next(1,den),factor=r.Next(1,13),total=den*factor;
            p=new(){{"people",total},{"numerator",num},{"denominator",den}};
            prompt=$"في استطلاع شارك {total} شخصًا، اختار {num}/{den} منهم الخيار الأول. كم شخصًا اختار هذا الخيار؟";
        }
        return new Item(family,prompt,
            "افهم المعطيات، مثلها بأشياء أو جدول، احسب ثم تحقق بالنموذج؛ الرسم الحر يراجعه المعلم.",
            AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)=>
        TryExpected(f,p,out decimal ans)?ans.ToString("0.##",CultureInfo.InvariantCulture):
            throw new InvalidOperationException("Invalid inquiry parameters");
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? response)=>
        !string.IsNullOrWhiteSpace(response)&&response.Length<=50&&
        TryExpected(f,p,out decimal expected)&&
        decimal.TryParse(Normalize(response),NumberStyles.Number,CultureInfo.InvariantCulture,out decimal actual)&&
        actual==expected;
}
