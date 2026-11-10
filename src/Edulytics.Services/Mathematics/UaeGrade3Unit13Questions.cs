using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
// Grade3 geometry numeric checks on existing exact engine; geometric constructions are teacher-reviewed.
internal static class UaeGrade3Unit13Questions
{
 internal const string Segments="uae.g3.u13.l01.line_point_ray";
 internal const string Angles="uae.g3.u13.l02.angle_types";
 internal const string Polygon="uae.g3.u13.l03.polygons";
 internal const string Triangle="uae.g3.u13.l04.triangle_types";
 internal const string Quad="uae.g3.u13.l05.quadrilaterals";
 internal const string QuadAttr="uae.g3.u13.l06.quadrilateral_attributes";
 internal const string Investigate="uae.g3.u13.l07.investigate_justify";
 internal const string Partition="uae.g3.u13.l08.partition_shapes";
 internal const string Solid="uae.g3.u13.l09.solid_figures";
 internal const string MoreSolid="uae.g3.u13.l10.more_solids";
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Segments or Angles or Polygon or Triangle or Quad or QuadAttr or Investigate or Partition or Solid or MoreSolid;
 public static Item Build(string f, Random rng)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported G3 geometry family");
  int a=0,b=0,m=0,answer=0;string prompt,solution;
  switch(f)
  {
   case Segments:
    a=rng.Next(3);answer=a==0?2:a==1?1:0;
    prompt=a switch{
      0=>"كم نقطة نهاية للقطعة المستقيمة ذات طول محدد؟",
      1=>"كم نقطة بداية لشعاع يمتد في اتجاه واحد دون نهاية؟",
      _=>"كم نقطة نهاية لمستقيم يمتد في الاتجاهين دون نهاية؟"};
    solution="أفرق بين القطعة ذات الطرفين، والشعاع ذي نقطة البداية، والمستقيم بلا طرفين.";break;
   case Angles:
    a=rng.Next(3);answer=a+1;
    prompt=a switch{
     0=>"زاوية أصغر من الزاوية القائمة. اكتب رقم نوعها: ١ حادة، ٢ قائمة، ٣ منفرجة.",
     1=>"زاوية يلتقي ضلعَاها على هيئة ركن مربع. اكتب نوعها: ١ حادة، ٢ قائمة، ٣ منفرجة.",
     _=>"زاوية أكبر من القائمة وأصغر من المستقيمة. ما نوعها: ١ حادة، ٢ قائمة، ٣ منفرجة؟"};
    solution="أقارن فتحة الزاوية بفتحة الزاوية القائمة دون الاعتماد على طول الضلعين.";break;
   case Polygon:
    a=rng.Next(3,9);answer=a;
    prompt=$"لشكل مضلع مغلق {a} رؤوس متصلة بأضلاع مستقيمة. كم ضلعًا له؟";
    solution="المضلع مغلق ويتكون من قطع مستقيمة، وله العدد نفسه من الرؤوس والأضلاع.";break;
   case Triangle:
    a=rng.Next(3);b=rng.Next(3,10);answer=a+1;
    prompt=a switch{
      0=>$"مثلث أطوال أضلاعه {b} و{b} و{b} وحدات. صنفه: ١ متساوي الأضلاع، ٢ متساوي الساقين فقط، ٣ مختلف الأضلاع.",
      1=>$"مثلث أطوال أضلاعه {b} و{b} و{b+1} وحدات. صنفه: ١ متساوي الأضلاع، ٢ متساوي الساقين فقط، ٣ مختلف الأضلاع.",
      _=>$"مثلث أطوال أضلاعه {b} و{b+1} و{b+2} وحدات. صنفه: ١ متساوي الأضلاع، ٢ متساوي الساقين فقط، ٣ مختلف الأضلاع."};
    solution="أقارن أطوال الأضلاع الثلاثة وأختار وصفًا دقيقًا دون استخدام اتجاه الرسم.";break;
   case Quad:
    a=rng.Next(3);m=rng.Next(2);
    answer=m==0?4:a==2?0:4;
    prompt=m==0?"كم ضلعًا لكل شكل رباعي مهما اختلفت أطواله واتجاهاته؟":
      a==0?"كم زاوية قائمة في المربع؟":a==1?"كم زاوية قائمة في المستطيل؟":
      "كم زاوية قائمة في متوازي أضلاع مائل ليست فيه أية زاوية قائمة؟";
    solution="أتعرف خصائص الشكل الرباعي، وأميز الزوايا القائمة من غير القائمة.";break;
   case QuadAttr:
    a=rng.Next(3);m=rng.Next(2);answer=m==0?4:2;
    prompt=m==0?"كم ضلعًا في مربع أو مستطيل أو معين بوصفها أشكالًا رباعية؟":
      $"كم زوجًا من الأضلاع المتوازية في {(a==0?"المربع":a==1?"المستطيل":"المعين")}؟";
    solution="المربع والمستطيل والمعين أربعة أضلاع ولكل منها زوجان من الأضلاع المتوازية.";break;
   case Investigate:
    a=rng.Next(2,9);m=rng.Next(2);answer=m==0?4*a:2*a+2;
    prompt=m==0?$"ارسم {a} مربعات منفصلة ثم عد كل أضلاعها، بما فيها أضلاع كل مربع. كم ضلعًا عُدّ؟":
      $"صف مكون من {a} مربعات متلاصقة من ضلع واحد في كل مرة. كم ضلع وحدة يظهر على الحد الخارجي للصف؟";
    solution="أميز الأضلاع المشتركة من الحدود الخارجية وأبرر الحساب بالرسم.";break;
   case Partition:
    a=rng.Next(2,7);b=rng.Next(2,7);answer=a*b;
    prompt=$"قسّم مستطيلًا إلى {a} صفوف و{b} أعمدة من مستطيلات صغيرة متساوية. كم جزءًا تكوّن؟";
    solution="أضرب عدد الصفوف في عدد الأعمدة؛ رسم الفواصل والتحقق من تساوي المساحات للمعلم.";break;
   case Solid:
    a=rng.Next(5);m=rng.Next(2);
    if(a<2){answer=m==0?6:8;prompt=m==0?$"كم وجهًا مستويًا في {(a==0?"المكعب":"متوازي المستطيلات")}؟":$"كم رأسًا في {(a==0?"المكعب":"متوازي المستطيلات")}؟";}
    else if(a==2){answer=m==0?2:0;prompt=m==0?"كم قاعدة مستوية دائرية للأسطوانة؟":"كم رأسًا حادًا للأسطوانة؟";}
    else if(a==3){answer=1;prompt=m==0?"كم قاعدة مستوية دائرية للمخروط؟":"كم رأسًا للمخروط؟";}
    else{answer=0;prompt=m==0?"كم قاعدة مستوية لكرة تامة الاستدارة؟":"كم رأسًا للكرة؟";}
    solution="أميز الوجه المستوي والحافة والرأس عن السطح المنحني في المجسمات.";break;
   case MoreSolid:
    a=rng.Next(6);answer=a switch{0=>6,1=>6,2=>5,3=>5,4=>12,_=>8};
    prompt=a switch{
      0=>"كم وجهًا مربعًا في شبكة مكعب إذا لم يحدث تداخل؟",
      1=>"كم وجهًا في متوازي المستطيلات؟",
      2=>"كم وجهًا في المنشور الثلاثي ذي قاعدتين مثلثتين؟",
      3=>"كم وجهًا في الهرم ذي القاعدة المربعة؟",
      4=>"كم حرفًا في المكعب؟",
      _=>"كم رأسًا في متوازي المستطيلات؟"};
    solution="أتعرف المجسم من أوجهه وحروفه ورؤوسه ثم أراجع النموذج الهندسي.";break;
   default:throw new InvalidOperationException("Unsupported");
  }
  return new Item(f,prompt,solution,AssessmentItemType.Numeric,
   new Dictionary<string,int>{{"a",a},{"b",b},{"mode",m},{"result",answer}});
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
    !p.TryGetValue("mode",out int m)||!p.TryGetValue("result",out int actual))return false;
  bool ok=f switch{
   Segments=>a>=0&&a<=2&&b==0&&m==0,
   Angles=>a>=0&&a<=2&&b==0&&m==0,
   Polygon=>a>=3&&a<=8&&b==0&&m==0,
   Triangle=>a>=0&&a<=2&&b>=3&&b<=9&&m==0,
   Quad=>a>=0&&a<=2&&b==0&&m>=0&&m<=1,
   QuadAttr=>a>=0&&a<=2&&b==0&&m>=0&&m<=1,
   Investigate=>a>=2&&a<=8&&b==0&&m>=0&&m<=1,
   Partition=>a>=2&&a<=6&&b>=2&&b<=6&&m==0,
   Solid=>a>=0&&a<=4&&b==0&&m>=0&&m<=1,
   MoreSolid=>a>=0&&a<=5&&b==0&&m==0,
   _=>false};
  if(!ok)return false;
  expected=f switch{
   Segments=>a==0?2:a==1?1:0,
   Angles=>a+1,
   Polygon=>a,
   Triangle=>a+1,
   Quad=>m==0?4:a==2?0:4,
   QuadAttr=>m==0?4:2,
   Investigate=>m==0?4*a:2*a+2,
   Partition=>a*b,
   Solid=>a<2?(m==0?6:8):a==2?(m==0?2:0):a==3?1:0,
   MoreSolid=>a switch{0=>6,1=>6,2=>5,3=>5,4=>12,_=>8},
   _=>throw new InvalidOperationException("Unsupported")};
  return actual==expected;
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p,out int n))throw new InvalidOperationException("Invalid G3 geometry parameters");
  return n.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)
 {
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string normalized=new string(answer.Trim().Select(c=>c is >= '٠' and <= '٩'?(char)('0'+c-'٠'):
    c is >= '۰' and <= '۹'?(char)('0'+c-'۰'):c).ToArray());
  return int.TryParse(normalized,NumberStyles.Integer,CultureInfo.InvariantCulture,out int n)&&n==expected;
 }
}
