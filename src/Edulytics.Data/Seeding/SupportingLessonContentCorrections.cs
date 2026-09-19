using Edulytics.Core.Curriculum;
using Edulytics.Core.Mathematics.Practice;

namespace Edulytics.Data.Seeding;

/// <summary>
/// Runtime content remediation for unresolved Supporting Mathematics lessons.
/// Explicit READY_VERIFIED lessons keep their existing authored content. The
/// same target manifest that creates generated Practice contracts selects the
/// remediation profile, so learner content and exact Practice stay aligned.
/// </summary>
internal static class SupportingLessonContentCorrections
{
    public static void Apply(CanonicalLessonContentPackDocument document)
    {
        foreach (var lesson in document.Lessons)
        {
            if (lesson.OutcomeCodes.Count != 0 ||
                SupportingPracticeTargetResolver.IsExplicitReadyLesson(lesson.LessonCode))
            {
                continue;
            }

            var title = PreferredTitle(lesson);
            if (!SupportingPracticeTargetResolver.TryResolve(title, out var spec) ||
                spec is null)
            {
                continue;
            }

            foreach (var translation in lesson.Translations)
            {
                var localizedTitle = string.IsNullOrWhiteSpace(translation.Title)
                    ? spec.NormalizedTitle
                    : translation.Title.Trim();
                var body = BuildBody(localizedTitle, spec.Profile, translation.CultureCode);
                translation.Explanation = body.Explanation;
                translation.KeyConceptsAndRules = body.KeyConcepts;
                translation.WorkedExamples = body.WorkedExamples;
                translation.StepByStepSolutions = body.Solutions;
                translation.CommonMistakes = body.CommonMistakes;
                translation.QuickSummary = body.QuickSummary;
            }
        }
    }

    private static string PreferredTitle(CanonicalLessonContentPackLesson lesson)
    {
        var english = lesson.Translations.FirstOrDefault(x =>
            x.CultureCode.StartsWith("en", StringComparison.OrdinalIgnoreCase));
        return english?.Title
            ?? lesson.Translations.FirstOrDefault()?.Title
            ?? string.Empty;
    }

    private static Body BuildBody(string title, string profile, string culture)
    {
        if (culture.StartsWith("pl", StringComparison.OrdinalIgnoreCase))
            return Polish(title, profile);
        if (culture.StartsWith("ar", StringComparison.OrdinalIgnoreCase))
            return Arabic(title, profile);
        return English(title, profile);
    }

    private static Body English(string title, string profile)
    {
        var concept = profile switch
        {
            "number" => "whole-number structure, place value, exact calculation and inverse checking",
            "fractions" => "fraction meaning, equivalent forms and exact numerator/denominator operations",
            "percentages" => "percent as a rate per hundred and exact multiplicative comparison",
            "ratio" => "multiplicative comparison, equivalent ratios and unit-rate reasoning",
            "sequences" => "term-to-term structure and exact nth-term reasoning",
            "algebra" => "equivalence, substitution, symbolic transformation and exact equation solving",
            "functions" => "input-output rules, graphs and exact function evaluation",
            "geometry" => "geometric properties, measurement, coordinates and invariant relationships",
            "measurement" => "units, scale, conversion and dimensional interpretation",
            "trigonometry" => "right-triangle and angle relationships using exact trigonometric structure",
            "statistics" => "data representation, centre, spread and evidence-based interpretation",
            "probability" => "sample spaces, relative frequency and exact probability relationships",
            "vectors" => "component form, magnitude, direction and exact vector operations",
            "calculus" => "rates of change, accumulation and exact polynomial reasoning",
            "mechanics" => "mathematical modelling of motion, force, momentum, energy and units",
            "reasoning" => "clear assumptions, valid deductions, counterexamples and mathematical justification",
            _ => "exact mathematical structure and verifiable reasoning"
        };

        var example = profile switch
        {
            "number" => "Example: identify the place values or operation first, calculate exactly, then use the inverse operation or estimation to check the result.",
            "fractions" => "Example: represent quantities with a common denominator before comparing or combining them, and simplify only by dividing numerator and denominator by the same non-zero factor.",
            "percentages" => "Example: 25% of 80 = 25/100 × 80 = 20. Check that 20 is one quarter of 80.",
            "ratio" => "Example: divide 60 in the ratio 2:3. There are 5 equal parts, each part is 12, so the shares are 24 and 36.",
            "sequences" => "Example: for 4, 7, 10, ... the common difference is 3, so a_n = 4 + 3(n-1).",
            "algebra" => "Example: preserve equality while simplifying. For 3x + 5 = 20, subtract 5 and divide by 3, then substitute the result back.",
            "functions" => "Example: if f(x)=2x+3, then f(4)=11. A coordinate generated by the rule must satisfy the same equation.",
            "geometry" => "Example: identify the relevant geometric invariant or formula, substitute only the stated measurements, and check the result against the original figure.",
            "measurement" => "Example: convert units before calculating. Since 1 m = 100 cm, 3.5 m corresponds to 350 cm.",
            "trigonometry" => "Example: identify opposite, adjacent and hypotenuse before selecting a trigonometric relationship, then check the result against triangle constraints.",
            "statistics" => "Example: calculate a statistic directly from the original data and interpret it in the context of the distribution rather than from a copied template.",
            "probability" => "Example: for equally likely outcomes, probability = favourable outcomes / total outcomes; for experimental probability, use relative frequency.",
            "vectors" => "Example: operate on corresponding components and verify the resulting direction or magnitude from the original components.",
            "calculus" => "Example: apply the derivative or antiderivative rule term by term, then verify by substitution, differentiation or evaluating the original bounds.",
            "mechanics" => "Example: choose the physical model, keep SI units consistent, solve exactly and perform a dimensional check on the result.",
            "reasoning" => "Example: test the assumptions, state each deduction, and use a counterexample when a universal statement is false.",
            _ => "Example: identify the governing relationship, calculate from the original quantities, and verify the result independently."
        };

        return new Body(
            $"{title} is treated as a focused Mathematics Practice target. The lesson develops {concept}. Explanations connect the representation, rule and reason for each step so the learner can reconstruct the method rather than copy a surface pattern.",
            $"Core rule for {title}: identify the mathematical structure first; use only relationships valid for that structure; preserve exact values until the final step; and verify the answer against the original conditions.",
            $"{example} This worked example is attached specifically to the target “{title}”.",
            $"1. Restate what {title} asks mathematically. 2. Select the governing rule or representation. 3. Substitute or transform the original quantities exactly. 4. Simplify in a controlled order. 5. Check the result using an inverse operation, original equation, geometric constraint, data total, probability bound, or dimensional condition as appropriate.",
            $"Do not choose a rule because the numbers look familiar. For {title}, common errors include changing the mathematical relationship, rounding too early, ignoring units or signs, or checking only the final arithmetic instead of the original condition.",
            $"{title}: identify the exact structure, apply the correct rule, and verify the result independently.");
    }

