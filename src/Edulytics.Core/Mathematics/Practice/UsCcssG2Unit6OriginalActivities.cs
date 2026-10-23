using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Grade2 Unit6 independent numeric checks; source visuals and open creative tasks must be teacher-reviewed.</summary>
public static class UsCcssG2Unit6OriginalActivities
{
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyDictionary<int,string> AuthenticTasks=new Dictionary<int,string>{
        [1]="Sort original printed cards by side/corner count; partner describes chosen actual source card",
        [2]="Source pictures A,C,D are pentagons and B is open; complete actual pictured fragments of 4-,5-,6-sided polygons with alternate drawings",
        [3]="Measure actual cm/in side lengths with ruler: Diego two sides3cm; Tyler four equal2-inch sides; original source shapes human-checked",
        [4]="Describe original real assigned 3-D solid in class poster, peer guesses and object attributes",
        [5]="OPTIONAL mental source numbers40,85,90,535 and genuine geometry center choice/games teacher only",
        [6]="Use 2–4 equal-size pattern blocks to make THREE different composites; source grid tracing and real outer-shape identification teacher only",
        [7]="Fold, cut, and compare real rectangles into equal-area halves, thirds, fourths (not merely equal number of pieces)",
        [8]="Publisher Lin quarter figures A/B/C and actual shaded representations require original-image equality-of-area review",
        [9]="Pizza fraction original source image and actual shaded equal thirds/halves/fourths; Clare eats entire3/3 pizza",
        [10]="OPTIONAL authentic class centers; source 25,50,75,100 blocks with true material counts",
        [11]="SOURCE real analog clock figures, compare4:00 and half past7, draw original10:00 and1:30 hands; quarter past/to cards",
        [12]="SOURCE original clock card times and analog hand positions, chronological sort and 3:05 leading zero",
        [13]="SOURCE daily personal routine a.m./p.m. has plausible multiple answers; teacher checks genuine original pictures and hand drawings",
        [14]="OPTIONAL number-talk55,55,65,60 and real shape and addition centers",
        [15]="Publisher Andre/Clare/Han/Mai true coin pictures are image-only; penny1 nickel5 dime10; no manufactured source totals",
        [16]="Publisher actual Elena/Tyler coin images require visual check; student multiple collections97c and minimum66c with genuine coin representation",
        [17]="Source original coin sets A/B pictures, correct100 cents=one dollar, multiple genuine coin compositions to make100",
        [18]="Source coin images Lin/Diego unknown; true price list pencils75 sharpener35 eraser45 pens18; do not invent image-only budgets",
        [19]="SOURCE tape diagram visible 68 whole and39 known part→29 missing; mental warm-up50,60,62,73, student match original A/B/C images teacher",
        [20]="OPTIONAL money and shapes centers; original number talk55,65,71,72; actual pupil outcomes not automated",
        [21]="OPTIONAL true FOUR blocks at least ONE hexagon; partner makes two original silhouettes and calculates ORIGINAL image-only store prices",
    };
    private static readonly IReadOnlyDictionary<int,int[]> SourceKeys=new Dictionary<int,int[]>{
        [1]=[3,4,5,6],
        [2]=[4,5,6,1],
        [3]=[8,6,9,2],
        [4]=[6,8,12,5],
        [5]=[40,85,90,535],
        [6]=[2,3,4,4],
        [7]=[2,3,4,3],
        [8]=[10,25,40,25],
        [9]=[45,55,50,55],
        [10]=[25,50,75,100],
        [11]=[0,15,30,45],
        [12]=[5,30,40,55],
        [13]=[15,30,45,5],
        [14]=[55,55,65,60],
        [15]=[1,5,10,15],
        [16]=[25,50,75,5],
        [17]=[50,50,75,100],
        [18]=[75,35,45,18],
        [19]=[50,60,62,73],
        [20]=[55,65,71,72],
        [21]=[4,1,2,2],
    };
    private static readonly IReadOnlyDictionary<int,int[]> IndependentKeys=new Dictionary<int,int[]>{
        [1]=[3,4,5,6],
        [2]=[4,5,6,6],
        [3]=[12,10,15,6],
        [4]=[6,8,12,5],
        [5]=[65,40,4,325],
        [6]=[6,2,4,10],
        [7]=[2,3,4,9],
        [8]=[2,4,8,45],
        [9]=[3,2,4,1],
        [10]=[75,50,100,80],
        [11]=[15,30,45,25],
        [12]=[5,30,45,10],
        [13]=[15,30,45,20],
        [14]=[35,15,70,4],
        [15]=[4,10,20,34],
        [16]=[25,60,75,5],
        [17]=[4,10,20,25],
        [18]=[75,20,60,53],
        [19]=[29,75,70,15],
        [20]=[55,75,80,25],
        [21]=[4,1,19,6],
    };
    private static Review Check(int lesson,IReadOnlyDictionary<int,int[]> keys,string? a,string? b,string? c,string? d,bool original)
    {
        if(!keys.TryGetValue(lesson,out var expected))return new(false,original,"Unknown source code");
        var arr=new[]{a,b,c,d};
        bool ok=arr.Select((x,i)=>int.TryParse(x,NumberStyles.None,CultureInfo.InvariantCulture,out var number)
                &&number>=0&&number<=1000&&number==expected[i]).All(x=>x);
        return new(ok,original,ok
            ?original?"Source-specific NUMERIC facts match; actual publisher image, constructed shape, clock, coin, pizza and student explanations require teacher."
                     :"New first-party Edulytics numeric examples correct; no claim of real source card/photo/currency/game grading."
            :"Recalculate the four numeric questions; publisher's image-only details and open work still need teacher review.");
    }
    public static Review Source(int n,string? a,string? b,string? c,string? d)=>
        Check(n,SourceKeys,a,b,c,d,true);
    public static Review NewMath(int n,string? a,string? b,string? c,string? d)=>
        Check(n,IndependentKeys,a,b,c,d,false);
}
