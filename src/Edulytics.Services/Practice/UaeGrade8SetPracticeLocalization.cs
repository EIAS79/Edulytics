using Edulytics.Services.Mathematics;
using System.Text.RegularExpressions;

namespace Edulytics.Services.Practice;

/// <summary>
/// Lesson-scoped Arabic presentation for verified two-set inclusion/exclusion.
/// This changes neither the mathematical family nor the correct answer, grader,
/// question fingerprint or generation parameters for other curricula.
/// Three-set Venn reasoning remains teacher-reviewed.
/// </summary>
internal static class UaeGrade8SetPracticeLocalization
{
    private const string TargetLesson =
        "PED:UAE-MOE-MATH:L8:GENERAL:12:04:USING-SETS-TO-SOLVE-PROBLEMS";
    private const string TargetFamily = "supporting.sets.union_count";

    public static string Prompt(string lessonCode, ExactSkillGeneratedQuestion question)
    {
        if (!string.Equals(lessonCode, TargetLesson, StringComparison.Ordinal))
            return IsGrade8General(lessonCode)
                ? Grade8ArithmeticPrompt(question) ?? question.Prompt
                : question.Prompt;

        var (a, b, intersection) = CheckedParameters(question);
        return $"في مسألة عن مجموعتين، تحتوي المجموعة أ على {a} عنصرًا، " +
               $"وتحتوي المجموعة ب على {b} عنصرًا، ويوجد {intersection} عنصرًا " +
               "ينتمي إلى المجموعتين معًا. كم عدد عناصر اتحاد المجموعتين أ∪ب؟";
    }

    public static string Solution(string lessonCode, ExactSkillGeneratedQuestion question)
    {
        if (!string.Equals(lessonCode, TargetLesson, StringComparison.Ordinal))
            return IsGrade8General(lessonCode) &&
                   Grade8ArithmeticPrompt(question) is not null
                ? Grade8ArithmeticSolution(question.Family) ?? question.Solution
                : question.Solution;

        var (a, b, intersection) = CheckedParameters(question);
        return $"نطبق مبدأ الشمول والاستبعاد: عدد عناصر الاتحاد = " +
               $"{a} + {b} − {intersection} = {a + b - intersection}. " +
               "نطرح عناصر التقاطع حتى لا نعدّها مرتين.";
    }


    private static bool IsGrade8General(string lessonCode) =>
        lessonCode.StartsWith("PED:UAE-MOE-MATH:L8:GENERAL:", StringComparison.Ordinal);

    // Translate only explicitly verified arithmetic prompt templates. Other
    // variants remain unmodified and are blocked by the runtime Arabic audit.
    // This does not change mathematical parameters, expected answers or grading.
    private static string? ConvertPrompt(
        string prompt, string pattern, Func<Match, string> translate)
    {
        var match = Regex.Match(prompt, pattern, RegexOptions.CultureInvariant);
        return match.Success ? translate(match) : null;
    }

