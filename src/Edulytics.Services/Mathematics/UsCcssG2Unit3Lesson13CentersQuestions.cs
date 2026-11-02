using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>First-party numeric companion; real 8-round stage1 and cards remain teacher-supervised.</summary>
internal static class UsCcssG2Unit3Lesson13CentersQuestions
{
 public const string Cm="usccss.g2.u03.l13.target_measurement_abs_cm";
 public const string Inch="usccss.g2.u03.l13.target_measurement_abs_in";
 public const string Add="usccss.g2.u03.l13.number_puzzle_add_compose";
 public const string Sub="usccss.g2.u03.l13.number_puzzle_sub_decompose";
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Cm or Inch or Add or Sub;
 public static Problem Build(string family,Random r){
  if(!Supports(family))throw new InvalidOperationException("Unapproved center question");
  var p=new Dictionary<string,int>(StringComparer.Ordinal);string prompt,solution;
  if(family is Cm or Inch){int max=family==Cm?30:10,target=r.Next(1,max+1),actual=Math.Clamp(target+r.Next(-5,6),0,Math.Min(40,max+8));p["target"]=target;p["actual"]=actual;p["unit"]=family==Cm?1:2;string u=family==Cm?"cm":"in";prompt=$"FIRST-PARTY target measurement: target {target} {u}, drawn measured length {actual} {u}. What is the NONNEGATIVE gap in {u}? Real partner measurement is checked by teacher.";solution=$"|{actual}−{target}|={Math.Abs(actual-target)} {u}.";}
  else if(family==Add){int at=r.Next(1,6),bt=r.Next(1,8-at),ao=r.Next(4,10),bo=r.Next(10-ao,10),a=10*at+ao,b=10*bt+bo;p["a"]=a;p["b"]=b;prompt=$"FIRST-PARTY Number Puzzles Stage4 (not the printed card): {a}+{b}=?. Compose a new ten.";solution=$"{a}+{b}={a+b}, ones total{ao+bo} makes a new ten.";}
  else{int at=r.Next(3,10),bt=r.Next(1,at),ao=r.Next(0,9),bo=r.Next(ao+1,10),a=at*10+ao,b=bt*10+bo;p["a"]=a;p["b"]=b;prompt=$"FIRST-PARTY Number Puzzles Stage4: {a}−{b}=?. Decompose a ten, not official gameboard.";solution=$"{a}−{b}={a-b}.";}
  return new Problem(family,prompt,solution,AssessmentItemType.Numeric,p);
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p){if(!Valid(family,p))throw new InvalidOperationException("Bad target/puzzle params");return (family is Cm or Inch?Math.Abs(p["actual"]-p["target"]):family==Add?p["a"]+p["b"]:p["a"]-p["b"]).ToString(CultureInfo.InvariantCulture);}
 public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>Valid(family,p)&&string.Equals(answer,Solve(family,p),StringComparison.Ordinal);
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p){
  if(!Supports(f))return false;
  if(f is Cm or Inch)return p.Count==3&&p.TryGetValue("target",out var t)&&t>=1&&t<=(f==Cm?30:10)&&p.TryGetValue("actual",out var a)&&a>=0&&a<=(f==Cm?38:18)&&p.TryGetValue("unit",out var u)&&u==(f==Cm?1:2);
  if(p.Count!=2||!p.TryGetValue("a",out var x)||!p.TryGetValue("b",out var y)||x is <10 or >99||y is <10 or >99)return false;
  return f==Add?x+y<=100&&x%10+y%10>=10:x>y&&x/10>y/10&&x%10<y%10;
 }
}
