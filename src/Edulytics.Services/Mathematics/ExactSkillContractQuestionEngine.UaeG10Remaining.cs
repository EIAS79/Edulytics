using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

// Original bounded closed-response subskills in G10 lessons 58–143.
// Source opening and representative page sampled; teacher-scored proofs and
// complete source-element coverage remain separate Stage D obligations.
public sealed partial class ExactSkillContractQuestionEngine
{
    private static ExactProblem BuildG10Remaining(string family, Random random, ExactSkillQuestionDifficulty difficulty)
    {
        int upper=difficulty==ExactSkillQuestionDifficulty.Challenge?10:8;
        int a=random.Next(2,upper),b=random.Next(2,upper),c=random.Next(2,upper),d=random.Next(2,upper);
        int e=random.Next(1,upper),f=random.Next(2,upper),x=a-6,y=b-5;
        string prompt=family switch
        {
            UaeG10RemainingResponse.L58_line_pair_type=>$"في مكعب ABCD-EFGH، حدد علاقة المستقيم AB مع المستقيم {new[]{"CD","BC","CG"}[a%3]}؛ اكتب 1 لمتوازيين، 2 لمتقاطعين، 3 لمتخالفين.",
            UaeG10RemainingResponse.L58_plane_pair_type=>$"في مكعب ABCD-EFGH، قارن المستوى ABCD بالمستوى {(a%2==0?"EFGH":"ABFE")}. اكتب 1 إذا كانا متوازيين، و2 إذا كانا متقاطعين.",
            UaeG10RemainingResponse.L63_perpendicular_point_line_distance=>$"ما أقصر مسافة من النقطة ({x},{y}) إلى المستقيم الأفقي y={c}؟",
            UaeG10RemainingResponse.L63_perpendicular_slope=>$"ميل مستقيم {a}/{b}؛ أوجد ميل مستقيم عمودي عليه ككسر.",
            UaeG10RemainingResponse.L64_reflection_vertical_x=>$"انعكس النقطة ({x},{y}) حول المستقيم x={c}. أوجد الإحداثي السيني للصورة.",
            UaeG10RemainingResponse.L64_reflection_horizontal_y=>$"انعكس النقطة ({x},{y}) حول المستقيم y={c}. أوجد الإحداثي الصادي للصورة.",
            UaeG10RemainingResponse.L64_reflection_diagonal_y=>$"انعكس النقطة ({x},{y}) حول المستقيم y=x. أوجد الإحداثي الصادي للصورة.",
            UaeG10RemainingResponse.L65_translation_x=>$"أزح النقطة ({x},{y}) بالمتجه ({c},{-d}). اكتب الإحداثي السيني للصورة.",
            UaeG10RemainingResponse.L65_translation_y=>$"أزح النقطة ({x},{y}) بالمتجه ({c},{-d}). اكتب الإحداثي الصادي للصورة.",
            UaeG10RemainingResponse.L66_rotation_90_x=>$"دور النقطة ({x},{y}) بزاوية 90° عكس عقارب الساعة حول الأصل. أوجد الإحداثي السيني.",
            UaeG10RemainingResponse.L66_rotation_180_y=>$"دور النقطة ({x},{y}) بزاوية 180° حول الأصل. أوجد الإحداثي الصادي.",
            UaeG10RemainingResponse.L66_rotation_270_y=>$"دور النقطة ({x},{y}) بزاوية 270° عكس عقارب الساعة حول الأصل. أوجد الإحداثي الصادي.",
            UaeG10RemainingResponse.L67_reflect_then_translate_x=>$"انعكس النقطة ({x},{y}) حول محور y، ثم أزحها {c} وحدات يمينًا. أوجد الإحداثي السيني النهائي.",
            UaeG10RemainingResponse.L67_translate_then_reflect_x=>$"أزح النقطة ({x},{y}) {c} وحدات يمينًا، ثم اعكسها حول محور y. أوجد الإحداثي السيني النهائي.",
            UaeG10RemainingResponse.L68_regular_polygon_symmetry_axes=>$"كم محور تناظر لمضلع منتظم له {a+3} أضلاع؟",
            UaeG10RemainingResponse.L68_least_rotation_angle=>$"ما أصغر زاوية دوران موجبة تعيد مضلعًا منتظمًا ذا {a+3} أضلاع إلى نفسه؟ اكتب القياس بالدرجات كعدد أو كسر.",
            UaeG10RemainingResponse.L69_dilation_origin_x=>$"مدد النقطة ({x},{y}) من الأصل بمعامل مقياس {c}. أوجد الإحداثي السيني.",
            UaeG10RemainingResponse.L69_dilation_side_length=>$"طول قطعة {a} سم. مددها بمعامل مقياس {b}/{c}. أوجد طول الصورة بالسنتيمتر.",
            UaeG10RemainingResponse.L70_classify_triangle_sides=>$"مثلث أضلاعه {a+4}، {a+4}، {a+4-(a%3==0?0:1)}. اكتب 1 لمتساوي الأضلاع، 2 لمتساوي الساقين غير متساوي الأضلاع، 3 لمختلف الأضلاع.",
            UaeG10RemainingResponse.L70_classify_triangle_angles=>$"زوايا مثلث هي {30+a}°، {60-a+(b%3-1)*15}°، {90-(b%3-1)*15}°. اكتب 1 لحاد، 2 لقائم، 3 لمنفرج.",
            UaeG10RemainingResponse.L71_third_triangle_angle=>$"قياس زاويتين في مثلث {25+3*a}° و{30+3*b}°. أوجد قياس الزاوية الثالثة بالدرجات.",
            UaeG10RemainingResponse.L71_exterior_triangle_angle=>$"قياس الزاويتين الداخليتين البعيدتين عن زاوية خارجية لمثلث {20+3*a}° و{35+2*b}°. أوجد قياس الخارجية.",
            UaeG10RemainingResponse.L72_corresponding_side_length=>$"إذا كان △ABC ≅ △DEF بالترتيب، وطول BC={a+b} سم، فما طول EF بالسنتيمتر؟",
            UaeG10RemainingResponse.L72_corresponding_side_equation=>$"إذا كان △ABC ≅ △DEF، وكان AB=2x+{b} وDE={2*a+b}، أوجد x.",
            UaeG10RemainingResponse.L73_sss_match_condition=>$"أضلاع مثلث هي {a+5}، {a+6}، {a+7} وأضلاع آخر {a+5}، {a+6}، {a+7+b%2}. هل يثبت SSS تطابقهما؟ اكتب 1 لنعم و0 للا.",
            UaeG10RemainingResponse.L73_sas_included_angle=>$"في مثلثين AB=DE={a+4} وBC=EF={b+4}، والزوايا {(c%2==1?"ABC وDEF":"BAC وEDF")} متطابقة. هل تكفي هذه المعطيات وحدها لتطبيق SAS؟ 1 لنعم و0 للا.",
            UaeG10RemainingResponse.L74_asa_included_side=>$"في △ABC و△DEF، الزاويتان A وD متطابقتان، والزاويتان B وE متطابقتان وقياسهما {30+a}°. والضلع {(b%2==0?"AB يطابق DE":"BC يطابق EF")}. اكتب 1 إذا كانت الحالة ASA و2 إذا كانت AAS.",
            UaeG10RemainingResponse.L74_aas_remaining_angle=>$"مثلثان متطابقان وفق AAS. في الأول زاويتان {25+a}° و{35+b}°. أوجد قياس الزاوية الثالثة في الثاني.",
            UaeG10RemainingResponse.L75_isosceles_base_angle=>$"زاوية رأس مثلث متساوي الساقين قياسها {20+2*a}°. ما قياس كل زاوية قاعدة؟",
            UaeG10RemainingResponse.L75_equilateral_perimeter=>$"ضلع مثلث متساوي الأضلاع طوله {a+b} سم. احسب محيطه بالسنتيمتر.",
            UaeG10RemainingResponse.L76_rigid_image_side_length=>$"قطعة طولها {a+b} سم، أزيحت ثم انعكست حول مستقيم. أوجد طول صورتها بالسنتيمتر.",
            UaeG10RemainingResponse.L76_rigid_image_angle=>$"زاوية قياسها {30+3*a+b}° دارت 90° ثم أزيحت. أوجد قياس زاوية صورتها بالدرجات.",
            UaeG10RemainingResponse.L77_coordinate_side_square=>$"في مثلث رؤوسه A(0,0)، B({a},{b})، C({a},0)، أوجد مربع طول AB.",
            UaeG10RemainingResponse.L77_coordinate_midpoint_x=>$"في مثلث، إحداثيات رأسين ({2*a},{b}) و({-2*c},{d}). أوجد الإحداثي السيني لمنتصف الضلع بينهما.",
            UaeG10RemainingResponse.L78_parallelogram_area=>$"متوازي أضلاع قاعدته {a+b} سم وارتفاعه العمودي {c} سم. أوجد مساحته بالسنتيمتر المربع.",
            UaeG10RemainingResponse.L78_triangle_area=>$"مثلث قاعدته {2*a} سم وارتفاعه {b} سم. أوجد مساحته بالسنتيمتر المربع.",
            UaeG10RemainingResponse.L79_perpendicular_bisector_distance=>$"تقع P على المنصف العمودي للقطعة AB. إذا كان PA={a+b} سم، فما PB؟",
            UaeG10RemainingResponse.L79_angle_bisector_equidistance=>$"تقع P داخل زاوية وعلى منصفها. بعدها العمودي عن أحد ضلعي الزاوية {a+c} سم؛ ما بعدها عن الضلع الآخر؟",
            UaeG10RemainingResponse.L80_centroid_vertex_segment=>$"طول متوسط مثلث من الرأس A إلى منتصف الضلع المقابل {3*a} سم. أوجد طول القطعة من A إلى مركز الثقل.",
            UaeG10RemainingResponse.L80_centroid_midpoint_segment=>$"طول متوسط مثلث {3*(a+b)} سم. أوجد المسافة من مركز الثقل إلى منتصف الضلع.",
            UaeG10RemainingResponse.L81_largest_opposite_angle=>$"أطوال أضلاع مثلث: AB={a+4}، BC={a+6}، CA={a+5}. ما الرأس المقابل لأكبر ضلع؟ 1=A، 2=B، 3=C.",
            UaeG10RemainingResponse.L81_exterior_angle_bound=>$"في مثلث زاويتان داخليتان بعيدتان قياساهما {20+a}° و{30+b}°. كم تزيد الزاوية الخارجية على الزاوية الداخلية الأولى؟",
            UaeG10RemainingResponse.L82_negated_inequality_membership=>$"لبدء برهان غير مباشر للعبارة x > {a} نفترض نفيها. هل القيمة x={a+(b%2==0?-1:1)*c} تحقق فرض النفي؟ 1 لنعم و0 للا.",
            UaeG10RemainingResponse.L82_contradiction_upper_bound=>$"لبرهان x>{a} من المعطى 2x={2*a+b}، نفترض x≤{a}. ما الحد الأعلى لـ 2x الناتج من هذا الفرض المتناقض؟",
            UaeG10RemainingResponse.L83_triangle_third_side_min_integer=>$"طولا ضلعين في مثلث {a} و{b} سم. ما أصغر طول صحيح ممكن للضلع الثالث؟",
            UaeG10RemainingResponse.L83_triangle_third_side_max_integer=>$"طولا ضلعين في مثلث {a} و{b} سم. ما أكبر طول صحيح ممكن للضلع الثالث؟",
            UaeG10RemainingResponse.L84_hinge_opposite_side_relation=>$"في مثلثين ضلعان متناظران بطولي {c+3} و{d+3}. قياس الزاوية المحصورة في الأول {25+5*a}° وفي الثاني {25+5*b}°. قارن طولي الضلع الثالث: 1 للأول أكبر، 0 للتساوي، -1 للأول أصغر.",
            UaeG10RemainingResponse.L84_hinge_converse_angle_relation=>$"مثلثان فيهما ضلعان متناظران طولا كل منهما {a+8} و{a+9}. طول الضلع الثالث في الأول {a+b} وفي الثاني {a+c}. قارن الزاويتين المحصورتين: 1 للأولى أكبر، 0 للتساوي، -1 للأولى أصغر.",
            UaeG10RemainingResponse.L85_proportion_missing_term=>$"حل التناسب {a}/{b}=x/{d}، واكتب x كعدد أو كسر.",
            UaeG10RemainingResponse.L85_partition_by_ratio=>$"قسّم {(a+b)*c} وحدة بنسبة {a}:{b}. أوجد مقدار الجزء الأول.",
            UaeG10RemainingResponse.L86_similar_polygon_side=>$"مضلعان متشابهان. ضلع طوله {a} في الأول يقابله ضلع طوله {b} في الثاني. ما طول الضلع في الثاني الذي يقابل طول {c} في الأول؟",
            UaeG10RemainingResponse.L86_similar_polygon_perimeter=>$"محيط مضلع {a+b+c} سم. مضلع مشابه له معامل المقياس من الأول إلى الثاني {d}. احسب محيط الثاني.",
            UaeG10RemainingResponse.L87_similar_triangle_aa_angle=>$"مثلثان متشابهان بالترتيب ABC وDEF، وقياس A={20+a}° وB={30+b}°. أوجد قياس F.",
            UaeG10RemainingResponse.L87_similar_triangle_corresponding_side=>$"مثلثان متشابهان. ضلع طوله {a} يقابل طول {a*c}؛ فما طول الضلع الذي يقابل طول {b} في الأول؟",
            UaeG10RemainingResponse.L88_parallel_segment_proportion=>$"في △ABC تقع D على AB وE على AC وDE∥BC. إذا كان AD={a} وDB={b} وAE={a*c}، أوجد EC.",
            UaeG10RemainingResponse.L88_triangle_midsegment_length=>$"قطعة تصل بين منتصفي ضلعين في مثلث، وطول الضلع الثالث {a+b} سم. أوجد طول القطعة.",
            UaeG10RemainingResponse.L89_similar_triangle_altitude=>$"مثلثان متشابهان، ضلع متناظر طوله {a} في الأول و{a*c} في الثاني. إذا كان الارتفاع المناظر في الأول {b}، أوجد الارتفاع في الثاني.",
            UaeG10RemainingResponse.L89_similar_triangle_median=>$"مثلثان متشابهان، ضلع متناظر طوله {a} في الأول و{b} في الثاني. طول متوسط مناظر في الأول {c}. أوجد طول المتوسط المناظر في الثاني.",
            UaeG10RemainingResponse.L90_similarity_dilation_translation_x=>$"مدد النقطة ({x},{y}) من الأصل بمعامل {a}، ثم أزح الصورة {c} وحدات يمينًا. أوجد الإحداثي السيني.",
            UaeG10RemainingResponse.L90_similarity_transformed_length=>$"قطعة طولها {a+b} سم، خضعت لدوران ثم تمدد بمعامل {c}/{d}. أوجد طول الصورة.",
            UaeG10RemainingResponse.L91_map_actual_distance_km=>$"مقياس خريطة: 1 سم يمثل {a} كم. المسافة على الخريطة {b+c} سم. أوجد المسافة الفعلية بالكيلومترات.",
            UaeG10RemainingResponse.L91_model_scale_length_cm=>$"مقياس نموذج 1:{10*a}. طول الجسم الحقيقي {b*c*10} سم. أوجد طول النموذج بالسنتيمتر.",
            UaeG10RemainingResponse.L92_polygon_interior_sum=>$"ما مجموع قياسات الزوايا الداخلية لمضلع محدب عدد أضلاعه {a+3}؟",
            UaeG10RemainingResponse.L92_regular_polygon_exterior_angle=>$"أوجد قياس الزاوية الخارجية لمضلع منتظم عدد أضلاعه {a+3}.",
            UaeG10RemainingResponse.L93_parallelogram_adjacent_angle=>$"قياس إحدى زوايا متوازي أضلاع {40+3*a}°. ما قياس زاوية مجاورة لها؟",
            UaeG10RemainingResponse.L93_parallelogram_diagonal_half=>$"قطرا متوازي أضلاع يتقاطعان في O. إذا كان AC={2*(a+b)} سم، فما AO؟",
            UaeG10RemainingResponse.L94_parallelogram_sufficient_condition=>$"شكل رباعي غير متقاطع: {(a%3==0?"قطراه ينصف كل منهما الآخر":a%3==1?"قطراه متطابقان فقط":"كل ضلعين متقابلين متطابقان")}. هل تكفي المعلومة لإثبات أنه متوازي أضلاع؟ 1 لنعم و0 للا.",
            UaeG10RemainingResponse.L94_parallelogram_diagonals_test=>$"قطرا شكل رباعي محدب يتقاطعان في O. AO={a} وOC={a}، وBO={b} وOD={b+c%2}. هل يثبت اختبار تنصيف القطرين أنه متوازي أضلاع؟ 1 لنعم و0 للا.",
            UaeG10RemainingResponse.L95_rectangle_equal_diagonals=>$"في مستطيل ABCD طول القطر AC={a+b+c} سم. أوجد طول القطر BD.",
            UaeG10RemainingResponse.L95_rectangle_diagonal_length=>$"مستطيل طول ضلعيه {3*a} و{4*a} سم. أوجد طول قطره.",
            UaeG10RemainingResponse.L96_rhombus_side_from_diagonals=>$"قطرا معين طولهما {6*a} و{8*a} سم. أوجد طول الضلع.",
            UaeG10RemainingResponse.L96_square_perimeter=>$"مربع طول ضلعه {a+b} سم. أوجد محيطه.",
            UaeG10RemainingResponse.L97_trapezoid_midsegment=>$"طولا قاعدتي شبه منحرف {a+4} و{a+b+4} سم. أوجد طول القطعة المتوسطة.",
            UaeG10RemainingResponse.L97_kite_perimeter=>$"شكل طائرة ورقية فيه زوجان من الأضلاع المتجاورة المتطابقة، طول الزوج الأول {a+2} والثاني {b+3} سم. أوجد المحيط.",
            UaeG10RemainingResponse.L133_sample_space_product_count=>$"تجربة تتكون من اختيار لون من {a} ألوان ثم رمز من {b} رموز. كم ناتجًا مرتبًا في فضاء العينة؟",
            UaeG10RemainingResponse.L133_sample_space_three_stages=>$"تجربة لها {a} خيارات في المرحلة الأولى و{b} في الثانية و{c} في الثالثة، وكل تركيب ممكن. كم ناتجًا؟",
            UaeG10RemainingResponse.L134_permutations_two_positions=>$"من {a+3} أشخاص، نختار رئيسًا ونائبًا مختلفين. كم ترتيبًا ممكنًا؟",
            UaeG10RemainingResponse.L134_combinations_two_members=>$"من {a+3} أشخاص، نختار عضوين دون اعتبار للترتيب. كم لجنة ممكنة؟",
            UaeG10RemainingResponse.L134_combination_probability=>$"صندوق فيه {a} قطع حمراء و{b} زرقاء، نسحب قطعتين دون إرجاع بانتظام. ما احتمال سحب حمراوين؟",
            UaeG10RemainingResponse.L135_geometric_segment_probability=>$"تختار نقطة بانتظام على قطعة طولها {a+b} سم. ما احتمال وقوعها داخل جزء محدد طوله {a} سم؟",
            UaeG10RemainingResponse.L135_geometric_rectangle_probability=>$"تختار نقطة بانتظام داخل مستطيل أبعاده {a+b} و{c+d}. ما احتمال وقوعها داخل مستطيل محتوى أبعاده {a} و{c}؟",
            UaeG10RemainingResponse.L136_simulation_relative_frequency=>$"في محاكاة مستقلة سُجّل نجاح {a} مرات وفشل {b} مرات. أوجد الاحتمال التجريبي للنجاح.",
            UaeG10RemainingResponse.L136_simulation_expected_count=>$"في محاكاة، سجلنا {a} نجاحات من {a+b} محاولات. إذا تكرر المعدل نفسه في {(a+b)*c} محاولة، كم نجاحًا نتوقع تقريبًا؟",
            UaeG10RemainingResponse.L138_independent_two_events=>$"صندوق يحوي {a} حمراء و{b} زرقاء. نسحب مرتين مع الإرجاع بانتظام. ما احتمال حمراوين؟",
            UaeG10RemainingResponse.L138_dependent_without_replacement=>$"صندوق يحوي {a} حمراء و{b} زرقاء. نسحب مرتين دون إرجاع بانتظام. ما احتمال حمراء ثم زرقاء؟",
            UaeG10RemainingResponse.L139_mutually_exclusive_union=>$"في فضاء عينة متساوي الاحتمال يضم {a+b+c} نتائج، للحدث A عدد {a} وللحدث B عدد {b} من النتائج، ولا تداخل. أوجد P(A∪B).",
            UaeG10RemainingResponse.L139_overlapping_union=>$"فضاء عينة فيه {a+b+c+d} نتائج. للحدث A عدد {a+c} وللحدث B عدد {b+c} واشتراكهما {c}. أوجد احتمال اتحادهما.",
            UaeG10RemainingResponse.L140_sequence_function_term=>$"متتالية معرفة بالدالة aₙ={a}n+{b} حيث n عدد صحيح موجب. أوجد a_{c}.",
            UaeG10RemainingResponse.L140_sequence_common_difference=>$"متتالية تبدأ بـ {a}، {a+b}، {a+2*b}، {a+3*b}. ما فرقها المشترك؟",
            UaeG10RemainingResponse.L141_arithmetic_nth_term=>$"متتالية حسابية حدها الأول {a} وفرقها {b}. أوجد الحد رقم {c}.",
            UaeG10RemainingResponse.L141_arithmetic_finite_sum=>$"متسلسلة حسابية حدها الأول {a} وفرقها {b} وعدد حدودها {c}. أوجد مجموعها.",
            UaeG10RemainingResponse.L142_geometric_nth_term=>$"متتالية هندسية حدها الأول {a} وأساسها {2+b%3}. أوجد الحد رقم {2+c%4}.",
            UaeG10RemainingResponse.L142_geometric_finite_sum=>$"متسلسلة هندسية حدها الأول {a} وأساسها {2+b%3} وعدد حدودها {2+c%4}. أوجد مجموعها.",
            UaeG10RemainingResponse.L143_geometric_infinite_sum=>$"متسلسلة هندسية لانهائية حدها الأول {a} وأساسها 1/{b}. أوجد مجموعها.",
            UaeG10RemainingResponse.L143_geometric_infinite_alternating_sum=>$"متسلسلة هندسية لانهائية حدها الأول {a} وأساسها −1/{b}. أوجد مجموعها.",
            UaeG10RemainingResponse.L143_geometric_infinite_convergence=>$"متسلسلة هندسية حدها الأول {a} وأساسها {(b%2==0?"1/"+c:c.ToString())}. هل تتقارب؟ 1 لنعم و0 للا.",
            _=>throw new InvalidOperationException("Unsupported G10 source-scoped candidate family.")
        };
        return Problem(family,prompt,"حدد المعطيات المطلوبة، واختر العلاقة أو النظرية المناسبة. تحقق من شروط صلاحيتها، ثم احسب القيمة أو اختر الحالة المطلوبة.",
            UaeG10RemainingResponse.IsFraction(family)?AssessmentItemType.ShortAnswer:AssessmentItemType.Numeric,
            ("a",a),("b",b),("c",c),("d",d),("e",e),("f",f));
    }
}

