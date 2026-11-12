using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit16GeometryQuestions
{
 internal static readonly HashSet<string> Families=[
 "uae.g4.u16.l01.construct_degrees","uae.g4.u16.l01.straight_remaining",
 "uae.g4.u16.l02.classify_degrees","uae.g4.u16.l02.distance_to_right_angle",
 "uae.g4.u16.l03.measure_degrees","uae.g4.u16.l03.compare_degrees",
 "uae.g4.u16.l04.draw_degrees","uae.g4.u16.l04.draw_supplement",
 "uae.g4.u16.l05.add_angles","uae.g4.u16.l05.subtract_angles",
 "uae.g4.u16.l06.triangle_perimeter","uae.g4.u16.l06.triangle_missing_angle",
 "uae.g4.u16.l07.quadrilateral_perimeter","uae.g4.u16.l07.quadrilateral_missing_angle",
 "uae.g4.u16.l08.model_shape_count","uae.g4.u16.l08.model_grid_total"];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f is not null&&Families.Contains(f);
 internal static Item Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported geometry family");
  int a=random.Next(20,70),b=random.Next(10,50),c=random.Next(10,50);
  if(f.Contains(".l02."))a=random.Next(20,161);
  if(f.Contains(".l06.")){a=random.Next(20,60);b=random.Next(20,60);c=random.Next(Math.Abs(a-b)+1,a+b);}
  if(f.Contains(".l07.")){a=random.Next(65,85);b=random.Next(65,85);c=random.Next(65,85);}
  if(f.Contains(".l08.")){a=random.Next(2,10);b=random.Next(2,10);c=random.Next(1,5);}
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  string prompt=f switch
  {
   "uae.g4.u16.l01.construct_degrees"=>$"ارسم زاوية بقياس {a} درجة. ما قياس الزاوية المطلوب رسمها؟",
   "uae.g4.u16.l01.straight_remaining"=>$"في زاوية مستقيمة ١٨٠ درجة رُسمت زاوية بقياس {a} درجة. ما قياس الباقي؟",
   "uae.g4.u16.l02.classify_degrees"=>$"زاوية قياسها {a} درجة. اكتب ٠ للحادة، و١ للقائمة، و٢ للمنفرجة.",
   "uae.g4.u16.l02.distance_to_right_angle"=>$"كم درجة تفصل قياس {a} درجة عن الزاوية القائمة ٩٠ درجة؟",
   "uae.g4.u16.l03.measure_degrees"=>$"تظهر المنقلة زاوية قياسها {a} درجة. ما قياسها؟",
   "uae.g4.u16.l03.compare_degrees"=>$"قارن زاويتين قياساهما {a} و{b} درجة. ما الفرق بين قياسيهما؟",
   "uae.g4.u16.l04.draw_degrees"=>$"المطلوب إنشاء زاوية بقياس {a} درجة. ما القياس الذي تحدده على المنقلة؟",
   "uae.g4.u16.l04.draw_supplement"=>$"زاوية قياسها {a} درجة. كم درجة تكملها إلى زاوية مستقيمة؟",
   "uae.g4.u16.l05.add_angles"=>$"زاويتان متجاورتان قياساهما {a} و{b} درجة. احسب مجموعهما.",
   "uae.g4.u16.l05.subtract_angles"=>$"زاوية قياسها {a+b} درجة تضم زاوية بقياس {b} درجة. ما قياس الزاوية الأخرى؟",
   "uae.g4.u16.l06.triangle_perimeter"=>$"مثلث أطوال أضلاعه {a} و{b} و{c} سم. ما محيطه؟",
   "uae.g4.u16.l06.triangle_missing_angle"=>$"في مثلث زاويتان داخليتان {a} و{b} درجة. ما قياس الزاوية الثالثة؟",
   "uae.g4.u16.l07.quadrilateral_perimeter"=>$"مستطيل ضلعاه {a} و{b} سم. ما محيطه؟",
   "uae.g4.u16.l07.quadrilateral_missing_angle"=>$"في رباعي ثلاث زوايا قياساتها {a} و{b} و{c} درجة. ما قياس الرابعة؟",
   "uae.g4.u16.l08.model_shape_count"=>$"في رسم منظم {a} صفوف، وفي كل صف {b} أشكال. كم شكلًا؟",
   _=>$"ارسم شبكة {a} صفوف و{b} أعمدة. كم مربع وحدة داخلها؟"
  };
  return new Item(f,prompt,"ارسم نموذجًا هندسيًا مناسبًا وحدد العلاقة بين الأضلاع والزوايا، ثم احسب المطلوب مع وحدة القياس.",
   AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
 {
   result=0;
   if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||!p.TryGetValue("c",out int c)||
      a is <1 or >180||b is <1 or >180||c is <1 or >180||(f.Contains(".l06.")&&a+b>=180)||
      (f.Contains(".l07.")&&(a+b+c>=360||a+b+c<=180))||
      (f=="uae.g4.u16.l06.triangle_perimeter"&&(a+b<=c||a+c<=b||b+c<=a)))return false;
   result=f switch
   {
    "uae.g4.u16.l01.construct_degrees" or "uae.g4.u16.l03.measure_degrees" or "uae.g4.u16.l04.draw_degrees" or "uae.g4.u16.l05.subtract_angles"=>a,
    "uae.g4.u16.l01.straight_remaining" or "uae.g4.u16.l04.draw_supplement"=>180-a,
    "uae.g4.u16.l02.classify_degrees"=>a<90?0:a==90?1:2,
    "uae.g4.u16.l02.distance_to_right_angle"=>Math.Abs(90-a),
    "uae.g4.u16.l03.compare_degrees"=>Math.Abs(a-b),
    "uae.g4.u16.l05.add_angles"=>a+b,
    "uae.g4.u16.l06.triangle_perimeter"=>a+b+c,
    "uae.g4.u16.l06.triangle_missing_angle"=>180-a-b,
    "uae.g4.u16.l07.quadrilateral_perimeter"=>2*(a+b),
    "uae.g4.u16.l07.quadrilateral_missing_angle"=>360-a-b-c,
    _=>a*b
   };
   return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid geometry parameters");
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
