using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 Unit 1 Lesson 8. Map route polyline (12+5 grid units) differs from
/// direct endpoint distance (13 grid units); link both to map scale, minutes
/// and mph, without accidentally treating grid points as real-world miles.
/// No publisher map artwork or coordinates are imported.
/// </summary>
internal static class UsCcssG7Unit1Lesson8MapRateQuestions
{
    public const string Family="usccss.g7.u01.l08.map_route_rate_time";
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    // Map: A(0,0), turning point P(12,0), B(12,5).
    // Road distance 17 grid units; direct distance sqrt(12^2+5^2)=13.
    private static readonly decimal[] MilesPerGridUnit=[0.5m,1m,2m];
    private static readonly int[] TripMinutes=[8,10,12,15];

    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unknown G7 L08 family");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["scale"]=random.Next(0,3),
            ["minutes"]=random.Next(0,4),
            ["mode"]=random.Next(0,4)
        };
        decimal mile= MilesPerGridUnit[p["scale"]];
        int minutes=TripMinutes[p["minutes"]];
        string map="A first-party schematic map shows a road from " +
            "A=(0,0) to P=(12,0) then B=(12,5). A helicopter flies " +
            "directly from A to B. One grid unit represents "+
            Fmt(mile)+" miles. The road is a polyline, not a straight line. ";
        string prompt=p["mode"] switch
        {
            0 => map+"A driver travels the road in "+minutes+
                 " minutes at constant speed. What is the driver's speed in MILES PER HOUR?",
            1 => map+"The helicopter travels A to B directly in "+minutes+
                 " minutes. What is the helicopter's speed in MILES PER HOUR?",
            2 => map+"The driver finishes the road trip in "+minutes+
                 " minutes. The speed limit is 55 miles per hour. Enter 1 if " +
                 "she was within that limit, or 0 if she exceeded it.",
            _ => "A scaled map uses a bar representing 4 miles. A bike route " +
                 "contains 12.5 such bar lengths. At a constant cycling speed " +
                 "of 15 miles per hour, how many MINUTES does the trip take?"
        };
        return new(Family,prompt,
            "Road: 12+5=17 grid units; air straight-line: " +
            "sqrt(12 squared + 5 squared)=13. Multiply by map " +
            "miles/grid unit; speed (mph)=miles/minutes*60. " +
            "For the bicycle, 12.5*4=50 miles and 50/15*60=200 minutes. " +
            "Route drawing and map measurement estimates require teacher review.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid L08 parameters.");
        if(p["mode"]==3)return "200";
        decimal miles=(p["mode"]==1 ? 13m : 17m)*MilesPerGridUnit[p["scale"]];
        decimal speed=miles*60m/TripMinutes[p["minutes"]];
        return p["mode"]==2 ? (speed<=55m ? "1":"0") : Fmt(speed);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        string value=answer.Trim();
        if(p["mode"]==2)
            return (value=="0"||value=="1")&&
                string.Equals(value,Solve(family,p),StringComparison.Ordinal);
        decimal expected=p["mode"]==3 ? 200m :
            ((p["mode"]==1 ? 13m:17m)*MilesPerGridUnit[p["scale"]]*60m /
             TripMinutes[p["minutes"]]);
        var split=value.Split('/');
        decimal given;
        if(split.Length==2)
        {
            if(!decimal.TryParse(split[0],NumberStyles.None,
                    CultureInfo.InvariantCulture,out var n) ||
               !decimal.TryParse(split[1],NumberStyles.None,
                    CultureInfo.InvariantCulture,out var d) || d<=0m)
                return false;
            given=n/d;
        }
        else if(split.Length==1)
        {
            if(!decimal.TryParse(value,NumberStyles.AllowDecimalPoint,
                    CultureInfo.InvariantCulture,out given))
                return false;
        }
        else return false;
        return given==expected;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==3&&
        p.TryGetValue("scale",out int s)&&s is >=0 and <=2 &&
        p.TryGetValue("minutes",out int t)&&t is >=0 and <=3 &&
        p.TryGetValue("mode",out int m)&&m is >=0 and <=3;

    private static string Fmt(decimal n)=>n.ToString(CultureInfo.InvariantCulture);
}
