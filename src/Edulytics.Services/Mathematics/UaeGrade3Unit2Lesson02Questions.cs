using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Uses shared exact engine; Grade3 Unit2 addition-grid patterns.
internal static class UaeGrade3Unit2Lesson02Questions
{
    internal const string Sum="uae.g3.u02.l02.addition_table_sum";
    internal const string Missing="uae.g3.u02.l02.missing_table_addend";
    internal const string Pattern="uae.g3.u02.l02.even_odd_step2";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Sum or Missing or Pattern;
    public static Item Build(string f,Random r)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported addition-grid family");
        int a=r.Next(0,11),b=r.Next(0,11),start=r.Next(0,3)*2+r.Next(0,2);
        int result=f==Sum?a+b:f==Missing?b:start+6;
        string prompt=f==Sum?$"في جدول الجمع من ٠ إلى ١٠: ما ناتج تقاطع الصف {a} مع العمود {b}؟"
           :f==Missing?$"أكمل الخانة المفقودة في جدول الجمع: {a} + □ = {a+b}، ما الرقم في العمود؟"
           :$"أكمل نمط الزيادة بمقدار ٢ في جدول الجمع: {start}، {start+2}، {start+4}، □.";
        var p=new Dictionary<string,int>{{"a",a},{"b",b},{"start",start},{"result",result}};
        return new Item(f,prompt,"أقرأ تقاطع الصف والعمود أو أتابع النمط وفق مقدار ثابت.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out var a)||a is <0 or >10||
           !p.TryGetValue("b",out var b)||b is <0 or >10||
           !p.TryGetValue("start",out var start)||start is <0 or >5||
           !p.TryGetValue("result",out var v))return false;
        result=f==Sum?a+b:f==Missing?b:start+6;
        return v==result;
    }
    public static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out var result))throw new InvalidOperationException("Invalid grid family");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
    {
        if(!Valid(f,p,out var result)||string.IsNullOrWhiteSpace(answer))return false;
        string ascii=new(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
           c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(ascii,NumberStyles.Integer,CultureInfo.InvariantCulture,out var value)&&value==result;
    }
}
