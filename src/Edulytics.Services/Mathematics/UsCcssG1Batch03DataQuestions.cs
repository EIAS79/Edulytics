using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party Grade 1 data-reading questions for IM 2021 Unit 1 L11-L13.
/// Reuses the exact numeric Practice UI. All three families share one
/// category-data generator and verify against validated source parameters.
/// </summary>
internal static class UsCcssG1Batch03DataQuestions
{
    public const string Statement="usccss.g1.u01.b03.check_data_statement";
    public const string Count="usccss.g1.u01.b03.count_category_data";
    public const string Answerable="usccss.g1.u01.b03.answerable_data_question";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    private static readonly string[][] Topics =
    [
        ["Cats","Dogs","Fish"],["Apples","Bananas","Oranges"],
        ["Red","Blue","Green"],["Squares","Triangles","Circles"],
        ["Monkeys","Lions","Parrots"],["Books","Blocks","Pencils"]
    ];

    public static bool Supports(string? family) =>
        family is Statement or Count or Answerable;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family)) throw new InvalidOperationException("Unknown G1 batch 03 family.");
        var a=random.Next(1,6);
        var b=random.Next(1,6);
        var c=random.Next(1,6);
        var topic=random.Next(Topics.Length);
        var index=random.Next(3);
        var kind=family==Count?random.Next(4):random.Next(2);
        var truth=random.Next(2);
        var labels=Topics[topic];
        var values=new[]{a,b,c};
        var summary="Read this fictional class data chart. "+
            string.Join("; ",Enumerable.Range(0,3).Select(i=>
                labels[i]+": "+new string('●',values[i])+" ("+values[i]+")"))+". ";
        var parameters=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["first"]=a,["second"]=b,["third"]=c,["topic"]=topic,
            ["index"]=index,["kind"]=kind,["truth"]=truth
        };
        string question,solution;
        if(family==Statement)
        {
            var claimed=truth==1?values[index]:values[index]+1;
            question=kind==0
                ? $"Statement: {claimed} children chose {labels[index]}. Is the statement true (1) or false (0)?"
                : $"Statement: There are {(truth==1?a+b+c:a+b+c+1)} responses in all. True (1) or false (0)?";
            solution="Check the actual labeled count, or add all three counts, before deciding.";
        }
        else if(family==Count)
        {
            question=kind==3?
                "How many children chose an option altogether? Enter the total.":
                $"How many children chose {labels[index]}? Enter that count.";
            solution="Read the labeled category or add each category once to obtain the total.";
        }
        else
        {
            question=kind==0?
                $"Can this chart answer 'How many chose {labels[index]}'? Enter 1 for yes, 0 for no.":
                "Can this chart tell us the first name of each child who voted? Enter 1 for yes, 0 for no.";
            solution="The chart shows category counts, not names or private information.";
        }
        return new Problem(family,summary+question,solution,AssessmentItemType.Numeric,parameters);
    }

    private static bool Valid(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family) || p.Count!=7 ||
            !p.TryGetValue("first",out var a) ||
            !p.TryGetValue("second",out var b) ||
            !p.TryGetValue("third",out var c) ||
            !p.TryGetValue("topic",out var topic) ||
            !p.TryGetValue("index",out var index) ||
            !p.TryGetValue("kind",out var kind) ||
            !p.TryGetValue("truth",out var truth))
            return false;
        return a is >=1 and <=5 && b is >=1 and <=5 && c is >=1 and <=5 &&
            topic>=0 && topic<Topics.Length && index is >=0 and <=2 &&
            kind is >=0 and <=3 && truth is >=0 and <=1 &&
            (family==Count || kind<=1);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p)) throw new InvalidOperationException("Invalid G1 batch 03 question parameters.");
        var answer=family switch
        {
            Statement => p["truth"],
            Count => p["kind"]==3?p["first"]+p["second"]+p["third"]:
                p["index"] switch{0=>p["first"],1=>p["second"],_=>p["third"]},
            Answerable => p["kind"]==0?1:0,
            _=>throw new InvalidOperationException("Unsupported G1 data question")
        };
        return answer.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer) =>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
}
