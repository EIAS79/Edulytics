using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// IM 2021 Grade5 U01 L07 digital subskill: Choose the cubic unit that matches the given measured lengths.
/// Actual image/physical construction/explanation: teacher-reviewed, never auto-passed.
/// </summary>
internal static class UsCcssG5Unit1Lesson7Questions
{
    public const string Family="usccss.g5.u01.l07.cubic_unit_context";
    private static readonly string[] SourceEquivalentPrompts =
    [
        "A freezer is 4 feet long, 2 feet wide and 3 feet tall. After requesting the missing width, which volume unit is correct? Enter 1 for cubic centimeters, 2 for cubic inches, or 3 for cubic feet. Teacher checks the information-gap questions.",
        "An Edulytiks-original model measures 2 inches by 3 inches by 4 inches. Select the volume unit: 1 cm cubed, 2 inches cubed, or 3 feet cubed. Teacher reviews the reason.",
        "An Edulytiks-original small block measures 2 centimeters by 3 centimeters by 5 centimeters. Choose 1 cm cubed, 2 inches cubed, or 3 feet cubed. Justify your unit selection to a teacher."
    ];
    private static readonly int[] IndependentAnswers = [3, 2, 1];
    private static string PromptFor(int variant)
    {
        if(variant<SourceEquivalentPrompts.Length)return SourceEquivalentPrompts[variant];
        var unit=variant%3 switch {0=>"centimeters",1=>"inches",_=>"feet"};
        return $"Edulytiks-original supplementary unit selection: a clearly measured storage box has sides {variant+1}, {variant+2} and 3 {unit}. Choose its correct volume unit: 1 cubic centimeters, 2 cubic inches, 3 cubic feet. Explain the context and chosen unit to the teacher.";
    }
    private static int AnswerFor(int variant) =>
        variant<IndependentAnswers.Length?IndependentAnswers[variant]:(variant%3)+1;
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);
    public static Problem Build(string family, Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported Grade5 source family");
        ArgumentNullException.ThrowIfNull(random);
        var variant=random.Next(12);
        var parameters=new Dictionary<string,int>(StringComparer.Ordinal) {["variant"]=variant};
        var solution="Original info-gap questioning, open size/context unit choices and physical cubic units are reviewed by teacher; no invented missing edge.";
        return new Problem(family,PromptFor(variant),solution,
            AssessmentItemType.Numeric,parameters);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> parameters)
    {
        if(!Valid(family,parameters))throw new InvalidOperationException("Invalid source question parameters");
        return AnswerFor(parameters["variant"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> parameters,string? answer) =>
        Valid(family,parameters)&&string.Equals(answer,Solve(family,parameters),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==1&&
        p.TryGetValue("variant",out var v)&&v>=0&&v<12;
}
