using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact tape model equation-writing for G6 U07L07.
 /// Reuses the Grade 6 one-step multiply/divide engine for objective numerical practice.</summary>
internal static class UaeG6U7L07MultiplicationLabQuestions
{
    internal const string Write="uae.g6.u07.l07.write_equal_groups";
    internal const string Inverse="uae.g6.u07.l07.inverse_division_statement";
    internal const string Groups="uae.g6.u07.l07.classify_equal_groups";
    internal sealed record Item(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Write or Inverse or Groups;
    private static string Normalize(string? input)=>new string((input??"").Where(c=>!char.IsWhiteSpace(c))
        .Select(c=>c switch {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '×' or '·'=>'*', '÷'=>'/', '−' or '–'=>'-', _=>c
        }).ToArray());
    private static bool TryExpected(string family,IReadOnlyDictionary<string,int>? p,out string expected)
    {
        expected="";
        if(!Supports(family)||p is null||p.Count!=2||
           !p.TryGetValue("groups",out int groups)||!p.TryGetValue("total",out int total)||
           groups is <2 or >12||total is <2 or >144||total%groups!=0)return false;
        expected=family switch {
            Write=>$"{groups}س={total}",
            Inverse=>$"س={total}/{groups}",
            _=>groups.ToString(CultureInfo.InvariantCulture)
        };
        return true;
    }
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported grouped equation");
        ArgumentNullException.ThrowIfNull(random);
        int groups=random.Next(2,13),each=random.Next(1,13),total=groups*each;
        Dictionary<string,int> p=new(){{"groups",groups},{"total",total}};
        string prompt=family switch {
            Write=>$"يمثل الشريط {groups} أجزاء متساوية، قيمة كل جزء س، والكل {total}. اكتب معادلة الضرب للحالة.",
            Inverse=>$"لديك {groups}س={total}، اكتب جملة القسمة المعكوسة على صورة س=الكل/عدد المجموعات.",
            _=>$"إذا قسمت {total} عنصرًا إلى مجموعات متساوية بكل مجموعة {each} عنصرًا، فكم مجموعة توجد؟"
        };
        return new Item(family,prompt,
          "كل جزء متساوٍ يمثله متغير، وعدد الأجزاء مضروبًا في قيمة الجزء يساوي الكل.",
          AssessmentItemType.ShortAnswer,p);
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)=>
        TryExpected(family,p,out var answer)?answer:
            throw new InvalidOperationException("Invalid grouped-equation parameters.");
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? response)
    {
        if(string.IsNullOrWhiteSpace(response)||response.Length>70||
           !TryExpected(family,p,out string expected))return false;
        string s=Normalize(response);
        if(s==expected)return true;
        int groups=p["groups"],total=p["total"];
        if(family==Write)return s==$"س*{groups}={total}"||s==$"{groups}*س={total}"||s==$"{total}={groups}س";
        if(family==Inverse)return s==$"س={total}÷{groups}"||s==$"س={total/groups}";
        return false;
    }
}
