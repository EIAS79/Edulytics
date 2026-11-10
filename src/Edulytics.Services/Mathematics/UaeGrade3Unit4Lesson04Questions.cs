using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;
// The exact engine's existing array family already handles products.
// Add only the genuinely missing factor-completion capability.
internal static class UaeGrade3Unit4Lesson04Questions
{
    internal const string MissingFactor="uae.g3.u04.l04.find_missing_array_factor";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family==MissingFactor;
    public static Item Build(string family,Random rng)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G3 array factor family");
        int rows=rng.Next(2,11),columns=rng.Next(2,11);
        int hidden=rng.Next(0,2),result=hidden==0?rows:columns;
        string prompt=hidden==0
            ? $"مصفوفة فيها □ صفوف، وكل صف يحوي {columns} عناصر، ومجموعها {rows*columns}. كم صفًا؟"
            : $"مصفوفة فيها {rows} صفوف ومجموعها {rows*columns} عنصرًا. كم عنصرًا في كل صف؟";
        var p=new Dictionary<string,int>{{"rows",rows},{"columns",columns},{"hidden",hidden},{"result",result}};
        return new Item(family,prompt,
            "أحدد العامل الذي إذا ضربته في العامل المعروف أعطاني ناتج المصفوفة، ثم أتحقق.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
    {
        expected=0;
        if(!Supports(f)||p.Count!=4||
           !p.TryGetValue("rows",out int rows)||rows is <2 or >10||
           !p.TryGetValue("columns",out int columns)||columns is <2 or >10||
           !p.TryGetValue("hidden",out int hidden)||hidden is <0 or >1||
           !p.TryGetValue("result",out int supplied))return false;
        expected=hidden==0?rows:columns;
        return supplied==expected;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int v))throw new InvalidOperationException("Invalid missing factor parameters");
        return v.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p,out int v)||string.IsNullOrWhiteSpace(answer))return false;
        string value=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
            c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(value,NumberStyles.Integer,CultureInfo.InvariantCulture,out int x)&&x==v;
    }
}
