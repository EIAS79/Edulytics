using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>
/// Original Edulytics Grade2 U01 L12 optional Sort and Display Stage 2.
/// Checks mathematics of THREE physically sorted categories totaling
/// 20–30 actual items without pretending to observe physical classroom
/// artifacts. Student-authored questions and optional centers are rubric scored.
/// </summary>
public static class UsCcssG2Unit1TwelfthLessonSortAndDisplay
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U01:L12";
    public const string Version="edulytics-original-g2-u01-l12-sort-20-30-centers-v1";
    public sealed record Review(bool MachineScorable,bool Correct,
        bool TeacherReviewNeeded,string Feedback);
    public sealed record Collection(
        string CategoryA,int CountA,string CategoryB,int CountB,
        string CategoryC,int CountC,int Total);
    public sealed record CollectionResult(Review Review,Collection? Data);

    public static readonly IReadOnlyList<string> CenterChoices=
    [
        "What's Behind My Back?",
        "How Close?",
        "Number Puzzles"
    ];

    public static CollectionResult CheckRealCollection(
        string? categoryA,string? categoryB,string? categoryC,
        string? countA,string? countB,string? countC)
    {
        var labels=new[]{categoryA?.Trim(),categoryB?.Trim(),categoryC?.Trim()};
        if(labels.Any(x=>string.IsNullOrWhiteSpace(x) || x.Length>40) ||
            labels.Distinct(StringComparer.OrdinalIgnoreCase).Count()!=3)
            return new(new(true,false,true,
                "Name exactly THREE different meaningful observed categories, each with a short name."),null);

        var counts=new[]{countA,countB,countC};
        if(counts.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(new(true,false,true,"Enter three non-negative counts of REAL objects."),null);
        var values=counts.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var total=values.Sum();
        if(values.Any(x=>x<1)||total is <20 or >30)
            return new(new(true,false,true,
                "Original Sort and Display Stage 2 requires 20 to 30 actual objects, sorted into 3 NONEMPTY categories."),null);

        return new(new(true,true,true,
            "Counts make a valid three-category collection of 20–30 objects. Teacher still verifies actual objects, exclusive sorting and representations."),
            new(labels[0]!,values[0],labels[1]!,values[1],labels[2]!,values[2],total));
    }

    public static Review CheckRepresentation(
        Collection? collection,string? title,string? kind,
        string? pictureKey,string? barStart,string? barStep)
    {
        if(collection is null)return new(false,false,true,"Enter a real collection first.");
        if(string.IsNullOrWhiteSpace(title)||title.Trim().Length is <4 or >120)
            return new(true,false,true,"Give your original data display a meaningful title.");
        var valid=kind switch
        {
            "Picture"=>pictureKey?.Trim()=="1",
            "Bar"=>barStart?.Trim()=="0"&&barStep?.Trim()=="1",
            _=>false
        };
        return new(true,valid,true,valid
            ?"Correct chart type and key/scale. Teacher checks the actually drawn symbol/rectangle display."
            :"Choose PICTURE with one-object-per-symbol key, or BAR with numbered scale from zero in steps of one.");
    }

    public static Review CheckTwoOriginalQuestions(
        Collection? c,string? questionA,string? questionB,
        string? firstAnswer,string? secondAnswer)
    {
        if(c is null)return new(false,false,true,"No valid source collection to ask about.");
        if(string.IsNullOrWhiteSpace(questionA)||questionA.Length<8||
            string.IsNullOrWhiteSpace(questionB)||questionB.Length<8||
            string.Equals(questionA.Trim(),questionB.Trim(),StringComparison.OrdinalIgnoreCase))
            return new(true,false,true,"Write two different questions that the actual three-category graph can answer.");
        if(!int.TryParse(firstAnswer,NumberStyles.None,CultureInfo.InvariantCulture,out var first)||
            !int.TryParse(secondAnswer,NumberStyles.None,CultureInfo.InvariantCulture,out var second))
            return new(true,false,true,"Enter partner's two whole-number graph answers.");
        var correct=first==c.CountA && second==c.CountB+c.CountC;
        return new(true,correct,true,correct
            ?"Proposed numerical answers agree with the original category counts, pending teacher review of student question wording and partner exchange."
            :"For this independent activity the first question targets Category A count, and the second combines B and C; inspect your true graph.");
    }

    public static Review CheckNumberTalk(
        string? seventeenMinusSeven,string? seventeenMinusNine,
        string? fifteenMinusFive,string? fifteenMinusEight)
    {
        var values=new[]{seventeenMinusSeven,seventeenMinusNine,
            fifteenMinusFive,fifteenMinusEight};
        if(values.Any(x=>!int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out _)))
            return new(true,false,false,"Enter four whole-number mental differences.");
        var parsed=values.Select(x=>int.Parse(x!,CultureInfo.InvariantCulture)).ToArray();
        var right=parsed.SequenceEqual(new[]{10,8,10,7});
        return new(true,right,false,right
            ?"The 17 and 15 differences use a ten, with two more or three more removed."
            :"Try 17−7=10 then subtract two more; 15−5=10 then subtract three more.");
    }

    public static readonly IReadOnlyList<string> TeacherRubric=
    [
        "Partner physically sorts a collection of between 20 and 30 REAL objects into exactly 3 disjoint nonempty categories and checks all objects were counted once.",
        "Learner constructs an independently titled picture OR bar graph with category labels, true counts and accurate key or numbered axis; teacher assesses actual symbols or bars.",
        "Student authors two answerable graph questions and a partner genuinely answers them; checker grades selected count and combined numerical subskills only.",
        "Student describes how 17−7 helps with 17−9 and how 15−5 helps with 15−8, or another valid make-ten strategy.",
        "Student chooses a previously introduced arithmetic center and explains partner activity; optional physical choice and Math Community norms are teacher observed."
    ];
}
