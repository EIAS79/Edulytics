using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Independent, original Edulytics optional Grade 2 U01 L06 center-day
/// mini-board. This lesson has no publisher Cool-down; this additional
/// reflection is an authored supplement, not misrepresented as source text.
/// Four digit cards are placed globally, never reused across the board.
/// </summary>
public static class UsCcssG2Unit1SixthLessonEdulyticsCenterCheck
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L06";
    public const string Version="edulytics-original-g2-u01-l06-center-check-v1";
    public static readonly IReadOnlyList<int> PuzzleDigitCards=[5,9,3,2,4];

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Reason);

    /// <summary>
    /// New four-equation task: 12 + a = 17, b + 7 = 16,
    /// 14 - c = 11, d + 6 = 8. Available cards 5,9,3,2,4.
    /// The arithmetic and global physical-card uniqueness are BOTH checked.
    /// </summary>
    public static Review CheckDigitBoard(string? first,string? second,
        string? third,string? fourth)
    {
        var raw=new[]{first,second,third,fourth};
        if(raw.Any(x=>!int.TryParse(x,NumberStyles.None,CultureInfo.InvariantCulture,out _)))
            return new(true,false,true,"Every blank needs one whole-number digit card.");
        var v=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var unique=v.Distinct().Count()==v.Length &&
            v.All(PuzzleDigitCards.Contains);
        var correct=v[0]==5 && v[1]==9 && v[2]==3 && v[3]==2;
        return new(true,unique && correct,true,
            unique && correct
                ?"All four independent equations are true and no physical digit card was reused."
                :"Use each card at most once across the entire gameboard and check all four equations.");
    }

    public static Review CheckWarmup(string? answer)
    {
        if(!int.TryParse(answer,NumberStyles.None,CultureInfo.InvariantCulture,out var n))
            return new(true,false,false,"Enter one numerical sum.");
        return new(true,n==36,false,n==36
            ?"Fourteen plus twenty-two makes thirty-six: add the tens and ones."
            :"Separate tens and ones in 14 + 22.");
    }

    public static readonly IReadOnlyList<string> SourceAlignedCenterChoices=
    [
        "What's Behind My Back? (earlier 10- and 20-cube rounds)",
        "How Close? (earlier near-20 and near-50 card stages)",
        "Number Puzzles (earlier within-10 or within-20 boards)"
    ];

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Student solves the real independent Number Puzzles board using one physical digit card at most once over the entire page.",
        "Student can explain which equation they solved first, why that was useful, and why each remaining equation stays true.",
        "Student makes a genuine choice from previously learned centers and works with a partner, documenting the strategy attempted.",
        "Teacher reviews student reflection, real center activity and classroom norms; correct warm-up arithmetic does not certify them."
    ];
}
