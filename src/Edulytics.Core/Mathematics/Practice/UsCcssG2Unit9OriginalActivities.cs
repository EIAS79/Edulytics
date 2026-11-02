using System.Globalization;
namespace Edulytics.Core.Mathematics.Practice;
/// <summary>Publisher originals are human-judged; independently authored numeric Grade2 U09 companion is separate.</summary>
public static class UsCcssG2Unit9OriginalActivities
{
 public sealed record Review(bool? Correct,bool TeacherReviewNeeded,string Feedback);
 public static readonly IReadOnlyDictionary<int,string> AuthenticTasks=new Dictionary<int,string>{
[1]="Actual mental fluency 10−5,11−5,12−6,13−6. Students identify PERSONAL unknown sums within20 and handwrite chosen practice cards; the two original spinners determine unknown-addend totals at random.",
[2]="Original source Heads Up card on pupil forehead is unknown before draw; partner uses total20 to give a complement. Original number card deck0–19 and chosen mental strategy are educator observed.",
[3]="Original Measurement Map cities measured in cm with a ruler: student draws and measures map segments, adds/subtracts REAL measured distances. Do not assign a fictitious source city distance or discuss map scales.",
[4]="Original foot-length table A12,B18,C20,D18,E18,F20,G17,H21 cm and source line plot; groups then measure ACTUAL unsharpened pencils and draw their own plots. Measurements are not predetermined.",
[5]="Original source invites student to represent 308; real partners draw random tens/ones added to 2 hundreds then combine. Create group poster for 356 in multiple valid base-ten ways.",
[6]="Original true/false 5h+2t+7ones=527;4h+12t+7ones=527;5h+7ones+2t=527. Original cards sorted into sets of equivalent expressions and student explanation.",
[7]="Original expression options A–L including 102+341,385−124,302−198,685+279; student chooses which are MORE/LESS challenging and defends their own place-value method.",
[8]="Original Heads Up partner game using cards, player A/B/C changes roles; choose Five in a Row Stage6 or Target Numbers Stage3–5 original center to practice fluency.",
[9]="Sort ORIGINAL one- and two-step story cards, e.g. 92¢ less dime and quarter,15 apples+18 apples−7. Student may use own legitimate categorization and selected source story/diagram.",
[10]="Original questionless park has 37 soccer,18 tennis,25 picnic; STUDENT AUTHORS sensible question; second story Elena29,14 less than Han,Han15 more than Diego. Ask coherent questions first.",
[11]="Match original printed tape diagrams, equations and story cards. Source 53 and25 may represent different whole/part roles; pupils author an original situation matching a tape diagram.",
[12]="Original race photo count UNKNOWN, students give reasonable low/close/high estimates. Write original story for given equation and CREATE math-walk own story; source picture count not exact.",
[13]="Student must SOLVE OWN previous story, create actual poster with original story, question, drawing, explanation and answer; real gallery walk teacher observation."
 };
 public static readonly IReadOnlyDictionary<int,string[]> IndependentNumericExamples=new Dictionary<int,string[]>{
[1]=new string[]{"13 − 7 = ?","7 + 8 = ?","16 − 9 = ?","9 + 6 = ?"},
[2]=new string[]{"? + 7 = 20","11 − 4 = ?","9 + ? = 17","16 − 7 = ?"},
[3]=new string[]{"NEW map: 4cm + 5cm","NEW map: 11cm − 7cm","NEW map: 6cm + 2cm + 4cm","NEW map: 13cm − 6cm"},
[4]=new string[]{"NEW line plot lengths 9,9,12,12,11: count","NEW plot: longer of 9 and12 cm","NEW plot: gap 12−9 cm","NEW plot: total cm"},
[5]=new string[]{"Independent: 3 hundreds+5 tens+6 ones","Independent:3h+4t+16ones","Independent:2h+15t+6ones","Independent:3h+0t+8ones"},
[6]=new string[]{"Independent:5h+2t+7ones","Independent:4h+12t+7ones","Independent:3h+22t+7ones","Independent:5h+1t+17ones"},
[7]=new string[]{"Independent:102+341","Independent:385−124","Independent:302−198","Independent:685+279"},
[8]=new string[]{"Independent:29+35","Independent:83−47","Independent:9+5","Independent:64−28"},
[9]=new string[]{"Independent story:92−10−25","Independent story:15+18−7","Independent story:47+15+18","Independent:54−26"},
[10]=new string[]{"INVENTED park:12+14+8 total","INVENTED park:34−12","INVENTED scene:20−9","INVENTED scene:18+7−6"},
[11]=new string[]{"INVENTED tapes:36+24 whole","INVENTED tapes:60−24 part","INVENTED tapes:72−18 difference","INVENTED tapes:54+18 whole"},
[12]=new string[]{"INVENTED story:23+17","INVENTED story:40−17","INVENTED story:18+12−5","INVENTED story:?+14=33"},
[13]=new string[]{"INVENTED poster:32+7","INVENTED poster:32+17","INVENTED poster:32+27","INVENTED poster:32+57"}
 };
 private static readonly IReadOnlyDictionary<int,int[]> IndependentKeys=new Dictionary<int,int[]>{
[1]=new int[]{6,15,7,15},
[2]=new int[]{13,7,8,9},
[3]=new int[]{9,4,12,7},
[4]=new int[]{5,12,3,53},
[5]=new int[]{356,356,356,308},
[6]=new int[]{527,527,527,527},
[7]=new int[]{443,261,104,964},
[8]=new int[]{64,36,14,36},
[9]=new int[]{57,26,80,28},
[10]=new int[]{34,22,11,19},
[11]=new int[]{60,36,54,72},
[12]=new int[]{40,23,25,19},
[13]=new int[]{39,49,59,89}
 };
 public static Review Source(int n,string? a,string? b,string? c,string? d)=>
  AuthenticTasks.ContainsKey(n)
    ?new(null,true,"Original publisher physical activity, source picture or student-authored story/poster/measurement requires educator review. No fabricated original answers.")
    :new(false,true,"Unknown Grade2 Unit9 source lesson.");
 public static Review NewMath(int n,string? a,string? b,string? c,string? d)
 {
  if(!IndependentKeys.TryGetValue(n,out var expected))return new(false,false,"Unknown first-party math");
  var v=new[]{a,b,c,d};
  bool correct=v.Select((s,i)=>int.TryParse(s,NumberStyles.None,CultureInfo.InvariantCulture,out var value)&&
    value>=0&&value<=1000&&value==expected[i]).All(x=>x);
  return new(correct,false,correct
    ?"Independent Edulytics numerical companion only: original pupil tasks still require teacher.":"Check the independent mathematical answers; source open work still requires educator.");
 }
}
