using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Original Grade3 fraction tasks routed via existing exact engine.
// Freehand drawings/oral reasoning remain teacher-reviewed. StageD tests NOT_RUN.
internal static class UaeGrade3Unit10Questions
{
 internal const string Unit="uae.g3.u10.l01.unit_fractions";
 internal const string Whole="uae.g3.u10.l02.part_of_whole";
 internal const string Set="uae.g3.u10.l03.part_of_set";
 internal const string Diagram="uae.g3.u10.l04.design_fraction_diagram";
 internal const string Line="uae.g3.u10.l05.fraction_number_line";
 internal const string Equiv="uae.g3.u10.l06.equivalent_fractions";
 internal const string Integer="uae.g3.u10.l07.fraction_as_whole";
 internal const string Compare="uae.g3.u10.l08.compare_fractions";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Unit or Whole or Set or Diagram or Line or Equiv or Integer or Compare;
 public static Item Build(string f,Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 fraction family");
  int a=1,b=rng.Next(2,9),mode=0,result=0;
  string prompt,solution;
  switch(f)
  {
   case Unit:
    a=1;mode=rng.Next(2);result=b;
    prompt=mode==0?$"قسّم شكلًا واحدًا إلى {b} أجزاء متساوية، ولون جزءًا واحدًا. كم عدد الأجزاء المتساوية في الكل؟":
       $"يمثل الكسر ١/{b} جزءًا واحدًا من كل. ما مقام الكسر؟";
    solution="مقام كسر الوحدة هو عدد الأجزاء المتساوية التي قُسم إليها كل واحد.";break;
   case Whole:
    a=rng.Next(1,b);mode=rng.Next(2);result=a;
    prompt=$"لُوّن {a} من {b} أجزاء متساوية في شكل واحد. ما بسط الكسر الذي يمثل الجزء الملون؟";
    solution="أعد الأجزاء الملونة للبسط وأعد كل الأجزاء المتساوية للمقام.";break;
   case Set:
    a=rng.Next(2,6);mode=rng.Next(2);result=a;
    prompt=$"هناك {b*a} قطعة مقسمة إلى {b} مجموعات متساوية. كم قطعة تمثل ١/{b} من المجموعة الكاملة؟";
    solution="أقسم المجموعة الكاملة إلى مجموعات متساوية وأعد عناصر مجموعة واحدة.";break;
   case Diagram:
    a=rng.Next(1,b);mode=rng.Next(2);result=3*a;
    prompt=$"صمم نموذجًا من {b*3} مربعات متساوية، ثم ظلل {a}/{b} منها. كم مربعًا يجب تظليله؟";
    solution="أقسم المربعات إلى مجموعات متساوية بعدد المقام ثم أختار مجموعات بعدد البسط. الرسم يصححه المعلم.";break;
   case Line:
    a=rng.Next(1,b);mode=rng.Next(2);result=a;
    prompt=$"خط أعداد من صفر إلى واحد مقسم إلى {b} فترات متساوية. النقطة عند القفزة {a} تمثل أي بسط في الكسر □/{b}؟";
    solution="أعد القفزات من الصفر إلى النقطة، لا عدد العلامات المرسومة.";break;
   case Equiv:
    a=rng.Next(1,b);mode=rng.Next(2);result=a*2;
    prompt=$"أكمل كسرًا مكافئًا: {a}/{b} = □/{b*2}. ما البسط المجهول؟";
    solution="أضرب البسط والمقام معًا في العامل نفسه وهو ٢.";break;
   case Integer:
    a=b;mode=rng.Next(2);result=1;
    prompt=$"إذا كانت أجزاء الشكل {b} متساوية وكلها ملونة، فما العدد الكلي الذي يمثله الكسر {b}/{b}؟";
    solution="كل الأجزاء متضمنة؛ الكسر الذي بسطه ومقامه العدد الموجب نفسه يساوي واحدًا.";break;
   case Compare:
    b=rng.Next(4,12);a=rng.Next(2,b-1);mode=rng.Next(2);result=mode==0?1:0;
    prompt=mode==0?$"هل الكسر {a}/{b} أكبر من {(a-1)}/{b}؟ اكتب ١ لنعم، و٠ للا.":
       $"هل الكسر {a}/{b} أكبر من {(a+1)}/{b}؟ اكتب ١ لنعم، و٠ للا.";
    solution="إذا اتفق المقام قارنت بين البسطين، فالجزء الذي بسطه أكبر يمثل مقدارًا أكبر.";break;
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
     b<2||b>11||mode<0||mode>1)return false;
  bool valid=f switch{
   Unit=>a==1&&b<=8,
   Whole=>a>=1&&a<b&&b<=8,
   Set=>a>=2&&a<=5&&b<=8,
   Diagram=>a>=1&&a<b&&b<=8,
   Line=>a>=1&&a<b&&b<=8,
   Equiv=>a>=1&&a<b&&b<=8,
   Integer=>a==b&&b<=8,
   Compare=>b>=4&&b<=11&&a>=2&&a<b-1,
   _=>false};
  if(!valid)return false;
  expected=f switch{
   Unit=>b,
   Whole or Set or Line=>a,
   Diagram=>3*a,
   Equiv=>2*a,
   Integer=>1,
   Compare=>mode==0?1:0,
   _=>throw new InvalidOperationException("Unsupported")};
  return got==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid fraction parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string s=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
   c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(s,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==expected;
 }
}
