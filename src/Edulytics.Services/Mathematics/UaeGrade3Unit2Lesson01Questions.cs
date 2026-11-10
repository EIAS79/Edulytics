using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade 3 adds only the missing three-addend grouping; commutative and identity reused from G2.
internal static class UaeGrade3Unit2Lesson01Questions
{
    internal const string Associative="uae.g3.u02.l01.associative_add";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f==Associative;
    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G3 addition property");
        int a=rng.Next(1,10),b=rng.Next(1,10),c=rng.Next(1,10),mode=rng.Next(0,2);
        int total=a+b+c;
        var prompt=mode==0?$"أكمل الناتج باستخدام خاصية التجميع: ({a} + {b}) + {c} = {a} + ({b} + {c}) = ؟"
            :$"أجمع {a} و{b} و{c} بإعادة التجميع: {a} + ({b} + {c}) = ({a} + {b}) + {c} = ؟";
        return new Item(family,prompt,"خاصية التجميع تغير الأقواس ولا تغير المجموع.",AssessmentItemType.Numeric,
            new Dictionary<string,int>{{"a",a},{"b",b},{"c",c},{"mode",mode},{"result",total}});
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int total)
    {
        total=0;
        if(!Supports(f)||p.Count!=5||!p.TryGetValue("a",out int a)||a is <1 or >9||
           !p.TryGetValue("b",out int b)||b is <1 or >9||
           !p.TryGetValue("c",out int c)||c is <1 or >9||
           !p.TryGetValue("mode",out int mode)||mode is <0 or >1||
           !p.TryGetValue("result",out int result))return false;
        total=a+b+c;return total==result;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int total))throw new InvalidOperationException("Invalid associative properties");
        return total.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out int total)||string.IsNullOrWhiteSpace(answer))return false;
        string ascii=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out int entered)&&entered==total;
    }
}
