using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson04DivisionModelQuestions
{
    internal const string Quotient="uae.g4.u06.l04.model_after_removing_items_quotient";
    internal const string G5DecimalGroup="uae.g5.u05.l10.tenths_equal_group_quotient";
    internal const string G5TenthsEach="uae.g5.u05.l10.tenths_per_group";
    internal const string G5WholeEach="uae.g5.u05.l10.whole_units_per_group";
    private static bool IsG5(string? f)=>f is G5DecimalGroup or G5TenthsEach or G5WholeEach;
    internal const string Remainder="uae.g4.u06.l04.model_after_removing_items_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder || IsG5(f);
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown model division family");
        if(IsG5(f))return BuildG5(f,random);
        int group=random.Next(2,9),each=random.Next(2,26),removed=random.Next(1,10);
        int remainder=random.Next(0,group),total=group*each+removed+remainder;
        string prompt=f==Quotient
         ?$"لديك {total} قطعة، استبعد {removed} قطع، وقسّم الباقي على {group} مجموعات. صمّم نموذجًا واكتب عدد القطع الكاملة في المجموعة الواحدة."
         :$"لديك {total} قطعة، استبعد {removed} قطع، وقسّم الباقي على {group} مجموعات. كم قطعة تتبقى بعد تكوين المجموعات المتساوية؟";
        return new Item(f,prompt,"أرسم مجموعات: أطرح غير المطلوب من المجموع، ثم أقسّم الباقي إلى مجموعات متساوية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"total",total},{"removed",removed},{"groups",group}});
    }

    // Extend existing equal-groups division model to tenths; students
    // distribute whole units and tenths to equal groups and check by ×.
    private static Item BuildG5(string f,Random rng)
    {
        int groups=rng.Next(2,9),perGroupTenths=rng.Next(2,30);
        int dividendTenths=checked(groups*perGroupTenths);
        string amount=(dividendTenths/10m).ToString("0.0",CultureInfo.InvariantCulture);
        string prompt=f switch
        {
            G5DecimalGroup=>$"قسّم {amount} إلى {groups} مجموعات متساوية باستخدام وحدات وأعشار. ما قيمة كل مجموعة؟",
            G5TenthsEach=>$"لديك {dividendTenths} قطعة أعشار موزعة بالتساوي على {groups} مجموعات. كم قطعة من عشرة لكل مجموعة؟",
            _=>$"عند تقسيم {amount} إلى {groups} مجموعات متساوية، كم وحدة كاملة في كل مجموعة؟"
        };
        return new Item(f,prompt,
            "أحوّل الوحدات إلى أعشار عند الحاجة، أقسم الأعشار ثم أراجع بالضرب.",
            AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"dividendTenths",dividendTenths},{"groups",groups}});
    }
    private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out decimal answer)
    {
        answer=0m;
        if(!IsG5(f)||p.Count!=2||
           !p.TryGetValue("dividendTenths",out int n)||
           !p.TryGetValue("groups",out int g)||
           n is <4 or >224||g is <2 or >8||
           n%g!=0||n/g is <2 or >29)return false;
        int tenthsEach=n/g;
        answer=f switch{G5DecimalGroup=>tenthsEach/10m,
            G5TenthsEach=>tenthsEach,_=>tenthsEach/10};
        return true;
    }
    private static string SolveG5(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!ValidG5(f,p,out decimal answer))
            throw new InvalidOperationException("Invalid G5 tenths division model");
        return answer.ToString("0.##",CultureInfo.InvariantCulture);
    }
    private static bool VerifyG5(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!ValidG5(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(ch=>ch switch{
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '٫'=>'.',_=>ch
        }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(normalized.Length>20||!normalized.All(ch=>ch is >= '0' and <= '9' or '.'))
            return false;
        if(!decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
            CultureInfo.InvariantCulture,out decimal provided))return false;
        int total=p["dividendTenths"],groups=p["groups"];
        return f switch{
            G5DecimalGroup=>provided*10m*groups==total,
            G5TenthsEach=>provided*groups==total,
            _=>provided==total/groups/10
        };
    }

    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("total",out int n)||
           !p.TryGetValue("removed",out int x)||!p.TryGetValue("groups",out int d)||
           d is <2 or >8||x is <1 or >9||n<=x+d||n>500)return false;
        answer=f==Quotient?(n-x)/d:(n-x)%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(IsG5(f))return SolveG5(f,p);
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid division model");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(IsG5(f))return VerifyG5(f,p,answer);
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var actual)&&actual==expected;
    }
}
