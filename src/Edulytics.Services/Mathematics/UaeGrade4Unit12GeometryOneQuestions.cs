using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
internal static class UaeGrade4Unit12GeometryOneQuestions
{
 internal const string Rays="uae.g4.u12.l01.rays_from_point";
 internal const string Segments="uae.g4.u12.l01.segments_from_points";
 internal const string LineRelation="uae.g4.u12.l02.line_relation";
 internal const string RightComplement="uae.g4.u12.l02.right_angle_complement";
 internal const string Axes="uae.g4.u12.l03.symmetry_axes";
 internal const string Mirror="uae.g4.u12.l03.mirrored_points";
 internal const string NetMissing="uae.g4.u12.l04.net_missing_faces";
 internal const string NetFull="uae.g4.u12.l04.net_total_faces";
 internal const string SolidFaces="uae.g4.u12.l05.solid_faces";
 internal const string G5NetType="uae.g5.u12.l07.net_solid_classification";
 internal const string G5NetFaces="uae.g5.u12.l07.net_total_faces";
 internal const string G5NetMatching="uae.g5.u12.l07.net_congruent_matching_faces";
 internal const string SolidVertices="uae.g4.u12.l05.solid_vertices";
 internal const string CubeCount="uae.g4.u12.l06.unit_cube_volume";
 internal const string LayerArea="uae.g4.u12.l06.layer_unit_squares";
 internal const string PrismVolume="uae.g4.u12.l07.prism_volume";
 internal const string MissingHeight="uae.g4.u12.l07.prism_missing_height";
 internal static readonly HashSet<string> Families=[
  Rays,Segments,LineRelation,RightComplement,Axes,Mirror,NetMissing,NetFull,
  SolidFaces,SolidVertices,CubeCount,LayerArea,PrismVolume,MissingHeight,
  G5NetType,G5NetFaces,G5NetMatching];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? family)=>family is not null&&Families.Contains(family);
 internal static Item Build(string family,Random random)
 {
  if(!Supports(family))throw new InvalidOperationException("Unsupported geometry I family");
  int a=random.Next(2,9),b=random.Next(2,9),c=random.Next(2,7);
  if(family is LineRelation)a=new[]{0,45,90}[random.Next(3)];
  if(family is RightComplement)a=random.Next(0,91);
  if(family is Axes)a=random.Next(1,5);
  if(family is SolidFaces or SolidVertices)a=random.Next(1,4);
  if(family is G5NetType or G5NetFaces or G5NetMatching)a=random.Next(1,3);
  if(family is NetMissing)a=random.Next(2,6);
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  string solid=a==1?"مكعب":a==2?"منشور مستطيل":"منشور ثلاثي";
  string prompt=family switch
  {
   Rays=>$"على رسم {a} نقاط مختلفة، كم شعاعًا يمكن رسمه من نقطة محددة نحو بقية النقاط؟",
   Segments=>$"على رسم {a} نقاط مختلفة، كم قطعة مستقيمة مختلفة بين كل زوج من النقاط؟",
   LineRelation=>$"يصنع مستقيمان زاوية {a}° عند التقاطع إن تقاطعا. اكتب ٠ للمتوازيين عند زاوية ٠°، و١ للمتعامدين عند ٩٠°، و٢ للمتقاطعين غير المتعامدين.",
   RightComplement=>$"زاوية قياسها {a}°. كم درجة نضيف حتى نحصل على زاوية قائمة؟",
   Axes=>$"ما عدد محاور التناظر للشكل ذي الرمز {a}؟ ١ مستطيل غير مربع، ٢ مربع، ٣ مثلث متساوي الأضلاع، ٤ خماسي منتظم.",
   Mirror=>$"توجد {b} نقاط لا تقع على محور التناظر في جهة واحدة. كم صورة منعكسة لها في الجهة الأخرى؟",
   NetMissing=>$"شبكة مكعب يجب أن تحتوي ٦ مربعات. إذا رسمنا {a} مربعات، كم مربعًا متبقيًا؟",
   NetFull=>$"كم وجهًا مربعًا في شبكة كاملة لمكعب؟",
   SolidFaces=>$"كم وجهًا في {solid}؟",
   G5NetType=>a==1
    ? $"شبكة صالحة من ستة مربعات متطابقة ضلع كل منها {b} سم. ما نوع المجسم بعد الطي؟ اكتب ٠ للمكعب أو ١ للمنشور مستطيل القاعدة غير المكعب."
    : $"شبكة صالحة من ستة أوجه مستطيلة لمجسم أبعاده {b} سم و{c+10} سم و{b+c+15} سم. ما نوعه بعد الطي؟ اكتب ٠ للمكعب أو ١ للمنشور مستطيل القاعدة غير المكعب.",
   G5NetFaces=>a==1
    ? $"شبكة صالحة لمكعب طول حرفه {b} سم. كم وجهًا للمجسم بعد الطي؟"
    : $"شبكة صالحة لمنشور مستطيل القاعدة أبعاده {b} سم و{c+10} سم و{b+c+15} سم. كم وجهًا للمجسم بعد الطي؟",
   G5NetMatching=>a==1
    ? $"لشبكة مكعب طول حرفه {b} سم ستة أوجه. كم وجهًا مطابقًا لوجه مربع واحد عند مقارنة جميع الأوجه مع احتساب الوجه نفسه؟"
    : $"منشور مستطيل القاعدة ذو ثلاثة أبعاد مختلفة {b} سم و{c+10} سم و{b+c+15} سم، كم وجهًا يطابق أكبر وجه من أوجهه مع احتساب الوجه نفسه؟",
   SolidVertices=>$"كم رأسًا في {solid}؟",
   CubeCount=>$"منشور مستطيل طوله {a} مكعبات وعرضه {b} مكعبات وارتفاعه {c} طبقات. كم مكعب وحدة يحتوي؟",
   LayerArea=>$"في طبقة من مكعبات الوحدة طولها {a} مكعبات وعرضها {b} مكعبات. كم مربع وحدة في الطبقة الواحدة؟",
   PrismVolume=>$"منشور مستطيل طوله {a} سم وعرضه {b} سم وارتفاعه {c} سم. ما حجمه بالسم المكعب؟",
   _=>$"منشور مستطيل حجمه {a*b*c} سم³، طول قاعدته {a} سم وعرضها {b} سم. ما ارتفاعه بالسنتيمتر؟"
  };
  return new Item(family,prompt,
   "استخدم رسمًا هندسيًا أصليًا، وحدد خواص العناصر، ثم تحقق بقانون أو عدّ مستقل.",
   AssessmentItemType.Numeric,p);
 }
 private static bool Valid(string f,IReadOnlyDictionary<string,int> p,out int result)
 {
  result=0;
  if(!Supports(f)||p.Count!=3||!p.TryGetValue("a",out int a)||!p.TryGetValue("b",out int b)||
     !p.TryGetValue("c",out int c)||a<0||a>90||b is <1 or >100||c is <1 or >100)return false;
  if(f is LineRelation && a is not (0 or 45 or 90))return false;
  if(f is RightComplement && a>90)return false;
  if(f is Axes && a is <1 or >4)return false;
  if(f is SolidFaces or SolidVertices && a is <1 or >3)return false;
  if(f is G5NetType or G5NetFaces or G5NetMatching && a is <1 or >2)return false;
  if(f is NetMissing && a is <1 or >6)return false;
  if(f is Rays or Segments && a<2)return false;
  if(f is CubeCount or LayerArea or PrismVolume or MissingHeight && a<1)return false;
  result=f switch
  {
    Rays=>a-1,
    Segments=>a*(a-1)/2,
    LineRelation=>a==0?0:a==90?1:2,
    RightComplement=>90-a,
    Axes=>a switch{1=>2,2=>4,3=>3,_=>5},
    Mirror=>b,
    NetMissing=>6-a,
    NetFull=>6,
    SolidFaces=>a==3?5:6,
    G5NetType=>a==1?0:1,
    G5NetFaces=>6,
    G5NetMatching=>a==1?6:2,
    SolidVertices=>a==3?6:8,
    CubeCount or PrismVolume=>a*b*c,
    LayerArea=>a*b,
    MissingHeight=>c,
    _=>throw new InvalidOperationException("Unsupported family")
  };
  return true;
 }
 internal static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
   if(!Valid(f,p,out int result))throw new InvalidOperationException("Invalid geometry I parameters");
   return result.ToString(CultureInfo.InvariantCulture);
 }
 internal static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)
 {
   if(!Valid(f,p,out int expected)||string.IsNullOrWhiteSpace(answer))return false;
   string normalized=new(answer.Trim().Select(ch=>ch switch
   {
     >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
     >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
     _=>ch
   }).Where(ch=>ch is not ' ' and not '٬' and not '\u00a0').ToArray());
   return int.TryParse(normalized,NumberStyles.None,CultureInfo.InvariantCulture,out int actual)&&actual==expected;
 }
}
