using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade3 measurements/time numeric families. Estimation, tool use and clock drawing are teacher assessed.
// Tests written, not run during stage A-B-C.
internal static class UaeGrade3Unit11Questions
{
 internal const string CapMeasure="uae.g3.u11.l01.capacity_units";
 internal const string CapWord="uae.g3.u11.l02.capacity_word_problem";
 internal const string MassMeasure="uae.g3.u11.l03.mass_units";
 internal const string MassWord="uae.g3.u11.l04.mass_word_problem";
 internal const string Time="uae.g3.u11.l05.time_to_minute";
 internal const string Elapsed="uae.g3.u11.l06.elapsed_time";
 internal const string Back="uae.g3.u11.l07.work_backward";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is CapMeasure or CapWord or MassMeasure or MassWord or Time or Elapsed or Back;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 measurement family");
  int a=0,b=0,mode=rng.Next(2),result=0;string prompt,solution;
  switch(f)
  {
   case CapMeasure:
    a=rng.Next(1,6);b=rng.Next(0,10)*100;result=a*1000+b;
    prompt=$"كم ملليلترًا في {a} لتر و{b} ملليلتر؟";
    solution="أحول اللترات إلى ملليلترات بضرب عدد اللترات في ١٠٠٠، ثم أجمع الباقي.";break;
   case CapWord:
    a=rng.Next(1,6);b=rng.Next(1,10)*100;
    result=mode==0?a*1000+b:a*1000-b;
    prompt=mode==0?$"لدى البائع {a} لترات عصير وأضاف {b} ملليلترًا؛ ما الحجم الإجمالي بالملليلترات؟":
     $"يوجد {a} لترات من الماء؛ استُهلك {b} ملليلترًا. كم ملليلترًا تبقى؟";
    solution="أوحد الوحدات إلى ملليلترات قبل الجمع أو الطرح.";break;
   case MassMeasure:
    a=rng.Next(1,9);b=rng.Next(0,10)*100;result=a*1000+b;
    prompt=$"ما كتلة {a} كيلوجرام و{b} جرام بوحدة الجرام؟";
    solution="أحول الكيلوجرام إلى ألف جرام، ثم أضيف الجرامات الباقية.";break;
   case MassWord:
    a=rng.Next(2,11);b=rng.Next(1,11)*50;result=a*b;
    prompt=mode==0?$"تزن الحقيبة {b} جرامًا، ما كتلة {a} حقائب متماثلة بالجرامات؟":
     $"يوجد {a} صناديق، كتلة كل صندوق {b} جرامًا. ما كتلتها الإجمالية؟";
    solution="أضرب عدد الأجسام في كتلة الواحد، مع الحفاظ على وحدة الجرام.";break;
   case Time:
    a=rng.Next(0,12);b=rng.Next(0,60);
    result=mode==0?a*60+b:b;
    prompt=mode==0?$"على الساعة {(a==0?12:a)}:{b:00} بعد الظهر. كم دقيقة مرت منذ ١٢:٠٠ ظهرًا؟":
     $"الساعة الآن {(a==0?12:a)}:{b:00}. كم دقيقة مرت منذ بداية هذه الساعة؟";
    solution="أقرأ عقرب الساعات ثم عقرب الدقائق، وأحسب كل ساعة ٦٠ دقيقة عند الحاجة.";break;
   case Elapsed:
    a=rng.Next(0,11);b=rng.Next(1,13);result=b*5;
    int from=8*60+a*5,to=from+b*5;
    prompt=mode==0?$"بدأ النشاط عند {from/60}:{from%60:00} وانتهى عند {to/60}:{to%60:00}. كم دقيقة استغرق؟":
     $"غادر التلميذ عند {from/60}:{from%60:00} ووصل عند {to/60}:{to%60:00}. ما الزمن المنقضي بالدقائق؟";
    solution="أمثل بداية الوقت ونهايته على خط زمني وأعد الفترات بالدقائق؛ الساعة ٦٠ دقيقة.";break;
   case Back:
    a=rng.Next(2,11);b=rng.Next(1,10);result=a*10+b;
    prompt=mode==0?$"بعد أن أنفق التلميذ {b} دراهم بقي معه {a*10} دراهم. كم درهمًا كان معه أولًا؟":
      $"أخرجت {b} أقلام من حقيبة فبقي فيها {a*10} قلمًا. كم قلمًا كان فيها قبل ذلك؟";
    solution="أبدأ من النهاية وأعكس الطرح إلى جمع لأجد الكمية الابتدائية.";break;
   default:throw new InvalidOperationException("Unsupported");
  }
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,
   new Dictionary<string,int>{{"a",a},{"b",b},{"mode",mode},{"result",result}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
    !p.TryGetValue("mode",out int mode)||!p.TryGetValue("result",out int got)||
    mode<0||mode>1)return false;
  bool ok=f switch{
   CapMeasure=>a>=1&&a<=5&&b>=0&&b<=900&&b%100==0,
   CapWord=>a>=1&&a<=5&&b>=100&&b<=900&&b%100==0,
   MassMeasure=>a>=1&&a<=8&&b>=0&&b<=900&&b%100==0,
   MassWord=>a>=2&&a<=10&&b>=50&&b<=500&&b%50==0,
   Time=>a>=0&&a<=11&&b>=0&&b<=59,
   Elapsed=>a>=0&&a<=10&&b>=1&&b<=12,
   Back=>a>=2&&a<=10&&b>=1&&b<=9,
   _=>false};
  if(!ok)return false;
  expected=f switch{
   CapMeasure or MassMeasure=>a*1000+b,
   CapWord=>mode==0?a*1000+b:a*1000-b,
   MassWord=>a*b,
   Time=>mode==0?a*60+b:b,
   Elapsed=>b*5,
   Back=>a*10+b,
   _=>throw new InvalidOperationException("Unsupported")};
  return got==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int val))throw new InvalidOperationException("Invalid G3 U11 numeric parameters");
  return val.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int exp)||string.IsNullOrWhiteSpace(answer))return false;
  string s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
    c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==exp;
 }
}
