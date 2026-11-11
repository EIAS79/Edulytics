using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit6Lesson06InterpretRemainderQuestions
{
    internal const string Extra="uae.g4.u06.l06.extra_groups_round_up";
    internal const string Left="uae.g4.u06.l06.leftover_items";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Extra or Left;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported contextual division family");
        int groups=r.Next(2,13),divisor=r.Next(2,10),remainder=r.Next(1,divisor);
        int count=groups*divisor+remainder;
        string prompt=f==Extra
            ?$"يوجد {count} طالبًا، وتتسع كل سيارة لـ {divisor} طلاب. ما عدد السيارات التي نحتاجها ليذهب الجميع؟"
            :$"يوجد {count} هدية، وتوضع {divisor} هدايا في كل صندوق كامل. كم هدية ستتبقى خارج الصناديق الكاملة؟";
        return new Item(f,prompt,"أقسم عدد الأشياء على سعة المجموعة. إذا احتاج كل شخص إلى مكان أضيف مجموعة للباقي، وإلا أفسر الباقي كما تسأل المسألة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"count",count},{"capacity",divisor}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("count",out int n)||
           !p.TryGetValue("capacity",out int d)||n is <5 or >150||d is <2 or >9)return false;
        result=f==Extra?(n+d-1)/d:n%d;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var result))throw new InvalidOperationException("Invalid context");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out var result)||string.IsNullOrWhiteSpace(answer))return false;
        string s=new(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var value)&&result==value;
    }
}
