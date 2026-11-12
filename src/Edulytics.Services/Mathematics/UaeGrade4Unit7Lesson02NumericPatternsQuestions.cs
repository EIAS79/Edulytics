using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit7Lesson02NumericPatternsQuestions
{
    internal const string Constant="uae.g4.u07.l02.constant_numeric_step";
    internal const string Alternating="uae.g4.u07.l02.alternating_numeric_rule";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Constant or Alternating;
    internal static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported numeric pattern");
        int start=r.Next(20,100),step=r.Next(2,10),index=r.Next(4,9);
        var values=new List<int>{start};
        for(int i=1;i<index;i++)
            values.Add(f==Constant?values[^1]+step:
                values[^1]+(i%2==1?-step:step+2));
        string sequence=string.Join("، ",values.Take(index-1));
        return new Item(f,$"أكمل النمط العددي {sequence}، □. ما العدد التالي؟",
            f==Constant?"أبحث عن الفرق الثابت بين الأعداد وأطبقه.":"أبحث عن قاعدتين تتناوبان بين طرح عدد وإضافة عدد مختلف.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"start",start},{"step",step},{"index",index}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("start",out int start)||
           !p.TryGetValue("step",out int step)||!p.TryGetValue("index",out int index)||
           start is <20 or >99||step is <2 or >9||index is <4 or >8)return false;
        result=start;
        for(int i=1;i<index;i++)result+=f==Constant?step:(i%2==1?-step:step+2);
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int x))throw new InvalidOperationException("Invalid pattern");
        return x.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out var x)||string.IsNullOrWhiteSpace(answer))return false;
        var s=new string(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var y)&&x==y;
    }
}
