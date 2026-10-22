using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>IM2021 G2 Unit7 L05-L08 independent exact sums and composition checks; original partner games require educator.</summary>
internal static class UsCcssG2Unit7NextFourQuestions
{
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static readonly IReadOnlyDictionary<string,int> Modes=new Dictionary<string,int>(StringComparer.Ordinal){
["usccss.g2.u07.l05.no_carry_add"]=50,
["usccss.g2.u07.l05.count_place_units"]=51,
["usccss.g2.u07.l05.no_carry_sub"]=52,
["usccss.g2.u07.l05.number_order_difference"]=53,
["usccss.g2.u07.l06.perfect_ten_sum"]=60,
["usccss.g2.u07.l06.ones_to_ten"]=61,
["usccss.g2.u07.l06.add_to_next_ten"]=62,
["usccss.g2.u07.l06.second_ten_sum"]=63,
["usccss.g2.u07.l07.compose_ten_sum"]=70,
["usccss.g2.u07.l07.compose_hundred_sum"]=71,
["usccss.g2.u07.l07.ten_composed"]=72,
["usccss.g2.u07.l07.hundred_composed"]=73,
["usccss.g2.u07.l08.ten_only_sum"]=80,
["usccss.g2.u07.l08.hundred_only_sum"]=81,
["usccss.g2.u07.l08.two_units_sum"]=82,
["usccss.g2.u07.l08.compare_sum_change"]=83};
 public static bool Supports(string? family)=>family is not null&&Modes.ContainsKey(family);
 public static Problem Build(string family,Random random)
 {
  if(!Modes.TryGetValue(family,out var mode))throw new InvalidOperationException("Not an approved G2 U07 L05-L08 first-party family");
  int a=random.Next(1,7),b=random.Next(1,8),c=random.Next(1,8);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"lesson",mode/10},{"mode",mode},{"a",a},{"b",b},{"c",c}};
  return new(family,Prompt(mode,a,b,c),"Independently authored place-value example; original publisher activity is educator-reviewed.",AssessmentItemType.Numeric,p);
 }
 private static int Base(int a,int b,int c)=>100*a+10*b+c;
 private static string Prompt(int m,int a,int b,int c)
 {
  int x=Base(a,b,c),ten=10-c,makeHundred=10*(9-b);
  return m switch{
   50=>$"FIRST-PARTY — OPTIONAL no-carry center: {x}+{10*(8-b)+(9-c)}=?",
   51=>$"FIRST-PARTY — A base-ten drawing has {a} hundreds, {b} tens and {c} ones. What is the number?",
   52=>$"FIRST-PARTY — No-carry number subtraction: {x}−{10*b}=?",
   53=>$"FIRST-PARTY — Two consecutive numbers are {x} and {x+1}. What is the difference?",
   60=>$"FIRST-PARTY — Ones make a ten: {x}+{10*(8-b)+ten}=?",
   61=>$"FIRST-PARTY — Find the missing ones-digit partner that makes {c}+?=10.",
   62=>$"FIRST-PARTY — Complete a ten: {x}+{ten}=?",
   63=>$"FIRST-PARTY — {x}+{20+ten}=? Use tens and ones.",
   70=>$"FIRST-PARTY — Compose a TEN from ones: {300+10*b+c}+{ten}=?",
   71=>$"FIRST-PARTY — Compose a HUNDRED from tens: {300+10*(b+1)+c}+{makeHundred}=?",
   72=>$"FIRST-PARTY — {c} ones plus {ten} ones make how many new tens?",
   73=>$"FIRST-PARTY — {b+1} tens plus {9-b} tens make how many new hundreds?",
   80=>$"FIRST-PARTY — Only compose ten: {x}+{ten}=?",
   81=>$"FIRST-PARTY — Only compose hundred: {100*a+10*(b+1)+c}+{makeHundred}=?",
   82=>$"FIRST-PARTY — Compose BOTH a ten and hundred: {100*a+10*(b+1)+c}+{makeHundred+ten}=?",
   83=>$"FIRST-PARTY — How much greater is the both-unit sum than the hundred-only sum for {100*a+10*(b+1)+c}? (use identical addends except {ten} more)",
   _=>throw new InvalidOperationException("Unknown first-party mode")
  };
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p))throw new InvalidOperationException("Invalid first-party G2 U07 parameter data");
  int a=p["a"],b=p["b"],c=p["c"],m=p["mode"],x=Base(a,b,c),ten=10-c,hundred=10*(9-b);
  int v=m switch{
   50=>x+10*(8-b)+(9-c),51=>x,52=>x-10*b,53=>1,
   60=>x+10*(8-b)+ten,61=>ten,62=>x+ten,63=>x+20+ten,
   70=>300+10*b+c+ten,71=>300+10*(b+1)+c+hundred,72=>1,73=>1,
   80=>x+ten,81=>100*a+10*(b+1)+c+hundred,
   82=>100*a+10*(b+1)+c+hundred+ten,83=>ten,
   _=>throw new InvalidOperationException("Unknown G2 U07 mode")};
  return v.ToString(CultureInfo.InvariantCulture);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
  f is not null&&Modes.TryGetValue(f,out var m)&&p.Count==5&&
  p.TryGetValue("lesson",out var l)&&l==m/10&&p.TryGetValue("mode",out var supplied)&&supplied==m&&
  p.TryGetValue("a",out var a)&&a>=1&&a<=6&&p.TryGetValue("b",out var b)&&b>=1&&b<=7&&
  p.TryGetValue("c",out var c)&&c>=1&&c<=7;
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string? actual)=>
  Valid(f,p)&&string.Equals(Solve(f,p),actual,StringComparison.Ordinal);
}
