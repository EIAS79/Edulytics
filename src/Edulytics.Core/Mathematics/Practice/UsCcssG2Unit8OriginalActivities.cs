using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Human-checked publisher U08 original activities; invented Edulytics numeric examples remain separate.</summary>
public static class UsCcssG2Unit8OriginalActivities
{
 public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
 public static readonly IReadOnlyDictionary<int,string> AuthenticTasks=new Dictionary<int,string>{
[1]="Share 4–15 actual counters in 2 equal partner groups, record total, both shares and 0/1 leftover on original T-chart. Open student work/real publisher toy picture teacher.",
[2]="Pair real red/yellow counters from source bags 4–15; determine 18,20 students paired exactly,19 one unpaired. Original source sock images and T-chart teacher.",
[3]="Original Color by Number publisher pictures: student colors to show even vs odd; teacher judges original arrangements and explanations.",
[4]="Original equal-addend and near-equal-addend decomposition: 8=4+4 vs 7=3+3+1; teacher reviews student-created equations.",
[5]="OPTIONAL 6,13,14 publisher dot images, parity pattern by stand/sit from zero and +1/+2 counters; original source images and class actions teacher.",
[6]="OPTIONAL Write Numbers Stage4 count by 2,5 or10, write 1–3 successive numbers per turn; original board and winner teacher.",
[7]="Source arrays with 6,7,9 counters: equal rows/columns, 7 cannot make 2+ rows AND columns without extras; original student physical arrays teacher.",
[8]="Count source array columns and rows and compare 3 rows×2 columns =2 columns×3; original visuals and drawings teacher.",
[9]="Source full waffle tray 5×4=20, compare equal repeated-addend sums by rows/columns; partly hidden original waffle graphic teacher.",
[10]="Build real arrays and author TWO equal-addend equations; 5 rows×3 columns gives 3+3+3+3+3=15 and5+5+5=15; drawings teacher.",
[11]="Tile true rectangles with equal square tiles, original D=4 rows×3 columns and actual source shapes; check equal sizes/gaps visually.",
[12]="Partition actual rectangle into equal-size square units with ruler/tiles, with and without guide ticks; student drawing teacher.",
[13]="OPTIONAL source true/false four2=4+4, three2=3+3, six2 NOT 5+5; chosen classroom centers and real board teacher."
 };
 public static readonly IReadOnlyDictionary<int,string[]> IndependentNumericExamples=new Dictionary<int,string[]>{
[1]=new string[]{"10 counters shared by TWO: each?","11 counters with two equal shares: leftover?","8 counters into PAIRS: number of pairs?","12 counters paired: leftovers?"},
[2]=new string[]{"18 students in pairs: pairs?","20 students in pairs: pairs?","19 students in pairs: leftovers?","14 objects into pairs: pairs?"},
[3]=new string[]{"12 objects into pairs: leftovers?","13 objects into pairs: leftovers?","12 objects in two equal groups: each?","15 objects paired: leftover?"},
[4]=new string[]{"Two equal groups of 4: total?","Two groups of 4 plus 1: total?","8 split into two equal groups: each?","9 shared in two equal groups: leftover?"},
[5]=new string[]{"Even 16 plus 1: leftovers after pairing?","Even 16 plus 2: leftovers?","Next even after 10?","Next odd after 11?"},
[6]=new string[]{"Count by 2 after 6: next?","Count by 5 after 10: next?","Count by 10 after 20: next?","Three jumps of 2 from 6: where?"},
[7]=new string[]{"True array 2 rows ×3: total?","Array 3×3: total?","7 counters: valid array with both dimensions >1? 1=yes/0=no","Array 3×4: total?"},
[8]=new string[]{"Array 3 rows ×4 columns: total?","Array 3 rows: how many rows?","Array 4 columns: how many columns?","Count 4 columns with 3 dots each: total?"},
[9]=new string[]{"Full 5×4 waffle tray: total?","3 rows of4: total?","3 rows of4: number of row addends?","3 rows of4: number of column addends?"},
[10]=new string[]{"5 rows of3 dots: total?","3 columns of5 dots: total?","5 rows: number of row addends?","3 columns: number of column addends?"},
[11]=new string[]{"4 rows of3 unit squares: number tiles?","4 rows by3 columns: number columns?","4 rows: how many rows?","4×3 tiles, remove one: remain?"},
[12]=new string[]{"3 rows ×4 columns of squares: total?","3 equal rows: number interior horizontal lines?","4 columns: number interior vertical lines?","3×4 tile rectangles: total squares?"},
[13]=new string[]{"Four 2s =4+4: total?","Three 2s =3+3: total?","Six 2s vs5+5: difference?","Next when skip counting2 after12?"}
 };
 private static readonly IReadOnlyDictionary<int,int[]> IndependentKeys=new Dictionary<int,int[]>{
[1]=new int[]{5,1,4,0},
[2]=new int[]{9,10,1,7},
[3]=new int[]{0,1,6,1},
[4]=new int[]{8,9,4,1},
[5]=new int[]{1,0,12,13},
[6]=new int[]{8,15,30,12},
[7]=new int[]{6,9,0,12},
[8]=new int[]{12,3,4,12},
[9]=new int[]{20,12,3,4},
[10]=new int[]{15,15,5,3},
[11]=new int[]{12,3,4,11},
[12]=new int[]{12,2,3,12},
[13]=new int[]{8,6,2,14}
 };
 public static Review Source(int n,string? a,string? b,string? c,string? d)=>
   AuthenticTasks.ContainsKey(n)
    ?new(null,true,"Original source drawings, physical pairs, array tiles, pupil explanations or optional game require educator; not numerically auto-certified.")
    :new(false,true,"Unknown original Grade2 Unit8 source lesson");
 public static Review NewMath(int n,string? a,string? b,string? c,string? d)
 {
  if(!IndependentKeys.TryGetValue(n,out var expected))return new(false,false,"Unknown first-party lesson");
  var values=new[]{a,b,c,d};
  bool ok=values.Select((x,i)=>int.TryParse(x,NumberStyles.None,CultureInfo.InvariantCulture,out var value)&&value>=0&&value<=1000&&value==expected[i]).All(x=>x);
  return new(ok,false,ok?"Independent Edulytics numeric answers correct: this never approves original published student work.":"First-party numerical values incorrect; original source tasks remain educator-reviewed.");
 }
}
