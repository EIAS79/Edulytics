using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Source-bound Unit4 lessons10-15; human classroom diagrams are not certified by numeric answers.</summary>
public static class UsCcssG2Unit4FinalSixSourcePractice
{
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
    public static readonly IReadOnlyDictionary<int,IReadOnlyList<string>> TeacherRubrics=
        new Dictionary<int,IReadOnlyList<string>>{
            [10]=["Original base-ten warm-up0→28→38→43 and equal-unit number-line hops","Clare's real base-ten diagram46−35=11 and independent58−24=34","Diego original33+45=78 (four tens + five ones), source23+24,50−32,40−26","Original publisher source figures, physical strategies and student discussion teacher reviewed"],
            [11]=["Original Number Talk32−6,43−8,51−5,52−7","Compare source Diego/Tyler authentic53−29=24 images","Demonstrate jump-to-ten for82−35=47 and66−28=38","True original source picture details and partner methods require teacher"],
            [12]=["Original making tens True/False 40=10+27+3 and47=20+7+3+10 and60=3+47+10","Use '?' on original number line for missing start, jump, or endpoint","True source mystery numbers32,53,38,49; teacher checks all original student arrow diagrams","Student-written mathematical riddle equations and physical number-line reasoning teacher-reviewed"],
            [13]=["Original Notice and Wonder: number line5→9 distance4 and corresponding tape model5+4=9","Source A-P card sort joins story, equation, tape diagram, number line using publisher ORIGINAL cards","Source Clare24→42 requires18 additional blocks; Andre37+39=76","Mai's publisher source train and real partner matches cannot be fabricated from incomplete graphic"],
            [14]=["OPTIONAL original True/False sums50,49,60 against50","REAL three targets chosen before game, both players begin source number line at origin","Spin all three authentic spinners and choose one move strategically, capture ONLY exact target","First to land on TWO actual target numbers wins; compare other real centers Scoot1 and How Close3"],
            [15]=["OPTIONAL birthday candle original photographs require genuine Notice and Wonder discussion","Original Kiran ages7→sister12→mom39→brother15→grandma68→uncle47→cousin14","Grandpa versus cousin50-year gap depends on full publisher context; cannot fabricate direction","Students create four genuine age entries and three personal riddles, no invented relatives/ages"],
        };
    private static readonly IReadOnlyDictionary<int,int[]> Original=new Dictionary<int,int[]>{
            [10]=[43,11,34,78],
            [11]=[26,35,46,45],
            [12]=[32,53,38,49],
            [13]=[9,18,76,4],
            [14]=[50,49,60,2],
            [15]=[12,39,15,68],
        };
    private static readonly IReadOnlyDictionary<int,int[]> Independent=new Dictionary<int,int[]>{
            [10]=[34,47,14,18],
            [11]=[24,47,38,53],
            [12]=[33,66,38,58],
            [13]=[21,62,24,49],
            [14]=[50,1,0,1],
            [15]=[15,36,23,40],
        };
    private static bool Integer(string? s,out int n)=>
        int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out n)&&n>=0&&n<=1000;
    private static Review Check(int lesson,string? a,string? b,string? c,string? d,bool source)
    {
        var bank=source?Original:Independent;
        if(!bank.TryGetValue(lesson,out var expected))return new(false,source,"Unknown original curricular lesson");
        var provided=new[]{a,b,c,d};
        bool correct=provided.Select((value,index)=>Integer(value,out var n)&&n==expected[index]).All(x=>x);
        return new(correct,source,correct
            ?source?"Original SOURCE numerical values match. Teacher must inspect original publisher diagrams, genuine student classroom work and discussion."
                   :"Original Edulytics first-party numerical responses correct; no claim about teacher-observed publisher outcomes."
            :source?"Check each original numerical question; physical and open-ended evidence still require a teacher."
                   :"Recalculate the four independently authored example values.");
    }
    public static Review Source(int lesson,string? a,string? b,string? c,string? d)=>Check(lesson,a,b,c,d,true);
    public static Review FirstParty(int lesson,string? a,string? b,string? c,string? d)=>Check(lesson,a,b,c,d,false);
}
