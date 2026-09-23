using System.Globalization;
using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Reviewed Polish presentation layer for the R8 runtime compiler.
///
/// The Mathematics engine remains language-neutral authority for parameter
/// generation, solving and independent verification. This layer deliberately
/// does not surface the engine's English prompt/solution text. Instead it
/// renders the already-reviewed Polish canonical target together with a
/// deterministic Polish description of the exact generated family and data.
/// </summary>
public static partial class RichLessonContentV2RuntimeComposer
{
    private static readonly HashSet<string> PolishHiddenParameters =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "variant",
            "mode",
            "ask",
            "method",
            "context",
            "criterion",
            "statistical",
            "isIdentity"
        };

    private static RichLessonContentV2Lesson ComposePolish(
        string lessonCode,
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> families)
    {
        var exampleCount = Math.Clamp(families.Count, 4, 12);
        var generated = Engine.Generate(
            fingerprintNamespace: "rich-lesson-content-v2-pl",
            scopeKey: lessonCode,
            allowedQuestionFamilies: families,
            difficulty: ExactSkillQuestionDifficulty.Standard,
            questionCount: exampleCount,
            seed: StableSeed(lessonCode),
            excludedExposureFingerprints: []);

        var familyLabels = families
            .Select(PolishFamilyLabel)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var examples = generated
            .Select((question, index) =>
                BuildPolishWorkedExample(
                    question,
                    index + 1,
                    contract))
            .ToArray();

        var result = new RichLessonContentV2Lesson
        {
            LessonCode = lessonCode,
            CultureCode = body.CultureCode,
            Title = body.Title,
            ExplanationParagraphs =
            [
                Clean(body.Explanation),
                Clean(body.KeyConceptsAndRules),
                $"Zweryfikowany kontrakt tej lekcji obejmuje następujące typy zadań: {JoinPolish(familyLabels)}.",
                "Przykłady poniżej są tworzone z tego samego kontraktu matematycznego co Practice. " +
                "Silnik wyznacza wynik, a niezależny weryfikator sprawdza go przed pokazaniem przykładu."
            ],
            KeyConcepts = BuildPolishConcepts(body, contract, familyLabels).ToList(),
            WorkedExamples = examples.ToList(),
            CommonMistakes = BuildPolishMistakes(body, familyLabels).ToList(),
            SummaryPoints =
            [
                Clean(body.QuickSummary),
                $"Dokładny zakres matematyczny: {PolishSkillLabel(contract.SkillId)}.",
                $"Ćwicz rozpoznawanie typów zadań: {JoinPolish(familyLabels)}.",
                "Porównuj sposób rozwiązania, nie tylko wynik końcowy.",
                "Na końcu sprawdź, czy wynik odpowiada dokładnie na pytanie i zachowuje wymagane zależności."
            ],
            Visuals =
            [
                new RichLessonVisual
                {
                    Kind = RichLessonVisualKind.EquationSet,
                    Title = $"{body.Title} — zweryfikowane przykłady",
                    Description =
                        "Skrócony zestaw wygenerowanych i zweryfikowanych przypadków z tej lekcji.",
                    PrimaryLabel = PolishSkillLabel(contract.SkillId),
                    Items = examples
                        .Take(4)
                        .Select(x => $"{x.Question} → {x.Answer}")
                        .ToList()
                }
            ],
            Videos = []
        };

        RichLessonContentV2Registry.Validate(
            new RichLessonContentV2Document
            {
                SchemaVersion = 1,
                ContentVersion = "rich-v2-r8-polish-runtime-verified-v1",
                PackCode = "R8-RUNTIME-PL",
                Lessons = [result]
            },
            $"runtime-pl:{lessonCode}");

        return result;
    }

    private static IReadOnlyList<RichLessonKeyConcept> BuildPolishConcepts(
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> familyLabels)
    {
        var result = new List<RichLessonKeyConcept>
        {
            new()
            {
                Title = "Główna idea",
                Definition = Clean(body.Explanation),
                Rule = Clean(body.KeyConceptsAndRules),
                Example =
                    "Zweryfikowane przykłady liczbowe znajdują się w sekcji przykładów z rozwiązaniem."
            },
            new()
            {
                Title = "Dokładny cel lekcji",
                Definition =
                    $"Lekcja realizuje zweryfikowaną umiejętność: {PolishSkillLabel(contract.SkillId)}.",
                Rule =
                    "Rozwiązuj dokładnie ten typ problemu, który wynika z celu lekcji; " +
                    "nie zastępuj go podobną, ale inną umiejętnością.",
                Example =
                    $"Practice używa zweryfikowanego mechanizmu {contract.Mechanic} dla tego samego celu."
            }
        };

        foreach (var label in familyLabels.Take(4))
        {
            result.Add(new RichLessonKeyConcept
            {
                Title = label,
                Definition =
                    $"To jeden ze zweryfikowanych typów zadań w lekcji „{body.Title}”.",
                Rule =
                    $"Najpierw rozpoznaj przypadek „{label.ToLowerInvariant()}”, " +
                    "a następnie zastosuj właściwą zależność matematyczną.",
                Example =
                    "Pełny przykład z konkretnymi danymi znajduje się poniżej."
            });
        }

        return result;
    }

    private static RichLessonWorkedExample BuildPolishWorkedExample(
        ExactSkillGeneratedQuestion question,
        int order,
        LessonPracticeContract contract)
    {
        var label = PolishFamilyLabel(question.Family);
        var questionText = BuildPolishQuestion(question);
        var answer = PolishAnswer(question.CorrectAnswer);

        return new RichLessonWorkedExample
        {
            Title = $"Przykład {order} — {label}",
            Question = questionText,
            Method = PolishMethodForFamily(question.Family),
            Steps =
            [
                $"Rozpoznaj typ zadania: {label}.",
                $"Zapisz dane z zadania: {DescribePolishData(question)}.",
                PolishMethodForFamily(question.Family),
                $"Wyznacz wynik: {answer}.",
                "Sprawdź wynik względem danych początkowych. " +
                "Edulytics dodatkowo weryfikuje wynik niezależnym mechanizmem matematycznym."
            ],
            Answer = answer,
            Check =
                $"Wynik zweryfikowany przez exact Mathematics engine dla rodziny „{label}”."
        };
    }

    private static IReadOnlyList<RichLessonCommonMistake> BuildPolishMistakes(
        CanonicalLessonTranslationRecord body,
        IReadOnlyList<string> familyLabels)
    {
        var first = familyLabels.FirstOrDefault() ?? "wymagany przypadek";

        return
        [
            new()
            {
                Mistake =
                    $"Rozwiązywanie podobnego zadania zamiast dokładnego celu lekcji „{body.Title}”.",
                WhyWrong =
                    "Zmienia to badaną zależność matematyczną i może prowadzić do poprawnych obliczeń dla niewłaściwego problemu.",
                Correction =
                    "Przed obliczeniem nazwij szukaną wielkość i sprawdź, czy użyta reguła odpowiada celowi lekcji."
            },
            new()
            {
                Mistake =
                    $"Traktowanie wszystkich zadań tak samo bez rozpoznania, czy chodzi o {JoinPolish(familyLabels)}.",
                WhyWrong =
                    "Różne przypadki mogą wymagać innej reprezentacji, kolejności działań albo sposobu sprawdzenia.",
                Correction =
                    $"Najpierw rozpoznaj przypadek, na przykład „{first.ToLowerInvariant()}”, a dopiero potem wykonaj obliczenia."
            },
            new()
            {
                Mistake =
                    "Zakończenie pracy po samym obliczeniu bez sprawdzenia sensu wyniku.",
                WhyWrong =
                    "Poprawnie wykonane działanie może nadal odpowiadać na inną wielkość, używać złej jednostki albo łamać warunek zadania.",
                Correction =
                    "Sprawdź wynik przez podstawienie, oszacowanie, działanie odwrotne albo drugą reprezentację odpowiednią dla tematu."
            }
        ];
    }

    private static string BuildPolishQuestion(ExactSkillGeneratedQuestion question)
    {
        if (string.Equals(
                question.Family,
                "supporting.geometry.shape_dimension",
                StringComparison.Ordinal) &&
            question.Parameters.TryGetValue("shape", out var shape))
        {
            return $"Określ, czy figura „{PolishShape(shape)}” jest figurą 2D czy bryłą 3D.";
        }

        if (string.Equals(
                question.Family,
                "supporting.measurement.calendar_next_day",
                StringComparison.Ordinal) &&
            question.Parameters.TryGetValue("day", out var day) &&
            question.Parameters.TryGetValue("direction", out var direction))
        {
            var relation = direction > 0 ? "jutro" : "wczoraj";
            return $"Jeżeli dziś jest {PolishDay(day)}, jaki dzień będzie {relation}?";
        }

        if (string.Equals(
                question.Family,
                "supporting.geometry.position_direction",
                StringComparison.Ordinal) &&
            question.Parameters.TryGetValue("start", out var start) &&
            question.Parameters.TryGetValue("steps", out var steps) &&
            question.Parameters.TryGetValue("direction", out var moveDirection))
        {
            var directionText = moveDirection > 0 ? "w prawo" : "w lewo";
            return $"Na osi liczbowej zacznij od {start} i przesuń się o {steps} kroków {directionText}. Gdzie się zatrzymasz?";
        }

        var data = DescribePolishData(question);
        var label = PolishFamilyLabel(question.Family);
        var instruction = PolishInstructionForFamily(question.Family);

        return string.IsNullOrWhiteSpace(data)
            ? $"{instruction} Typ zadania: {label}."
            : $"{instruction} Typ zadania: {label}. Dane: {data}.";
    }

    private static string DescribePolishData(ExactSkillGeneratedQuestion question)
    {
        var values = question.Parameters
            .Where(x => !PolishHiddenParameters.Contains(x.Key))
            .OrderBy(x => x.Key, StringComparer.Ordinal)
            .Take(12)
            .Select(x => $"{PolishParameterLabel(x.Key)} = {PolishParameterValue(x.Key, x.Value)}")
            .ToArray();

        return values.Length == 0
            ? "brak dodatkowych danych liczbowych"
            : string.Join(", ", values);
    }

    private static string PolishInstructionForFamily(string family)
    {
        if (family.Contains("compare", StringComparison.Ordinal))
            return "Porównaj podane wartości zgodnie z warunkami zadania.";
        if (family.Contains("add", StringComparison.Ordinal))
            return "Wykonaj wymagane dodawanie.";
        if (family.Contains("subtract", StringComparison.Ordinal))
            return "Wykonaj wymagane odejmowanie.";
        if (family.Contains("multiply", StringComparison.Ordinal))
            return "Wykonaj wymagane mnożenie.";
        if (family.Contains("divide", StringComparison.Ordinal))
            return "Wykonaj wymagane dzielenie.";
        if (family.Contains("round", StringComparison.Ordinal))
            return "Zaokrąglij wartość do wskazanego miejsca.";
        if (family.Contains("area", StringComparison.Ordinal))
            return "Wyznacz wymagane pole.";
        if (family.Contains("perimeter", StringComparison.Ordinal) ||
            family.Contains("circumference", StringComparison.Ordinal))
            return "Wyznacz wymaganą długość obwodu.";
        if (family.Contains("volume", StringComparison.Ordinal))
            return "Wyznacz wymaganą objętość.";
        if (family.Contains("angle", StringComparison.Ordinal))
            return "Wyznacz albo sklasyfikuj wymagany kąt.";
        if (family.Contains("mean", StringComparison.Ordinal))
            return "Wyznacz średnią.";
        if (family.Contains("median", StringComparison.Ordinal))
            return "Wyznacz medianę.";
        if (family.Contains("range", StringComparison.Ordinal))
            return "Wyznacz rozstęp.";
        if (family.Contains("probability", StringComparison.Ordinal))
            return "Wyznacz wymagane prawdopodobieństwo.";
        if (family.Contains("equation", StringComparison.Ordinal) ||
            family.Contains("solve", StringComparison.Ordinal) ||
            family.Contains("inequality", StringComparison.Ordinal))
            return "Rozwiąż podane równanie albo nierówność.";
        if (family.Contains("evaluate", StringComparison.Ordinal) ||
            family.Contains("substitute", StringComparison.Ordinal))
            return "Podstaw dane i oblicz wymaganą wartość.";
        if (family.Contains("sequence", StringComparison.Ordinal) ||
            family.Contains("series", StringComparison.Ordinal))
            return "Zastosuj regułę ciągu albo szeregu i wyznacz wymaganą wartość.";
        if (family.Contains("vector", StringComparison.Ordinal))
            return "Wykonaj wskazaną operację na wektorach.";
        if (family.Contains("derivative", StringComparison.Ordinal))
            return "Wyznacz pochodną i oblicz wymaganą wartość.";
        if (family.Contains("integral", StringComparison.Ordinal))
            return "Oblicz wskazaną całkę.";
        if (family.Contains("transform", StringComparison.Ordinal) ||
            family.Contains("reflect", StringComparison.Ordinal) ||
            family.Contains("rotate", StringComparison.Ordinal) ||
            family.Contains("translate", StringComparison.Ordinal))
            return "Wykonaj wskazane przekształcenie geometryczne.";

        return "Rozwiąż zadanie zgodnie z dokładną regułą matematyczną tej lekcji.";
    }

    private static string PolishMethodForFamily(string family)
    {
        if (family.Contains("pythagorean", StringComparison.Ordinal))
            return "Użyj twierdzenia Pitagorasa a² + b² = c² i sprawdź otrzymaną długość.";
        if (family.Contains("percent", StringComparison.Ordinal))
            return "Zapisz procent jako część ze 100, wykonaj odpowiednie mnożenie lub zmianę i sprawdź wynik względem wartości początkowej.";
        if (family.Contains("fraction", StringComparison.Ordinal))
            return "Zachowaj znaczenie licznika i mianownika; w razie potrzeby użyj wspólnego mianownika, skracania albo równoważnego zapisu.";
        if (family.Contains("ratio", StringComparison.Ordinal) ||
            family.Contains("rate", StringComparison.Ordinal))
            return "Zapisz relację między wielkościami, sprowadź ją do wymaganej postaci i sprawdź proporcję.";
        if (family.Contains("mean", StringComparison.Ordinal))
            return "Dodaj wszystkie wartości i podziel sumę przez liczbę wartości.";
        if (family.Contains("median", StringComparison.Ordinal))
            return "Uporządkuj dane i znajdź wartość środkową; przy parzystej liczbie danych użyj średniej z dwóch środkowych.";
        if (family.Contains("range", StringComparison.Ordinal))
            return "Odejmij najmniejszą wartość od największej.";
        if (family.Contains("area", StringComparison.Ordinal))
            return "Dobierz właściwy wzór na pole, podstaw dane z zachowaniem jednostek i sprawdź sens wyniku.";
        if (family.Contains("volume", StringComparison.Ordinal))
            return "Dobierz wzór na objętość, podstaw wymiary i zachowaj jednostki sześcienne.";
        if (family.Contains("perimeter", StringComparison.Ordinal) ||
            family.Contains("circumference", StringComparison.Ordinal))
            return "Zastosuj właściwą zależność na obwód i sprawdź, czy uwzględniono wszystkie wymagane długości.";
        if (family.Contains("angle", StringComparison.Ordinal))
            return "Użyj odpowiedniej własności kątów lub figury, zapisz równanie i sprawdź sumę kątów.";
        if (family.Contains("equation", StringComparison.Ordinal) ||
            family.Contains("linear", StringComparison.Ordinal))
            return "Wykonuj równoważne działania po obu stronach, wyizoluj niewiadomą i sprawdź wynik przez podstawienie.";
        if (family.Contains("inequality", StringComparison.Ordinal))
            return "Przekształcaj obie strony nierówności; przy mnożeniu lub dzieleniu przez liczbę ujemną odwróć znak nierówności.";
        if (family.Contains("sequence", StringComparison.Ordinal))
            return "Rozpoznaj stałą regułę ciągu, zastosuj ją do wskazanego wyrazu i sprawdź na sąsiednich wyrazach.";
        if (family.Contains("probability", StringComparison.Ordinal))
            return "Określ przestrzeń zdarzeń, policz przypadki wymagane przez zadanie i zapisz prawdopodobieństwo w uproszczonej postaci.";
        if (family.Contains("vector", StringComparison.Ordinal))
            return "Pracuj składowa po składowej i sprawdź wynik zgodnie z definicją operacji na wektorach.";
        if (family.Contains("derivative", StringComparison.Ordinal))
            return "Różniczkuj zgodnie z regułami rachunku różniczkowego, a następnie podstaw wskazaną wartość.";
        if (family.Contains("integral", StringComparison.Ordinal))
            return "Wyznacz funkcję pierwotną, zastosuj granice całkowania i odejmij wartość dla dolnej granicy od wartości dla górnej.";
        if (family.Contains("round", StringComparison.Ordinal))
            return "Znajdź dwie sąsiednie wielokrotności wskazanego miejsca, porównaj z punktem połowy i wybierz bliższą.";
        if (family.Contains("place_value", StringComparison.Ordinal))
            return "Odczytaj pozycję cyfry w zapisie dziesiętnym i połącz ją z odpowiednią potęgą liczby 10.";

        return "Zastosuj definicję lub regułę właściwą dla tego dokładnego typu zadania, wykonaj obliczenia i niezależnie sprawdź wynik.";
    }

    private static string PolishFamilyLabel(string family)
    {
        var tokens = family
            .Split(['.', '_', '-'], StringSplitOptions.RemoveEmptyEntries)
            .Where(x => !string.Equals(x, "supporting", StringComparison.OrdinalIgnoreCase))
            .Select(PolishMathToken)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .ToArray();

        return tokens.Length == 0
            ? "zweryfikowany przypadek matematyczny"
            : string.Join(" · ", tokens);
    }

    private static string PolishSkillLabel(string skillId) =>
        string.Join(
            " · ",
            skillId
                .Split(['.', '_', '-'], StringSplitOptions.RemoveEmptyEntries)
                .Where(x => !string.Equals(x, "supporting", StringComparison.OrdinalIgnoreCase))
                .Select(PolishMathToken)
                .Where(x => !string.IsNullOrWhiteSpace(x)));

    private static string PolishMathToken(string token)
    {
        var value = token.ToLowerInvariant();
        return value switch
        {
            "number" => "liczby",
            "whole" => "całkowite nieujemne",
            "real" => "rzeczywiste",
            "negative" => "ujemne",
            "place" => "pozycja",
            "value" => "wartość",
            "round" or "rounding" => "zaokrąglanie",
            "compare" => "porównywanie",
            "order" => "porządkowanie",
            "add" => "dodawanie",
            "subtract" => "odejmowanie",
            "multiply" => "mnożenie",
            "divide" => "dzielenie",
            "operations" or "operation" => "działania",
            "lcm" => "NWW",
            "gcf" => "NWD",
            "prime" => "pierwsze",
            "factor" => "czynnik",
            "power" or "power10" => "potęga",
            "root" => "pierwiastek",
            "fractions" or "fraction" => "ułamki",
            "numerator" => "licznik",
            "denominator" => "mianownik",
            "equivalent" => "równoważne",
            "simplify" => "upraszczanie",
            "represent" or "interpret" => "reprezentacja",
            "bar" => "model paskowy",
            "unlike" => "różne mianowniki",
            "common" => "wspólne",
            "benchmark" => "punkt odniesienia",
            "quantity" => "wielkość",
            "decimals" => "ułamki dziesiętne",
            "percent" or "percentages" => "procenty",
            "increase" => "zwiększenie",
            "decrease" => "zmniejszenie",
            "fdp" => "ułamki · dziesiętne · procenty",
            "ratio" => "stosunek",
            "rate" => "wskaźnik",
            "unit" => "jednostkowy",
            "algebra" or "algebraic" => "algebra",
            "linear" => "liniowe",
            "equation" => "równanie",
            "inequality" => "nierówność",
            "variables" => "zmienne",
            "both" => "obie strony",
            "substitute" => "podstawianie",
            "expand" => "rozwijanie",
            "simultaneous" => "układ równań",
            "quadratic" => "kwadratowe",
            "expressions" => "wyrażenia",
            "inverse" => "odwrotna",
            "proportion" => "proporcjonalność",
            "functions" => "funkcje",
            "composite" => "złożenie",
            "evaluate" => "obliczanie wartości",
            "sequences" => "ciągi",
            "series" => "szeregi",
            "geometric" => "geometryczne",
            "skip" => "liczenie skokowe",
            "next" => "następny wyraz",
            "sum" => "suma",
            "geometry" => "geometria",
            "coordinate" => "współrzędne",
            "analytic" => "analityczna",
            "angle" or "angles" => "kąty",
            "parallel" => "proste równoległe",
            "supplementary" => "kąty przyległe",
            "similarity" => "podobieństwo",
            "polygons" => "wielokąty",
            "interior" => "wewnętrzne",
            "regular" => "foremne",
            "rectangle" => "prostokąt",
            "triangle" => "trójkąt",
            "right" => "prostokątny",
            "pythagorean" => "twierdzenie Pitagorasa",
            "circle" => "okrąg",
            "arc" => "łuk",
            "circumference" => "obwód okręgu",
            "area" => "pole",
            "perimeter" => "obwód",
            "volume" => "objętość",
            "surface" => "powierzchnia",
            "cuboid" => "prostopadłościan",
            "shape" => "figura",
            "dimension" => "wymiar",
            "position" => "położenie",
            "direction" => "kierunek",
            "midpoint" => "środek odcinka",
            "distance" => "odległość",
            "axis" => "oś",
            "reflect" => "odbicie",
            "translate" => "przesunięcie",
            "transformations" => "przekształcenia",
            "locus" => "miejsce geometryczne",
            "equidistant" => "równoodległe",
            "measurement" => "pomiary",
            "conversion" => "zamiana jednostek",
            "time" => "czas",
            "elapsed" => "upływ czasu",
            "money" => "pieniądze",
            "calendar" => "kalendarz",
            "day" => "dzień",
            "iteration" => "powtarzanie jednostki",
            "scale" => "skala",
            "statistics" => "statystyka",
            "center" => "miary położenia",
            "spread" or "range" => "rozproszenie",
            "mean" => "średnia",
            "median" => "mediana",
            "charts" => "wykresy",
            "collection" => "zbieranie danych",
            "table" => "tabela",
            "total" => "suma",
            "probability" => "prawdopodobieństwo",
            "theoretical" => "teoretyczne",
            "event" => "zdarzenie",
            "conditional" => "warunkowe",
            "binomial" => "dwumianowe",
            "coins" => "monety",
            "combinatorics" => "kombinatoryka",
            "choose" => "wybór",
            "calculus" => "analiza matematyczna",
            "derivative" => "pochodna",
            "definite" => "oznaczona",
            "integral" => "całka",
            "optimization" => "optymalizacja",
            "exponentials" => "funkcje wykładnicze",
            "exponent" => "wykładnik",
            "logarithms" => "logarytmy",
            "indices" => "potęgi",
            "surds" => "pierwiastki",
            "standard" => "standardowa",
            "form" => "postać",
            "trigonometry" => "trygonometria",
            "sine" => "sinus",
            "cosine" => "cosinus",
            "graph" => "wykres",
            "identity" => "tożsamość",
            "vectors" => "wektory",
            "magnitude" => "długość",
            "component" => "składowa",
            "between" => "między punktami",
            "reasoning" => "rozumowanie",
            "multistep" => "wielokrokowe",
            "verify" => "weryfikacja",
            "direct" => "bezpośredni",
            "exact" => "dokładny",
            "build" => "budowanie idei",
            "apply" => "zastosowanie",
            "core" => "podstawowy",
            "mixed" => "mieszany",
            "find" => "wyznaczanie",
            "missing" => "brakująca wartość",
            "recognize" => "rozpoznawanie",
            "select" => "wybór",
            "greater" => "większa wartość",
            "true" => "prawda",
            "false" => "fałsz",
            "three" => "trzy",
            "two" => "dwa",
            "one" => "jeden",
            "within" => "w zakresie",
            "columnar" => "pisemne",
            "complement" => "dopełnienie",
            "relation" => "relacja",
            "method" => "metoda",
            "rule" => "reguła",
            "coefficient" => "współczynnik",
            "intercept" => "wyraz wolny",
            "gradient" => "nachylenie",
            "intersection" => "punkt przecięcia",
            "solid" => "bryły",
            "semicircle" => "półokrąg",
            "degrees" => "stopnie",
            "smallest" => "najmniejszy",
            "special" => "wartości szczególne",
            "double" => "podwójny",
            "around" => "wokół punktu",
            "point" or "points" => "punkt",
            "side" or "sides" => "bok",
            "leg" => "przyprostokątna",
            "length" => "długość",
            "line" or "lines" => "prosta",
            "from" => "z",
            "of" => "",
            "or" => "lub",
            "plus" => "plus",
            "equals" => "równa się",
            "ax" or "b" or "c" or "y" => value,
            "100" => "100",
            _ when value.All(char.IsDigit) => value,
            _ when value.Length <= 2 => value,
            _ => "przypadek"
        };
    }

    private static string PolishParameterLabel(string key)
    {
        return key switch
        {
            "left" => "lewa wartość",
            "right" => "prawa wartość",
            "larger" => "większa wartość",
            "smaller" => "mniejsza wartość",
            "quantity" => "wielkość",
            "percent" => "procent",
            "numerator" => "licznik",
            "denominator" => "mianownik",
            "dividend" => "dzielna",
            "divisor" => "dzielnik",
            "quotient" => "iloraz",
            "remainder" => "reszta",
            "difference" => "różnica",
            "sum" => "suma",
            "product" => "iloczyn",
            "base" => "podstawa",
            "height" => "wysokość",
            "width" => "szerokość",
            "length" => "długość",
            "radius" => "promień",
            "angle" => "kąt",
            "angleA" => "kąt A",
            "angleB" => "kąt B",
            "knownAngle" => "znany kąt",
            "missingAngle" => "szukany kąt",
            "hypotenuse" => "przeciwprostokątna",
            "opposite" => "bok naprzeciw",
            "scaleFactor" => "skala podobieństwa",
            "first" => "pierwszy wyraz",
            "difference" => "różnica",
            "ratio" => "iloraz ciągu",
            "n" => "n",
            "target" => "wartość docelowa",
            "position" => "pozycja",
            "step" => "krok",
            "steps" => "liczba kroków",
            "start" => "początek",
            "end" => "koniec",
            "count" => "liczba elementów",
            "value" => "wartość",
            "digit" => "cyfra",
            "place" => "miejsce",
            "power" => "wykładnik",
            "exponent" => "wykładnik",
            "factor" => "czynnik",
            "scalar" => "skalar",
            "distance" => "odległość",
            "frequency" => "częstość",
            "favourable" => "przypadki sprzyjające",
            "total" => "liczba wszystkich przypadków",
            "trials" => "liczba prób",
            "given" => "liczba przypadków warunku",
            "intersection" => "część wspólna",
            "rangeA" => "rozstęp A",
            "rangeB" => "rozstęp B",
            "cost" => "koszt",
            "paid" => "kwota zapłacona",
            "units" => "liczba jednostek",
            "intervals" => "liczba przedziałów",
            "coefficient" => "współczynnik",
            "argument" => "argument",
            "lambda" => "λ",
            "force" => "siła",
            "normal" => "reakcja normalna",
            "speed" => "prędkość",
            "t" => "czas",
            "u" => "prędkość początkowa",
            "x" or "y" or "a" or "b" or "c" or "d" or "g" or "h" or
            "k" or "p" or "q" or "l" or "w" or "x1" or "x2" or
            "y1" or "y2" or "a1" or "a2" or "b1" or "b2" or
            "f1" or "f2" or "f3" or "f4" or "v1" or "v2" or "v3" or
            "v4" or "v5" or "m1" or "m2" or "n1" or "n2" or "n3" or
            "d1" or "d2" or "d3" or "ax" or "ay" or "bx" or "by" or
            "dx" or "dy" => key,
            _ => HumanizePolishParameterKey(key)
        };
    }

    private static string HumanizePolishParameterKey(string key)
    {
        var parts = Regex
            .Matches(key, @"[A-Z]?[a-z]+|[A-Z]+(?![a-z])|\d+")
            .Select(x => PolishMathToken(x.Value))
            .Where(x => !string.IsNullOrWhiteSpace(x) &&
                        !string.Equals(x, "przypadek", StringComparison.Ordinal))
            .ToArray();

        return parts.Length == 0
            ? "parametr"
            : string.Join(" ", parts);
    }

    private static string PolishParameterValue(string key, int value)
    {
        if (string.Equals(key, "direction", StringComparison.OrdinalIgnoreCase))
            return value > 0 ? "w prawo / naprzód" : "w lewo / wstecz";

        if (string.Equals(key, "shape", StringComparison.OrdinalIgnoreCase))
            return PolishShape(value);

        if (string.Equals(key, "day", StringComparison.OrdinalIgnoreCase))
            return PolishDay(value);

        return value.ToString(CultureInfo.InvariantCulture);
    }

    private static string PolishAnswer(string answer)
    {
        var value = answer.Trim();
        return value.ToLowerInvariant() switch
        {
            "true" => "prawda",
            "false" => "fałsz",
            "positive" => "dodatnia",
            "negative" => "ujemna",
            "equal" => "równe",
            "acute" => "kąt ostry",
            "right" => "kąt prosty",
            "obtuse" => "kąt rozwarty",
            "straight" => "kąt półpełny",
            "reflex" => "kąt wklęsły",
            "perpendicular bisector" => "symetralna odcinka",
            "left" => "lewo",
            "monday" => "poniedziałek",
            "tuesday" => "wtorek",
            "wednesday" => "środa",
            "thursday" => "czwartek",
            "friday" => "piątek",
            "saturday" => "sobota",
            "sunday" => "niedziela",
            "square" => "kwadrat",
            "rectangle" => "prostokąt",
            "triangle" => "trójkąt",
            "circle" => "koło",
            "cube" => "sześcian",
            "cuboid" => "prostopadłościan",
            "sphere" => "kula",
            "cylinder" => "walec",
            "triangular prism" => "graniastosłup trójkątny",
            "ruler" => "linijka",
            "scale" => "waga",
            "measuring jug" => "dzbanek z podziałką",
            "as x increases, y generally increases" => "gdy x rośnie, y na ogół rośnie",
            "as x increases, y generally decreases" => "gdy x rośnie, y na ogół maleje",
            "there is no clear upward or downward trend" => "brak wyraźnego trendu rosnącego lub malejącego",
            _ => value
        };
    }

    private static string PolishShape(int shape) =>
        shape switch
        {
            0 => "kwadrat",
            1 => "prostokąt",
            2 => "trójkąt",
            3 => "koło",
            4 => "sześcian",
            5 => "prostopadłościan",
            6 => "kula",
            7 => "walec",
            _ => "figura"
        };

    private static string PolishDay(int day) =>
        ((day % 7) + 7) % 7 switch
        {
            0 => "poniedziałek",
            1 => "wtorek",
            2 => "środa",
            3 => "czwartek",
            4 => "piątek",
            5 => "sobota",
            _ => "niedziela"
        };

    private static string JoinPolish(IReadOnlyList<string> values)
    {
        if (values.Count == 0)
            return "zweryfikowane przypadki tej lekcji";
        if (values.Count == 1)
            return values[0];
        if (values.Count == 2)
            return $"{values[0]} oraz {values[1]}";

        return $"{string.Join(", ", values.Take(values.Count - 1))} oraz {values[^1]}";
    }
}
