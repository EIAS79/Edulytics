using Edulytics.Services.Mathematics;
using System.Text.RegularExpressions;

namespace Edulytics.Services.Practice;

/// <summary>
/// Remaining verified variants of shared families used by Grade 8 General.
/// Regex anchors prevent unintended meaning changes for unknown templates.
/// </summary>
internal static partial class UaeGrade8SetPracticeLocalization
{
    private static string? LatestGrade8Prompt(ExactSkillGeneratedQuestion q)
    {
        var text=q.Prompt;
        var translated=q.Family switch
        {
            "usccss.g7.u02.l10.graphs_origin_slope" =>
                ConvertPrompt(text, @"^A straight-line model through the origin contains the point (.+?)\. Find the exact y/x slope k of y=kx\..*$",
                    m=>$"نموذج خطي يمر بنقطة الأصل والنقطة {m.Groups[1].Value}. أوجد قيمة الميل k في المعادلة y=kx، واكتب الناتج الدقيق.")
                ?? ConvertPrompt(text, @"^Blueberries cost \$(.+?) per pound, without a fixed fee\. What is the price in dollars for (.+?) pounds\?.*$",
                    m=>$"سعر رطل التوت الأزرق {m.Groups[1].Value} دولارات دون رسم ثابت. ما ثمن {m.Groups[2].Value} رطلًا بالدولار؟ اكتب الناتج الدقيق."),
            "functions.table.lookup.invert" =>
                ConvertPrompt(text, @"^Table: x=\[(.+?)\]; f\(x\)=\[(.+?)\]\. Find all x-values in the table with f\(x\)=(.+?)\. Enter both inputs separated by commas\.$",
                    m=>$"في الجدول x=[{m.Groups[1].Value}] و f(x)=[{m.Groups[2].Value}]، أوجد كل قيم x التي تحقق f(x)={m.Groups[3].Value}. اكتب المدخلين مفصولين بفاصلة."),
            "usccss.g7.u02.l12.compare_same_axes_rates" =>
                ConvertPrompt(text, @"^Time is measured from one shared clock\. Decide whether the described distance-time graph runs from (.+?) through (.+?)\. Enter 1 if distance is proportional to the shared elapsed time, otherwise 0\..*$",
                    m=>$"يُقاس الزمن من ساعة مرجعية واحدة. يمتد رسم المسافة مقابل الزمن من {m.Groups[1].Value} مرورًا بالنقطة {m.Groups[2].Value}. هل المسافة تتناسب طرديًا مع الزمن المنقضي؟ اكتب 1 لنعم و0 للا."),
            "usccss.g7.u01.l02.scale_factor" =>
                ConvertPrompt(text, @"^An original right triangle has side lengths (.+?) units\. A proposed copy has corresponding side lengths (.+?) units\. Determine the scale factor from the proposed copy back to the original triangle\. Check ALL three matching sides, not just one\. Enter the number of the mathematically correct choice: (.+?)\.$",
                    m=>$"مثلث أصلي قائم الزاوية أطوال أضلاعه {m.Groups[1].Value}، وصورته المقترحة أطوال أضلاعها المناظرة {m.Groups[2].Value}. أوجد معامل القياس من الصورة إلى المثلث الأصلي، وتحقق من الأضلاع الثلاثة. اختر رقم الإجابة الصحيحة: {m.Groups[3].Value.Replace("no uniform scale","لا يوجد معامل تماثل ثابت",StringComparison.Ordinal)}."),
            "usccss.g7.u01.l04.angle_and_distance_scaling" =>
                ConvertPrompt(text, @"^The original rectangle has vertices (.+?)\. The candidate polygon has boundary-order vertices (.+?)\. The claimed scale factor is (.+?)\. All four corresponding angles are right angles\. Are these polygons scaled copies at that factor\? Check corresponding distances AND angles, including the diagonal\. Enter 1 for yes, or 0 for no\.$",
                    m=>$"رؤوس المستطيل الأصلي {m.Groups[1].Value}. ورؤوس الشكل المقترح بالترتيب {m.Groups[2].Value}. معامل التشابه المقترح {m.Groups[3].Value}، والزوايا المتناظرة كلها قائمة. هل الشكلان متشابهان بهذا المعامل؟ تحقق من جميع المسافات والزوايا والأقطار، ثم اكتب 1 لنعم أو 0 للا."),
            "usccss.g7.u01.l04.hidden_corresponding_distance" =>
                ConvertPrompt(text, @"^Two scaled W-shaped figures have corresponding visible segments (.+?) units in the original and (.+?) units in its copy\. The distance between TWO other points need not be a drawn side; it still uses the same scale factor\. An unseen distance in the copy is (.+?) units\. What is its distance in the original\? Enter the exact integer answer\.$",
                    m=>$"شكلان متشابهان على هيئة W؛ قطعة مرئية طولها {m.Groups[1].Value} وحدة في الأصلي ونظيرتها {m.Groups[2].Value} في الصورة. تسري نسبة القياس على جميع المسافات المناظرة حتى غير المرسومة. إذا كانت مسافة في الصورة {m.Groups[3].Value} وحدات، فما طولها في الأصل؟ اكتب الناتج الدقيق."),
            "usccss.g7.u01.l10.change_scale_length_and_area" =>
                ConvertPrompt(text, @"^Two drawings show the SAME actual playground, one at 1 cm to (.+?) m and the other at 1 cm to (.+?) m\. By what factor do lengths IN THE NEW DRAWING increase\? Enter an exact integer or fraction in the specified units\..*$",
                    m=>$"رسمان للملعب الحقيقي نفسه: في الأول كل 1 سم يمثل {m.Groups[1].Value} مترًا، وفي الثاني كل 1 سم يمثل {m.Groups[2].Value} مترًا. ما معامل تغير الأطوال في الرسم الجديد بالنسبة للقديم؟ اكتب عددًا أو كسرًا دقيقًا."),
            "usccss.g7.u01.l06.scaled_area_square_factor" =>
                ConvertPrompt(text, @"^An original shape is a parallelogram of area (.+?) square units\. Draw a uniformly scaled copy at length scale factor k=(.+?)\. What is the area of the copy in square units\? Enter an exact whole number or reduced fraction such as 9/25\..*$",
                    m=>$"مساحة متوازي الأضلاع الأصلي {m.Groups[1].Value} وحدة مربعة. رُسمت صورة مشابهة بمعامل تغير الأطوال k={m.Groups[2].Value}. أوجد مساحة الصورة بوحدة مربعة، واكتب عددًا صحيحًا أو كسرًا مبسطًا دقيقًا."),
            "statistics.two_way.conditional_relative" =>
                ConvertPrompt(text, @"^Fictional two-way data: Group A Sports(\d+)/Arts(\d+), Group B Sports(\d+)/Arts(\d+)\. Of all Sports respondents, what percent were in Group A\? \(column-relative\) Enter a percent rounded to two decimal places without the % sign\.$",
                    m=>$"استطلاع لمجموعتين: المجموعة أ رياضة {m.Groups[1].Value} وفنون {m.Groups[2].Value}، والمجموعة ب رياضة {m.Groups[3].Value} وفنون {m.Groups[4].Value}. من جميع من اختاروا الرياضة، ما النسبة المئوية لأفراد المجموعة أ؟ قرب إلى منزلتين عشريتين دون رمز %."),
            "usccss.g5.u07.l07.source_quadrilateral_always_sometimes_never" =>
                ConvertPrompt(text, @"^Source always/sometimes/never: a square is a rectangle Select 1: always; 2: sometimes; 3: never\..*$",
                    _=>"هل المربع مستطيل دائمًا أم أحيانًا أم أبدًا؟ اختر 1 لدائمًا، أو 2 لأحيانًا، أو 3 لأبدًا."),
            "usccss.g5.u07.l04.source_quadrilateral_sort_attributes" =>
                ConvertPrompt(text, @"^All original source quadrilaterals have exactly how many sides\? Select 1: 4; 2: 3; 3: 5\..*$",
                    _=>"كم ضلعًا لكل شكل رباعي؟ اختر 1 لأربعة أضلاع، أو 2 لثلاثة أضلاع، أو 3 لخمسة أضلاع."),
            _ => null
        };
        return translated ?? LatestGrade8ThirdPrompt(q);
    }
}
