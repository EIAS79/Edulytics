using System.Globalization;
using System.Text.RegularExpressions;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// First-party Compare story/tape/equation card sort in unknown smaller,
/// unknown larger, and unknown difference positions. Original IM source
/// shell quantities preserved; new remaining card stories independently
/// authored. Mathematically equivalent equation cards are accepted.
/// </summary>
public static class UsCcssG2Unit1FifteenthLessonCompareCards
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L15";
    public const string Version="edulytics-g2-u01-l15-three-unknown-positions-v1";

    public sealed record CompareCard(
        string Id,string Story,string DiagramKey,string UnknownPosition,
        string LargerName,string SmallerName,int Larger,int Smaller,int Difference)
    {
        public int Answer=>UnknownPosition switch
        {
            "Larger"=>Larger,
            "Smaller"=>Smaller,
            "Difference"=>Difference,
            _=>throw new InvalidOperationException("Not a valid missing part")
        };
    }
    public static readonly IReadOnlyList<CompareCard> Cards=
    [
        new("A","Diego has 55 shells. Lin has 20 FEWER. How many shells has Lin?",
            "TapeSmaller","Smaller","Diego","Lin",55,35,20),
        new("B","Tala has 34 seashells. Rami has 20 MORE. How many shells has Rami?",
            "TapeLarger","Larger","Rami","Tala",54,34,20),
        new("C","Nora collected 62 pebbles; Omar collected 50. How many MORE has Nora?",
            "TapeDifference","Difference","Nora","Omar",62,50,12)
    ];

    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Feedback);

    private static readonly Regex Expression=new(
        @"^\s*(\d{1,3})\s*([+\-−])\s*(\d{1,3})\s*=\s*(\d{1,3})\s*$",
        RegexOptions.Compiled|RegexOptions.CultureInvariant);

    public static Review CheckCard(string? cardId,string? diagram,string? unknown,
        string? answer,string? equationOne,string? equationTwo)
    {
        var card=Cards.SingleOrDefault(x=>x.Id==cardId);
        if(card is null)
            return new(true,false,true,"Select an original Edulytics compare story card.");
        var numeric=int.TryParse(answer,NumberStyles.None,
            CultureInfo.InvariantCulture,out var a) && a==card.Answer;
        var correct=diagram==card.DiagramKey && unknown==card.UnknownPosition &&
            numeric && IsMatchingEquation(card,equationOne) &&
            IsMatchingEquation(card,equationTwo) &&
            !string.Equals(equationOne?.Trim(),equationTwo?.Trim(),
                StringComparison.Ordinal);
        return new(true,correct,true,correct
            ?"The unknown is placed on the correct tape and two different equivalent equations match the story. Teacher reviews story-to-diagram reasoning and actual partner exchange."
            :"Match the longer/shorter tape labels, put ? on the appropriate larger/smaller/gap part and supply TWO different mathematically equivalent equations.");
    }

    public static bool IsMatchingEquation(CompareCard card,string? expression)
    {
        var match=Expression.Match(expression??"");
        if(!match.Success)return false;
        var x=int.Parse(match.Groups[1].Value,CultureInfo.InvariantCulture);
        var y=int.Parse(match.Groups[3].Value,CultureInfo.InvariantCulture);
        var z=int.Parse(match.Groups[4].Value,CultureInfo.InvariantCulture);
        if(match.Groups[2].Value=="+")
            return x+y==z && z==card.Larger &&
                ((x==card.Smaller&&y==card.Difference)||
                 (x==card.Difference&&y==card.Smaller));
        if(x-y!=z)return false;
        return x==card.Larger &&
            ((y==card.Smaller&&z==card.Difference) ||
             (y==card.Difference&&z==card.Smaller));
    }

    public static Review CheckNumberTalk(
        string? first,string? second,string? third,string? fourth)
    {
        var raw=new[]{first,second,third,fourth};
        if(raw.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,true,"Enter all four positive sums.");
        var actual=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var good=actual.SequenceEqual(new[]{14,14,13,14});
        return new(true,good,true,good
            ?"4+8+2=14, 4+5+3+2=14, 9+3+1=13 and 9+5=14. Explain how you made ten."
            :"Try making ten by combining 8+2 or 9+1 before adding the remaining number.");
    }

    public static Review CheckIndependentCoolDown(
        string? smallerAnswer,string? largerAnswer,string? differenceAnswer)
    {
        var raw=new[]{smallerAnswer,largerAnswer,differenceAnswer};
        if(raw.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,false,"Enter each unknown as a whole number.");
        var values=raw.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var correct=values.SequenceEqual(new[]{37,76,14});
        return new(true,correct,false,correct
            ?"Mira67/Sami30 fewer gives37. Arin46/Sana30 more gives76. Lina84/Omar70 gives gap14."
            :"First decide whether the question asks for the smaller amount, larger amount or difference; then use the tape.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Learner independently labels Diego55, Lin?, dashed difference20 on an aligned original tape; correctly recognizes unknown SMALLER Lin35.",
        "Partner sorts three Edulytics-authored Compare story cards into matching independently drawn TAPE diagram keys and TWO different equivalent equation cards.",
        "Learner discriminates unknown LARGER, SMALLER and DIFFERENCE positions without choosing subtraction solely from 'fewer' or addition solely from 'more'.",
        "Multiple valid equivalent equations are accepted. Teacher checks written justification that every card's labels, quantities and question mark match the story.",
        "Teacher reviews genuine learner drawing, accessible manipulative/cube alternatives, partner discussion and Number Talk strategy; numerical checks never certify those activities."
    ];
}
