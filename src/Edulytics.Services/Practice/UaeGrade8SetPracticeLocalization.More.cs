using Edulytics.Services.Mathematics;
using System.Text.RegularExpressions;

namespace Edulytics.Services.Practice;

/// <summary>
/// Additional UAE Grade 8 Arabic-only learner text for proven exact question
/// templates. Each anchored pattern must preserve the mathematical parameters
/// and response meaning. Unknown variants deliberately remain unmodified and
/// must fail the academic-language release gate.
/// </summary>
internal static partial class UaeGrade8SetPracticeLocalization
{
    private static string? AdditionalGrade8Prompt(ExactSkillGeneratedQuestion q)
    {
        var text = q.Prompt;
        var translated = q.Family switch
        {
            "algebra.linear.variables_both_sides" =>
                ConvertPrompt(text, @"^Solve (.+)\.$",
                    m => $"حل المعادلة التي تحتوي على المتغير في طرفيها: {m.Groups[1].Value}. اكتب قيمة x."),
            "geometry.congruence.identify_criterion" =>
                ConvertPrompt(text, @"^Two triangles each have corresponding angles (.+?) and (.+?) with the included side (.+?)\. Which congruence criterion applies\? Answer SSS, SAS, ASA, or RHS\.$",
                    m => $"مثلثان لهما زاويتان متناظرتان قياسهما {m.Groups[1].Value} و{m.Groups[2].Value} والضلع المحصور بينهما طوله {m.Groups[3].Value}. ما معيار تطابقهما؟ اكتب SSS أو SAS أو ASA أو RHS.")
                ?? ConvertPrompt(text, @"^Two right triangles each have hypotenuse (.+?) and one corresponding leg (.+?)\. Which congruence criterion applies\? Answer SSS, SAS, ASA, or RHS\.$",
                    m => $"مثلثان قائما الزاوية لهما وتر طوله {m.Groups[1].Value} وضلع قائم متناظر طوله {m.Groups[2].Value}. ما معيار تطابقهما؟ اكتب SSS أو SAS أو ASA أو RHS."),
            "usccss.g7.u02.l10.graphs_origin_slope" =>
                ConvertPrompt(text, @"^A shop sells shirts for \$(\d+) each with no fixed fee\. Find the total price in dollars of (\d+) shirts\..*$",
                    m => $"يبيع متجر قمصانًا بسعر {m.Groups[1].Value} دولارات للقميص، من دون رسم ثابت. ما ثمن {m.Groups[2].Value} قميص بالدولار؟ اكتب عددًا دقيقًا.")
                ?? ConvertPrompt(text, @"^Inspect the complete graph defined by (.+?)\. Is it a straight line through the origin with a fixed y/x\? Enter 1 for YES proportional or 0 for NO\..*$",
                    m => $"افحص التمثيل الكامل للعلاقة {m.Groups[1].Value}. هل هي مستقيم يمر بالأصل وله نسبة ثابتة y/x؟ اكتب 1 إذا كانت تناسبية، و0 إن لم تكن.")
                ?? ConvertPrompt(text, @"^A proposed graphical relationship is a straight line through (.+?) and (.+?)\. Enter 1 if the entire specified graph is proportional, otherwise 0\..*$",
                    m => $"تمثل العلاقة البيانية مستقيمًا يمر بالنقطتين {m.Groups[1].Value} و{m.Groups[2].Value}. اكتب 1 إن كانت علاقة تناسبية على الرسم الكامل، وإلا اكتب 0."),
            "algebra.linear.systems.solution_count.classify" =>
                ConvertPrompt(text, @"^How many solutions does the system (.+?) have\? Answer one, none, or infinitely-many\..*$",
                    m => $"كم عدد حلول نظام المعادلتين {m.Groups[1].Value}؟ اكتب one لحل واحد، أو none لعدم وجود حل، أو infinitely-many للحلول غير المنتهية."),
            "algebra.linear.systems.two_by_two.integer_coefficients" =>
                ConvertPrompt(text, @"^Solve the system (.+?)\. Enter the exact ordered pair x,y\..*$",
                    m => $"حل نظام المعادلتين {m.Groups[1].Value}. اكتب الزوج المرتب للحل بالصيغة x,y."),
            "algebra.modeling.linear_relationship.select_equation" =>
                ConvertPrompt(text, @"^A service has a fixed fee of (\d+) dollars and then costs (\d+) dollars per unit\. Let x be the number of units and y the total cost\. Write the equation relating x and y.*$",
                    m => $"لخدمة رسم ثابت قدره {m.Groups[1].Value} دولارًا وتكلفة {m.Groups[2].Value} دولارًا لكل وحدة. إذا كان x عدد الوحدات وy التكلفة الكلية، فاكتب معادلة العلاقة بينهما."),
            "functions.domain_range.context_bounds" =>
                ConvertPrompt(text, @"^A tank starts with (\d+) litres and drains at (\d+) litres per hour\. V\(t\)=(.+?) is modeled only from t=0 through t=(\d+) hours\. Enter the endpoint form exactly as (.+?)\. Explain.*$",
                    m => $"يبدأ خزان بسعة {m.Groups[1].Value} لترًا، وينقص حجمه {m.Groups[2].Value} لترًا في الساعة وفق V(t)={m.Groups[3].Value} خلال الفترة 0 إلى {m.Groups[4].Value} ساعات. اكتب المجال والمدى بالصيغة {m.Groups[5].Value}، مع حساب الحدود الصحيحة."),
            "functions.notation.evaluate_context" =>
                ConvertPrompt(text, @"^A named function (.+?) gives a quantity in metres after t hours and obeys (.+?)\. Evaluate (.+?)\. Enter the integer output only\..*$",
                    m => $"دالة {m.Groups[1].Value} تعطي مقدارًا بالأمتار بعد t ساعة وتحقق {m.Groups[2].Value}. أوجد {m.Groups[3].Value} واكتب الناتج الصحيح فقط."),
            "functions.quadratic.output_roots.solve" =>
                ConvertPrompt(text, @"^Given (.+?), solve (.+?) over the reals\. Enter both distinct integer roots separated by commas.*$",
                    m => $"إذا كانت {m.Groups[1].Value}، فحل المعادلة {m.Groups[2].Value} في مجموعة الأعداد الحقيقية. اكتب الجذرين الصحيحين المختلفين مفصولين بفاصلة."),
            "functions.relations.unique_output.classify" =>
                ConvertPrompt(text, @"^Is the relation (.+?) a function of its first coordinate\? Enter yes or no\..*$",
                    m => $"هل العلاقة المحددة بالأزواج المرتبة {m.Groups[1].Value} تمثل دالة من الإحداثي الأول إلى الثاني؟ اكتب yes لنعم أو no للا."),
            "functions.table.lookup.invert" =>
                ConvertPrompt(text, @"^Table: x=\[(.+?)\]; f\(x\)=\[(.+?)\]\. Evaluate f\((.+?)\) using the table\. Enter the integer output\.$",
                    m => $"من جدول القيم x=[{m.Groups[1].Value}] و f(x)=[{m.Groups[2].Value}]، أوجد f({m.Groups[3].Value}) واكتب الناتج الصحيح."),
            "ratio.unit_rate.direct" =>
                ConvertPrompt(text, @"^(.+?) items are produced in (.+?) minutes\. How many items are produced per 1 minute\?$",
                    m => $"يتم إنتاج {m.Groups[1].Value} وحدة في {m.Groups[2].Value} دقيقة. كم وحدة تُنتج في دقيقة واحدة؟"),
            "ratio.unit_rate.equivalent_ratio" =>
                ConvertPrompt(text, @"^(.+?) items in (.+?) minutes has the same unit rate as \? items in (.+?) minutes\. Find the missing number\.$",
                    m => $"إذا أُنتجت {m.Groups[1].Value} وحدة في {m.Groups[2].Value} دقيقة، فكم وحدة تُنتج في {m.Groups[3].Value} دقيقة بالمعدل الثابت نفسه؟"),
            "supporting.algebra.expressions.mixed" =>
                ConvertPrompt(text, @"^Evaluate (.+?) when x=(.+?)\.$",
                    m => $"احسب قيمة التعبير {m.Groups[1].Value} عندما x={m.Groups[2].Value}."),
            "statistics.center_spread.mad" =>
                ConvertPrompt(text, @"^The synthetic data are (.+?)\. Calculate the mean absolute deviation \(MAD\) from the mean\. Enter an exact decimal or fraction\.$",
                    m => $"البيانات هي {m.Groups[1].Value}. أوجد متوسط الانحرافات المطلقة عن المتوسط الحسابي (MAD). اكتب كسرًا أو عددًا عشريًا دقيقًا."),
            "statistics.correlation.direction" =>
                ConvertPrompt(text, @"^Points (.+?) lie on a perfect line\. Does r indicate a positive or negative linear relationship\?$",
                    m => $"تقع النقاط {m.Groups[1].Value} على مستقيم. هل يشير معامل الارتباط r إلى علاقة خطية موجبة أم سالبة؟"),
            "statistics.distribution_shape.classify" =>
                ConvertPrompt(text, @"^A synthetic numerical distribution has values (.+?) with frequencies (.+?) in the same order\. Classify its overall shape as (.+)$",
                    m => $"توزيع بيانات قيمه {m.Groups[1].Value} وتكراراتها المناظرة بالترتيب {m.Groups[2].Value}. صنّف شكل التوزيع وفق الخيارات التالية: {m.Groups[3].Value}"),
            "statistics.two_way.marginal_count" =>
                ConvertPrompt(text, @"^In a fictional club survey, Group A has Sports(\d+) and Arts(\d+); Group B has Sports(\d+) and Arts(\d+)\. How many respondents selected Sports in total\?$",
                    m => $"في استطلاع لنادٍ، المجموعة أ: رياضة {m.Groups[1].Value} وفنون {m.Groups[2].Value}، والمجموعة ب: رياضة {m.Groups[3].Value} وفنون {m.Groups[4].Value}. كم شخصًا اختار الرياضة إجمالًا؟"),
            "statistics.two_way.conditional_relative" =>
                ConvertPrompt(text, @"^Fictional two-way data: Group A Sports(\d+)/Arts(\d+), Group B Sports(\d+)/Arts(\d+)\. What percent of Group A chose Sports\? \(row-relative\) Enter a percent rounded to two decimal places.*$",
                    m => $"بيانات جدول ذي مدخلين: المجموعة أ رياضة {m.Groups[1].Value} وفنون {m.Groups[2].Value}، والمجموعة ب رياضة {m.Groups[3].Value} وفنون {m.Groups[4].Value}. ما النسبة المئوية من المجموعة أ التي اختارت الرياضة؟ قرب لمنزلتين عشريتين."),
            "supporting.statistics.scatter_correlation" =>
                ConvertPrompt(text, @"^A scatter plot of (.+?) and (.+?) contains the points (.+?)\. Classify the association as positive, negative, or none\.$",
                    m => $"مخطط انتشار بين {m.Groups[1].Value} و{m.Groups[2].Value} يحتوي النقاط {m.Groups[3].Value}. صنّف الارتباط: positive موجب، negative سالب، none لا ارتباط."),
            "usccss.g7.u05.rational_linear_equation" =>
                ConvertPrompt(text, @"^Solve (.+?)\. Give x in simplest form\.$",
                    m => $"حل المعادلة {m.Groups[1].Value}. اكتب قيمة x في أبسط صورة."),
            "usccss.g7.u07.prism_volume_base_height" =>
                ConvertPrompt(text, @"^A right prism has base area (.+?) square units and height (.+?) units\. Find its volume\.$",
                    m => $"منشور قائم مساحة قاعدته {m.Groups[1].Value} وحدة مربعة وارتفاعه {m.Groups[2].Value} وحدة. احسب حجمه."),
            "usccss.number.signed_rational_divide" =>
                ConvertPrompt(text, @"^Calculate (.+?)\. Give the rational number in simplest form\.$",
                    m => $"احسب {m.Groups[1].Value}، واكتب الناتج النسبي في أبسط صورة."),
            "usccss.g7.u01.l06.scaled_area_square_factor" =>
                ConvertPrompt(text, @"^An original shape is a triangle of area (.+?) square units\. Draw a uniformly scaled copy at length scale factor k=(.+?)\. By what AREA factor.*$",
                    m => $"مساحة مثلث أصلي {m.Groups[1].Value} وحدة مربعة، ورُسمت صورة مشابهة بمعامل أطوال k={m.Groups[2].Value}. ما معامل تغير المساحة؟"),
            "usccss.g7.u02.l12.compare_same_axes_rates" =>
                ConvertPrompt(text, @"^Two proportional lines share the same axes and scales\. Their constants are A=(.+?), B=(.+?)\. Enter the larger constant of proportionality\..*$",
                    m => $"مستقيمان تناسبيان على المحاور والمقاييس نفسها. ثابت التناسب للأول A={m.Groups[1].Value} وللثاني B={m.Groups[2].Value}. اكتب ثابت التناسب الأكبر."),
            "usccss.g7.u01.l02.scale_factor" =>
                ConvertPrompt(text, @"^An original right triangle has side lengths (.+?) units\. A proposed copy has corresponding side lengths (.+?) units\. Determine the scale factor from the original triangle to the proposed copy\. Check ALL three matching sides, not just one\. Enter the number of the mathematically correct choice: (.+?)\.$",
                    m => $"مثلث قائم أصلي أطوال أضلاعه {m.Groups[1].Value} وحدات، ومثلث مرشح مشابه أطوال أضلاعه المناظرة {m.Groups[2].Value} وحدات. أوجد معامل التمدد من الأصل إلى الصورة وتحقق من الأضلاع الثلاثة. اختر رقم الإجابة الصحيحة: {m.Groups[3].Value.Replace("no uniform scale","لا يوجد معامل تماثل ثابت",StringComparison.Ordinal)}."),
            "usccss.g7.u01.l04.angle_and_distance_scaling" =>
                ConvertPrompt(text, @"^The original rectangle has vertices (.+?)\. The candidate polygon has boundary-order vertices (.+?)\. The claimed scale factor is (.+?)\. The proposed copy has oblique angles even though both shapes use straight edges\. Are these polygons scaled copies at that factor\? Check corresponding distances AND angles, including the diagonal\. Enter 1 for yes, or 0 for no\.$",
                    m => $"رؤوس المستطيل الأصلي {m.Groups[1].Value}. رؤوس الشكل المرشح بالترتيب {m.Groups[2].Value}. معامل التشابه المزعوم {m.Groups[3].Value}. للصورة المقترحة زوايا مائلة رغم أن أضلاع الشكلين مستقيمة. هل الشكلان متشابهان بهذا المعامل؟ تحقق من المسافات والزوايا المناظرة والقطر، ثم اكتب 1 لنعم أو 0 للا."),
            "usccss.g7.u01.l04.hidden_corresponding_distance" =>
                ConvertPrompt(text, @"^Two scaled W-shaped figures have corresponding visible segments (.+?) units in the original and (.+?) units in its copy\. The distance between TWO other points need not be a drawn side; it still uses the same scale factor\. An unseen distance in the original is (.+?) units\. What is its distance in the copy\? Enter the exact integer answer\.$",
                    m => $"شكلان متشابهان على هيئة W، طول قطعة مرئية في الأصلي {m.Groups[1].Value} وحدات وطول نظيرتها في الصورة {m.Groups[2].Value} وحدة. المسافة بين نقطتين أخريين تتغير بالمعامل نفسه حتى لو لم تكن ضلعًا مرسومًا. إذا كانت المسافة في الأصلي {m.Groups[3].Value}، فما المسافة المناظرة في الصورة؟ اكتب الناتج الدقيق."),
            "usccss.g7.u01.l10.change_scale_length_and_area" =>
                ConvertPrompt(text, @"^A triangular plot has real base (.+?) m and height (.+?) m\. A drawing uses 1 cm to (.+?) m\. What is the base in drawing centimetres\? Enter an exact integer or fraction in the specified units\..*$",
                    m => $"قطعة أرض مثلثة طول قاعدتها الحقيقي {m.Groups[1].Value} مترًا وارتفاعها {m.Groups[2].Value} مترًا. مقياس الرسم 1 سم لكل {m.Groups[3].Value} مترًا. ما طول القاعدة في الرسم بالسنتيمتر؟ اكتب عددًا صحيحًا أو كسرًا دقيقًا."),
            "statistics.linear_model.fit_select" =>
                ConvertPrompt(text, @"^For points (.+?), which line has smaller sum of squared residuals\? A\) (.+?); B\) (.+?)\. Enter A or B\..*$",
                    m => $"للنقاط {m.Groups[1].Value}، أي المستقيمين يحقق مجموع مربعات بواقي أصغر؟ الاختيار A: {m.Groups[2].Value}؛ الاختيار B: {m.Groups[3].Value}. اكتب A أو B."),
            "statistics.linear_model.predict" =>
                ConvertPrompt(text, @"^A hand-fit model predicts (.+?)\. What does it predict at x=\$?(.+?)\? Enter the number rounded to one decimal place\..*$",
                    m => $"نموذج خطي مقدَّر معادلته {m.Groups[1].Value}. ما القيمة المتوقعة عندما x={m.Groups[2].Value}؟ قرب الناتج إلى منزلة عشرية واحدة."),
            "statistics.linear_model.residual" =>
                ConvertPrompt(text, @"^At x=\$?(.+?), observed y=\$?(.+?) while predicted (.+?)\. What is observed minus predicted\? Enter the signed residual to one decimal place\.$",
                    m => $"عند x={m.Groups[1].Value} كانت القيمة المرصودة y={m.Groups[2].Value}، وكانت القيمة المتوقعة من النموذج {m.Groups[3].Value}. أوجد الباقي (المرصود ناقص المتوقع) مع الإشارة، مقربًا إلى منزلة عشرية واحدة."),
            "usccss.g5.u07.l04.source_quadrilateral_sort_attributes" =>
                ConvertPrompt(text, @"^Source category square has how many right angles\? Select 1: 2; 2: 0; 3: 4\..*$",
                    _ => "كم عدد الزوايا القائمة في المربع؟ اختر 1 إذا كانت زاويتين، أو 2 إذا لم توجد زوايا قائمة، أو 3 إذا كانت أربع زوايا قائمة.")
                ?? ConvertPrompt(text, @"^Source figure with NO parallel sides, number of pairs parallel Select 1: 1; 2: 0; 3: 2\..*$",
                    _ => "رباعي ليس فيه أضلاع متوازية. كم زوجًا من الأضلاع المتوازية يوجد فيه؟ اختر 1 لزوج واحد، أو 2 لصفر، أو 3 لزوجين."),
            "usccss.g5.u07.l07.source_quadrilateral_always_sometimes_never" =>
                ConvertPrompt(text, @"^Source always/sometimes/never: a parallelogram is a rhombus Select 1: always; 2: sometimes; 3: never\..*$",
                    _ => "هل متوازي الأضلاع معين دائمًا أم أحيانًا أم أبدًا؟ اختر 1 لدائمًا، أو 2 لأحيانًا، أو 3 لأبدًا.")
                ?? ConvertPrompt(text, @"^Source always/sometimes/never: a square is a rhombus Select 1: sometimes; 2: never; 3: always\..*$",
                    _ => "هل المربع معين دائمًا أم أحيانًا أم أبدًا؟ اختر 1 لأحيانًا، أو 2 لأبدًا، أو 3 لدائمًا."),
            _ => null
        };
        return translated ?? LatestGrade8Prompt(q);
    }

