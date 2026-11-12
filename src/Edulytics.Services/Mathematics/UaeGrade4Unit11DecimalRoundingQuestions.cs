using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit11DecimalRoundingQuestions
{
 internal const string Whole="uae.g4.u11.l01.round_whole";
 internal const string Tenth="uae.g4.u11.l01.round_tenth";
 internal const string Hundredth="uae.g4.u11.l01.round_hundredth";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? family)=>family is Whole or Tenth or Hundredth;
 internal static Item Build(string family,Random random)
 {
    if(!Supports(family))throw new InvalidOperationException("Unsupported decimal rounding family");
    int thousandths=random.Next(0,999999);
    var p=new Dictionary<string,int>{{"thousandthsValue",thousandths}};
    string num=(thousandths/1000m).ToString("0.000",CultureInfo.InvariantCulture);
    string dest=family switch{Whole=>"عدد كلي",Tenth=>"عُشر",_=>"جزء من مئة"};
    return new Item(family,$"قرب {num} إلى أقرب {dest}. اكتب العدد العشري الناتج.",
        "انظر إلى المنزلة المجاورة يمين منزلة التقريب؛ إذا بلغت خمسة فارفع المنزلة وإلا فاحتفظ بها.",
        AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out decimal result)
 {
    result=0m;
    if(!Supports(f)||p.Count!=1||!p.TryGetValue("thousandthsValue",out int n)||n<0||n>999999)return false;
    int digits=f switch{Whole=>0,Tenth=>1,_=>2};
    result=Math.Round(n/1000m,digits,MidpointRounding.AwayFromZero);
    return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
    if(!Valid(f,p,out decimal result))throw new InvalidOperationException("Invalid decimal-rounding input");
    return result.ToString(f switch{Whole=>"0",Tenth=>"0.0",_=>"0.00"},CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
    if(!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
    string normalized=new(answer.Trim().Select(c=>c switch
    {
       >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
       >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
       '٫'=>'.',_=>c
    }).Where(c=>c is not ' ' and not '٬' and not '\u00a0').ToArray());
    if(normalized.Length>20||!normalized.All(c=>c is >= '0' and <= '9' or '.'))return false;
    return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal actual)&&actual==expected;
 }
}
