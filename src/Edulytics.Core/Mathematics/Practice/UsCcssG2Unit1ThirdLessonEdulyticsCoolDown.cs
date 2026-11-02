using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original, self-authored Edulytics Grade 2 U01 L03 exit ticket:
/// missing addends with a whole of twenty, 20-cube visual parts, truth of
/// equations, and optional unknown-minuend extension. It does not reproduce
/// the publisher's educator-only Cool-down.
/// </summary>
public static class UsCcssG2Unit1ThirdLessonEdulyticsCoolDown
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L03";
    public const string Version="edulytics-original-g2-u01-l03-cooldown-v1";

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Message);

    public static Review VerifyHiddenPart(string? answer)
        => CheckNumeric(answer,13,"20 cubes are the whole; 7 visible and 13 hidden.");

    public static Review VerifyRemovedPart(string? answer)
        => CheckNumeric(answer,11,"Twenty minus eleven leaves nine.");

    public static Review VerifyChallengeStartingAmount(string? answer)
        => CheckNumeric(answer,25,"Twenty-five minus five equals twenty. This optional extension starts above twenty.");

    public static Review VerifyEquationsTruth(string? first,string? second)
    {
        // These independent, authored statements are deliberately not copied
        // from the original printable paper. A student must distinguish truth.
        var correct=first?.Trim()=="1" && second?.Trim()=="0";
        return new(true,correct,false,correct
            ?"20 − 8 = 12 is true; 20 − 8 = 13 is false."
            :"Enter 1 for a true equation and 0 for a false equation.");
    }

    private static Review CheckNumeric(string? answer,int expected,string success)
    {
        if(!int.TryParse(answer,NumberStyles.None,
                CultureInfo.InvariantCulture,out var value))
            return new(true,false,false,"Enter one non-negative whole number.");
        return new(true,value==expected,false,value==expected
            ?success:"Use the whole/part relationship and check your arithmetic.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric =
    [
        "Learner represents exactly twenty cubes as two groups of ten, and clearly identifies known and hidden parts.",
        "Learner writes an unknown-addend equation before solving and relates the same parts by subtraction.",
        "Learner explains a make-ten or other correct strategy for the unknown, and explains the optional greater-than-twenty starting value when attempted.",
        "Partner roles, student diagrams and Math Community norms are teacher-reviewed; do not claim an automatically scored integer proves classroom performance."
    ];
}
