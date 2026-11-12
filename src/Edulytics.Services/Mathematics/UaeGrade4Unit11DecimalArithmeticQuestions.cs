using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit11DecimalArithmeticQuestions
{
 internal static readonly HashSet<string> Families=[
  "uae.g4.u11.l04.base_ten_sum","uae.g4.u11.l04.regroup_unit",
  "uae.g4.u11.l05.model_sum","uae.g4.u11.l05.fractional_hundredths",
  "uae.g4.u11.l06.column_sum","uae.g4.u11.l06.whole_part",
  "uae.g4.u11.l07.commutative_sum","uae.g4.u11.l07.associative_sum",
  "uae.g4.u11.l08.base_ten_difference","uae.g4.u11.l08.regroup_unit",
  "uae.g4.u11.l09.model_difference","uae.g4.u11.l09.fractional_hundredths",
  "uae.g4.u11.l10.column_difference","uae.g4.u11.l10.check_addition"];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f!=null&&Families.Contains(f);
 private static bool IsSubtract(string f)=>f.Contains(".l08.")||f.Contains(".l09.")||f.Contains(".l10.");
 private static bool IsProperty(string f)=>f.Contains(".l07.");
 internal static Item Build(string f,Random random)
 {
   if(!Supports(f))throw new InvalidOperationException("Unsupported grade 4 decimal arithmetic");
   int a=random.Next(125,1500);
   int b=random.Next(25,IsSubtract(f)?a:1500);
   int c=IsProperty(f)?random.Next(10,600):0;
   var p=new Dictionary<string,int>{{"aHundredths",a},{"bHundredths",b},{"cHundredths",c}};
   string x=(a/100m).ToString("0.00",CultureInfo.InvariantCulture);
   string y=(b/100m).ToString("0.00",CultureInfo.InvariantCulture);
   string z=(c/100m).ToString("0.00",CultureInfo.InvariantCulture);
   string relation=IsSubtract(f)?"−":"+";
   string expression=IsProperty(f)?$"{x} + {y} + {z}":$"{x} {relation} {y}";
   string question=f switch
   {
     "uae.g4.u11.l04.regroup_unit"=>$"عند جمع {x} و{y} بمكعبات أجزاء المئة، كم وحدة كاملة تنتج عن إعادة تجميع الأجزاء الكسرية؟ اكتب ٠ أو ١.",
     "uae.g4.u11.l08.regroup_unit"=>$"عند طرح {y} من {x} باستخدام مكعبات العد العشري، هل تحتاج إلى فك وحدة كاملة من الجزء الكسري؟ اكتب ١ للنعم و٠ للا.",
     "uae.g4.u11.l05.fractional_hundredths" or "uae.g4.u11.l09.fractional_hundredths"=>$"احسب {expression}. ما بسط الجزء الكسري المتبقي إذا كان مقامه ١٠٠ بعد فصل الوحدات الكاملة؟",
     "uae.g4.u11.l06.whole_part"=>$"احسب {expression}. كم وحدة كاملة في المجموع؟",
     "uae.g4.u11.l10.check_addition"=>$"احسب الفرق {expression} ثم اجمع الفرق مع المطروح. اكتب العدد الذي تستعيده.",
     _=>$"احسب {expression} مع محاذاة الفاصلة العشرية. اكتب الناتج."
   };
   return new Item(f,question,"مثل العددين في المنازل المتناظرة واحسب بالأجزاء من مئة، ثم أعد تجميع الوحدة عند الحاجة.",AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out decimal result)
 {
   result=0m;
   if(!Supports(f)||p.Count!=3||!p.TryGetValue("aHundredths",out int a)||
      !p.TryGetValue("bHundredths",out int b)||!p.TryGetValue("cHundredths",out int c)||
      a is <0 or >20000||b is <0 or >20000||c is <0 or >20000||
      (!IsProperty(f)&&c!=0)||(IsSubtract(f)&&b>a))return false;
   int sum=IsSubtract(f)?a-b:a+b+c;
   result=f switch
   {
     "uae.g4.u11.l04.regroup_unit"=>(a%100+b%100)/100,
     "uae.g4.u11.l08.regroup_unit"=>a%100<b%100?1:0,
     "uae.g4.u11.l05.fractional_hundredths" or "uae.g4.u11.l09.fractional_hundredths"=>sum%100,
     "uae.g4.u11.l06.whole_part"=>sum/100,
     "uae.g4.u11.l10.check_addition"=>a/100m,
     _=>sum/100m
   };
   return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
    if(!Valid(f,p,out decimal ans))throw new InvalidOperationException("Invalid decimal arithmetic parameters");
    return ans.ToString("0.##",CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(c=>c switch{
      >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
      >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
      '٫'=>'.',_=>c
   }).Where(c=>c is not ' ' and not '٬' and not '\u00a0').ToArray());
   if(normalized.Length>20||!normalized.All(c=>c is >= '0' and <= '9' or '.'))return false;
   return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,out decimal actual)&&actual==expected;
 }
}
