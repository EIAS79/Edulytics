using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;

/// <summary>Mathematical numeric checks; authentic IM Grade2 U05 blocks and written names need teacher observation.</summary>
public static class UsCcssG2Unit5FirstSixOriginal
{
    public const string Prefix="PED:US-CCSS-MATH:G2:U05:L";
    public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
    private static readonly IReadOnlyDictionary<int,int[]> SourceKeys=new Dictionary<int,int[]>{
        [1]=[96,100,100,111],
        [2]=[9,11,15,30],
        [3]=[84,495,188,283],
        [4]=[672,253,509,180],
        [5]=[357,192,238,316],
        [6]=[248,562,433,951],
    };
    private static readonly IReadOnlyDictionary<int,int[]> NewKeys=new Dictionary<int,int[]>{
        [1]=[10,100,96,111],
        [2]=[30,7,300,29],
        [3]=[495,4,9,5],
        [4]=[507,609,482,30],
        [5]=[465,500,654,456],
        [6]=[248,562,433,951],
    };
    public static readonly IReadOnlyDictionary<int,IReadOnlyList<string>> TeacherRubrics=
       new Dictionary<int,IReadOnlyList<string>>{
        [1]=["Original Choral Count by ONES from90 to120, transition99 to100","Real Andre initial8 tens16 ones value96, then8 tens20 ones value100","Actually exchange10 tens→1 hundred, minimize block count","Teacher sees actual Priya100 ones/Kiran10 tens/Lin1 hundred source diagram matches","Original pair physically represents100+11=111 and discusses equivalent forms","Publisher image positions and restricted Cool-down not automatically graded"],
        [2]=["Original Choral Count by TENS0 through300, explain repeating patterns","Actual group4 manipulates≥50 tens rods, no hundreds blocks for Activity1","Source build90/110/150,300 with 1/2/3 hundreds blocks option","Teacher inspects genuine Jada4hundreds unfinished and Han15 tens unfinished diagrams for700","Physically complete Jada to7hundreds, Han to70tens; explain why 7h=70t","Publisher cool-down requires educator access; actual block work teacher-reviewed"],
        [3]=["Original Number Talk42+42,21+63,50+34,48+36 all84","Real group bucket2 hundreds28 tens15 ones=495 and canonical4h9t5o","Original MLR7 block sorting and peer gallery are human-evaluated","Original Mai1h7t18o=188; source Diego's image must be visually inspected (total283)","Find fewest units by exchanging10 ones for1 ten and10 tens for1 hundred","Protected original teacher-only Cool-down never inferred from numeric questions"],
        [4]=["Original three image warmup3h2t4o=324 and1h2t7o=127","Solve all SIX publisher riddles672,253,735,509,364,180 with zero-place discussion","Mixed-up equations462,267,530,325,372 and836 with reordered components","Original Clara37 versus Elena307 for7ones+3hundreds; justify307","Teacher observes real base-ten blocks, original diagrams and partner riddles","Do not claim typed numeric answers certify visual/source work"],
        [5]=["Original four True/False expanded-form statements: true,true,false,true","Source images 357,192,238,316,421, and standard expanded notation hundreds+tens+ones","Real THREE number cubes, physical pair rolls and smallest/largest ordering","Partner may find different valid arrangement from same dice, compare with actual student work","Standard expanded form writes values in descending places; a true sum can have different order","Restricted Cool-down teacher-only; no made-up original dice outcomes"],
        [6]=["Original Which One Doesn't Belong? images A325 words,B325 blocks,C253 numeral,D325 units","Accept multiple justified source figure selections; educator grades open reasons","Original number names248,562,433,627,951,318; hyphen words for tens+ones","Group poster numeral, diagram, expanded form and written words; real gallery walk","Student written words/spelling and true diagram equivalence require teacher review","Independent name-to-number exact tasks never certify student-composed word forms"],
       };
    private static Review Evaluate(int number,IReadOnlyDictionary<int,int[]> expected,string? a,string? b,string? c,string? d,bool teacher)
    {
        if(!expected.TryGetValue(number,out var keys))return new(false,teacher,"Unknown source lesson number");
        string?[] values=[a,b,c,d];
        bool correct=values.Select((str,i)=>
            int.TryParse(str,NumberStyles.None,CultureInfo.InvariantCulture,out var n)&&
            n>=0&&n<=999&&n==keys[i]).All(v=>v);
        return new(correct,teacher,correct
           ?teacher?"Original numeric relationships are correct, but actual publisher figures and hands-on tasks require educator checks."
                   :"Original Edulytics new numeric exercises mathematically correct. This does not grade original images, word writing or classmates."
           :"Revisit place values and distinguish original published quantities from independent numerical examples.");
    }
    public static Review Source(int n,string? a,string? b,string? c,string? d)=>
       Evaluate(n,SourceKeys,a,b,c,d,true);
    public static Review FirstParty(int n,string? a,string? b,string? c,string? d)=>
       Evaluate(n,NewKeys,a,b,c,d,false);
}
