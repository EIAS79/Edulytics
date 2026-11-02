using System.Globalization;
using Edulytics.Core.Enums;
namespace Edulytics.Services.Mathematics;
/// <summary>Source-aligned numeric subskills ONLY for first-edition G4 U01 L02.
/// Physical paired rectangle posters, gallery discussion and free-form written
/// factor-set proofs remain teacher-reviewed. </summary>
internal static class UsCcssG4Unit1SecondLessonQuestions
{
 public const string Partner="usccss.g4.u01.l02.find_partner";
 public const string Factor="usccss.g4.u01.l02.is_side_factor";
 public const string Count="usccss.g4.u01.l02.count_unique_pairs";
 private static readonly int[] SourceAreas=[11,27,25,5,16,8,9,18,24,12,14,28,15,30,19,20];
 public sealed record Problem(string Family,string Prompt,string Solution,AssessmentItemType ItemType,IReadOnlyDictionary<string,int> Parameters);
 public static bool Supports(string? f)=>f is Partner or Factor or Count;
 public static Problem Build(string family,Random random)
 {
  if(!Supports(family))throw new InvalidOperationException("Unknown G4 U01 L02 source family.");
  var area=SourceAreas[random.Next(SourceAreas.Length)];
  var p=new Dictionary<string,int>(StringComparer.Ordinal){{"area",area}};
  string prompt;
  if(family==Partner)
  {
   var candidates=Enumerable.Range(1,area).Where(x=>area%x==0).ToArray();
   var width=candidates[random.Next(candidates.Length)];
   p["width"]=width;
   prompt=$"A rectangle has area {area} square units and one whole-number side is {width} units. What is its other side length (units)?";
  }
  else if(family==Factor)
  {
   var candidate=random.Next(1,Math.Min(31,area+2));
   p["candidate"]=candidate;
   prompt=$"A group draws whole-number rectangles with area {area} square units. Can a side length be {candidate} units? Enter 1 for yes, 0 for no.";
  }
  else
   prompt=$"How many DIFFERENT unordered whole-number factor pairs, ignoring rotations, make a rectangular area of {area} square units? Enter the number.";
  return new Problem(family,prompt,"Test exact whole-number divisors and count rotations only once.",AssessmentItemType.Numeric,p);
 }
 public static string Solve(string family,IReadOnlyDictionary<string,int> p)
 {
  if(!Valid(family,p))throw new InvalidOperationException("Invalid G4 U01 L02 parameters.");
  var a=p["area"];
  var result=family switch
  {
   Partner => a/p["width"],
   Factor => a%p["candidate"]==0 ? 1:0,
   Count => Enumerable.Range(1,(int)Math.Sqrt(a)).Count(i=>a%i==0),
   _ => throw new InvalidOperationException("Unknown source family.")
  };
  return result.ToString(CultureInfo.InvariantCulture);
 }
 public static bool Verify(string family,IReadOnlyDictionary<string,int> p,string answer)=>
  Valid(family,p) && string.Equals(Solve(family,p),answer,StringComparison.Ordinal);
 private static bool Valid(string? family,IReadOnlyDictionary<string,int> p)
 {
  if(!Supports(family)||!p.TryGetValue("area",out var a)||!SourceAreas.Contains(a))return false;
  return family switch
  {
   Partner=>p.Count==2&&p.TryGetValue("width",out var w)&&w>=1&&w<=a&&a%w==0,
   Factor=>p.Count==2&&p.TryGetValue("candidate",out var c)&&c>=1&&c<=Math.Min(30,a+1),
   Count=>p.Count==1,
   _=>false
  };
 }
}
