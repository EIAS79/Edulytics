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
 [4]="Warm-up 586−100,486−20,457−200,257−30. Activity 1 compare Mai counting up and Lin decomposing 500−387=113; solve 600−476 and 400−134. Activity 2 compare 427+351=778 as expanded-form and place-by-place calculation; solve 725+243,863−432,163+326,692−571 using choice of valid strategies. Original worked-method images, partner discussions and students' selected method explanations require teacher review; only numerical checks are automatically marked.",
 [5]="OPTIONAL original number grouping images: first base-ten diagram 2 hundreds 5 tens 4 ones = 254; public warmup figures include 474 and 676. Center Five in a Row Stage7 adds within 1000 WITHOUT composing; students move a paper clip on each turn, add chosen board numbers, cover the sum and get five in a row. Second activity lets students choose Get Your Numbers in Order, Mystery Number or The Greatest of Them All. Original pictured diagram, game board values, paperclip moves, five-in-a-row win, actual student partner strategy and optional center choices are not reconstructed or auto-graded.",
 [6]="Original warm-up 28+2=30,28+12=40,67+3=70,67+23=90; first activity sum table 245+15=260,247+23=270,249+31=280;134+26=160,133+37=170,138+42=180;351+19=370,356+24=380,355+35=390. Original Perfect10 Card Sort: match three-digit and two-digit cards so ones add to exactly ten. Publisher printed Perfect 10 card graphics, students' free card pairings, blocks and explanations require human review. Source protected Cool-down unavailable.",
 [7]="Source base-ten diagrams 3 hundreds 5 tens 3 ones,3 hundreds 4 tens 13 ones and 2 hundreds 15 tens 3 ones ALL represent 353. Activity1 364+28=392 composes a ten; 364+82=446 composes a hundred. Activity2 Walk About and Add requires actual A three-digit/B two-digit cards and partner predictions of composing ten or hundred; synthesis 428+42=470. Actual source diagrams, physical place value blocks, card partner pairing and student explanations are teacher-only; no invented official card values.",
 [8]="Original Activity1 three related sums 273+18=291 (compose one ten),273+81=354 (compose one hundred),273+88=361 (compose both a ten and a hundred); students compare how carrying differs, represent valid base-ten strategies and interpret peers' methods. Original publisher images, drawings, actual student work, pairwise explanations and unavailable protected Cool-down are educator-reviewed."
 };
 public static readonly IReadOnlyDictionary<int,string[]> SourceNumericExamples=new Dictionary<int,string[]>{
 [1]=new string[]{"81 − 79","203 − 198","680 − 673","501 − 499"},
 [2]=new string[]{"223 + 400","428 − 200","482 − 300","658 + 300"},
 [3]=new string[]{"375 − 24","189 − 73","647 − 46","? + 540 = 1000"},
 [4]=new string[]{"500 − 387","600 − 476","400 − 134","427 + 351"}
 };
 public static readonly IReadOnlyDictionary<int,string[]> IndependentNumericExamples=new Dictionary<int,string[]>{
 [1]=new string[]{"312 − 307","334 + 200","374 + 60","423 − 40"},
 [2]=new string[]{"300 + 200","462 + 30","648 − 100","568 − 40"},
 [3]=new string[]{"400 − 299","250 − 120","540 − 200","570 + ? = 640"},
 [4]=new string[]{"450 − 400","700 − 450","520 − 400","640 − 340"}
 };
 // Additional public source numerical check labels (actual publisher images and center choices remain human-only).
 public static readonly IReadOnlyDictionary<int,string[]> NextSourceNumericExamples=new Dictionary<int,string[]>{[5]=new string[]{"Original first diagram count","Original second diagram count","Original third diagram count","Five markers needed to win"},
 [6]=new string[]{"28 + 2","28 + 12","67 + 3","67 + 23"},
 [7]=new string[]{"Three base-ten diagrams total","364 + 28","364 + 82","428 + 42"},
 [8]=new string[]{"273 + 18","273 + 81","273 + 88","(273+88) − (273+81)"}};
 public static readonly IReadOnlyDictionary<int,string[]> NextIndependentNumericExamples=new Dictionary<int,string[]>{[5]=new string[]{"Independent no-carry sum 423+45","Independent place number 3 tens 5 ones","Independent no-carry 503+101","Independent place difference 512−500"},
 [6]=new string[]{"Independent make ten 124+6","Independent partner 273+7","Independent 512+8","Independent: how many ones complete 3 to 10?"},
 [7]=new string[]{"Independent one ten sum 357+8","Independent one hundred sum 370+80","Independent: 10 ones become how many tens?","Independent: 10 tens become how many hundreds?"},
 [8]=new string[]{"Independent 274+36","Independent 372+78","Independent 473+87","Independent (473+87)−(372+78)"}};
 private static readonly IReadOnlyDictionary<int,int[]> SourceKeys=new Dictionary<int,int[]>
 {
 [1]=new int[]{2,5,7,2},
 [2]=new int[]{623,228,182,958},
 [3]=new int[]{351,116,601,460},
 [4]=new int[]{113,124,266,778},
 [5]=new int[]{254,474,676,5},
 [6]=new int[]{30,40,70,90},
 [7]=new int[]{353,392,446,470},
 [8]=new int[]{291,354,361,7}
 };
 private static readonly IReadOnlyDictionary<int,int[]> IndependentKeys=new Dictionary<int,int[]>
 {
 [1]=new int[]{5,534,434,383},
 [2]=new int[]{500,492,548,528},
 [3]=new int[]{101,130,340,70},
 [4]=new int[]{50,250,120,300},
 [5]=new int[]{468,35,604,12},
 [6]=new int[]{130,280,520,7},
 [7]=new int[]{365,450,1,1},
 [8]=new int[]{310,450,560,110}
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
