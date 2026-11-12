using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit10DecimalPlaceValueQuestions
{
    internal const string PlaceValueDecimal="uae.g4.u10.l01.place_value_decimal";
    internal const string HundredthsCount="uae.g4.u10.l01.hundredths_count";
    internal const string TenthsDigit="uae.g4.u10.l01.tenths_digit";
    internal const string TenthsDecimal="uae.g4.u10.l02.tenths_decimal";
    internal const string TenthsCount="uae.g4.u10.l02.tenths_count";
    internal const string HundredthsDecimal="uae.g4.u10.l03.hundredths_decimal";
    internal const string HundredthsFraction="uae.g4.u10.l03.hundredths_fraction_numerator";
    internal const string DecimalToHundredths="uae.g4.u10.l04.decimal_to_hundredths";
    internal const string DecimalToTenths="uae.g4.u10.l04.decimal_to_tenths";
    internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    internal static bool Supports(string? family)=>family is PlaceValueDecimal or HundredthsCount or TenthsDigit
        or TenthsDecimal or TenthsCount or HundredthsDecimal or HundredthsFraction
        or DecimalToHundredths or DecimalToTenths;
    private static bool IsDecimal(string family)=>family is PlaceValueDecimal or TenthsDecimal or HundredthsDecimal;
    internal static Item Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported decimal place-value family");
        int whole=family is PlaceValueDecimal or HundredthsCount or TenthsDigit ? random.Next(0,5):0;
        int tenths=random.Next(0,10);
        int hundredths=family is TenthsDecimal or TenthsCount or DecimalToTenths?0:random.Next(0,10);
        var p=new Dictionary<string,int>{{"whole",whole},{"tenths",tenths},{"hundredths",hundredths}};
        string prompt=family switch
        {
            PlaceValueDecimal=>$"اكتب العدد الذي يحتوي على {whole} آحاد و{tenths} أعشار و{hundredths} أجزاء من مئة بصيغة عشرية.",
            HundredthsCount=>$"كم جزءًا من مئة في {whole} آحاد و{tenths} أعشار و{hundredths} أجزاء من مئة؟",
            TenthsDigit=>$"ما رقم منزلة الأعشار في عدد مكوّن من {whole} آحاد و{tenths} أعشار و{hundredths} أجزاء من مئة؟",
            TenthsDecimal=>$"اكتب {tenths} أعشار في صورة عدد عشري.",
            TenthsCount=>$"كم جزءًا من عشرة في {tenths} أعشار؟",
            HundredthsDecimal=>$"اكتب {tenths*10+hundredths} جزءًا من مئة في صورة عدد عشري.",
            HundredthsFraction=>$"ما بسط الكسر الذي مقامه ١٠٠ ويمثل {tenths} أعشار و{hundredths} أجزاء من مئة؟",
            DecimalToHundredths=>$"كم جزءًا من مئة في العدد ٠٫{tenths}{hundredths}؟",
            _=>$"اكتب ٠٫{tenths} على صورة كسر مقامه ١٠. ما البسط؟"
        };
        return new Item(family,prompt,"ضع كل رقم في منزلته: آحاد، أعشار، أجزاء مئة؛ كل عشر يساوي عشرة أجزاء من مئة.",
            AssessmentItemType.Numeric,p);
    }
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p,out decimal answer)
    {
        answer=0m;
        if(!Supports(family)||p.Count!=3||!p.TryGetValue("whole",out int whole)||
           !p.TryGetValue("tenths",out int t)||!p.TryGetValue("hundredths",out int h)||
           whole is <0 or >6||t is <0 or >9||h is <0 or >9||
           (family is TenthsDecimal or TenthsCount or DecimalToTenths && h!=0)||
           (family is HundredthsDecimal or HundredthsFraction or DecimalToHundredths or DecimalToTenths && whole!=0))return false;
        answer=family switch
        {
            PlaceValueDecimal=>whole+t/10m+h/100m,
            HundredthsCount=>whole*100+t*10+h,
            TenthsDigit=>t,
            TenthsDecimal=>t/10m,
            TenthsCount=>t,
            HundredthsDecimal=>t/10m+h/100m,
            HundredthsFraction or DecimalToHundredths=>t*10+h,
            _=>t
        };
        return true;
    }
    internal static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p,out decimal result))throw new InvalidOperationException("Invalid decimal parameters");
        string fmt=family switch {TenthsDecimal=>"0.0",PlaceValueDecimal or HundredthsDecimal=>"0.00",_=>"0"};
        return result.ToString(fmt,CultureInfo.InvariantCulture);
    }
    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
        string normalized=new(answer.Trim().Select(c=>c switch
        {
            >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
            '٫'=>'.',
            _=>c
        }).Where(c=>c is not ' ' and not '٬' and not '\u00a0').ToArray());
        if(normalized.Length>20||!normalized.All(c=>c is >= '0' and <= '9' or '.'))return false;
        var style=IsDecimal(family)?NumberStyles.AllowDecimalPoint:NumberStyles.None;
        return decimal.TryParse(normalized,style,CultureInfo.InvariantCulture,out decimal actual)&&actual==expected;
    }
}
