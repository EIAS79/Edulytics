using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit14MetricMeasurementQuestions
{
 internal const string TimeMinutes="uae.g4.u14.l01.time_minutes";
 internal const string TimeWithMinutes="uae.g4.u14.l01.time_with_minutes";
 internal const string DotsTotal="uae.g4.u14.l02.dot_plot_total";
 internal const string DotsMost="uae.g4.u14.l02.dot_plot_most";
 internal const string GuessMultiply="uae.g4.u14.l03.guess_check_product";
 internal const string GuessRemaining="uae.g4.u14.l03.guess_check_remaining";
 internal const string LengthCm="uae.g4.u14.l04.meters_centimeters";
 internal const string LengthMm="uae.g4.u14.l04.centimeters_millimeters";
 internal const string CapacityMl="uae.g4.u14.l05.liters_milliliters";
 internal const string CapacityCombined="uae.g4.u14.l05.combined_milliliters";
 internal const string MassGrams="uae.g4.u14.l06.kilograms_grams";
 internal const string MassCombined="uae.g4.u14.l06.combined_grams";
 internal const string ListPairs="uae.g4.u14.l07.organized_list_pairs";
 internal const string ListTriples="uae.g4.u14.l07.organized_list_triplets";
 internal const string ConvertMm="uae.g4.u14.l08.convert_mm";
 internal const string ConvertCm="uae.g4.u14.l08.convert_cm";
 internal const string MeasurementTotal="uae.g4.u14.l09.measurement_total_ml";
 internal const string MeasurementTime="uae.g4.u14.l09.measurement_time_minutes";
 internal static readonly HashSet<string> Families=[TimeMinutes,TimeWithMinutes,DotsTotal,DotsMost,GuessMultiply,
  GuessRemaining,LengthCm,LengthMm,CapacityMl,CapacityCombined,MassGrams,MassCombined,ListPairs,ListTriples,ConvertMm,ConvertCm,
  MeasurementTotal,MeasurementTime];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f!=null&&Families.Contains(f);
 internal static Item Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported metric measurement family");
  int a=random.Next(2,12),b=random.Next(2,12),c=random.Next(1,Math.Min(10,a*b));
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  string prompt=f switch
  {
   TimeMinutes=>$"استغرقت رحلة {a} ساعات. كم دقيقة في الرحلة؟",
   TimeWithMinutes=>$"استغرق نشاط {a} ساعات و{b} دقائق. ما المدة الكلية بالدقائق؟",
   DotsTotal=>$"في مخطط نقاط مجمعة ظهرت ثلاث فئات بعدد {a}، {b}، {c} من النقاط. كم عدد النقاط كلها؟",
   DotsMost=>$"في مخطط نقاط مجمعة ظهرت ثلاث فئات بتكرارات {a}، {b}، {c}. ما أعلى تكرار؟",
   GuessMultiply=>$"رتب التلاميذ {a} صفوف، في كل صف {b} طلاب. كم طالبًا؟ استخدم التخمين والتحقق.",
   GuessRemaining=>$"هناك {a} مجموعات فيها {b} عناصر لكل مجموعة، واستُخدم {c} عناصر. كم يتبقى؟",
   LengthCm=>$"طول شريط {a} أمتار. كم سنتيمترًا؟",
   LengthMm=>$"طول قلم {a} سنتيمترات. كم ملليمترًا؟",
   CapacityMl=>$"كم ملليلترًا في {a} لترات؟",
   CapacityCombined=>$"لديك {a} لترات و{b} ملليلترات. كم يساوي ذلك بالملليلتر؟",
   MassGrams=>$"كم جرامًا في {a} كيلوجرامات؟",
   MassCombined=>$"كتلة حقيبة {a} كيلوجرامات و{b} جرامات. ما الكتلة الكلية بالجرام؟",
   ListPairs=>$"اختر وجبة من {a} أنواع طعام و{b} أنواع شراب. كم اختيارًا ممكنًا في قائمة منظمة؟",
   ListTriples=>$"يمكن اختيار {a} ألوان و{b} أحجام و{c} أشكال. كم توليفة ممكنة؟",
   ConvertMm=>$"حوّل {a} سنتيمترات إلى ملليمترات.",
   ConvertCm=>$"حوّل {a} أمتار إلى سنتيمترات.",
   MeasurementTotal=>$"خزانتان للسعة: الأولى {a} لترات والثانية {b} ملليلترات. ما مجموع سعتيهما بالملليلتر؟",
   _=>$"تستغرق مهمة {a} ساعات و{b} دقائق. اكتب إجمالي المدة بالدقائق."
  };
  return new Item(f,prompt,"حدد الوحدة ومعدل التحويل ثم اضرب أو اجمع أو اطرح بعد توحيد الوحدات، وتحقق من معقولية الناتج.",
   AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int ans)
 {
  ans=0;
  if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
    !p.TryGetValue("c",out int c)||a is <1 or >100||b is <1 or >100||c is <1 or >100||
    (f==GuessRemaining&&c>a*b))return false;
  ans=f switch
  {
   TimeMinutes=>a*60,
   TimeWithMinutes or MeasurementTime=>a*60+b,
   DotsTotal=>a+b+c,
   DotsMost=>Math.Max(a,Math.Max(b,c)),
   GuessMultiply or ListPairs=>a*b,
   GuessRemaining=>a*b-c,
   LengthCm or ConvertCm=>a*100,
   LengthMm or ConvertMm=>a*10,
   CapacityMl or MassGrams=>a*1000,
   CapacityCombined or MassCombined or MeasurementTotal=>a*1000+b,
   ListTriples=>a*b*c,
   _=>throw new InvalidOperationException("Unknown metric family")
  };
  return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid metric values");
  return ans.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string normalized=new(answer.Trim().Select(c=>c switch{
    >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
    >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
  }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
  return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
