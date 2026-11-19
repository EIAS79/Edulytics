using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

/// <summary>Arabic question templates for remaining inspected Grade8 practice variants.</summary>
internal static partial class UaeGrade8SetPracticeLocalization
{
    private static string? LatestGrade8ThirdPrompt(ExactSkillGeneratedQuestion q)
    {
        var text=q.Prompt;
        var translated=q.Family switch
        {
            "usccss.g7.u02.l10.graphs_origin_slope" =>
                ConvertPrompt(text, @"^A proposed graphical relationship is a curved graph (.+?) beginning at (.+?)\. Enter 1 if the entire specified graph is proportional, otherwise 0\..*$",
                    m=>$"علاقة بيانية يمثلها منحنى {m.Groups[1].Value} يبدأ عند النقطة {m.Groups[2].Value}. هل العلاقة التناسبية المباشرة قائمة على المنحنى كله؟ اكتب 1 لنعم أو 0 للا.")
                ?? ConvertPrompt(text, @"^A proposed graphical relationship is a line with positive intercept (.+?) and positive slope (.+?)\. Enter 1 if the entire specified graph is proportional, otherwise 0\..*$",
                    m=>$"علاقة بيانية يمثلها مستقيم مقطعه الصادي الموجب {m.Groups[1].Value} وميله الموجب {m.Groups[2].Value}. هل تمثل تناسبًا طرديًا على المجال كله؟ اكتب 1 لنعم أو 0 للا."),
            "usccss.g7.u02.l12.compare_same_axes_rates" =>
                ConvertPrompt(text, @"^Two positive proportional lines use identical unlabeled axis scales\. At the same positive x, line B is above line A\. Enter 1 if A has the larger constant or 2 if B has the larger constant\..*$",
                    _=>"مستقيمان يمثلان تناسبًا طرديًا موجبًا ويستخدمان مقياس المحاور نفسه. عند قيمة موجبة واحدة لـx، يقع المستقيم B فوق A. اكتب 1 إذا كان ثابت التناسب في A أكبر، أو 2 إذا كان في B أكبر."),
            "geometry.congruence.identify_criterion" =>
                ConvertPrompt(text, @"^Two triangles each have corresponding side lengths (.+?), (.+?), and (.+?)\. Which congruence criterion proves they are congruent\? Answer SSS, SAS, ASA, or RHS\.$",
                    m=>$"مثلثان لهما أطوال أضلاع متناظرة {m.Groups[1].Value} و{m.Groups[2].Value} و{m.Groups[3].Value}. ما معيار التطابق الذي يثبته ذلك؟ اكتب SSS أو SAS أو ASA أو RHS.")
                ?? ConvertPrompt(text, @"^Two triangles each have corresponding sides (.+?) and (.+?) with the included angle (.+?)\. Which congruence criterion applies\? Answer SSS, SAS, ASA, or RHS\.$",
                    m=>$"مثلثان لهما ضلعان متناظران طولهما {m.Groups[1].Value} و{m.Groups[2].Value} والزاوية المحصورة بينهما {m.Groups[3].Value}. ما معيار التطابق؟ اكتب SSS أو SAS أو ASA أو RHS."),
            "usccss.g7.u01.l10.change_scale_length_and_area" =>
                ConvertPrompt(text, @"^Two drawings show the SAME actual playground, one at 1 cm to (.+?) m and the other at 1 cm to (.+?) m\. At the second scale, what real area \(square metres\) is 1 drawn square cm\? Enter an exact integer or fraction in the specified units\..*$",
                    m=>$"رسمان للملعب نفسه: الأول كل 1 سم يقابل {m.Groups[1].Value} مترًا، والثاني كل 1 سم يقابل {m.Groups[2].Value} مترًا. في الرسم الثاني، كم مترًا مربعًا تقابل مساحة 1 سم² على الورق؟ اكتب الناتج الدقيق."),
            "usccss.g7.u01.l06.scaled_area_square_factor" =>
                ConvertPrompt(text, @"^An original shape is a first-party figure made from (.+?) original-size pattern blocks \(each block is 1 square unit\)\. Draw a uniformly scaled copy at length scale factor k=(.+?)\. What is the area of the copy in square units\? Enter an exact whole number or reduced fraction such as 9/25\..*$",
                    m=>$"شكل أصلي مؤلف من {m.Groups[1].Value} أجزاء متساوية مساحة كل منها وحدة مربعة. رُسمت صورة مشابهة بمعامل أطوال k={m.Groups[2].Value}. ما مساحة الصورة بوحدة مربعة؟ اكتب عددًا صحيحًا أو كسرًا مبسطًا دقيقًا."),
            "statistics.two_way.conditional_relative" =>
                ConvertPrompt(text, @"^Fictional two-way data: Group A Sports(\d+)/Arts(\d+), Group B Sports(\d+)/Arts(\d+)\. What percent of all respondents were Group A Sports\? \(joint-relative\) Enter a percent rounded to two decimal places without the % sign\.$",
                    m=>$"جدول ثنائي لمجموعتين: أ رياضة {m.Groups[1].Value} وفنون {m.Groups[2].Value}، ب رياضة {m.Groups[3].Value} وفنون {m.Groups[4].Value}. ما النسبة المئوية من جميع المشاركين الذين ينتمون إلى المجموعة أ واختاروا الرياضة؟ اكتب النسبة إلى منزلتين عشريتين من دون رمز %."),
            _ => null
        };
        return translated ?? LatestGrade8FourthPrompt(q);
    }
}
