using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10Lesson07DecimalAdditionQuestions
{
    internal const string SumHundredths="uae.g4.u10.l07.sum_hundredths";
    internal const string SumDecimal="uae.g4.u10.l07.sum_decimal";
    internal const string RelevantSum="uae.g4.u10.l09.relevant_sum";
    internal const string RelevantDecimal="uae.g4.u10.l09.relevant_decimal";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? f)=>f is SumHundredths or SumDecimal or RelevantSum or RelevantDecimal;
    internal static Item Build(string f,Random random)
    {
        if(!Supports(f))throw new InvalidOperationException("Unsupported decimal fraction addition");
        int t=random.Next(0,10),h=random.Next(0,10),irrelevant=random.Next(2,9);
        var p=new Dictionary<string,int>{{"tenths",t},{"hundredths",h},{"otherDays",irrelevant}};
        string prompt=f switch{
            SumHundredths=>$"أوجد {t}/١٠ + {h}/١٠٠. ما بسط المجموع عند كتابته بمقام ١٠٠؟",
            SumDecimal=>$"أوجد {t}/١٠ + {h}/١٠٠. اكتب الناتج بالصورة العشرية.",
            RelevantSum=>$"استغرق نشاط {irrelevant} أيام. في أول يومين أتم الطالب {t}/١٠ و{h}/١٠٠ من العمل. كم جزءًا من مئة أنجز خلال اليومين؟",
            _=>$"استغرق نشاط {irrelevant} أيام. في أول يومين أتم الطالب {t}/١٠ و{h}/١٠٠ من العمل. اكتب الجزء المنجز خلال اليومين عددًا عشريًا."
        };
        return new Item(f,prompt,"حوّل الأعشار إلى أجزاء مئة، وأضف البسوط؛ المدة الكلية غير لازمة لحساب إنجاز اليومين.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out decimal answer)
    {
        answer=0m;
        if(!Supports(f)||p.Count!=3||!p.TryGetValue("tenths",out int t)||
            !p.TryGetValue("hundredths",out int h)||!p.TryGetValue("otherDays",out int days)||
            t is <0 or >9||h is <0 or >9||days is <2 or >9)return false;
        int sum=10*t+h;
        answer=f is SumDecimal or RelevantDecimal?sum/100m:sum;
        return true;
    }
    internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(f,p,out decimal result))throw new InvalidOperationException("Invalid decimal fraction sum");
        return result.ToString(f is SumDecimal or RelevantDecimal?"0.00":"0",CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch{
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '٫'=>'.', _=>c
        }).Where(c=>c is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(normalized.Length>20||!normalized.All(c=>c is >= '0' and <= '9' or '.'))return false;
        var style=f is SumDecimal or RelevantDecimal?NumberStyles.AllowDecimalPoint:NumberStyles.None;
        return decimal.TryParse(normalized,style,CultureInfo.InvariantCulture,out decimal actual)&&actual==expected;
    }
}