    private static string? AdditionalGrade8Solution(string family) => family switch
    {
        "algebra.linear.variables_both_sides" =>
            "اجمع حدود المتغير في طرف واحد والثوابت في الطرف الآخر، ثم اقسم على معامل المتغير.",
        "geometry.congruence.identify_criterion" =>
            "قارن المعطيات المتناظرة وحدد معيار التطابق الصحيح: الأضلاع الثلاثة SSS، ضلعان والزاوية المحصورة SAS، زاويتان والضلع المحصور ASA، أو وتر وضلع قائم RHS.",
        "usccss.g7.u02.l10.graphs_origin_slope" =>
            "العلاقة التناسبية تحقق y=mx وتمر بنقطة الأصل، وتكون نسبة y/x ثابتة عندما x لا يساوي صفرًا.",
        "algebra.linear.systems.solution_count.classify" =>
            "مستقيمان متقاطعان لهما حل واحد، ومتوازيان مختلفان بلا حلول، ومتطابقان لهما حلول غير منتهية.",
        "algebra.linear.systems.two_by_two.integer_coefficients" =>
            "حل نظام المعادلتين بالحذف أو التعويض، ثم تحقق من الزوج المرتب في المعادلتين.",
        "algebra.modeling.linear_relationship.select_equation" =>
            "التكلفة الكلية تساوي الرسم الثابت زائد تكلفة الوحدة مضروبة في عدد الوحدات.",
        "functions.domain_range.context_bounds" =>
            "استخدم مجال الزمن المعطى وحدّد أصغر وأكبر حجم متاح داخل المجال.",
        "functions.notation.evaluate_context" =>
            "عوّض بقيمة المدخل في قاعدة الدالة ثم احسب الناتج.",
        "functions.quadratic.output_roots.solve" =>
            "استخدم خاصية حاصل الضرب الصفري لإيجاد جذري المعادلة.",
        "functions.relations.unique_output.classify" =>
            "العلاقة دالة إذا لم يرتبط المدخل الواحد بأكثر من مخرج واحد.",
        "functions.table.lookup.invert" =>
            "ابحث عن المدخل في جدول x وخذ المخرج المناظر له في جدول f(x).",
        "ratio.unit_rate.direct" or "ratio.unit_rate.equivalent_ratio" =>
            "اقسم مقدار الإنتاج على الزمن أولًا لتحديد معدل الوحدة، ثم اضرب في الزمن المطلوب.",
        "supporting.algebra.expressions.mixed" =>
            "عوّض في التعبير بالقيمة المعطاة، ثم طبّق ترتيب العمليات.",
        "statistics.center_spread.mad" =>
            "احسب المتوسط الحسابي ثم متوسط القيم المطلقة لانحراف كل قيمة عنه.",
        "statistics.correlation.direction" or "supporting.statistics.scatter_correlation" =>
            "إذا زادت القيم معًا فالارتباط موجب؛ وإذا تناقص أحد المتغيرين مع زيادة الآخر فالارتباط سالب.",
        "statistics.two_way.marginal_count" =>
            "اجمع تكرارات العمود المطلوب من مجموعتي الجدول.",
        "statistics.two_way.conditional_relative" =>
            "اقسم تكرار الفئة المطلوبة على مجموع صف المجموعة ثم اضرب في مئة.",
        "usccss.g7.u05.rational_linear_equation" =>
            "استخدم العمليات العكسية لحل المعادلة ثم بسّط الكسر الناتج.",
        "usccss.g7.u07.prism_volume_base_height" =>
            "الحجم يساوي مساحة القاعدة مضروبة في الارتفاع.",
        "usccss.number.signed_rational_divide" =>
            "القسمة على كسر تساوي الضرب في مقلوبه، ثم اختصر الناتج.",
        "usccss.g7.u01.l06.scaled_area_square_factor" =>
            "تتغير المساحة بمربع معامل التشابه الطولي k.",
        "usccss.g7.u02.l12.compare_same_axes_rates" =>
            "قارن ثابتَي التناسب عدديًا في المقياس نفسه.",
        "usccss.g7.u01.l02.scale_factor" or
        "usccss.g7.u01.l04.angle_and_distance_scaling" or
        "usccss.g7.u01.l04.hidden_corresponding_distance" =>
            "قارن أطوال الأضلاع والزوايا المناظرة باستخدام معامل التشابه نفسه، ثم تحقق من جميع الأبعاد.",
        "usccss.g7.u01.l10.change_scale_length_and_area" =>
            "اقسم الطول الحقيقي على طول الواقع الذي يقابله سنتيمتر واحد في مقياس الرسم.",
        "statistics.linear_model.fit_select" =>
            "احسب لكل نقطة الفرق بين القيمة المرصودة والمتوقعة، ثم مربعه واجمع المربعات للمقارنة.",
        "statistics.linear_model.predict" =>
            "عوّض بقيمة x في معادلة النموذج وأعد التقريب وفق المطلوب.",
        "statistics.linear_model.residual" =>
            "الباقي يساوي القيمة المرصودة ناقص القيمة التي يتوقعها النموذج.",
        "usccss.g5.u07.l04.source_quadrilateral_sort_attributes" or
        "usccss.g5.u07.l07.source_quadrilateral_always_sometimes_never" =>
            "قارن شروط الشكل الرباعي وصفاته اللازمة، ثم اختر رقم البديل الصحيح.",
        _ => null
    };
}
