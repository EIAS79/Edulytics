using Edulytics.Services.Mathematics;

namespace Edulytics.Services.Practice;

/// <summary>Checked symmetric variants for rate comparison and area scaling.</summary>
internal static partial class UaeGrade8SetPracticeLocalization
{
    private static string? LatestGrade8FifthPrompt(ExactSkillGeneratedQuestion q)
    {
        var text=q.Prompt;
        var translated=q.Family switch
        {
            "usccss.g7.u02.l12.compare_same_axes_rates" =>
                ConvertPrompt(text, @"^Two positive proportional lines use identical unlabeled axis scales\. At the same positive x, line A is above line B\. Enter 1 if A has the larger constant or 2 if B has the larger constant\..*$",
                    _=>"مستقيمان يمثلان تناسبًا طرديًا موجبًا ويستخدمان مقياس المحاور نفسه. عند قيمة موجبة واحدة لـx، يقع المستقيم A فوق B. اكتب 1 إذا كان ثابت التناسب في A أكبر، أو 2 إذا كان في B أكبر."),
            "usccss.g7.u01.l10.change_scale_length_and_area" =>
                ConvertPrompt(text, @"^A rectangular first-party park has a drawing (.+?) cm by (.+?) cm at 1 cm to (.+?) metres\. The SAME REAL park is redrawn at 1 cm to (.+?) metres\. By what factor did the DRAWING area increase\? Enter an exact integer or fraction in the specified units\..*$",
                    m=>$"رسم لحديقة مستطيلة بأبعاد {m.Groups[1].Value} سم و{m.Groups[2].Value} سم، ومقياسه 1 سم لكل {m.Groups[3].Value} مترًا. أُعيد رسم الحديقة الحقيقية نفسها بمقياس 1 سم لكل {m.Groups[4].Value} مترًا. بمقدار أي معامل زادت مساحة الرسم؟ اكتب عددًا صحيحًا أو كسرًا دقيقًا."),
            "usccss.g7.u01.l06.scaled_area_square_factor" =>
                ConvertPrompt(text, @"^An original shape is a triangle of area (.+?) square units\. Draw a uniformly scaled copy at length scale factor k=(.+?)\. What is the area of the copy in square units\? Enter an exact whole number or reduced fraction such as 9/25\..*$",
                    m=>$"مساحة مثلث أصلي {m.Groups[1].Value} وحدات مربعة، ورُسمت صورته بمعامل أطوال k={m.Groups[2].Value}. ما مساحة الصورة بوحدات مربعة؟ اكتب عددًا صحيحًا أو كسرًا مبسطًا دقيقًا."),
            _ => null
        };
        return translated ?? Grade8ParameterizedSharedPracticePrompt(q);
    }
}
