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
 internal const string G5SolidType="uae.g5.u12.l08.solid_type";
 internal const string G5SolidFaces="uae.g5.u12.l08.solid_faces";
 internal const string G5SolidEdges="uae.g5.u12.l08.solid_edges";
 internal const string G5SolidVertices="uae.g5.u12.l08.solid_vertices";
 internal const string CubeCount="uae.g4.u12.l06.unit_cube_volume";
 internal const string LayerArea="uae.g4.u12.l06.layer_unit_squares";
 internal const string PrismVolume="uae.g4.u12.l07.prism_volume";
 internal const string G5Layer="uae.g5.u12.l09.model_cubes_per_layer";
 internal const string G5CubeTotal="uae.g5.u12.l09.model_total_unit_cubes";
 internal const string G5Volume="uae.g5.u12.l09.model_volume_cm3";
 internal const string G5MissingLayers="uae.g5.u12.l09.model_missing_layers";
 internal const string MissingHeight="uae.g4.u12.l07.prism_missing_height";
 internal const string G5VolumeLwh="uae.g5.u12.l10.volume_lwh";
 internal const string G5VolumeBh="uae.g5.u12.l10.volume_base_area_height";
 internal const string G5PrismHeight="uae.g5.u12.l10.missing_prism_height";
 internal const string G5VolumeCompare="uae.g5.u12.l10.compare_prism_volumes";
 internal const string G5CompositeBase="uae.g5.u12.l11.base_layer_cubes";
 internal const string G5CompositeUpper="uae.g5.u12.l11.upper_layers_cubes";
 internal const string G5CompositeTotal="uae.g5.u12.l11.composite_total_cubes";
 internal const string G5CompositeError="uae.g5.u12.l11.incorrect_product_correction";
 internal const string G5SumPrisms="uae.g5.u12.l12.sum_two_prisms";
 internal const string G5TopBase="uae.g5.u12.l12.base_plus_top";
 internal const string G5Cutout="uae.g5.u12.l12.prism_with_removed_section";
 internal const string G5MissingTop="uae.g5.u12.l12.upper_volume_from_total";
 internal const string G5ModelTotal="uae.g5.u12.l13.model_total_units";
 internal const string G5ModelLayers="uae.g5.u12.l13.model_missing_layers";
 internal const string G5ModelLoop="uae.g5.u12.l13.model_loop_stations";
 internal const string G5ModelTiles="uae.g5.u12.l13.model_tiles_per_row";
 internal static readonly HashSet<string> Families=[
  Rays,Segments,LineRelation,RightComplement,Axes,Mirror,NetMissing,NetFull,
  SolidFaces,SolidVertices,CubeCount,LayerArea,PrismVolume,MissingHeight,
  G5NetType,G5NetFaces,G5NetMatching,G5SolidType,G5SolidFaces,G5SolidEdges,G5SolidVertices,
  G5Layer,G5CubeTotal,G5Volume,G5MissingLayers,
  G5VolumeLwh,G5VolumeBh,G5PrismHeight,G5VolumeCompare,
  G5CompositeBase,G5CompositeUpper,G5CompositeTotal,G5CompositeError,
  G5SumPrisms,G5TopBase,G5Cutout,G5MissingTop,
  G5ModelTotal,G5ModelLayers,G5ModelLoop,G5ModelTiles];
 internal sealed record Item(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 internal static bool Supports(string? family)=>family is not null&&Families.Contains(family);
 internal static Item Build(string family,Random random)
 {
  if(!Supports(family))throw new InvalidOperationException("Unsupported geometry I family");
  int a=random.Next(2,9),b=random.Next(2,9),c=random.Next(2,7);
  if(family is LineRelation)a=new[]{0,45,90}[random.Next(3)];
  if(family is RightComplement)a=random.Next(0,91);
  if(family is Axes)a=random.Next(1,5);
  if(family is SolidFaces or SolidVertices or G5SolidType or G5SolidFaces or G5SolidEdges or G5SolidVertices)a=random.Next(1,4);
  if(family is G5NetType or G5NetFaces or G5NetMatching)a=random.Next(1,3);
  if(family is NetMissing)a=random.Next(2,6);
  var p=new Dictionary<string,int>{{"a",a},{"b",b},{"c",c}};
  string solid=a==1?"مكعب":a==2?"منشور مستطيل":"منشور ثلاثي";
  string solidDescription=a switch
  {
    1=>$"مجسم جميع أوجهه ستة مربعات متطابقة طول ضلع المربع {b} سم",
    2=>$"مجسم بقاعدتين مستطيلتين متطابقتين وأربعة أوجه جانبية مستطيلة؛ أبعاده {b} سم و{b+c} سم و{b+2*c} سم",
    _=>$"مجسم بقاعدتين مثلثيتين متطابقتين وثلاثة أوجه جانبية مستطيلة، طول إحدى حواف القاعدة {b} سم والارتفاع {c} سم"
  };
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
   G5SolidType=>$"وصف المجسم: {solidDescription}. اختر نوعه: ٠ مكعب، ١ منشور مستطيل القاعدة غير مكعب، ٢ منشور ثلاثي.",
   G5SolidFaces=>$"وصف المجسم: {solidDescription}. كم وجهًا لهذا المجسم؟",
   G5SolidEdges=>$"وصف المجسم: {solidDescription}. كم حرفًا (حافة) لهذا المجسم؟",
   G5SolidVertices=>$"وصف المجسم: {solidDescription}. كم رأسًا لهذا المجسم؟",
   CubeCount=>$"منشور مستطيل طوله {a} مكعبات وعرضه {b} مكعبات وارتفاعه {c} طبقات. كم مكعب وحدة يحتوي؟",
   G5Layer=>$"طبقة في نموذج من مكعبات وحدة طولها {a} مكعبات وعرضها {b} مكعبات. كم مكعبًا في الطبقة الواحدة؟",
   G5CubeTotal=>$"نموذج منشور مستطيل بلا فجوات من مكعبات سنتيمتر مكعب، طوله {a} مكعبات وعرضه {b} وارتفاعه {c} طبقات. كم مكعبًا يلزم لتكوينه؟",
   G5Volume=>$"نموذج منشور بلا فجوات طوله {a} سم وعرضه {b} سم وارتفاعه {c} سم، مبني من مكعبات طول حرفها ١ سم. ما حجمه بالسم المكعب؟",
   G5MissingLayers=>$"حجم منشور مستطيل {a*b*c} سم³ ومساحة قاعدته {a*b} سم². ما ارتفاعه بوحدة السنتيمتر أو عدد طبقات مكعبات ١ سم؟",
   LayerArea=>$"في طبقة من مكعبات الوحدة طولها {a} مكعبات وعرضها {b} مكعبات. كم مربع وحدة في الطبقة الواحدة؟",
   PrismVolume=>$"منشور مستطيل طوله {a} سم وعرضه {b} سم وارتفاعه {c} سم. ما حجمه بالسم المكعب؟",
   G5VolumeLwh=>$"منشور مستطيل طوله {a} سم وعرضه {b} سم وارتفاعه {c} سم. احسب حجمه بالسم المكعب.",
   G5CompositeBase=>$"نموذج من مكعبات حجم كل منها ١ سم³، طبقة القاعدة مستطيل طوله {a+2} مكعبات وعرضه {b+2} مكعبات. كم مكعبًا في القاعدة؟",
   G5SumPrisms=>$"مجسم مركب من منشورين مستطيلين متصلين عند وجه دون أي تداخل حجمي. أبعاد الأول {a}×{b}×{c} سم، والثاني {a+1}×{b}×{c+1} سم. احسب الحجم الكلي بالسم المكعب.",
   G5ModelTotal=>$"نموذج صندوق من مكعبات وحدة، في كل صف {a} مكعبات وفي كل طبقة {b} صفوف، وهناك {c} طبقات. كم مكعبًا في الصندوق؟",
   G5ModelLayers=>$"صندوق يحتوي {a*b*c} مكعب وحدة، وفي كل طبقة {a*b} مكعبًا. أنشئ نموذجًا من طبقات متساوية، ثم احسب عدد الطبقات.",
   G5ModelLoop=>$"طول مسار دائري مغلق {a*b} مترًا، وتوضع محطة واحدة كل {b} أمتار على المحيط. كم محطة مميزة على المسار دون تكرار نقطة البداية؟",
   G5ModelTiles=>$"مصفوفة مستطيلة فيها {a*b} قطعة بلاط موزعة بالتساوي على {b} صفوف. كم قطعة في كل صف؟",
   G5TopBase=>$"قاعدة صلبة أبعادها {a+2}×{b+2}×{c} سم. فوقها منشور أبعاده {a}×{b}×٢ سم، ولا يوجد تداخل. ما حجم المجسم المركب بالسم المكعب؟",
   G5Cutout=>$"منشور مستطيل كامل أبعاده {a+2}×{b+2}×{c+2} سم، قُطع منه جزء بأبعاد {a}×{b}×{c} سم داخله. ما الحجم الباقي بالسم المكعب؟",
   G5MissingTop=>$"حجم مجسم مركب من قاعدة وكتلة علوية دون تداخل هو {(a+2)*(b+2)*c+2*a*b} سم³. أبعاد القاعدة {a+2}×{b+2}×{c} سم. ما حجم الكتلة العلوية بالسم المكعب؟",
   G5CompositeUpper=>$"فوق قاعدة أكبر وُضعت كتلة إضافية مستقلة أبعادها {a} مكعبات طولًا و{b} عرضًا و{c} طبقات. كم مكعبًا في الجزء العلوي فقط؟",
   G5CompositeTotal=>$"شكل مركب بلا فجوات: قاعدة ارتفاعها طبقة واحدة أبعادها {a+2}×{b+2} مكعبات، وفوقها جزء أبعاده {a}×{b}×{c} طبقات إضافية. كم مكعب وحدة يلزم؟",
   G5CompositeError=>$"قاعدة من {(a+2)*(b+2)} مكعب وحدة وجزء علوي إضافي من {a*b*c} مكعبات بلا تداخل. طالب ضرب العددين بدل جمعهما. ما الإجمالي الصحيح لحجم الشكل بالسم³؟",
   G5VolumeBh=>$"مساحة قاعدة منشور مستطيل {a*b} سم² وارتفاعه {c} سم. احسب حجمه بالسم المكعب.",
   G5PrismHeight=>$"حجم منشور مستطيل {a*b*c} سم³ ومساحة قاعدته {a*b} سم². احسب ارتفاعه بالسنتيمتر.",
   G5VolumeCompare=>$"صندوق أ أبعاده {a} سم و{b+1} سم و{c} سم، وصندوق ب أبعاده {a+1} سم و{b} سم و{c} سم. أيهما أكبر حجمًا؟ اكتب ٠ إذا تساويا، ١ إذا كان أ أكبر، ٢ إذا كان ب أكبر.",
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
  if(f is SolidFaces or SolidVertices or G5SolidType or G5SolidFaces or G5SolidEdges or G5SolidVertices &&
      a is <1 or >3)return false;
  if(f is G5NetType or G5NetFaces or G5NetMatching && a is <1 or >2)return false;
  if(f is NetMissing && a is <1 or >6)return false;
  if(f is Rays or Segments && a<2)return false;
  if(f is CubeCount or LayerArea or PrismVolume or MissingHeight or G5Layer or G5CubeTotal or G5Volume or G5MissingLayers or G5VolumeLwh or G5VolumeBh or G5PrismHeight or G5VolumeCompare or G5CompositeBase or G5CompositeUpper or G5CompositeTotal or G5CompositeError or G5SumPrisms or G5TopBase or G5Cutout or G5MissingTop or G5ModelTotal or G5ModelLayers or G5ModelLoop or G5ModelTiles && a<1)return false;
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
    G5SolidType=>a-1,
    G5SolidFaces=>a==3?5:6,
    G5SolidEdges=>a==3?9:12,
    G5SolidVertices=>a==3?6:8,
    CubeCount or PrismVolume=>a*b*c,
    G5Layer=>a*b,
    G5CubeTotal or G5Volume=>a*b*c,
    G5MissingLayers=>c,
    G5VolumeLwh or G5VolumeBh=>a*b*c,
    G5PrismHeight=>c,
    G5VolumeCompare=>a==b?0:a>b?1:2,
    G5CompositeBase=>(a+2)*(b+2),
    G5CompositeUpper=>a*b*c,
    G5CompositeTotal or G5CompositeError=>(a+2)*(b+2)+a*b*c,
    G5SumPrisms=>a*b*c+(a+1)*b*(c+1),
    G5TopBase=>(a+2)*(b+2)*c+2*a*b,
    G5Cutout=>(a+2)*(b+2)*(c+2)-a*b*c,
    G5MissingTop=>2*a*b,
    G5ModelTotal=>a*b*c,
    G5ModelLayers=>c,
    G5ModelLoop or G5ModelTiles=>a,
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
