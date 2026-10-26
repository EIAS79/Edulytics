using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Exact numeric fraction comparison subskills of 2021 source Grade4 U02 L03.</summary>
internal static class UsCcssG4Unit2ThirdLessonQuestions
{
 public const string SameDen="usccss.g4.u02.l03.same_denominator";
 public const string SameNum="usccss.g4.u02.l03.same_numerator";
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is SameDen or SameNum;
 public static Problem Build(string f,Random random)
 {
  if(!Supports(f))throw new InvalidOperationException("Unknown fraction comparison source family");
  int a,b,x,y;
  if(f==SameDen){x=y=new[]{3,4,5,6,8,10}[random.Next(6)];a=random.Next(1,11);b=a+random.Next(1,5);}
  else{a=b=new[]{1,2,3,4,9,70}[random.Next(6)];x=new[]{3,6,8,10,20}[random.Next(5)];y=new[]{5,6,8,10,100}[random.Next(5)];if(x==y)y=x==100?20:100;}
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"n1",a},{"d1",x},{"n2",b},{"d2",y}};
  return new Problem(f,$"Which is greater: {a}/{x} or {b}/{y}? Enter1 for first,2 for second.","Cross-multiply exactly; explain same denominator or numerator.",AssessmentItemType.Numeric,p);
 }
 public static string Solve(string f,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(f,p))throw new InvalidOperationException("Invalid original fraction comparator parameters");
  long left=(long)p["n1"]*p["d2"],right=(long)p["n2"]*p["d1"];
  return (left>right?1:2).ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string f,IReadOnlyDictionary<string,int> p,string answer)=>Valid(f,p)&&string.Equals(Solve(f,p),answer,StringComparison.Ordinal);
 private static bool Valid(string? f,IReadOnlyDictionary<string,int> p)
 {
  if(!Supports(f)||p.Count!=4||!p.TryGetValue("n1",out var a)||!p.TryGetValue("n2",out var b)||!p.TryGetValue("d1",out var x)||!p.TryGetValue("d2",out var y))return false;
  if(a<1||b<1||x<1||y<1||a>70||b>70||x>100||y>100)return false;
  return f==SameDen?x==y&&a!=b:a==b&&x!=y;
 }
}