    private static string? Grade8ArithmeticPrompt(ExactSkillGeneratedQuestion q)
    {
        var text = q.Prompt;
        return q.Family switch
        {
            "supporting.algebra.simplify" =>
                ConvertPrompt(text, @"^Simplify (.+)\.$",
                    m => $"بسّط التعبير الجبري {m.Groups[1].Value} بجمع الحدود المتشابهة."),
            "supporting.algebra.expand" =>
                ConvertPrompt(text, @"^Expand (.+)\.$",
                    m => $"استخدم خاصية التوزيع لفك الأقواس في {m.Groups[1].Value}."),
            "supporting.algebra.equations.mixed" or
            "algebra.linear.ax_plus_b_equals_c" =>
                ConvertPrompt(text, @"^Solve (.+)\.$",
                    m => $"حل المعادلة {m.Groups[1].Value}، واكتب قيمة المتغير x."),
            "supporting.powers10.evaluate" =>
                ConvertPrompt(text, @"^Evaluate (.+)\.$",
                    m => $"احسب القيمة الدقيقة للتعبير {m.Groups[1].Value}."),
            "supporting.powers10.multiply" or
            "supporting.powers10.divide" =>
                ConvertPrompt(text, @"^Calculate (.+)\.$",
                    m => $"احسب قيمة التعبير {m.Groups[1].Value}."),
            "supporting.powers10.missing_exponent" or
            "supporting.standard_form.power10_exponent" =>
                ConvertPrompt(text, @"^(.+)\. Find n\.$",
                    m => $"أوجد قيمة الأس n التي تحقق المعادلة {m.Groups[1].Value}."),
            "geometry.coordinate.evaluate_linear_rule" =>
                ConvertPrompt(text, @"^For the straight line (.+?), find y when x = (.+)\.$",
                    m => $"للمستقيم {m.Groups[1].Value}، أوجد قيمة y عندما x = {m.Groups[2].Value}."),
            "geometry.coordinate.y_intercept_from_rule" =>
                ConvertPrompt(text, @"^For the straight line (.+?), find the y-intercept\.$",
                    m => $"أوجد المقطع الصادي للمستقيم {m.Groups[1].Value}."),
            "geometry.similarity.scale_factor" =>
                ConvertPrompt(text, @"^A side of length (.+?) corresponds to a side of length (.+?) in a similar shape\. Find the scale factor from the first shape to the second\.$",
                    m => $"طول ضلع في الشكل الأول {m.Groups[1].Value}، وطول الضلع المناظر في شكل مشابه {m.Groups[2].Value}. أوجد معامل التشابه من الأول إلى الثاني."),
            "algebra.linear.standard_form.slope_intercept" =>
                ConvertPrompt(text, @"^Rewrite (.+?) as y=mx\+b\. Enter the slope and y-intercept as m,b\..*$",
                    m => $"أعد كتابة المعادلة {m.Groups[1].Value} على صورة y=mx+b. اكتب الميل ثم المقطع الصادي بالصيغة m,b."),
            "geometry.angles.supplementary" =>
                ConvertPrompt(text, @"^Two angles form a straight line\. One angle is (.+?)\. Find the other angle\.$",
                    m => $"زاويتان متجاورتان تشكلان زاوية مستقيمة. إذا كان قياس الأولى {m.Groups[1].Value}، فأوجد قياس الثانية."),
            "geometry.polygons.missing_interior_angle" =>
                ConvertPrompt(text, @"^A (.+?)-sided polygon has known interior angles totalling (.+?)\. Find the missing interior angle\.$",
                    m => $"مضلع عدد أضلاعه {m.Groups[1].Value}، مجموع قياسات زواياه الداخلية المعروفة {m.Groups[2].Value}. أوجد قياس الزاوية الداخلية المتبقية."),
            "fractions.compare.unlike.common_denominator" =>
                ConvertPrompt(text, @"^Compare (.+?) and (.+?)\. Enter <, >, or =\.$",
                    m => $"قارن بين {m.Groups[1].Value} و{m.Groups[2].Value}. اكتب الرمز المناسب (< أو > أو =)."),
            "statistics.center_spread.mean" =>
                ConvertPrompt(text, @"^Find the mean of (.+)\.$",
                    m => $"احسب المتوسط الحسابي للقيم التالية: {m.Groups[1].Value}."),
            "statistics.center_spread.median" =>
                ConvertPrompt(text, @"^Find the median of (.+)\.$",
                    m => $"أوجد الوسيط للقيم التالية: {m.Groups[1].Value}."),
            "statistics.center_spread.range" =>
                ConvertPrompt(text, @"^A data set has minimum (.+?) and maximum (.+?)\. Find its range\.$",
                    m => $"أصغر قيمة في مجموعة بيانات هي {m.Groups[1].Value} وأكبر قيمة هي {m.Groups[2].Value}. أوجد المدى."),
            "supporting.geometry.circle_area_pi_coefficient" =>
                ConvertPrompt(text, @"^A circle has radius (.+?)\. Its area is kπ\. Find k\.$",
                    m => $"دائرة نصف قطرها {m.Groups[1].Value}. إذا كانت مساحتها kπ، فأوجد قيمة k."),
            "supporting.geometry.circle_circumference_pi_coefficient" =>
                ConvertPrompt(text, @"^A circle has radius (.+?)\. Its circumference is kπ\. Find k\.$",
                    m => $"دائرة نصف قطرها {m.Groups[1].Value}. إذا كان محيطها kπ، فأوجد قيمة k."),
            "supporting.statistics.table_total" =>
                ConvertPrompt(text, @"^A frequency table has counts (.+?)\. Find the total frequency\.$",
                    m => $"يعرض جدول تكراري القيم {m.Groups[1].Value}. أوجد مجموع التكرارات."),
            "supporting.functions.polynomial_value" =>
                ConvertPrompt(text, @"^For (.+?), find (.+?)\.$",
                    m => $"لدالة كثيرة الحدود {m.Groups[1].Value}، أوجد {m.Groups[2].Value}."),
            "geometry.coordinate.gradient_between_points" =>
                ConvertPrompt(text, @"^Find the gradient of the line through (.+) and (.+)\.$",
                    m => $"أوجد ميل المستقيم المار بالنقطتين {m.Groups[1].Value} و{m.Groups[2].Value}."),
            "supporting.statistics.quartile" =>
                ConvertPrompt(text, @"^An ordered data summary states the lower quartile is (.+?)\. What is Q1\?$",
                    m => $"في ملخص بيانات مرتبة، الربيع الأدنى يساوي {m.Groups[1].Value}. ما قيمة Q1؟"),
            _ => null
        };
    }

