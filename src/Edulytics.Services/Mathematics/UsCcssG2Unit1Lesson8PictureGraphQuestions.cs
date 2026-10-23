using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// First-party picture-symbol numerical analogues, not publisher figures,
/// for IM 2021 G2 Unit 1 Lesson 8. One symbol equals one observation.
/// </summary>
internal static class UsCcssG2Unit1Lesson8PictureGraphQuestions
{
    public const string Count="usccss.g2.u01.l08.read_pictograph_category";
    public const string Combined="usccss.g2.u01.l08.combine_or_compare_categories";
    public const string CanAnswer="usccss.g2.u01.l08.graph_question_answerable";

    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family)=>family is Count or Combined or CanAnswer;

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G2 U01 L08 picture graph family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["carrots"]=random.Next(0,10),
            ["peas"]=random.Next(0,10),
            ["corn"]=random.Next(0,10)
        };
        string prompt="Picture graph — vegetables children chose today. " +
            "Key: every ● is ONE child. " +
            "Carrots: "+new string('●',p["carrots"])+" ("+p["carrots"]+" symbols); "+
            "Peas: "+new string('●',p["peas"])+" ("+p["peas"]+" symbols); "+
            "Corn: "+new string('●',p["corn"])+" ("+p["corn"]+" symbols). ";
        string solution;
        if(family==Count)
        {
            var category=random.Next(3);
            p["category"]=category;
            prompt+="How many children chose "+
                new[]{"carrots","peas","corn"}[category]+"? Enter the count.";
            solution="Read the key and count symbols in ONLY the named category.";
        }
        else if(family==Combined)
        {
            var op=random.Next(2);
            p["operation"]=op;
            prompt+=op==0
                ? "How many chose carrots or peas altogether? Enter the total."
                : "How many MORE chose carrots than peas, or peas than carrots? Enter the absolute difference.";
            solution="Combine the category counts for a total, or subtract the " +
                "smaller count from the larger count to find how many more.";
        }
        else
        {
            var ask=random.Next(3);
            p["question"]=ask;
            prompt+=ask switch
            {
                0 => "Can this picture graph answer 'How many children chose peas today?'",
                1 => "Can this picture graph answer 'How many chose peas and carrots altogether today?'",
                _ => "Can this picture graph answer 'How many adults chose apples last year?'"
            };
            prompt+=" Enter 1 for YES or 0 for NO.";
            solution="Only questions answerable from shown categories and today's " +
                "recorded population are supported. Do not invent missing data.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid G2 U01 L08 source graph parameters");
        var n=family switch
        {
            Count=>p["category"] switch {0=>p["carrots"],1=>p["peas"],_=>p["corn"]},
            Combined=>p["operation"]==0
                ?p["carrots"]+p["peas"]
                :Math.Abs(p["carrots"]-p["peas"]),
            CanAnswer=>p["question"]==2?0:1,
            _=>throw new InvalidOperationException("Unknown G2 family")
        };
        return n.ToString(CultureInfo.InvariantCulture);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p) && string.Equals(answer,Solve(family,p),StringComparison.Ordinal);

    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family) ||
            !p.TryGetValue("carrots",out var a) || a is <0 or >9 ||
            !p.TryGetValue("peas",out var b) || b is <0 or >9 ||
            !p.TryGetValue("corn",out var c) || c is <0 or >9 ||
            p.Count!=4) return false;
        var key=family==Count?"category":family==Combined?"operation":"question";
        return p.TryGetValue(key,out var value) &&
            value>=0 && value<=(family==Count || family==CanAnswer?2:1);
    }
}
