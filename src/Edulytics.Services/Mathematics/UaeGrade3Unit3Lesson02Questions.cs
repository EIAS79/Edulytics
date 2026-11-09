using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
// Source-aligned G3 estimation extends existing exact Practice engine.
internal static class UaeGrade3Unit3Lesson02Questions
{
    internal const string Family="uae.g3.u03.l02.estimate_difference_by_rounding";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==Family;
    private static int Highest(int n)=>n>=1000?1000:n>=100?100:10;
    private static int Round(int n,int place)=>((n+place/2)/place)*place;

    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported G3 estimated subtraction");
        int a=rng.Next(201,9900),b=rng.Next(100,a);
        int mode=rng.Next(3);
        int pA=mode==0?10:mode==1?100:Highest(a);
        int pB=mode==0?10:mode==1?100:Highest(b);
        int estimated=Round(a,pA)-Round(b,pB);
        if(estimated<0)throw new InvalidOperationException("Rounded estimate unexpectedly negative");
        var prompt=mode==0?$"قدّر الفرق {a} − {b} بالتقريب إلى أقرب عشرة لكلا العددين. ما الفرق التقديري؟"
           :mode==1?$"قرّب كلا العددين إلى أقرب مئة ثم قدّر: {a} − {b} = ؟"
           :$"قدّر الفرق {a} − {b} بالتقريب إلى أعلى منزلة لكل عدد، ثم اطرح العددين المقربين.";
        return new Item(family,prompt,
            "أقرّب كل عدد إلى المنزلة المختارة أولًا، ثم أطرح القيم المقرّبة.",
            AssessmentItemType.Numeric,new Dictionary<string,int>
            {["a"]=a,["b"]=b,["mode"]=mode,["placeA"]=pA,["placeB"]=pB,["result"]=estimated});
    }
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p,out int estimated)
    {
        estimated=0;
        if(!Supports(family)||p.Count!=6||!p.TryGetValue("a",out int a)||a is <201 or >9899||
            !p.TryGetValue("b",out int b)||b<100||b>=a||
            !p.TryGetValue("mode",out int mode)||mode is <0 or >2||
            !p.TryGetValue("placeA",out int pa)||!p.TryGetValue("placeB",out int pb)||
            !p.TryGetValue("result",out int supplied))return false;
        if(pa!=(mode==0?10:mode==1?100:Highest(a))||
           pb!=(mode==0?10:mode==1?100:Highest(b)))return false;
        estimated=Round(a,pa)-Round(b,pb);
        return estimated>=0&&estimated==supplied;
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out int v))throw new InvalidOperationException("Invalid G3 estimated subtraction parameters");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(family,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        string ascii=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out int value)&&value==expected;
    }
}
