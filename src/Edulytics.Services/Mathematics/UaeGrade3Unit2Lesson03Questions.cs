using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Numeric place-value patterns reuse the existing exact Practice engine.
internal static class UaeGrade3Unit2Lesson03Questions
{
    internal const string Add="uae.g3.u02.l03.add_place_step";
    internal const string Missing="uae.g3.u02.l03.find_missing_step";
    internal const string Next="uae.g3.u02.l03.complete_pattern";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Add or Missing or Next;
    public static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported G3 additive pattern family");
        int step=new[]{1,10,100,1000}[rng.Next(4)];
        int n=rng.Next(100,7000);int result=f==Missing?step:f==Next?n+3*step:n+step;
        var prompt=f==Add?$"في القيمة المكانية، ما العدد الناتج عن {n} + {step}؟"
           :f==Missing?$"ما مقدار الزيادة في جملة الجمع: {n} + □ = {n+step}؟"
           :$"أكمل النمط العددي: {n}، {n+step}، {n+2*step}، □.";
        return new Item(f,prompt,"حدد المنازل وأضف المقدار الثابت ثم أعد التجميع إذا لزم.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"start",n},{"step",step},{"result",result}});
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int value)
    {
        value=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("start",out int n)||n is <100 or >6999||
           !p.TryGetValue("step",out int step)||step is not (1 or 10 or 100 or 1000)||
           !p.TryGetValue("result",out int result))return false;
        value=f==Missing?step:f==Next?n+3*step:n+step;return value==result&&value<=10000;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int v))throw new InvalidOperationException("Invalid G3 additive patterns");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p,out int v)||string.IsNullOrWhiteSpace(answer))return false;
        var t=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(t,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==v;
    }
}