    private static string? Grade8ArithmeticSolution(string family) => family switch
    {
        "supporting.algebra.simplify" => "اجمع معاملات الحدود المتشابهة واحتفظ بالثوابت منفصلة.",
        "supporting.algebra.expand" => "وزّع العامل خارج القوس على جميع الحدود داخل القوس، ثم اجمع الحدود المتشابهة.",
        "supporting.algebra.equations.mixed" or "algebra.linear.ax_plus_b_equals_c" =>
            "استخدم العمليات العكسية للمحافظة على تساوي طرفَي المعادلة حتى تعزل المتغير.",
        "supporting.powers10.evaluate" or "supporting.powers10.multiply" or
        "supporting.powers10.divide" or "supporting.powers10.missing_exponent" or
        "supporting.standard_form.power10_exponent" =>
            "طبّق قواعد قوى العدد عشرة ثم احسب القيمة أو الأس المطلوب.",
        "geometry.coordinate.evaluate_linear_rule" =>
            "عوّض بقيمة x المعطاة في معادلة المستقيم، ثم احسب قيمة y.",
        "geometry.coordinate.y_intercept_from_rule" =>
            "لإيجاد المقطع الصادي عوّض x=0 في معادلة المستقيم.",
        "geometry.similarity.scale_factor" =>
            "معامل التشابه يساوي طول الضلع المناظر في الشكل الثاني مقسومًا على طوله في الشكل الأول.",
        "algebra.linear.standard_form.slope_intercept" =>
            "رتّب المعادلة بحيث تنفرد y في طرف واحد؛ معامل x هو الميل والحد الثابت هو المقطع الصادي.",
        "geometry.angles.supplementary" =>
            "مجموع قياسي الزاويتين المتجاورتين على مستقيم يساوي 180 درجة.",
        "geometry.polygons.missing_interior_angle" =>
            "مجموع زوايا المضلع الداخلية يساوي (عدد الأضلاع−2)×180 درجة؛ اطرح مجموع الزوايا المعروفة.",
        "fractions.compare.unlike.common_denominator" =>
            "وحّد مقامي الكسرين أو حوّلهما إلى قيمتين متكافئتين ثم قارن.",
        "statistics.center_spread.mean" => "اقسم مجموع القيم على عددها.",
        "statistics.center_spread.median" => "رتّب القيم، ثم حدد القيمة الوسطى أو متوسط القيمتين الوسطيتين.",
        "statistics.center_spread.range" => "اطرح أصغر قيمة من أكبر قيمة.",
        "supporting.geometry.circle_area_pi_coefficient" =>
            "مساحة الدائرة تساوي π مضروبًا في مربع نصف القطر.",
        "supporting.geometry.circle_circumference_pi_coefficient" =>
            "محيط الدائرة يساوي 2π مضروبًا في نصف القطر.",
        "supporting.statistics.table_total" => "اجمع تكرارات جميع فئات الجدول.",
        "supporting.functions.polynomial_value" => "عوّض في قاعدة الدالة ثم بسّط.",
        "geometry.coordinate.gradient_between_points" =>
            "الميل يساوي فرق الصادات مقسومًا على فرق السينات.",
        "supporting.statistics.quartile" => "الربيع الأول Q1 هو نفسه الربيع الأدنى.",
        _ => null
    };

    private static (int A, int B, int Intersection) CheckedParameters(
        ExactSkillGeneratedQuestion question)
    {
        if (!string.Equals(question.Family, TargetFamily, StringComparison.Ordinal) ||
            !question.Parameters.TryGetValue("a", out var a) ||
            !question.Parameters.TryGetValue("b", out var b) ||
            !question.Parameters.TryGetValue("intersection", out var intersection) ||
            a < 1 || b < 1 || intersection < 1 || intersection > Math.Min(a, b))
        {
            throw new InvalidOperationException(
                "UAE G8 U12L04 Arabic Practice requires valid two-set union parameters.");
        }

        return (a, b, intersection);
    }
}
