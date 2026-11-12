using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson04DivisionModelQuestions
{
    internal const string Quotient="uae.g4.u06.l04.model_after_removing_items_quotient";
    internal const string Remainder="uae.g4.u06.l04.model_after_removing_items_remainder";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Quotient or Remainder;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown model division family");
        int group=random.Next(2,9),each=random.Next(2,26),removed=random.Next(1,10);
        int remainder=random.Next(0,group),total=group*each+removed+remainder;
        string prompt=f==Quotient
         ?$"لديك {total} قطعة، استبعد {removed} قطع، وقسّم الباقي على {group} مجموعات. صمّم نموذجًا واكتب عدد القطع الكاملة في المجموعة الواحدة."
         :$"لديك {total} قطعة، استبعد {removed} قطع، وقسّم الباقي على {group} مجموعات. كم قطعة تتبقى بعد تكوين المجموعات المتساوية؟";
        return new Item(f,prompt,"أرسم مجموعات: أطرح غير المطلوب من المجموع، ثم أقسّم الباقي إلى مجموعات متساوية.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"total",total},{"removed",removed},{"groups",group}});
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
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid division model");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var actual)&&actual==expected;
    }
}
