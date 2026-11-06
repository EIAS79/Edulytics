using System.Globalization;
using Edulytics.Core.Enums;

namespace Edulytics.Services.Mathematics;

/// <summary>
/// Grade 7 U01:L07 scale drawings: drawing-centimetres mapped to actual
/// metres. This is NOT the centimetres-to-metres physical unit conversion.
/// Drawing measurements and scale-bar ruler work remain teacher-reviewed.
/// </summary>
internal static class UsCcssG7Unit1Lesson7DrawingScaleQuestions
{
    public const string Family="usccss.g7.u01.l07.drawing_actual_distance_units";
    private static readonly int[] ActualMetersPerDrawingCm=[2,5,10,20];
    internal sealed record Problem(string Family,string Prompt,string Solution,
        AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);

    public static bool Supports(string? family) =>
        string.Equals(family,Family,StringComparison.Ordinal);

    public static Problem Build(string family,Random random)
    {
        if(!Supports(family))throw new InvalidOperationException("Unsupported L07 source family.");
        var p=new Dictionary<string,int>(StringComparer.Ordinal)
        {
            ["scale"]=random.Next(0,ActualMetersPerDrawingCm.Length),
            ["drawingTwice"]=random.Next(1,41),
            ["direction"]=random.Next(0,2)
        };
        int metersPerCm=ActualMetersPerDrawingCm[p["scale"]];
        decimal cm=p["drawingTwice"]/2m,real=cm*metersPerCm;
        string phrase=p["direction"]==0
            ? "The figure measures "+Fmt(cm)+" cm on the DRAWING. " +
              "What is the corresponding ACTUAL length in metres?"
            : "The actual object is "+Fmt(real)+" m long. " +
              "How many centimetres long should it be ON THE DRAWING?";
        string prompt="A first-party scale diagram uses 1 centimetre on the " +
            "drawing to represent "+metersPerCm+" metres in reality. "+
            phrase+" Enter the exact numerical value in the unit requested. " +
            "Do not confuse the map scale with a conversion of centimetres into metres.";
        return new(Family,prompt,
            "Multiply drawing centimetres by actual metres per drawing cm; " +
            "divide actual metres by the scale to recover drawing cm. " +
            "Measuring the diagram and explaining the scale-bar approximation " +
            "requires an educator.",
            AssessmentItemType.ShortAnswer,p);
    }

    public static string Solve(string family,IReadOnlyDictionary<string,int> p)
    {
        if(!Valid(family,p))throw new InvalidOperationException("Invalid L07 parameters.");
        decimal cm=p["drawingTwice"]/2m;
        return Fmt(p["direction"]==0
            ? cm*ActualMetersPerDrawingCm[p["scale"]]:cm);
    }

    public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
    {
        if(!Valid(family,p)||string.IsNullOrWhiteSpace(answer)||answer.Length>32)
            return false;
        decimal target=p["drawingTwice"]/2m;
        if(p["direction"]==0)target*=ActualMetersPerDrawingCm[p["scale"]];
        decimal given;
        var split=answer.Trim().Split('/');
        if(split.Length==2)
        {
            if(!decimal.TryParse(split[0],NumberStyles.None,
                   CultureInfo.InvariantCulture,out var num) ||
               !decimal.TryParse(split[1],NumberStyles.None,
                   CultureInfo.InvariantCulture,out var den) || den<=0m)
                return false;
            given=num/den;
        }
        else if(split.Length==1)
        {
            if(!decimal.TryParse(split[0],NumberStyles.AllowDecimalPoint,
                   CultureInfo.InvariantCulture,out given))
                return false;
        }
        else return false;
        return given==target;
    }

    private static bool Valid(string? family,IReadOnlyDictionary<string,int>? p) =>
        Supports(family)&&p is not null&&p.Count==3&&
        p.TryGetValue("scale",out int s)&&s is >=0 and <=3 &&
        p.TryGetValue("drawingTwice",out int c)&&c is >=1 and <=40 &&
        p.TryGetValue("direction",out int dir)&&dir is 0 or 1;

    private static string Fmt(decimal d)=>d.ToString(CultureInfo.InvariantCulture);
}
