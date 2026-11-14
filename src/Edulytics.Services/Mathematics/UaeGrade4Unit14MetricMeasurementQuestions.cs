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
 internal static bool Supports(string? f)=>f!=null&&(Families.Contains(f)||IsG5(f));
 internal static Item Build(string f,Random random)
 {
  if(IsG5(f))return BuildG5(f,random);
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
  if(IsG5(f))return SolveG5(f,p);
  if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid metric values");
  return ans.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
  if(IsG5(f))return VerifyG5(f,p,answer);
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string normalized=new(answer.Trim().Select(c=>c switch{
    >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
    >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
  }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
  return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
// UAE G5 Common U10: extend the existing metric/line-plot exact families,
// with Grade 5 decimals and teacher-reviewed physical measurement tasks.
private const string G5PlotCount = "uae.g5.u10.l01.fraction_plot_count";
private const string G5PlotTotal = "uae.g5.u10.l01.fraction_plot_total";
private const string G5RulerCm = "uae.g5.u10.l02.ruler_nearest_centimeter";
private const string G5RulerMm = "uae.g5.u10.l02.ruler_length_millimeters";
private const string G5LengthCm = "uae.g5.u10.l03.meters_tenths_to_centimeters";
private const string G5LengthM = "uae.g5.u10.l03.millimeters_to_meters";
private const string G5MassDiff = "uae.g5.u10.l04.mass_difference_kg_g";
private const string G5MassG = "uae.g5.u10.l05.kilograms_tenths_to_grams";
private const string G5MassKg = "uae.g5.u10.l05.grams_to_kilograms";
private const string G5CapMl = "uae.g5.u10.l06.liters_tenths_to_milliliters";
private const string G5CapL = "uae.g5.u10.l06.milliliters_to_liters";
private const string G5LogicalRemaining = "uae.g5.u10.l07.logical_remaining";
private const string G5LogicalOriginal = "uae.g5.u10.l07.logical_original_total";

private static bool IsG5(string? family) =>
    family is G5PlotCount or G5PlotTotal or G5RulerCm or G5RulerMm or
      G5LengthCm or G5LengthM or G5MassDiff or G5MassG or G5MassKg or
      G5CapMl or G5CapL or G5LogicalRemaining or G5LogicalOriginal;

private static Item BuildG5(string family, Random rng)
{
    int a=rng.Next(2,10), b=rng.Next(1,10), c=rng.Next(1,10);
    // These legacy family IDs retain stable registry routing; generated tasks now use source-aligned logical clues.
    var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
    string question=family switch
    {
        G5PlotCount=>$"تظهر ١/٤ متر على مخطط نقاط بطول {a} علامات، و١/٣ متر بعدد {b} علامات و١/٢ متر بعدد {c} علامات. كم علامة عند ١/٤ متر؟",
        G5PlotTotal=>$"مخطط قياسات يضم {a} علامات فوق ١/٤ و{b} فوق ١/٣ و{c} فوق ١/٢. كم عدد القياسات كلها؟",
        G5RulerCm=>$"يمتد جسم من صفر المسطرة إلى {a} سنتيمترًا و{b} ملليمترات. ما طوله مقربًا إلى أقرب سنتيمتر؟",
        G5RulerMm=>$"ما طول جسم بالسنتيمترات والملليمترات: {a} سم و{b} ملم، إذا كتبنا الطول بالملليمترات فقط؟",
        G5LengthCm=>$"حوّل {a} أمتار و{b} أعشار المتر إلى سنتيمترات.",
        G5LengthM=>$"حوّل {a*1000+b*10} ملليمتر إلى متر. اكتب عددًا عشريًا.",
        G5MassDiff=>$"كتلة حقيبة {a} كيلوجرامات، وكتلة علبة {b*100} جرام. كم جرامًا تزيد كتلة الحقيبة عن العلبة؟",
        G5MassG=>$"حوّل {a} كيلوجرامات و{b} أعشار الكيلوجرام إلى جرامات.",
        G5MassKg=>$"حوّل {a*1000+b*10} جرامًا إلى كيلوجرامات بالعدد العشري.",
        G5CapMl=>$"حوّل {a} لترات و{b} أعشار اللتر إلى ملليلترات.",
        G5CapL=>$"حوّل {a*1000+b*10} ملليلتر إلى لترات بالعدد العشري.",
        G5LogicalRemaining=>$"ثلاث أشجار ارتفاعاتها {a*100+b} و{a*100+b+c} و{a*100+b+2*c} سنتيمترًا. شجرة مريم أطول من شجرة سارة وأقصر من شجرة هدى. ما ارتفاع شجرة مريم بالسنتيمتر؟ نظّم المعطيات واستبعد القيم غير الممكنة.",
        G5LogicalOriginal=>$"أعمار ثلاث أشجار هي {a} و{a+b} و{a+b+c} سنوات. شجرة النخيل أكبر عمرًا من شجرة الليمون، والليمون أكبر من شجرة الزيتون. كم عمر شجرة النخيل؟ ضع الأعمار في جدول من الأصغر إلى الأكبر.",
        _=>throw new InvalidOperationException("Unsupported G5 metric family.")
    };
    return new Item(family,question,
        "أقرأ الوحدات أو التكرارات، أستعمل الجدول الصحيح، وأتحقق بعلاقة عكسية أو بعملية مكافئة.",
        family is G5LengthM or G5MassKg or G5CapL ? AssessmentItemType.Numeric :
        AssessmentItemType.Numeric,p);
}

private static bool TryG5Answer(string family,IReadOnlyDictionary<string,int> p,
    out decimal expected)
{
    expected=0m;
    if(!IsG5(family)||p.Count!=3||!p.TryGetValue("a",out int a)||
       !p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
       a is <2 or >9||b is <1 or >9||c is <1 or >9)return false;
    expected=family switch
    {
        G5PlotCount=>a,
        G5PlotTotal=>a+b+c,
        G5RulerCm=>(a*10+b+5)/10,
        G5RulerMm=>a*10+b,
        G5LengthCm=>a*100+b*10,
        G5LengthM=>a+b/100m,
        G5MassDiff=>a*1000-b*100,
        G5MassG=>a*1000+b*100,
        G5MassKg=>a+b/100m,
        G5CapMl=>a*1000+b*100,
        G5CapL=>a+b/100m,
        G5LogicalRemaining=>a*100+b+c,
        G5LogicalOriginal=>a+b+c,
        _=>throw new InvalidOperationException("Unsupported G5 metric family.")
    };
    return expected>=0m;
}

private static string SolveG5(string family,IReadOnlyDictionary<string,int> p)
{
    if(!TryG5Answer(family,p,out decimal expected))
        throw new InvalidOperationException("Invalid G5 metric parameters.");
    return expected.ToString(family is G5LengthM or G5MassKg or G5CapL?
        "0.00":"0",CultureInfo.InvariantCulture);
}

private static bool VerifyG5(string family,IReadOnlyDictionary<string,int> p,string? answer)
{
    if(!TryG5Answer(family,p,out decimal expected)||
       string.IsNullOrWhiteSpace(answer)||answer.Length>35)return false;
    string normalized=new(answer.Trim().Select(ch=>ch switch
    {
        >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
        >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
        '٫'=>'.',
        _=>ch
    }).Where(ch=>!char.IsWhiteSpace(ch)&&ch is not ',' and not '٬').ToArray());
    var style=family is G5LengthM or G5MassKg or G5CapL
        ? NumberStyles.AllowDecimalPoint : NumberStyles.None;
    return decimal.TryParse(normalized,style,CultureInfo.InvariantCulture,out decimal provided)
        &&provided==expected;
}

}
