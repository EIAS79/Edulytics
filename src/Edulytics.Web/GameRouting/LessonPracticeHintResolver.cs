using System.Text.Json;

namespace Edulytics.Web.GameRouting;

public static class LessonPracticeHintResolver
{
    public static string Resolve(
        string? family,
        string? parametersJson,
        string locale)
    {
        IReadOnlyDictionary<string, int> parameters =
            ParseParameters(parametersJson);

        return Resolve(family, parameters, locale);
    }

    public static string Resolve(
        string? family,
        IReadOnlyDictionary<string, int>? parameters,
        string locale)
    {
        parameters ??= new Dictionary<string, int>(
            StringComparer.Ordinal);

        var mode = parameters.TryGetValue("mode", out var parsedMode)
            ? parsedMode
            : -1;

        var copy = family switch
        {
            "supporting.reasoning.multistep" =>
                mode switch
                {
                    0 => (
                        "Undo the addition first, then divide by the multiplier.",
                        "Najpierw cofnij dodawanie, potem podziel przez mnożnik.",
                        "اعكس الجمع أولًا، ثم اقسم على معامل الضرب."),
                    1 => (
                        "Undo the multiplication first, then undo the addition.",
                        "Najpierw cofnij mnożenie, potem dodawanie.",
                        "اعكس الضرب أولًا، ثم اعكس الجمع."),
                    2 => (
                        "Translate 'greater than' carefully, then rebuild the multiplicative relationship.",
                        "Uważnie przetłumacz relację „więcej niż”, a potem odtwórz zależność mnożeniową.",
                        "فسّر «أكبر من» بدقة، ثم أعد بناء العلاقة الضربية."),
                    3 => (
                        "Find one equal part first, then multiply by the number of parts.",
                        "Najpierw znajdź jedną równą część, potem pomnóż przez liczbę części.",
                        "أوجد قيمة جزء واحد أولًا، ثم اضرب في عدد الأجزاء."),
                    4 => (
                        "Undo the multiplication first, then add back the amount that was removed.",
                        "Najpierw cofnij mnożenie, potem dodaj z powrotem odjętą wartość.",
                        "اعكس الضرب أولًا، ثم أضف المقدار الذي تم طرحه."),
                    5 => (
                        "Undo the subtraction first, then divide by the multiplier.",
                        "Najpierw cofnij odejmowanie, potem podziel przez mnożnik.",
                        "اعكس الطرح أولًا، ثم اقسم على معامل الضرب."),
                    6 => (
                        "Represent the comparison with an equation before calculating.",
                        "Przed obliczeniem zapisz porównanie jako równanie.",
                        "مثّل المقارنة بمعادلة قبل الحساب."),
                    7 => (
                        "Work backwards through the operations in the reverse order.",
                        "Cofaj działania w odwrotnej kolejności.",
                        "اعمل عكسيًا عبر العمليات بالترتيب المعاكس."),
                    _ => (
                        "Identify the operations in order, then work backwards to find the unknown.",
                        "Rozpoznaj kolejność działań, a potem pracuj wstecz, aby znaleźć niewiadomą.",
                        "حدد العمليات بالترتيب، ثم اعمل عكسيًا لإيجاد المجهول.")
                },

            "supporting.powers10.evaluate" => (
                "Write the power of ten as repeated multiplication, then evaluate it.",
                "Zapisz potęgę dziesięciu jako wielokrotne mnożenie, a potem oblicz.",
                "اكتب قوة العشرة كضرب متكرر ثم احسبها."),

            "supporting.powers10.multiply" => (
                "Multiplying by 10^n shifts every digit n places to the left in place value.",
                "Mnożenie przez 10^n przesuwa każdą cyfrę o n miejsc w lewo.",
                "الضرب في 10^n ينقل كل رقم n خانات إلى اليسار في القيمة المكانية."),

            "supporting.powers10.divide" => (
                "Dividing by 10^n shifts every digit n places to the right in place value.",
                "Dzielenie przez 10^n przesuwa każdą cyfrę o n miejsc w prawo.",
                "القسمة على 10^n تنقل كل رقم n خانات إلى اليمين في القيمة المكانية."),

            "supporting.powers10.missing_exponent" => (
                "Count how many powers of ten scale the starting number to the result.",
                "Policz, ile potęg dziesięciu skaluje liczbę początkową do wyniku.",
                "احسب عدد قوى العشرة التي تنقل العدد الابتدائي إلى الناتج."),

            "supporting.indices.power_or_root" =>
                mode == 1
                    ? (
                        "Find the positive number whose square gives the radicand.",
                        "Znajdź dodatnią liczbę, której kwadrat daje liczbę pod pierwiastkiem.",
                        "أوجد العدد الموجب الذي مربعه يساوي العدد تحت الجذر.")
                    : (
                        "Write the power as repeated multiplication before calculating.",
                        "Przed obliczeniem zapisz potęgę jako wielokrotne mnożenie.",
                        "اكتب القوة كضرب متكرر قبل الحساب."),

            "algebra.relationships.two_unknowns.total_difference" => (
                "Use both relationships together; one equation alone is not enough.",
                "Użyj obu zależności razem; jedno równanie nie wystarczy.",
                "استخدم العلاقتين معًا؛ معادلة واحدة لا تكفي."),

            "measurement.scale.equal_intervals.read_value" => (
                "Count equal intervals, find the value of one interval, then locate the pointer.",
                "Policz równe przedziały, znajdź wartość jednego przedziału, potem odczytaj wskazanie.",
                "احسب الفواصل المتساوية، ثم قيمة الفاصل الواحد، ثم حدد المؤشر."),

            "fractions.compare.unlike.common_denominator" => (
                "Use a common denominator or cross-products to compare the complete fraction values.",
                "Porównaj wartości ułamków za pomocą wspólnego mianownika lub mnożenia krzyżowego.",
                "قارن قيمة الكسرين باستخدام مقام مشترك أو الضرب التبادلي."),

            "fractions.equivalent.missing_value" or
            "fractions.equivalent.recognize" or
            "fractions.equivalent.generate_multiple" or
            "fractions.equivalent.number_line" or
            "fractions.equivalent.reduce_common_factor" => (
                "Equivalent fractions keep the same value when numerator and denominator change by the same factor.",
                "Ułamki równoważne zachowują wartość, gdy licznik i mianownik zmieniają się przez ten sam czynnik.",
                "الكسور المتكافئة تحافظ على القيمة عندما يتغير البسط والمقام بالعامل نفسه."),

            "ratio.unit_rate.direct" or
            "ratio.unit_rate.equivalent_ratio" => (
                "Keep the same multiplicative relationship on both parts of the ratio.",
                "Zachowaj tę samą zależność mnożeniową po obu stronach stosunku.",
                "حافظ على العلاقة الضربية نفسها في جزأي النسبة."),

            _ => (
                "Use the mathematical relationship shown in the question and check each step.",
                "Użyj zależności matematycznej pokazanej w pytaniu i sprawdź każdy krok.",
                "استخدم العلاقة الرياضية في السؤال وتحقق من كل خطوة.")
        };

        return locale.Equals("pl", StringComparison.OrdinalIgnoreCase)
            ? copy.Item2
            : locale.Equals("ar", StringComparison.OrdinalIgnoreCase)
                ? copy.Item3
                : copy.Item1;
    }

    private static IReadOnlyDictionary<string, int> ParseParameters(
        string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new Dictionary<string, int>(StringComparer.Ordinal);

        try
        {
            using var document = JsonDocument.Parse(json);

            JsonElement element = document.RootElement;
            if (element.ValueKind == JsonValueKind.Object &&
                element.TryGetProperty("parameters", out var nested))
            {
                element = nested;
            }

            if (element.ValueKind != JsonValueKind.Object)
                return new Dictionary<string, int>(StringComparer.Ordinal);

            var result = new Dictionary<string, int>(
                StringComparer.Ordinal);

            foreach (var property in element.EnumerateObject())
            {
                if (property.Value.ValueKind == JsonValueKind.Number &&
                    property.Value.TryGetInt32(out var value))
                {
                    result[property.Name] = value;
                }
            }

            return result;
        }
        catch (JsonException)
        {
            return new Dictionary<string, int>(StringComparer.Ordinal);
        }
    }
}
