using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Unit7 L09-L18 first-party exact-number families; publisher figures and original student-created open responses are excluded.</summary>
internal static class UsCcssG2Unit7LastTenQuestions
{
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static readonly IReadOnlyDictionary<string,int> ModeByFamily=new Dictionary<string,int>(StringComparer.Ordinal)
 {
 ["usccss.g2.u07.l09.add_by_place"]=90,
 ["usccss.g2.u07.l09.compose_ten_hundred"]=91,
 ["usccss.g2.u07.l09.three_digits_sum"]=92,
 ["usccss.g2.u07.l09.recover_missing_ten"]=93,
 ["usccss.g2.u07.l10.next_hundred"]=100,
 ["usccss.g2.u07.l10.missing_to_thousand"]=101,
 ["usccss.g2.u07.l10.three_digit_sum"]=102,
 ["usccss.g2.u07.l10.near_hundred_bridge"]=103,
 ["usccss.g2.u07.l11.make_hundred_cents"]=110,
 ["usccss.g2.u07.l11.four_addends"]=111,
 ["usccss.g2.u07.l11.goal_thousand_gap"]=112,
 ["usccss.g2.u07.l11.center_sum"]=113,
 ["usccss.g2.u07.l12.decompose_ten"]=120,
 ["usccss.g2.u07.l12.take_tens"]=121,
 ["usccss.g2.u07.l12.partner_blocks"]=122,
 ["usccss.g2.u07.l12.related_difference"]=123,
 ["usccss.g2.u07.l13.decompose_hundred"]=130,
 ["usccss.g2.u07.l13.decompose_ten"]=131,
 ["usccss.g2.u07.l13.compare_strategies"]=132,
 ["usccss.g2.u07.l13.difference"]=133,
 ["usccss.g2.u07.l14.zero_exchange"]=140,
 ["usccss.g2.u07.l14.one_exchange"]=141,
 ["usccss.g2.u07.l14.two_exchanges"]=142,
 ["usccss.g2.u07.l14.sorted_difference"]=143,
 ["usccss.g2.u07.l15.decompose_both"]=150,
 ["usccss.g2.u07.l15.count_back_tens"]=151,
 ["usccss.g2.u07.l15.unknown_addend"]=152,
 ["usccss.g2.u07.l15.subtract_difference"]=153,
 ["usccss.g2.u07.l16.close_hundred_count_on"]=160,
 ["usccss.g2.u07.l16.subtract_by_place"]=161,
 ["usccss.g2.u07.l16.borrow_both"]=162,
 ["usccss.g2.u07.l16.near_hundred_gap"]=163,
 ["usccss.g2.u07.l17.target_addition"]=170,
 ["usccss.g2.u07.l17.target_subtraction"]=171,
 ["usccss.g2.u07.l17.add_units"]=172,
 ["usccss.g2.u07.l17.subtract_units"]=173,
 ["usccss.g2.u07.l18.new_chart_difference"]=180,
 ["usccss.g2.u07.l18.new_chart_sum"]=181,
 ["usccss.g2.u07.l18.new_chart_leftover"]=182,
 ["usccss.g2.u07.l18.new_chart_known_bars"]=183
 };
 public static bool Supports(string? family)=>family is not null&&ModeByFamily.ContainsKey(family);
 public static Problem Build(string family,Random random)
 {
  if(!ModeByFamily.TryGetValue(family,out var mode))throw new InvalidOperationException("Unknown G2 Unit7 L09-L18 original-source-aligned first-party family");
  int a=random.Next(1,5),b=random.Next(1,5),c=random.Next(1,6);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"lesson",mode/10},{"mode",mode},{"a",a},{"b",b},{"c",c}};
  return new(family,Prompt(mode,a,b,c),"Independently authored exact arithmetic; do not infer source diagrams, printed cards or pupil explanation.",AssessmentItemType.Numeric,p);
 }
 private static int Hundreds(int a,int b,int c)=>100*a+10*b+c;
 private static int ChartA(int a,int b)=>100*a+10*b;
 private static int ChartB(int a,int b,int c)=>ChartA(a,b)+c;
 private static string Prompt(int m,int a,int b,int c)
 {
  int x=Hundreds(a,b,c), y=Hundreds(1,c,b);
  return m switch{
   90=>$"FIRST-PARTY — Add by place {x}+{y}=? Explain hundreds, tens, ones.",
   91=>$"FIRST-PARTY — Compose a ten and hundred: {100*a+90+c}+{130-c}=?",
   92=>$"FIRST-PARTY — Add two three-digit numbers: {x}+{200+10*c+b}=?",
   93=>$"FIRST-PARTY — Correctly add the new ten: {x}+{10-c}=?",
   100=>$"FIRST-PARTY — {100*a+99} needs how many to reach {100*(a+1)}?",
   101=>$"FIRST-PARTY — Which missing addend makes {300+100*a+10*b+c}+?=1000?",
   102=>$"FIRST-PARTY — Add {x}+{200+10*c+b} using your choice of method.",
   103=>$"FIRST-PARTY — Use a friendly hundred: {199+10*a+c}+{10*b+c}=?",
   110=>$"FIRST-PARTY — Cents: 80+20+{10*b+c}=?",
   111=>$"FIRST-PARTY — Cents: 75+25+{10*b+c}+{10*a}=?",
   112=>$"FIRST-PARTY — How far is {x} from the target 1000?",
   113=>$"FIRST-PARTY — Center addition with invented values {x}+100=?",
   120=>$"FIRST-PARTY — Subtract after decomposing a ten: 354−{10*b+8}=?",
   121=>$"FIRST-PARTY — Source-compatible new subtraction with a ten trade: 264−{10*b+8}=?",
   122=>$"FIRST-PARTY — Subtract from 343 with a ten trade: 343−{100+10*b+7}=?",
   123=>$"FIRST-PARTY — Compute 451−{300+10*b+5}=? Check by addition.",
   130=>$"FIRST-PARTY — Exchange a hundred for tens: 336−{40+c}=?",
   131=>$"FIRST-PARTY — Exchange a ten for ones: 245−{26+b%4}=?",
   132=>$"FIRST-PARTY — Find 244−64 using a hundred-to-tens decomposition.",
   133=>$"FIRST-PARTY — Find 352−71 using an equivalent place-value equation.",
   140=>$"FIRST-PARTY — How many place units must be decomposed to subtract 457−45 by place?",
   141=>$"FIRST-PARTY — How many units must be decomposed for 449−88 by place?",
   142=>$"FIRST-PARTY — How many units must be decomposed for 341−68 by place?",
   143=>$"FIRST-PARTY — Difference of 244−67?",
   150=>$"FIRST-PARTY — Decompose a ten and a hundred: 726−558=?",
   151=>$"FIRST-PARTY — Decompose both units: 602−487=?",
   152=>$"FIRST-PARTY — Count on from 298 to 402. What is the difference?",
   153=>$"FIRST-PARTY — Difference 700−589=?",
   160=>$"FIRST-PARTY — Count on 298→300→400→402; find 402−298.",
   161=>$"FIRST-PARTY — Subtract by place: 535−214=?",
   162=>$"FIRST-PARTY — Correctly borrow both units: 735−457=?",
   163=>$"FIRST-PARTY — Count on from 599 to 701. How many?",
   170=>$"FIRST-PARTY — Target game with fictional number {x}: amount to ADD to get 1000?",
   171=>$"FIRST-PARTY — From fictional value {x}, SUBTRACT {100*a}. What remains?",
   172=>$"FIRST-PARTY — Center place-value addition {x}+{100+10*b}=?",
   173=>$"FIRST-PARTY — Center place-value subtraction {x}−{10*b+c}=?",
   180=>$"FIRST-PARTY INVENTED BAR CHART (not publisher): A={ChartA(a,b)}, B={ChartB(a,b,c)}. How many more in B than A?",
   181=>$"FIRST-PARTY INVENTED BAR CHART (not publisher): A={ChartA(a,b)}, B={ChartB(a,b,c)}. What total in the TWO visible categories?",
   182=>$"FIRST-PARTY INVENTED BAR CHART (not publisher): A={ChartA(a,b)}, B={ChartB(a,b,c)}, and C is hidden. What is B−A? (Do NOT guess C.)",
   183=>$"FIRST-PARTY INVENTED BAR CHART: A={ChartA(a,b)}, B={ChartB(a,b,c)}, C is hidden. How many categories have KNOWN values?",
   _=>throw new InvalidOperationException("Unknown Unit7 first-party mode")
  };
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(family,p))throw new InvalidOperationException("Invalid Unit7 exact source-independent question parameters");
  int a=p["a"],b=p["b"],c=p["c"],m=p["mode"],x=Hundreds(a,b,c);
  int answer=m switch{
   90=>x+Hundreds(1,c,b),91=>100*a+220,92=>x+200+10*c+b,93=>x+10-c,
   100=>1,101=>1000-(300+100*a+10*b+c),102=>x+200+10*c+b,103=>199+10*a+c+10*b+c,
   110=>100+10*b+c,111=>100+10*b+c+10*a,112=>1000-x,113=>x+100,
   120=>354-10*b-8,121=>264-10*b-8,122=>343-100-10*b-7,123=>451-300-10*b-5,
   130=>336-40-c,131=>245-26-b%4,132=>180,133=>281,
   140=>0,141=>1,142=>2,143=>177,
   150=>168,151=>115,152=>104,153=>111,
   160=>104,161=>321,162=>278,163=>102,
   170=>1000-x,171=>10*b+c,172=>x+100+10*b,173=>100*a,
   180=>c,181=>ChartA(a,b)+ChartB(a,b,c),182=>c,183=>2,
   _=>throw new InvalidOperationException("Invalid unit7 firstparty mode")};
  return answer.ToString(CultureInfo.InvariantCulture);
 }
 private static bool Valid(string? family,IReadOnlyDictionary<string,int> data)=>
  family is not null&&ModeByFamily.TryGetValue(family,out var m)&&data.Count==5&&
  data.TryGetValue("lesson",out var lesson)&&lesson==m/10&&
  data.TryGetValue("mode",out var mode)&&mode==m&&
  data.TryGetValue("a",out var a)&&a>=1&&a<=4&&
  data.TryGetValue("b",out var b)&&b>=1&&b<=4&&
  data.TryGetValue("c",out var c)&&c>=1&&c<=5;
 public static bool Verify(string family,IReadOnlyDictionary<string,int> data,string? actual)=>
  Valid(family,data)&&string.Equals(actual,Solve(family,data),StringComparison.Ordinal);
}