internal static class UaeG10RemainingResponse
{
    internal const string L58_line_pair_type="uae.g10.general.u07.l01.line_pair_type";
    internal const string L58_plane_pair_type="uae.g10.general.u07.l01.plane_pair_type";
    internal const string L63_perpendicular_point_line_distance="uae.g10.general.u07.l06.perpendicular_point_line_distance";
    internal const string L63_perpendicular_slope="uae.g10.general.u07.l06.perpendicular_slope";
    internal const string L64_reflection_vertical_x="uae.g10.general.u08.l01.reflection_vertical_x";
    internal const string L64_reflection_horizontal_y="uae.g10.general.u08.l01.reflection_horizontal_y";
    internal const string L64_reflection_diagonal_y="uae.g10.general.u08.l01.reflection_diagonal_y";
    internal const string L65_translation_x="uae.g10.general.u08.l02.translation_x";
    internal const string L65_translation_y="uae.g10.general.u08.l02.translation_y";
    internal const string L66_rotation_90_x="uae.g10.general.u08.l03.rotation_90_x";
    internal const string L66_rotation_180_y="uae.g10.general.u08.l03.rotation_180_y";
    internal const string L66_rotation_270_y="uae.g10.general.u08.l03.rotation_270_y";
    internal const string L67_reflect_then_translate_x="uae.g10.general.u08.l04.reflect_then_translate_x";
    internal const string L67_translate_then_reflect_x="uae.g10.general.u08.l04.translate_then_reflect_x";
    internal const string L68_regular_polygon_symmetry_axes="uae.g10.general.u08.l05.regular_polygon_symmetry_axes";
    internal const string L68_least_rotation_angle="uae.g10.general.u08.l05.least_rotation_angle";
    internal const string L69_dilation_origin_x="uae.g10.general.u08.l06.dilation_origin_x";
    internal const string L69_dilation_side_length="uae.g10.general.u08.l06.dilation_side_length";
    internal const string L70_classify_triangle_sides="uae.g10.general.u09.l01.classify_triangle_sides";
    internal const string L70_classify_triangle_angles="uae.g10.general.u09.l01.classify_triangle_angles";
    internal const string L71_third_triangle_angle="uae.g10.general.u09.l02.third_triangle_angle";
    internal const string L71_exterior_triangle_angle="uae.g10.general.u09.l02.exterior_triangle_angle";
    internal const string L72_corresponding_side_length="uae.g10.general.u09.l03.corresponding_side_length";
    internal const string L72_corresponding_side_equation="uae.g10.general.u09.l03.corresponding_side_equation";
    internal const string L73_sss_match_condition="uae.g10.general.u09.l04.sss_match_condition";
    internal const string L73_sas_included_angle="uae.g10.general.u09.l04.sas_included_angle";
    internal const string L74_asa_included_side="uae.g10.general.u09.l05.asa_included_side";
    internal const string L74_aas_remaining_angle="uae.g10.general.u09.l05.aas_remaining_angle";
    internal const string L75_isosceles_base_angle="uae.g10.general.u09.l06.isosceles_base_angle";
    internal const string L75_equilateral_perimeter="uae.g10.general.u09.l06.equilateral_perimeter";
    internal const string L76_rigid_image_side_length="uae.g10.general.u09.l07.rigid_image_side_length";
    internal const string L76_rigid_image_angle="uae.g10.general.u09.l07.rigid_image_angle";
    internal const string L77_coordinate_side_square="uae.g10.general.u09.l08.coordinate_side_square";
    internal const string L77_coordinate_midpoint_x="uae.g10.general.u09.l08.coordinate_midpoint_x";
    internal const string L78_parallelogram_area="uae.g10.general.u09.l09.parallelogram_area";
    internal const string L78_triangle_area="uae.g10.general.u09.l09.triangle_area";
    internal const string L79_perpendicular_bisector_distance="uae.g10.general.u10.l01.perpendicular_bisector_distance";
    internal const string L79_angle_bisector_equidistance="uae.g10.general.u10.l01.angle_bisector_equidistance";
    internal const string L80_centroid_vertex_segment="uae.g10.general.u10.l02.centroid_vertex_segment";
    internal const string L80_centroid_midpoint_segment="uae.g10.general.u10.l02.centroid_midpoint_segment";
    internal const string L81_largest_opposite_angle="uae.g10.general.u10.l03.largest_opposite_angle";
    internal const string L81_exterior_angle_bound="uae.g10.general.u10.l03.exterior_angle_bound";
    internal const string L82_negated_inequality_membership="uae.g10.general.u10.l04.negated_inequality_membership";
    internal const string L82_contradiction_upper_bound="uae.g10.general.u10.l04.contradiction_upper_bound";
    internal const string L83_triangle_third_side_min_integer="uae.g10.general.u10.l05.triangle_third_side_min_integer";
    internal const string L83_triangle_third_side_max_integer="uae.g10.general.u10.l05.triangle_third_side_max_integer";
    internal const string L84_hinge_opposite_side_relation="uae.g10.general.u10.l06.hinge_opposite_side_relation";
    internal const string L84_hinge_converse_angle_relation="uae.g10.general.u10.l06.hinge_converse_angle_relation";
    internal const string L85_proportion_missing_term="uae.g10.general.u11.l01.proportion_missing_term";
    internal const string L85_partition_by_ratio="uae.g10.general.u11.l01.partition_by_ratio";
    internal const string L86_similar_polygon_side="uae.g10.general.u11.l02.similar_polygon_side";
    internal const string L86_similar_polygon_perimeter="uae.g10.general.u11.l02.similar_polygon_perimeter";
    internal const string L87_similar_triangle_aa_angle="uae.g10.general.u11.l03.similar_triangle_aa_angle";
    internal const string L87_similar_triangle_corresponding_side="uae.g10.general.u11.l03.similar_triangle_corresponding_side";
    internal const string L88_parallel_segment_proportion="uae.g10.general.u11.l04.parallel_segment_proportion";
    internal const string L88_triangle_midsegment_length="uae.g10.general.u11.l04.triangle_midsegment_length";
    internal const string L89_similar_triangle_altitude="uae.g10.general.u11.l05.similar_triangle_altitude";
    internal const string L89_similar_triangle_median="uae.g10.general.u11.l05.similar_triangle_median";
    internal const string L90_similarity_dilation_translation_x="uae.g10.general.u11.l06.similarity_dilation_translation_x";
    internal const string L90_similarity_transformed_length="uae.g10.general.u11.l06.similarity_transformed_length";
    internal const string L91_map_actual_distance_km="uae.g10.general.u11.l07.map_actual_distance_km";
    internal const string L91_model_scale_length_cm="uae.g10.general.u11.l07.model_scale_length_cm";
    internal const string L92_polygon_interior_sum="uae.g10.general.u12.l01.polygon_interior_sum";
    internal const string L92_regular_polygon_exterior_angle="uae.g10.general.u12.l01.regular_polygon_exterior_angle";
    internal const string L93_parallelogram_adjacent_angle="uae.g10.general.u12.l02.parallelogram_adjacent_angle";
    internal const string L93_parallelogram_diagonal_half="uae.g10.general.u12.l02.parallelogram_diagonal_half";
    internal const string L94_parallelogram_sufficient_condition="uae.g10.general.u12.l03.parallelogram_sufficient_condition";
    internal const string L94_parallelogram_diagonals_test="uae.g10.general.u12.l03.parallelogram_diagonals_test";
    internal const string L95_rectangle_equal_diagonals="uae.g10.general.u12.l04.rectangle_equal_diagonals";
    internal const string L95_rectangle_diagonal_length="uae.g10.general.u12.l04.rectangle_diagonal_length";
    internal const string L96_rhombus_side_from_diagonals="uae.g10.general.u12.l05.rhombus_side_from_diagonals";
    internal const string L96_square_perimeter="uae.g10.general.u12.l05.square_perimeter";
    internal const string L97_trapezoid_midsegment="uae.g10.general.u12.l06.trapezoid_midsegment";
    internal const string L97_kite_perimeter="uae.g10.general.u12.l06.kite_perimeter";
    internal const string L133_sample_space_product_count="uae.g10.general.u16.l01.sample_space_product_count";
    internal const string L133_sample_space_three_stages="uae.g10.general.u16.l01.sample_space_three_stages";
    internal const string L134_permutations_two_positions="uae.g10.general.u16.l02.permutations_two_positions";
    internal const string L134_combinations_two_members="uae.g10.general.u16.l02.combinations_two_members";
    internal const string L134_combination_probability="uae.g10.general.u16.l02.combination_probability";
    internal const string L135_geometric_segment_probability="uae.g10.general.u16.l03.geometric_segment_probability";
    internal const string L135_geometric_rectangle_probability="uae.g10.general.u16.l03.geometric_rectangle_probability";
    internal const string L136_simulation_relative_frequency="uae.g10.general.u16.l04.simulation_relative_frequency";
    internal const string L136_simulation_expected_count="uae.g10.general.u16.l04.simulation_expected_count";
    internal const string L138_independent_two_events="uae.g10.general.u16.l06.independent_two_events";
    internal const string L138_dependent_without_replacement="uae.g10.general.u16.l06.dependent_without_replacement";
    internal const string L139_mutually_exclusive_union="uae.g10.general.u16.l07.mutually_exclusive_union";
    internal const string L139_overlapping_union="uae.g10.general.u16.l07.overlapping_union";
    internal const string L140_sequence_function_term="uae.g10.general.u17.l01.sequence_function_term";
    internal const string L140_sequence_common_difference="uae.g10.general.u17.l01.sequence_common_difference";
    internal const string L141_arithmetic_nth_term="uae.g10.general.u17.l02.arithmetic_nth_term";
    internal const string L141_arithmetic_finite_sum="uae.g10.general.u17.l02.arithmetic_finite_sum";
    internal const string L142_geometric_nth_term="uae.g10.general.u17.l03.geometric_nth_term";
    internal const string L142_geometric_finite_sum="uae.g10.general.u17.l03.geometric_finite_sum";
    internal const string L143_geometric_infinite_sum="uae.g10.general.u17.l04.geometric_infinite_sum";
    internal const string L143_geometric_infinite_alternating_sum="uae.g10.general.u17.l04.geometric_infinite_alternating_sum";
    internal const string L143_geometric_infinite_convergence="uae.g10.general.u17.l04.geometric_infinite_convergence";
    internal static bool Supports(string? family)=>family is L58_line_pair_type or L58_plane_pair_type or L63_perpendicular_point_line_distance or L63_perpendicular_slope or L64_reflection_vertical_x or L64_reflection_horizontal_y or L64_reflection_diagonal_y or L65_translation_x or L65_translation_y or L66_rotation_90_x or L66_rotation_180_y or L66_rotation_270_y or L67_reflect_then_translate_x or L67_translate_then_reflect_x or L68_regular_polygon_symmetry_axes or L68_least_rotation_angle or L69_dilation_origin_x or L69_dilation_side_length or L70_classify_triangle_sides or L70_classify_triangle_angles or L71_third_triangle_angle or L71_exterior_triangle_angle or L72_corresponding_side_length or L72_corresponding_side_equation or L73_sss_match_condition or L73_sas_included_angle or L74_asa_included_side or L74_aas_remaining_angle or L75_isosceles_base_angle or L75_equilateral_perimeter or L76_rigid_image_side_length or L76_rigid_image_angle or L77_coordinate_side_square or L77_coordinate_midpoint_x or L78_parallelogram_area or L78_triangle_area or L79_perpendicular_bisector_distance or L79_angle_bisector_equidistance or L80_centroid_vertex_segment or L80_centroid_midpoint_segment or L81_largest_opposite_angle or L81_exterior_angle_bound or L82_negated_inequality_membership or L82_contradiction_upper_bound or L83_triangle_third_side_min_integer or L83_triangle_third_side_max_integer or L84_hinge_opposite_side_relation or L84_hinge_converse_angle_relation or L85_proportion_missing_term or L85_partition_by_ratio or L86_similar_polygon_side or L86_similar_polygon_perimeter or L87_similar_triangle_aa_angle or L87_similar_triangle_corresponding_side or L88_parallel_segment_proportion or L88_triangle_midsegment_length or L89_similar_triangle_altitude or L89_similar_triangle_median or L90_similarity_dilation_translation_x or L90_similarity_transformed_length or L91_map_actual_distance_km or L91_model_scale_length_cm or L92_polygon_interior_sum or L92_regular_polygon_exterior_angle or L93_parallelogram_adjacent_angle or L93_parallelogram_diagonal_half or L94_parallelogram_sufficient_condition or L94_parallelogram_diagonals_test or L95_rectangle_equal_diagonals or L95_rectangle_diagonal_length or L96_rhombus_side_from_diagonals or L96_square_perimeter or L97_trapezoid_midsegment or L97_kite_perimeter or L133_sample_space_product_count or L133_sample_space_three_stages or L134_permutations_two_positions or L134_combinations_two_members or L134_combination_probability or L135_geometric_segment_probability or L135_geometric_rectangle_probability or L136_simulation_relative_frequency or L136_simulation_expected_count or L138_independent_two_events or L138_dependent_without_replacement or L139_mutually_exclusive_union or L139_overlapping_union or L140_sequence_function_term or L140_sequence_common_difference or L141_arithmetic_nth_term or L141_arithmetic_finite_sum or L142_geometric_nth_term or L142_geometric_finite_sum or L143_geometric_infinite_sum or L143_geometric_infinite_alternating_sum or L143_geometric_infinite_convergence;
    internal static bool IsFraction(string family)=>family is L63_perpendicular_slope or L68_least_rotation_angle or L69_dilation_side_length or L85_proportion_missing_term or L86_similar_polygon_side or L88_triangle_midsegment_length or L89_similar_triangle_median or L90_similarity_transformed_length or L91_model_scale_length_cm or L92_regular_polygon_exterior_angle or L97_trapezoid_midsegment or L134_combination_probability or L135_geometric_segment_probability or L135_geometric_rectangle_probability or L136_simulation_relative_frequency or L138_independent_two_events or L138_dependent_without_replacement or L139_mutually_exclusive_union or L139_overlapping_union or L143_geometric_infinite_sum or L143_geometric_infinite_alternating_sum;
    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)=>
        Supports(family) && p.Count==6 && new[]{"a","b","c","d","e","f"}.All(p.ContainsKey) &&
        p["a"] is >=2 and <=9 && p["b"] is >=2 and <=9 && p["c"] is >=2 and <=9 &&
        p["d"] is >=2 and <=9 && p["e"] is >=1 and <=9 && p["f"] is >=2 and <=9;

    private static long Gcd(long a,long b)
    {
        a=Math.Abs(a); b=Math.Abs(b);
        while(b!=0){long r=a%b;a=b;b=r;}
        return a;
    }
    private static long Power(long a,long exponent)=>checked((long)Math.Pow(a,exponent));
    private static long PowerByLoop(long a,long exponent)
    {
        long product=1;
        for(long i=0;i<exponent;i++)product*=a;
        return product;
    }

    internal static string Format(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid bounded G10 parameters.");
        long a=p["a"],b=p["b"],c=p["c"],d=p["d"],x=a-6,y=b-5;
        (long numerator,long denominator)=family switch
        {
            L58_line_pair_type=>(1+a%3,1),
            L58_plane_pair_type=>(a%2==0?1:2,1),
            L63_perpendicular_point_line_distance=>(Math.Abs(y-c),1),
            L63_perpendicular_slope=>(-b,a),
            L64_reflection_vertical_x=>(2*c-x,1),
            L64_reflection_horizontal_y=>(2*c-y,1),
            L64_reflection_diagonal_y=>(x,1),
            L65_translation_x=>(x+c,1),
            L65_translation_y=>(y-d,1),
            L66_rotation_90_x=>(-y,1),
            L66_rotation_180_y=>(-y,1),
            L66_rotation_270_y=>(-x,1),
            L67_reflect_then_translate_x=>(-x+c,1),
            L67_translate_then_reflect_x=>(-x-c,1),
            L68_regular_polygon_symmetry_axes=>(a+3,1),
            L68_least_rotation_angle=>(360,a+3),
            L69_dilation_origin_x=>(c*x,1),
            L69_dilation_side_length=>(a*b,c),
            L70_classify_triangle_sides=>(a%3==0?1:2,1),
            L70_classify_triangle_angles=>(b%3==0?3:b%3==1?2:1,1),
            L71_third_triangle_angle=>(125-3*a-3*b,1),
            L71_exterior_triangle_angle=>(55+3*a+2*b,1),
            L72_corresponding_side_length=>(a+b,1),
            L72_corresponding_side_equation=>(a,1),
            L73_sss_match_condition=>(b%2==0?1:0,1),
            L73_sas_included_angle=>(c%2==1?1:0,1),
            L74_asa_included_side=>(b%2==0?1:2,1),
            L74_aas_remaining_angle=>(120-a-b,1),
            L75_isosceles_base_angle=>(80-a,1),
            L75_equilateral_perimeter=>(3*(a+b),1),
            L76_rigid_image_side_length=>(a+b,1),
            L76_rigid_image_angle=>(30+3*a+b,1),
            L77_coordinate_side_square=>(a*a+b*b,1),
            L77_coordinate_midpoint_x=>(a-c,1),
            L78_parallelogram_area=>((a+b)*c,1),
            L78_triangle_area=>(a*b,1),
            L79_perpendicular_bisector_distance=>(a+b,1),
            L79_angle_bisector_equidistance=>(a+c,1),
            L80_centroid_vertex_segment=>(2*a,1),
            L80_centroid_midpoint_segment=>(a+b,1),
            L81_largest_opposite_angle=>(1,1),
            L81_exterior_angle_bound=>(30+b,1),
            L82_negated_inequality_membership=>(b%2==0?1:0,1),
            L82_contradiction_upper_bound=>(2*a,1),
            L83_triangle_third_side_min_integer=>(Math.Abs(a-b)+1,1),
            L83_triangle_third_side_max_integer=>(a+b-1,1),
            L84_hinge_opposite_side_relation=>(Math.Sign(a-b),1),
            L84_hinge_converse_angle_relation=>(Math.Sign(b-c),1),
            L85_proportion_missing_term=>(a*d,b),
            L85_partition_by_ratio=>(a*c,1),
            L86_similar_polygon_side=>(b*c,a),
            L86_similar_polygon_perimeter=>((a+b+c)*d,1),
            L87_similar_triangle_aa_angle=>(130-a-b,1),
            L87_similar_triangle_corresponding_side=>(b*c,1),
            L88_parallel_segment_proportion=>(b*c,1),
            L88_triangle_midsegment_length=>(a+b,2),
            L89_similar_triangle_altitude=>(b*c,1),
            L89_similar_triangle_median=>(b*c,a),
            L90_similarity_dilation_translation_x=>(a*x+c,1),
            L90_similarity_transformed_length=>((a+b)*c,d),
            L91_map_actual_distance_km=>(a*(b+c),1),
            L91_model_scale_length_cm=>(b*c,a),
            L92_polygon_interior_sum=>(180*(a+1),1),
            L92_regular_polygon_exterior_angle=>(360,a+3),
            L93_parallelogram_adjacent_angle=>(140-3*a,1),
            L93_parallelogram_diagonal_half=>(a+b,1),
            L94_parallelogram_sufficient_condition=>(a%3==1?0:1,1),
            L94_parallelogram_diagonals_test=>(c%2==0?1:0,1),
            L95_rectangle_equal_diagonals=>(a+b+c,1),
            L95_rectangle_diagonal_length=>(5*a,1),
            L96_rhombus_side_from_diagonals=>(5*a,1),
            L96_square_perimeter=>(4*(a+b),1),
            L97_trapezoid_midsegment=>(2*a+b+8,2),
            L97_kite_perimeter=>(2*(a+b+5),1),
            L133_sample_space_product_count=>(a*b,1),
            L133_sample_space_three_stages=>(a*b*c,1),
            L134_permutations_two_positions=>((a+3)*(a+2),1),
            L134_combinations_two_members=>((a+3)*(a+2)/2,1),
            L134_combination_probability=>(a*(a-1),(a+b)*(a+b-1)),
            L135_geometric_segment_probability=>(a,a+b),
            L135_geometric_rectangle_probability=>(a*c,(a+b)*(c+d)),
            L136_simulation_relative_frequency=>(a,a+b),
            L136_simulation_expected_count=>(a*c,1),
            L138_independent_two_events=>(a*a,(a+b)*(a+b)),
            L138_dependent_without_replacement=>(a*b,(a+b)*(a+b-1)),
            L139_mutually_exclusive_union=>(a+b,a+b+c),
            L139_overlapping_union=>(a+b+c,a+b+c+d),
            L140_sequence_function_term=>(a*c+b,1),
            L140_sequence_common_difference=>(b,1),
            L141_arithmetic_nth_term=>(a+(c-1)*b,1),
            L141_arithmetic_finite_sum=>(c*(2*a+(c-1)*b)/2,1),
            L142_geometric_nth_term=>(a*Power(2+b%3,1+c%4),1),
            L142_geometric_finite_sum=>(a*(Power(2+b%3,2+c%4)-1)/(1+b%3),1),
            L143_geometric_infinite_sum=>(a*b,b-1),
            L143_geometric_infinite_alternating_sum=>(a*b,b+1),
            L143_geometric_infinite_convergence=>(b%2==0?1:0,1),
            _=>throw new InvalidOperationException()
        };
        long g=Gcd(numerator,denominator); numerator/=g; denominator/=g;
        return denominator==1?numerator.ToString(CultureInfo.InvariantCulture):
            numerator.ToString(CultureInfo.InvariantCulture)+"/"+denominator.ToString(CultureInfo.InvariantCulture);
    }

    internal static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>48)return false;
        string raw=string.Concat(answer.Trim().Select(ch=>ch switch
        {
            >= '٠' and <= '٩'=>(char)('0'+ch-'٠'),
            >= '۰' and <= '۹'=>(char)('0'+ch-'۰'),
            '−' or '–'=>'-', '⁄' or '∕'=>'/', _=>ch
        }));
        var parts=raw.Split('/');
        if(parts.Length is <1 or >2 || (!IsFraction(family)&&parts.Length!=1))return false;
        if(!long.TryParse(parts[0],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out long n))return false;
        long q=1;
        if(parts.Length==2&&!long.TryParse(parts[1],NumberStyles.AllowLeadingSign,CultureInfo.InvariantCulture,out q))return false;
        if(n is < -1000000 or >1000000 || q is <=0 or >1000000)return false;
        long a=p["a"],b=p["b"],c=p["c"],d=p["d"],x=a-6,y=b-5;
        // Algebraic identities, enumeration and direct theorem conditions,
        // rather than string comparison with the generator answer.
        return family switch
        {
            L58_line_pair_type=>n==q*(1+a%3),
            L58_plane_pair_type=>n==q*(a%2==0?1:2),
            L63_perpendicular_point_line_distance=>n*n==q*q*(y-c)*(y-c)&&n>=0,
            L63_perpendicular_slope=>n*a+q*b==0,
            L64_reflection_vertical_x=>n+q*x==2*q*c,
            L64_reflection_horizontal_y=>n+q*y==2*q*c,
            L64_reflection_diagonal_y=>n==q*x,
            L65_translation_x=>n-q*x==q*c,
            L65_translation_y=>n-q*y==-q*d,
            L66_rotation_90_x=>n+q*y==0,
            L66_rotation_180_y=>n+q*y==0,
            L66_rotation_270_y=>n+q*x==0,
            L67_reflect_then_translate_x=>n+q*x==q*c,
            L67_translate_then_reflect_x=>n+q*x+q*c==0,
            L68_regular_polygon_symmetry_axes=>n-q*a==3*q,
            L68_least_rotation_angle=>n*(a+3)==360*q,
            L69_dilation_origin_x=>n==q*c*x,
            L69_dilation_side_length=>n*c==q*a*b,
            L70_classify_triangle_sides=>n==q*(a%3==0?1:2),
            L70_classify_triangle_angles=>n==q*(b%3==0?3:b%3==1?2:1),
            L71_third_triangle_angle=>n+q*(25+3*a)+q*(30+3*b)==180*q,
            L71_exterior_triangle_angle=>n==q*(20+3*a)+q*(35+2*b),
            L72_corresponding_side_length=>n-q*a==q*b,
            L72_corresponding_side_equation=>2*n+q*b==q*(2*a+b),
            L73_sss_match_condition=>n==q*(a+7==a+7+b%2?1:0),
            L73_sas_included_angle=>n==q*(c%2==1?1:0),
            L74_asa_included_side=>n==q*(b%2==0?1:2),
            L74_aas_remaining_angle=>n+q*(25+a)+q*(35+b)==180*q,
            L75_isosceles_base_angle=>2*n+q*(20+2*a)==180*q,
            L75_equilateral_perimeter=>n==3*q*a+3*q*b,
            L76_rigid_image_side_length=>n-q*a==q*b,
            L76_rigid_image_angle=>n-q*(3*a+b)==30*q,
            L77_coordinate_side_square=>n-q*a*a==q*b*b,
            L77_coordinate_midpoint_x=>2*n==q*(2*a-2*c),
            L78_parallelogram_area=>n==q*a*c+q*b*c,
            L78_triangle_area=>2*n==q*2*a*b,
            L79_perpendicular_bisector_distance=>n-q*a==q*b,
            L79_angle_bisector_equidistance=>n-q*a==q*c,
            L80_centroid_vertex_segment=>3*n==2*q*(3*a),
            L80_centroid_midpoint_segment=>3*n==q*3*(a+b),
            L81_largest_opposite_angle=>n==q,
            L81_exterior_angle_bound=>n+q*(20+a)==q*(50+a+b),
            L82_negated_inequality_membership=>n==q*(a+(b%2==0?-1:1)*c<=a?1:0),
            L82_contradiction_upper_bound=>n==2*q*a,
            L83_triangle_third_side_min_integer=>n==q*(Math.Max(a,b)-Math.Min(a,b)+1),
            L83_triangle_third_side_max_integer=>n+q==q*a+q*b,
            L84_hinge_opposite_side_relation=>n==q*(a>b?1:a==b?0:-1),
            L84_hinge_converse_angle_relation=>n==q*(a+b>a+c?1:a+b==a+c?0:-1),
            L85_proportion_missing_term=>n*b==q*a*d,
            L85_partition_by_ratio=>n*(a+b)==q*a*(a+b)*c,
            L86_similar_polygon_side=>n*a==q*b*c,
            L86_similar_polygon_perimeter=>n==q*a*d+q*b*d+q*c*d,
            L87_similar_triangle_aa_angle=>n+q*(20+a)+q*(30+b)==180*q,
            L87_similar_triangle_corresponding_side=>n*a==q*b*a*c,
            L88_parallel_segment_proportion=>n*a==q*b*a*c,
            L88_triangle_midsegment_length=>2*n==q*(a+b),
            L89_similar_triangle_altitude=>n*a==q*b*a*c,
            L89_similar_triangle_median=>n*a==q*b*c,
            L90_similarity_dilation_translation_x=>n-q*c==q*a*x,
            L90_similarity_transformed_length=>n*d==q*(a+b)*c,
            L91_map_actual_distance_km=>n==q*a*b+q*a*c,
            L91_model_scale_length_cm=>n*(10*a)==q*b*c*10,
            L92_polygon_interior_sum=>n==180*q*((a+3)-2),
            L92_regular_polygon_exterior_angle=>n*(a+3)==360*q,
            L93_parallelogram_adjacent_angle=>n+q*(40+3*a)==180*q,
            L93_parallelogram_diagonal_half=>2*n==q*2*(a+b),
            L94_parallelogram_sufficient_condition=>n==q*(a%3==1?0:1),
            L94_parallelogram_diagonals_test=>n==q*(b==b+c%2?1:0),
            L95_rectangle_equal_diagonals=>n-q*a-q*b==q*c,
            L95_rectangle_diagonal_length=>n*n==q*q*(9*a*a+16*a*a)&&n>0,
            L96_rhombus_side_from_diagonals=>n*n==q*q*(9*a*a+16*a*a)&&n>0,
            L96_square_perimeter=>n==4*q*a+4*q*b,
            L97_trapezoid_midsegment=>2*n==q*(a+4)+q*(a+b+4),
            L97_kite_perimeter=>n==q*(a+2)+q*(a+2)+q*(b+3)+q*(b+3),
            L133_sample_space_product_count=>n==q*Enumerable.Range(1,(int)a).Sum(_=>b),
            L133_sample_space_three_stages=>n==q*Enumerable.Range(1,(int)a).Sum(_=>b*c),
            L134_permutations_two_positions=>n==q*Enumerable.Range(1,(int)a+3).Sum(_=>a+2),
            L134_combinations_two_members=>n==q*Enumerable.Range(1,(int)a+2).Sum(i=>(long)i),
            L134_combination_probability=>n*(a+b)*(a+b-1)==q*a*(a-1),
            L135_geometric_segment_probability=>n*(a+b)==q*a,
            L135_geometric_rectangle_probability=>n*(a+b)*(c+d)==q*a*c,
            L136_simulation_relative_frequency=>n*(a+b)==q*a,
            L136_simulation_expected_count=>n*(a+b)==q*a*(a+b)*c,
            L138_independent_two_events=>n*(a+b)*(a+b)==q*a*a,
            L138_dependent_without_replacement=>n*(a+b)*(a+b-1)==q*a*b,
            L139_mutually_exclusive_union=>n*(a+b+c)==q*a+q*b,
            L139_overlapping_union=>n*(a+b+c+d)==q*(a+c)+q*(b+c)-q*c,
            L140_sequence_function_term=>n-q*b==q*a*c,
            L140_sequence_common_difference=>n+q*(a+b)==q*(a+2*b),
            L141_arithmetic_nth_term=>n-q*a==q*(c-1)*b,
            L141_arithmetic_finite_sum=>n==q*Enumerable.Range(0,(int)c).Sum(i=>a+i*b),
            L142_geometric_nth_term=>n==q*a*PowerByLoop(2+b%3,1+c%4),
            L142_geometric_finite_sum=>n==q*Enumerable.Range(0,(int)(2+c%4)).Sum(i=>a*PowerByLoop(2+b%3,i)),
            L143_geometric_infinite_sum=>n*(b-1)==q*a*b,
            L143_geometric_infinite_alternating_sum=>n*(b+1)==q*a*b,
            L143_geometric_infinite_convergence=>n==q*(b%2==0?1:0),
            _=>false
        };
    }
}
