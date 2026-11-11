using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit7Lesson01ShapePatternQuestions
{
    internal const string Length="uae.g4.u07.l01.repeating_pattern_unit_length";
    internal const string Next="uae.g4.u07.l01.repeating_pattern_symbol_position";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is Length or Next;
    internal static Item Build(string f,Random rng)
    {
        if(!Supports(f))throw new InvalidOperationException("Unknown shape pattern family");
        int period=rng.Next(2,5),position=rng.Next(7,22);
        string[] symbols=["نجمة","قمر","دائرة","مربع"];
        string sequence=string.Join("، ",Enumerable.Range(0,period*2).Select(i=>symbols[i%period]));
        string prompt=f==Length
          ?$"تأمل نمط الأشكال المتكرر: {sequence}. كم شكلًا في الوحدة النمطية التي تتكرر؟"
          :$"تأمل نمط الأشكال: {sequence}. ما ترتيب الشكل في الموضع {position} داخل الوحدة المتكررة؟ اكتب ١ للشكل الأول و٢ للثاني وهكذا.";
        return new Item(f,prompt,"أحدد الوحدة النمطية الأصغر التي تتكرر دون تغيير، ثم أستخدم ترتيب الموضع داخلها.",
            AssessmentItemType.Numeric,new Dictionary<string,int>{{"period",period},{"position",position}});
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
    {
        result=0;
        if(!Supports(f)||p.Count!=2||!p.TryGetValue("period",out int period)||
           !p.TryGetValue("position",out int position)||period is <2 or >4||position is <7 or >21)return false;
        result=f==Length?period:(position-1)%period+1;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid shape pattern");
        return result.ToString(CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
        var s=new string(answer.Trim().Where(c=>c!=','&&c!='٬'&&c!=' '&&c!='\u00a0')
            .Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
        return int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
    }
}
