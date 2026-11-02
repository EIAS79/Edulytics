using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>Original first-party randomized mathematics for Choral Count and
/// source unknown-addend methods, without tens composition or decomposition.</summary>
internal static class UsCcssG2Unit2Lesson2UnknownAddendQuestions
{
    public const string Count="usccss.g2.u02.l02.count_back_tens";
    public const string Add="usccss.g2.u02.l02.unknown_addend_add_on";
    public const string Take="usccss.g2.u02.l02.unknown_addend_take_away";
    public sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? family)=>family is Count or Add or Take;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not a G2 U02 L02 family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal);
        string prompt,solution;
        if(family==Count)
        {
            var start=r.Next(60,100);
            var steps=r.Next(1,Math.Min(7,start/10)+1);
            var end=start-10*steps;
            p["start"]=start;p["steps"]=steps;p["end"]=end;
            prompt=$"Choral count backward from {start} by TEN. After {steps} backward jumps of ten, what number do you reach?";
            solution=$"{start} − {steps} × 10 = {end}. The ones digit remains {start%10}.";
        }
        else
        {
            var firstTens=r.Next(1,6);
            var missingTens=r.Next(1,10-firstTens);
            var firstOnes=r.Next(0,9);
            var missingOnes=r.Next(0,10-firstOnes);
            var known=firstTens*10+firstOnes;
            var missing=missingTens*10+missingOnes;
            var total=known+missing;
            p["known"]=known;p["missing"]=missing;p["total"]=total;
            prompt=family==Add
                ? $"FIRST-PARTY missing-addend: {known} + ? = {total}. Count on by tens and ones to find the missing number without carrying."
                : $"FIRST-PARTY base-ten blocks: {known} + ? = {total}. Start with the TOTAL {total} and take away known {known}, without breaking a ten. What is missing?";
            solution=$"{known} + {missing} = {total} and {total} − {known} = {missing}. "+
                "Two complementary tools, same unknown addend.";
        }
        return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid Grade2 missing addend parameters");
        return (family==Count?p["end"]:p["missing"]).ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
    {
        if(!Supports(family))return false;
        if(family==Count)
        {
            return p.Count==3&&p.TryGetValue("start",out var start)&&start is >=60 and <100
                &&p.TryGetValue("steps",out var steps)&&steps>=1&&steps<=7
                &&p.TryGetValue("end",out var end)&&end>=0&&end==start-10*steps;
        }
        return p.Count==3&&p.TryGetValue("known",out var known)&&known>=10&&known<=99
            &&p.TryGetValue("missing",out var missing)&&missing>=10&&missing<=99
            &&p.TryGetValue("total",out var total)&&total>=20&&total<=99
            &&known+missing==total&&(known%10+missing%10)<10;
    }
}
