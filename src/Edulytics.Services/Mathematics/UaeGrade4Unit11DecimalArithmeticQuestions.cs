using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit11DecimalArithmeticQuestions
{
 private const string G5Product="uae.g5.u05.l02.model_decimal_product";
 private const string G5Hundredths="uae.g5.u05.l02.model_shaded_hundredths";
 private const string G5Wholes="uae.g5.u05.l02.model_complete_wholes";
 private const string WholeProduct="uae.g5.u05.l03.decimal_times_whole",UnitCost="uae.g5.u05.l03.unit_price_total",PointPlacement="uae.g5.u05.l03.decimal_point_placement";
 private const string AreaProduct="uae.g5.u05.l04.area_tenths_product",AreaCells="uae.g5.u05.l04.area_shaded_hundredths",AreaExtended="uae.g5.u05.l04.area_extended_product";
 private const string DecimalFactor="uae.g5.u05.l05.multiply_decimal_factors",DecimalRate="uae.g5.u05.l05.apply_decimal_price_rate",DecimalPoint="uae.g5.u05.l05.restore_decimal_point";
 private const string DecimalPow="uae.g5.u05.l06.decimal_times_power_ten",DecimalShift="uae.g5.u05.l06.decimal_shift_positions",DecimalPricePow="uae.g5.u05.l06.price_times_power_ten";
 private static bool IsG5Pow(string? f)=>f is DecimalPow or DecimalShift or DecimalPricePow;
 private const string DividePow="uae.g5.u05.l14.decimal_divide_power_ten",DivideShift="uae.g5.u05.l14.decimal_left_shift_places",DividePrice="uae.g5.u05.l14.equal_payment_decimal";
 private static bool IsG5DividePow(string? f)=>f is DividePow or DivideShift or DividePrice;
 private static bool IsG5Factor(string? f)=>f is DecimalFactor or DecimalRate or DecimalPoint;
 private static bool IsG5Area(string? f)=>f is AreaProduct or AreaCells or AreaExtended;
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
 internal static bool Supports(string? f)=>f!=null&&(Families.Contains(f)||IsG5(f)||IsG5Whole(f)||IsG5Area(f)||IsG5Factor(f)||IsG5Pow(f)||IsG5DividePow(f));
 private static bool IsSubtract(string f)=>f.Contains(".l08.")||f.Contains(".l09.")||f.Contains(".l10.");
 private static bool IsProperty(string f)=>f.Contains(".l07.");
 internal static Item Build(string f,Random random)
 {
   if(!Supports(f))throw new InvalidOperationException("Unsupported grade 4 decimal arithmetic");
   if(IsG5DividePow(f))return BuildG5DividePow(f,random);
   if(IsG5Pow(f))return BuildG5Pow(f,random);
   if(IsG5Factor(f))return BuildG5Factor(f,random);
   if(IsG5Area(f))return BuildG5Area(f,random);
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






 // UAE Grade5 U05 L14: inverse of existing exact decimal multiplication
 // by 10^k. Integer-scaled dividend preserves up to six decimal places.
 private static Item BuildG5DividePow(string f,Random random)
 {
     int exponent=random.Next(1,f==DividePrice?3:4);
     int valueThousandths;
     if(f==DividePrice)
     {
         int centsPerInstallment=random.Next(10,701);
         valueThousandths=checked(centsPerInstallment*G5TenPower(exponent)*10);
     }
     else valueThousandths=random.Next(1,999001);
     string amount=(valueThousandths/1000m).ToString("0.###",CultureInfo.InvariantCulture);
     int divisor=G5TenPower(exponent);
     string prompt=f switch
     {
         DivideShift=>$"عند قسمة {amount} على ١٠ أس {exponent}، كم منزلة تنتقل الفاصلة العشرية إلى اليسار؟",
         DividePrice=>$"قيمة فاتورة {amount} درهمًا، تُقسم إلى {divisor} دفعات متساوية. كم قيمة كل دفعة بالدرهم دون تقريب؟",
         _=>$"احسب {amount} ÷ ١٠ أس {exponent} دون تقريب. تحقق من إجابتك بالضرب."
     };
     return new Item(f,prompt,
         "القسمة على قوة عشرة تنقل الفاصلة إلى اليسار بعدد مراتب الأس؛ أضيف الأصفار الضرورية وأتحقق بالضرب.",
         AssessmentItemType.Numeric,new Dictionary<string,int>{{"thousandths",valueThousandths},{"pow",exponent}});
 }
 private static bool ValidG5DividePow(string f,IReadOnlyDictionary<string,int> p,out decimal expected)
 {
     expected=0m;
     if(!IsG5DividePow(f)||p.Count!=2||
         !p.TryGetValue("thousandths",out int v)||!p.TryGetValue("pow",out int exponent)||
         v is <1 or >999000||exponent is <1 or >3||
         (f==DividePrice&&(exponent>2||v% (G5TenPower(exponent)*10)!=0)))
         return false;
     expected=f==DivideShift?exponent:v/(1000m*G5TenPower(exponent));
     return true;
 }
 private static bool VerifyG5DividePow(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
     if(!ValidG5DividePow(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
     string normalized=new(answer.Trim().Select(ch=>ch switch
     {
         >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
         >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
         '٫'=>'.',_=>ch
     }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0' and not '\u202f').ToArray());
     if(normalized.Length>32||!normalized.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
     var style=f==DivideShift?NumberStyles.None:NumberStyles.AllowDecimalPoint;
     if(!decimal.TryParse(normalized,style,CultureInfo.InvariantCulture,out decimal answerValue))return false;
     return f==DivideShift?answerValue==p["pow"]:
         answerValue*1000m*G5TenPower(p["pow"])==p["thousandths"];
 }

 // Decimal 10^k reuses existing decimal engine with thousandth precision.
 private static int G5TenPower(int exponent)=>exponent switch{1=>10,2=>100,3=>1000,
   _=>throw new InvalidOperationException("Unsupported power of ten")};
 private static Item BuildG5Pow(string f,Random rng)
 {
   int value=rng.Next(1,f==DecimalPricePow?10000:99001),pow=rng.Next(1,4);
   string x=(value/1000m).ToString("0.###",CultureInfo.InvariantCulture);
   int baseTen=G5TenPower(pow);
   string prompt=f switch
   {
     DecimalShift=>$"عند ضرب {x} في ١٠ أس {pow}، كم منزلة تنتقل الفاصلة العشرية إلى اليمين؟",
     DecimalPricePow=>$"ثمن الوحدة {x} درهمًا. ما تكلفة {baseTen} وحدة متساوية؟",
     _=>$"أوجد {x} × ١٠ أس {pow} دون تقريب."
   };
   return new Item(f,prompt,"كل ضربة بعشرة تنقل الفاصلة منزلة واحدة يمينًا.",
      AssessmentItemType.Numeric,new Dictionary<string,int>{{"thousandths",value},{"pow",pow}});
 }
 private static bool ValidG5Pow(string f,IReadOnlyDictionary<string,int> p,out decimal expected)
 {
   expected=0m;
   if(!IsG5Pow(f)||p.Count!=2||!p.TryGetValue("thousandths",out int v)||
      !p.TryGetValue("pow",out int pow)||v is <1 or >99000||pow is <1 or >3||
      (f==DecimalPricePow&&v>9999))return false;
   expected=f==DecimalShift?pow:(decimal)v*G5TenPower(pow)/1000m;
   return true;
 }
 private static bool VerifyG5Pow(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!ValidG5Pow(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
   string s=new(answer.Trim().Select(ch=>ch switch
   {
      >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
      >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
      '٫'=>'.',_=>ch
   }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
   if(s.Length>24||!s.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
   if(!decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,
      out decimal v))return false;
   return f==DecimalShift?v==p["pow"]:
      v*1000m==(decimal)p["thousandths"]*G5TenPower(p["pow"]);
 }

 // Decimal multiplication reuses shared decimal engine. Persist hundredths
 // only, and reconstruct product without loss of up to four decimal places.
 private static Item BuildG5Factor(string family,Random rng)
 {
    int a=rng.Next(5,901),b=rng.Next(5,901);
    string left=(a/100m).ToString("0.00",CultureInfo.InvariantCulture);
    string right=(b/100m).ToString("0.00",CultureInfo.InvariantCulture);
    string prompt=family switch
    {
       DecimalFactor=>$"اضرب {left} × {right} دون تقريب وتحقق من موضع الفاصلة.",
       DecimalRate=>$"مقدار المادة المستهلكة {left} كيلوغرامًا لكل عبوة، واشترى شخص {right} عبوة مكافئة. ما الكمية الدقيقة بالكيلوغرام؟",
       _=>$"ناتج ضرب الأعداد الصحيحة {a} × {b} يساوي {a*b}. أعد الفاصلة العشرية لتجد {left} × {right} دون تقريب."
    };
    return new Item(family,prompt,"أضرب الأعداد الصحيحة ثم أعيد أربع مراتب عشرية قبل حذف الأصفار النهائية فقط.",
      AssessmentItemType.Numeric,new Dictionary<string,int>{{"leftHundredths",a},{"rightHundredths",b}});
 }
 private static bool ValidG5Factor(string f,IReadOnlyDictionary<string,int> p,out decimal product)
 {
    product=0m;
    if(!IsG5Factor(f)||p.Count!=2||
       !p.TryGetValue("leftHundredths",out int a)||
       !p.TryGetValue("rightHundredths",out int b)||
       a is <1 or >999||b is <1 or >999)return false;
    product=(decimal)a*b/10000m;
    return true;
 }
 private static bool VerifyG5Factor(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
    if(!ValidG5Factor(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
    string s=new(answer.Trim().Select(ch=>ch switch
    {
       >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
       >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
       '٫'=>'.',_=>ch
    }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
    if(s.Length>24||!s.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
    return decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,
       out decimal parsed)&&parsed*10000m==(decimal)p["leftHundredths"]*p["rightHundredths"];
 }

 // Area-grid model: preserve up to four decimal places (e.g. 0.11×0.5=0.055).
 private static Item BuildG5Area(string f,Random rng)
 {
    int a=f==AreaExtended?rng.Next(10,241):rng.Next(1,10)*10;
    int b=rng.Next(1,10)*10;
    var p=new Dictionary<string,int>{{"aHundredths",a},{"bHundredths",b}};
    string x=(a/100m).ToString("0.##",CultureInfo.InvariantCulture);
    string y=(b/100m).ToString("0.##",CultureInfo.InvariantCulture);
    string prompt=f switch
    {
       AreaCells=>$"في شبكة ١٠×١٠، ظلّل منطقة تمثل {x} × {y}. كم جزءًا من المئة يحتوي التقاطع المظلل؟",
       AreaExtended=>$"باستخدام شبكة الأعشار والمئة، أوجد مساحة النموذج {x} × {y} عددًا عشريًا.",
       _=>$"مثل {x} × {y} على شبكة المئة. ما ناتج الضرب في الصورة العشرية؟"
    };
    return new Item(f,prompt,"أضرب عدد الأجزاء ثم أحول من أجزاء من عشرة أو مئة إلى ناتج عشري.",
      AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Area(string f,IReadOnlyDictionary<string,int> p,out decimal expected)
 {
    expected=0m;
    if(!IsG5Area(f)||p.Count!=2||
      !p.TryGetValue("aHundredths",out int a)||!p.TryGetValue("bHundredths",out int b)||
      a is <10 or >240||b is <10 or >90||b%10!=0||
      (f!=AreaExtended&&(a>90||a%10!=0)))return false;
    expected=f==AreaCells?(decimal)a*b/100m:(decimal)a*b/10000m;
    return true;
 }
 private static bool VerifyG5Area(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
    if(!ValidG5Area(f,p,out _)||string.IsNullOrWhiteSpace(answer))return false;
    string s=new(answer.Trim().Select(ch=>ch switch{
      >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
      >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
      '٫'=>'.',_=>ch
    }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
    if(s.Length>24||!s.All(ch=>ch is >= '0' and <= '9' or '.'))return false;
    if(!decimal.TryParse(s,NumberStyles.AllowDecimalPoint,CultureInfo.InvariantCulture,
      out decimal v))return false;
    int a=p["aHundredths"],b=p["bHundredths"];
    return f==AreaCells?v*100m==(decimal)a*b:v*10000m==(decimal)a*b;
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
   // UAE G6 U03:L04 — source-scoped extension of existing Grade5 exact
   // decimal-factor multiplication; two millionth-scaled integers preserve
   // up to twelve fractional places without binary floating-point rounding.
   if (f=="uae.g5.u05.l05.multiply_decimal_factors" &&
       p.TryGetValue("g6DecimalFirstMillionths",out var first))
   {
       if (!p.TryGetValue("g6DecimalSecondMillionths",out var second) ||
           first is <1 or >2000000000 || second is <1 or >2000000000)
           return false;
       result=(decimal)first*second/1000000000000m;
       return true;
   }

   // UAE G6 Unit3 Exploration Lab: exact signed powers of ten, using
   // the pre-existing Grade5 family with this new lesson's isolated parameters.
   if (f=="uae.g5.u05.l06.decimal_times_power_ten" &&
       p.TryGetValue("g6PowerBaseMillionths",out var baseMillionths))
   {
       if (!p.TryGetValue("g6PowerExponent",out var exponent) ||
           baseMillionths is <1 or >1000000000 || exponent is <-4 or >7)
           return false;
       decimal scale=exponent switch
       {
           -4=>0.0001m,-3=>0.001m,-2=>0.01m,-1=>0.1m,
           0=>1m,1=>10m,2=>100m,3=>1000m,4=>10000m,
           5=>100000m,6=>1000000m,7=>10000000m,_=>0m
       };
       result=(decimal)baseMillionths*scale/1000000m;
       return true;
   }

   if (f=="uae.g5.u05.l03.decimal_times_whole" &&
       p.TryGetValue("g6DecimalFactorTenThousandths",out var n))
   {
       if (!p.TryGetValue("g6WholeFactor",out var whole) ||
           n is <1 or >2000000 || whole is <1 or >100)
           return false;
       result=(decimal)n*whole/10000m;
       return true;
   }

   if (f=="uae.g5.u05.l05.multiply_decimal_factors" &&
       p.TryGetValue("g6EstimateProduct",out var estimateMode) && estimateMode==1)
   {
       if (!p.TryGetValue("aTenths",out var a) || !p.TryGetValue("bTenths",out var b) ||
           !p.TryGetValue("placeA",out var placeA) || !p.TryGetValue("placeB",out var placeB) ||
           a is <1 or >10000 || b is <1 or >10000 ||
           placeA is not (1 or 10) || placeB is not (1 or 10))
           return false;
       var roundedA=Math.Round((a/10m)/placeA,0,MidpointRounding.AwayFromZero)*placeA;
       var roundedB=Math.Round((b/10m)/placeB,0,MidpointRounding.AwayFromZero)*placeB;
       result=roundedA*roundedB;
       return true;
   }

   if ((f is "uae.g4.u11.l06.column_sum" or "uae.g4.u11.l10.column_difference") &&
       p.TryGetValue("g6Thousandths",out var enabled) && enabled==1)
   {
       if (!p.TryGetValue("aThousandths",out var a) ||
           !p.TryGetValue("bThousandths",out var b) ||
           a is <0 or >999999 || b is <0 or >999999 ||
           (f=="uae.g4.u11.l10.column_difference" && b>a))
           return false;
       result=(f=="uae.g4.u11.l10.column_difference"? a-b : a+b)/1000m;
       return true;
   }

   if(IsG5DividePow(f))return ValidG5DividePow(f,p,out result);
   if(IsG5Pow(f))return ValidG5Pow(f,p,out result);
   if(IsG5Factor(f))return ValidG5Factor(f,p,out result);
   if(IsG5Area(f))return ValidG5Area(f,p,out result);
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
    return ans.ToString(p.ContainsKey("g6PowerBaseMillionths")?"0.##########":
       p.ContainsKey("g6DecimalFirstMillionths")?"0.############":
       p.ContainsKey("g6DecimalFactorTenThousandths")?"0.####":
       p.TryGetValue("g6Thousandths",out var enabled) && enabled==1 ?
       "0.###" : IsG5DividePow(f)?"0.######":(IsG5Area(f)||IsG5Factor(f))?"0.####":"0.##",CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if (p.ContainsKey("g6PowerBaseMillionths"))
   {
       if(!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
       var normalized=new string(answer.Trim().Select(c=>c switch{
           >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
           >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
           '٫'=>'.',_=>c
       }).ToArray());
       if(normalized.Length>35||
          !normalized.All(c=>c is >= '0' and <= '9' or '.'))return false;
       return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
           CultureInfo.InvariantCulture,out var actual)&&actual==expected;
   }
   if (p.ContainsKey("g6DecimalFirstMillionths"))
   {
       if (!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))
           return false;
       string normalized=new(answer.Trim().Select(c=>c switch
       {
           >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
           >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
           '٫'=>'.',_=>c
       }).ToArray());
       if (normalized.Length>35 ||
           !normalized.All(c=>c is >= '0' and <= '9' or '.'))
           return false;
       return decimal.TryParse(normalized,NumberStyles.AllowDecimalPoint,
           CultureInfo.InvariantCulture,out var actual)&&actual==expected;
   }
   if (p.ContainsKey("g6DecimalFactorTenThousandths"))
   {
       if(!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
       string western=new(answer.Trim().Select(c=>c switch {
           >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
           >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
           '٫'=>'.',_=>c
       }).ToArray());
       return decimal.TryParse(western,NumberStyles.AllowDecimalPoint|NumberStyles.AllowLeadingSign,
           CultureInfo.InvariantCulture,out var actual)&&actual==expected;
   }
   if (p.TryGetValue("g6EstimateProduct",out var g6) && g6==1)
   {
       if(!Valid(f,p,out decimal expected)||string.IsNullOrWhiteSpace(answer))return false;
       string western=new(answer.Trim().Select(c=>c switch{
           >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
           >= '۰' and <= '۹'=>(char)('0'+c-'۰'),
           _=>c
       }).ToArray());
       return decimal.TryParse(western,NumberStyles.Number,CultureInfo.InvariantCulture,out var actual)
           && actual==expected;
   }
   if(IsG5DividePow(f))return VerifyG5DividePow(f,p,answer);
   if(IsG5Pow(f))return VerifyG5Pow(f,p,answer);
   if(IsG5Factor(f))return VerifyG5Factor(f,p,answer);
   if(IsG5Area(f))return VerifyG5Area(f,p,answer);
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
