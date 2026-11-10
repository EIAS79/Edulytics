using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Original Grade3 Unit14 source-scoped area and perimeter numeric families reuse the exact Practice engine.
// Shape drawings, units and learner reasoning remain assessed with teacher rubric. StageD NOT_RUN.
internal static class UaeGrade3Unit14Questions
{
 internal const string PerimHands="uae.g3.u14.l01.find_perimeter";
 internal const string Perim="uae.g3.u14.l02.perimeter";
 internal const string AreaHands="uae.g3.u14.l03.understand_area";
 internal const string AreaMeasure="uae.g3.u14.l04.measure_area";
 internal const string Tile="uae.g3.u14.l05.tile_rectangles";
 internal const string Rectangle="uae.g3.u14.l06.rectangle_area";
 internal const string Distrib="uae.g3.u14.l07.area_distributive";
 internal const string Composite="uae.g3.u14.l08.composite_area";
 internal const string AreaPerim="uae.g3.u14.l09.area_perimeter";
 internal const string Diagram="uae.g3.u14.l10.problem_solving_diagram";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is PerimHands or Perim or AreaHands or AreaMeasure or Tile or Rectangle or Distrib or Composite or AreaPerim or Diagram;
 public static Item Build(string f, Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported Grade3 area/perimeter family");
  int a=rng.Next(3,13),b=rng.Next(3,13),m=rng.Next(2),answer=0;string prompt,solution;
  switch(f)
  {
   case PerimHands:
    answer=m==0?2*(a+b):4*a;
    prompt=m==0?$"استخدم شريط قياس حول مستطيل طوله {a} سم وعرضه {b} سم. ما محيطه بالسنتيمترات؟":
      $"استخدم أربعة أضلاع متساوية طول كل منها {a} سم لتكوين مربع. ما محيطه؟";
    solution="أجمع أطوال كل الأضلاع حول الحد الخارجي للشكل؛ للمستطيل ٢×(الطول+العرض).";break;
   case Perim:
    answer=m==0?2*(a+b):3*a;
    prompt=m==0?$"أوجد محيط مستطيل طوله {a} أمتار وعرضه {b} أمتار.":
      $"أوجد محيط مثلث متساوي الأضلاع طول ضلعه {a} سم.";
    solution="محيط الشكل يساوي مجموع أطوال أضلاعه الخارجية مع الحفاظ على وحدة الطول.";break;
   case AreaHands:
    a=rng.Next(2,10);b=rng.Next(2,10);answer=a*b;
    prompt=$"ارسم شبكة من {a} صفوف و{b} أعمدة من مربعات وحدة متساوية. كم مربع وحدة داخلها؟";
    solution="المساحة عدد مربعات الوحدة التي تغطي داخل الشكل دون فجوات أو تداخل.";break;
   case AreaMeasure:
    a=rng.Next(3,10);b=rng.Next(3,10);answer=m==0?a*b:a*b-4;
    prompt=m==0?$"عد مربعات الوحدة التي تغطي مستطيلًا من {a} صفوف و{b} أعمدة. كم وحدة مربعة؟":
      $"شكل داخل شبكة {a} × {b} مربعات حُذف من ركنه مربع مكون من ٢ × ٢ مربعات وحدة. ما مساحته الباقية؟";
    solution="أحسب مربعات المساحة كاملة ثم أطرح مساحة الجزء المفرغ إذا وُجد.";break;
   case Tile:
    a=rng.Next(2,11);b=rng.Next(2,11);answer=a*b;
    prompt=$"أريد تغطية مستطيل بطول {a} بلاطات مربعة وعرض {b} بلاطات دون فراغات. كم بلاطة تكفي؟";
    solution="أرتب البلاطات في صفوف وأعمدة وأضرب عدد الصفوف في عدد الأعمدة.";break;
   case Rectangle:
    answer=a*b;prompt=m==0?$"ما مساحة مستطيل طوله {a} سم وعرضه {b} سم بالسنتيمترات المربعة؟":
      $"لدى حديقة مستطيلة {a} صفوف، في كل صف {b} متر مربع. كم مترًا مربعًا في الحديقة؟";
    solution="المساحة = الطول × العرض، وتُكتب بالوحدات المربعة وليس بالسنتيمترات الخطية.";break;
   case Distrib:
    answer=a*(b+(m==0?2:3));
    prompt=m==0?$"مستطيل ارتفاعه {a} وحدات وعرضه {b}+٢ وحدات. استخدم خاصية التوزيع لحساب المساحة.":
      $"أوجد مساحة مستطيل ارتفاعه {a} وحدات، وعرضه مقسم إلى {b} و٣ وحدات، باستخدام مجموع مساحتي الجزأين.";
    solution="أجمع مساحتي الجزأين: أ×(ب+ج) = أ×ب + أ×ج.";break;
   case Composite:
    a=rng.Next(4,13);b=rng.Next(4,13);
    answer=m==0?a*b-4:a*b+2*a;
    prompt=m==0?$"شكل مركب من مستطيل {a} × {b} وحدات مربعة أزيلت منه زاوية بمساحة ٢ × ٢. ما المساحة المتبقية؟":
      $"يتكون شكل مركب من مستطيل {a} × {b} ومربعين من صف واحد، مجموع مساحتهما {a} × ٢، دون تداخل. ما المساحة الإجمالية؟";
    solution="أقسم الشكل المركب إلى أجزاء غير متداخلة؛ أجمع أو أطرح مساحات المستطيلات.";break;
   case AreaPerim:
    answer=m==0?a*b:2*(a+b);
    prompt=m==0?$"أوجد مساحة مستطيل طوله {a} سم وعرضه {b} سم بوحدة سم².":
      $"أوجد محيط مستطيل طوله {a} سم وعرضه {b} سم بوحدة سم.";
    solution="المساحة داخل الشكل بوحدة مربعة؛ المحيط طول حول الحد الخارجي بوحدة طول.";break;
   case Diagram:
    answer=m==0?a*b:2*(a+b);
    prompt=m==0?$"ارسم مخططًا لحديقة مستطيلة طولها {a} م وعرضها {b} م. ما مساحتها بوحدة متر مربع؟":
      $"ارسم مخططًا لحديقة مستطيلة طولها {a} م وعرضها {b} م. ما طول السياج حولها بالأمتار؟";
    solution="أحدد المطلوب ووحدة الجواب، وأرسم مخططًا بأبعاد معلّمة، ثم أحسب المساحة أو المحيط.";break;
   default:throw new InvalidOperationException("Unsupported");
  }
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,
   new Dictionary<string,int>{{"a",a},{"b",b},{"mode",m},{"result",answer}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
    !p.TryGetValue("mode",out int m)||!p.TryGetValue("result",out int given)||
    m<0||m>1)return false;
  bool ok=f switch{
   PerimHands or Perim or Rectangle or Distrib or AreaPerim or Diagram=>a>=3&&a<=12&&b>=3&&b<=12,
   AreaHands=>a>=2&&a<=9&&b>=2&&b<=9,
   AreaMeasure=>a>=3&&a<=9&&b>=3&&b<=9,
   Tile=>a>=2&&a<=10&&b>=2&&b<=10,
   Composite=>a>=4&&a<=12&&b>=4&&b<=12,
   _=>false};
  if(!ok)return false;
  expected=f switch{
   PerimHands=>m==0?2*(a+b):4*a,
   Perim=>m==0?2*(a+b):3*a,
   AreaHands or Tile or Rectangle=>a*b,
   AreaMeasure=>m==0?a*b:a*b-4,
   Distrib=>a*(b+(m==0?2:3)),
   Composite=>m==0?a*b-4:a*b+2*a,
   AreaPerim or Diagram=>m==0?a*b:2*(a+b),
   _=>throw new InvalidOperationException("Unsupported")};
  return expected==given;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid G3 area/perimeter parameters");
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
