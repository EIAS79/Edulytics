using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party original Edulytics G2 Unit9 exact math; publisher original card draws, map measures, stories/posters remain human only.</summary>
internal static class UsCcssG2Unit9ExactQuestions
{
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static readonly string[][] Names=new string[][]{
new string[]{"mental_subtract_to_ten","double_and_near","mental_add_within20","missing_addend"},
new string[]{"unknown_to20","true_equivalent_sum","heads_up_missing_card","mentally_subtract"},
new string[]{"invented_map_segment_sum","invented_map_distance_difference","invented_cm_third_segment","invented_map_total"},
new string[]{"invented_line_plot_count","invented_line_plot_max","invented_line_plot_difference","invented_measurement_total"},
new string[]{"hundreds_tens_ones","exchange_hundred_for_tens","exchange_ten_for_ones","three_digit_number"},
new string[]{"equivalent_base_ten","unknown_tens_after_trade","unknown_ones_after_trade","expanded_value"},
new string[]{"three_digit_add","three_digit_subtract","add_with_regrouping","gap_to_next_hundred"},
new string[]{"mental_add_two_digit","mental_subtract_two_digit","multiple_tens","make_ten_and_add"},
new string[]{"story_two_step_apples","story_give_away","story_compare","story_unknown_change"},
new string[]{"invented_three_groups_total","invented_two_groups_gap","invented_unvisited_count","invented_comparison_question"},
new string[]{"tape_unknown_part","tape_unknown_whole","tape_difference","tape_equivalent_add"},
new string[]{"new_story_unknown_part","new_story_two_step","new_story_comparison","new_story_unknown_start"},
new string[]{"new_poster_story_sum","new_poster_story_difference","new_poster_two_step","new_poster_unknown_addend"}
 };
 private static readonly IReadOnlyDictionary<string,int> Modes=BuildModes();
 private static IReadOnlyDictionary<string,int> BuildModes()
 {
  var d=new Dictionary<string,int>(StringComparer.Ordinal);
  for(int lesson=1;lesson<=13;lesson++)
   for(int i=0;i<4;i++)d["usccss.g2.u09.l"+lesson.ToString("D2",CultureInfo.InvariantCulture)+"."+Names[lesson-1][i]]=lesson*10+i;
  return d;
 }
 public static bool Supports(string? family)=>family is not null&&Modes.ContainsKey(family);
 public static Problem Build(string family,Random random)
 {
  if(!Modes.TryGetValue(family,out var mode))throw new InvalidOperationException("Unapproved Grade2 Unit9 family");
  int a=random.Next(2,8),b=random.Next(2,8),c=random.Next(1,6);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"lesson",mode/10},{"mode",mode},{"a",a},{"b",b},{"c",c}};
  return new(family,Prompt(mode,a,b,c),
    "Independent first-party numeric math only; publisher measurement pictures, pupils' story questions and poster explanations are teacher reviewed.",AssessmentItemType.Numeric,p);
 }
 private static string Prompt(int m,int a,int b,int c)
 {
  int place=100*a+10*b+c, two=10*a+b, other=100+10*c+b;
  return m switch{
   10=>$"NEW mental math within20: 10−{a}=?",
   11=>$"NEW doubles: {a}+{a+1}=?",
   12=>$"NEW mental addition: {a}+{b}=?",
   13=>$"NEW missing addend: {a}+?=20",
   20=>$"NEW fluency flip: ?+{a}=20",
   21=>$"NEW equivalent make-ten: {a}+{b}+?={a+b+10}. What is ?",
   22=>$"NEW heads-up WITHOUT source card: which card value complements {a} to 20?",
   23=>$"NEW mental subtraction: {a+b}−{a}=?",
   30=>$"FIRST-PARTY INVENTED cm route, NOT publisher map: {a}cm + {b}cm = ?cm",
   31=>$"FIRST-PARTY INVENTED cm route: {a+b}cm compared with {a}cm: gap?",
   32=>$"FIRST-PARTY INVENTED third segment: first {a}cm, second {c}cm, third {b}cm. Third length?",
   33=>$"FIRST-PARTY INVENTED 3-segment route: {a}cm+{b}cm+{c}cm=?cm",
   40=>$"INVENTED line plot only: 5 observations {a},{a},{b},{b},{c}cm. How many observations?",
   41=>$"INVENTED line plot: {a},{a},{b},{b},{c}cm. Which of {a}cm and {b}cm is LONGER? Answer its length.",
   42=>$"INVENTED line plot: compare observations {a}cm and {b}cm; difference?",
   43=>$"INVENTED measurement lengths {a}cm,{b}cm,{c}cm. Total?",
   50=>$"FIRST-PARTY base ten: {a} hundreds,{b} tens,{c} ones. Number?",
   51=>$"FIRST-PARTY place exchange: {a} hundreds,{b} tens,{c} ones. How many tens if ONE hundred becomes ten tens?",
   52=>$"FIRST-PARTY place exchange: {a} hundreds,{b} tens,{c} ones. Ones after exchanging ONE ten?",
   53=>$"FIRST-PARTY place value: {100*a}+{10*b}+{c}=?",
   60=>$"FIRST-PARTY: {a} hundreds,{b} tens,{c} ones. What total?",
   61=>$"FIRST-PARTY equivalence: {a} hundreds,{b} tens,{c} ones. Number of TENS after trading one hundred?",
   62=>$"FIRST-PARTY equivalence: {a} hundreds,{b} tens,{c} ones. Number of ONES after trading one ten?",
   63=>$"FIRST-PARTY expanded notation for {place}: what value?",
   70=>$"NEW three-digit sum: {place}+{other}=?",
   71=>$"NEW three-digit subtraction: {place}−{10*b+c}=?",
   72=>$"NEW three-digit compose tens: {place}+{90-c}=?",
   73=>$"NEW distance to next whole hundred: {place} to {100*(a+1)}=?",
   80=>$"NEW two-digit addition: {two}+{10+c}=?",
   81=>$"NEW two-digit subtraction: {two}−{10+c}=?",
   82=>$"NEW two-digit fluency: {10*a}+{10*b}=?",
   83=>$"NEW near-ten sum: {two}+{10-b}=?",
   90=>$"INVENTED story, NOT publisher apples: First basket {two} apples, second {b} apples, {c} removed. Remaining?",
   91=>$"INVENTED story, NOT publisher: {two} cards, give away {b}. Remaining?",
   92=>$"INVENTED story: {two} books and {10*a} books. How many MORE in first pile?",
   93=>$"INVENTED story: original stock unknown. After adding {b} have {two}. Starting stock?",
   100=>$"INVENTED park question (not publisher 37,18,25): {two} playing,{b} walking,{c} sitting. TOTAL?",
   101=>$"INVENTED park: {two} playing versus {b} walking. Difference?",
   102=>$"INVENTED park: {two} present, {c} depart. How many remain?",
   103=>$"INVENTED park: {two} soccer and {b} tennis. Soccer minus tennis?",
   110=>$"INVENTED tape diagram: whole {two}, known part {b}. Missing part?",
   111=>$"INVENTED tape diagram: parts {two} and {b}. Whole?",
   112=>$"INVENTED compare tapes: longer {two}, shorter {b}. Difference?",
   113=>$"INVENTED parts {two}+?={two+b}. Missing part?",
   120=>$"INVENTED story, NOT the student's original: {two} stickers total, {b} are blue. Other colors?",
   121=>$"INVENTED 2-step story: {two} collected, {b} more, {c} used. Remaining?",
   122=>$"INVENTED story: {two} marbles versus {b}; how many more?",
   123=>$"INVENTED starting-unknown story: ?+{b}={two}. Starting amount?",
   130=>$"INVENTED poster companion: {two} tickets and {b} tickets. All tickets?",
   131=>$"INVENTED poster companion: {two} toys, {b} given away. Left?",
   132=>$"INVENTED two-step poster companion: {two} toys+{b} more−{c} used. Left?",
   133=>$"INVENTED poster companion: ?+{b}={two}. Missing initial amount?",
   _=>throw new InvalidOperationException("Unknown G2 Unit9 mode")
  };
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> data)
 {
  if(!Valid(family,data))throw new InvalidOperationException("Invalid or tampered G2 Unit9 numeric parameters");
  int m=data["mode"],a=data["a"],b=data["b"],c=data["c"],place=100*a+10*b+c,two=10*a+b,other=100+10*c+b;
  int ans=m switch{
   10=>10-a,11=>2*a+1,12=>a+b,13=>20-a,
   20=>20-a,21=>10,22=>20-a,23=>b,
   30=>a+b,31=>b,32=>b,33=>a+b+c,
   40=>5,41=>Math.Max(a,b),42=>Math.Abs(a-b),43=>a+b+c,
   50=>place,51=>b+10,52=>c+10,53=>place,
   60=>place,61=>b+10,62=>c+10,63=>place,
   70=>place+other,71=>100*a,72=>place+90-c,73=>100-b*10-c,
   80=>two+10+c,81=>two-10-c,82=>10*(a+b),83=>two+10-b,
   90=>two+b-c,91=>two-b,92=>b,93=>two-b,
   100=>two+b+c,101=>two-b,102=>two-c,103=>two-b,
   110=>two-b,111=>two+b,112=>two-b,113=>b,
   120=>two-b,121=>two+b-c,122=>two-b,123=>two-b,
   130=>two+b,131=>two-b,132=>two+b-c,133=>two-b,
   _=>throw new InvalidOperationException("Unknown Grade2 U09 firstparty mode")
  };
  return ans.ToString(CultureInfo.InvariantCulture);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
  f is not null&&Modes.TryGetValue(f,out var mode)&&p.Count==5&&
  p.TryGetValue("lesson",out var l)&&l==mode/10&&p.TryGetValue("mode",out var m)&&m==mode&&
  p.TryGetValue("a",out var a)&&a>=2&&a<=7&&
  p.TryGetValue("b",out var b)&&b>=2&&b<=7&&
  p.TryGetValue("c",out var c)&&c>=1&&c<=5;
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? answer)=>
  Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
}
