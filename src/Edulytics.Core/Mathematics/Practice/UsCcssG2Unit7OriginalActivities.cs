using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Actual source numeric anchors are human-reviewed and distinct from invented first-party exercises.</summary>
public static class UsCcssG2Unit7OriginalActivities
{
 public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
 public static readonly IReadOnlyDictionary<int,string> AuthenticTasks=new Dictionary<int,string>
 {
 [1]="Warm-up 586−6, 586−8, 434−5, 352−4. Activity 1 contrasts Tyler base-ten subtraction and Elena counting on for 81−79; locate 203/198, 673/680, 501/499 and solve 400−396. Activity 2 extends number lines 234,334,434,?,?; 502,?,702,?,902; ?,?,332,342,352; 278,?,478,578,?; and 739,749,?,769,?. Real student-labelled number lines, paired comparison methods and explanations need educator review; the publisher Cool-down requires sign-in.",
 [2]="Warm-up 34+20,34+60,58+30,158+40. Activity 1 students roll number cubes: start 297 and add hundreds, 432 and add tens, 982 and subtract tens, 351 and add hundreds, 805 and subtract hundreds. Activity 2 with actual block stories: 223+400=623,428−200=228,836+60=896,572−20=552,658+300=958,482−300=182. Source random dice rolls, physical base-ten blocks, original drawings and partner representations need human inspection; no predetermined single source cube outcome.",
 [3]="Warm-up 120+20,120+200,124+30,124+300. Activity 1 compare Jada base-ten and Andre number-line solutions 375−24=351; try 189−73 and 647−46. Activity 2 paint-splatter unknowns include ?+540=1,000; 250+?=1,000; 600−440=?; 680+?=900; ?+590=1,000; 900−370=?. Publisher splatter artwork is not copied; student number-line drawings and discussions are human-reviewed; Cool-down login required.",
 [4]="Warm-up 586−100,486−20,457−200,257−30. Activity 1 compare Mai counting up and Lin decomposing 500−387=113; solve 600−476 and 400−134. Activity 2 compare 427+351=778 as expanded-form and place-by-place calculation; solve 725+243,863−432,163+326,692−571 using choice of valid strategies. Original worked-method images, partner discussions and students' selected method explanations require teacher review; only numerical checks are automatically marked."
 };
 private static readonly IReadOnlyDictionary<int,int[]> SourceKeys=new Dictionary<int,int[]>
 {
 [1]=new int[]{2,5,7,2},
 [2]=new int[]{623,228,182,958},
 [3]=new int[]{351,116,601,460},
 [4]=new int[]{113,124,266,778}
 };
 private static readonly IReadOnlyDictionary<int,int[]> IndependentKeys=new Dictionary<int,int[]>
 {
 [1]=new int[]{5,534,434,383},
 [2]=new int[]{500,492,548,528},
 [3]=new int[]{101,130,340,70},
 [4]=new int[]{50,250,120,300}
 };
 private static Review Check(int n,IReadOnlyDictionary<int,int[]> keys,string? x,string? y,string? z,string? w,bool original)
 {
  if(!keys.TryGetValue(n,out var expected))return new(false,original,"Unknown lesson");
  var raw=new[]{x,y,z,w};
  bool ok=raw.Select((value,i)=>int.TryParse(value,NumberStyles.None,CultureInfo.InvariantCulture,out var k)&&
    k>=0&&k<=1000&&k==expected[i]).All(a=>a);
  return new(ok,original,ok
   ?original?"Source numeric results only: original diagrams, number-line labels, partner choices, dice rolls and explanations need educator review.":"First-party independent answers correct. This is not certification of original source media."
   :"Recalculate the source-specific numbers. Open student work requires teacher review.");
 }
 public static Review Source(int n,string? a,string? b,string? c,string? d)=>Check(n,SourceKeys,a,b,c,d,true);
 public static Review NewMath(int n,string? a,string? b,string? c,string? d)=>Check(n,IndependentKeys,a,b,c,d,false);
}
