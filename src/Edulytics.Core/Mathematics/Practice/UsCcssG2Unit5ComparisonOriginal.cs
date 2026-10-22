using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Authentic public source number checks U05 L07–L14; image, center and open responses are human.</summary>
public static class UsCcssG2Unit5ComparisonOriginal
{
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Explanation);
    private static readonly IReadOnlyDictionary<int,int[]> SourceKeys=new Dictionary<int,int[]>{
        [7]=[1,0,1,1],
        [8]=[3,30,300,700],
        [9]=[371,317,483,443],
        [10]=[76,76,99,99],
        [11]=[1,1,0,1],
        [12]=[30,37,36,36],
        [13]=[604,406,64,604],
        [14]=[529,529,529,529],
    };
    private static readonly IReadOnlyDictionary<int,int[]> FirstPartyKeys=new Dictionary<int,int[]>{
        [7]=[100,101,0,1],
        [8]=[234,450,678,901],
        [9]=[483,443,40,1],
        [10]=[142,274,330,300],
        [11]=[413,415,732,721],
        [12]=[638,681,672,644],
        [13]=[701,502,641,400],
        [14]=[320,452,671,805],
    };
    public static readonly IReadOnlyDictionary<int,IReadOnlyList<string>> SourceHumanRubrics=
        new Dictionary<int,IReadOnlyList<string>>{
        [7]=["ORIGINAL optional TrueFalse100>99 true,100<99+1 false,98+3>100 true,150>100 true","Teacher inspects genuine center Stage2 Mystery Number three digit cards and real clues, card10 removed","Partner plays Jump the Line, Mystery Number or Number Puzzles using original stage resources","Numeric companion must NOT claim observed dice, cards, peer clues or original assessment"],
        [8]=["Real original choral count by tens0..100 and hundreds0..1000","Three original number lines0..10,0..100,0..1000 have TEN equal gaps with scales1,10,100","Original pupil labels3,30,300 on source images; genuine620..630 source line has unit interval1","Actual figure positions700,472,940,356,590, learner dot placement and real partner talk teacher reviewed"],
        [9]=["Original intentionally unknown estimation dot on300..400 remains open-ended; no one fixed original answer","Published student Diego/Clare/Jada compare371>317 using blocks, place values and300..400 line","Actual points483 and443 on source400..500 line and explanatory comparison require teacher","Original printed estimate variation and image review cannot be reduced to a single numeric key"],
        [10]=["Warm-up original four sums36+40,46+30,59+40,69+30 result76,76,99,99","Original Mai/Tyler base-ten graphics and pairs261<313,239>165,224=224","Teacher observes that1h12t4o=2h2t4o=224, normalizing actual block quantities","Machine numeric calculations don't certify full publisher source diagrams or partner reasoning"],
        [11]=["Warm-up original comparisons true,true,false,true with expanded place-value expressions","Original individual521<523,889>878 plus UNIQUE source 529>519,687<752,495<501,810>793 assignment","Authentic Greatest of Them All Stage2 card game with irrevocable digit placements, real rounds","0-leading number invalid; original digits2,0,9→largest920, smallest209; other pupil permutations require teacher"],
        [12]=["Original subtraction warm-up30,37,36,36","Original Kiran erroneous ordering272 before269; Andre correct207,217,269,272,290","Source order real five dots839/765/788/815/719 in ASC and199/245/173/218/137 DESC","Another publisher set545,454,405,504,445 ASC, real plotted points and explanations teacher-observed"],
        [13]=["OPTIONAL How Many Do You See original diagrams604,406,64—different place values","Real Stage2 Get Your Numbers in Order source board and original digit cards0..9, draw3","Place each pupil-created 3-digit number in growing order or pass and score1, fewest points wins","Real teacher inspects true slot state, partner game decisions and original center choice"],
        [14]=["OPTIONAL original published cup: Jada,Noah,Clare each counted529 beans by different methods","DO NOT treat different actual student cups as all containing529","Real class physically counts collection one-half to one cup, groups by5/10/100, checks no double count","Original jar/cup photographs, actual sticky notes, MLR7 gallery and group totals teacher-reviewed"],
        };
    private static Review Check(int n,string? a,string? b,string? c,string? d,bool original)
    {
        var map=original?SourceKeys:FirstPartyKeys;
        if(!map.TryGetValue(n,out var expected))return new(false,original,"Invalid G2 Unit5 lesson");
        var values=new[]{a,b,c,d};
        bool correct=values.Select((x,i)=>int.TryParse(x,NumberStyles.None,
            CultureInfo.InvariantCulture,out var value)&&value>=0&&value<=1000&&value==expected[i]).All(x=>x);
        return new(correct,original,correct
         ?original?"Original numeric facts are correct; physical work, valid alternative reasoning and authentic images need teacher review."
                  :"Independently authored Edulytics numeric examples correct; real source images/games still teacher-only."
         :"Recheck the source quantities and exact place value calculation; no guessed original image mark is graded.");
    }
    public static Review Original(int n,string? a,string? b,string? c,string? d)=>Check(n,a,b,c,d,true);
    public static Review FirstParty(int n,string? a,string? b,string? c,string? d)=>Check(n,a,b,c,d,false);
}
