using System.Collections.Concurrent;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Edulytics.Core.Curriculum;
using Edulytics.Core.Lessons;
using Edulytics.Core.Mathematics.Practice;
using Edulytics.Services.Mathematics;

namespace Edulytics.Services.LessonContent;

/// <summary>
/// Catalogue-wide Rich Lesson Content V2 compiler for lessons that do not yet
/// have a hand-authored/curated Rich V2 sidecar.
///
/// The compiler is intentionally conservative:
/// - explicit Rich V2 sidecars remain authoritative and are resolved first;
/// - only READY_VERIFIED lesson Practice contracts may be used;
/// - every generated worked example comes from the exact Mathematics engine,
///   which solves and independently verifies the answer before it is returned;
/// - English and Polish are authored through separate deterministic presentation
///   paths. Polish is enabled only for the reviewed PL-NATIONAL exact Practice
///   contracts and never by translating an unrelated English lesson body.
/// </summary>
public static partial class RichLessonContentV2RuntimeComposer
{
    private static readonly ConcurrentDictionary<string, RichLessonContentV2Lesson>
        Cache = new(StringComparer.Ordinal);

    private static readonly ExactSkillContractQuestionEngine Engine = new();

    private static readonly Regex SentenceSplit =
        new(@"(?<=[.!?])\s+", RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static RichLessonContentV2Lesson? TryCompose(
        string lessonCode,
        CanonicalLessonTranslationRecord body)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(lessonCode);
        ArgumentNullException.ThrowIfNull(body);

        var culture = NormalizeCulture(body.CultureCode);

        if (!string.Equals(culture, "en", StringComparison.Ordinal) &&
            !string.Equals(culture, "pl", StringComparison.Ordinal))
        {
            return null;
        }

        if (!LessonPracticeContractRegistry.TryResolve(
                lessonCode,
                out var contract) ||
            contract is null ||
            !string.Equals(
                contract.Readiness,
                "READY_VERIFIED",
                StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        if (string.Equals(culture, "pl", StringComparison.Ordinal) &&
            !string.Equals(
                contract.SourceType,
                "PolishOfficialOutcomeMap",
                StringComparison.Ordinal))
        {
            return null;
        }

        var supportedFamilies = contract.AllowedQuestionFamilies
            .Where(ExactSkillContractQuestionEngine.SupportsFamily)
            .Distinct(StringComparer.Ordinal)
            .ToArray();

        if (supportedFamilies.Length == 0)
            return null;

        var cacheKey = string.Join(
            "\n",
            lessonCode,
            culture,
            body.Title,
            body.Explanation,
            body.KeyConceptsAndRules,
            body.WorkedExamples,
            body.StepByStepSolutions,
            body.CommonMistakes,
            body.QuickSummary,
            contract.ContractVersion);

        try
        {
            return Cache.GetOrAdd(
                cacheKey,
                _ => string.Equals(culture, "pl", StringComparison.Ordinal)
                    ? ComposePolish(
                        lessonCode,
                        body,
                        contract,
                        supportedFamilies)
                    : Compose(
                        lessonCode,
                        body,
                        contract,
                        supportedFamilies));
        }
        catch (InvalidOperationException)
        {
            // Rich V2 is an enhancement layer. If a lesson cannot be compiled
            // deterministically, fail closed to the existing canonical body
            // rather than breaking Student/Teacher lesson access.
            return null;
        }
    }

    private static RichLessonContentV2Lesson Compose(
        string lessonCode,
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> families)
    {
        if (string.Equals(
                NormalizeCulture(body.CultureCode),
                "pl",
                StringComparison.Ordinal))
        {
            return ComposePolish(
                lessonCode,
                body,
                contract,
                families);
        }

        var exampleCount = Math.Clamp(
            families.Count,
            4,
            12);

        var generated = Engine.Generate(
            fingerprintNamespace: "rich-lesson-content-v2",
            scopeKey: lessonCode,
            allowedQuestionFamilies: families,
            difficulty: ExactSkillQuestionDifficulty.Standard,
            questionCount: exampleCount,
            seed: StableSeed(lessonCode),
            excludedExposureFingerprints: []);

        var familyLabels = families
            .Select(HumanizeFamily)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var explanation = BuildExplanation(
            body,
            contract,
            familyLabels);

        var keyConcepts = BuildConcepts(
            body,
            contract,
            familyLabels);

        var examples = generated
            .Select((question, index) =>
                BuildWorkedExample(
                    question,
                    index + 1,
                    contract))
            .ToArray();

        var mistakes = BuildMistakes(
            body,
            contract,
            familyLabels);

        var summary = BuildSummary(
            body,
            contract,
            familyLabels);

        var visuals = BuildVisuals(
            body.Title,
            examples,
            contract);

        var result = new RichLessonContentV2Lesson
        {
            LessonCode = lessonCode,
            CultureCode = body.CultureCode,
            Title = body.Title,
            ExplanationParagraphs = explanation.ToList(),
            KeyConcepts = keyConcepts.ToList(),
            WorkedExamples = examples.ToList(),
            CommonMistakes = mistakes.ToList(),
            SummaryPoints = summary.ToList(),
            Visuals = visuals.ToList(),
            Videos = []
        };

        RichLessonContentV2Registry.Validate(
            new RichLessonContentV2Document
            {
                SchemaVersion = 1,
                ContentVersion = "rich-v2-r8-runtime-verified-v1",
                PackCode = "R8-RUNTIME",
                Lessons = [result]
            },
            $"runtime:{lessonCode}");

        return result;
    }


    private static RichLessonContentV2Lesson ComposePolish(
        string lessonCode,
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> families)
    {
        var exampleCount = Math.Clamp(
            families.Count,
            4,
            12);

        var generated = Engine.Generate(
            fingerprintNamespace: "rich-lesson-content-v2-pl",
            scopeKey: lessonCode,
            allowedQuestionFamilies: families,
            difficulty: ExactSkillQuestionDifficulty.Standard,
            questionCount: exampleCount,
            seed: StableSeed(lessonCode),
            excludedExposureFingerprints: []);

        var familyLabels = families
            .Select(PolishMathematicsTextLocalizer.FamilyLabel)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        var examples = generated
            .Select((question, index) =>
                BuildPolishWorkedExample(
                    question,
                    index + 1,
                    contract))
            .ToArray();

        var explanation = new[]
        {
            PolishMathematicsTextLocalizer.Localize(body.Explanation),
            PolishMathematicsTextLocalizer.Localize(body.KeyConceptsAndRules),
            $"Zweryfikowany kontrakt tej lekcji obejmuje następujące postacie zadań: " +
            $"{JoinPolish(familyLabels)}. Wszystkie pozostają w zakresie dokładnego celu " +
            "matematycznego przypisanego do oficjalnego wymagania.",
            $"Przykłady poniżej pochodzą z tego samego kontraktu Practice co zadania ucznia. " +
            "Każdy przypadek jest najpierw generowany, następnie rozwiązywany przez silnik " +
            "matematyczny i niezależnie weryfikowany przed zbudowaniem treści Rich V2."
        };

        var concepts = new List<RichLessonKeyConcept>
        {
            new()
            {
                Title = "Główna idea",
                Definition = PolishMathematicsTextLocalizer.Localize(body.Explanation),
                Rule = PolishMathematicsTextLocalizer.Localize(body.KeyConceptsAndRules),
                Example = PolishMathematicsTextLocalizer.Localize(body.WorkedExamples)
            },
            new()
            {
                Title = "Dokładny cel lekcji",
                Definition =
                    $"Zweryfikowana umiejętność tej lekcji: " +
                    $"„{PolishMathematicsTextLocalizer.SkillLabel(contract.SkillId)}”.",
                Rule =
                    "Rozwiązuj dokładnie ten cel matematyczny; nie zastępuj go podobnym, " +
                    "ale innym typem zadania.",
                Example =
                    "Edulytics dopuszcza wyłącznie rodziny z zatwierdzonego kontraktu " +
                    "Practice, dla których działa solver i niezależny verifier."
            }
        };

        foreach (var label in familyLabels.Take(4))
        {
            concepts.Add(new RichLessonKeyConcept
            {
                Title = label,
                Definition =
                    $"To jedna ze zweryfikowanych postaci zadania w lekcji „{body.Title}”.",
                Rule =
                    "Najpierw rozpoznaj relację matematyczną, a dopiero potem wykonaj " +
                    "obliczenie lub przekształcenie.",
                Example =
                    "Pełny przykład liczbowy z rozwiązaniem znajduje się w sekcji " +
                    "przykładów poniżej."
            });
        }

        var firstFamily = familyLabels.FirstOrDefault() ?? "wymagany przypadek";
        var mistakes = new[]
        {
            new RichLessonCommonMistake
            {
                Mistake = PolishMathematicsTextLocalizer.Localize(body.CommonMistakes),
                WhyWrong =
                    "Taki błąd zmienia relację matematyczną albo stosuje regułę poza jej zakresem.",
                Correction =
                    "Wróć do dokładnego celu lekcji, wykonaj obliczenie ponownie i sprawdź " +
                    "wynik niezależną metodą."
            },
            new RichLessonCommonMistake
            {
                Mistake =
                    $"Traktowanie każdej postaci zadania tak samo, bez rozpoznania przypadku " +
                    $"takiego jak „{firstFamily}”.",
                WhyWrong =
                    "Różne postacie mogą wymagać innej reprezentacji, kolejności działań " +
                    "albo sposobu sprawdzenia.",
                Correction =
                    "Najpierw rozpoznaj typ relacji, potem wybierz odpowiadającą mu metodę."
            },
            new RichLessonCommonMistake
            {
                Mistake = "Zakończenie pracy na samym obliczeniu bez sprawdzenia odpowiedzi.",
                WhyWrong =
                    "Poprawnie wykonane działanie może nadal odpowiadać na inną wielkość, " +
                    "mieć złą jednostkę albo nie spełniać warunków zadania.",
                Correction =
                    "Sprawdź wynik przez podstawienie, działanie odwrotne, oszacowanie albo " +
                    "inną metodę właściwą dla danego typu zadania."
            }
        };

        var summary = new[]
        {
            PolishMathematicsTextLocalizer.Localize(body.QuickSummary),
            $"Zweryfikowana umiejętność: {PolishMathematicsTextLocalizer.SkillLabel(contract.SkillId)}.",
            $"Rozpoznawaj następujące postacie zadań: {JoinPolish(familyLabels)}.",
            "Porównuj metody rozwiązania, a nie tylko końcowe odpowiedzi.",
            "Zawsze sprawdzaj, czy wynik odpowiada dokładnie na pytanie i zachowuje " +
            "relację matematyczną z treści zadania."
        };

        var visuals = new[]
        {
            new RichLessonVisual
            {
                Kind = RichLessonVisualKind.EquationSet,
                Title = $"{body.Title} — zweryfikowane przykłady",
                Description =
                    "Zestaw kilku zweryfikowanych pytań i odpowiedzi z tej lekcji. " +
                    "Pełne rozumowanie znajduje się w przykładach z rozwiązaniem.",
                PrimaryLabel =
                    PolishMathematicsTextLocalizer.SkillLabel(contract.SkillId),
                Items = examples
                    .Take(4)
                    .Select(example =>
                        $"{example.Question}  →  {example.Answer}")
                    .ToList()
            }
        };

        var result = new RichLessonContentV2Lesson
        {
            LessonCode = lessonCode,
            CultureCode = body.CultureCode,
            Title = body.Title,
            ExplanationParagraphs = explanation.ToList(),
            KeyConcepts = concepts,
            WorkedExamples = examples.ToList(),
            CommonMistakes = mistakes.ToList(),
            SummaryPoints = summary.ToList(),
            Visuals = visuals.ToList(),
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

    private static RichLessonWorkedExample BuildPolishWorkedExample(
        ExactSkillGeneratedQuestion question,
        int order,
        LessonPracticeContract contract)
    {
        var familyLabel =
            PolishMathematicsTextLocalizer.FamilyLabel(question.Family);
        var prompt =
            PolishMathematicsTextLocalizer.Localize(question.Prompt);
        var solverExplanation =
            PolishMathematicsTextLocalizer.Localize(question.Solution);
        var answer =
            PolishMathematicsTextLocalizer.LocalizeAnswer(question.CorrectAnswer);

        return new RichLessonWorkedExample
        {
            Title = $"Przykład {order} — {familyLabel}",
            Question = prompt,
            Method =
                $"Zastosuj zweryfikowaną metodę „" +
                $"{PolishMathematicsTextLocalizer.SkillLabel(contract.SkillId)}” " +
                $"dla przypadku „{familyLabel}”.",
            Steps =
            [
                $"Przeczytaj zadanie: {prompt}",
                $"Rozpoznaj przypadek „{familyLabel}”.",
                solverExplanation,
                $"Podaj dokładną odpowiedź: {answer}.",
                "Sprawdź wynik względem pierwotnych danych i relacji. " +
                "Ten przykład został niezależnie zweryfikowany przez silnik matematyczny Edulytics."
            ],
            Answer = answer,
            Check =
                "Zweryfikowano przez dokładny solver i niezależny verifier " +
                $"dla rodziny „{question.Family}”."
        };
    }

    private static string JoinPolish(IReadOnlyList<string> values)
    {
        if (values.Count == 0)
            return "zweryfikowane przypadki lekcji";

        if (values.Count == 1)
            return values[0];

        if (values.Count == 2)
            return $"{values[0]} i {values[1]}";

        return $"{string.Join(", ", values.Take(values.Count - 1))} oraz {values[^1]}";
    }

    private static IReadOnlyList<string> BuildExplanation(
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> familyLabels)
    {
        var primary = Clean(body.Explanation);
        var concepts = Clean(body.KeyConceptsAndRules);
        var worked = Clean(body.WorkedExamples);

        return
        [
            primary,
            concepts,
            $"This lesson is not limited to one question shape. " +
            $"The verified lesson contract covers {JoinNatural(familyLabels)}. " +
            $"These are the main cases the learner should be able to recognize and solve.",
            $"A reliable way to study {body.Title} is to connect the mathematical idea " +
            $"to an actual question, carry out the required calculation or reasoning, " +
            $"and then verify the result. The worked examples below are generated from " +
            $"the same verified question families used by Edulytics Practice for this lesson. " +
            $"The existing lesson example is: {worked}"
        ];
    }

    private static IReadOnlyList<RichLessonKeyConcept> BuildConcepts(
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> familyLabels)
    {
        var result = new List<RichLessonKeyConcept>
        {
            new()
            {
                Title = "Core idea",
                Definition = Clean(body.Explanation),
                Rule = Clean(body.KeyConceptsAndRules),
                Example = Clean(body.WorkedExamples)
            },
            new()
            {
                Title = "Exact lesson target",
                Definition =
                    $"The verified curriculum-neutral skill for this lesson is " +
                    $"“{HumanizeSkill(contract.SkillId)}”.",
                Rule =
                    "Solve only the mathematical target represented by the lesson; " +
                    "do not replace it with a nearby but different skill.",
                Example =
                    $"Edulytics Practice routes this lesson through the " +
                    $"{contract.Mechanic} verified mechanic."
            }
        };

        foreach (var label in familyLabels.Take(4))
        {
            result.Add(new RichLessonKeyConcept
            {
                Title = label,
                Definition =
                    $"This is one verified question form within {body.Title}.",
                Rule =
                    $"Recognize when the question is asking for {label.ToLowerInvariant()} " +
                    "and apply the matching mathematical relationship before calculating.",
                Example =
                    "A complete numerical example for this case appears in the worked-example section below."
            });
        }

        return result;
    }

    private static RichLessonWorkedExample BuildWorkedExample(
        ExactSkillGeneratedQuestion question,
        int order,
        LessonPracticeContract contract)
    {
        var familyLabel = HumanizeFamily(question.Family);
        var solverExplanation = Clean(question.Solution);

        return new RichLessonWorkedExample
        {
            Title = $"Worked example {order} — {familyLabel}",
            Question = question.Prompt,
            Method =
                $"Use the verified {HumanizeSkill(contract.SkillId)} method for " +
                $"{familyLabel.ToLowerInvariant()}.",
            Steps =
            [
                $"Read the actual question: {question.Prompt}",
                $"Identify this as the “{familyLabel}” case of the lesson.",
                solverExplanation,
                $"State the exact answer: {question.CorrectAnswer}.",
                "Check the result against the original quantities and relationships. " +
                "Edulytics also independently verifies this generated example before it is shown."
            ],
            Answer = question.CorrectAnswer,
            Check =
                $"Verified by the exact Mathematics engine for family “{question.Family}”."
        };
    }

    private static IReadOnlyList<RichLessonCommonMistake> BuildMistakes(
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> familyLabels)
    {
        var firstFamily = familyLabels.FirstOrDefault() ?? "the required case";

        return
        [
            new()
            {
                Mistake = Clean(body.CommonMistakes),
                WhyWrong =
                    "This mistake changes or misuses the mathematical relationship described by the lesson.",
                Correction =
                    $"Return to the core rule for {body.Title}, then check the result against the original question."
            },
            new()
            {
                Mistake =
                    $"Treating every question as the same case instead of recognizing whether it is {JoinNatural(familyLabels)}.",
                WhyWrong =
                    "Different question forms can require a different representation, operation order, or comparison/check.",
                Correction =
                    $"First identify the case — for example {firstFamily.ToLowerInvariant()} — then apply the matching method."
            },
            new()
            {
                Mistake = "Stopping after a calculation without checking whether the answer fits the original problem.",
                WhyWrong =
                    "A calculation can be arithmetically neat but still answer the wrong quantity, use the wrong unit, or violate the relationship in the question.",
                Correction =
                    "Substitute, compare, estimate, or use the inverse/independent check that is appropriate to the topic before accepting the final answer."
            }
        ];
    }

    private static IReadOnlyList<string> BuildSummary(
        CanonicalLessonTranslationRecord body,
        LessonPracticeContract contract,
        IReadOnlyList<string> familyLabels) =>
        [
            Clean(body.QuickSummary),
            $"Core verified skill: {HumanizeSkill(contract.SkillId)}.",
            $"Be ready for these question forms: {JoinNatural(familyLabels)}.",
            "Use the worked examples to compare methods, not just final answers.",
            "Always check that the final result answers the original question and preserves the mathematical relationship."
        ];

    private static IReadOnlyList<RichLessonVisual> BuildVisuals(
        string title,
        IReadOnlyList<RichLessonWorkedExample> examples,
        LessonPracticeContract contract)
    {
        var equationItems = examples
            .Take(4)
            .Select(example =>
                $"{example.Question}  →  {example.Answer}")
            .ToList();

        return
        [
            new RichLessonVisual
            {
                Kind = RichLessonVisualKind.EquationSet,
                Title = $"{title} — question forms",
                Description =
                    "A compact view of several verified question-and-answer pairs from this lesson. " +
                    "Use the worked examples above for the full reasoning.",
                PrimaryLabel = HumanizeSkill(contract.SkillId),
                Items = equationItems
            }
        ];
    }

    private static int StableSeed(string value)
    {
        var hash = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        var seed = BitConverter.ToInt32(hash, 0);
        return seed == 0 ? 1 : seed;
    }

    private static string HumanizeSkill(string value) =>
        HumanizeTokens(value.Split('.', StringSplitOptions.RemoveEmptyEntries));

    private static string HumanizeFamily(string value)
    {
        var tokens = value
            .Split('.', StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length > 1 &&
            string.Equals(
                tokens[0],
                "supporting",
                StringComparison.OrdinalIgnoreCase))
        {
            tokens = tokens.Skip(1).ToArray();
        }

        // Question-family identifiers do not share a universal two-segment
        // namespace. Keep the mathematical concept in learner-facing labels:
        // ratio.unit_rate.direct -> "Ratio Unit Rate Direct",
        // vectors.magnitude.exact -> "Vectors Magnitude Exact".
        return HumanizeTokens(tokens);
    }

    private static string HumanizeTokens(IEnumerable<string> tokens)
    {
        var value = string.Join(
            " ",
            tokens.SelectMany(token =>
                token.Split(
                    ['_', '-'],
                    StringSplitOptions.RemoveEmptyEntries)));

        if (string.IsNullOrWhiteSpace(value))
            return "Verified mathematical case";

        return CultureInfo.InvariantCulture.TextInfo.ToTitleCase(
            value.ToLowerInvariant());
    }

    private static string JoinNatural(IReadOnlyList<string> values)
    {
        if (values.Count == 0)
            return "the verified lesson cases";

        if (values.Count == 1)
            return values[0];

        if (values.Count == 2)
            return $"{values[0]} and {values[1]}";

        return $"{string.Join(", ", values.Take(values.Count - 1))}, and {values[^1]}";
    }

    private static string Clean(string? value)
    {
        var normalized = Regex.Replace(
            value ?? string.Empty,
            @"\s+",
            " ").Trim();

        if (!string.IsNullOrWhiteSpace(normalized))
            return normalized;

        return "This lesson requires a reviewed mathematical explanation.";
    }

    private static string NormalizeCulture(string cultureCode)
    {
        if (string.IsNullOrWhiteSpace(cultureCode))
            return "en";

        var value = cultureCode.Trim();
        var separator = value.IndexOf('-');
        return (separator > 0 ? value[..separator] : value)
            .ToLowerInvariant();
    }
}
