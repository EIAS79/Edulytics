using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Independently gradeable subskills from IM 2021 Grade 4 U01 L01.
/// This does NOT grade tile construction, paper drawings, warm-up explanations,
/// partner talk, or the original protected publisher Cool-down.
/// </summary>
internal static class UsCcssG4Unit1FirstLessonQuestions
{
    public const string FixedArea = "usccss.g4.u01.l01.fixed_width_area";
    public const string MultipleDecision = "usccss.g4.u01.l01.area_is_multiple";
    public const string MissingLength = "usccss.g4.u01.l01.missing_whole_length";
    public const string AboveThirty = "usccss.g4.u01.l01.possible_area_above30";

    public sealed record Problem(string Family, string Prompt, string Solution,
        AssessmentItemType ItemType, IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        family is FixedArea or MultipleDecision or MissingLength or AboveThirty;

    public static Problem Build(string family, Random random)
    {
        if (!Supports(family))
            throw new InvalidOperationException("Unsupported G4 U01 L01 source family");

        var p = new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt;
        string method;

        if (family == FixedArea)
        {
            var width = random.Next(2,5); // source: 2, 3, 4 tile widths
            var length = random.Next(1,11);
            p["width"]=width;
            p["length"]=length;
            prompt=$"You build a rectangle {width} tiles wide and {length} tiles long. " +
                "What is the area, in square tiles? Enter the number.";
            method=$"Multiply width by length: {width} × {length} = {width*length} square tiles.";
        }
        else if (family == MissingLength)
        {
            var length=random.Next(1,11);
            p["width"]=3;
            p["area"]=3*length;
            prompt=$"Elena's rectangle is 3 units wide with an area of {3*length} " +
                "square units. How many whole units long is it?";
            method=$"Divide {3*length} square units by width 3: length {length} units.";
        }
        else if (family == MultipleDecision)
        {
            var area=random.Next(1,31);
            p["width"]=3;
            p["area"]=area;
            prompt=$"Elena is limited to 30 square units and has width 3. " +
                $"Can she construct a whole-number-sided rectangle with area {area} square units? " +
                "Enter 1 for yes or 0 for no.";
            method="A possible area is a positive multiple of the width 3.";
        }
        else
        {
            var area=random.Next(31,49);
            p["width"]=3;
            p["area"]=area;
            prompt=$"Elena removes the 30-square-unit limit. Can a width-3, whole-number-sided " +
                $"rectangle have area {area} square units? Enter 1 for yes or 0 for no.";
            method="For area above 30, whole-number rectangles of width 3 still have areas divisible by 3.";
        }

        return new Problem(family,prompt,method,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family, IReadOnlyDictionary<string,int> p)
    {
        if (!Valid(family,p))
            throw new InvalidOperationException("Invalid G4 U01 L01 source task parameters");

        var result = family switch
        {
            FixedArea => p["width"] * p["length"],
            MissingLength => p["area"] / 3,
            MultipleDecision or AboveThirty => p["area"] % 3 == 0 ? 1 : 0,
            _ => throw new InvalidOperationException("Unknown G4 L01 question")
        };
        return result.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family, IReadOnlyDictionary<string,int> p, string answer) =>
        Valid(family,p) &&
        string.Equals(Solve(family,p),answer,StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if (!Supports(family)) return false;
        if (family == FixedArea)
            return p.Count==2 && p.TryGetValue("width",out var width) &&
                width is >=2 and <=4 && p.TryGetValue("length",out var length) &&
                length is >=1 and <=10;
        if (p.Count!=2 || !p.TryGetValue("width",out var w) || w!=3 ||
            !p.TryGetValue("area",out var area)) return false;
        return family switch
        {
            MissingLength => area is >=3 and <=30 && area%3==0,
            MultipleDecision => area is >=1 and <=30,
            AboveThirty => area is >=31 and <=48,
            _ => false
        };
    }
}
