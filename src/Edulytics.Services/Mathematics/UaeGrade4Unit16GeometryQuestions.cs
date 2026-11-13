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
 "uae.g4.u16.l08.model_shape_count","uae.g4.u16.l08.model_grid_total",
 "uae.g5.u12.l02.congruent_side_count","uae.g5.u12.l02.acute_angle_count",
 "uae.g5.u12.l02.right_angle_count","uae.g5.u12.l02.obtuse_angle_count",
 "uae.g5.u12.l02.third_angle",
 "uae.g5.u12.l03.triangle_side_type","uae.g5.u12.l03.triangle_angle_type",
 "uae.g5.u12.l05.opposite_parallel_pairs","uae.g5.u12.l05.opposite_equal_pairs",
 "uae.g5.u12.l05.right_angle_count","uae.g5.u12.l05.opposite_equal_angle_pairs"];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? f)=>f is not null&&Families.Contains(f);
 private static bool IsG5Quad(string? f)=>f is
  "uae.g5.u12.l05.opposite_parallel_pairs" or
  "uae.g5.u12.l05.opposite_equal_pairs" or
  "uae.g5.u12.l05.right_angle_count" or
  "uae.g5.u12.l05.opposite_equal_angle_pairs";
 private static bool IsG5Triangle(string? f)=>f is
  "uae.g5.u12.l02.congruent_side_count" or
  "uae.g5.u12.l02.acute_angle_count" or
  "uae.g5.u12.l02.right_angle_count" or
  "uae.g5.u12.l02.obtuse_angle_count" or
  "uae.g5.u12.l02.third_angle" or
  "uae.g5.u12.l03.triangle_side_type" or
  "uae.g5.u12.l03.triangle_angle_type";
 internal static Item Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unsupported geometry family");
  if(IsG5Quad(f))return BuildG5Quad(f,random);
  if(IsG5Triangle(f))return BuildG5Triangle(f,random);
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
   if(IsG5Quad(f))return SolveG5Quad(f,p);
   if(IsG5Triangle(f))return SolveG5Triangle(f,p);
   if(!Valid(f,p,out int ans))throw new InvalidOperationException("Invalid geometry parameters");
   return ans.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
  if(IsG5Quad(f))return VerifyG5Quad(f,p,answer);
  if(IsG5Triangle(f))return VerifyG5Triangle(f,p,answer);
  if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
  string normalized=new(answer.Trim().Select(c=>c switch{
   >= '٠' and <= '٩'=>(char)('0'+c-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+c-'۰'),_=>c
  }).Where(c=>c is not ' ' and not ',' and not '٬' and not '\u00a0').ToArray());
  return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }

 // Grade 5 U12 L02 exact numerical reading. Physical ruler/protractor
 // accuracy and original drawings are separately teacher-reviewed.
 private static Item BuildG5Triangle(string f,Random rng)
 {
  int a,b,c;
  if(f is "uae.g5.u12.l02.congruent_side_count" or "uae.g5.u12.l03.triangle_side_type")
  {
   int[][] sideSets=[[5,5,5],[6,6,9],[4,5,6],[7,8,10],[4,7,7],[3,4,5]];
   int[] chosen=sideSets[rng.Next(sideSets.Length)];
   int shift=rng.Next(0,4);
   a=chosen[0]+shift;b=chosen[1]+shift;c=chosen[2]+shift;
  }
  else
  {
   int[][] angleSets=[[60,60,60],[35,55,90],[105,45,30],[45,65,70],[90,45,45],[120,30,30]];
   int[] chosen=angleSets[rng.Next(angleSets.Length)];
   a=chosen[0];b=chosen[1];c=chosen[2];
  }
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  string question=f switch
  {
   "uae.g5.u12.l02.congruent_side_count"=>
    $"مثلث أطوال أضلاعه {a} سم و{b} سم و{c} سم. كم ضلعًا يطابق في الطول ضلعًا آخر؟ إذا لم يتطابق شيء اكتب ٠.",
   "uae.g5.u12.l02.acute_angle_count"=>
    $"زوايا مثلث {a}° و{b}° و{c}°. كم زاوية حادة فيه؟",
   "uae.g5.u12.l02.right_angle_count"=>
    $"زوايا مثلث {a}° و{b}° و{c}°. كم زاوية قائمة فيه؟",
   "uae.g5.u12.l02.obtuse_angle_count"=>
    $"زوايا مثلث {a}° و{b}° و{c}°. كم زاوية منفرجة فيه؟",
   "uae.g5.u12.l02.third_angle"=>
    $"زاويتان في مثلث {a}° و{b}°. ما قياس الزاوية الثالثة بالدرجات؟",
   "uae.g5.u12.l03.triangle_side_type"=>
    $"أطوال ثلاثة أضلاع صالحة لمثلث: {a} سم و{b} سم و{c} سم. صنّف نوعه بحسب الأضلاع: ٠ متساوي الأضلاع، ١ متساوي الساقين غير المتساوي الأضلاع، ٢ مختلف الأضلاع.",
   "uae.g5.u12.l03.triangle_angle_type"=>
    $"زوايا مثلث {a}° و{b}° و{c}°. صنّف المثلث: ٠ حاد الزوايا، ١ قائم الزاوية، ٢ منفرج الزاوية.",
   _=>throw new InvalidOperationException("Unknown G5 triangle family")
  };
  return new Item(f,question,
   "أقارن الأضلاع للتطابق أو أقارن كل زاوية بـ٩٠°، ولإيجاد الزاوية الثالثة أطرح الزاويتين من ١٨٠°. يعاين المعلم القياسات والرسم الفعلي.",
   AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Triangle(string family,IReadOnlyDictionary<string,int> p,out int result)
 {
  result=0;
  if(!IsG5Triangle(family)||p.Count!=3||
   !p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
   !p.TryGetValue("c",out int c)||a<=0||b<=0||c<=0)return false;
  if(family is "uae.g5.u12.l02.congruent_side_count" or "uae.g5.u12.l03.triangle_side_type")
  {
   if(a>50||b>50||c>50||a+b<=c||a+c<=b||b+c<=a)return false;
   result=family=="uae.g5.u12.l03.triangle_side_type"
       ? a==b&&b==c?0:a==b||b==c||a==c?1:2
       : a==b&&b==c?3:a==b||b==c||a==c?2:0;
   return true;
  }
  if(a>=180||b>=180||c>=180||a+b+c!=180)return false;
  result=family switch
  {
   "uae.g5.u12.l02.acute_angle_count"=>new[]{a,b,c}.Count(v=>v<90),
   "uae.g5.u12.l02.right_angle_count"=>new[]{a,b,c}.Count(v=>v==90),
   "uae.g5.u12.l02.obtuse_angle_count"=>new[]{a,b,c}.Count(v=>v>90),
   "uae.g5.u12.l03.triangle_angle_type"=>new[]{a,b,c}.Any(v=>v>90)?2:
       new[]{a,b,c}.Any(v=>v==90)?1:0,
   _=>180-a-b
  };
  return true;
 }
 private static string SolveG5Triangle(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5Triangle(family,p,out int result))
   throw new InvalidOperationException("Invalid Grade5 triangle");
  return result.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Triangle(string family,IReadOnlyDictionary<string,int> p,string? answer)
 {
  if(!ValidG5Triangle(family,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>20)return false;
  string normalized=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   _=>ch
  }).ToArray());
  if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int submitted))return false;
  int a=p["a"],b=p["b"],c=p["c"];
  int independent=family switch
  {
   "uae.g5.u12.l02.congruent_side_count"=>
    a==b&&b==c?3:a==b||b==c||a==c?2:0,
   "uae.g5.u12.l02.acute_angle_count"=>(a<90?1:0)+(b<90?1:0)+(c<90?1:0),
   "uae.g5.u12.l02.right_angle_count"=>(a==90?1:0)+(b==90?1:0)+(c==90?1:0),
   "uae.g5.u12.l02.obtuse_angle_count"=>(a>90?1:0)+(b>90?1:0)+(c>90?1:0),
   "uae.g5.u12.l03.triangle_side_type"=>
    a==b&&b==c?0:a==b||b==c||a==c?1:2,
   "uae.g5.u12.l03.triangle_angle_type"=>
    a>90||b>90||c>90?2:a==90||b==90||c==90?1:0,
   _=>180-a-b
  };
  return submitted==independent;
 }


 // UAE Grade 5 U12 Lesson 5: reuse the existing quadrilateral geometry service.
 // Physical measurements/drawings are separately assessed by the teacher.
 private static readonly string[] Grade5QuadrilateralDescriptions =
 [
  "مربع بأربعة أضلاع متطابقة وأربع زوايا قائمة",
  "مستطيل غير مربع، له أربع زوايا قائمة وزوجان من الأضلاع المتقابلة المتطابقة",
  "متوازي أضلاع مائل ليس مستطيلًا ولا معينًا",
  "معين مائل ليس مربعًا: أربعة أضلاع متطابقة دون زوايا قائمة",
  "شبه منحرف متساوي الساقين ليس مستطيلًا وله زوج واحد فقط متوازٍ",
  "شبه منحرف قائم غير مستطيل، له زاويتان قائمتان وزوج متوازٍ واحد فقط",
  "رباعي محدب مختلف الأضلاع، لا يحتوي على ضلعين متقابلين متوازيين أو متطابقين",
  "رباعي على هيئة طائرة ورقية غير معين، له زوجان من الأضلاع المتجاورة المتطابقة لا المتقابلة"
 ];
 private static Item BuildG5Quad(string family,Random rng)
 {
  int kind=rng.Next(Grade5QuadrilateralDescriptions.Length);
  var p=new Dictionary<string,int>{{"kind",kind}};
  string task=family switch
  {
   "uae.g5.u12.l05.opposite_parallel_pairs"=>
    "كم زوجًا من الأضلاع المتقابلة المتوازية في هذا الرباعي؟",
   "uae.g5.u12.l05.opposite_equal_pairs"=>
    "كم زوجًا من الأضلاع المتقابلة المتطابقة في الطول؟",
   "uae.g5.u12.l05.right_angle_count"=>
    "كم زاوية قائمة في هذا الرباعي؟",
   _=>"كم زوجًا من الزوايا المتقابلة المتطابقة في القياس؟"
  };
  return new Item(family,$"الشكل الموصوف: {Grade5QuadrilateralDescriptions[kind]}. {task}",
   "أفحص الأضلاع المتقابلة لا المتجاورة وأفرق بين التوازي والتطابق، ثم أحدد الزوايا القائمة. يراجع المعلم الرسم والقياس العملي.",
   AssessmentItemType.Numeric,p);
 }
 private static bool ValidG5Quad(string family,IReadOnlyDictionary<string,int> p,out int expected)
 {
  expected=0;
  if(!IsG5Quad(family)||p.Count!=1||
    !p.TryGetValue("kind",out int kind)||kind<0||kind>=Grade5QuadrilateralDescriptions.Length)return false;
  expected=family switch
  {
   "uae.g5.u12.l05.opposite_parallel_pairs"=>kind<4?2:kind<6?1:0,
   "uae.g5.u12.l05.opposite_equal_pairs"=>kind<4?2:0,
   "uae.g5.u12.l05.right_angle_count"=>kind<2?4:kind==5?2:0,
   "uae.g5.u12.l05.opposite_equal_angle_pairs"=>kind<4?2:kind==7?1:0,
   _=>throw new InvalidOperationException("Unknown Grade5 quadrilateral family")
  };
  return true;
 }
 private static string SolveG5Quad(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!ValidG5Quad(family,p,out int result))throw new InvalidOperationException("Invalid quadrilateral kind");
  return result.ToString(CultureInfo.InvariantCulture);
 }
 private static bool VerifyG5Quad(string family,IReadOnlyDictionary<string,int> p,string? answer)
 {
  if(!ValidG5Quad(family,p,out _)||string.IsNullOrWhiteSpace(answer)||answer.Length>16)return false;
  string normalized=new(answer.Trim().Select(ch=>ch switch
  {
   >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
   >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
   _=>ch
  }).ToArray());
  if(!int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int value))return false;
  int kind=p["kind"];
  int independent=family switch
  {
   "uae.g5.u12.l05.opposite_parallel_pairs"=>new[]{2,2,2,2,1,1,0,0}[kind],
   "uae.g5.u12.l05.opposite_equal_pairs"=>new[]{2,2,2,2,0,0,0,0}[kind],
   "uae.g5.u12.l05.right_angle_count"=>new[]{4,4,0,0,0,2,0,0}[kind],
   _=>new[]{2,2,2,2,0,0,0,1}[kind]
  };
  return value==independent;
 }

}
