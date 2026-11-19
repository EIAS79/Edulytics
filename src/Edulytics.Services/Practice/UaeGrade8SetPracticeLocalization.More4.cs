using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

/// <summary>Final inspected Arabic representation of coordinate and scaling variants.</summary>
internal static partial class UaeGrade8SetPracticeLocalization
{
    private static string? LatestGrade8FourthPrompt(ExactSkillGeneratedQuestion q)
    {
        var text=q.Prompt;
        var translated=q.Family switch
        {
            "usccss.g7.u02.l12.compare_same_axes_rates" =>
                ConvertPrompt(text, @"^Time is measured from one shared clock\. Decide whether the described distance-time graph starts later and runs from (.+?) to (.+?)\. Enter 1 if distance is proportional to the shared elapsed time, otherwise 0\..*$",
                    m=>$"يُقاس الزمن من ساعة واحدة مشتركة. يبدأ تمثيل المسافة والزمن متأخرًا ويمتد من {m.Groups[1].Value} إلى {m.Groups[2].Value}. هل المسافة تتناسب طرديًا مع الزمن المنقضي من بدء الساعة المشتركة؟ اكتب 1 لنعم و0 للا."),
            "usccss.g7.u01.l10.change_scale_length_and_area" =>
                ConvertPrompt(text, @"^A restaurant's floor plan uses 1 drawing inch for (.+?) real feet, with drawing area (.+?) square inches\. What REAL area \(square feet\) is represented by 1 drawing square inch\? Enter an exact integer or fraction in the specified units\..*$",
                    m=>$"مخطط أرضية مطعم يستخدم 1 بوصة على الرسم مقابل {m.Groups[1].Value} قدمًا في الواقع، وتبلغ المساحة المرسومة {m.Groups[2].Value} بوصة مربعة. ما المساحة الحقيقية بالأقدام المربعة التي تقابل بوصة مربعة واحدة على الرسم؟ اكتب عددًا أو كسرًا دقيقًا."),
            "usccss.g7.u01.l06.scaled_area_square_factor" =>
                ConvertPrompt(text, @"^An original shape is a first-party figure made from (.+?) original-size pattern blocks \(each block is 1 square unit\)\. Draw a uniformly scaled copy at length scale factor k=(.+?)\. By what AREA factor \(not length factor\) has the original area changed\? Enter an exact whole number or reduced fraction such as 9/25\..*$",
                    m=>$"يتكون شكل أصلي من {m.Groups[1].Value} قطع، مساحة كل واحدة وحدة مربعة. إذا رُسمت صورة مشابهة بمعامل أطوال k={m.Groups[2].Value}، فما معامل تغير المساحة، لا الطول؟ اكتب عددًا صحيحًا أو كسرًا مبسطًا دقيقًا."),
            _ => null
        };
        return translated ?? LatestGrade8FifthPrompt(q);
    }
}
