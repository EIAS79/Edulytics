using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Independent Grade2 Unit7 L1-L4 exact-number families. All publisher representations stay educator-reviewed.</summary>
internal static class UsCcssG2Unit7FirstFourQuestions
{
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 private static readonly IReadOnlyDictionary<string,int> Modes=new Dictionary<string,int>(StringComparer.Ordinal)
 {
 ["usccss.g2.u07.l01.near_difference"]=10,
 ["usccss.g2.u07.l01.count_tens"]=11,
 ["usccss.g2.u07.l01.count_hundreds"]=12,
 ["usccss.g2.u07.l01.count_back_tens"]=13,
 ["usccss.g2.u07.l02.add_hundreds"]=20,
 ["usccss.g2.u07.l02.add_tens"]=21,
 ["usccss.g2.u07.l02.subtract_hundreds"]=22,
 ["usccss.g2.u07.l02.subtract_tens"]=23,
 ["usccss.g2.u07.l03.subtract_by_place"]=30,
 ["usccss.g2.u07.l03.missing_to_hundred"]=31,
 ["usccss.g2.u07.l03.subtract_from_hundred"]=32,
 ["usccss.g2.u07.l03.unknown_addend"]=33,
 ["usccss.g2.u07.l04.count_on_to_hundred"]=40,
 ["usccss.g2.u07.l04.expanded_add"]=41,
 ["usccss.g2.u07.l04.expanded_subtract"]=42,
 ["usccss.g2.u07.l04.unknown_addend"]=43
 };
 public static bool Supports(string? family)=>family is not null&&Modes.ContainsKey(family);
 public static Problem Build(string family,Random random)
 {
  if(!Modes.TryGetValue(family,out var mode))throw new InvalidOperationException("Unknown Grade2 Unit7 family");
  int a=random.Next(120,600),b=random.Next(1,8),c=random.Next(1,5);
  var p=new Dictionary<string,int>(StringComparer.Ordinal){["lesson"]=mode/10,["mode"]=mode,["a"]=a,["b"]=b,["c"]=c};
  return new(family,Prompt(mode,a,b,c),$"Independent first-party {family} place-value reasoning and exact integer check.",
     AssessmentItemType.Numeric,p);
 }
 private static string Prompt(int mode,int a,int b,int c)
 {
  return mode switch{
   10=>"FIRST-PARTY — Count from "+a+" to "+(a+b)+". What is the difference?",
   11=>"FIRST-PARTY — Complete "+a+" + "+(b*10)+" = ? (tens jumps)",
   12=>"FIRST-PARTY — Complete "+(100+a%250)+" + "+(c*100)+" = ? (hundreds jumps)",
   13=>"FIRST-PARTY — Count back: "+(a+80)+" − "+(10*b)+" = ?",
   20=>"FIRST-PARTY — Base-ten hundreds: "+(100+a%200)+" + "+(100*c)+" = ?",
   21=>"FIRST-PARTY — Base-ten tens: "+a+" + "+(10*b)+" = ?",
   22=>"FIRST-PARTY — Base-ten hundreds: "+(a+300)+" − "+(100*c)+" = ?",
   23=>"FIRST-PARTY — Base-ten tens: "+(a+80)+" − "+(10*b)+" = ?",
   30=>"FIRST-PARTY — Open number line by place: "+(a-a%10+8)+" − "+(10*b+c)+" = ?",
   31=>"FIRST-PARTY — Hidden addend: "+(100+c*100-b*10)+" + ? = "+(100+c*100),
   32=>"FIRST-PARTY — Subtract from whole hundred: "+(500+c*100)+" − "+a+" = ?",
   33=>"FIRST-PARTY — Find the hidden addend: "+(a+b*10)+" + ? = "+(a+100),
   40=>"FIRST-PARTY — Count on to the next hundred: from "+a+", how many more to reach "+((a+99)/100*100)+"?",
   41=>"FIRST-PARTY — Add by expanded form: "+a+" + "+(10*b+c)+" = ?",
   42=>"FIRST-PARTY — Subtract by tens and ones: "+(a-a%10+8)+" − "+(10*b+c)+" = ?",
   43=>"FIRST-PARTY — What makes an equivalent addition? "+(a+b*10)+" + ? = "+(a+100),
   _=>throw new InvalidOperationException("Unknown unit7 mode")
  };
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> data)
 {
  if(!Valid(family,data))throw new InvalidOperationException("Invalid or tampered Unit7 first-party parameter");
  int a=data["a"],b=data["b"],c=data["c"],m=data["mode"];
  int v=m switch{
   10=>b,11=>a+10*b,12=>100+a%250+100*c,13=>a+80-10*b,
   20=>100+a%200+100*c,21=>a+10*b,22=>a+300-100*c,23=>a+80-10*b,
   30=>a-a%10+8-10*b-c,31=>10*b,32=>500+100*c-a,33=>100-10*b,
   40=>((a+99)/100*100)-a,41=>a+10*b+c,42=>a-a%10+8-10*b-c,43=>100-10*b,
   _=>throw new InvalidOperationException("Unknown mode")
  };
  return v.ToString(CultureInfo.InvariantCulture);
 }
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)=>
  f is not null&&Modes.TryGetValue(f,out var mode)&&p.Count==5&&
  p.TryGetValue("lesson",out var lesson)&&lesson==mode/10&&
  p.TryGetValue("mode",out var suppliedMode)&&suppliedMode==mode&&
  p.TryGetValue("a",out var a)&&a>=120&&a<600&&
  p.TryGetValue("b",out var b)&&b>=1&&b<8&&
  p.TryGetValue("c",out var c)&&c>=1&&c<5;
 public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string? answer)
 {
  if(!Valid(family,p))return false;
  return string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
 }
}
