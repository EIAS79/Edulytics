using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 G5 U03:L04 original city park + Elena colored park.
/// Exact bounded digital subskill only; original student drawings, partner
/// interpretations and teacher Cool-down remain TEACHER_REVIEW_REQUIRED.
/// Reuses the existing ExactSkillContractQuestionEngine. Authored tests NOT_RUN.
/// </summary>
public static class UsCcssG5U03L04ParkQuestions
{
    public const string NestedFamily =
        "usccss.g5.u03.l04.park_nested_original_whole_fraction";
    public const string RegionFamily =
        "usccss.g5.u03.l04.elena_park_expression_region";
    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string, int> Parameters);
    private static readonly (int Part, int Whole, int Portion, int Parts, string Label)[] Cases =
    [
        (2, 3, 1, 2, "soccer fields"), // 1/2 of 2/3 = 1/3
        (1, 2, 1, 5, "swings"),       // 1/5 of 1/2 = 1/10
        (1, 2, 3, 5, "grass"),        // 3/5 of 1/2 = 3/10
        (1, 2, 4, 5, "pond"),         // 4/5 of 1/2 = 2/5
        (1, 2, 2, 5, "basketball")    // 2/5 of 1/2 = 1/5
    ];
    public static bool Supports(string? family) => family is NestedFamily or RegionFamily;
    public static Problem Build(string family, Random random)
    {
        if (!Supports(family)) throw new InvalidOperationException("Not Grade 5 U03 L04.");
        ArgumentNullException.ThrowIfNull(random);
        int caseNumber = family == RegionFamily ? random.Next(1, 5) : random.Next(5);
        IReadOnlyDictionary<string, int> p = new Dictionary<string, int>(StringComparer.Ordinal)
        { ["case"] = caseNumber, ["slot"] = random.Next(3) };
        if (!TryGet(family, p, out var example))
            throw new InvalidOperationException("Invalid source park choice.");
        string correct, wrongA, wrongB, prompt, explain;
        if (family == NestedFamily)
        {
            correct = Simplify(example.Part * example.Portion, example.Whole * example.Parts);
            wrongA = Simplify(example.Part, example.Whole);
            wrongB = Simplify(example.Portion, example.Parts);
            prompt = caseNumber == 0
                ? "A city uses 2/3 of the ORIGINAL park for sports; 1/2 of the sports land is for soccer. What FRACTION OF THE WHOLE PARK is soccer? "
                : $"In Elena's park, {example.Label} occupies {example.Portion}/{example.Parts} of the selected {example.Part}/{example.Whole} half of the ORIGINAL park. What fraction OF THE WHOLE park is it? ";
            explain = $"Take {example.Portion}/{example.Parts} of {example.Part}/{example.Whole} of the ORIGINAL whole = {correct}. " +
                "The first fraction is a selected part of the original park, not a new whole.";
        }
        else
        {
            correct = example.Label;
            wrongA = Cases[(caseNumber % 4) + 1].Label;
            wrongB = Cases[(caseNumber + 1) % 4 + 1].Label;
            if (wrongA == correct) wrongA = "soccer fields";
            if (wrongB == correct || wrongB == wrongA) wrongB = "soccer fields";
            prompt = $"Elena's source park is a rectangle of 5 rows by 2 equal cells. Exactly {example.Portion} of the 10 cells are {example.Label}. " +
                $"Which labeled region does ({example.Portion}/5) x (1/2) describe? ";
            explain = $"The region {correct} occupies {example.Portion} out of 10 equal cells in Elena's original park. ";
        }
        string[] options = p["slot"] switch
        {
            0 => [correct, wrongA, wrongB],
            1 => [wrongA, correct, wrongB],
            _ => [wrongA, wrongB, correct]
        };
        prompt += $"Select 1: {options[0]}; 2: {options[1]}; 3: {options[2]}. " +
            "The learner's actual park drawing, source-image placement and partner explanation require TEACHER_REVIEW_REQUIRED.";
        explain += "An exact selected choice is not evidence of correct freehand diagrams or written reasoning.";
        return new Problem(family,prompt,explain,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family, IReadOnlyDictionary<string, int> parameters)
    {
        if (!TryGet(family, parameters, out _))
            throw new InvalidOperationException("Malformed original park parameters.");
        return (parameters["slot"] + 1).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family, IReadOnlyDictionary<string, int>? parameters, string? answer) =>
        TryGet(family, parameters, out _) &&
        string.Equals(answer, (parameters!["slot"] + 1).ToString(CultureInfo.InvariantCulture),
            StringComparison.Ordinal);
    private static bool TryGet(string? family, IReadOnlyDictionary<string, int>? p,
        out (int Part,int Whole,int Portion,int Parts,string Label) found)
    {
        found = default;
        if (!Supports(family) || p is null || p.Count != 2 ||
            !p.TryGetValue("case", out int item) || item is < 0 or > 4 ||
            !p.TryGetValue("slot", out int slot) || slot is < 0 or > 2 ||
            (family == RegionFamily && item == 0))
            return false;
        found = Cases[item];
        return true;
    }
    private static string Simplify(int numerator, int denominator)
    {
        int a = numerator, b = denominator;
        while (b != 0) { int t = a % b; a = b; b = t; }
        return $"{numerator / a}/{denominator / a}";
    }
}
