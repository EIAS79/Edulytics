using System.Globalization;

namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Source-published coordinates; original plotted images and student point marks need educator eyes.</summary>
public static class UsCcssG2Unit4ThirdLessonUnlabeledTicks
{
    public const string LessonCode="PED:US-CCSS-MATH:G2:U04:L03";
    public sealed record Review(bool Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyList<string> TeacherRubric=[
        "Notice and Wonder all three authentic0..30 figures showing one, five and ten unit jumps",
        "Original activity1 target24 on0..30 by5 tick labels: one unit BEFORE25",
        "Original activity1 target37 on0..40 by10 tick labels: three units BEFORE40",
        "Original activity1 target48 on20..60 by10 tick labels: two units BEFORE50",
        "Original activity1 target83 on60..90 by5 tick labels: three units AFTER80",
        "Original activity2 place17,59,43,35 on each authentic publisher partial-label diagram; only17 and43 have textual image details",
        "A point must be aligned with an original image's actual equally spaced unit intervals; numeric answer alone cannot prove placement",
        "Number-line absolute coordinate still counts from0, even if drawing window starts at20 or60",
        "Original11 or31 printed/blank tick labels may be sparse but unseen positions are not fabricated",
        "MLR2 partner explanation, original real student diagram and inaccessible publisher Cool-down stay teacher only"
    ];
    private static bool N(string? value,int lower,int upper,out int result)=>
        int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out result)&&result>=lower&&result<=upper;
    public static Review SourceActivity1(string? n24,string? n37,string? n48,string? n83)
    {
        bool ok=N(n24,0,100,out var a)&&a==24&&N(n37,0,100,out var b)&&b==37&&
            N(n48,0,100,out var c)&&c==48&&N(n83,0,100,out var d)&&d==83;
        return new(ok,true,ok?"24,37,48,83 are true SOURCE values. Teacher checks physical marks and actual diagram images."
            :"Original numbers24 on0..30,37 on0..40,48 on20..60,83 on60..90.");
    }
    public static Review SourceLandmarkOffsets(string? before25,string? before40,string? before50,string? after80)
    {
        bool ok=N(before25,0,10,out var a)&&a==1&&N(before40,0,10,out var b)&&b==3&&
            N(before50,0,10,out var c)&&c==2&&N(after80,0,10,out var d)&&d==3;
        return new(ok,true,ok?"1,3,2,3 equal UNIT intervals from nearest printed anchor; original image positions require educator."
            :"Original targets are25−1=24,40−3=37,50−2=48,80+3=83.");
    }
    public static Review FirstPartyNumeric(string? fromZero83,string? range40To60,string? diff43Minus17)
    {
        bool ok=N(fromZero83,0,100,out var a)&&a==83&&
            N(range40To60,0,100,out var b)&&b==20&&
            N(diff43Minus17,0,100,out var c)&&c==26;
        return new(ok,false,ok?"Independent numeric values83,20,26 correct. Not publisher image or Cool-down."
            :"The distance from0 to83 is83 units; 60−40=20;43−17=26.");
    }
}
