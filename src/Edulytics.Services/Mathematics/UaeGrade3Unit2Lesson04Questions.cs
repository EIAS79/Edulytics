using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 3 mental-addition mechanics extend the shared exact engine.
internal static class UaeGrade3Unit2Lesson04Questions
{
    internal const string Family="uae.g3.u02.l04.mental_addition";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    public static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported mental addition family");
        int mode=random.Next(2),a=random.Next(40,700),b,change;
        if(mode==0)
        {
            b=random.Next(20,350);
            change=0;
        }
        else
        {
            int multiple=random.Next(3,25)*10;
            change=random.Next(1,5);
            b=multiple-change;
        }
        var total=a+b;
        string prompt=mode==0
           ?$"فكك إلى مئات وعشرات وآحاد ثم احسب ذهنيًا: {a} + {b} = ؟"
           :$"احسب {a} + {b} ذهنياً. اجعل {b} أقرب مضاعف للعشرة بإضافة {change} ثم عوّض بطرح المقدار نفسه من {a}. ما المجموع الدقيق؟";
        return new Item(family,prompt,"أستخدم التفكيك أو أضيف إلى حد وأطرح المقدار نفسه من الحد الآخر.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"change",change},{"result",total}});
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int total)
    {
        total=0;
        if(!Supports(f)||p.Count!=5||!p.TryGetValue("a",out int a)||a is <40 or >=700||
            !p.TryGetValue("b",out int b)||b is <16 or >350||
            !p.TryGetValue("mode",out int m)||m is <0 or >1||
            !p.TryGetValue("change",out int c)||!p.TryGetValue("result",out int given))return false;
        if(m==0&&c!=0||m==1&&(c is <1 or >4||(b+c)%10!=0))return false;
        total=a+b;
        return total==given;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int t))throw new InvalidOperationException("Invalid mental sum parameters");
        return t.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out var total)||string.IsNullOrWhiteSpace(answer))return false;
        var ascii=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out var value)&&value==total;
    }
}
