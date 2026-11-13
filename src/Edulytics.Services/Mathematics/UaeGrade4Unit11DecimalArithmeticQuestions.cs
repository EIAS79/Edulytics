using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit11DecimalArithmeticQuestions
{
 private const string G5Product="uae.g5.u05.l02.model_decimal_product";
 private const string G5Hundredths="uae.g5.u05.l02.model_shaded_hundredths";
 private const string G5Wholes="uae.g5.u05.l02.model_complete_wholes";
 private const string WholeProduct="uae.g5.u05.l03.decimal_times_whole",UnitCost="uae.g5.u05.l03.unit_price_total",PointPlacement="uae.g5.u05.l03.decimal_point_placement";
 private static bool IsG5Whole(string? f)=>f is WholeProduct or UnitCost or PointPlacement;
 private static bool IsG5(string? f)=>f is G5Product or G5Hundredths or G5Wholes;
 internal static readonly HashSet<string> Families=[
  "uae.g4.u11.l04.base_ten_sum","uae.g4.u11.l04.regroup_unit",
  "uae.g4.u11.l05.model_sum","uae.g4.u11.l05.fractional_hundredths",
  "uae.g4.u11.l06.column_sum","uae.g4.u11.l06.whole_part",
  "uae.g4.u11.l07.commutative_sum","uae.g4.u11.l07.associative_sum",
  "uae.g4.u11.l08.base_ten_difference","uae.g4.u11.l08.regroup_unit",
  "uae.g4.u11.l09.model_difference","uae.g4.u11.l09.fractional_hundredths",
  "uae.g4.u11.l10.column_difference","uae.g4.u11.l10.check_addition"];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f!=null&&(Families.Contains(f)||IsG5(f)||IsG5Whole(f));
 private static bool IsSubtract(string f)=>f.Contains(".l08.")||f.Contains(".l09.")||f.Contains(".l10.");
 private static bool IsProperty(string f)=>f.Contains(".l07.");
 internal static Item Build(string f,Random random)
 {
   if(!Supports(f))throw new InvalidOperationException("Unsupported grade 4 decimal arithmetic");
   if(IsG5Whole(f))return BuildG5Whole(f,random);
   if(IsG5(f))return BuildG5(f,random);
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


 // Reuse decimal arithmetic engine: persist unscaled hundredths, never answers.
 private static Item BuildG5Whole(string f,Random rng)
 {
   int a=rng.Next(5,f==UnitCost?601:1000),b=rng.Next(2,f==UnitCost?21:71);
   string x=(a/100m).ToString("0.00",CultureInfo.InvariantCulture);
   string question=f switch
   {
     WholeProduct=>$"احسب {x} × {b}، ثم تحقق بالتقدير.",
     UnitCost=>$"ثمن القطعة {x} درهمًا. ما ثمن {b} قطع؟",
     _=>$"اضرب {a} في {b} كعددين صحيحين، ثم ضع الفاصلة لتجد {x} × {b}."
   };
   return new Item(f,question,"احسب بمواضع الأرقام الصحيحة ثم أعد منزلتين عشريتين.",
     AssessmentItemType.Numeric,new Dictionary<string,int>{{"hundredths",a},{"whole",b}});
 }
 private static bool ValidG5Whole(string f,IReadOnlyDictionary<string,int> p,out decimal answer)
 {
   answer=0m;
   if(!IsG5Whole(f)||p.Count!=2||!p.TryGetValue("hundredths",out int a)||
      !p.TryGetValue("whole",out int b)||a is <5 or >999||b is <2 or >70||
      (f==UnitCost&&(a>600||b>20)))return false;
   answer=(decimal)a*b/100m;
   return true;
 }
 private static bool VerifyG5Whole(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!ValidG5Whole(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
   string s=new(answer.Trim().Select(ch=>ch switch {
     >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
     >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
     '٫'=>'.',_=>ch
   }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
   if(s.Length>24||!s.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
   return decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,
     out decimal parsed)&&parsed*100m==(decimal)p["hundredths"]*p["whole"];
 }

 private static Item BuildG5(string f,Random rng)
 {
   int t=rng.Next(1,10),groups=rng.Next(2,6);
   string prompt=f switch
   {
     G5Product=>$"في {groups} شبكات ظلّل {t} أعشار في كل واحدة. ما ناتج {t}/10 × {groups} بالصورة العشرية؟",
     G5Hundredths=>$"ظلّل {t} أعشار في كل واحدة من {groups} شبكات المئة، ما إجمالي المربعات المظللة من أجزاء المئة؟",
     _=>$"في {groups} نماذج بكل منها {t} أعشار، كم وحدة كاملة تكونت من الأجزاء المظللة؟"
   };
   return new Item(f,prompt,"أجمع الأعشار وأُعيد تجميع كل عشرة أعشار إلى وحدة.",
       AssessmentItemType.Numeric,new Dictionary<string,int>{{"tenths",t},{"groups",groups}});
 }
 private static bool ValidG5(string f,IReadOnlyDictionary<string,int> p,out decimal expected)
 {
   expected=0m;
   if(!IsG5(f)||p.Count!=2||!p.TryGetValue("tenths",out int t)||
       !p.TryGetValue("groups",out int g)||t is <1 or >9||g is <2 or >5)return false;
   expected=f switch {G5Product=>t*g/10m,G5Hundredths=>t*g*10m,_=>(t*g)/10};
   return true;
 }
 private static bool VerifyG5(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!ValidG5(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
   string s=new(answer.Trim().Select(ch=>ch switch{
      >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
      >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
      '٫'=>'.',_=>ch
   }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
   if(!decimal.TryParse(s,f==G5Product?NumberStyles.AllowDecimalPoint:NumberStyles.None,
       CultureInfo.InvariantCulture,out decimal value))return false;
   int t=p["tenths"],g=p["groups"];
   return f switch{
     G5Product=>value*10m==t*g,
     G5Hundredths=>value==t*g*10m,
     _=>value==Math.Floor((t*g)/10m)};
 }

 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out decimal result)
 {
   result=0m;
   if(IsG5Whole(f))return ValidG5Whole(f,p,out result);
   if(IsG5(f))return ValidG5(f,p,out result);
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
   if(IsG5Whole(f))return VerifyG5Whole(f,p,answer);
   if(IsG5(f))return VerifyG5(f,p,answer);
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
