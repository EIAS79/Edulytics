using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact plot interpretations of an independent plant dataset; never invents Han's image.</summary>
internal static class UsCcssG2Unit3Lesson16PlantDataQuestions
{
    public const string Total="usccss.g2.u03.l16.plant_plot_total";
    public const string Mode="usccss.g2.u03.l16.plant_plot_mode";
    public const string Difference="usccss.g2.u03.l16.plant_plot_height_difference";
    public const string Above="usccss.g2.u03.l16.plant_plot_count_above";
    public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
    public static bool Supports(string? f)=>f is Total or Mode or Difference or Above;
    public static Problem Build(string family,Random r)
    {
        if(!Supports(family))throw new InvalidOperationException("Not Group B style measurement line plot");
        int first=r.Next(18,29),center=first+r.Next(2,5),last=center+r.Next(2,5);
        int countFirst=r.Next(1,4),countCenter=r.Next(4,7),countLast=r.Next(1,4);
        var p=new Dictionary<string,int>(StringComparer.Ordinal){
            ["first"]=first,["center"]=center,["last"]=last,
            ["countFirst"]=countFirst,["countCenter"]=countCenter,["countLast"]=countLast};
        var data=$"FIRST-PARTY plant heights: {first}cm has {countFirst} Xs, {center}cm has {countCenter} Xs, {last}cm has {countLast} Xs";
        var prompt=family switch{
            Total=>$"{data}. How many plants TOTAL?",
            Mode=>$"{data}. What measured height was MOST COMMON (cm)?",
            Difference=>$"{data}. Find the DIFFERENCE between tallest and shortest plants (cm).",
            _=>$"{data}. How many plants had heights GREATER than {center}cm?"
        };
        int value=family switch{Total=>countFirst+countCenter+countLast,Mode=>center,Difference=>last-first,_=>countLast};
        return new Problem(family,prompt,$"Each X is one plant, and length labels are true centimeters. Answer {value}. Not Han's authentic plot.",AssessmentItemType.Numeric,p);
    }
    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid plant-height plot parameters");
        int n=family switch{Total=>p["countFirst"]+p["countCenter"]+p["countLast"],Mode=>p["center"],
            Difference=>p["last"]-p["first"],Above=>p["countLast"],_=>throw new InvalidOperationException("Invalid family")};
        return n.ToString(CultureInfo.InvariantCulture);
    }
    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
        Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
    private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
        Supports(f)&&p.Count==6&&p.TryGetValue("first",out var first)&&first is >=18 and <=28&&
        p.TryGetValue("center",out var c)&&c>=first+2&&c<=first+4&&
        p.TryGetValue("last",out var last)&&last>=c+2&&last<=c+4&&last<=36&&
        p.TryGetValue("countFirst",out var a)&&a is >=1 and <=3&&
        p.TryGetValue("countCenter",out var b)&&b is >=4 and <=6&&
        p.TryGetValue("countLast",out var d)&&d is >=1 and <=3;
}
