using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Adds an equal-groups variation to the EXISTING exact Practice engine, not a new engine.
internal static class UaeGrade3Unit5Lesson01Questions
{
    internal const string Family="uae.g3.u05.l01.model_equal_groups_division";
    internal sealed record Item(string Family,string Prompt,string Solution,
      AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    public static Item Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G3 division model");
        int groups=r.Next(2,11),each=r.Next(2,11),total=groups*each,mode=r.Next(0,2);
        int result=mode==0?each:groups;
        string prompt=mode==0?$"قسّم {total} قطعة على {groups} مجموعات متساوية. كم قطعة في كل مجموعة؟"
          :$"معك {total} قطعة وتضع {each} قطع في كل مجموعة. كم مجموعة متساوية يمكنك تكوينها؟";
        return new Item(family,prompt,"أرسم مجموعات متساوية وأتحقق بالضرب: المجموعات × العناصر في كل مجموعة = الكل.",
          AssessmentItemType.Numeric,
          new Dictionary<string,int>{{"groups",groups},{"each",each},{"total",total},{"mode",mode},{"result",result}});
    }
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(family)||p.Count!=5||
           !p.TryGetValue("groups",out int groups)||groups is <2 or >10||
           !p.TryGetValue("each",out int each)||each is <2 or >10||
           !p.TryGetValue("total",out int total)||total!=groups*each||
           !p.TryGetValue("mode",out int mode)||mode is <0 or >1||
           !p.TryGetValue("result",out int supplied))return false;
        result=mode==0?each:groups;return supplied==result;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int result))throw new InvalidOperationException("Invalid equal-group parameters");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out int result)||string.IsNullOrWhiteSpace(answer))return false;
        string digits=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
           c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(digits,NumberStyles.Integer,CultureInfo.InvariantCulture,out var entered)&&entered==result;
    }
}
