using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit8Lesson01FactorMultipleQuestions
{
    internal const string MissingFactor="uae.g4.u08.l01.missing_factor";
    internal const string IsMultiple="uae.g4.u08.l01.is_multiple";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is MissingFactor or IsMultiple;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported factor family");
        int a=random.Next(2,13),b=random.Next(2,13),offset=random.Next(0,2);
        IReadOnlyDictionary<string,int> parameters=f==MissingFactor?
            new Dictionary<string,int>{{"a",a},{"b",b},{"offset",0}}:
            new Dictionary<string,int>{{"a",a},{"b",b},{"offset",offset}};
        string prompt=f==MissingFactor?
            $"أكمل زوج العوامل: {a*b} = {a} × □. اكتب العدد الناقص.":
            $"هل العدد {a*b+offset} من مضاعفات {a}؟ اكتب ١ لنعم أو ٠ للا.";
        string solution=f==MissingFactor?
            $"اقسم {a*b} على {a} لتحصل على العامل الآخر.":
            $"أوجد باقي قسمة {a*b+offset} على {a}؛ إذا كان صفرًا فالعدد مضاعف.";
        return new Item(f,prompt,solution,AssessmentItemType.Numeric,parameters);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int answer)
    {
        answer=0;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||
            !p.TryGetValue("b",out int b)||!p.TryGetValue("offset",out int offset)||
            a is <2 or >12 || b is <2 or >12 || offset is <0 or >1 ||
            (f==MissingFactor && offset!=0))return false;
        answer=f==MissingFactor?b:offset==0?1:0;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int answer))throw new InvalidOperationException("Invalid parameters");
        return answer.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        var value=new string(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            _=>c
        }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
        return int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out int result)&&result==expected;
    }
}