    private static Body Polish(string title, string profile)
    {
        var baseBody = English(title, profile);
        return new Body(
            $"{title} jest traktowane jako konkretny cel ćwiczeń matematycznych. Uczeń rozpoznaje strukturę zadania, dobiera właściwą regułę, wykonuje obliczenia dokładnie i sprawdza wynik na podstawie warunków początkowych.",
            $"Zasada dla „{title}”: najpierw rozpoznaj typ zależności matematycznej, następnie zastosuj właściwy wzór lub działanie, zachowaj wartości dokładne i wykonaj niezależne sprawdzenie.",
            $"Przykład dla „{title}”: {baseBody.WorkedExamples}",
            $"1. Ustal, czego dokładnie dotyczy „{title}”. 2. Wybierz właściwą zależność. 3. Podstaw dane i wykonuj działania po kolei. 4. Uprość wynik. 5. Sprawdź go za pomocą działania odwrotnego, równania, własności geometrycznej, sumy danych lub warunku jednostek.",
            $"Najczęstsze błędy: użycie niewłaściwej reguły, zbyt wczesne zaokrąglanie, pominięcie znaku albo jednostki oraz brak sprawdzenia wyniku w warunkach zadania.",
            $"{title}: rozpoznaj strukturę, zastosuj właściwą regułę i sprawdź wynik.");
    }

    private static Body Arabic(string title, string profile)
    {
        var baseBody = English(title, profile);
        return new Body(
            $"يُعالج درس «{title}» كهدف رياضي محدد. يحدد الطالب بنية المسألة أولًا، ثم يختار القاعدة المناسبة، ويجري الحساب بدقة، ويتحقق من النتيجة بالرجوع إلى المعطيات الأصلية.",
            $"القاعدة الأساسية في «{title}»: حدّد العلاقة الرياضية الصحيحة، استخدم القاعدة أو التمثيل المناسب، احتفظ بالقيم الدقيقة قدر الإمكان، ثم نفّذ تحققًا مستقلًا من الإجابة.",
            $"مثال خاص بهدف «{title}»: {baseBody.WorkedExamples}",
            $"1. حدّد المطلوب رياضيًا في «{title}». 2. اختر العلاقة أو القاعدة المناسبة. 3. عوّض بالمعطيات الأصلية ونفّذ الخطوات بترتيب صحيح. 4. بسّط الناتج. 5. تحقّق منه باستخدام العملية العكسية أو المعادلة الأصلية أو الخاصية الهندسية أو مجموع البيانات أو حدود الاحتمال أو الوحدات حسب نوع المسألة.",
            $"من الأخطاء الشائعة: اختيار قاعدة لا تنطبق على الهدف، التقريب المبكر، إهمال الإشارة أو الوحدة، وعدم فحص الإجابة باستخدام شروط المسألة الأصلية.",
            $"«{title}»: حدّد البنية الرياضية، طبّق القاعدة الصحيحة، ثم تحقّق من النتيجة بشكل مستقل.");
    }

    private sealed record Body(
        string Explanation,
        string KeyConcepts,
        string WorkedExamples,
        string Solutions,
        string CommonMistakes,
        string QuickSummary);
}
